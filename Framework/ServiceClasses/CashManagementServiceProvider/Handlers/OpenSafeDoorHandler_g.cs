/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * OpenSafeDoorHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashManagement
{
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(OpenSafeDoorCommand))]
    public partial class OpenSafeDoorHandler : ICommandHandler
    {
        public OpenSafeDoorHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(OpenSafeDoorHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(OpenSafeDoorHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(OpenSafeDoorHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(OpenSafeDoorHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var openSafeDoorCmd = command.IsA<OpenSafeDoorCommand>($"Invalid parameter in the OpenSafeDoor Handle method. {nameof(OpenSafeDoorCommand)}");
            openSafeDoorCmd.Header.RequestId.HasValue.IsTrue();

            IOpenSafeDoorEvents events = new OpenSafeDoorEvents(Connection, openSafeDoorCmd.Header.RequestId.Value);

            var result = await HandleOpenSafeDoor(events, openSafeDoorCmd, cancel);
            await Connection.SendMessageAsync(new OpenSafeDoorCompletion(openSafeDoorCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var openSafeDoorcommand = command.IsA<OpenSafeDoorCommand>();
            openSafeDoorcommand.Header.RequestId.HasValue.IsTrue();

            OpenSafeDoorCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => OpenSafeDoorCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => OpenSafeDoorCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => OpenSafeDoorCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => OpenSafeDoorCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => OpenSafeDoorCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new OpenSafeDoorCompletion(openSafeDoorcommand.Header.RequestId.Value, new OpenSafeDoorCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICashManagementDevice Device { get => Provider.Device.IsA<ICashManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashManagementService CashManagement { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
