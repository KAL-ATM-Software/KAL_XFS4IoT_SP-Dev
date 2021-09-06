/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
            /// optional text describing why the nonce was cleared. The value of this text shouldn't be relied on.
            /// </summary>
            [DataMember(Name = "reasonDescription")]
            public string ReasonDescription { get; init; }

        }

    }
}
