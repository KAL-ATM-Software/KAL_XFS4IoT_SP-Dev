/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.0")]
    [Command(Name = "KeyManagement.GetKeyDetail")]
    public sealed class GetKeyDetailCommand : Command<GetKeyDetailCommand.PayloadData>
    {
        public GetKeyDetailCommand()
            : base()
        { }

        public GetKeyDetailCommand(int RequestId, GetKeyDetailCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string KeyName = null)
                : base()
            {
                this.KeyName = KeyName;
            }

            /// <summary>
            /// Name of the key for which detailed information is requested.
            /// If this property is null, detailed information about all the keys in the encryption module is returned.
            /// <example>Key01</example>
            /// </summary>
            [DataMember(Name = "keyName")]
            public string KeyName { get; init; }

        }
    }
}
