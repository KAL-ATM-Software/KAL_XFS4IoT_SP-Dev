/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * GetActiveInterfaceHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.VendorApplication.Commands;
using XFS4IoT.VendorApplication.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.VendorApplication
{
    [CommandHandler(XFSConstants.ServiceClass.VendorApplication, typeof(GetActiveInterfaceCommand))]
    public partial class GetActiveInterfaceHandler : ICommandHandler
    {
        public GetActiveInterfaceHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetActiveInterfaceHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetActiveInterfaceHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IVendorApplicationDevice>();

            VendorApplication = Provider.IsA<IVendorApplicationService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetActiveInterfaceHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetActiveInterfaceHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getActiveInterfaceCmd = command.IsA<GetActiveInterfaceCommand>($"Invalid parameter in the GetActiveInterface Handle method. {nameof(GetActiveInterfaceCommand)}");
            getActiveInterfaceCmd.Header.RequestId.HasValue.IsTrue();

            IGetActiveInterfaceEvents events = new GetActiveInterfaceEvents(Connection, getActiveInterfaceCmd.Header.RequestId.Value);

            var result = await HandleGetActiveInterface(events, getActiveInterfaceCmd, cancel);
            await Connection.SendMessageAsync(new GetActiveInterfaceCompletion(getActiveInterfaceCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getActiveInterfacecommand = command.IsA<GetActiveInterfaceCommand>();
            getActiveInterfacecommand.Header.RequestId.HasValue.IsTrue();

            GetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => GetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetActiveInterfaceCompletion(getActiveInterfacecommand.Header.RequestId.Value, new GetActiveInterfaceCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IVendorApplicationDevice Device { get => Provider.Device.IsA<IVendorApplicationDevice>(); }
        private IServiceProvider Provider { get; }
        private IVendorApplicationService VendorApplication { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
