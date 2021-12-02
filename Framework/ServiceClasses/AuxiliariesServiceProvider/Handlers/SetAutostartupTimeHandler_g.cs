/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * SetAutostartupTimeHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Auxiliaries.Commands;
using XFS4IoT.Auxiliaries.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Auxiliaries
{
    [CommandHandler(XFSConstants.ServiceClass.Auxiliaries, typeof(SetAutostartupTimeCommand))]
    public partial class SetAutostartupTimeHandler : ICommandHandler
    {
        public SetAutostartupTimeHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetAutostartupTimeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetAutostartupTimeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IAuxiliariesDevice>();

            Auxiliaries = Provider.IsA<IAuxiliariesService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetAutostartupTimeHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SetAutostartupTimeHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var setAutostartupTimeCmd = command.IsA<SetAutostartupTimeCommand>($"Invalid parameter in the SetAutostartupTime Handle method. {nameof(SetAutostartupTimeCommand)}");
            setAutostartupTimeCmd.Header.RequestId.HasValue.IsTrue();

            ISetAutostartupTimeEvents events = new SetAutostartupTimeEvents(Connection, setAutostartupTimeCmd.Header.RequestId.Value);

            var result = await HandleSetAutostartupTime(events, setAutostartupTimeCmd, cancel);
            await Connection.SendMessageAsync(new SetAutostartupTimeCompletion(setAutostartupTimeCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setAutostartupTimecommand = command.IsA<SetAutostartupTimeCommand>();
            setAutostartupTimecommand.Header.RequestId.HasValue.IsTrue();

            SetAutostartupTimeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetAutostartupTimeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => SetAutostartupTimeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetAutostartupTimeCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetAutostartupTimeCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetAutostartupTimeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetAutostartupTimeCompletion(setAutostartupTimecommand.Header.RequestId.Value, new SetAutostartupTimeCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IAuxiliariesDevice Device { get => Provider.Device.IsA<IAuxiliariesDevice>(); }
        private IServiceProvider Provider { get; }
        private IAuxiliariesService Auxiliaries { get; }
        private ILogger Logger { get; }
    }

}
