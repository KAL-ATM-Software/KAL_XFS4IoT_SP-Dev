/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * GetCommandNonceHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Common
{
    [CommandHandler(XFSConstants.ServiceClass.Common, typeof(GetCommandNonceCommand))]
    public partial class GetCommandNonceHandler : ICommandHandler
    {
        public GetCommandNonceHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetCommandNonceHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetCommandNonceHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICommonDevice>();

            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetCommandNonceHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetCommandNonceHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getCommandNonceCmd = command.IsA<GetCommandNonceCommand>($"Invalid parameter in the GetCommandNonce Handle method. {nameof(GetCommandNonceCommand)}");
            getCommandNonceCmd.Header.RequestId.HasValue.IsTrue();

            IGetCommandNonceEvents events = new GetCommandNonceEvents(Connection, getCommandNonceCmd.Header.RequestId.Value);

            var result = await HandleGetCommandNonce(events, getCommandNonceCmd, cancel);
            await Connection.SendMessageAsync(new GetCommandNonceCompletion(getCommandNonceCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getCommandNoncecommand = command.IsA<GetCommandNonceCommand>();
            getCommandNoncecommand.Header.RequestId.HasValue.IsTrue();

            GetCommandNonceCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetCommandNonceCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetCommandNonceCompletion(getCommandNoncecommand.Header.RequestId.Value, new GetCommandNonceCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICommonDevice Device { get => Provider.Device.IsA<ICommonDevice>(); }
        private IServiceProvider Provider { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
