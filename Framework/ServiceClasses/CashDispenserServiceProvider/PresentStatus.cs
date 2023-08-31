/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.CashDispenser
{
    /// <summary>
    /// PresentStatus
    /// Representing the last dispense / presented status
    /// </summary>
    [Serializable()]
    public class CashDispenserPresentStatus
    {
        public CashDispenserPresentStatus(PresentStatusEnum Status = PresentStatusEnum.Unknown,
                                          Denomination LastDenomination = null,
                                          string Token = null)
        {
            this.Status = Status;
            this.LastDenomination = LastDenomination;
            this.DispenseToken = Token;
        }

        /// <summary>
        /// Last dispense status
        /// </summary>
        public enum PresentStatusEnum
        {
            Presented,
            NotPresented,
            Unknown
        }

        /// <summary>
        /// Supplies the status of the last dispense or present operation
        /// </summary>
        public PresentStatusEnum Status { get; set; }

        /// <summary>
        /// Key is cash unit name and the value is the number of cash to be used
        /// </summary>
        public Denomination LastDenomination { get; set; }

        /// <summary>
        /// Last E2E dispense authorisation token used.
        /// </summary>
        /// <remarks>
        /// Note that this is the incoming token that authorised the last dispense operation. This
        /// is _not_ the outgoing token that validates the GetPresentStatus result. That has to be 
        /// calculated later. 
        /// </remarks>
        public string DispenseToken { get; set; }
    }
}
