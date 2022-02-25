/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * SetMatchHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Biometric.Commands;
using XFS4IoT.Biometric.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Biometric
{
    [CommandHandler(XFSConstants.ServiceClass.Biometric, typeof(SetMatchCommand))]
    public partial class SetMatchHandler : ICommandHandler
    {
        public SetMatchHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetMatchHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetMatchHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IBiometricDevice>();

            Biometric = Provider.IsA<IBiometricService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetMatchHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SetMatchHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var setMatchCmd = command.IsA<SetMatchCommand>($"Invalid parameter in the SetMatch Handle method. {nameof(SetMatchCommand)}");
            setMatchCmd.Header.RequestId.HasValue.IsTrue();

            ISetMatchEvents events = new SetMatchEvents(Connection, setMatchCmd.Header.RequestId.Value);

            var result = await HandleSetMatch(events, setMatchCmd, cancel);
            await Connection.SendMessageAsync(new SetMatchCompletion(setMatchCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setMatchcommand = command.IsA<SetMatchCommand>();
            setMatchcommand.Header.RequestId.HasValue.IsTrue();

            SetMatchCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetMatchCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => SetMatchCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetMatchCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetMatchCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetMatchCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetMatchCompletion(setMatchcommand.Header.RequestId.Value, new SetMatchCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IBiometricDevice Device { get => Provider.Device.IsA<IBiometricDevice>(); }
        private IServiceProvider Provider { get; }
        private IBiometricService Biometric { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
