/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoTFramework.Common;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashManagement
{
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(GetTellerInfoCommand))]
    public partial class GetTellerInfoHandler : ICommandHandler
    {
        public GetTellerInfoHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetTellerInfoHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetTellerInfoHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetTellerInfoHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetTellerInfoHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getTellerInfoCmd = command.IsA<GetTellerInfoCommand>($"Invalid parameter in the GetTellerInfo Handle method. {nameof(GetTellerInfoCommand)}");
            getTellerInfoCmd.Header.RequestId.HasValue.IsTrue();

            IGetTellerInfoEvents events = new GetTellerInfoEvents(Connection, getTellerInfoCmd.Header.RequestId.Value);

            var result = await HandleGetTellerInfo(events, getTellerInfoCmd, cancel);
            await Connection.SendMessageAsync(new GetTellerInfoCompletion(getTellerInfoCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getTellerInfocommand = command.IsA<GetTellerInfoCommand>();
            getTellerInfocommand.Header.RequestId.HasValue.IsTrue();

            GetTellerInfoCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetTellerInfoCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => GetTellerInfoCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetTellerInfoCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetTellerInfoCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetTellerInfoCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetTellerInfoCompletion(getTellerInfocommand.Header.RequestId.Value, new GetTellerInfoCompletion.PayloadData(errorCode, commandException.Message));

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
