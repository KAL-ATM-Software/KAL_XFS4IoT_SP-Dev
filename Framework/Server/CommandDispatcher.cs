/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using XFS4IoT;

namespace XFS4IoTServer
{
    public class CommandDispatcher : ICommandDispatcher
    {
        public CommandDispatcher(IEnumerable<XFSConstants.ServiceClass> Services, ILogger Logger)
        {
            this.Logger = Logger.IsNotNull();
            this.ServiceClasses = Services.IsNotNull();
            CommandQueue = new(Logger);
        }

        public async Task Dispatch(IServiceProvider ServiceProvider, IConnection Connection, MessageBase Command, CancellationToken token)
        {
            try
            {
                Connection.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(Connection)}");
                Command.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(Command)}");

                await Connection.SendMessageAsync(new Acknowledge(Command.Header.RequestId.Value, Command.Header.Name, Command.Header.Version, null));

                //Use linked cancellation token to ensure we cancel if the parent token is cancelled.
                CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(token);
                
                //Use timeout if it is not infinite
                if (Command.Header.Timeout is not null && Command.Header.Timeout > 0)
                    cts.CancelAfter((int)Command.Header.Timeout);

                (ICommandHandler handler, bool async) = CreateHandler(ServiceProvider, Command.GetType(), Connection);

                // Check supported command and version by the device class
                if (!MessagesSupported.ContainsKey(Command.Header.Name) ||
                    !MessagesSupported[Command.Header.Name].Versions.IsNotNull($"No command version stored internally for {Command.Header.Name} command.").Contains(Command.Header.Version))
                {
                    await handler.HandleError(Command, 
                                              new NotSupportedException(!MessagesSupported.ContainsKey(Command.Header.Name) ?
                                                $"{Command.Header.Name} Command is not supported by the service." :
                                                $"{Command.Header.Name} Command version ({Command.Header.Version}) is not supported by the service."));
                    return;
                }

                // Received command is supported by the device and process it.
                if (async)
                {
                    try
                    {
                        Logger.Log("Dispatcher", $"Running {Command.Header.Name} id:{Command.Header.RequestId}");
                        await handler.Handle(Command, cts.Token);
                        Logger.Log("Dispatcher", $"Completed {Command.Header.Name} id:{Command.Header.RequestId}");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Dispatcher", $"Caught exception running async command {Command.Header.Name} id:{Command.Header.RequestId}.{Environment.NewLine}{ex}");

                        if (ex is TaskCanceledException or OperationCanceledException)
                        {
                            ex = new TimeoutCanceledException(ex.Message, ex, false);
                        }

                        await handler.HandleError(Command, ex);
                    }

                    cts.Dispose();
                }
                else
                {
                    Logger.Log("Dispatcher", $"Queing command a {handler} handler for {Command.Header.Name} id:{Command.Header.RequestId}");
                    await CommandQueue.EnqueueCommandAsync(handler, Connection, Command, cts);
                }
            }
            catch (Exception e) when (e.InnerException is System.Net.WebSockets.WebSocketException we)
            {
                Logger.Log("Dispatcher", $"Exception responding to client - assume the client went away. {we}");
            }
            catch(Exception e) when (e is TaskCanceledException or OperationCanceledException)
            {
                Connection.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(Connection)}");
                Command.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(Command)}");

                Exception ex = new TimeoutCanceledException(e.Message, e, false);
                var (handler, _) = CreateHandler(ServiceProvider, Command.GetType(), Connection);
                await handler.HandleError(Command, ex);
            }
        }

        public Task DispatchError(IServiceProvider ServiceProvider, IConnection Connection, MessageBase Command, Exception CommandErrorexception)
        {
            Connection.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(Connection)}");
            Command.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(Command)}");
            CommandErrorexception.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(CommandErrorexception)}");

            var (handler, _) = CreateHandler(ServiceProvider, Command.GetType(), Connection);
            return handler.HandleError( Command, CommandErrorexception ) ?? Task.CompletedTask;
        }

        // Add supported command handlers mapping with the command type
        // This method must be called from each device class implementation
        public static void AddHandler(IServiceProvider ServiceProvider, Type commandType, Func<IConnection, ICommandDispatcher, ILogger, ICommandHandler> factory, bool async)
        {
            Dictionary<Type, HandlerFactoryClass> newHandler = new()
            {
                { commandType, new(factory, async) }
            };
            handlerFactories.TryAdd(ServiceProvider, newHandler);
            handlerFactories.TryGetValue(ServiceProvider, out var existingFactory).IsTrue($"Faild to find handler list per connection.  {ServiceProvider.GetType().Name}");
            existingFactory.TryAdd(commandType, new HandlerFactoryClass(factory, async));
        }

        /// <summary>
        /// Return the handler associated with command type to execute.
        /// </summary>
        private (ICommandHandler handler, bool async) CreateHandler(IServiceProvider ServiceProvider, Type type, IConnection connection)
        {
            handlerFactories.TryGetValue(ServiceProvider, out var handlers).IsTrue($"No handler registered for {connection.GetType().Name}");
            handlers.TryGetValue(type, out var hander).IsTrue($"No handler registered for {type.Name}");

            try
            {
                return (hander.Factory(connection, this, Logger), hander.Async);
            }
            catch (Exception ex)
            {
                Contracts.Fail($"Caught exception while assigning hander for {type.Name}. {ex}");
            }

            // won't reach here, but to make the compiler happy.
            return (null, false);
        }

        // Hold handers supported by the framework
        private sealed class HandlerFactoryClass(Func<IConnection, ICommandDispatcher, ILogger, ICommandHandler> Factory, bool Async)
        {
            public bool Async { get; init; } = Async;
            public Func<IConnection, ICommandDispatcher, ILogger, ICommandHandler> Factory { get; init; } = Factory.IsNotNull($"Invalid parameter in the {nameof(HandlerFactoryClass)} constructor. {nameof(Factory)}");
        }
        private static readonly Dictionary<IServiceProvider, Dictionary<Type, HandlerFactoryClass>> handlerFactories = [];

        private readonly CommandQueue CommandQueue;

        public virtual Task RunAsync(CancellationSource cancellationSource) => CommandQueue.RunAsync(cancellationSource.Token);

        public virtual async Task<bool> AnyValidRequestID(IConnection Connection, List<int> RequestIds, CancellationToken token)
        {
            return await CommandQueue.AnyValidRequestID(Connection, RequestIds, token);
        }

        public virtual async Task CancelCommandsAsync(IConnection Connection, List<int> RequestIds, CancellationToken token)
        {
            await CommandQueue.TryCancelItemsAsync(Connection, RequestIds, token);
        }

        private record HandlerDetails(Type Type, bool Async);

        private protected readonly ILogger Logger;

        public IEnumerable<XFSConstants.ServiceClass> ServiceClasses { get; protected set; }

        /// <summary>
        /// Storing the commands and events supported by the device class for received command check
        /// </summary>
        public Dictionary<string, MessageTypeInfo> MessagesSupported { get; set; }
    }
}
