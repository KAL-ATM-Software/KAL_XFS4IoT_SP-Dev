/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using XFS4IoT;

namespace XFS4IoTServer
{
    public class CommandDispatcher : ICommandDispatcher
    {
        public CommandDispatcher(IEnumerable<XFSConstants.ServiceClass> Services, ILogger Logger, AssemblyName AssemblyName = null)
        {
            this.Logger = Logger.IsNotNull();
            this.ServiceClasses = Services.IsNotNull();
            CommandQueue = new(Logger);

            // Find all the classes (in the named assembly if a name is give,) which 
            // have the CommandHandlerAttribute, and match them with the 'Type' value on 
            // that attribute. 
            // that is, for [CommandHandler(typeof(MessageA))] class HandlerA : ICommandHandler
            // record that MessageA is handled by HandlerA

            Add(from assem in AppDomain.CurrentDomain.GetAssemblies()
                where assem.IsDynamic == false && (AssemblyName == null || assem.GetName().FullName == AssemblyName.FullName)
                from type in assem.ExportedTypes
                from CustomAttributeData attrib in type.CustomAttributes
                where attrib.AttributeType == typeof(CommandHandlerAttribute)
                where attrib.ConstructorArguments.Count == 2
                where attrib.ConstructorArguments[0].ArgumentType == typeof(XFSConstants.ServiceClass) && ServiceClasses.Contains((XFSConstants.ServiceClass)attrib.ConstructorArguments[0].Value)
                where attrib.ConstructorArguments[1].ArgumentType == typeof(Type)
                let namedArg = attrib.ConstructorArguments[1].Value
                let async = (
                                from asyncAttrib in type.CustomAttributes
                                where asyncAttrib.AttributeType == typeof(CommandHandlerAsyncAttribute)
                                select asyncAttrib
                            )
                            .FirstOrDefault() != default
                select (message: namedArg as Type, handler: type, async)
                );

            var handlers = string.Join("\n", from next in MessageHandlers select $"{next.Key} => {next.Value.Type} Async:{next.Value.Async}");
            Logger.Log(Constants.Component, $"Dispatch, found Command Handler classes:\n{handlers}");

            // Double check that command handler classes were declared with the right constructor so that we'll be able to use them. 
            // Would be nice to be able to test this at compile time. 
            foreach (Type handlerClass in from x in MessageHandlers.Values select x.Type )
            {
                try
                {
                    ConstructorInfo constructorInfo = handlerClass.GetConstructor(new Type[] { typeof(IConnection), typeof(ICommandDispatcher), typeof(ILogger) });
                    Contracts.IsNotNull(constructorInfo, $"Failed to find constructor for {handlerClass}");
                }
                catch (Exception e) when (e.Message.Contains("Failed to find constructor"))
                {
                    Contracts.Fail($"Handler class {handlerClass.FullName} doesn't have a constructor that takes ({nameof(ICommandDispatcher)},{nameof(ILogger)}). Exception: {e.Message}");
                }
            }
        }

        public async Task Dispatch(IConnection Connection, MessageBase Command, CancellationToken token)
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

                (ICommandHandler handler, bool async) = CreateHandler(Command.GetType(), Connection);

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
                var (handler, _) = CreateHandler(Command.GetType(), Connection);
                await handler.HandleError(Command, ex);
            }
        }

        public Task DispatchError(IConnection Connection, MessageBase Command, Exception CommandErrorexception)
        {
            Connection.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(Connection)}");
            Command.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(Command)}");
            CommandErrorexception.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(CommandErrorexception)}");

            var (handler, _) = CreateHandler(Command.GetType(), Connection);
            return handler.HandleError( Command, CommandErrorexception ) ?? Task.CompletedTask;
        }

        private (ICommandHandler handler, bool async) CreateHandler(Type type, IConnection Connection)
        {
            Type handlerClass = MessageHandlers[type].Type;
            bool async = MessageHandlers[type].Async;
            Contracts.IsTrue(
                typeof(ICommandHandler).IsAssignableFrom(handlerClass),
                $"Class {handlerClass.Name} is registered to handle {type.Name} but isn't a {nameof(ICommandHandler)}");

            // Create a new handler object. Effectively the same as: 
            // ICommandHandler handler = new handlerClass( this, Logger );
            var handler =  handlerClass.GetConstructor(new Type[] { typeof(IConnection), typeof(ICommandDispatcher), typeof(ILogger) })
                               .IsNotNull($"Failed to find constructor for {handlerClass}")
                               .Invoke(parameters: new object[] { Connection, this, Logger })
                               .IsA<ICommandHandler>();
            return (handler, async);
        }

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

        private void Add(IEnumerable<(Type, Type, bool Async)> types)
        {
            foreach (var (Message, Handler, async) in types)
                MessageHandlers.Add(Message, new HandlerDetails(Handler, async));
        }

        private readonly Dictionary<Type, HandlerDetails > MessageHandlers = [];

        private record HandlerDetails(Type Type, bool Async);

        private protected readonly ILogger Logger;

        public IEnumerable<Type> Commands { get => MessageHandlers.Keys; }
        public IEnumerable<XFSConstants.ServiceClass> ServiceClasses { get; }

        /// <summary>
        /// Storing the commands and events supported by the device class for received command check
        /// </summary>
        public Dictionary<string, MessageTypeInfo> MessagesSupported { get; set; }
    }
}
