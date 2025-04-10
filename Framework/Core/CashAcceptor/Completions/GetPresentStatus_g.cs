/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashAcceptor.GetPresentStatus")]
    public sealed class GetPresentStatusCompletion : Completion<GetPresentStatusCompletion.PayloadData>
    {
        public GetPresentStatusCompletion(int RequestId, GetPresentStatusCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CashManagement.OutputPositionEnum? Position = null, PresentStateEnum? PresentState = null, AdditionalBunchesEnum? AdditionalBunches = null, int? BunchesRemaining = null, CashManagement.StorageCashCountsClass ReturnedItems = null, CashManagement.StorageCashCountsClass TotalReturnedItems = null, CashManagement.StorageCashCountsClass RemainingItems = null)
                : base()
            {
                this.Position = Position;
                this.PresentState = PresentState;
                this.AdditionalBunches = AdditionalBunches;
                this.BunchesRemaining = BunchesRemaining;
                this.ReturnedItems = ReturnedItems;
                this.TotalReturnedItems = TotalReturnedItems;
                this.RemainingItems = RemainingItems;
            }

            [DataMember(Name = "position")]
            public CashManagement.OutputPositionEnum? Position { get; init; }

            public enum PresentStateEnum
            {
                Presented,
                NotPresented,
                Unknown
            }

            /// <summary>
            /// Supplies the status of the items that were to be presented by the most recent attempt to present or return
            /// items to the customer. The following values are possible:
            /// 
            /// * ```presented``` - The items were presented. This status is set as soon as the customer has access to the items.
            /// * ```notPresented``` - The customer has not had access to the items.
            /// * ```unknown``` - It is not known if the customer had access to the items.
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
            /// Specifies whether additional bunches of items are remaining to be presented as a result of the
            /// most recent operation. The following values are possible:
            /// 
            /// * ```none``` - No additional bunches remain.
            /// * ```oneMore``` - At least one additional bunch remains.
            /// * ```unknown``` - It is unknown whether additional bunches remain.
            /// </summary>
            [DataMember(Name = "additionalBunches")]
            public AdditionalBunchesEnum? AdditionalBunches { get; init; }

            /// <summary>
            /// If *additionalBunches* is *oneMore*, specifies the number of additional bunches of items remaining to
            /// be presented as a result of the current operation.
            /// This property is null if any of the following are true:
            /// * If the number of additional bunches is at least one, but the precise number is unknown.
            /// * *additionalBunches* is not *oneMore*.
            /// </summary>
            [DataMember(Name = "bunchesRemaining")]
            [DataTypes(Minimum = 0)]
            public int? BunchesRemaining { get; init; }

            /// <summary>
            /// Array holding a list of counts of banknotes which have been moved to the output position as a result of the
            /// most recent operation.
            /// </summary>
            [DataMember(Name = "returnedItems")]
            public CashManagement.StorageCashCountsClass ReturnedItems { get; init; }

            /// <summary>
            /// Array of cumulative counts of banknotes which have been moved to the output position.
            /// This value will be reset when a *CashAcceptor.CashInStart*, *CashAcceptor.CashIn*,
            /// *CashAcceptor.CashInEnd*, *CashManagement.Retract*, *CashManagement.Reset* or *CashAcceptor.CashInRollback*
            /// command is executed.
            /// </summary>
            [DataMember(Name = "totalReturnedItems")]
            public CashManagement.StorageCashCountsClass TotalReturnedItems { get; init; }

            /// <summary>
            /// Array of counts of banknotes on the intermediate stacker or transport which have not been yet moved to the
            /// output position.
            /// </summary>
            [DataMember(Name = "remainingItems")]
            public CashManagement.StorageCashCountsClass RemainingItems { get; init; }

        }
    }
}
