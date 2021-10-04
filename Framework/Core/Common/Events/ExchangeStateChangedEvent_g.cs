/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * ExchangeStateChangedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Common.Events
{

    [DataContract]
    [Event(Name = "Common.ExchangeStateChangedEvent")]
    public sealed class ExchangeStateChangedEvent : UnsolicitedEvent<ExchangeStateChangedEvent.PayloadData>
    {

        public ExchangeStateChangedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(ExchangeEnum? Exchange = null)
                : base()
            {
                this.Exchange = Exchange;
            }

            [DataMember(Name = "exchange")]
            public ExchangeEnum? Exchange { get; init; }

        }

    }
}
