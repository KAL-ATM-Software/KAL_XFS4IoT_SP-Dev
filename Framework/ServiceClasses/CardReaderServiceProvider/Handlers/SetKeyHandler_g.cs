/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CardReader
{
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(SetKeyCommand))]
    public partial class SetKeyHandler : ICommandHandler
    {
        public SetKeyHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetKeyHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetKeyHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetKeyHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var setKeyCmd = command.IsA<SetKeyCommand>($"Invalid parameter in the SetKey Handle method. {nameof(SetKeyCommand)}");
            setKeyCmd.Header.RequestId.HasValue.IsTrue();

            ISetKeyEvents events = new SetKeyEvents(Connection, setKeyCmd.Header.RequestId.Value);

            var result = await HandleSetKey(events, setKeyCmd, cancel);
            await Connection.SendMessageAsync(new SetKeyCompletion(setKeyCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var setKeycommand = command.IsA<SetKeyCommand>();
            setKeycommand.Header.RequestId.HasValue.IsTrue();

            SetKeyCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetKeyCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => SetKeyCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => SetKeyCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => SetKeyCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetKeyCompletion(setKeycommand.Header.RequestId.Value, new SetKeyCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICardReaderDevice Device { get => Provider.Device.IsA<ICardReaderDevice>(); }
        private IServiceProvider Provider { get; }
        private ICardReaderServiceClass CardReader { get; }
        private ILogger Logger { get; }
    }

}
