/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * GetMixTableHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashDispenser, typeof(GetMixTableCommand))]
    public partial class GetMixTableHandler : ICommandHandler
    {
        public GetMixTableHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetMixTableHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetMixTableHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashDispenserDevice>();

            CashDispenser = Provider.IsA<ICashDispenserService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetMixTableHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetMixTableHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getMixTableCmd = command.IsA<GetMixTableCommand>($"Invalid parameter in the GetMixTable Handle method. {nameof(GetMixTableCommand)}");
            getMixTableCmd.Header.RequestId.HasValue.IsTrue();

            IGetMixTableEvents events = new GetMixTableEvents(Connection, getMixTableCmd.Header.RequestId.Value);

            var result = await HandleGetMixTable(events, getMixTableCmd, cancel);
            await Connection.SendMessageAsync(new GetMixTableCompletion(getMixTableCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getMixTablecommand = command.IsA<GetMixTableCommand>();
            getMixTablecommand.Header.RequestId.HasValue.IsTrue();

            GetMixTableCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetMixTableCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetMixTableCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetMixTableCompletion(getMixTablecommand.Header.RequestId.Value, new GetMixTableCompletion.PayloadData(errorCode, commandException.Message));

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
