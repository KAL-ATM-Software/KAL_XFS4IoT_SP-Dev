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
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.CashDispenser
{
    /// <summary>
    /// DenominationBase
    /// Representing output data of the Denominate and PresentStatus
    /// </summary>
    public class Denomination
    {
        public Denomination(Dictionary<string, double> CurrencyAmounts,
                            Dictionary<string, int> Values = null)
        {
            this.CurrencyAmounts = CurrencyAmounts;
            if (Values is not null)
            {
                // Copy only value positive
                this.Values = Values.Where(v => v.Value > 0).ToDictionary(v => v.Key, v => v.Value);
            }
        }

        /// <summary>
        /// Currencies to use for dispensing cash
        /// </summary>
        public Dictionary<string, double> CurrencyAmounts { get; set; }

        /// <summary>
        /// Key is cash unit name and the value is the number of cash to be used
        /// </summary>
        public Dictionary<string, int> Values { get; set; }

    }
}
