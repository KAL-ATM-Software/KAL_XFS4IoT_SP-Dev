/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * GenerateAuthenticationEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Crypto
{
    internal class GenerateAuthenticationEvents : CryptoEvents, IGenerateAuthenticationEvents
    {

        public GenerateAuthenticationEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

        public async Task DUKPTKSNEvent(XFS4IoT.KeyManagement.Events.DUKPTKSNEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.KeyManagement.Events.DUKPTKSNEvent(requestId, Payload));

    }
}
