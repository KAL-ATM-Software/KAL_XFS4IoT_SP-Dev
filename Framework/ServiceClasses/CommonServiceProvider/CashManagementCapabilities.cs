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
using XFS4IoT;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// CashManagementCapabilities
    /// Store device capabilites for the cash management interface for cash devices
    /// </summary>
    public sealed class CashManagementCapabilitiesClass
    {
        /// <summary>
        /// Common output shutter position
        /// default - Output location is determined by Service.
        /// left - Present items to left side of device.
        /// right - Present items to right side of device.
        /// center - Present items to center output position.
        /// top - Present items to the top output position.
        /// bottom - Present items to the bottom output position.
        /// front - Present items to the front output position.
        /// rear - Present items to the rear output position.
        /// reject - Reject bin is used as output location.
        /// </summary>
        [Flags]
        public enum OutputPositionEnum
        {
            NotSupported = 0,
            Default =  1 << 0,
            Left = 1 << 1,
            Right = 1 << 2,
            Center = 1 << 3,
            Top = 1 << 4,
            Bottom = 1 << 5,
            Front = 1 << 6,
            Rear = 1 << 7,
        }

        /// <summary>
        /// Common shutter position - prefix 'Out' is an output position and 'In' is an input position 
        /// default - Output location is determined by Service.
        /// left - Present items to left side of device.
        /// right - Present items to right side of device.
        /// center - Present items to center output position.
        /// top - Present items to the top output position.
        /// bottom - Present items to the bottom output position.
        /// front - Present items to the front output position.
        /// rear - Present items to the rear output position.
        /// reject - Reject bin is used as output location.
        /// </summary>
        [Flags]
        public enum PositionEnum
        {
            NotSupported = 0,
            OutDefault = 1 << 0,
            OutLeft = 1 << 1,
            OutRight = 1 << 2,
            OutCenter = 1 << 3,
            OutTop = 1 << 4,
            OutBottom = 1 << 5,
            OutFront = 1 << 6,
            OutRear = 1 << 7,
            InDefault = 1 << 8,
            InLeft = 1 << 9,
            InRight = 1 << 10,
            InCenter = 1 << 11,
            InTop = 1 << 12,
            InBottom = 1 << 13,
            InFront = 1 << 14,
            InRear = 1 << 15,
        }

        [Flags]
        public enum ExchangeTypesEnum
        {
            NotSupported = 0,
            ByHand = 1 << 0,
        }

        [Flags]
        public enum ItemInfoTypesEnum
        {
            NotSupported = 0,
            SerialNumber = 1 << 0,
            Signature = 1 << 1,
            ImageFile = 1 << 2,
        }

        /// <summary>
        /// Retract - The items may be retracted to a retract cash unit.
        /// Transport - The items may be retracted to the transport.
        /// Stacker - The items may be retracted to the intermediate stacker.
        /// Reject - The items may be retracted to a reject cash unit.
        /// ItemCassette - The items may be retracted to the item cassettes, i.e. cassettes that can be dispensed from.
        /// Default - The item may be retracted to the default position.
        /// </summary>
        [Flags]
        public enum RetractAreaEnum
        {
            Default = 0,
            Retract = 1 << 0,
            Transport = 1 << 1,
            Stacker = 1 << 2,
            Reject = 1 << 3,
            ItemCassette = 1 << 4,
        }

        /// <summary>
        /// Present - The items may be presented.
        /// Retract - The items may be moved to a retract cash unit.
        /// Reject - The items may be moved to a reject bin.
        /// ItemCassette - The items may be moved to the item cassettes, i.e. cassettes that can be dispensed from.
        /// </summary>
        [Flags]
        public enum RetractTransportActionEnum
        {
            NotSupported = 0,
            Present = 1 << 0,
            Retract = 1 << 1,
            Reject = 1 << 2,
            ItemCassette = 1 << 3,
        }

        /// <summary>
        /// Present - The items may be presented.
        /// Retract - The items may be moved to a retract cash unit.
        /// Reject - The items may be moved to a reject bin.
        /// ItemCassette - The items may be moved to the item cassettes, i.e. cassettes that can be dispensed from.
        /// </summary>
        [Flags]
        public enum RetractStackerActionEnum
        {
            NotSupported = 0,
            Present = 1 << 0,
            Retract = 1 << 1,
            Reject = 1 << 2,
            ItemCassette = 1 << 3,
        }

        public enum TypeEnum
        {
            TellerBill,
            SelfServiceBill,
            TellerCoin,
            SelfServiceCoin
        }


        public CashManagementCapabilitiesClass(PositionEnum Positions,
                                               bool ShutterControl,
                                               RetractAreaEnum RetractAreas,
                                               RetractTransportActionEnum RetractTransportActions,
                                               RetractStackerActionEnum RetractStackerActions,
                                               ExchangeTypesEnum ExchangeTypes,
                                               ItemInfoTypesEnum ItemInfoTypes,
                                               bool CashBox,
                                               bool ClassificationList,
                                               Dictionary<string, BanknoteItem> AllBanknoteItems)
        {
            this.Positions = Positions;
            this.ShutterControl = ShutterControl;
            this.RetractAreas = RetractAreas;
            this.RetractTransportActions = RetractTransportActions;
            this.RetractStackerActions = RetractStackerActions;
            this.ExchangeTypes = ExchangeTypes;
            this.ItemInfoTypes = ItemInfoTypes;
            this.CashBox = CashBox;
            this.ClassificationList = ClassificationList;
            this.AllBanknoteItems = AllBanknoteItems;
        }

        /// <summary>
        /// Details of banknote item
        /// </summary>
        public sealed class BanknoteItem
        {
            public BanknoteItem(int NoteId,
                                string Currency,
                                double Value,
                                int Release,
                                bool Enabled)
            {
                this.NoteId = NoteId;
                this.Currency = Currency;
                this.Value = Value;
                this.Release = Release;
                this.Enabled = Enabled;
            }
            public BanknoteItem(BanknoteItem Item)
            {
                Item.IsNotNull("Null copy constractor passed in. " + nameof(BanknoteItem));
                NoteId = Item.NoteId;
                Currency = Item.Currency;
                Value = Item.Value;
                Release = Item.Release;
                Enabled = Item.Enabled;
            }

            /// <summary>
            /// A unique number identifying a single cash item. 
            /// Each unique combination of the other properties will have a different noteID.
            /// </summary>
            public int NoteId { get; init; }

            /// <summary>
            /// ISO 4217 currency.
            /// </summary>
            public string Currency { get; init; }

            /// <summary>
            /// Absolute value of all contents, 0 if mixed. May only be modified in an exchange state if applicable. May be 
            /// a floating point value to allow for coins and notes which have a value which is not a whole multiple
            /// of the currency unit.
            /// </summary>
            public double Value { get; init; }

            /// <summary>
            /// The release of the cash item. The higher this number is, the newer the release.
            /// </summary>
            public int Release { get; init; }

            /// <summary>
            /// This item is enabled to be recognized
            /// </summary>
            public bool Enabled { get; set; }
        }

        /// <summary>
        /// Supported positions
        /// </summary>
        public PositionEnum Positions { get; init; }

        /// <summary>
        /// If set to TRUE the shutter is controlled implicitly by the Service. 
        /// If set to FALSE the shutter must be controlled explicitly by the application
        /// using the Dispenser.OpenShutter and the Dispenser.CloseShutter commands.
        /// This property is always true if the device has no shutter. This field applies to all shutters and all positions.
        /// </summary>
        public bool ShutterControl { get; init; }

        /// <summary>
        /// Retract areas support of this device
        /// </summary>
        public RetractAreaEnum RetractAreas { get; init; }

        /// <summary>
        /// Action support on retracting cash to the transport
        /// </summary>
        public RetractTransportActionEnum RetractTransportActions { get; init; }

        /// <summary>
        /// Action support on retracting cash to the stacker
        /// </summary>
        public RetractStackerActionEnum RetractStackerActions { get; init; }

        /// <summary>
        /// Supported exchange types
        /// </summary>
        public ExchangeTypesEnum ExchangeTypes { get; init; }

        /// <summary>
        /// Specifies the types of information that can be retrieved through the CashManagement.GetItemInfo command.
        /// </summary>
        public ItemInfoTypesEnum ItemInfoTypes { get; init; }

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
        /// Cash item can be recognised and handled by the device component
        /// </summary>
        public Dictionary<string, BanknoteItem> AllBanknoteItems { get; init; }
    }
}
