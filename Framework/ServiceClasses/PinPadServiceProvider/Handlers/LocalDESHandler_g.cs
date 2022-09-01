/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * LocalDESHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.PinPad.Commands;
using XFS4IoT.PinPad.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.PinPad
{
    [CommandHandler(XFSConstants.ServiceClass.PinPad, typeof(LocalDESCommand))]
    public partial class LocalDESHandler : ICommandHandler
    {
        public LocalDESHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(LocalDESHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(LocalDESHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPinPadDevice>();

            PinPad = Provider.IsA<IPinPadService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(LocalDESHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(LocalDESHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var localDESCmd = command.IsA<LocalDESCommand>($"Invalid parameter in the LocalDES Handle method. {nameof(LocalDESCommand)}");
            localDESCmd.Header.RequestId.HasValue.IsTrue();

            ILocalDESEvents events = new LocalDESEvents(Connection, localDESCmd.Header.RequestId.Value);

            var result = await HandleLocalDES(events, localDESCmd, cancel);
            await Connection.SendMessageAsync(new LocalDESCompletion(localDESCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var localDEScommand = command.IsA<LocalDESCommand>();
            localDEScommand.Header.RequestId.HasValue.IsTrue();

            LocalDESCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => LocalDESCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => LocalDESCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => LocalDESCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => LocalDESCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => LocalDESCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => LocalDESCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => LocalDESCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => LocalDESCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => LocalDESCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => LocalDESCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => LocalDESCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => LocalDESCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => LocalDESCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => LocalDESCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => LocalDESCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new LocalDESCompletion(localDEScommand.Header.RequestId.Value, new LocalDESCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IPinPadDevice Device { get => Provider.Device.IsA<IPinPadDevice>(); }
        private IServiceProvider Provider { get; }
        private IPinPadService PinPad { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
