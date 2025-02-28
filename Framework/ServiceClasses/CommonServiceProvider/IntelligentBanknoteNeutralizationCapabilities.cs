/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using static XFS4IoTFramework.Common.IBNSCapabilitiesClass;

namespace XFS4IoTFramework.Common
{
    public sealed class IBNSCapabilitiesClass(
        IBNSCapabilitiesClass.ModeEnum Mode,
        bool GasSensor = false,
        bool LightSensor = false,
        bool SeismicSensor = false,
        bool SafeIntrusionDetection = false,
        bool ExternalDryContactStatusBox = false,
        bool RealTimeClock = false,
        bool PhysicalStorageUnitsAccessControl = false,
        Dictionary<IBNSCapabilitiesClass.CustomInputEnum, CustomInputClass> CustomInputStatus = null,
        Dictionary<string, CustomInputClass> VendorSpecificCustomInputStatus = null
        )
    {
        public enum CustomInputEnum
        {
            Maintenance,
            TriggerNeutralization,
            DisableGas,
            DisableSeismic,
            DisableSafeDoorAttack
        }

        public enum ModeEnum
        {
            Autonomous,
            ClientControlled,
            VendorSpecific,
        }

        /// <summary>
        /// Indicates the operating mode of the banknote neutralization.
        /// * ```autonomous``` - The banknote neutralization autonomously activates the surveillance as soon as the safe door is closed and locked and to deactivate it when it detects a legal entry.
        /// * ```clientControlled``` - The Client Application is in charge of arming and disarming the system.
        /// * ```vendorSpecific``` - Neither autonomous nor programmable.The mode is vendor specific.
        /// </summary>
        public ModeEnum Mode = Mode;

        /// <summary>
        /// Indicates the presence and management of a gas sensor in the banknote neutralization.
        /// </summary>
        public bool GasSensor = GasSensor;

        /// <summary>
        /// Indicates the presence and management of a light sensor in the banknote neutralization.
        /// </summary>
        public bool LightSensor = LightSensor;

        /// <summary>
        /// Indicates the presence and management of a seismic sensor in the banknote neutralization.
        /// </summary>
        public bool SeismicSensor = SeismicSensor;

        /// <summary>
        /// Indicates the presence and management of a safe intrusion detection in the banknote neutralization.
        /// </summary>
        public bool SafeIntrusionDetection = SafeIntrusionDetection;

        /// <summary>
        /// Indicates the presence and management of an external dry Contact Box in the banknote neutralization.
        /// </summary>
        public bool ExternalDryContactStatusBox = ExternalDryContactStatusBox;

        /// <summary>
        /// Indicates the presence and management of a Real Time Clock in the banknote neutralization.
        /// </summary>
        public bool RealTimeClock = RealTimeClock;

        /// <summary>
        /// Indicates the presence of a physical access to the Storage Units and controlled by the banknote neutralization.
        /// </summary>
        public bool PhysicalStorageUnitsAccessControl = PhysicalStorageUnitsAccessControl;

        public sealed class CustomInputClass(bool ActiveInput = false)
        {
            /// <summary>
            /// This input is configured and active.
            /// </summary>
            public bool ActiveInput { get; init; } = ActiveInput;
        }

        /// <summary>
        /// A list of state for one or more custom input accordingly.
        /// </summary>
        public Dictionary<CustomInputEnum, CustomInputClass> CustomInputStatus { get; init; } = CustomInputStatus;

        /// <summary>
        /// A list of vendor specific state for one or more custom input accordingly
        /// </summary>
        public Dictionary<string, CustomInputClass> VendorSpecificCustomInputStatus { get; init; } = VendorSpecificCustomInputStatus;
    }
}
