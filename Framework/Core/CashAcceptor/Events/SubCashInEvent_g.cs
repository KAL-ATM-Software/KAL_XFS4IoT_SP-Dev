/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * SubCashInEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashAcceptor.Events
{

    [DataContract]
    [Event(Name = "CashAcceptor.SubCashInEvent")]
    public sealed class SubCashInEvent : Event<SubCashInEvent.PayloadData>
    {

        public SubCashInEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(List<NoteNumberClass> NoteNumber = null)
                : base()
            {
                this.NoteNumber = NoteNumber;
            }

            [DataContract]
            public sealed class NoteNumberClass
            {
                public NoteNumberClass(int? NoteID = null, int? Count = null)
                {
                    this.NoteID = NoteID;
                    this.Count = Count;
                }

                /// <summary>
                /// Identification of note type. The Note ID represents the note identifiers reported by the *CashAcceptor.BanknoteTypes* command. 
                /// If this value is zero then the note type is unknown.
                /// </summary>
                [DataMember(Name = "noteID")]
                public int? NoteID { get; init; }

                /// <summary>
                /// Actual count of cash items. The value is incremented each time cash items are moved to a cash unit. 
                /// In the case of recycle cash units this count is decremented as defined in the description of the *logicalCount* field.
                /// </summary>
                [DataMember(Name = "count")]
                public int? Count { get; init; }

            }

            /// <summary>
            /// Array of banknote numbers the cash unit contains.
            /// </summary>
            [DataMember(Name = "noteNumber")]
            public List<NoteNumberClass> NoteNumber { get; init; }

        }

    }
}
