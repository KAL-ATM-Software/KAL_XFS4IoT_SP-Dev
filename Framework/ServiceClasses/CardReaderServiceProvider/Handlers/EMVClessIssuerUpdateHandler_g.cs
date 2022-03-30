/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoTFramework.Common;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CardReader
{
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(EMVClessIssuerUpdateCommand))]
    public partial class EMVClessIssuerUpdateHandler : ICommandHandler
    {
        public EMVClessIssuerUpdateHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EMVClessIssuerUpdateHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(EMVClessIssuerUpdateHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(EMVClessIssuerUpdateHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(EMVClessIssuerUpdateHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var eMVClessIssuerUpdateCmd = command.IsA<EMVClessIssuerUpdateCommand>($"Invalid parameter in the EMVClessIssuerUpdate Handle method. {nameof(EMVClessIssuerUpdateCommand)}");
            eMVClessIssuerUpdateCmd.Header.RequestId.HasValue.IsTrue();

            IEMVClessIssuerUpdateEvents events = new EMVClessIssuerUpdateEvents(Connection, eMVClessIssuerUpdateCmd.Header.RequestId.Value);

            var result = await HandleEMVClessIssuerUpdate(events, eMVClessIssuerUpdateCmd, cancel);
            await Connection.SendMessageAsync(new EMVClessIssuerUpdateCompletion(eMVClessIssuerUpdateCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var eMVClessIssuerUpdatecommand = command.IsA<EMVClessIssuerUpdateCommand>();
            eMVClessIssuerUpdatecommand.Header.RequestId.HasValue.IsTrue();

            EMVClessIssuerUpdateCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => EMVClessIssuerUpdateCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => EMVClessIssuerUpdateCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => EMVClessIssuerUpdateCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => EMVClessIssuerUpdateCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => EMVClessIssuerUpdateCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new EMVClessIssuerUpdateCompletion(eMVClessIssuerUpdatecommand.Header.RequestId.Value, new EMVClessIssuerUpdateCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICardReaderDevice Device { get => Provider.Device.IsA<ICardReaderDevice>(); }
        private IServiceProvider Provider { get; }
        private ICardReaderService CardReader { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
