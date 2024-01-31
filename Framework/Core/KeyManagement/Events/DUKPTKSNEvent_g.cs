/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * DUKPTKSNEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.KeyManagement.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "KeyManagement.DUKPTKSNEvent")]
    public sealed class DUKPTKSNEvent : Event<DUKPTKSNEvent.PayloadData>
    {

        public DUKPTKSNEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string Key = null, List<byte> Ksn = null)
                : base()
            {
                this.Key = Key;
                this.Ksn = Ksn;
            }

            /// <summary>
            /// Specifies the name of the DUKPT Key derivation key.
            /// <example>Key01</example>
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            /// <summary>
            /// The KSN.
            /// <example>S1NORGF0YQ==</example>
            /// </summary>
            [DataMember(Name = "ksn")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> Ksn { get; init; }

        }

    }
}
