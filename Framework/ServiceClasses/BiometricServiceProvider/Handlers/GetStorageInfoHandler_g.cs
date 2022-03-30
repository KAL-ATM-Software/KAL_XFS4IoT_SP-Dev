/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * GetStorageInfoHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Biometric, typeof(GetStorageInfoCommand))]
    public partial class GetStorageInfoHandler : ICommandHandler
    {
        public GetStorageInfoHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetStorageInfoHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetStorageInfoHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IBiometricDevice>();

            Biometric = Provider.IsA<IBiometricService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetStorageInfoHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetStorageInfoHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getStorageInfoCmd = command.IsA<GetStorageInfoCommand>($"Invalid parameter in the GetStorageInfo Handle method. {nameof(GetStorageInfoCommand)}");
            getStorageInfoCmd.Header.RequestId.HasValue.IsTrue();

            IGetStorageInfoEvents events = new GetStorageInfoEvents(Connection, getStorageInfoCmd.Header.RequestId.Value);

            var result = await HandleGetStorageInfo(events, getStorageInfoCmd, cancel);
            await Connection.SendMessageAsync(new GetStorageInfoCompletion(getStorageInfoCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getStorageInfocommand = command.IsA<GetStorageInfoCommand>();
            getStorageInfocommand.Header.RequestId.HasValue.IsTrue();

            GetStorageInfoCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetStorageInfoCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => GetStorageInfoCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetStorageInfoCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetStorageInfoCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetStorageInfoCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetStorageInfoCompletion(getStorageInfocommand.Header.RequestId.Value, new GetStorageInfoCompletion.PayloadData(errorCode, commandException.Message));

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
