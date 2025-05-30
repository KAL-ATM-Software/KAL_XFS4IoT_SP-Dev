/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * StartKeyExchange_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = StartKeyExchange
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "KeyManagement.StartKeyExchange")]
    public sealed class StartKeyExchangeCommand : Command<MessagePayload>
    {
        public StartKeyExchangeCommand(int RequestId, int Timeout)
            : base(RequestId, null, Timeout)
        { }

    }
}
