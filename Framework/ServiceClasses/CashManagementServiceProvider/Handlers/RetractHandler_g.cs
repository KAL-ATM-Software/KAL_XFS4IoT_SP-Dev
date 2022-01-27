/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * RetractHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashManagement
{
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(RetractCommand))]
    public partial class RetractHandler : ICommandHandler
    {
        public RetractHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(RetractHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(RetractHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(RetractHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(RetractHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var retractCmd = command.IsA<RetractCommand>($"Invalid parameter in the Retract Handle method. {nameof(RetractCommand)}");
            retractCmd.Header.RequestId.HasValue.IsTrue();

            IRetractEvents events = new RetractEvents(Connection, retractCmd.Header.RequestId.Value);

            var result = await HandleRetract(events, retractCmd, cancel);
            await Connection.SendMessageAsync(new RetractCompletion(retractCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var retractcommand = command.IsA<RetractCommand>();
            retractcommand.Header.RequestId.HasValue.IsTrue();

            RetractCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => RetractCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => RetractCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => RetractCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => RetractCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => RetractCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new RetractCompletion(retractcommand.Header.RequestId.Value, new RetractCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICashManagementDevice Device { get => Provider.Device.IsA<ICashManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashManagementService CashManagement { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
