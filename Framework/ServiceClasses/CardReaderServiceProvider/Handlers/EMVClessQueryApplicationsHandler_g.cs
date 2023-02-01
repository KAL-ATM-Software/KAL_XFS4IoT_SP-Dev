/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
using XFS4IoTFramework.Common;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CardReader
{
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(EMVClessQueryApplicationsCommand))]
    public partial class EMVClessQueryApplicationsHandler : ICommandHandler
    {
        public EMVClessQueryApplicationsHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EMVClessQueryApplicationsHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(EMVClessQueryApplicationsHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(EMVClessQueryApplicationsHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(EMVClessQueryApplicationsHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var eMVClessQueryApplicationsCmd = command.IsA<EMVClessQueryApplicationsCommand>($"Invalid parameter in the EMVClessQueryApplications Handle method. {nameof(EMVClessQueryApplicationsCommand)}");
            eMVClessQueryApplicationsCmd.Header.RequestId.HasValue.IsTrue();

            IEMVClessQueryApplicationsEvents events = new EMVClessQueryApplicationsEvents(Connection, eMVClessQueryApplicationsCmd.Header.RequestId.Value);

            var result = await HandleEMVClessQueryApplications(events, eMVClessQueryApplicationsCmd, cancel);
            await Connection.SendMessageAsync(new EMVClessQueryApplicationsCompletion(eMVClessQueryApplicationsCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var eMVClessQueryApplicationscommand = command.IsA<EMVClessQueryApplicationsCommand>();
            eMVClessQueryApplicationscommand.Header.RequestId.HasValue.IsTrue();

            EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => EMVClessQueryApplicationsCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new EMVClessQueryApplicationsCompletion(eMVClessQueryApplicationscommand.Header.RequestId.Value, new EMVClessQueryApplicationsCompletion.PayloadData(errorCode, commandException.Message));

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
