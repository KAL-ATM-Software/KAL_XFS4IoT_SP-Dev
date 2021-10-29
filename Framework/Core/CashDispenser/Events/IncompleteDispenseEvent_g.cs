/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * IncompleteDispenseEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashDispenser.Events
{

    [DataContract]
    [Event(Name = "CashDispenser.IncompleteDispenseEvent")]
    public sealed class IncompleteDispenseEvent : Event<IncompleteDispenseEvent.PayloadData>
    {

        public IncompleteDispenseEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(Dictionary<string, double> Currencies = null, Dictionary<string, int> Values = null, Dictionary<string, double> CashBox = null)
                : base()
            {
                this.Currencies = Currencies;
                this.Values = Values;
                this.CashBox = CashBox;
            }

            /// <summary>
            /// List of currency and amount combinations for denomination requests or output. There will be one entry for 
            /// each currency in the denomination. The property name is the ISO 4217 currency identifier. This list can be 
            /// omitted on a request if _values_ specifies the entire request.
            /// </summary>
            [DataMember(Name = "currencies")]
            public Dictionary<string, double> Currencies { get; init; }

            /// <summary>
            /// This list specifies the number of items to take from the cash units. If specified in a request, the output 
            /// denomination must include these items.
            /// 
            /// The property name is storage unit object name as stated by the [Storage.GetStorage](#storage.getstorage)
            /// command. The value of the entry is the number of items to take from that unit.
            /// </summary>
            [DataMember(Name = "values")]
            public Dictionary<string, int> Values { get; init; }

            /// <summary>
            /// Only applies to Teller Dispensers. Amount to be paid from the teller’s cash box.
            /// </summary>
            [DataMember(Name = "cashBox")]
            public Dictionary<string, double> CashBox { get; init; }

        }

    }
}
