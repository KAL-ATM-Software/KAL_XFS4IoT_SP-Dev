/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * InitializedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.KeyManagement.Events
{

    [DataContract]
    [Event(Name = "KeyManagement.InitializedEvent")]
    public sealed class InitializedEvent : UnsolicitedEvent<InitializedEvent.PayloadData>
    {

        public InitializedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string Ident = null, string Key = null)
                : base()
            {
                this.Ident = Ident;
                this.Key = Key;
            }

            /// <summary>
            /// The Base64 encoded value of the ID key formatted. if not required, this property is omitted.
            /// </summary>
            [DataMember(Name = "ident")]
            public string Ident { get; init; }

            /// <summary>
            /// The Base64 encoded value of the encryption key. if not required, this property is omitted.
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

        }

    }
}
