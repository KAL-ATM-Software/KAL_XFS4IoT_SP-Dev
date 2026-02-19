/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Commands;
using XFS4IoT.ServicePublisher.Commands;
using XFS4IoT.ServicePublisher.Completions;

namespace XFS4IoTServer.Test
{

    [TestClass]
    public class MessageDispatcherTest
    {

        [TestMethod]
        public async Task NewMessageDispatcherTest()
        {
            var dispatcher = new TestServiceProvider(new[] { XFSConstants.ServiceClass.Publisher }, new TestLogger());

            await dispatcher.Dispatch(dispatcher, new TestConnection(), new TestMessage1(), CancellationToken.None);
            await dispatcher.Dispatch(dispatcher, new TestConnection(), new TestMessage2(), CancellationToken.None);
            await dispatcher.Dispatch(dispatcher, new TestConnection(), new TestMessage3(), CancellationToken.None);
        }

        public sealed class TestServiceProvider : CommandDispatcher, IServiceProvider
        {
            public TestServiceProvider(IEnumerable<XFSConstants.ServiceClass> Services, ILogger Logger)
            : base(Services, Logger)
            {
                List<Type> testCommands = [typeof(TestMessage1), typeof(TestMessage2), typeof(TestMessage3)];
                Dictionary<string, MessageTypeInfo> supportedMsgs = [];
                foreach (var type in testCommands)
                {
                    CommandAttribute commandAttrib = Attribute.GetCustomAttribute(type, typeof(CommandAttribute)) as CommandAttribute;
                    Assert.AreEqual(true, commandAttrib is not null);

                    XFS4VersionAttribute versionAttrib = Attribute.GetCustomAttribute(type, typeof(XFS4VersionAttribute)) as XFS4VersionAttribute;
                    Assert.AreEqual(true, versionAttrib is not null);

                    supportedMsgs.Add(commandAttrib.Name, new MessageTypeInfo(MessageTypeInfo.MessageTypeEnum.Command, new([versionAttrib.Version])));

                    MessageCollection.Add(MessageHeader.TypeEnum.Command, commandAttrib.Name, type);
                    if (type == typeof(TestMessage1))
                    {
                        AddHandler(this, type, (connection, dispatcher, logger) => new TestMessageHandler1(connection, dispatcher, logger), true);
                    }
                    else
                    {
                        AddHandler(this, type, (connection, dispatcher, logger) => new TestMessageHandler2(connection, dispatcher, logger), true);
                    }
                }

                SetMessagesSupported(supportedMsgs);
            }

            public string Name { get; } = String.Empty;
            public Uri Uri { get; }
            public Uri WSUri { get; }
            public IDevice Device { get => Contracts.Fail<IDevice>("A device object was requested from the Publisher service, but the publisher service does not have a device class"); }

            public Task BroadcastEvent(object payload)
            {
                throw Contracts.Fail<Exception>($"No broadcast events defined for test method.");
            }
            public void SetJsonSchemaValidator(IJsonSchemaValidator JsonSchemaValidator)
            {
                throw Contracts.Fail<Exception>($"No jason schema validator supported for test method.");
            }

            public Task BroadcastEvent(IEnumerable<IConnection> connections, object payload)
            {
                throw Contracts.Fail<Exception>($"No broadcast events defined for the service publisher. Do not call {nameof(BroadcastEvent)} on this class.");
            }

            public void SetMessagesSupported(Dictionary<string, MessageTypeInfo> MessagesSupported)
            {
                base.MessagesSupported = MessagesSupported;
            }
            public Dictionary<string, MessageTypeInfo> GetMessagesSupported() => base.MessagesSupported;
        }
    }

