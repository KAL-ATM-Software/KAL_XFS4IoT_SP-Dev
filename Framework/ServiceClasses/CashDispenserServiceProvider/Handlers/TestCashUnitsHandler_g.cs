/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * TestCashUnitsHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashDispenser
{
    [CommandHandler(XFSConstants.ServiceClass.CashDispenser, typeof(TestCashUnitsCommand))]
    public partial class TestCashUnitsHandler : ICommandHandler
    {
        public TestCashUnitsHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(TestCashUnitsHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(TestCashUnitsHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashDispenserDevice>();

            CashDispenser = Provider.IsA<ICashDispenserService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(TestCashUnitsHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(TestCashUnitsHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var testCashUnitsCmd = command.IsA<TestCashUnitsCommand>($"Invalid parameter in the TestCashUnits Handle method. {nameof(TestCashUnitsCommand)}");
            testCashUnitsCmd.Header.RequestId.HasValue.IsTrue();

            ITestCashUnitsEvents events = new TestCashUnitsEvents(Connection, testCashUnitsCmd.Header.RequestId.Value);

            var result = await HandleTestCashUnits(events, testCashUnitsCmd, cancel);
            await Connection.SendMessageAsync(new TestCashUnitsCompletion(testCashUnitsCmd.Header.RequestId.Value, result.Payload, result.CompletionCode, result.ErrorDescription));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var testCashUnitsCommand = command.IsA<TestCashUnitsCommand>();
            testCashUnitsCommand.Header.RequestId.HasValue.IsTrue();

            MessageHeader.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => MessageHeader.CompletionCodeEnum.InvalidData,
                InternalErrorException => MessageHeader.CompletionCodeEnum.InternalError,
                UnsupportedDataException => MessageHeader.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => MessageHeader.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => MessageHeader.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => MessageHeader.CompletionCodeEnum.HardwareError,
                UserErrorException => MessageHeader.CompletionCodeEnum.UserError,
                FraudAttemptException => MessageHeader.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => MessageHeader.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => MessageHeader.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => MessageHeader.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => MessageHeader.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => MessageHeader.CompletionCodeEnum.TimeOut,
                _ => MessageHeader.CompletionCodeEnum.InternalError
            };

            var response = new TestCashUnitsCompletion(testCashUnitsCommand.Header.RequestId.Value, null, errorCode, commandException.Message);

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICashDispenserDevice Device { get => Provider.Device.IsA<ICashDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashDispenserService CashDispenser { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
