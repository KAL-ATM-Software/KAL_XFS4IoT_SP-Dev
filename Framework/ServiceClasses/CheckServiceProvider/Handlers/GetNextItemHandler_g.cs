/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * GetNextItemHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Check, typeof(GetNextItemCommand))]
    public partial class GetNextItemHandler : ICommandHandler
    {
        public GetNextItemHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetNextItemHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetNextItemHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICheckDevice>();

            Check = Provider.IsA<ICheckService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetNextItemHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetNextItemHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getNextItemCmd = command.IsA<GetNextItemCommand>($"Invalid parameter in the GetNextItem Handle method. {nameof(GetNextItemCommand)}");
            getNextItemCmd.Header.RequestId.HasValue.IsTrue();

            IGetNextItemEvents events = new GetNextItemEvents(Connection, getNextItemCmd.Header.RequestId.Value);

            var result = await HandleGetNextItem(events, getNextItemCmd, cancel);
            await Connection.SendMessageAsync(new GetNextItemCompletion(getNextItemCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getNextItemcommand = command.IsA<GetNextItemCommand>();
            getNextItemcommand.Header.RequestId.HasValue.IsTrue();

            GetNextItemCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetNextItemCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => GetNextItemCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => GetNextItemCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => GetNextItemCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => GetNextItemCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => GetNextItemCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => GetNextItemCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => GetNextItemCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => GetNextItemCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => GetNextItemCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => GetNextItemCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => GetNextItemCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetNextItemCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetNextItemCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetNextItemCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetNextItemCompletion(getNextItemcommand.Header.RequestId.Value, new GetNextItemCompletion.PayloadData(errorCode, commandException.Message));

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
