/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
                    ConstructorInfo constructorInfo = handlerClass.GetConstructor(new Type[] { typeof(ICommandDispatcher), typeof(ILogger) });
                    Contracts.IsNotNull(constructorInfo, $"Failed to find constructor for {handlerClass}");
                }
                catch (Exception e) when (e.Message.Contains("Failed to find constructor"))
                {
                    Contracts.Fail($"Handler class {handlerClass.FullName} doesn't have a constructor that takes ({nameof(ICommandDispatcher)},{nameof(ILogger)}). Exception: {e.Message}");
                }
            }
        }

        public async Task Dispatch(IConnection Connection, MessageBase Command)
        {
            Connection.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(Connection)}");
            Command.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(Command)}");

            var cts = new CancellationTokenSource();
            (ICommandHandler handler, bool async) = CreateHandler(Command.GetType());
            if (async)
            {
                Logger.Log("Dispatcher", $"Running {Command.Headers.Name} id:{Command.Headers.RequestId}");
                await handler.Handle(Connection, Command, cts.Token);
                Logger.Log("Dispatcher", $"Completed {Command.Headers.Name} id:{Command.Headers.RequestId}");
                cts.Dispose();
            }
            else
            {
                Logger.Log("Dispatcher", $"Queing command a {handler} handler for {Command.Headers.Name} id:{Command.Headers.RequestId}");
                CommandQueue.Post(new CommandQueueRecord(handler, Connection, Command, cts));
            }
        }

        public Task DispatchError(IConnection Connection, MessageBase Command, Exception CommandErrorexception)
        {
            Connection.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(Connection)}");
            Command.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(Command)}");
            CommandErrorexception.IsNotNull($"Invalid parameter in the {nameof(Dispatch)} method. {nameof(CommandErrorexception)}");

            var (handler, _) = CreateHandler(Command.GetType());
            return handler.HandleError( Connection, Command, CommandErrorexception ) ?? Task.CompletedTask;
        }

        private (ICommandHandler handler, bool async) CreateHandler(Type type)
        {
            Type handlerClass = MessageHandlers[type].Type;
            bool async = MessageHandlers[type].Async;
            Contracts.IsTrue(
                typeof(ICommandHandler).IsAssignableFrom(handlerClass),
                $"Class {handlerClass.Name} is registered to handle {type.Name} but isn't a {nameof(ICommandHandler)}");

            //Logger.Log(Constants.Component, $"Dispatch: Handling {type} message with {handlerClass.Name}");

            // Create a new handler object. Effectively the same as: 
            // ICommandHandler handler = new handlerClass( this, Logger );
            var handler =  handlerClass.GetConstructor(new Type[] { typeof(ICommandDispatcher), typeof(ILogger) })
                               .IsNotNull($"Failed to find constructor for {handlerClass}")
                               .Invoke(parameters: new object[] { this, Logger })
                               .IsA<ICommandHandler>();
            return (handler, async);
        }

        private readonly BufferBlock<CommandQueueRecord> CommandQueue = new BufferBlock<CommandQueueRecord>();
        private record CommandQueueRecord ( ICommandHandler CommandHandler, IConnection Connection, MessageBase command, CancellationTokenSource cts );

        public virtual async Task RunAsync()
        {
            // Execute queued commands
            while( true )
            {
                var (handler, connection, command, cts) = await CommandQueue.ReceiveAsync();
                try
                {
                    Logger.Log("Dispatcher", $"Running {command.Headers.Name} id:{command.Headers.RequestId}");
                    await handler.Handle(connection, command, cts.Token);
                    Logger.Log("Dispatcher", $"Completed {command.Headers.Name} id:{command.Headers.RequestId}");
                }
                catch (Exception ex)
                {
                    Logger.Log("Dispatcher", $"Caught exception running {command.Headers.Name} id:{command.Headers.RequestId}");
                    await handler.HandleError(connection, command, ex);
                }
                cts.Dispose();
            }
        }
        private void Add(IEnumerable<(Type, Type, bool Async)> types)
        {
            foreach (var (Message, Handler, async) in types)
                MessageHandlers.Add(Message, new HandlerDetails(Handler, async));
        }

        private readonly Dictionary<Type, HandlerDetails > MessageHandlers = new();

        private record HandlerDetails(Type Type, bool Async);

        private readonly ILogger Logger;

        public IEnumerable<Type> Commands { get => MessageHandlers.Keys; }
        public IEnumerable<XFSConstants.ServiceClass> ServiceClasses { get; }
    }
}
