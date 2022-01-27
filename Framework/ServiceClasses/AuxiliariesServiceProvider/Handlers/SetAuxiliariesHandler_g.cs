/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * SetAuxiliariesHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Auxiliaries.Commands;
using XFS4IoT.Auxiliaries.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Auxiliaries
{
    [CommandHandler(XFSConstants.ServiceClass.Auxiliaries, typeof(SetAuxiliariesCommand))]
    public partial class SetAuxiliariesHandler : ICommandHandler
    {
        public SetAuxiliariesHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetAuxiliariesHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetAuxiliariesHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IAuxiliariesDevice>();

            Auxiliaries = Provider.IsA<IAuxiliariesService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetAuxiliariesHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SetAuxiliariesHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var setAuxiliariesCmd = command.IsA<SetAuxiliariesCommand>($"Invalid parameter in the SetAuxiliaries Handle method. {nameof(SetAuxiliariesCommand)}");
            setAuxiliariesCmd.Header.RequestId.HasValue.IsTrue();

            ISetAuxiliariesEvents events = new SetAuxiliariesEvents(Connection, setAuxiliariesCmd.Header.RequestId.Value);

            var result = await HandleSetAuxiliaries(events, setAuxiliariesCmd, cancel);
            await Connection.SendMessageAsync(new SetAuxiliariesCompletion(setAuxiliariesCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setAuxiliariescommand = command.IsA<SetAuxiliariesCommand>();
            setAuxiliariescommand.Header.RequestId.HasValue.IsTrue();

            SetAuxiliariesCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetAuxiliariesCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => SetAuxiliariesCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetAuxiliariesCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetAuxiliariesCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetAuxiliariesCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetAuxiliariesCompletion(setAuxiliariescommand.Header.RequestId.Value, new SetAuxiliariesCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IAuxiliariesDevice Device { get => Provider.Device.IsA<IAuxiliariesDevice>(); }
        private IServiceProvider Provider { get; }
        private IAuxiliariesService Auxiliaries { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
