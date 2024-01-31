/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ImportKeyTokenHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(ImportKeyTokenCommand))]
    public partial class ImportKeyTokenHandler : ICommandHandler
    {
        public ImportKeyTokenHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ImportKeyTokenHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ImportKeyTokenHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ImportKeyTokenHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ImportKeyTokenHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var importKeyTokenCmd = command.IsA<ImportKeyTokenCommand>($"Invalid parameter in the ImportKeyToken Handle method. {nameof(ImportKeyTokenCommand)}");
            importKeyTokenCmd.Header.RequestId.HasValue.IsTrue();

            IImportKeyTokenEvents events = new ImportKeyTokenEvents(Connection, importKeyTokenCmd.Header.RequestId.Value);

            var result = await HandleImportKeyToken(events, importKeyTokenCmd, cancel);
            await Connection.SendMessageAsync(new ImportKeyTokenCompletion(importKeyTokenCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var importKeyTokencommand = command.IsA<ImportKeyTokenCommand>();
            importKeyTokencommand.Header.RequestId.HasValue.IsTrue();

            ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ImportKeyTokenCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ImportKeyTokenCompletion(importKeyTokencommand.Header.RequestId.Value, new ImportKeyTokenCompletion.PayloadData(errorCode, commandException.Message));

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
