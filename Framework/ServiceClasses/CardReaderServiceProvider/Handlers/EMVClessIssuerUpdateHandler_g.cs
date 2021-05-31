/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessIssuerUpdateHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CardReader
{
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(EMVClessIssuerUpdateCommand))]
    public partial class EMVClessIssuerUpdateHandler : ICommandHandler
    {
        public EMVClessIssuerUpdateHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EMVClessIssuerUpdateHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(EMVClessIssuerUpdateHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(EMVClessIssuerUpdateHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var eMVClessIssuerUpdateCmd = command.IsA<EMVClessIssuerUpdateCommand>($"Invalid parameter in the EMVClessIssuerUpdate Handle method. {nameof(EMVClessIssuerUpdateCommand)}");
            eMVClessIssuerUpdateCmd.Headers.RequestId.HasValue.IsTrue();

            IEMVClessIssuerUpdateEvents events = new EMVClessIssuerUpdateEvents(Connection, eMVClessIssuerUpdateCmd.Headers.RequestId.Value);

            var result = await HandleEMVClessIssuerUpdate(events, eMVClessIssuerUpdateCmd, cancel);
            await Connection.SendMessageAsync(new EMVClessIssuerUpdateCompletion(eMVClessIssuerUpdateCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var eMVClessIssuerUpdatecommand = command.IsA<EMVClessIssuerUpdateCommand>();
            eMVClessIssuerUpdatecommand.Headers.RequestId.HasValue.IsTrue();

            EMVClessIssuerUpdateCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => EMVClessIssuerUpdateCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => EMVClessIssuerUpdateCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => EMVClessIssuerUpdateCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new EMVClessIssuerUpdateCompletion(eMVClessIssuerUpdatecommand.Headers.RequestId.Value, new EMVClessIssuerUpdateCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICardReaderDevice Device { get => Provider.Device.IsA<ICardReaderDevice>(); }
        private IServiceProvider Provider { get; }
        private ICardReaderServiceClass CardReader { get; }
        private ILogger Logger { get; }
    }

}
