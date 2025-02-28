/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashManagement
{
    [Serializable()]
    public sealed class CashInStatusClass
    {
        public enum StatusEnum
        {
            Ok,
            Rollback,
            Active,
            Retract,
            Unknown,
            Reset,
        }

        public CashInStatusClass()
        {
            Status = StatusEnum.Unknown;
            NumOfRefusedItems = 0;
            CashCounts = new();
        }

        /// <summary>
        /// Status of the currently active or most recently ended cash-in transaction. The following values are possible:
        /// 
        /// * ```Ok``` - The cash-in transaction is complete and has ended with CashAcceptor.CashInEnd.
        /// * ```Rollback``` - The cash-in transaction ended with CashAcceptor.CashInRollback.
        /// * ```Active``` - There is a cash-in transaction active. See the CashAcceptor.CashInStart command description 
        /// for a definition of an active cash-in transaction.
        /// * ```Retract``` - The cash-in transaction ended with CashManagement.Retract.
        /// * ```Unknown``` - The state of the cash-in transaction is unknown. This status is also set if the ItemCounts 
        /// details are not known or are not reliable.
        /// * ```Reset``` - The cash-in transaction ended with CashManagement.Reset.
        /// </summary>
        public StatusEnum Status { get; set; }

        /// <summary>
        /// Specifies the number of items refused during the currently active or most recently ended cash-in
        /// transaction period.
        /// </summary>
        public int NumOfRefusedItems { get; set; }

        /// <summary>
        /// List of banknote types that were inserted, identified, and accepted during the currently active or most 
        /// recently ended cash-in transaction period. If items have been rolled back (status is rollback) they will 
        /// be included in this list.
        /// Includes any identified notes.
        /// </summary>
        public StorageCashCountClass CashCounts { get; set; }
    }
}
