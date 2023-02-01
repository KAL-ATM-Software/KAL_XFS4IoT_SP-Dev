/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * PresentMediaHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(PresentMediaCommand))]
    public partial class PresentMediaHandler : ICommandHandler
    {
        public PresentMediaHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PresentMediaHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(PresentMediaHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(PresentMediaHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(PresentMediaHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var presentMediaCmd = command.IsA<PresentMediaCommand>($"Invalid parameter in the PresentMedia Handle method. {nameof(PresentMediaCommand)}");
            presentMediaCmd.Header.RequestId.HasValue.IsTrue();

            IPresentMediaEvents events = new PresentMediaEvents(Connection, presentMediaCmd.Header.RequestId.Value);

            var result = await HandlePresentMedia(events, presentMediaCmd, cancel);
            await Connection.SendMessageAsync(new PresentMediaCompletion(presentMediaCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var presentMediacommand = command.IsA<PresentMediaCommand>();
            presentMediacommand.Header.RequestId.HasValue.IsTrue();

            PresentMediaCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => PresentMediaCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => PresentMediaCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => PresentMediaCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => PresentMediaCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => PresentMediaCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => PresentMediaCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => PresentMediaCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => PresentMediaCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => PresentMediaCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => PresentMediaCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => PresentMediaCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => PresentMediaCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => PresentMediaCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => PresentMediaCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => PresentMediaCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new PresentMediaCompletion(presentMediacommand.Header.RequestId.Value, new PresentMediaCompletion.PayloadData(errorCode, commandException.Message));

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
