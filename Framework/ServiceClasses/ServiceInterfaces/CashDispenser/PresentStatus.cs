/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    public class PresentStatus
    {
        public PresentStatus(PresentStatusEnum Status = PresentStatusEnum.Unknown,
                             Denomination LastDenomination = null,
                             string Token = null)
        {
            this.Status = Status;
            this.LastDenomination = LastDenomination;
            this.Token = Token;
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
        /// E2E token used last
        /// </summary>
        public string Token { get; set; }
    }
}
