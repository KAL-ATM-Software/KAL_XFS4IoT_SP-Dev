/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * CapabilitiesHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Common
{
    [CommandHandler(XFSConstants.ServiceClass.Common, typeof(CapabilitiesCommand))]
    public partial class CapabilitiesHandler : ICommandHandler
    {
        public CapabilitiesHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CapabilitiesHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(CapabilitiesHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICommonDevice>();

            Common = Provider.IsA<ICommonServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(CapabilitiesHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var capabilitiesCmd = command.IsA<CapabilitiesCommand>($"Invalid parameter in the Capabilities Handle method. {nameof(CapabilitiesCommand)}");
            capabilitiesCmd.Headers.RequestId.HasValue.IsTrue();

            ICapabilitiesEvents events = new CapabilitiesEvents(Connection, capabilitiesCmd.Headers.RequestId.Value);

            var result = await HandleCapabilities(events, capabilitiesCmd, cancel);
            await Connection.SendMessageAsync(new CapabilitiesCompletion(capabilitiesCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var capabilitiescommand = command.IsA<CapabilitiesCommand>();
            capabilitiescommand.Headers.RequestId.HasValue.IsTrue();

            CapabilitiesCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => CapabilitiesCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => CapabilitiesCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => CapabilitiesCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new CapabilitiesCompletion(capabilitiescommand.Headers.RequestId.Value, new CapabilitiesCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICommonDevice Device { get => Provider.Device.IsA<ICommonDevice>(); }
        private IServiceProvider Provider { get; }
        private ICommonServiceClass Common { get; }
        private ILogger Logger { get; }
    }

}
