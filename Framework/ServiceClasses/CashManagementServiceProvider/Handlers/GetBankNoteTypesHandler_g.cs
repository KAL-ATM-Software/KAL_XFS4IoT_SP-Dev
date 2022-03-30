/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * GetBankNoteTypesHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(GetBankNoteTypesCommand))]
    public partial class GetBankNoteTypesHandler : ICommandHandler
    {
        public GetBankNoteTypesHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetBankNoteTypesHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetBankNoteTypesHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetBankNoteTypesHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetBankNoteTypesHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getBankNoteTypesCmd = command.IsA<GetBankNoteTypesCommand>($"Invalid parameter in the GetBankNoteTypes Handle method. {nameof(GetBankNoteTypesCommand)}");
            getBankNoteTypesCmd.Header.RequestId.HasValue.IsTrue();

            IGetBankNoteTypesEvents events = new GetBankNoteTypesEvents(Connection, getBankNoteTypesCmd.Header.RequestId.Value);

            var result = await HandleGetBankNoteTypes(events, getBankNoteTypesCmd, cancel);
            await Connection.SendMessageAsync(new GetBankNoteTypesCompletion(getBankNoteTypesCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getBankNoteTypescommand = command.IsA<GetBankNoteTypesCommand>();
            getBankNoteTypescommand.Header.RequestId.HasValue.IsTrue();

            GetBankNoteTypesCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetBankNoteTypesCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => GetBankNoteTypesCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetBankNoteTypesCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetBankNoteTypesCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetBankNoteTypesCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetBankNoteTypesCompletion(getBankNoteTypescommand.Header.RequestId.Value, new GetBankNoteTypesCompletion.PayloadData(errorCode, commandException.Message));

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
