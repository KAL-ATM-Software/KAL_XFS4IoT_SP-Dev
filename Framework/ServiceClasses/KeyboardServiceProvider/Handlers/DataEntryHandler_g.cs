/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * DataEntryHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Keyboard.Commands;
using XFS4IoT.Keyboard.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Keyboard
{
    [CommandHandler(XFSConstants.ServiceClass.Keyboard, typeof(DataEntryCommand))]
    public partial class DataEntryHandler : ICommandHandler
    {
        public DataEntryHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DataEntryHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(DataEntryHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyboardDevice>();

            Keyboard = Provider.IsA<IKeyboardServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(DataEntryHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var dataEntryCmd = command.IsA<DataEntryCommand>($"Invalid parameter in the DataEntry Handle method. {nameof(DataEntryCommand)}");
            dataEntryCmd.Header.RequestId.HasValue.IsTrue();

            IDataEntryEvents events = new DataEntryEvents(Connection, dataEntryCmd.Header.RequestId.Value);

            var result = await HandleDataEntry(events, dataEntryCmd, cancel);
            await Connection.SendMessageAsync(new DataEntryCompletion(dataEntryCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var dataEntrycommand = command.IsA<DataEntryCommand>();
            dataEntrycommand.Header.RequestId.HasValue.IsTrue();

            DataEntryCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => DataEntryCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => DataEntryCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => DataEntryCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => DataEntryCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new DataEntryCompletion(dataEntrycommand.Header.RequestId.Value, new DataEntryCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IKeyboardDevice Device { get => Provider.Device.IsA<IKeyboardDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyboardServiceClass Keyboard { get; }
        private ILogger Logger { get; }
    }

}
