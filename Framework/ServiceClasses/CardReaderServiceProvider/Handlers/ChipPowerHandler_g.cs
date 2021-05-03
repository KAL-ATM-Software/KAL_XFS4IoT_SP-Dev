/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CardReader
{
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(ChipPowerCommand))]
    public partial class ChipPowerHandler : ICommandHandler
    {
        public ChipPowerHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ChipPowerHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ChipPowerHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ChipPowerHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var chipPowerCmd = command.IsA<ChipPowerCommand>($"Invalid parameter in the ChipPower Handle method. {nameof(ChipPowerCommand)}");
            
            IChipPowerEvents events = new ChipPowerEvents(Connection, chipPowerCmd.Headers.RequestId);

            var result = await HandleChipPower(events, chipPowerCmd, cancel);
            await Connection.SendMessageAsync(new ChipPowerCompletion(chipPowerCmd.Headers.RequestId, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var chipPowercommand = command.IsA<ChipPowerCommand>();

            ChipPowerCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => ChipPowerCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => ChipPowerCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ChipPowerCompletion(chipPowercommand.Headers.RequestId, new ChipPowerCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICardReaderDevice Device { get => Provider.Device.IsA<ICardReaderDevice>(); }
        private IServiceProvider Provider { get; }
        private ICardReaderServiceClass CardReader { get; }
        private ILogger Logger { get; }
    }

}
