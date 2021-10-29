/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * SetVersionsHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Common, typeof(SetVersionsCommand))]
    public partial class SetVersionsHandler : ICommandHandler
    {
        public SetVersionsHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetVersionsHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetVersionsHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICommonDevice>();

            Common = Provider.IsA<ICommonServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetVersionsHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var setVersionsCmd = command.IsA<SetVersionsCommand>($"Invalid parameter in the SetVersions Handle method. {nameof(SetVersionsCommand)}");
            setVersionsCmd.Header.RequestId.HasValue.IsTrue();

            ISetVersionsEvents events = new SetVersionsEvents(Connection, setVersionsCmd.Header.RequestId.Value);

            var result = await HandleSetVersions(events, setVersionsCmd, cancel);
            await Connection.SendMessageAsync(new SetVersionsCompletion(setVersionsCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var setVersionscommand = command.IsA<SetVersionsCommand>();
            setVersionscommand.Header.RequestId.HasValue.IsTrue();

            SetVersionsCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetVersionsCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => SetVersionsCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => SetVersionsCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => SetVersionsCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetVersionsCompletion(setVersionscommand.Header.RequestId.Value, new SetVersionsCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICommonDevice Device { get => Provider.Device.IsA<ICommonDevice>(); }
        private IServiceProvider Provider { get; }
        private ICommonServiceClass Common { get; }
        private ILogger Logger { get; }
    }

}
