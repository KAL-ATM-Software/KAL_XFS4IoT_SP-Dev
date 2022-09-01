/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetPositionCapabilitiesHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashAcceptor
{
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(GetPositionCapabilitiesCommand))]
    public partial class GetPositionCapabilitiesHandler : ICommandHandler
    {
        public GetPositionCapabilitiesHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetPositionCapabilitiesHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetPositionCapabilitiesHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetPositionCapabilitiesHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetPositionCapabilitiesHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getPositionCapabilitiesCmd = command.IsA<GetPositionCapabilitiesCommand>($"Invalid parameter in the GetPositionCapabilities Handle method. {nameof(GetPositionCapabilitiesCommand)}");
            getPositionCapabilitiesCmd.Header.RequestId.HasValue.IsTrue();

            IGetPositionCapabilitiesEvents events = new GetPositionCapabilitiesEvents(Connection, getPositionCapabilitiesCmd.Header.RequestId.Value);

            var result = await HandleGetPositionCapabilities(events, getPositionCapabilitiesCmd, cancel);
            await Connection.SendMessageAsync(new GetPositionCapabilitiesCompletion(getPositionCapabilitiesCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getPositionCapabilitiescommand = command.IsA<GetPositionCapabilitiesCommand>();
            getPositionCapabilitiescommand.Header.RequestId.HasValue.IsTrue();

            GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetPositionCapabilitiesCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetPositionCapabilitiesCompletion(getPositionCapabilitiescommand.Header.RequestId.Value, new GetPositionCapabilitiesCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICashAcceptorDevice Device { get => Provider.Device.IsA<ICashAcceptorDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashAcceptorService CashAcceptor { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
