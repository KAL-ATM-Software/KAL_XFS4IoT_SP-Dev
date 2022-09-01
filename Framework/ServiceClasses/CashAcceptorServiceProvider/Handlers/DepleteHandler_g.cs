/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * DepleteHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(DepleteCommand))]
    public partial class DepleteHandler : ICommandHandler
    {
        public DepleteHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DepleteHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(DepleteHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(DepleteHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(DepleteHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var depleteCmd = command.IsA<DepleteCommand>($"Invalid parameter in the Deplete Handle method. {nameof(DepleteCommand)}");
            depleteCmd.Header.RequestId.HasValue.IsTrue();

            IDepleteEvents events = new DepleteEvents(Connection, depleteCmd.Header.RequestId.Value);

            var result = await HandleDeplete(events, depleteCmd, cancel);
            await Connection.SendMessageAsync(new DepleteCompletion(depleteCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var depletecommand = command.IsA<DepleteCommand>();
            depletecommand.Header.RequestId.HasValue.IsTrue();

            DepleteCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => DepleteCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => DepleteCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => DepleteCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => DepleteCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => DepleteCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => DepleteCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => DepleteCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => DepleteCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => DepleteCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => DepleteCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => DepleteCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => DepleteCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => DepleteCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => DepleteCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => DepleteCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new DepleteCompletion(depletecommand.Header.RequestId.Value, new DepleteCompletion.PayloadData(errorCode, commandException.Message));

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
