/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * GetAutoStartupTimeHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Auxiliaries, typeof(GetAutoStartupTimeCommand))]
    public partial class GetAutoStartupTimeHandler : ICommandHandler
    {
        public GetAutoStartupTimeHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetAutoStartupTimeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetAutoStartupTimeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IAuxiliariesDevice>();

            Auxiliaries = Provider.IsA<IAuxiliariesService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetAutoStartupTimeHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetAutoStartupTimeHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getAutoStartupTimeCmd = command.IsA<GetAutoStartupTimeCommand>($"Invalid parameter in the GetAutoStartupTime Handle method. {nameof(GetAutoStartupTimeCommand)}");
            getAutoStartupTimeCmd.Header.RequestId.HasValue.IsTrue();

            IGetAutoStartupTimeEvents events = new GetAutoStartupTimeEvents(Connection, getAutoStartupTimeCmd.Header.RequestId.Value);

            var result = await HandleGetAutoStartupTime(events, getAutoStartupTimeCmd, cancel);
            await Connection.SendMessageAsync(new GetAutoStartupTimeCompletion(getAutoStartupTimeCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getAutoStartupTimecommand = command.IsA<GetAutoStartupTimeCommand>();
            getAutoStartupTimecommand.Header.RequestId.HasValue.IsTrue();

            GetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => GetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetAutoStartupTimeCompletion(getAutoStartupTimecommand.Header.RequestId.Value, new GetAutoStartupTimeCompletion.PayloadData(errorCode, commandException.Message));

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
