/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * PrepareDispenseHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashDispenser
{
    [CommandHandler(XFSConstants.ServiceClass.CashDispenser, typeof(PrepareDispenseCommand))]
    public partial class PrepareDispenseHandler : ICommandHandler
    {
        public PrepareDispenseHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PrepareDispenseHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(PrepareDispenseHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashDispenserDevice>();

            CashDispenser = Provider.IsA<ICashDispenserService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(PrepareDispenseHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(PrepareDispenseHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var prepareDispenseCmd = command.IsA<PrepareDispenseCommand>($"Invalid parameter in the PrepareDispense Handle method. {nameof(PrepareDispenseCommand)}");
            prepareDispenseCmd.Header.RequestId.HasValue.IsTrue();

            IPrepareDispenseEvents events = new PrepareDispenseEvents(Connection, prepareDispenseCmd.Header.RequestId.Value);

            var result = await HandlePrepareDispense(events, prepareDispenseCmd, cancel);
            await Connection.SendMessageAsync(new PrepareDispenseCompletion(prepareDispenseCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var prepareDispensecommand = command.IsA<PrepareDispenseCommand>();
            prepareDispensecommand.Header.RequestId.HasValue.IsTrue();

            PrepareDispenseCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => PrepareDispenseCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => PrepareDispenseCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => PrepareDispenseCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => PrepareDispenseCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => PrepareDispenseCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new PrepareDispenseCompletion(prepareDispensecommand.Header.RequestId.Value, new PrepareDispenseCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICashDispenserDevice Device { get => Provider.Device.IsA<ICashDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashDispenserService CashDispenser { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
