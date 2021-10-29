/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * IncompleteRetractEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashManagement.Events
{

    [DataContract]
    [Event(Name = "CashManagement.IncompleteRetractEvent")]
    public sealed class IncompleteRetractEvent : Event<IncompleteRetractEvent.PayloadData>
    {

        public IncompleteRetractEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(Dictionary<string, StorageCashInClass> ItemNumberList = null, ReasonEnum? Reason = null)
                : base()
            {
                this.ItemNumberList = ItemNumberList;
                this.Reason = Reason;
            }

            /// <summary>
            /// The values in this structure report the amount and number of each denomination that were successfully moved
            /// during the command prior to the failure.
            /// </summary>
            [DataMember(Name = "itemNumberList")]
            public Dictionary<string, StorageCashInClass> ItemNumberList { get; init; }

            public enum ReasonEnum
            {
                RetractFailure,
                RetractAreaFull,
                ForeignItemsDetected,
                InvalidBunch
            }

            /// <summary>
            /// The reason for not having retracted items. Following values are possible:
            /// 
            /// * ```retractFailure``` - The retract has partially failed for a reason not covered by the other reasons 
            /// listed in this event, for example failing to pick an item to be retracted.
            /// * ```retractAreaFull``` - The storage area specified in the command payload has become full during the 
            /// retract operation.
            /// * ```foreignItemsDetected``` - Foreign items have been detected.
            /// * ```invalidBunch``` - An invalid bunch of items has been detected, e.g. it is too large or could not be
            /// processed.
            /// </summary>
            [DataMember(Name = "reason")]
            public ReasonEnum? Reason { get; init; }

        }

    }
}
