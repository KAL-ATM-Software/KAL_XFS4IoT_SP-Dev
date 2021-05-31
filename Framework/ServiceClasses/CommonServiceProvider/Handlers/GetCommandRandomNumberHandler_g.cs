/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * GetCommandRandomNumberHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Common, typeof(GetCommandRandomNumberCommand))]
    public partial class GetCommandRandomNumberHandler : ICommandHandler
    {
        public GetCommandRandomNumberHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetCommandRandomNumberHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetCommandRandomNumberHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICommonDevice>();

            Common = Provider.IsA<ICommonServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetCommandRandomNumberHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var getCommandRandomNumberCmd = command.IsA<GetCommandRandomNumberCommand>($"Invalid parameter in the GetCommandRandomNumber Handle method. {nameof(GetCommandRandomNumberCommand)}");
            getCommandRandomNumberCmd.Headers.RequestId.HasValue.IsTrue();

            IGetCommandRandomNumberEvents events = new GetCommandRandomNumberEvents(Connection, getCommandRandomNumberCmd.Headers.RequestId.Value);

            var result = await HandleGetCommandRandomNumber(events, getCommandRandomNumberCmd, cancel);
            await Connection.SendMessageAsync(new GetCommandRandomNumberCompletion(getCommandRandomNumberCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var getCommandRandomNumbercommand = command.IsA<GetCommandRandomNumberCommand>();
            getCommandRandomNumbercommand.Headers.RequestId.HasValue.IsTrue();

            GetCommandRandomNumberCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetCommandRandomNumberCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => GetCommandRandomNumberCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => GetCommandRandomNumberCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetCommandRandomNumberCompletion(getCommandRandomNumbercommand.Headers.RequestId.Value, new GetCommandRandomNumberCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICommonDevice Device { get => Provider.Device.IsA<ICommonDevice>(); }
        private IServiceProvider Provider { get; }
        private ICommonServiceClass Common { get; }
        private ILogger Logger { get; }
    }

}
