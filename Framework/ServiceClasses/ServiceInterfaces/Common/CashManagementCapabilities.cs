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
            OutDefault = 0x000001,
            OutLeft = 0x000002,
            OutRight = 0x000004,
            OutCenter = 0x000008,
            OutTop = 0x000010,
            OutBottom = 0x000020,
            OutFront = 0x000040,
            OutRear = 0x000080,
            InDefault = 0x000100,
            InLeft = 0x000200,
            InRight = 0x000400,
            InCenter = 0x000800,
            InTop = 0x001000,
            InBottom = 0x002000,
            InFront = 0x004000,
            InRear = 0x008000,
        }

        [Flags]
        public enum ExchangeTypesEnum
        {
            NotSupported = 0,
            ByHand = 0x0001,
        }

        [Flags]
        public enum ItemInfoTypesEnum
        {
            NotSupported = 0,
            SerialNumber = 0x0001,
            Signature = 0x0002,
            ImageFile = 0x0004,
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
            Retract = 0x0001,
            Transport = 0x0002,
            Stacker = 0x0004,
            Reject = 0x0008,
            ItemCassette = 0x0010,
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
            Present = 0x0001,
            Retract = 0x0002,
            Reject = 0x0004,
            ItemCassette = 0x0008,
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
            Present = 0x0001,
            Retract = 0x0002,
            Reject = 0x0004,
            ItemCassette = 0x0008,
        }


        public CashManagementCapabilitiesClass(PositionEnum Positions,
                                               bool ShutterControl,
                                               RetractAreaEnum RetractAreas,
                                               RetractTransportActionEnum RetractTransportActions,
                                               RetractStackerActionEnum RetractStackerActions,
                                               ExchangeTypesEnum ExchangeTypes,
                                               ItemInfoTypesEnum ItemInfoTypes,
                                               bool SafeDoor,
                                               bool CashBox,
                                               bool ClassificationList)
        {
            this.Positions = Positions;
            this.ShutterControl = ShutterControl;
            this.RetractAreas = RetractAreas;
            this.RetractTransportActions = RetractTransportActions;
            this.RetractStackerActions = RetractStackerActions;
            this.ExchangeTypes = ExchangeTypes;
            this.ItemInfoTypes = ItemInfoTypes;
            this.SafeDoor = SafeDoor;
            this.CashBox = CashBox;
            this.ClassificationList = ClassificationList;
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
    }
}
