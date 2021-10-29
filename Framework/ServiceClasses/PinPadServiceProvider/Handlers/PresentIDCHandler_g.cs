/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * PresentIDCHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.PinPad, typeof(PresentIDCCommand))]
    public partial class PresentIDCHandler : ICommandHandler
    {
        public PresentIDCHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PresentIDCHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(PresentIDCHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPinPadDevice>();

            PinPad = Provider.IsA<IPinPadServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(PresentIDCHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var presentIDCCmd = command.IsA<PresentIDCCommand>($"Invalid parameter in the PresentIDC Handle method. {nameof(PresentIDCCommand)}");
            presentIDCCmd.Header.RequestId.HasValue.IsTrue();

            IPresentIDCEvents events = new PresentIDCEvents(Connection, presentIDCCmd.Header.RequestId.Value);

            var result = await HandlePresentIDC(events, presentIDCCmd, cancel);
            await Connection.SendMessageAsync(new PresentIDCCompletion(presentIDCCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var presentIDCcommand = command.IsA<PresentIDCCommand>();
            presentIDCcommand.Header.RequestId.HasValue.IsTrue();

            PresentIDCCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => PresentIDCCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => PresentIDCCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new PresentIDCCompletion(presentIDCcommand.Header.RequestId.Value, new PresentIDCCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPinPadDevice Device { get => Provider.Device.IsA<IPinPadDevice>(); }
        private IServiceProvider Provider { get; }
        private IPinPadServiceClass PinPad { get; }
        private ILogger Logger { get; }
    }

}
