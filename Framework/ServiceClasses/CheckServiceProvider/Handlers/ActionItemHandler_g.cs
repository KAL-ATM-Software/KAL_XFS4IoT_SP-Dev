/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * ActionItemHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Check.Commands;
using XFS4IoT.Check.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Check
{
    [CommandHandler(XFSConstants.ServiceClass.Check, typeof(ActionItemCommand))]
    public partial class ActionItemHandler : ICommandHandler
    {
        public ActionItemHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ActionItemHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ActionItemHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICheckDevice>();

            Check = Provider.IsA<ICheckService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ActionItemHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ActionItemHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var actionItemCmd = command.IsA<ActionItemCommand>($"Invalid parameter in the ActionItem Handle method. {nameof(ActionItemCommand)}");
            actionItemCmd.Header.RequestId.HasValue.IsTrue();

            IActionItemEvents events = new ActionItemEvents(Connection, actionItemCmd.Header.RequestId.Value);

            var result = await HandleActionItem(events, actionItemCmd, cancel);
            await Connection.SendMessageAsync(new ActionItemCompletion(actionItemCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var actionItemcommand = command.IsA<ActionItemCommand>();
            actionItemcommand.Header.RequestId.HasValue.IsTrue();

            ActionItemCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ActionItemCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ActionItemCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ActionItemCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ActionItemCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ActionItemCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ActionItemCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ActionItemCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ActionItemCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ActionItemCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ActionItemCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ActionItemCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ActionItemCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ActionItemCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ActionItemCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ActionItemCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ActionItemCompletion(actionItemcommand.Header.RequestId.Value, new ActionItemCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICheckDevice Device { get => Provider.Device.IsA<ICheckDevice>(); }
        private IServiceProvider Provider { get; }
        private ICheckService Check { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
