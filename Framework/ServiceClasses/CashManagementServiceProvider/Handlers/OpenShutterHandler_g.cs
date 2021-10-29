/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * OpenShutterHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashManagement
{
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(OpenShutterCommand))]
    public partial class OpenShutterHandler : ICommandHandler
    {
        public OpenShutterHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(OpenShutterHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(OpenShutterHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(OpenShutterHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var openShutterCmd = command.IsA<OpenShutterCommand>($"Invalid parameter in the OpenShutter Handle method. {nameof(OpenShutterCommand)}");
            openShutterCmd.Header.RequestId.HasValue.IsTrue();

            IOpenShutterEvents events = new OpenShutterEvents(Connection, openShutterCmd.Header.RequestId.Value);

            var result = await HandleOpenShutter(events, openShutterCmd, cancel);
            await Connection.SendMessageAsync(new OpenShutterCompletion(openShutterCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var openShuttercommand = command.IsA<OpenShutterCommand>();
            openShuttercommand.Header.RequestId.HasValue.IsTrue();

            OpenShutterCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => OpenShutterCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => OpenShutterCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => OpenShutterCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => OpenShutterCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => OpenShutterCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new OpenShutterCompletion(openShuttercommand.Header.RequestId.Value, new OpenShutterCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICashManagementDevice Device { get => Provider.Device.IsA<ICashManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashManagementServiceClass CashManagement { get; }
        private ILogger Logger { get; }
    }

}