    public class TestServiceProvider : IServiceProvider
    {
        public string Name { get; } = String.Empty;
        public Uri Uri { get; } = new Uri(string.Empty);
        public Uri WSUri { get; } = new Uri(string.Empty);
        public IDevice Device { get => throw new NotImplementedException(); }
        public Task BroadcastEvent(object payload) => throw new NotImplementedException();
        public Task BroadcastEvent(IEnumerable<IConnection> connections, object payload) => throw new NotImplementedException();
        public Task<bool> AnyValidRequestID(IConnection Connection, List<int> RequestIds, CancellationToken token) => throw new NotImplementedException();
        public Task CancelCommandsAsync(IConnection Connection, List<int> RequestIds, CancellationToken Token) => throw new NotImplementedException();
        public Task Dispatch(IServiceProvider ServiceProvider, IConnection Connection, MessageBase Command, CancellationToken Token) => throw new NotImplementedException();
        public Task DispatchError(IServiceProvider ServiceProvider, IConnection Connection, MessageBase Command, Exception CommandException) => throw new NotImplementedException();
        public Task RunAsync(CancellationSource cancellationSource) => throw new NotImplementedException();
        public void SetJsonSchemaValidator(IJsonSchemaValidator JsonSchemaValidator) => throw new NotImplementedException();
        /// <summary>
        /// Set supported commands and events to the dispatcher.
        /// </summary>
        /// <param name="MessagesSupported"></param>
        public void SetMessagesSupported(Dictionary<string, MessageTypeInfo> MessagesSupported)
        { }
        public Dictionary<string, MessageTypeInfo> GetMessagesSupported() => [];
    }

    internal class TestLogger : ILogger
    {
        private static void WriteLine(string v) => System.Diagnostics.Debug.Write(v);

        public void Trace(string SubSystem, string Operation, string Message) => WriteLine($"SubSystem:{SubSystem},Operation:{Operation},Event:{Message}");

        public void Warning(string SubSystem, string Message) => Trace(SubSystem, "WARNING", Message);

        public void Log(string SubSystem, string Message) => Trace(SubSystem, "INFO", Message);

        public void TraceSensitive(string SubSystem, string Operation, string Message) => Trace(SubSystem, Operation, Message);

        public void WarningSensitive(string SubSystem, string Message) => Warning(SubSystem, Message);

        public void LogSensitive(string SubSystem, string Message) => Log(SubSystem, Message);
    }

    class TestConnection : IConnection
    {
        public Task SendMessageAsync(object result) => Task.CompletedTask; //Return CompletedTask when sending Acknowledge
    }

    [XFS4Version(Version = "2.0")]
    [Command(Name = "Common.TestMessage1")]
    public class TestMessage1 : XFS4IoT.Commands.Command<XFS4IoT.Commands.MessagePayload>
    {
        public TestMessage1() : base(new Random().Next(), null, 0)
        { }
    }
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Common.TestMessage2")]
    public class TestMessage2 : XFS4IoT.Commands.Command<XFS4IoT.Commands.MessagePayload>
    {
        public TestMessage2() : base(new Random().Next(), null, 0)
        { }
    }
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Common.TestMessage3")]
    public class TestMessage3 : XFS4IoT.Commands.Command<XFS4IoT.Commands.MessagePayload>
    {
        public TestMessage3() : base(new Random().Next(), null, 0)
        { }
    }

    [CommandHandler(XFSConstants.ServiceClass.Publisher, typeof(TestMessage1))]
    public class TestMessageHandler1 : ICommandHandler
    {
        public TestMessageHandler1(IConnection _, ICommandDispatcher _1, ILogger _2 ){}

        public async Task Handle(object command, CancellationToken cancel)
        {
            Assert.IsInstanceOfType(command, typeof(TestMessage1));
            await Task.CompletedTask;
        }

        public async Task HandleError(object command, Exception commandException)
        {
            Assert.IsInstanceOfType(command, typeof(TestMessage1));
            await Task.CompletedTask;
        }
    }
    [CommandHandler(XFSConstants.ServiceClass.Publisher, typeof(TestMessage2))]
    [CommandHandler(XFSConstants.ServiceClass.Publisher, typeof(TestMessage3))]
    //[CommandHandler(typeof(Int32))] // Non-CommandMessage types will FE on process startup. 
    public class TestMessageHandler2 : ICommandHandler
    {
        public TestMessageHandler2(IConnection _, ICommandDispatcher _1, ILogger _2) { }

        public async Task Handle(object command, CancellationToken cancel)
        {
            Assert.IsTrue(command is TestMessage2 || command is TestMessage3);

            await Task.CompletedTask;
        }

        public async Task HandleError(object command, Exception commandException)
        {
            Assert.IsTrue(command is TestMessage2 || command is TestMessage3);

            await Task.CompletedTask;
        }
    }
}
