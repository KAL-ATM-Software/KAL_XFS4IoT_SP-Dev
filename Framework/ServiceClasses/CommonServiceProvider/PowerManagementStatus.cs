/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace XFS4IoTFramework.Common
{
    public sealed class PowerManagementStatusClass(
        PowerManagementStatusClass.PowerInfoClass PowerInfo = null,
        int PowerSaveRecoveryTime = -1) : StatusBase
    {
        public sealed class PowerInfoClass(
            PowerInfoClass.PoweringStatusEnum PowerInStatus,
            PowerInfoClass.PoweringStatusEnum PowerOutStatus,
            PowerInfoClass.BatteryStatusEnum BatteryStatus = PowerInfoClass.BatteryStatusEnum.NotSupported,
            PowerInfoClass.BatteryChargingStatusEnum BatteryChargingStatus = PowerInfoClass.BatteryChargingStatusEnum. NotSupported
            ) : StatusBase
        {
            public enum PoweringStatusEnum
            {
                Powering,
                NotPower,
            }

            public enum BatteryStatusEnum
            {
                Full,
                Low,
                Operational,
                Critical,
                Failure,
                NotSupported,
            }

            public enum BatteryChargingStatusEnum
            {
                Charging,
                Discharging,
                NotCharging,
                NotSupported,
            }

            /// <summary>
            /// Specify the input power or mains power status. Specified as one of the following:
            /// * ```Powering``` - The input power source is live and supplying power to the power supply module.
            /// * ```NoPower``` - The input power source is not supplying power to the power supply module.
            /// * ```NotSupported``` - The input power source is not supported.
            /// </summary>
            public PoweringStatusEnum PowerInStatus
            {
                get { return powerInStatus; }
                set
                {
                    if (powerInStatus != value)
                    {
                        powerInStatus = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private PoweringStatusEnum powerInStatus = PowerInStatus;

            /// <summary>
            /// Specify the output power status. Specified as one of the following:
            /// * ```Powering``` - The input power source is live and supplying power to the power supply module.
            /// * ```NoPower``` - The input power source is not supplying power to the power supply module.
            /// * ```NotSupported``` - The input power source is not supported.
            /// </summary>
            public PoweringStatusEnum PowerOutStatus
            {
                get { return powerOutStatus; }
                set
                {
                    if (powerOutStatus != value)
                    {
                        powerOutStatus = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private PoweringStatusEnum powerOutStatus = PowerOutStatus;

            /// <summary>
            /// The charge level of the battery. Specified as one of the following:
            /// * ```Full``` - The battery charge level is full, either the battery is new or fully charged for a rechargeable battery.
            /// * ```Low``` - Although the battery level is still operational, this is an advance notice which should trigger a maintenance schedule without delay.
            /// * ```Operational``` - The charge level is nominally between the levels "full" and "low".
            /// * ```Critical``` - The battery level is no longer operational, this is an alert which should trigger maintenance without delay.Consider that the device may also not be powered properly.
            /// * ```Failure``` - A battery fault detected. The device powered by the battery is no longer powered properly. Immediate maintenance should be performed.This may be a failure from the battery charging module for a rechargeable battery.
            /// * ```NotSupported``` - The device does not support battery status.
            /// </summary>
            public BatteryStatusEnum BatteryStatus
            {
                get { return batteryStatus; }
                set
                {
                    if (batteryStatus != value)
                    {
                        batteryStatus = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private BatteryStatusEnum batteryStatus = BatteryStatus;

            /// <summary>
            /// The charging status of the battery. This will be null if the battery is not rechargeable. Specified as one of the following:
            /// * ```Charging``` - The battery is charging power.When the battery is fully charged, the state changes to "notCharging" and the property BatteryStatus reports "full".   
            /// * ```Discharging``` - The battery is discharging power.
            /// * ```NotCharging``` - The battery is not charging power.
            /// * ```NotSupported``` - The device does not support battery charging status.
            /// </summary>
            public BatteryChargingStatusEnum BatteryChargingStatus
            {
                get { return batteryChargingStatus; }
                set
                {
                    if (batteryChargingStatus != value)
                    {
                        batteryChargingStatus = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private BatteryChargingStatusEnum batteryChargingStatus = BatteryChargingStatus;
        }

        /// <summary>
        /// Information that can be generically applied to module providing power ranging from a simple non-rechargeable 
        /// battery to a more complex device such as a UPS.
        /// </summary>
        public PowerInfoClass PowerInfo { get; init; } = PowerInfo;

        /// <summary>
        /// Specifies the actual number of seconds required by the device to resume its normal operational state from
        /// the current power saving mode.
        /// This value is 0 if the power saving mode has not been activated.
        /// if this value is negative, the device doesn't support power save mode.
        /// </summary>
        public int PowerSaveRecoveryTime
        {
            get { return powerSaveRecoveryTime; }
            set
            {
                if (powerSaveRecoveryTime != value)
                {
                    powerSaveRecoveryTime = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int powerSaveRecoveryTime = PowerSaveRecoveryTime;
    }
}
