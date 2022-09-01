/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * PresentHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashDispenser, typeof(PresentCommand))]
    public partial class PresentHandler : ICommandHandler
    {
        public PresentHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PresentHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(PresentHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashDispenserDevice>();

            CashDispenser = Provider.IsA<ICashDispenserService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(PresentHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(PresentHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var presentCmd = command.IsA<PresentCommand>($"Invalid parameter in the Present Handle method. {nameof(PresentCommand)}");
            presentCmd.Header.RequestId.HasValue.IsTrue();

            IPresentEvents events = new PresentEvents(Connection, presentCmd.Header.RequestId.Value);

            var result = await HandlePresent(events, presentCmd, cancel);
            await Connection.SendMessageAsync(new PresentCompletion(presentCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var presentcommand = command.IsA<PresentCommand>();
            presentcommand.Header.RequestId.HasValue.IsTrue();

            PresentCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => PresentCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => PresentCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => PresentCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => PresentCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => PresentCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => PresentCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => PresentCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => PresentCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => PresentCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => PresentCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => PresentCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => PresentCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => PresentCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => PresentCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => PresentCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new PresentCompletion(presentcommand.Header.RequestId.Value, new PresentCompletion.PayloadData(errorCode, commandException.Message));

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
