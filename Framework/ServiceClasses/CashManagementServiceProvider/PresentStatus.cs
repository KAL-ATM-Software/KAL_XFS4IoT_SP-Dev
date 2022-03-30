/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashManagement
{
    [Serializable()]
    public sealed record CashManagementPresentStatus
    {
        public enum PresentStateEnum
        {
            Presented,
            NotPresented,
            Unknown,
        }

        public enum AdditionalBunchesEnum
        {
            None,
            OneMore,
            Unknown,
        }

        public CashManagementPresentStatus(PresentStateEnum PresentState,
                                           AdditionalBunchesEnum AdditionalBunches,
                                           int BunchesRemaining,
                                           StorageCashCountClass ReturnedItems,
                                           StorageCashCountClass TotalReturnedItems,
                                           StorageCashCountClass RemainingItems)
        {
            this.PresentState = PresentState;
            this.AdditionalBunches = AdditionalBunches;
            this.BunchesRemaining = BunchesRemaining;
            this.ReturnedItems = new(ReturnedItems);
            this.TotalReturnedItems = new(TotalReturnedItems);
            this.RemainingItems = new(RemainingItems);
        }

        public CashManagementPresentStatus(CashManagementPresentStatus presentStatus)
        {
            if (presentStatus is null)
            {
                presentStatus = new();
            }
            PresentState = presentStatus.PresentState;
            AdditionalBunches = presentStatus.AdditionalBunches;
            BunchesRemaining = presentStatus.BunchesRemaining;
            ReturnedItems = new(presentStatus.ReturnedItems);
            TotalReturnedItems = new(presentStatus.TotalReturnedItems);
            RemainingItems = new(presentStatus.RemainingItems);
        }

        public CashManagementPresentStatus()
        {
            PresentState = PresentStateEnum.Unknown;
            AdditionalBunches = AdditionalBunchesEnum.Unknown;
            BunchesRemaining = 0;
            ReturnedItems = new();
            TotalReturnedItems = new();
            RemainingItems = new();
        }

        /// <summary>
        /// Supplies the status of the items that were to be presented by the most recent attempt to present or return 
        /// items to the customer. The following values are possible:
        /// 
        /// * ```Presented``` - The items were presented. This status is set as soon as the customer has access to the items.
        /// * ```NotPresented``` - The customer has not had access to the items.
        /// * ```Unknown``` - It is not known if the customer had access to the items.
        /// </summary>
        public PresentStateEnum PresentState { get; set; }


        /// <summary>
        /// Specifies whether or not additional bunches of items are remaining to be presented as a result of the 
        /// most recent operation. The following values are possible:
        /// 
        /// * ```None``` - No additional bunches remain.
        /// * ```OneMore``` - At least one additional bunch remains.
        /// * ```Unknown``` - It is unknown whether additional bunches remain.
        /// </summary>
        public AdditionalBunchesEnum AdditionalBunches { get; set; }

        /// <summary>
        /// If AdditionalBunches is OneMore, specifies the number of additional bunches of items remaining to 
        /// be presented as a result of the current operation.
        /// </summary>
        public int BunchesRemaining { get; set; }

        /// <summary>
        /// A list of counts of banknotes which have been moved to the output position as a result of the most recent operation.
        /// </summary>
        public StorageCashCountClass ReturnedItems { get; set; }

        /// <summary>
        /// Cumulative counts of banknotes which have been moved to the output position. 
        /// This value will be reset when a CashAcceptor.CashInStart, CashAcceptor.CashIn, 
        /// CashAcceptor.CashInEnd, CashManagement.Retract, CashManagement.Reset or CashAcceptor.CashInRollback
        /// command is executed.
        /// </summary>
        public StorageCashCountClass TotalReturnedItems { get; set; }

        /// <summary>
        /// Counts of banknotes on the intermediate stacker or transport which have not been yet moved to the output position.
        /// </summary>
        public StorageCashCountClass RemainingItems { get; set; }
    }
}
