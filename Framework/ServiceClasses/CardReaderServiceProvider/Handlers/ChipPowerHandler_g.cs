/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ChipPowerHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(ChipPowerCommand))]
    public partial class ChipPowerHandler : ICommandHandler
    {
        public ChipPowerHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ChipPowerHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ChipPowerHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ChipPowerHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ChipPowerHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var chipPowerCmd = command.IsA<ChipPowerCommand>($"Invalid parameter in the ChipPower Handle method. {nameof(ChipPowerCommand)}");
            chipPowerCmd.Header.RequestId.HasValue.IsTrue();

            IChipPowerEvents events = new ChipPowerEvents(Connection, chipPowerCmd.Header.RequestId.Value);

            var result = await HandleChipPower(events, chipPowerCmd, cancel);
            await Connection.SendMessageAsync(new ChipPowerCompletion(chipPowerCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var chipPowercommand = command.IsA<ChipPowerCommand>();
            chipPowercommand.Header.RequestId.HasValue.IsTrue();

            ChipPowerCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ChipPowerCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ChipPowerCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ChipPowerCompletion(chipPowercommand.Header.RequestId.Value, new ChipPowerCompletion.PayloadData(errorCode, commandException.Message));

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
