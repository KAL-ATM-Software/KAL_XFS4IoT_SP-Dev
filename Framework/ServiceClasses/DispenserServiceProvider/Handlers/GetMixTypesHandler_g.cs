/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * GetMixTypesHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Dispenser.Commands;
using XFS4IoT.Dispenser.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Dispenser
{
    [CommandHandler(XFSConstants.ServiceClass.Dispenser, typeof(GetMixTypesCommand))]
    public partial class GetMixTypesHandler : ICommandHandler
    {
        public GetMixTypesHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetMixTypesHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetMixTypesHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IDispenserDevice>();

            Dispenser = Provider.IsA<IDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetMixTypesHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var getMixTypesCmd = command.IsA<GetMixTypesCommand>($"Invalid parameter in the GetMixTypes Handle method. {nameof(GetMixTypesCommand)}");
            getMixTypesCmd.Header.RequestId.HasValue.IsTrue();

            IGetMixTypesEvents events = new GetMixTypesEvents(Connection, getMixTypesCmd.Header.RequestId.Value);

            var result = await HandleGetMixTypes(events, getMixTypesCmd, cancel);
            await Connection.SendMessageAsync(new GetMixTypesCompletion(getMixTypesCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var getMixTypescommand = command.IsA<GetMixTypesCommand>();
            getMixTypescommand.Header.RequestId.HasValue.IsTrue();

            GetMixTypesCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => GetMixTypesCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetMixTypesCompletion(getMixTypescommand.Header.RequestId.Value, new GetMixTypesCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IDispenserDevice Device { get => Provider.Device.IsA<IDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private IDispenserServiceClass Dispenser { get; }
        private ILogger Logger { get; }
    }

}
