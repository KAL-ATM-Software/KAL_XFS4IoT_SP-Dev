/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * PowerSaveControlHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Common, typeof(PowerSaveControlCommand))]
    public partial class PowerSaveControlHandler : ICommandHandler
    {
        public PowerSaveControlHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PowerSaveControlHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(PowerSaveControlHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICommonDevice>();

            Common = Provider.IsA<ICommonServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(PowerSaveControlHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var powerSaveControlCmd = command.IsA<PowerSaveControlCommand>($"Invalid parameter in the PowerSaveControl Handle method. {nameof(PowerSaveControlCommand)}");
            powerSaveControlCmd.Header.RequestId.HasValue.IsTrue();

            IPowerSaveControlEvents events = new PowerSaveControlEvents(Connection, powerSaveControlCmd.Header.RequestId.Value);

            var result = await HandlePowerSaveControl(events, powerSaveControlCmd, cancel);
            await Connection.SendMessageAsync(new PowerSaveControlCompletion(powerSaveControlCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var powerSaveControlcommand = command.IsA<PowerSaveControlCommand>();
            powerSaveControlcommand.Header.RequestId.HasValue.IsTrue();

            PowerSaveControlCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new PowerSaveControlCompletion(powerSaveControlcommand.Header.RequestId.Value, new PowerSaveControlCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICommonDevice Device { get => Provider.Device.IsA<ICommonDevice>(); }
        private IServiceProvider Provider { get; }
        private ICommonServiceClass Common { get; }
        private ILogger Logger { get; }
    }

}
