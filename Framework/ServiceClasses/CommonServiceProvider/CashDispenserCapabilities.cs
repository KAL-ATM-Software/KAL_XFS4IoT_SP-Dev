/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    public sealed class CashDispenserCapabilitiesClass(
        CashManagementCapabilitiesClass.TypeEnum Type,
        int MaxDispenseItems,
        bool ShutterControl,
        CashManagementCapabilitiesClass.RetractAreaEnum RetractAreas,
        CashManagementCapabilitiesClass.RetractTransportActionEnum RetractTransportActions,
        CashManagementCapabilitiesClass.RetractStackerActionEnum RetractStackerActions,
        bool IntermediateStacker,
        bool ItemsTakenSensor,
        CashManagementCapabilitiesClass.OutputPositionEnum OutputPositions,
        CashDispenserCapabilitiesClass.MoveItemEnum MoveItems)
    {
        /// <summary>
        /// FromCashUnit - The Dispenser can dispense items from the cash units to the intermediate stacker while there are items on the transport.
        /// ToCashUnit - The Dispenser can retract items to the cash units while there are items on the intermediate stacker.
        /// ToTransport - The Dispenser can retract items to the transport while there are items on the intermediate stacker.
        /// ToStacker - The Dispenser can dispense items from the cash units to the intermediate stacker while there are already items on the
        /// intermediate stacker that have not been in customer access.Items remaining on the stacker from a previous dispense
        /// may first need to be rejected explicitly by the application if they are not to be presented.
        /// </summary>
        [Flags]
        public enum MoveItemEnum
        {
            NotSupported = 0,
            FromCashUnit = 1 << 0,
            ToCashUnit = 1 << 1,
            ToTransport = 1 << 2,
            ToStacker = 1 << 3,
        }

        /// <summary>
        /// Supplies the type of CDM
        /// </summary>
        public CashManagementCapabilitiesClass.TypeEnum Type { get; init; } = Type;

        /// <summary>
        /// Supplies the maximum number of items that can be dispensed in a single dispense operation. 
        /// </summary>
        public int MaxDispenseItems { get; init; } = MaxDispenseItems;

        /// <summary>
        /// If set to TRUE the shutter is controlled implicitly by the Service. 
        /// If set to FALSE the shutter must be controlled explicitly by the application
        /// using the Dispenser.OpenShutter and the Dispenser.CloseShutter commands.
        /// This property is always true if the device has no shutter. This field applies to all shutters and all positions.
        /// </summary>
        public bool ShutterControl { get; init; } = ShutterControl;

        /// <summary>
        /// Retract areas support of this device
        /// </summary>
        public CashManagementCapabilitiesClass.RetractAreaEnum RetractAreas { get; init; } = RetractAreas;

        /// <summary>
        /// Action support on retracting cash to the transport
        /// </summary>
        public CashManagementCapabilitiesClass.RetractTransportActionEnum RetractTransportActions { get; init; } = RetractTransportActions;

        /// <summary>
        /// Action support on retracting cash to the stacker
        /// </summary>
        public CashManagementCapabilitiesClass.RetractStackerActionEnum RetractStackerActions { get; init; } = RetractStackerActions;

        /// <summary>
        /// Specifies whether or not the Dispenser supports stacking items to an intermediate position before the items are moved to the exit position.
        /// </summary>
        public bool IntermediateStacker { get; init; } = IntermediateStacker;

        /// <summary>
        ///  Specifies whether the Dispenser can detect when items at the exit position are taken by the user. 
        /// </summary>
        public bool ItemsTakenSensor { get; init; } = ItemsTakenSensor;

        /// <summary>
        /// Supported output positions
        /// </summary>
        public CashManagementCapabilitiesClass.OutputPositionEnum OutputPositions { get; init; } = OutputPositions;

        /// <summary>
        /// Move items from stacker or transport to the unit
        /// </summary>
        public MoveItemEnum MoveItems { get; init; } = MoveItems;
    }
}
