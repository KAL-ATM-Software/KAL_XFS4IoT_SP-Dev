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

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// CashManagementCapabilities
    /// Store device capabilites for the cash management interface for cash devices
    /// </summary>
    public sealed class CashManagementCapabilitiesClass
    {
        [Flags]
        public enum ExchangeTypesEnum
        {
            NotSupported = 0,
            ByHand = 0x0001,
            ToCassettes = 0x0002,
            ClearRecycler = 0x0004,
            DepositInto = 0x0008,
        }

        [Flags]
        public enum ItemInfoTypesEnum
        {
            NotSupported = 0,
            SerialNumber = 0x0001,
            Signature = 0x0002,
            ImageFile = 0x0004,
        }

        public CashManagementCapabilitiesClass(ExchangeTypesEnum ExchangeTypes,
                                               ItemInfoTypesEnum ItemInfoTypes,
                                               bool SafeDoor,
                                               bool CashBox,
                                               bool ClassificationList,
                                               bool PhysicalNoteList)
        {
            this.ExchangeTypes = ExchangeTypes;
            this.ItemInfoTypes = ItemInfoTypes;
            this.SafeDoor = SafeDoor;
            this.CashBox = CashBox;
            this.ClassificationList = ClassificationList;
            this.PhysicalNoteList = PhysicalNoteList;
        }

        /// <summary>
        /// Supported exchange types
        /// </summary>
        public ExchangeTypesEnum ExchangeTypes { get; init; }

        /// <summary>
        /// Specifies the types of information that can be retrieved through the CashManagement.GetItemInfo command.
        /// </summary>
        public ItemInfoTypesEnum ItemInfoTypes { get; init; }

        /// <summary>
        /// Specifies whether or not the  CashManagement.OpenSafeDoor command is supported.
        /// </summary>
        public bool SafeDoor { get; init; }

        /// <summary>
        /// This field is only applicable to teller type devices. 
        /// It specifies whether or not tellers have been assigned a cash box.
        /// </summary>
        public bool CashBox { get; init; }

        /// <summary>
        /// Specifies whether the device has the capability to maintain a classification list of serial numbers as well as
        /// supporting the associated operations. This can either be TRUE if the device has the capability or FALSE if it does not.
        /// </summary>
        public bool ClassificationList { get; init; }

        /// <summary>
        /// Specifies whether the Service supports note number lists on physical cash units.
        /// This can either be TRUE if the Service has the capability or FALSE if it does not.
        /// </summary>
        public bool PhysicalNoteList { get; init; }
    }
}
