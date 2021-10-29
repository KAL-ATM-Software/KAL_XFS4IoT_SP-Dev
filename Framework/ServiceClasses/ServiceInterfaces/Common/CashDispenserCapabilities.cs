/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// CashDispenserCapabilities
    /// Store device capabilites for the cash dispenser device
    /// </summary>
    public sealed class CashDispenserCapabilitiesClass
    {
        /// <summary>
        /// tellerBill - The Dispenser is a Teller Bill Dispenser.
        /// selfServiceBill - The Dispenser is a Self-Service Bill Dispenser.
        /// tellerCoin - The Dispenser is a Teller Coin Dispenser.
        /// selfServiceCoin - The Dispenser is a Self-Service Coin Dispenser.
        /// </summary>
        public enum TypeEnum
        {
            tellerBill,
            selfServiceBill,
            tellerCoin,
            selfServiceCoin,
        }

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
            Default = 0x0001,
            Left = 0x0002,
            Right = 0x0004,
            Center = 0x0008,
            Top = 0x0010,
            Bottom = 0x0020,
            Front = 0x0040,
            Rear = 0x0080,
        }

        /// <summary>
        /// FromCashUnit - The Dispenser can dispense items from the cash units to the intermediate stacker while there are items on the transport.
        /// ToCashUnit - The Dispenser can retract items to the cash units while there are items on the intermediate stacker.
        /// ToTransport - The Dispenser can retract items to the transport while there are items on the intermediate stacker.
        /// ToStacker - The Dispenser can dispense items from the cash units to the intermediate stacker while there are already items on the
        /// intermediate stacker that have not been in customer access.Items remaining on the stacker from a previous dispense
        /// may first need to be rejected explicitly by the application if they are not to be presented.
        /// </summary>
        public enum MoveItemEnum
        {
            FromCashUnit,
            ToCashUnit,
            ToTransport,
            ToStacker,
        }

        public CashDispenserCapabilitiesClass(TypeEnum Type,
                                              int MaxDispenseItems,
                                              bool ShutterControl,
                                              CashManagementCapabilitiesClass.RetractAreaEnum RetractAreas,
                                              CashManagementCapabilitiesClass.RetractTransportActionEnum RetractTransportActions,
                                              CashManagementCapabilitiesClass.RetractStackerActionEnum RetractStackerActions,
                                              bool IntermediateStacker,
                                              bool ItemsTakenSensor,
                                              OutputPositionEnum OutputPositions,
                                              Dictionary<MoveItemEnum, bool> MoveItems)
        {
            this.Type = Type;
            this.MaxDispenseItems = MaxDispenseItems;
            this.ShutterControl = ShutterControl;
            this.RetractAreas = RetractAreas;
            this.RetractTransportActions = RetractTransportActions;
            this.RetractStackerActions = RetractStackerActions;
            this.IntermediateStacker = IntermediateStacker;
            this.ItemsTakenSensor = ItemsTakenSensor;
            this.OutputPositions = OutputPositions;
            this.MoveItems = MoveItems;
        }

        /// <summary>
        /// Supplies the type of CDM
        /// </summary>
        public TypeEnum Type { get; init; }

        /// <summary>
        /// Supplies the maximum number of items that can be dispensed in a single dispense operation. 
        /// </summary>
        public int MaxDispenseItems { get; init; }

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
        public CashManagementCapabilitiesClass.RetractAreaEnum RetractAreas { get; init; }

        /// <summary>
        /// Action support on retracting cash to the transport
        /// </summary>
        public CashManagementCapabilitiesClass.RetractTransportActionEnum RetractTransportActions { get; init; }

        /// <summary>
        /// Action support on retracting cash to the stacker
        /// </summary>
        public CashManagementCapabilitiesClass.RetractStackerActionEnum RetractStackerActions { get; init; }

        /// <summary>
        /// Specifies whether or not the Dispenser supports stacking items to an intermediate position before the items are moved to the exit position.
        /// </summary>
        public bool IntermediateStacker { get; init; }

        /// <summary>
        ///  Specifies whether the Dispenser can detect when items at the exit position are taken by the user. 
        /// </summary>
        public bool ItemsTakenSensor { get; init; }

        /// <summary>
        /// Supported output positions
        /// </summary>
        public OutputPositionEnum OutputPositions { get; init; }

        /// <summary>
        /// Move items from stacker or transport to the unit
        /// </summary>
        public Dictionary<MoveItemEnum, bool> MoveItems { get; init; }
    }
}
