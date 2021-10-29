﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTServer
{
    internal class CommandQueue
    {
        public CommandQueue(ILogger Logger)
        {
            this.Logger = Logger.IsNotNull();
        }

        internal record QueueItem(ICommandHandler CommandHandler, IConnection Connection, MessageBase Command, CancellationTokenSource cts);

        /// <summary>
        /// Queue an item to be processed in RunAsync.
        /// </summary>
        public async Task EnqueueCommandAsync(ICommandHandler CommandHandler, IConnection Connection, MessageBase Command, CancellationTokenSource cts)
        {
            try
            {
                await syncObject.WaitAsync();

                Contents.Add(new(CommandHandler, Connection, Command, cts));
                // Signal RunAsync if we are waiting for a command.
                NewItemEvent.Set();
            }
            finally
            {
                syncObject.Release();
            }
        }

        /// <summary>
        /// Check for at least one valid RequestId for Cancel request.
        /// </summary>
        /// <param name="Connection">Connection requesting the cancel operation.</param>
        /// <param name="RequestIds">RequestIDs to check.</param>
        /// <returns>True if any specified ids are valid or RequestIds is null.</returns>
        public async Task<bool> AnyValidRequestID(IConnection Connection, List<int> RequestIds)
        {
            //Any commands
            if (RequestIds is null)
                return true;

            try
            {
                await syncObject.WaitAsync();

                //Check current command
                if (CurrentCommand != null &&CurrentCommand.Connection == Connection && RequestIds.Contains(CurrentCommand.Command.Header.RequestId.Value))
                    return true;

                //Check queued commands
                return Contents.Any(c => c.Connection == Connection && RequestIds.Contains(c.Command.Header.RequestId.Value));
            }
            finally
            {
                syncObject.Release();
            }
        }

        /// <summary>
        /// Attempt to cancel active or queued items
        /// </summary>
        /// <param name="Connection">Connection requesting the cancel operation.</param>
        /// <param name="RequestIds">RequestIDs to cancel.</param>
        /// <returns>True if any specified ids are cancelled or RequestIds is null.</returns>
        public async Task TryCancelItemsAsync(IConnection Connection, List<int> RequestIds)
        {
            if(RequestIds is null || RequestIds.Count == 0)
            {
                await TryCancelAllAsync(Connection);
            }
            else
            {
                await TryCancelSpecificAsync(Connection, RequestIds);
            }
        }

        /// <summary>
        /// Attempt to cancel all commands for the connection.
        /// </summary>
        /// <param name="Connection">The connection to cancel commands for.</param>
        private async Task TryCancelAllAsync(IConnection Connection)
        {
            try
            {
                await syncObject.WaitAsync();
                
                //Request Cancel for running command
                if (CurrentCommand != null && CurrentCommand.Connection == Connection)
                {
                    CurrentCommand.cts.Cancel();
                    CurrentCommandCancelRequested = true;
                }
                foreach (var cmd in Contents.Where(c => c.Connection == Connection).ToList())
                {
                    //Complete command with Cancelled response
                    await cmd.CommandHandler.HandleError(cmd.Connection, cmd.Command, new TimeoutCanceledException(true));
                    cmd.cts.Dispose();
                    Contents.Remove(cmd);
                }
            }
            finally
            {
                syncObject.Release();
            }
        }

        /// <summary>
        /// Attempt to cancel specified commands for the connection.
        /// </summary>
        /// <param name="Connection">The connection to cancel commands for.</param>
        /// <param name="RequestIds">RequestIDs to cancel.</param>
        /// <returns>True if any specified ids are cancelled.</returns>
        private async Task TryCancelSpecificAsync(IConnection Connection, List<int> RequestIds)
        {
            try
            {
                await syncObject.WaitAsync();

                //Cancel specified commands
                foreach (int id in RequestIds)
                {
                    if (CurrentCommand is not null && CurrentCommand.Command.Header.RequestId == id && CurrentCommand.Connection == Connection)
                    {
                        //Request Cancel for running command
                        CurrentCommand.cts.Cancel();
                        CurrentCommandCancelRequested = true;
                    }
                    else
                    {
                        var foundID = Contents.Find(c => c.Command.Header.RequestId == id && c.Connection == Connection);
                        if (foundID is not null)
                        {
                            //Complete command with Cancelled response
                            await foundID.CommandHandler.HandleError(foundID.Connection, foundID.Command, new TimeoutCanceledException(true));
                            foundID.cts.Dispose();
                            Contents.Remove(foundID);
                        }
                    }
                }
            }
            finally
            {
                syncObject.Release();
            }
        }

        /// <summary>
        /// Retrieve the next item in the queue.
        /// </summary>
        private async Task<QueueItem> ReceiveItemAsync()
        {
            try
            {
                await syncObject.WaitAsync();
                NewItemEvent.Reset();
                if (Contents.Count > 0)
                {
                    CurrentCommand.IsNull($"Expected {nameof(CurrentCommand)} to be null.");
                    CurrentCommand = Contents[0];
                    Contents.RemoveAt(0);
                    return CurrentCommand.IsNotNull($"{nameof(CurrentCommand)} was null."); ;
                }
            }
            finally
            {
                syncObject.Release();
            }

            await Task.Run(() =>
            {
                NewItemEvent.WaitOne();
                try
                {
                    syncObject.Wait();
                    Contracts.IsTrue(Contents.Count > 0, "No item found after NewItemEvent signaled.");
                    CurrentCommand.IsNull($"Expected {nameof(CurrentCommand)} to be null.");
                    CurrentCommand = Contents[0];
                    Contents.RemoveAt(0);
                }
                finally
                {
                    syncObject.Release();
                }
            });
            return CurrentCommand.IsNotNull($"{nameof(CurrentCommand)} was null.");
        }

        /// <summary>
        /// Run the command queue
        /// </summary>
        public async Task RunAsync()
        {
            // Execute queued commands
            while (true)
            {
                //Wait for item to be received.
                var (handler, connection, command, cts) = await ReceiveItemAsync();

                try
                {
                    Logger.Log("Dispatcher", $"Running {command.Header.Name} id:{command.Header.RequestId}");

                    //Throw OperationCanceledException if command timeout has already been reached.
                    cts.Token.ThrowIfCancellationRequested();
                    await handler.Handle(connection, command, cts.Token);
                    
                    Logger.Log("Dispatcher", $"Completed {command.Header.Name} id:{command.Header.RequestId}");
                }
                catch (Exception ex)
                {
                    Logger.Log("Dispatcher", $"Caught exception running {command.Header.Name} id:{command.Header.RequestId}.{Environment.NewLine}{ex}");

                    if(ex is TaskCanceledException or OperationCanceledException)
                    {
                        //Check if cancel was requested.
                        bool cancelRequested = false;
                        try
                        {
                            await syncObject.WaitAsync();
                            cancelRequested = CurrentCommandCancelRequested;
                        }
                        finally
                        {
                            syncObject.Release();
                        }
                        ex = new TimeoutCanceledException(ex.Message, ex, cancelRequested);
                    }

                    await handler.HandleError(connection, command, ex);
                }

                //Command is complete - unassign CurrentCommand.
                try
                {
                    await syncObject.WaitAsync();
                    CurrentCommand = null;
                    CurrentCommandCancelRequested = false;
                }
                finally
                {
                    syncObject.Release();
                }

                cts.Dispose();
            }
        }

        private readonly SemaphoreSlim syncObject = new(1, 1);
        private readonly List<QueueItem> Contents = new();
        private readonly AutoResetEvent NewItemEvent = new(false);
        private QueueItem CurrentCommand = null;
        private bool CurrentCommandCancelRequested = false;
        private readonly ILogger Logger;
    }
}
