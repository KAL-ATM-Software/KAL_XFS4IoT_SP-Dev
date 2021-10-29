/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * SetPinBlockDataHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.PinPad, typeof(SetPinBlockDataCommand))]
    public partial class SetPinBlockDataHandler : ICommandHandler
    {
        public SetPinBlockDataHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetPinBlockDataHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetPinBlockDataHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPinPadDevice>();

            PinPad = Provider.IsA<IPinPadServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetPinBlockDataHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var setPinBlockDataCmd = command.IsA<SetPinBlockDataCommand>($"Invalid parameter in the SetPinBlockData Handle method. {nameof(SetPinBlockDataCommand)}");
            setPinBlockDataCmd.Header.RequestId.HasValue.IsTrue();

            ISetPinBlockDataEvents events = new SetPinBlockDataEvents(Connection, setPinBlockDataCmd.Header.RequestId.Value);

            var result = await HandleSetPinBlockData(events, setPinBlockDataCmd, cancel);
            await Connection.SendMessageAsync(new SetPinBlockDataCompletion(setPinBlockDataCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var setPinBlockDatacommand = command.IsA<SetPinBlockDataCommand>();
            setPinBlockDatacommand.Header.RequestId.HasValue.IsTrue();

            SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetPinBlockDataCompletion(setPinBlockDatacommand.Header.RequestId.Value, new SetPinBlockDataCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPinPadDevice Device { get => Provider.Device.IsA<IPinPadDevice>(); }
        private IServiceProvider Provider { get; }
        private IPinPadServiceClass PinPad { get; }
        private ILogger Logger { get; }
    }

}
