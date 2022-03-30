/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.CashManagement
{
    public sealed class ItemClassificationListClass
    {
        public ItemClassificationListClass()
        {
            Version = string.Empty;
            ItemClassifications = new();
        }

        /// <summary>
        /// The version number of the classification
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The list of elements for the item classification
        /// </summary>
        public List<ItemClassificationClass> ItemClassifications { get; init; }
    }

    public sealed class ItemClassificationClass
    {
        public ItemClassificationClass(string SerialNumber, 
                                       string Currency,
                                       double Value, 
                                       NoteLevelEnum Level)
        {
            this.SerialNumber = SerialNumber;
            this.Currency = Currency;
            this.Value = Value;
            this.Level = Level;
        }

        /// <summary>
        /// This string defines the serial number or a mask of serial numbers of one element with the 
        /// defined currency and value.
        /// </summary>
        public string SerialNumber { get; init; }

        /// <summary>
        /// ISO 4217 currency identifier
        /// </summary>
        public string Currency { get; init; }

        /// <summary>
        /// Absolute value of a cash item or items. May be a floating point value to allow for coins and notes which have
        /// a value which is not a whole multiple of the currency unit.
        /// If applied to a storage unit, this applies to all contents, may be 0 if mixed and may only be modified in
        /// an exchange state if applicable.
        /// </summary>
        public double Value { get; init; }

        /// <summary>
        /// Item type
        /// </summary>
        public NoteLevelEnum Level { get; init; }
    }
}
