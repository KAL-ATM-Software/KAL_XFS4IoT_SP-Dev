/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * SetMixTableHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashDispenser, typeof(SetMixTableCommand))]
    public partial class SetMixTableHandler : ICommandHandler
    {
        public SetMixTableHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetMixTableHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetMixTableHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashDispenserDevice>();

            CashDispenser = Provider.IsA<ICashDispenserService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetMixTableHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SetMixTableHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var setMixTableCmd = command.IsA<SetMixTableCommand>($"Invalid parameter in the SetMixTable Handle method. {nameof(SetMixTableCommand)}");
            setMixTableCmd.Header.RequestId.HasValue.IsTrue();

            ISetMixTableEvents events = new SetMixTableEvents(Connection, setMixTableCmd.Header.RequestId.Value);

            var result = await HandleSetMixTable(events, setMixTableCmd, cancel);
            await Connection.SendMessageAsync(new SetMixTableCompletion(setMixTableCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setMixTablecommand = command.IsA<SetMixTableCommand>();
            setMixTablecommand.Header.RequestId.HasValue.IsTrue();

            SetMixTableCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetMixTableCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetMixTableCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetMixTableCompletion(setMixTablecommand.Header.RequestId.Value, new SetMixTableCompletion.PayloadData(errorCode, commandException.Message));

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
