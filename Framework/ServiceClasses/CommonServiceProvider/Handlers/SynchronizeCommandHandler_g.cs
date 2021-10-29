/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * SynchronizeCommandHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Common, typeof(SynchronizeCommandCommand))]
    public partial class SynchronizeCommandHandler : ICommandHandler
    {
        public SynchronizeCommandHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SynchronizeCommandHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SynchronizeCommandHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICommonDevice>();

            Common = Provider.IsA<ICommonServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SynchronizeCommandHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var synchronizeCommandCmd = command.IsA<SynchronizeCommandCommand>($"Invalid parameter in the SynchronizeCommand Handle method. {nameof(SynchronizeCommandCommand)}");
            synchronizeCommandCmd.Header.RequestId.HasValue.IsTrue();

            ISynchronizeCommandEvents events = new SynchronizeCommandEvents(Connection, synchronizeCommandCmd.Header.RequestId.Value);

            var result = await HandleSynchronizeCommand(events, synchronizeCommandCmd, cancel);
            await Connection.SendMessageAsync(new SynchronizeCommandCompletion(synchronizeCommandCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var synchronizeCommandcommand = command.IsA<SynchronizeCommandCommand>();
            synchronizeCommandcommand.Header.RequestId.HasValue.IsTrue();

            SynchronizeCommandCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SynchronizeCommandCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => SynchronizeCommandCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SynchronizeCommandCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SynchronizeCommandCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SynchronizeCommandCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SynchronizeCommandCompletion(synchronizeCommandcommand.Header.RequestId.Value, new SynchronizeCommandCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICommonDevice Device { get => Provider.Device.IsA<ICommonDevice>(); }
        private IServiceProvider Provider { get; }
        private ICommonServiceClass Common { get; }
        private ILogger Logger { get; }
    }

}
