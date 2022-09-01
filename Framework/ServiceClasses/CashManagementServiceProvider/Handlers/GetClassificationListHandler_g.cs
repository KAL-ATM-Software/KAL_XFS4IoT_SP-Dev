/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * GetClassificationListHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(GetClassificationListCommand))]
    public partial class GetClassificationListHandler : ICommandHandler
    {
        public GetClassificationListHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetClassificationListHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetClassificationListHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetClassificationListHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetClassificationListHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getClassificationListCmd = command.IsA<GetClassificationListCommand>($"Invalid parameter in the GetClassificationList Handle method. {nameof(GetClassificationListCommand)}");
            getClassificationListCmd.Header.RequestId.HasValue.IsTrue();

            IGetClassificationListEvents events = new GetClassificationListEvents(Connection, getClassificationListCmd.Header.RequestId.Value);

            var result = await HandleGetClassificationList(events, getClassificationListCmd, cancel);
            await Connection.SendMessageAsync(new GetClassificationListCompletion(getClassificationListCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getClassificationListcommand = command.IsA<GetClassificationListCommand>();
            getClassificationListcommand.Header.RequestId.HasValue.IsTrue();

            GetClassificationListCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetClassificationListCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetClassificationListCompletion(getClassificationListcommand.Header.RequestId.Value, new GetClassificationListCompletion.PayloadData(errorCode, commandException.Message));

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
