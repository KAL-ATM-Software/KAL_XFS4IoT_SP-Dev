/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * ResetHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Check, typeof(ResetCommand))]
    public partial class ResetHandler : ICommandHandler
    {
        public ResetHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ResetHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ResetHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICheckDevice>();

            Check = Provider.IsA<ICheckService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ResetHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ResetHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var resetCmd = command.IsA<ResetCommand>($"Invalid parameter in the Reset Handle method. {nameof(ResetCommand)}");
            resetCmd.Header.RequestId.HasValue.IsTrue();

            IResetEvents events = new ResetEvents(Connection, resetCmd.Header.RequestId.Value);

            var result = await HandleReset(events, resetCmd, cancel);
            await Connection.SendMessageAsync(new ResetCompletion(resetCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var resetcommand = command.IsA<ResetCommand>();
            resetcommand.Header.RequestId.HasValue.IsTrue();

            ResetCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ResetCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ResetCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ResetCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ResetCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ResetCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ResetCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ResetCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ResetCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ResetCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ResetCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ResetCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ResetCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ResetCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ResetCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ResetCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ResetCompletion(resetcommand.Header.RequestId.Value, new ResetCompletion.PayloadData(errorCode, commandException.Message));

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
