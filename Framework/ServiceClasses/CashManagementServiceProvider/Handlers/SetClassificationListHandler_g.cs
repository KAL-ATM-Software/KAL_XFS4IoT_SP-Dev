/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * SetClassificationListHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashManagement
{
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(SetClassificationListCommand))]
    public partial class SetClassificationListHandler : ICommandHandler
    {
        public SetClassificationListHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetClassificationListHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetClassificationListHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetClassificationListHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var setClassificationListCmd = command.IsA<SetClassificationListCommand>($"Invalid parameter in the SetClassificationList Handle method. {nameof(SetClassificationListCommand)}");
            setClassificationListCmd.Header.RequestId.HasValue.IsTrue();

            ISetClassificationListEvents events = new SetClassificationListEvents(Connection, setClassificationListCmd.Header.RequestId.Value);

            var result = await HandleSetClassificationList(events, setClassificationListCmd, cancel);
            await Connection.SendMessageAsync(new SetClassificationListCompletion(setClassificationListCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var setClassificationListcommand = command.IsA<SetClassificationListCommand>();
            setClassificationListcommand.Header.RequestId.HasValue.IsTrue();

            SetClassificationListCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetClassificationListCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => SetClassificationListCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => SetClassificationListCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetClassificationListCompletion(setClassificationListcommand.Header.RequestId.Value, new SetClassificationListCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICashManagementDevice Device { get => Provider.Device.IsA<ICashManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashManagementServiceClass CashManagement { get; }
        private ILogger Logger { get; }
    }

}
