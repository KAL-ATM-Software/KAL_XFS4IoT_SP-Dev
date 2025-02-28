/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * NonceClearedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Common.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "Common.NonceClearedEvent")]
    public sealed class NonceClearedEvent : UnsolicitedEvent<NonceClearedEvent.PayloadData>
    {

        public NonceClearedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string ReasonDescription = null)
                : base()
            {
                this.ReasonDescription = ReasonDescription;
            }

            /// <summary>
            /// Optional text describing why the nonce was cleared. The value of this text should not be relied on.
            /// <example>Nonce cleared by timeout</example>
            /// </summary>
            [DataMember(Name = "reasonDescription")]
            public string ReasonDescription { get; init; }

        }

    }
}
