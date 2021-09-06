/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * GetKeyDetail_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = GetKeyDetail
    [DataContract]
    [Command(Name = "KeyManagement.GetKeyDetail")]
    public sealed class GetKeyDetailCommand : Command<GetKeyDetailCommand.PayloadData>
    {
        public GetKeyDetailCommand(int RequestId, GetKeyDetailCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string KeyName = null)
                : base(Timeout)
            {
                this.KeyName = KeyName;
            }

            /// <summary>
            /// Name of the key for which detailed information is requested.
            /// </summary>
            [DataMember(Name = "keyName")]
            public string KeyName { get; init; }

        }
    }
}
