/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PowerManagement interface.
 * PowerManagementSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.PowerManagement
{

    public enum BatteryStatusEnum
    {
        Full,
        Low,
        Operational,
        Critical,
        Failure
    }


    public enum BatteryChargingStatusEnum
    {
        Charging,
        Discharging,
        NotCharging
    }


    [DataContract]
    public sealed class PowerInfoClass
    {
        public PowerInfoClass(PowerInStatusEnum? PowerInStatus = null, PowerOutStatusEnum? PowerOutStatus = null, BatteryStatusEnum? BatteryStatus = null, BatteryChargingStatusEnum? BatteryChargingStatus = null)
        {
            this.PowerInStatus = PowerInStatus;
            this.PowerOutStatus = PowerOutStatus;
            this.BatteryStatus = BatteryStatus;
            this.BatteryChargingStatus = BatteryChargingStatus;
        }

        public enum PowerInStatusEnum
        {
            Powering,
            NoPower
        }

        /// <summary>
        /// Specify the input power or mains power status. Specified as one of the following:
        /// * ```powering``` - The input power source is live and supplying power to the power supply module.
        /// * ```noPower``` - The input power source is not supplying power to the power supply module.
        /// 
        /// This property may be null in [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
        /// </summary>
        [DataMember(Name = "powerInStatus")]
        public PowerInStatusEnum? PowerInStatus { get; init; }

        public enum PowerOutStatusEnum
        {
            Powering,
            NoPower
        }

        /// <summary>
        /// Specify the output power status. Specified as one of the following:
        /// * ```powering``` - The power supply module is supplying power to the connected devices.
        /// * ```noPower``` - The power supply module is not supplying power to the connected devices.
        /// 
        /// This property may be null in [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
        /// </summary>
        [DataMember(Name = "powerOutStatus")]
        public PowerOutStatusEnum? PowerOutStatus { get; init; }

        [DataMember(Name = "batteryStatus")]
        public BatteryStatusEnum? BatteryStatus { get; init; }

        [DataMember(Name = "batteryChargingStatus")]
        public BatteryChargingStatusEnum? BatteryChargingStatus { get; init; }

    }


    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(PowerInfoClass Info = null, int? PowerSaveRecoveryTime = null)
        {
            this.Info = Info;
            this.PowerSaveRecoveryTime = PowerSaveRecoveryTime;
        }

        [DataMember(Name = "info")]
        public PowerInfoClass Info { get; init; }

        /// <summary>
        /// Specifies the actual number of seconds required by the device to resume its normal operational state from
        /// the current power saving mode. This value is 0 if the power saving mode has not been activated. This property
        /// is null if power save control is not supported or [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged..
        /// <example>10</example>
        /// </summary>
        [DataMember(Name = "powerSaveRecoveryTime")]
        [DataTypes(Minimum = 0)]
        public int? PowerSaveRecoveryTime { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(bool? PowerSaveControl = null, bool? BatteryRechargeable = null)
        {
            this.PowerSaveControl = PowerSaveControl;
            this.BatteryRechargeable = BatteryRechargeable;
        }

        /// <summary>
        /// Specifies whether power saving control is available.
        /// </summary>
        [DataMember(Name = "powerSaveControl")]
        public bool? PowerSaveControl { get; init; }

        /// <summary>
        /// Specifies whether the battery is rechargeable or not. 
        /// </summary>
        [DataMember(Name = "batteryRechargeable")]
        public bool? BatteryRechargeable { get; init; }

    }


}
