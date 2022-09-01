/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * GetMixTypesHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashDispenser
{
    [CommandHandler(XFSConstants.ServiceClass.CashDispenser, typeof(GetMixTypesCommand))]
    public partial class GetMixTypesHandler : ICommandHandler
    {
        public GetMixTypesHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetMixTypesHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetMixTypesHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashDispenserDevice>();

            CashDispenser = Provider.IsA<ICashDispenserService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetMixTypesHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetMixTypesHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getMixTypesCmd = command.IsA<GetMixTypesCommand>($"Invalid parameter in the GetMixTypes Handle method. {nameof(GetMixTypesCommand)}");
            getMixTypesCmd.Header.RequestId.HasValue.IsTrue();

            IGetMixTypesEvents events = new GetMixTypesEvents(Connection, getMixTypesCmd.Header.RequestId.Value);

            var result = await HandleGetMixTypes(events, getMixTypesCmd, cancel);
            await Connection.SendMessageAsync(new GetMixTypesCompletion(getMixTypesCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getMixTypescommand = command.IsA<GetMixTypesCommand>();
            getMixTypescommand.Header.RequestId.HasValue.IsTrue();

            GetMixTypesCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetMixTypesCompletion(getMixTypescommand.Header.RequestId.Value, new GetMixTypesCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICashDispenserDevice Device { get => Provider.Device.IsA<ICashDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashDispenserService CashDispenser { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
