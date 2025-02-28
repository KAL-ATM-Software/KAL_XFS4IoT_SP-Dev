﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.ServicePublisher.Commands;
using XFS4IoT.ServicePublisher.Completions;

namespace Server
{
    [CommandHandler(XFSConstants.ServiceClass.Publisher, typeof(XFS4IoT.ServicePublisher.Commands.GetServicesCommand))]
    public class GetServiceHandler : ICommandHandler
    {

        public GetServiceHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger Logger)   
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetServiceHandler)} constructor. {nameof(Dispatcher)}");

            this.ServicePublisher = Dispatcher.IsA<ServicePublisher>($"Expected a {nameof(XFS4IoTServer.ServicePublisher)} got {Dispatcher.GetType().FullName}");
            this.Logger = Logger.IsNotNull($"Invalid parameter received in the {nameof(GetServiceHandler)} constructor. {nameof(Logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter received in the {nameof(GetServiceHandler)} constructor. {nameof(Connection)}");
        }

        public IConnection Connection { get; }
        public ServicePublisher ServicePublisher { get; }
        public ILogger Logger { get; }

        public async Task Handle(object command, CancellationToken cancel)
        {
            Connection.IsNotNull($"Invalid parameter received in the {nameof(Handle)} method. {nameof(Connection)}");
            command.IsNotNull($"Invalid parameter received in the {nameof(Handle)} method. {nameof(command)}");
            Contracts.IsNotNull(cancel, $"Invalid parameter received in the {nameof(Handle)} method. {nameof(cancel)}");

            XFS4IoT.ServicePublisher.Commands.GetServicesCommand getServiceCommand = command as XFS4IoT.ServicePublisher.Commands.GetServicesCommand;
            getServiceCommand.IsNotNull($"Unexpected command received in the {nameof(Handle)} method. {nameof(command)}");
            getServiceCommand.Header.RequestId.HasValue.IsTrue();

            // For now just return good result and fixed services available
            GetServicesCompletion.PayloadData payload = new("KAL",
                                                            ServicePublisher.Services.Select(c => new XFS4IoT.ServicePublisher.ServiceClass(c.WSUri.AbsoluteUri)).ToList());

            await Connection.SendMessageAsync(new GetServicesCompletion(getServiceCommand.Header.RequestId.Value, payload, MessageHeader.CompletionCodeEnum.Success, null));
        }

        public async Task HandleError(object command, Exception commandErrorException)
        {
            Connection.IsNotNull($"Invalid parameter received in the {nameof(Handle)} method. {nameof(Connection)}");
            command.IsNotNull($"Invalid parameter received in the {nameof(Handle)} method. {nameof(command)}");
            commandErrorException.IsNotNull($"Invalid parameter received in the {nameof(Handle)} method. {nameof(commandErrorException)}");

            GetServicesCommand getServiceCommand = command as GetServicesCommand;
            getServiceCommand.IsNotNull($"Unexpected command received in the {nameof(Handle)} method. {nameof(command)}");
            getServiceCommand.Header.RequestId.HasValue.IsTrue();

            MessageHeader.CompletionCodeEnum errorCode = MessageHeader.CompletionCodeEnum.InternalError;
            if (commandErrorException.GetType() == typeof(InvalidDataException))
                errorCode = MessageHeader.CompletionCodeEnum.InvalidData;
  
            await Connection.SendMessageAsync(new GetServicesCompletion(getServiceCommand.Header.RequestId.Value, null, errorCode, commandErrorException.Message));
        }
    }
}
