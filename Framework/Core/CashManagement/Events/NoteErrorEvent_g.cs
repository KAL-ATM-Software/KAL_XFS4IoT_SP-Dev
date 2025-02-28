/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * NoteErrorEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashManagement.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "CashManagement.NoteErrorEvent")]
    public sealed class NoteErrorEvent : Event<NoteErrorEvent.PayloadData>
    {

        public NoteErrorEvent(int RequestId, PayloadData Payload)
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
                DoubleNote,
                LongNote,
                SkewedNote,
                IncorrectCount,
                NotesTooClose,
                OtherNoteError,
                ShortNote
            }

            /// <summary>
            /// The reason for the note detection error. Following values are possible:
            /// 
            /// * ```doubleNote``` - A double note has been detected.
            /// * ```longNote``` - A long note has been detected.
            /// * ```skewedNote``` - A skewed note has been detected.
            /// * ```incorrectCount``` - An item counting error has occurred.
            /// * ```notesTooClose``` - Notes have been detected as being too close.
            /// * ```otherNoteError``` - An item error not covered by the other values has been detected.
            /// * ```shortNote``` - A short note has been detected.
            /// </summary>
            [DataMember(Name = "reason")]
            public ReasonEnum? Reason { get; init; }

        }

    }
}
