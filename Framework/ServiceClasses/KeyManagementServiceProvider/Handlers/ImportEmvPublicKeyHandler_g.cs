/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ImportEmvPublicKeyHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.KeyManagement
{
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(ImportEmvPublicKeyCommand))]
    public partial class ImportEmvPublicKeyHandler : ICommandHandler
    {
        public ImportEmvPublicKeyHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ImportEmvPublicKeyHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ImportEmvPublicKeyHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ImportEmvPublicKeyHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ImportEmvPublicKeyHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var importEmvPublicKeyCmd = command.IsA<ImportEmvPublicKeyCommand>($"Invalid parameter in the ImportEmvPublicKey Handle method. {nameof(ImportEmvPublicKeyCommand)}");
            importEmvPublicKeyCmd.Header.RequestId.HasValue.IsTrue();

            IImportEmvPublicKeyEvents events = new ImportEmvPublicKeyEvents(Connection, importEmvPublicKeyCmd.Header.RequestId.Value);

            var result = await HandleImportEmvPublicKey(events, importEmvPublicKeyCmd, cancel);
            await Connection.SendMessageAsync(new ImportEmvPublicKeyCompletion(importEmvPublicKeyCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var importEmvPublicKeycommand = command.IsA<ImportEmvPublicKeyCommand>();
            importEmvPublicKeycommand.Header.RequestId.HasValue.IsTrue();

            ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ImportEmvPublicKeyCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ImportEmvPublicKeyCompletion(importEmvPublicKeycommand.Header.RequestId.Value, new ImportEmvPublicKeyCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IKeyManagementDevice Device { get => Provider.Device.IsA<IKeyManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyManagementService KeyManagement { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
