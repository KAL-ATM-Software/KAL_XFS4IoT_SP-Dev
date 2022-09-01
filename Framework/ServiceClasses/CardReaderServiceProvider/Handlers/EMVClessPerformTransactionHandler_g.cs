/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessPerformTransactionHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(EMVClessPerformTransactionCommand))]
    public partial class EMVClessPerformTransactionHandler : ICommandHandler
    {
        public EMVClessPerformTransactionHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EMVClessPerformTransactionHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(EMVClessPerformTransactionHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(EMVClessPerformTransactionHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(EMVClessPerformTransactionHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var eMVClessPerformTransactionCmd = command.IsA<EMVClessPerformTransactionCommand>($"Invalid parameter in the EMVClessPerformTransaction Handle method. {nameof(EMVClessPerformTransactionCommand)}");
            eMVClessPerformTransactionCmd.Header.RequestId.HasValue.IsTrue();

            IEMVClessPerformTransactionEvents events = new EMVClessPerformTransactionEvents(Connection, eMVClessPerformTransactionCmd.Header.RequestId.Value);

            var result = await HandleEMVClessPerformTransaction(events, eMVClessPerformTransactionCmd, cancel);
            await Connection.SendMessageAsync(new EMVClessPerformTransactionCompletion(eMVClessPerformTransactionCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var eMVClessPerformTransactioncommand = command.IsA<EMVClessPerformTransactionCommand>();
            eMVClessPerformTransactioncommand.Header.RequestId.HasValue.IsTrue();

            EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => EMVClessPerformTransactionCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new EMVClessPerformTransactionCompletion(eMVClessPerformTransactioncommand.Header.RequestId.Value, new EMVClessPerformTransactionCompletion.PayloadData(errorCode, commandException.Message));

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
