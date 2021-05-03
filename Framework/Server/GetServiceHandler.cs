/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;

namespace Server
{
    [CommandHandler(XFSConstants.ServiceClass.Publisher, typeof(XFS4IoT.Common.Commands.GetServiceCommand))]
    public class GetServiceHandler : ICommandHandler
    {

        public GetServiceHandler(ICommandDispatcher Dispatcher, ILogger Logger)   
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetServiceHandler)} constructor. {nameof(Dispatcher)}");
            Contracts.IsTrue(Dispatcher is ServicePublisher, $"Expected a {nameof(XFS4IoTServer.ServicePublisher)} got {Dispatcher.GetType().FullName}");
            Logger.IsNotNull($"Invalid parameter received in the {nameof(GetServiceHandler)} constructor. {nameof(Logger)}");

            this.ServicePublisher = (ServicePublisher)Dispatcher;
            this.Logger = Logger;
        }

        public ServicePublisher ServicePublisher { get; }
        public ILogger Logger { get; }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            Connection.IsNotNull($"Invalid parameter received in the {nameof(Handle)} method. {nameof(Connection)}");
            command.IsNotNull($"Invalid parameter received in the {nameof(Handle)} method. {nameof(command)}");
            Contracts.IsNotNull(cancel, $"Invalid parameter received in the {nameof(Handle)} method. {nameof(cancel)}");

            XFS4IoT.Common.Commands.GetServiceCommand getServiceCommand = command as XFS4IoT.Common.Commands.GetServiceCommand;
            getServiceCommand.IsNotNull($"Unexpected command received in the {nameof(Handle)} method. {nameof(command)}");

            // For now just return good result and fixed services available
            GetServiceCompletion.PayloadData payLoad = new(GetServiceCompletion.PayloadData.CompletionCodeEnum.Success,
                                                            "ok",
                                                            "KAL",
                                                            from service in ServicePublisher.Services
                                                            select new GetServiceCompletion.PayloadData.ServiceUriDetails(service.WSUri.AbsoluteUri));

            await Connection.SendMessageAsync(new GetServiceCompletion(getServiceCommand.Headers.RequestId, payLoad));
        }

        public async Task HandleError(IConnection Connection, object command, Exception commandErrorException)
        {
            Connection.IsNotNull($"Invalid parameter received in the {nameof(Handle)} method. {nameof(Connection)}");
            command.IsNotNull($"Invalid parameter received in the {nameof(Handle)} method. {nameof(command)}");
            commandErrorException.IsNotNull($"Invalid parameter received in the {nameof(Handle)} method. {nameof(commandErrorException)}");

            GetServiceCommand getServiceCommand = command as GetServiceCommand;
            getServiceCommand.IsNotNull($"Unexpected command received in the {nameof(Handle)} method. {nameof(command)}");

            GetServiceCompletion.PayloadData.CompletionCodeEnum errorCode = GetServiceCompletion.PayloadData.CompletionCodeEnum.InternalError;
            if (commandErrorException.GetType() == typeof(InvalidDataException))
                errorCode = GetServiceCompletion.PayloadData.CompletionCodeEnum.InvalidData;

            GetServiceCompletion.PayloadData payLoad = new(errorCode, commandErrorException.Message);
  
            await Connection.SendMessageAsync(new GetServiceCompletion(getServiceCommand.Headers.RequestId, payLoad));
        }
    }
}
