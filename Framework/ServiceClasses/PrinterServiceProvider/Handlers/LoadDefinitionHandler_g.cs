/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * LoadDefinitionHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(LoadDefinitionCommand))]
    public partial class LoadDefinitionHandler : ICommandHandler
    {
        public LoadDefinitionHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(LoadDefinitionHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(LoadDefinitionHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(LoadDefinitionHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var loadDefinitionCmd = command.IsA<LoadDefinitionCommand>($"Invalid parameter in the LoadDefinition Handle method. {nameof(LoadDefinitionCommand)}");
            loadDefinitionCmd.Header.RequestId.HasValue.IsTrue();

            ILoadDefinitionEvents events = new LoadDefinitionEvents(Connection, loadDefinitionCmd.Header.RequestId.Value);

            var result = await HandleLoadDefinition(events, loadDefinitionCmd, cancel);
            await Connection.SendMessageAsync(new LoadDefinitionCompletion(loadDefinitionCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var loadDefinitioncommand = command.IsA<LoadDefinitionCommand>();
            loadDefinitioncommand.Header.RequestId.HasValue.IsTrue();

            LoadDefinitionCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new LoadDefinitionCompletion(loadDefinitioncommand.Header.RequestId.Value, new LoadDefinitionCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPrinterDevice Device { get => Provider.Device.IsA<IPrinterDevice>(); }
        private IServiceProvider Provider { get; }
        private IPrinterServiceClass Printer { get; }
        private ILogger Logger { get; }
    }

}
