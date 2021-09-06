/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "PinPad.GetQueryPCIPTSDeviceId")]
    public sealed class GetQueryPCIPTSDeviceIdCommand : Command<GetQueryPCIPTSDeviceIdCommand.PayloadData>
    {
        public GetQueryPCIPTSDeviceIdCommand(int RequestId, GetQueryPCIPTSDeviceIdCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout)
                : base(Timeout)
            {
            }

        }
    }
}
