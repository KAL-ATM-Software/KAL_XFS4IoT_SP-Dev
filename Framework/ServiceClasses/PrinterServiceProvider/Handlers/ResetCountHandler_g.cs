/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ResetCountHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Printer
{
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(ResetCountCommand))]
    public partial class ResetCountHandler : ICommandHandler
    {
        public ResetCountHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ResetCountHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ResetCountHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ResetCountHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ResetCountHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var resetCountCmd = command.IsA<ResetCountCommand>($"Invalid parameter in the ResetCount Handle method. {nameof(ResetCountCommand)}");
            resetCountCmd.Header.RequestId.HasValue.IsTrue();

            IResetCountEvents events = new ResetCountEvents(Connection, resetCountCmd.Header.RequestId.Value);

            var result = await HandleResetCount(events, resetCountCmd, cancel);
            await Connection.SendMessageAsync(new ResetCountCompletion(resetCountCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var resetCountcommand = command.IsA<ResetCountCommand>();
            resetCountcommand.Header.RequestId.HasValue.IsTrue();

            ResetCountCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ResetCountCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ResetCountCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ResetCountCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ResetCountCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ResetCountCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ResetCountCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ResetCountCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ResetCountCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ResetCountCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ResetCountCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ResetCountCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ResetCountCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ResetCountCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ResetCountCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ResetCountCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ResetCountCompletion(resetCountcommand.Header.RequestId.Value, new ResetCountCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IPrinterDevice Device { get => Provider.Device.IsA<IPrinterDevice>(); }
        private IServiceProvider Provider { get; }
        private IPrinterService Printer { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
