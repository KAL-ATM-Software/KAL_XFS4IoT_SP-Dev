/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * SetMediaParametersHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Check, typeof(SetMediaParametersCommand))]
    public partial class SetMediaParametersHandler : ICommandHandler
    {
        public SetMediaParametersHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetMediaParametersHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetMediaParametersHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICheckDevice>();

            Check = Provider.IsA<ICheckService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetMediaParametersHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SetMediaParametersHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var setMediaParametersCmd = command.IsA<SetMediaParametersCommand>($"Invalid parameter in the SetMediaParameters Handle method. {nameof(SetMediaParametersCommand)}");
            setMediaParametersCmd.Header.RequestId.HasValue.IsTrue();

            ISetMediaParametersEvents events = new SetMediaParametersEvents(Connection, setMediaParametersCmd.Header.RequestId.Value);

            var result = await HandleSetMediaParameters(events, setMediaParametersCmd, cancel);
            await Connection.SendMessageAsync(new SetMediaParametersCompletion(setMediaParametersCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setMediaParameterscommand = command.IsA<SetMediaParametersCommand>();
            setMediaParameterscommand.Header.RequestId.HasValue.IsTrue();

            SetMediaParametersCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetMediaParametersCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetMediaParametersCompletion(setMediaParameterscommand.Header.RequestId.Value, new SetMediaParametersCompletion.PayloadData(errorCode, commandException.Message));

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
