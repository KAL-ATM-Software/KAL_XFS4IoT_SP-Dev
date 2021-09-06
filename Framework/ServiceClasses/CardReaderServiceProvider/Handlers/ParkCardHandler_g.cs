/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ParkCardHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(ParkCardCommand))]
    public partial class ParkCardHandler : ICommandHandler
    {
        public ParkCardHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ParkCardHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ParkCardHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ParkCardHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var parkCardCmd = command.IsA<ParkCardCommand>($"Invalid parameter in the ParkCard Handle method. {nameof(ParkCardCommand)}");
            parkCardCmd.Header.RequestId.HasValue.IsTrue();

            IParkCardEvents events = new ParkCardEvents(Connection, parkCardCmd.Header.RequestId.Value);

            var result = await HandleParkCard(events, parkCardCmd, cancel);
            await Connection.SendMessageAsync(new ParkCardCompletion(parkCardCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var parkCardcommand = command.IsA<ParkCardCommand>();
            parkCardcommand.Header.RequestId.HasValue.IsTrue();

            ParkCardCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ParkCardCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => ParkCardCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => ParkCardCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => ParkCardCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ParkCardCompletion(parkCardcommand.Header.RequestId.Value, new ParkCardCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICardReaderDevice Device { get => Provider.Device.IsA<ICardReaderDevice>(); }
        private IServiceProvider Provider { get; }
        private ICardReaderServiceClass CardReader { get; }
        private ILogger Logger { get; }
    }

}
