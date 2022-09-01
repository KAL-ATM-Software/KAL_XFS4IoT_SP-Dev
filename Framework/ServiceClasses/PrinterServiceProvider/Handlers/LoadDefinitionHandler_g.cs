/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * LoadDefinitionHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Printer
{
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(LoadDefinitionCommand))]
    public partial class LoadDefinitionHandler : ICommandHandler
    {
        public LoadDefinitionHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(LoadDefinitionHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(LoadDefinitionHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(LoadDefinitionHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(LoadDefinitionHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var loadDefinitionCmd = command.IsA<LoadDefinitionCommand>($"Invalid parameter in the LoadDefinition Handle method. {nameof(LoadDefinitionCommand)}");
            loadDefinitionCmd.Header.RequestId.HasValue.IsTrue();

            ILoadDefinitionEvents events = new LoadDefinitionEvents(Connection, loadDefinitionCmd.Header.RequestId.Value);

            var result = await HandleLoadDefinition(events, loadDefinitionCmd, cancel);
            await Connection.SendMessageAsync(new LoadDefinitionCompletion(loadDefinitionCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var loadDefinitioncommand = command.IsA<LoadDefinitionCommand>();
            loadDefinitioncommand.Header.RequestId.HasValue.IsTrue();

            LoadDefinitionCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => LoadDefinitionCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new LoadDefinitionCompletion(loadDefinitioncommand.Header.RequestId.Value, new LoadDefinitionCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IPrinterDevice Device { get => Provider.Device.IsA<IPrinterDevice>(); }
        private IServiceProvider Provider { get; }
        private IPrinterService Printer { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
