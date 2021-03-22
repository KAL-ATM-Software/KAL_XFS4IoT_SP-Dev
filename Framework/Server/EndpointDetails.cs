/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using XFS4IoT;

namespace XFS4IoTServer
{
    public class EndpointDetails
    {
        public EndpointDetails(string ServerAddressUri, string ServerAddressWUri, int Port)
        {
            ServerAddressUri.IsNotNullOrWhitespace($"Unexpected server address specified. {nameof(EndpointDetails)}");
            ServerAddressWUri.IsNotNullOrWhitespace($"Unexpected server address for websockets specified. {nameof(EndpointDetails)}");
            this.Port = Port;
            this.SecureConnection = SecureConnection;
            this.ServerAddressUri = ServerAddressUri;
            SecureConnection = !ServerAddressUri.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase);
            this.ServerAddressWUri = ServerAddressWUri;
        }

        public int Port { get; }

        public bool SecureConnection { get; }

        public (Uri Uri, Uri WSUri) ServiceUri(XFSConstants.ServiceClass ServiceName)
        => (
            Uri: new Uri($"{ServerAddressUri}:{Port}/xfs4iot/v1.0/{ServiceName}/"),
            WSUri: new Uri($"{ServerAddressWUri}:{Port}/xfs4iot/v1.0/{ServiceName}/")
        );

        private readonly string ServerAddressUri;
        private readonly string ServerAddressWUri;
    }
}