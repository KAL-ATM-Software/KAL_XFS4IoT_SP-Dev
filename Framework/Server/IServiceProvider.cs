/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTServer
{
    public interface IServiceProvider : ICommandDispatcher
    {
        Task RunAsync();
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

    }
}