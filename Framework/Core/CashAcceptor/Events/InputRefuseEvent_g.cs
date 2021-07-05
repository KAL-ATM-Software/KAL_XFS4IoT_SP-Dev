/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * InputRefuseEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashAcceptor.Events
{

    [DataContract]
    [Event(Name = "CashAcceptor.InputRefuseEvent")]
    public sealed class InputRefuseEvent : Event<InputRefuseEvent.PayloadData>
    {

        public InputRefuseEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(ReasonEnum? Reason = null)
                : base()
            {
                this.Reason = Reason;
            }

            public enum ReasonEnum
            {
                CashInUnitFull,
                InvalidBill,
                NoBillsToDeposit,
                DepositFailure,
                CommonInputComponentFailure,
                StackerFull,
                ForeignItemsDetected,
                InvalidBunch,
                Counterfeit,
                LimitOverTotalItems,
                LimitOverAmount
            }

            /// <summary>
            /// Reason for refusing a part of the amount. Following values are possible:
            /// 
            /// \"cashInUnitFull\": Cash unit is full.
            /// 
            /// \"invalidBill\": Recognition of the items took place, but one or more of the items are invalid.
            /// 
            /// \"noBillsToDeposit\": There are no items in the input area.
            /// 
            /// \"depositFailure\": A deposit has failed for a reason not covered by the other reasons and the failure is not a fatal hardware problem, for example failing to pick an item from the input area.
            /// 
            /// \"commonInputComponentFailure\": Failure of a common input component which is shared by all cash units.
            /// 
            /// \"stackerFull\": The intermediate stacker is full.
            /// 
            /// \"foreignItemsDetected\": Foreign items have been detected in the input position.
            /// 
            /// \"invalidBunch\": Recognition of the items did not take place. The bunch of notes inserted is invalid, e.g. it is too large or was inserted incorrectly.
            /// 
            /// \"counterfeit\": One or more counterfeit items have been detected and refused. This is only applicable where notes are not classified as level 2 and the device is capable of differentiating between invalid and counterfeit items.
            /// 
            /// \"limitOverTotalItems\": Number of items count exceeded the limitation set with the CashAcceptor.SetCashInLimit command.
            /// 
            /// \"limitOverAmount\": Amount exceeded the limitation set with the CashAcceptor.SetCashInLimit command.
            /// </summary>
            [DataMember(Name = "reason")]
            public ReasonEnum? Reason { get; private set; }

        }

    }
}
