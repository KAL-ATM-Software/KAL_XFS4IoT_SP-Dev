/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
            using var sync = await DisposableLock.Create(syncObject, cts.Token);

            Contents.Add(new(CommandHandler, Connection, Command, cts));
            // Signal RunAsync if we are waiting for a command.
            NewItemEvent.Set();
        }

        /// <summary>
        /// Check for at least one valid RequestId for Cancel request.
        /// </summary>
        /// <param name="Connection">Connection requesting the cancel operation.</param>
        /// <param name="RequestIds">RequestIDs to check.</param>
        /// <param name="token">CancellationToken to use while waiting for our SyncObject.</param>
        /// <returns>True if any specified ids are valid or RequestIds is null.</returns>
        public async Task<bool> AnyValidRequestID(IConnection Connection, List<int> RequestIds, CancellationToken token)
        {
            //Any commands
            if (RequestIds is null)
                return true;

            using var sync = await DisposableLock.Create(syncObject, token);

            //Check current command
            if (CurrentCommand != null && CurrentCommand.Connection == Connection && RequestIds.Contains(CurrentCommand.Command.Header.RequestId.Value))
                return true;

            //Check queued commands
            return Contents.Any(c => c.Connection == Connection && RequestIds.Contains(c.Command.Header.RequestId.Value));
        }

        /// <summary>
        /// Attempt to cancel active or queued items
        /// </summary>
        /// <param name="Connection">Connection requesting the cancel operation.</param>
        /// <param name="RequestIds">RequestIDs to cancel.</param>
        /// <param name="token">CancellationToken to use while waiting for our SyncObject.</param>
        /// <returns>True if any specified ids are cancelled or RequestIds is null.</returns>
        public async Task TryCancelItemsAsync(IConnection Connection, List<int> RequestIds, CancellationToken token)
        {
            try
            {
                if (RequestIds is null || RequestIds.Count == 0)
                {
                    await TryCancelAllAsync(Connection, token);
                }
                else
                {
                    await TryCancelSpecificAsync(Connection, RequestIds, token);
                }
            }
            catch(Exception ex)
            {
                Logger.Warning("CommandQueue", "Caught exception inside TryCancelItemsAsync. \n" + ex);
                throw;
            }
        }

        /// <summary>
        /// Attempt to cancel all commands for the connection.
        /// </summary>
        /// <param name="Connection">The connection to cancel commands for.</param>
        /// <param name="token">CancellationToken to use while waiting for our SyncObject.</param>
        private async Task TryCancelAllAsync(IConnection Connection, CancellationToken token)
        {
            using var sync = await DisposableLock.Create(syncObject, token);

            //Request Cancel for running command
            if (CurrentCommand != null && CurrentCommand.Connection == Connection)
            {
                CurrentCommand.cts.Cancel();
                CurrentCommandCancelRequested = true;
            }
            foreach (var cmd in Contents.Where(c => c.Connection == Connection).ToList())
            {
                //Complete command with Cancelled response
                await cmd.CommandHandler.HandleError(cmd.Command, new TimeoutCanceledException(true));
                cmd.cts.Dispose();
                Contents.Remove(cmd);
                NewItemEvent.RemovedItem();
            }
        }

        /// <summary>
        /// Attempt to cancel specified commands for the connection.
        /// </summary>
        /// <param name="Connection">The connection to cancel commands for.</param>
        /// <param name="RequestIds">RequestIDs to cancel.</param>
        /// <param name="token">CancellationToken to use while waiting for our SyncObject.</param>
        /// <returns>True if any specified ids are cancelled.</returns>
        private async Task TryCancelSpecificAsync(IConnection Connection, List<int> RequestIds, CancellationToken token)
        {
            using var sync = await DisposableLock.Create(syncObject, token);

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
                        await foundID.CommandHandler.HandleError(foundID.Command, new TimeoutCanceledException(true));
                        foundID.cts.Dispose();
                        Contents.Remove(foundID);
                        NewItemEvent.RemovedItem();
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve the next item in the queue.
        /// </summary>
        private async Task<QueueItem> ReceiveItemAsync(CancellationToken token)
        {
            await NewItemEvent.WaitAsync(token);

            using var sync = await DisposableLock.Create(syncObject, token);

            Contracts.IsTrue(Contents.Count > 0, "No item found after NewItemEvent signaled.");
            CurrentCommand.IsNull($"Expected {nameof(CurrentCommand)} to be null.");
            CurrentCommand = Contents[0];
            Contents.RemoveAt(0);

            return CurrentCommand.IsNotNull($"{nameof(CurrentCommand)} was null.");
        }

        /// <summary>
        /// Run the command queue
        /// </summary>
        public async Task RunAsync(CancellationToken token)
        {
            // Execute queued commands
            while (!token.IsCancellationRequested)
            {
                //Wait for item to be received.
                var (handler, _, command, cts) = await ReceiveItemAsync(token);

                try
                {
                    Logger.Log("Dispatcher", $"Running {command.Header.Name} id:{command.Header.RequestId}");

                    //Throw OperationCanceledException if command timeout has already been reached.
                    cts.Token.ThrowIfCancellationRequested();
                    await handler.Handle(command, cts.Token);

                    Logger.Log("Dispatcher", $"Completed {command.Header.Name} id:{command.Header.RequestId}");
                }
                catch (Exception ex)
                {
                    Logger.Log("Dispatcher", $"Caught exception running {command.Header.Name} id:{command.Header.RequestId}.{Environment.NewLine}{ex}");

                    if (ex is TaskCanceledException or OperationCanceledException)
                    {
                        //Check if cancel was requested.
                        bool cancelRequested = false;

                        using var sync = await DisposableLock.Create(syncObject, token);
                        cancelRequested = CurrentCommandCancelRequested;
                        ex = new TimeoutCanceledException(ex.Message, ex, cancelRequested);
                    }

                    await handler.HandleError(command, ex);
                }

                //Command is complete - unassign CurrentCommand.
                {
                    using var sync = await DisposableLock.Create(syncObject, token);
                    CurrentCommand = null;
                    CurrentCommandCancelRequested = false;
                }

                cts.Dispose();
            }
        }

        private readonly SemaphoreSlim syncObject = new(1, 1);
        private readonly List<QueueItem> Contents = new();
        private readonly AsyncAutoResetEvent NewItemEvent = new();
        private QueueItem CurrentCommand = null;
        private bool CurrentCommandCancelRequested = false;
        private readonly ILogger Logger;

        private class AsyncAutoResetEvent
        {
            public async Task WaitAsync(CancellationToken token)
            {
                TaskCompletionSource<bool> tcs = new();
                using CancellationTokenRegistration ctr = token.Register(() => tcs.TrySetCanceled());

                lock (WaitingTaskCompletionSources)
                {
                    token.ThrowIfCancellationRequested();
                    if (Signaled > 0)
                    {
                        Signaled--;
                        return;
                    }
                    else
                        WaitingTaskCompletionSources.Enqueue(tcs);
                }

                await tcs.Task;
            }

            public void Set()
            {
                TaskCompletionSource<bool> sourceToSignal = null;
                lock (WaitingTaskCompletionSources)
                {

                    if (WaitingTaskCompletionSources.Count > 0)
                        sourceToSignal = WaitingTaskCompletionSources.Dequeue();
                    else
                    {
                        Signaled++;
                    }
                }
			}

            public void RemovedItem()
            {
                lock(WaitingTaskCompletionSources)
                {
                    if (Signaled > 0)
                        Signaled--;
                }
            }

            private readonly static Task CompletedTask = Task.FromResult(true);
            private readonly Queue<TaskCompletionSource<bool>> WaitingTaskCompletionSources = new Queue<TaskCompletionSource<bool>>();
            private int Signaled;
        }
    }
}
