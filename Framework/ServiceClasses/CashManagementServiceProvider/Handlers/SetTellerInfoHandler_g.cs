/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * SetTellerInfoHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(SetTellerInfoCommand))]
    public partial class SetTellerInfoHandler : ICommandHandler
    {
        public SetTellerInfoHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetTellerInfoHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetTellerInfoHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetTellerInfoHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SetTellerInfoHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var setTellerInfoCmd = command.IsA<SetTellerInfoCommand>($"Invalid parameter in the SetTellerInfo Handle method. {nameof(SetTellerInfoCommand)}");
            setTellerInfoCmd.Header.RequestId.HasValue.IsTrue();

            ISetTellerInfoEvents events = new SetTellerInfoEvents(Connection, setTellerInfoCmd.Header.RequestId.Value);

            var result = await HandleSetTellerInfo(events, setTellerInfoCmd, cancel);
            await Connection.SendMessageAsync(new SetTellerInfoCompletion(setTellerInfoCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setTellerInfocommand = command.IsA<SetTellerInfoCommand>();
            setTellerInfocommand.Header.RequestId.HasValue.IsTrue();

            SetTellerInfoCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetTellerInfoCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => SetTellerInfoCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetTellerInfoCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetTellerInfoCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetTellerInfoCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetTellerInfoCompletion(setTellerInfocommand.Header.RequestId.Value, new SetTellerInfoCompletion.PayloadData(errorCode, commandException.Message));

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
