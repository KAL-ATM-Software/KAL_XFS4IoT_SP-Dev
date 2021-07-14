/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * GetTellerInfoHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(GetTellerInfoCommand))]
    public partial class GetTellerInfoHandler : ICommandHandler
    {
        public GetTellerInfoHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetTellerInfoHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetTellerInfoHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetTellerInfoHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var getTellerInfoCmd = command.IsA<GetTellerInfoCommand>($"Invalid parameter in the GetTellerInfo Handle method. {nameof(GetTellerInfoCommand)}");
            getTellerInfoCmd.Header.RequestId.HasValue.IsTrue();

            IGetTellerInfoEvents events = new GetTellerInfoEvents(Connection, getTellerInfoCmd.Header.RequestId.Value);

            var result = await HandleGetTellerInfo(events, getTellerInfoCmd, cancel);
            await Connection.SendMessageAsync(new GetTellerInfoCompletion(getTellerInfoCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var getTellerInfocommand = command.IsA<GetTellerInfoCommand>();
            getTellerInfocommand.Header.RequestId.HasValue.IsTrue();

            GetTellerInfoCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetTellerInfoCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => GetTellerInfoCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => GetTellerInfoCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetTellerInfoCompletion(getTellerInfocommand.Header.RequestId.Value, new GetTellerInfoCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICashManagementDevice Device { get => Provider.Device.IsA<ICashManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashManagementServiceClass CashManagement { get; }
        private ILogger Logger { get; }
    }

}
