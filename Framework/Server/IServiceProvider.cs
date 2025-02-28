/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTServer
{
    public interface IServiceProvider : ICommandDispatcher
    {
        string Name { get; }

        Uri Uri { get; }

        Uri WSUri { get; }

        /// <summary>
        /// The device class the integrates this service with real hardware. 
        /// </summary>
        IDevice Device { get; }

        /// <summary>
        /// Broadcast an unsolicited event to all connections.
        /// </summary>
        /// <param name="payload">The XFS payload for the message</param>
        Task BroadcastEvent(object payload);

        /// <summary>
        /// Broadcast an unsolicited event to specified connections.
        /// </summary>
        /// <param name="connections">The connections to broadcast to</param>
        /// <param name="payload">The XFS payload for the message</param>
        Task BroadcastEvent(IEnumerable<IConnection> connections, object payload);

        /// <summary>
        /// Set JSON schema dependency injection to the services.
        /// </summary>
        /// <param name="JsonSchemaValidator"></param>
        void SetJsonSchemaValidator(IJsonSchemaValidator JsonSchemaValidator);

        /// <summary>
        /// The list of commands and events device class supproted and associated with versions.
        /// The framework only supports one latest version of commands and events.
        /// </summary>

        void SetMessagesSupported(Dictionary<string, MessageTypeInfo> MessagesSupported);
        Dictionary<string, MessageTypeInfo> GetMessagesSupported();
    }
}