/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using XFS4IoTServer;

namespace XFS4IoTServerTest
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using XFS4IoT;
    using XFS4IoT.CardReader.Commands;
    using static Assert;

    [TestClass]
    public class EndPointTest
    {
        //[TestMethod]
        public void EndPoint()
        {
            //var test = new EndPoint( 
            //    new System.Uri("http://localhost:8088/XFS/CashAcceptor/"), 
            //    CommandDecoder,
            //    CommandHandler: null,
            //    Logger );

            Inconclusive();
        }

        //[TestMethod]
        public void TestRunEndpoint()
        {
            var test = new EndPoint(new System.Uri("http://localhost:8088/XFS/CashAcceptor/"),
                CommandDecoder: CommandDecoder,
                CommandDispatcher: CommandDispatcher,
                Logger: Logger);

            test.RunAsync().Wait();

            Inconclusive();
        }

        /// <summary>
        /// Test set of commands to support
        /// </summary>
        private readonly IMessageDecoder CommandDecoder = new MessageDecoder(MessageDecoder.AutoPopulateType.Command, Assembly.GetAssembly(typeof(EndPointTest))?.GetName())
        {
            { typeof( ReadRawDataCommand) },
            //{ typeof( ReadRawData) },
        };

        private readonly ICommandDispatcher CommandDispatcher = new TestCommandDispatcher()
        {
        };
        private readonly ILogger Logger = new DebugLogger();

        private class DebugLogger : ILogger
        {
            public void Trace(string SubSystem, string Operation, string Message) => Debug.WriteLine($"{DateTime.Now:hh:mm:ss.fff} ({(DateTime.Now - Start).TotalSeconds:000.000}): {Message}");
        
            public void Warning(string SubSystem, string Message) => Trace(SubSystem, "WARNING", Message);

            public void Log(string SubSystem, string Message) => Trace(SubSystem, "INFO", Message);

            public void TraceSensitive(string SubSystem, string Operation, string Message) => Trace(SubSystem, Operation, Message);

            public void WarningSensitive(string SubSystem, string Message) => Trace(SubSystem, "WARNING", Message);

            public void LogSensitive(string SubSystem, string Message) => Trace(SubSystem, "INFO", Message);

            private readonly DateTime Start = DateTime.Now;
        }

        private class TestCommandDispatcher : ICommandDispatcher
        {
            public Task Dispatch(IConnection Connection, MessageBase Command) => throw new System.NotImplementedException();
            public Task DispatchError(IConnection Connection, MessageBase Command, Exception CommandException) => throw new System.NotImplementedException();
            public Task RunAsync() => throw new System.NotImplementedException();
            public IEnumerator GetEnumerator() => throw new System.NotImplementedException();
            public Task<bool> CancelCommandsAsync(IConnection Connection, List<int> RequestIds) => throw new NotImplementedException();

            public IEnumerable<Type> Commands { get => throw new NotImplementedException(); }
        }
    }
}
