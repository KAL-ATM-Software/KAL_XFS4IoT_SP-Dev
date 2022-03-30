/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * ExitModeRequestHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.VendorMode, typeof(ExitModeRequestCommand))]
    public partial class ExitModeRequestHandler : ICommandHandler
    {
        public ExitModeRequestHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ExitModeRequestHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ExitModeRequestHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IVendorModeDevice>();

            VendorMode = Provider.IsA<IVendorModeService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ExitModeRequestHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ExitModeRequestHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var exitModeRequestCmd = command.IsA<ExitModeRequestCommand>($"Invalid parameter in the ExitModeRequest Handle method. {nameof(ExitModeRequestCommand)}");
            exitModeRequestCmd.Header.RequestId.HasValue.IsTrue();

            IExitModeRequestEvents events = new ExitModeRequestEvents(Connection, exitModeRequestCmd.Header.RequestId.Value);

            var result = await HandleExitModeRequest(events, exitModeRequestCmd, cancel);
            await Connection.SendMessageAsync(new ExitModeRequestCompletion(exitModeRequestCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var exitModeRequestcommand = command.IsA<ExitModeRequestCommand>();
            exitModeRequestcommand.Header.RequestId.HasValue.IsTrue();

            ExitModeRequestCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ExitModeRequestCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => ExitModeRequestCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ExitModeRequestCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ExitModeRequestCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ExitModeRequestCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ExitModeRequestCompletion(exitModeRequestcommand.Header.RequestId.Value, new ExitModeRequestCompletion.PayloadData(errorCode, commandException.Message));

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
