/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * GetPinBlockHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.PinPad, typeof(GetPinBlockCommand))]
    public partial class GetPinBlockHandler : ICommandHandler
    {
        public GetPinBlockHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetPinBlockHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetPinBlockHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPinPadDevice>();

            PinPad = Provider.IsA<IPinPadServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetPinBlockHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var getPinBlockCmd = command.IsA<GetPinBlockCommand>($"Invalid parameter in the GetPinBlock Handle method. {nameof(GetPinBlockCommand)}");
            getPinBlockCmd.Header.RequestId.HasValue.IsTrue();

            IGetPinBlockEvents events = new GetPinBlockEvents(Connection, getPinBlockCmd.Header.RequestId.Value);

            var result = await HandleGetPinBlock(events, getPinBlockCmd, cancel);
            await Connection.SendMessageAsync(new GetPinBlockCompletion(getPinBlockCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var getPinBlockcommand = command.IsA<GetPinBlockCommand>();
            getPinBlockcommand.Header.RequestId.HasValue.IsTrue();

            GetPinBlockCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetPinBlockCompletion(getPinBlockcommand.Header.RequestId.Value, new GetPinBlockCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPinPadDevice Device { get => Provider.Device.IsA<IPinPadDevice>(); }
        private IServiceProvider Provider { get; }
        private IPinPadServiceClass PinPad { get; }
        private ILogger Logger { get; }
    }

}
