/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * OpenShutterHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Dispenser.Commands;
using XFS4IoT.Dispenser.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Dispenser
{
    [CommandHandler(XFSConstants.ServiceClass.Dispenser, typeof(OpenShutterCommand))]
    public partial class OpenShutterHandler : ICommandHandler
    {
        public OpenShutterHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(OpenShutterHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(OpenShutterHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IDispenserDevice>();

            Dispenser = Provider.IsA<IDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(OpenShutterHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var openShutterCmd = command.IsA<OpenShutterCommand>($"Invalid parameter in the OpenShutter Handle method. {nameof(OpenShutterCommand)}");
            openShutterCmd.Headers.RequestId.HasValue.IsTrue();

            IOpenShutterEvents events = new OpenShutterEvents(Connection, openShutterCmd.Headers.RequestId.Value);

            var result = await HandleOpenShutter(events, openShutterCmd, cancel);
            await Connection.SendMessageAsync(new OpenShutterCompletion(openShutterCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var openShuttercommand = command.IsA<OpenShutterCommand>();
            openShuttercommand.Headers.RequestId.HasValue.IsTrue();

            OpenShutterCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => OpenShutterCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => OpenShutterCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => OpenShutterCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new OpenShutterCompletion(openShuttercommand.Headers.RequestId.Value, new OpenShutterCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IDispenserDevice Device { get => Provider.Device.IsA<IDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private IDispenserServiceClass Dispenser { get; }
        private ILogger Logger { get; }
    }

}
