/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * LocalVisaHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.PinPad, typeof(LocalVisaCommand))]
    public partial class LocalVisaHandler : ICommandHandler
    {
        public LocalVisaHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(LocalVisaHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(LocalVisaHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPinPadDevice>();

            PinPad = Provider.IsA<IPinPadServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(LocalVisaHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var localVisaCmd = command.IsA<LocalVisaCommand>($"Invalid parameter in the LocalVisa Handle method. {nameof(LocalVisaCommand)}");
            localVisaCmd.Header.RequestId.HasValue.IsTrue();

            ILocalVisaEvents events = new LocalVisaEvents(Connection, localVisaCmd.Header.RequestId.Value);

            var result = await HandleLocalVisa(events, localVisaCmd, cancel);
            await Connection.SendMessageAsync(new LocalVisaCompletion(localVisaCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var localVisacommand = command.IsA<LocalVisaCommand>();
            localVisacommand.Header.RequestId.HasValue.IsTrue();

            LocalVisaCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => LocalVisaCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => LocalVisaCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => LocalVisaCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => LocalVisaCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new LocalVisaCompletion(localVisacommand.Header.RequestId.Value, new LocalVisaCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPinPadDevice Device { get => Provider.Device.IsA<IPinPadDevice>(); }
        private IServiceProvider Provider { get; }
        private IPinPadServiceClass PinPad { get; }
        private ILogger Logger { get; }
    }

}
