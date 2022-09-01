/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetCodelineMappingHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(GetCodelineMappingCommand))]
    public partial class GetCodelineMappingHandler : ICommandHandler
    {
        public GetCodelineMappingHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetCodelineMappingHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetCodelineMappingHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetCodelineMappingHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetCodelineMappingHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getCodelineMappingCmd = command.IsA<GetCodelineMappingCommand>($"Invalid parameter in the GetCodelineMapping Handle method. {nameof(GetCodelineMappingCommand)}");
            getCodelineMappingCmd.Header.RequestId.HasValue.IsTrue();

            IGetCodelineMappingEvents events = new GetCodelineMappingEvents(Connection, getCodelineMappingCmd.Header.RequestId.Value);

            var result = await HandleGetCodelineMapping(events, getCodelineMappingCmd, cancel);
            await Connection.SendMessageAsync(new GetCodelineMappingCompletion(getCodelineMappingCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getCodelineMappingcommand = command.IsA<GetCodelineMappingCommand>();
            getCodelineMappingcommand.Header.RequestId.HasValue.IsTrue();

            GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetCodelineMappingCompletion(getCodelineMappingcommand.Header.RequestId.Value, new GetCodelineMappingCompletion.PayloadData(errorCode, commandException.Message));

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
