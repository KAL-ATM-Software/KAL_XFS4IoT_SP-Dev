/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * TestCashUnitsHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Dispenser.Commands;
using XFS4IoT.Dispenser.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Dispenser
{
    [CommandHandler(XFSConstants.ServiceClass.Dispenser, typeof(TestCashUnitsCommand))]
    public partial class TestCashUnitsHandler : ICommandHandler
    {
        public TestCashUnitsHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(TestCashUnitsHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(TestCashUnitsHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IDispenserDevice>();

            Dispenser = Provider.IsA<IDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(TestCashUnitsHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var testCashUnitsCmd = command.IsA<TestCashUnitsCommand>($"Invalid parameter in the TestCashUnits Handle method. {nameof(TestCashUnitsCommand)}");
            testCashUnitsCmd.Headers.RequestId.HasValue.IsTrue();

            ITestCashUnitsEvents events = new TestCashUnitsEvents(Connection, testCashUnitsCmd.Headers.RequestId.Value);

            var result = await HandleTestCashUnits(events, testCashUnitsCmd, cancel);
            await Connection.SendMessageAsync(new TestCashUnitsCompletion(testCashUnitsCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var testCashUnitscommand = command.IsA<TestCashUnitsCommand>();
            testCashUnitscommand.Headers.RequestId.HasValue.IsTrue();

            TestCashUnitsCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => TestCashUnitsCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => TestCashUnitsCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => TestCashUnitsCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new TestCashUnitsCompletion(testCashUnitscommand.Headers.RequestId.Value, new TestCashUnitsCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IDispenserDevice Device { get => Provider.Device.IsA<IDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private IDispenserServiceClass Dispenser { get; }
        private ILogger Logger { get; }
    }

}
