/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Printer
{
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(GetCodelineMappingCommand))]
    public partial class GetCodelineMappingHandler : ICommandHandler
    {
        public GetCodelineMappingHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetCodelineMappingHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetCodelineMappingHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetCodelineMappingHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var getCodelineMappingCmd = command.IsA<GetCodelineMappingCommand>($"Invalid parameter in the GetCodelineMapping Handle method. {nameof(GetCodelineMappingCommand)}");
            getCodelineMappingCmd.Header.RequestId.HasValue.IsTrue();

            IGetCodelineMappingEvents events = new GetCodelineMappingEvents(Connection, getCodelineMappingCmd.Header.RequestId.Value);

            var result = await HandleGetCodelineMapping(events, getCodelineMappingCmd, cancel);
            await Connection.SendMessageAsync(new GetCodelineMappingCompletion(getCodelineMappingCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var getCodelineMappingcommand = command.IsA<GetCodelineMappingCommand>();
            getCodelineMappingcommand.Header.RequestId.HasValue.IsTrue();

            GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => GetCodelineMappingCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetCodelineMappingCompletion(getCodelineMappingcommand.Header.RequestId.Value, new GetCodelineMappingCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPrinterDevice Device { get => Provider.Device.IsA<IPrinterDevice>(); }
        private IServiceProvider Provider { get; }
        private IPrinterServiceClass Printer { get; }
        private ILogger Logger { get; }
    }

}
