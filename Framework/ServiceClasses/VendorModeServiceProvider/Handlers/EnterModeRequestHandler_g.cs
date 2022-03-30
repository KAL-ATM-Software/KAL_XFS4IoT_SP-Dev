/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * EnterModeRequestHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.VendorMode, typeof(EnterModeRequestCommand))]
    public partial class EnterModeRequestHandler : ICommandHandler
    {
        public EnterModeRequestHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EnterModeRequestHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(EnterModeRequestHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IVendorModeDevice>();

            VendorMode = Provider.IsA<IVendorModeService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(EnterModeRequestHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(EnterModeRequestHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var enterModeRequestCmd = command.IsA<EnterModeRequestCommand>($"Invalid parameter in the EnterModeRequest Handle method. {nameof(EnterModeRequestCommand)}");
            enterModeRequestCmd.Header.RequestId.HasValue.IsTrue();

            IEnterModeRequestEvents events = new EnterModeRequestEvents(Connection, enterModeRequestCmd.Header.RequestId.Value);

            var result = await HandleEnterModeRequest(events, enterModeRequestCmd, cancel);
            await Connection.SendMessageAsync(new EnterModeRequestCompletion(enterModeRequestCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var enterModeRequestcommand = command.IsA<EnterModeRequestCommand>();
            enterModeRequestcommand.Header.RequestId.HasValue.IsTrue();

            EnterModeRequestCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => EnterModeRequestCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => EnterModeRequestCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => EnterModeRequestCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => EnterModeRequestCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => EnterModeRequestCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new EnterModeRequestCompletion(enterModeRequestcommand.Header.RequestId.Value, new EnterModeRequestCompletion.PayloadData(errorCode, commandException.Message));

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
