/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * AcceptItemHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Check, typeof(AcceptItemCommand))]
    public partial class AcceptItemHandler : ICommandHandler
    {
        public AcceptItemHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(AcceptItemHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(AcceptItemHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICheckDevice>();

            Check = Provider.IsA<ICheckService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(AcceptItemHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(AcceptItemHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var acceptItemCmd = command.IsA<AcceptItemCommand>($"Invalid parameter in the AcceptItem Handle method. {nameof(AcceptItemCommand)}");
            acceptItemCmd.Header.RequestId.HasValue.IsTrue();

            IAcceptItemEvents events = new AcceptItemEvents(Connection, acceptItemCmd.Header.RequestId.Value);

            var result = await HandleAcceptItem(events, acceptItemCmd, cancel);
            await Connection.SendMessageAsync(new AcceptItemCompletion(acceptItemCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var acceptItemcommand = command.IsA<AcceptItemCommand>();
            acceptItemcommand.Header.RequestId.HasValue.IsTrue();

            AcceptItemCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => AcceptItemCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => AcceptItemCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => AcceptItemCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => AcceptItemCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => AcceptItemCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => AcceptItemCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => AcceptItemCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => AcceptItemCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => AcceptItemCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => AcceptItemCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => AcceptItemCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => AcceptItemCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => AcceptItemCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => AcceptItemCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => AcceptItemCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new AcceptItemCompletion(acceptItemcommand.Header.RequestId.Value, new AcceptItemCompletion.PayloadData(errorCode, commandException.Message));

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
