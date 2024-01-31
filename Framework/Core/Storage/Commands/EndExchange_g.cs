/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * EndExchange_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Storage.Commands
{
    //Original name = EndExchange
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Storage.EndExchange")]
    public sealed class EndExchangeCommand : Command<MessagePayload>
    {
        public EndExchangeCommand(int RequestId, int Timeout)
            : base(RequestId, null, Timeout)
        { }

    }
}
