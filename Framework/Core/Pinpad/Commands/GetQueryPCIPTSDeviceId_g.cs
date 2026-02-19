/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * GetQueryPCIPTSDeviceId_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.PinPad.Commands
{
    //Original name = GetQueryPCIPTSDeviceId
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "PinPad.GetQueryPCIPTSDeviceId")]
    public sealed class GetQueryPCIPTSDeviceIdCommand : Command<MessagePayload>
    {
        public GetQueryPCIPTSDeviceIdCommand()
            : base()
        { }

        public GetQueryPCIPTSDeviceIdCommand(int RequestId, int Timeout)
            : base(RequestId, null, Timeout)
        { }

    }
}
