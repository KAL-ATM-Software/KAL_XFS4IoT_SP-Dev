/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * GetItemInfoHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(GetItemInfoCommand))]
    public partial class GetItemInfoHandler : ICommandHandler
    {
        public GetItemInfoHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetItemInfoHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetItemInfoHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetItemInfoHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var getItemInfoCmd = command.IsA<GetItemInfoCommand>($"Invalid parameter in the GetItemInfo Handle method. {nameof(GetItemInfoCommand)}");
            getItemInfoCmd.Header.RequestId.HasValue.IsTrue();

            IGetItemInfoEvents events = new GetItemInfoEvents(Connection, getItemInfoCmd.Header.RequestId.Value);

            var result = await HandleGetItemInfo(events, getItemInfoCmd, cancel);
            await Connection.SendMessageAsync(new GetItemInfoCompletion(getItemInfoCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var getItemInfocommand = command.IsA<GetItemInfoCommand>();
            getItemInfocommand.Header.RequestId.HasValue.IsTrue();

            GetItemInfoCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetItemInfoCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => GetItemInfoCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => GetItemInfoCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetItemInfoCompletion(getItemInfocommand.Header.RequestId.Value, new GetItemInfoCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICashManagementDevice Device { get => Provider.Device.IsA<ICashManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashManagementServiceClass CashManagement { get; }
        private ILogger Logger { get; }
    }

}
