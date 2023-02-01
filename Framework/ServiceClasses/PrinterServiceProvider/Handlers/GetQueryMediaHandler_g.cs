/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetQueryMediaHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(GetQueryMediaCommand))]
    public partial class GetQueryMediaHandler : ICommandHandler
    {
        public GetQueryMediaHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetQueryMediaHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetQueryMediaHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetQueryMediaHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetQueryMediaHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getQueryMediaCmd = command.IsA<GetQueryMediaCommand>($"Invalid parameter in the GetQueryMedia Handle method. {nameof(GetQueryMediaCommand)}");
            getQueryMediaCmd.Header.RequestId.HasValue.IsTrue();

            IGetQueryMediaEvents events = new GetQueryMediaEvents(Connection, getQueryMediaCmd.Header.RequestId.Value);

            var result = await HandleGetQueryMedia(events, getQueryMediaCmd, cancel);
            await Connection.SendMessageAsync(new GetQueryMediaCompletion(getQueryMediaCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getQueryMediacommand = command.IsA<GetQueryMediaCommand>();
            getQueryMediacommand.Header.RequestId.HasValue.IsTrue();

            GetQueryMediaCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetQueryMediaCompletion(getQueryMediacommand.Header.RequestId.Value, new GetQueryMediaCompletion.PayloadData(errorCode, commandException.Message));

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
