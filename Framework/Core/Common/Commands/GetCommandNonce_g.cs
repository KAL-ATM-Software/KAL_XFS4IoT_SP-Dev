/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * GetCommandNonce_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Common.Commands
{
    //Original name = GetCommandNonce
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Common.GetCommandNonce")]
    public sealed class GetCommandNonceCommand : Command<MessagePayload>
    {
        public GetCommandNonceCommand(int RequestId, int Timeout)
            : base(RequestId, null, Timeout)
        { }

    }
}
