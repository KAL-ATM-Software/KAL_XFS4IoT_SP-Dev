/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * MaintainPinHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.PinPad.Commands;
using XFS4IoT.PinPad.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.PinPad
{
    [CommandHandler(XFSConstants.ServiceClass.PinPad, typeof(MaintainPinCommand))]
    public partial class MaintainPinHandler : ICommandHandler
    {
        public MaintainPinHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(MaintainPinHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(MaintainPinHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPinPadDevice>();

            PinPad = Provider.IsA<IPinPadServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(MaintainPinHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var maintainPinCmd = command.IsA<MaintainPinCommand>($"Invalid parameter in the MaintainPin Handle method. {nameof(MaintainPinCommand)}");
            maintainPinCmd.Header.RequestId.HasValue.IsTrue();

            IMaintainPinEvents events = new MaintainPinEvents(Connection, maintainPinCmd.Header.RequestId.Value);

            var result = await HandleMaintainPin(events, maintainPinCmd, cancel);
            await Connection.SendMessageAsync(new MaintainPinCompletion(maintainPinCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var maintainPincommand = command.IsA<MaintainPinCommand>();
            maintainPincommand.Header.RequestId.HasValue.IsTrue();

            MaintainPinCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => MaintainPinCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new MaintainPinCompletion(maintainPincommand.Header.RequestId.Value, new MaintainPinCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPinPadDevice Device { get => Provider.Device.IsA<IPinPadDevice>(); }
        private IServiceProvider Provider { get; }
        private IPinPadServiceClass PinPad { get; }
        private ILogger Logger { get; }
    }

}
