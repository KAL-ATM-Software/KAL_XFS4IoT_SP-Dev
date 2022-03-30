/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * ExitModeAcknowledgeHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.VendorMode.Commands;
using XFS4IoT.VendorMode.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.VendorMode
{
    [CommandHandler(XFSConstants.ServiceClass.VendorMode, typeof(ExitModeAcknowledgeCommand))]
    public partial class ExitModeAcknowledgeHandler : ICommandHandler
    {
        public ExitModeAcknowledgeHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ExitModeAcknowledgeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ExitModeAcknowledgeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IVendorModeDevice>();

            VendorMode = Provider.IsA<IVendorModeService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ExitModeAcknowledgeHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ExitModeAcknowledgeHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var exitModeAcknowledgeCmd = command.IsA<ExitModeAcknowledgeCommand>($"Invalid parameter in the ExitModeAcknowledge Handle method. {nameof(ExitModeAcknowledgeCommand)}");
            exitModeAcknowledgeCmd.Header.RequestId.HasValue.IsTrue();

            IExitModeAcknowledgeEvents events = new ExitModeAcknowledgeEvents(Connection, exitModeAcknowledgeCmd.Header.RequestId.Value);

            var result = await HandleExitModeAcknowledge(events, exitModeAcknowledgeCmd, cancel);
            await Connection.SendMessageAsync(new ExitModeAcknowledgeCompletion(exitModeAcknowledgeCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var exitModeAcknowledgecommand = command.IsA<ExitModeAcknowledgeCommand>();
            exitModeAcknowledgecommand.Header.RequestId.HasValue.IsTrue();

            ExitModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ExitModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => ExitModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ExitModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ExitModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ExitModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ExitModeAcknowledgeCompletion(exitModeAcknowledgecommand.Header.RequestId.Value, new ExitModeAcknowledgeCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IVendorModeDevice Device { get => Provider.Device.IsA<IVendorModeDevice>(); }
        private IServiceProvider Provider { get; }
        private IVendorModeService VendorMode { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
