/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT BarcodeReader interface.
 * ReadHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.BarcodeReader.Commands;
using XFS4IoT.BarcodeReader.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.BarcodeReader
{
    [CommandHandler(XFSConstants.ServiceClass.BarcodeReader, typeof(ReadCommand))]
    public partial class ReadHandler : ICommandHandler
    {
        public ReadHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReadHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ReadHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IBarcodeReaderDevice>();

            BarcodeReader = Provider.IsA<IBarcodeReaderService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ReadHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ReadHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var readCmd = command.IsA<ReadCommand>($"Invalid parameter in the Read Handle method. {nameof(ReadCommand)}");
            readCmd.Header.RequestId.HasValue.IsTrue();

            IReadEvents events = new ReadEvents(Connection, readCmd.Header.RequestId.Value);

            var result = await HandleRead(events, readCmd, cancel);
            await Connection.SendMessageAsync(new ReadCompletion(readCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var readcommand = command.IsA<ReadCommand>();
            readcommand.Header.RequestId.HasValue.IsTrue();

            ReadCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ReadCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => ReadCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ReadCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ReadCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ReadCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ReadCompletion(readcommand.Header.RequestId.Value, new ReadCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IBarcodeReaderDevice Device { get => Provider.Device.IsA<IBarcodeReaderDevice>(); }
        private IServiceProvider Provider { get; }
        private IBarcodeReaderService BarcodeReader { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
