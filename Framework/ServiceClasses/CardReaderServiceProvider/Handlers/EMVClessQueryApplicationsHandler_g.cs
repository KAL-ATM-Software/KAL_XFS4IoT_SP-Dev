/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessQueryApplicationsHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(EMVClessQueryApplicationsCommand))]
    public partial class EMVClessQueryApplicationsHandler : ICommandHandler
    {
        public EMVClessQueryApplicationsHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EMVClessQueryApplicationsHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(EMVClessQueryApplicationsHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(EMVClessQueryApplicationsHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var eMVClessQueryApplicationsCmd = command.IsA<EMVClessQueryApplicationsCommand>($"Invalid parameter in the EMVClessQueryApplications Handle method. {nameof(EMVClessQueryApplicationsCommand)}");
            eMVClessQueryApplicationsCmd.Header.RequestId.HasValue.IsTrue();

            IEMVClessQueryApplicationsEvents events = new EMVClessQueryApplicationsEvents(Connection, eMVClessQueryApplicationsCmd.Header.RequestId.Value);

            var result = await HandleEMVClessQueryApplications(events, eMVClessQueryApplicationsCmd, cancel);
            await Connection.SendMessageAsync(new EMVClessQueryApplicationsCompletion(eMVClessQueryApplicationsCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var eMVClessQueryApplicationscommand = command.IsA<EMVClessQueryApplicationsCommand>();
            eMVClessQueryApplicationscommand.Header.RequestId.HasValue.IsTrue();

            EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new EMVClessQueryApplicationsCompletion(eMVClessQueryApplicationscommand.Header.RequestId.Value, new EMVClessQueryApplicationsCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICardReaderDevice Device { get => Provider.Device.IsA<ICardReaderDevice>(); }
        private IServiceProvider Provider { get; }
        private ICardReaderServiceClass CardReader { get; }
        private ILogger Logger { get; }
    }

}
