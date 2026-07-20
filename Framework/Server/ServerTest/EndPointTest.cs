/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ErrorHandling.ErrorHandler = TestErrorHandler;
        }

        private static readonly ErrorHandling.ErrorDelegate TestErrorHandler = message => throw new TestFatalErrorException(message);

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
   
        [TestMethod]
        [Timeout(30000)]
    public async Task EndPointConnections_AreSafeToEnumerateDuringConnectionChurn()
        {
            var backgroundFaults = new System.Collections.Concurrent.ConcurrentQueue<string>();

            void RecordBackgroundFault(string source, Exception exception)
            {
                var message = $"{source}: {exception}";
                backgroundFaults.Enqueue(message);
                TestContext?.WriteLine(message);
            }

            UnhandledExceptionEventHandler unhandledExceptionHandler = (sender, args) =>
            {
                RecordBackgroundFault(nameof(AppDomain.UnhandledException), args.ExceptionObject as Exception ?? new Exception(args.ExceptionObject?.ToString()));
            };
            EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler = (sender, args) =>
            {
                RecordBackgroundFault(nameof(TaskScheduler.UnobservedTaskException), args.Exception);
                args.SetObserved();
            };

            AppDomain.CurrentDomain.UnhandledException += unhandledExceptionHandler;
            TaskScheduler.UnobservedTaskException += unobservedTaskExceptionHandler;

            // --- Arrange: a real EndPoint on a test port, pumping in the background ---
            // (mirror however EndPointTest.cs constructs these: decoder, dispatcher, logger)
            using var endpoint = new EndPoint(
                new Uri("http://localhost:8099/XFS/CardReader/"),
                CommandDecoder: CommandDecoder,       // reuse EndPointTest's fields/helpers
                Logger: new SilentLogger());

            using var cts = new CancellationTokenSource();
            var runTask = endpoint.RunAsync(cts.Token);  // the loop that Add/Removes ConnectionDetails
            var runTaskFaultObserver = runTask.ContinueWith(
                task => RecordBackgroundFault("EndPoint.RunAsync", task.Exception),
                TaskContinuationOptions.OnlyOnFaulted);
            var wsUri = new Uri("ws://localhost:8099/XFS/CardReader/");

            InvalidOperationException enumerationFailure = null;

            try
            {
                // --- Churn: rapidly connect and gracefully close raw clients (drives Add/Remove) ---
                var churn = Task.Run(async () =>
                {
                    try
                    {
                        while (!cts.IsCancellationRequested)
                        {
                            using var ws = new System.Net.WebSockets.ClientWebSocket();
                            try
                            {
                                await ws.ConnectAsync(wsUri, cts.Token);
                                using var shortCts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
                                await ws.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, string.Empty, shortCts.Token);
                            }
                            catch
                            {
                            }

                            await Task.Delay(1, cts.Token);
                        }
                    }
                    catch
                    {
                    }
                });

                // --- Enumerate exactly like BroadcastEvent does, in a tight loop ---
                var enumerate = Task.Run(() =>
                {
                    try
                    {
                        var sw = System.Diagnostics.Stopwatch.StartNew();
                        while (sw.Elapsed < TimeSpan.FromSeconds(6) && !cts.IsCancellationRequested)
                        {
                            foreach (var c in endpoint.Connections)   // <-- the line under test
                            {
                                System.Threading.Thread.SpinWait(40); // widen the window -> deterministic on unpatched
                            }
                        }
                    }
                    catch (InvalidOperationException ex) { enumerationFailure = ex; }  // unpatched: "Collection was modified"
                    finally { cts.Cancel(); }
                });

                await Task.WhenAll(churn, enumerate);

                try
                {
                    await runTask;
                }
                catch (Exception ex)
                {
                    RecordBackgroundFault("EndPoint.RunAsync awaited", ex);
                }
            }
            finally
            {
                cts.Cancel();
                AppDomain.CurrentDomain.UnhandledException -= unhandledExceptionHandler;
                TaskScheduler.UnobservedTaskException -= unobservedTaskExceptionHandler;
            }

            foreach (var backgroundFault in backgroundFaults)
            {
                TestContext?.WriteLine(backgroundFault);
            }

            // --- Assert ---
            Assert.IsNull(enumerationFailure,
                $"Enumerating EndPoint.Connections threw under connection churn: {enumerationFailure}");
            Assert.IsTrue(backgroundFaults.IsEmpty,
                $"Background fault captured during connection churn: {string.Join(Environment.NewLine, backgroundFaults)}");
        }

        //[TestMethod]
        public void TestRunEndpoint()
        {
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CardReader.ReadRawData", typeof(ReadRawDataCommand));

            var test = new EndPoint(new System.Uri("http://localhost:8088/XFS/CashAcceptor/"),
                CommandDecoder,
                Logger);
            test.Register("/XFS/CashAcceptor/", CommandDispatcher, null);

            test.RunAsync(CancellationToken.None).Wait();

            Inconclusive();
        }

        /// <summary>
        /// Test set of commands to support
        /// </summary>
        private readonly IMessageDecoder CommandDecoder = new MessageDecoder();

        private readonly ICommandDispatcher CommandDispatcher = new TestCommandDispatcher()
        {
        };
        private readonly XFS4IoTServer.IServiceProvider ServiceProvider = new TestServiceProvider();
        private readonly ILogger Logger = new DebugLogger();

        private class TestFatalErrorException : Exception
        {
            public TestFatalErrorException(string message) : base(message)
            {
            }
        }

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

        private class SilentLogger : ILogger
        {
            public void Trace(string SubSystem, string Operation, string Message) { }

            public void Warning(string SubSystem, string Message) { }

            public void Log(string SubSystem, string Message) { }

            public void TraceSensitive(string SubSystem, string Operation, string Message) { }

            public void WarningSensitive(string SubSystem, string Message) { }

            public void LogSensitive(string SubSystem, string Message) { }
        }

        private class TestCommandDispatcher : ICommandDispatcher
        {
            public Task Dispatch(XFS4IoTServer.IServiceProvider ServiceProvider, IConnection Connection, MessageBase Command, CancellationToken Token) => throw new System.NotImplementedException();
            public Task DispatchError(XFS4IoTServer.IServiceProvider ServiceProvider, IConnection Connection, MessageBase Command, Exception CommandException) => throw new System.NotImplementedException();
            public Task RunAsync(CancellationSource cancellationSource) => throw new System.NotImplementedException();
            public IEnumerator GetEnumerator() => throw new System.NotImplementedException();
            public Task CancelCommandsAsync(IConnection Connection, List<int> RequestIds, CancellationToken Token) => Task.CompletedTask;
            public Task<bool> AnyValidRequestID(IConnection Connection, List<int> RequestIds, CancellationToken token) => throw new NotImplementedException();
            public IEnumerable<Type> Commands { get => throw new NotImplementedException(); }
        }

        private class TestServiceProvider : TestCommandDispatcher, XFS4IoTServer.IServiceProvider
        {
            public string Name => nameof(TestServiceProvider);
            public Uri Uri => new("http://localhost:8099/XFS/CardReader/");
            public Uri WSUri => new("ws://localhost:8099/XFS/CardReader/");
            public IDevice Device => null;
            public Task BroadcastEvent(object payload) => Task.CompletedTask;
            public Task BroadcastEvent(IEnumerable<IConnection> connections, object payload) => Task.CompletedTask;
            public void SetJsonSchemaValidator(IJsonSchemaValidator JsonSchemaValidator) { }
            public void SetMessagesSupported(Dictionary<string, MessageTypeInfo> MessagesSupported) { }
            public Dictionary<string, MessageTypeInfo> GetMessagesSupported() => new();
        }
    }
}
