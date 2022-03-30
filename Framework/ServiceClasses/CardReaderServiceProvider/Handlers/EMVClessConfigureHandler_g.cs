/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessConfigureHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(EMVClessConfigureCommand))]
    public partial class EMVClessConfigureHandler : ICommandHandler
    {
        public EMVClessConfigureHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EMVClessConfigureHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(EMVClessConfigureHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(EMVClessConfigureHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(EMVClessConfigureHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var eMVClessConfigureCmd = command.IsA<EMVClessConfigureCommand>($"Invalid parameter in the EMVClessConfigure Handle method. {nameof(EMVClessConfigureCommand)}");
            eMVClessConfigureCmd.Header.RequestId.HasValue.IsTrue();

            IEMVClessConfigureEvents events = new EMVClessConfigureEvents(Connection, eMVClessConfigureCmd.Header.RequestId.Value);

            var result = await HandleEMVClessConfigure(events, eMVClessConfigureCmd, cancel);
            await Connection.SendMessageAsync(new EMVClessConfigureCompletion(eMVClessConfigureCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var eMVClessConfigurecommand = command.IsA<EMVClessConfigureCommand>();
            eMVClessConfigurecommand.Header.RequestId.HasValue.IsTrue();

            EMVClessConfigureCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => EMVClessConfigureCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => EMVClessConfigureCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => EMVClessConfigureCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => EMVClessConfigureCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => EMVClessConfigureCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new EMVClessConfigureCompletion(eMVClessConfigurecommand.Header.RequestId.Value, new EMVClessConfigureCompletion.PayloadData(errorCode, commandException.Message));

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
