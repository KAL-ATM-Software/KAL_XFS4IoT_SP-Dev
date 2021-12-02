/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * ClearCommandNonceHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Common
{
    [CommandHandler(XFSConstants.ServiceClass.Common, typeof(ClearCommandNonceCommand))]
    public partial class ClearCommandNonceHandler : ICommandHandler
    {
        public ClearCommandNonceHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ClearCommandNonceHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ClearCommandNonceHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICommonDevice>();

            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ClearCommandNonceHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ClearCommandNonceHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var clearCommandNonceCmd = command.IsA<ClearCommandNonceCommand>($"Invalid parameter in the ClearCommandNonce Handle method. {nameof(ClearCommandNonceCommand)}");
            clearCommandNonceCmd.Header.RequestId.HasValue.IsTrue();

            IClearCommandNonceEvents events = new ClearCommandNonceEvents(Connection, clearCommandNonceCmd.Header.RequestId.Value);

            var result = await HandleClearCommandNonce(events, clearCommandNonceCmd, cancel);
            await Connection.SendMessageAsync(new ClearCommandNonceCompletion(clearCommandNonceCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var clearCommandNoncecommand = command.IsA<ClearCommandNonceCommand>();
            clearCommandNoncecommand.Header.RequestId.HasValue.IsTrue();

            ClearCommandNonceCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ClearCommandNonceCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => ClearCommandNonceCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ClearCommandNonceCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ClearCommandNonceCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ClearCommandNonceCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ClearCommandNonceCompletion(clearCommandNoncecommand.Header.RequestId.Value, new ClearCommandNonceCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICommonDevice Device { get => Provider.Device.IsA<ICommonDevice>(); }
        private IServiceProvider Provider { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
