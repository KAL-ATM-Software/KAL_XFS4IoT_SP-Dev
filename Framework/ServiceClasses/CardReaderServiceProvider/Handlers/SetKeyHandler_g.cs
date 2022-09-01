/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * SetKeyHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(SetKeyCommand))]
    public partial class SetKeyHandler : ICommandHandler
    {
        public SetKeyHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetKeyHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetKeyHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetKeyHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SetKeyHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var setKeyCmd = command.IsA<SetKeyCommand>($"Invalid parameter in the SetKey Handle method. {nameof(SetKeyCommand)}");
            setKeyCmd.Header.RequestId.HasValue.IsTrue();

            ISetKeyEvents events = new SetKeyEvents(Connection, setKeyCmd.Header.RequestId.Value);

            var result = await HandleSetKey(events, setKeyCmd, cancel);
            await Connection.SendMessageAsync(new SetKeyCompletion(setKeyCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setKeycommand = command.IsA<SetKeyCommand>();
            setKeycommand.Header.RequestId.HasValue.IsTrue();

            SetKeyCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetKeyCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => SetKeyCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => SetKeyCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => SetKeyCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => SetKeyCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => SetKeyCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => SetKeyCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => SetKeyCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => SetKeyCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => SetKeyCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => SetKeyCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => SetKeyCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetKeyCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetKeyCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetKeyCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetKeyCompletion(setKeycommand.Header.RequestId.Value, new SetKeyCompletion.PayloadData(errorCode, commandException.Message));

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
