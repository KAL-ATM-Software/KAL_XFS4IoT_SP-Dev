/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetCashInStatus_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.GetCashInStatus")]
    public sealed class GetCashInStatusCompletion : Completion<GetCashInStatusCompletion.PayloadData>
    {
        public GetCashInStatusCompletion(int RequestId, GetCashInStatusCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, StatusEnum? Status = null, int? NumOfRefused = null, NoteNumberListClass NoteNumberList = null, UnfitNoteNumberListClass UnfitNoteNumberList = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Status = Status;
                this.NumOfRefused = NumOfRefused;
                this.NoteNumberList = NoteNumberList;
                this.UnfitNoteNumberList = UnfitNoteNumberList;
            }

            public enum StatusEnum
            {
                Ok,
                Rollback,
                Active,
                Retract,
                Unknown,
                Reset
            }

            /// <summary>
            /// Status of the currently active or most recently ended cash-in transaction. Following values are possible:
            /// 
            /// "ok": The cash-in transaction is complete and has ended with a CashAcceptor.CashInEnd command call.
            /// 
            /// "rollback": The cash-in transaction was has ended with a CashAcceptor.CashInRollback command call.
            /// 
            /// "active": There is a cash-in transaction active. See the CashAcceptor.CashInStart command description for a definition of an active cash-in transaction.
            /// 
            /// "retract": The cash-in transaction ended with a Retract command call.
            /// 
            /// "unknown": The state of the cash-in transaction is unknown. This status is also set if the noteNumberList details are not known or are not reliable.
            /// 
            /// "reset": The cash-in transaction ended with a Reset command call.
            /// </summary>
            [DataMember(Name = "status")]
            public StatusEnum? Status { get; init; }

            /// <summary>
            /// Specifies the number of items refused during the currently active or most recently ended cash-in transaction period.
            /// </summary>
            [DataMember(Name = "numOfRefused")]
            public int? NumOfRefused { get; init; }

            [DataContract]
            public sealed class NoteNumberListClass
            {
                public NoteNumberListClass(List<NoteNumberClass> NoteNumber = null)
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

            /// <summary>
            /// List of banknote types that were inserted, identified and accepted during the currently active or most recently ended cash-in transaction period. 
            /// If items have been rolled back (status is "rollback") they will be included in this list. If status is "retract" or "reset" 
            /// then identified and accepted items moved to Cash-In or Recycle cash units are included in this list, but items moved to the Retract or Reject cash units 
            /// are not included. noteNumberList includes any level 2 or level 3 notes, and all level 4 fit and unfit notes.
            /// </summary>
            [DataMember(Name = "noteNumberList")]
            public NoteNumberListClass NoteNumberList { get; init; }

            [DataContract]
            public sealed class UnfitNoteNumberListClass
            {
                public UnfitNoteNumberListClass(List<NoteNumberClass> NoteNumber = null)
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

            /// <summary>
            /// List of level 4 unfit banknote types that were inserted, identified and accepted during the currently active or most recently ended cash-in transaction period.  
            /// If items have been rolled back (status is "rollback") they will be included in this list. If status is "retract" or "reset" then identified 
            /// and accepted items moved to Cash-In units are included in this list, but items moved to the Retract or Reject cash units are not included. 
            /// </summary>
            [DataMember(Name = "unfitNoteNumberList")]
            public UnfitNoteNumberListClass UnfitNoteNumberList { get; init; }

        }
    }
}
