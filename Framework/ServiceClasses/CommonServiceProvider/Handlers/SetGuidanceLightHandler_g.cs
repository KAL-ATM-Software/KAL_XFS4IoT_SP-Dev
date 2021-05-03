/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * SetGuidanceLightHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Common, typeof(SetGuidanceLightCommand))]
    public partial class SetGuidanceLightHandler : ICommandHandler
    {
        public SetGuidanceLightHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetGuidanceLightHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetGuidanceLightHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICommonDevice>();

            Common = Provider.IsA<ICommonServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetGuidanceLightHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var setGuidanceLightCmd = command.IsA<SetGuidanceLightCommand>($"Invalid parameter in the SetGuidanceLight Handle method. {nameof(SetGuidanceLightCommand)}");
            
            ISetGuidanceLightEvents events = new SetGuidanceLightEvents(Connection, setGuidanceLightCmd.Headers.RequestId);

            var result = await HandleSetGuidanceLight(events, setGuidanceLightCmd, cancel);
            await Connection.SendMessageAsync(new SetGuidanceLightCompletion(setGuidanceLightCmd.Headers.RequestId, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var setGuidanceLightcommand = command.IsA<SetGuidanceLightCommand>();

            SetGuidanceLightCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetGuidanceLightCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => SetGuidanceLightCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => SetGuidanceLightCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetGuidanceLightCompletion(setGuidanceLightcommand.Headers.RequestId, new SetGuidanceLightCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICommonDevice Device { get => Provider.Device.IsA<ICommonDevice>(); }
        private IServiceProvider Provider { get; }
        private ICommonServiceClass Common { get; }
        private ILogger Logger { get; }
    }

}
