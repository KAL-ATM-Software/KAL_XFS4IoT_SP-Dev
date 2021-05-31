/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EjectCardHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(EjectCardCommand))]
    public partial class EjectCardHandler : ICommandHandler
    {
        public EjectCardHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EjectCardHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(EjectCardHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(EjectCardHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var ejectCardCmd = command.IsA<EjectCardCommand>($"Invalid parameter in the EjectCard Handle method. {nameof(EjectCardCommand)}");
            ejectCardCmd.Headers.RequestId.HasValue.IsTrue();

            IEjectCardEvents events = new EjectCardEvents(Connection, ejectCardCmd.Headers.RequestId.Value);

            var result = await HandleEjectCard(events, ejectCardCmd, cancel);
            await Connection.SendMessageAsync(new EjectCardCompletion(ejectCardCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var ejectCardcommand = command.IsA<EjectCardCommand>();
            ejectCardcommand.Headers.RequestId.HasValue.IsTrue();

            EjectCardCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => EjectCardCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => EjectCardCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => EjectCardCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new EjectCardCompletion(ejectCardcommand.Headers.RequestId.Value, new EjectCardCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICardReaderDevice Device { get => Provider.Device.IsA<ICardReaderDevice>(); }
        private IServiceProvider Provider { get; }
        private ICardReaderServiceClass CardReader { get; }
        private ILogger Logger { get; }
    }

}
