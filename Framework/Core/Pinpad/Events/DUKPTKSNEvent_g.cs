/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * DUKPTKSNEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.PinPad.Events
{

    [DataContract]
    [Event(Name = "PinPad.DUKPTKSNEvent")]
    public sealed class DUKPTKSNEvent : Event<DUKPTKSNEvent.PayloadData>
    {

        public DUKPTKSNEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string Key = null, string Ksn = null)
                : base()
            {
                this.Key = Key;
                this.Ksn = Ksn;
            }

            /// <summary>
            /// Specifies the name of the DUKPT Key derivation key. 
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            /// <summary>
            /// Base64 encoded structure that contains the KSN.
            /// </summary>
            [DataMember(Name = "ksn")]
            public string Ksn { get; init; }

        }

    }
}
