/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetPresentStatus_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.GetPresentStatus")]
    public sealed class GetPresentStatusCompletion : Completion<GetPresentStatusCompletion.PayloadData>
    {
        public GetPresentStatusCompletion(int RequestId, GetPresentStatusCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, PositionEnum? Position = null, PresentStateEnum? PresentState = null, AdditionalBunchesEnum? AdditionalBunches = null, int? BunchesRemaining = null, ReturnedItemsClass ReturnedItems = null, TotalReturnedItemsClass TotalReturnedItems = null, RemainingItemsClass RemainingItems = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Position = Position;
                this.PresentState = PresentState;
                this.AdditionalBunches = AdditionalBunches;
                this.BunchesRemaining = BunchesRemaining;
                this.ReturnedItems = ReturnedItems;
                this.TotalReturnedItems = TotalReturnedItems;
                this.RemainingItems = RemainingItems;
            }

            public enum PositionEnum
            {
                Left,
                Right,
                Center,
                Top,
                Bottom,
                Front,
                Rear
            }

            /// <summary>
            /// Specifies the output position. Following values are possible:
            /// 
            /// "left": Left output position.
            /// 
            /// "right": Right output position.
            /// 
            /// "center": Center output position.
            /// 
            /// "top": Top output position.
            /// 
            /// "bottom": Bottom output position.
            /// 
            /// "front": Front output position.
            /// 
            /// "rear": Rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

            public enum PresentStateEnum
            {
                Presented,
                NotPresented,
                Unknown
            }

            /// <summary>
            /// Supplies the status of the items that were to be presented by the most recent attempt to present or return items to the customer. Following values are possible:
            /// 
            /// "presented": The items were presented. This status is set as soon as the customer has access to the items.
            /// 
            /// "notPresented": The customer has not had access to the items.
            /// 
            /// "unknown": It is not known if the customer had access to the items.
            /// </summary>
            [DataMember(Name = "presentState")]
            public PresentStateEnum? PresentState { get; init; }

            public enum AdditionalBunchesEnum
            {
                None,
                OneMore,
                Unknown
            }

            /// <summary>
            /// Specifies whether or not additional bunches of items are remaining to be presented as a result of the most recent operation. Following values are possible:
            /// 
            /// "none": No additional bunches remain.
            /// 
            /// "oneMore": At least one additional bunch remains.
            /// 
            /// "unknown": It is unknown whether additional bunches remain.
            /// </summary>
            [DataMember(Name = "additionalBunches")]
            public AdditionalBunchesEnum? AdditionalBunches { get; init; }

            /// <summary>
            /// If *additionalBunches* is "oneMore", specifies the number of additional bunches of items remaining to be presented as a result of the current operation. 
            /// If the number of additional bunches is at least one, but the precise number is unknown, *bunchesRemaining* will be 255 (TODO: Check if there is a better way to represent this state). 
            /// For any other value of *additionalBunches*, *bunchesRemaining* will be zero.
            /// </summary>
            [DataMember(Name = "bunchesRemaining")]
            public int? BunchesRemaining { get; init; }

            [DataContract]
            public sealed class ReturnedItemsClass
            {
                public ReturnedItemsClass(List<NoteNumberClass> NoteNumber = null)
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
            /// Array holding a list of banknote numbers which have been moved to the output position as a result of the most recent operation.
            /// </summary>
            [DataMember(Name = "returnedItems")]
            public ReturnedItemsClass ReturnedItems { get; init; }

            [DataContract]
            public sealed class TotalReturnedItemsClass
            {
                public TotalReturnedItemsClass(List<NoteNumberClass> NoteNumber = null)
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
            /// Array of cumulative banknote numbers which have been moved to the output position. 
            /// This value will be reset when the CashInStart, CashIn, CashInEnd, Retract, Reset or CashInRollback command is executed.
            /// </summary>
            [DataMember(Name = "totalReturnedItems")]
            public TotalReturnedItemsClass TotalReturnedItems { get; init; }

            [DataContract]
            public sealed class RemainingItemsClass
            {
                public RemainingItemsClass(List<NoteNumberClass> NoteNumber = null)
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
            /// Array of banknote numbers on the intermediate stacker or transport which have not been yet moved to the output position.
            /// </summary>
            [DataMember(Name = "remainingItems")]
            public RemainingItemsClass RemainingItems { get; init; }

        }
    }
}
