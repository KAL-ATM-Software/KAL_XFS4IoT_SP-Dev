/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * ExpelMediaHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Check, typeof(ExpelMediaCommand))]
    public partial class ExpelMediaHandler : ICommandHandler
    {
        public ExpelMediaHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ExpelMediaHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ExpelMediaHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICheckDevice>();

            Check = Provider.IsA<ICheckService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ExpelMediaHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ExpelMediaHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var expelMediaCmd = command.IsA<ExpelMediaCommand>($"Invalid parameter in the ExpelMedia Handle method. {nameof(ExpelMediaCommand)}");
            expelMediaCmd.Header.RequestId.HasValue.IsTrue();

            IExpelMediaEvents events = new ExpelMediaEvents(Connection, expelMediaCmd.Header.RequestId.Value);

            var result = await HandleExpelMedia(events, expelMediaCmd, cancel);
            await Connection.SendMessageAsync(new ExpelMediaCompletion(expelMediaCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var expelMediacommand = command.IsA<ExpelMediaCommand>();
            expelMediacommand.Header.RequestId.HasValue.IsTrue();

            ExpelMediaCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ExpelMediaCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ExpelMediaCompletion(expelMediacommand.Header.RequestId.Value, new ExpelMediaCompletion.PayloadData(errorCode, commandException.Message));

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
