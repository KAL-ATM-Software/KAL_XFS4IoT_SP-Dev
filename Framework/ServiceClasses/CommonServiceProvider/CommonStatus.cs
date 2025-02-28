﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Common
{
    public sealed class CommonStatusClass(
        CommonStatusClass.DeviceEnum Device,
        CommonStatusClass.PositionStatusEnum DevicePosition,
        int PowerSaveRecoveryTime, // Obsolete property
        CommonStatusClass.AntiFraudModuleEnum AntiFraudModule,
        CommonStatusClass.ExchangeEnum Exchange,
        CommonStatusClass.EndToEndSecurityEnum EndToEndSecurity) : StatusBase
    {
        public enum PositionStatusEnum
        {
            InPosition,
            NotInPosition,
            Unknown
        }

        public enum ExchangeEnum
        {
            NotSupported,
            Active,
            Inactive
        }

        public enum DeviceEnum
        {
            Online,
            Offline,
            PowerOff,
            NoDevice,
            HardwareError,
            UserError,
            DeviceBusy,
            FraudAttempt,
            PotentialFraud
        }

        public enum AntiFraudModuleEnum
        {
            NotSupported,
            Ok,
            Inoperable,
            DeviceDetected,
            Unknown
        }

        public enum EndToEndSecurityEnum
        {
            NotSupported,
            NotEnforced,
            NotConfigured,
            Enforced
        }

        public enum ErrorEventIdEnum
        {
            Hardware,
            Software,
            User,
            FraudAttempt
        }

        public enum ErrorActionEnum
        {
            Reset,
            SoftwareError,
            Configuration,
            Clear,
            Maintenance,
            Suspend
        }

        /// <summary>
        /// Specifies the state of the device. Following values are possible:
        /// 
        /// * ```online``` - The device is online. This is returned when the device is present and operational.
        /// * ```offline``` - The device is offline (e.g. the operator has taken the device offline by turning a switch).
        /// * ```powerOff``` - The device is powered off or physically not connected.
        /// * ```noDevice``` - The device is not intended to be there, e.g. this type of self service machine does not contain such a device or it is internally not configured.
        /// * ```hardwareError``` - The device is inoperable due to a hardware error.
        /// * ```userError``` - The device is present but a person is preventing proper device operation.
        /// * ```deviceBusy``` - The device is busy and unable to process a command at this time.
        /// * ```fraudAttempt``` - The device is present but is inoperable because it has detected a fraud attempt.
        /// * ```potentialFraud``` - The device has detected a potential fraud attempt and is capable of remaining in service. In this case the application should make the decision as to whether to take the device offline.
        /// </summary>
        public DeviceEnum Device 
        {
            get { return device; } 
            set
            {
                if (device != value)
                {
                    device = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private DeviceEnum device = Device;

        /// <summary>
        /// Position of the device. Following values are possible:
        /// 
        /// * ```inPosition``` - The device is in its normal operating position, or is fixed in place and cannot be moved.
        /// * ```notInPosition``` - The device has been removed from its normal operating position.
        /// * ```unknown``` - Due to a hardware error or other condition, the position of the device cannot be determined.
        /// </summary>
        public PositionStatusEnum DevicePosition 
        { 
            get { return devicePosition; }
            set
            {
                if (devicePosition != value)
                {
                    devicePosition = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private PositionStatusEnum devicePosition = DevicePosition;

        /// <summary>
        /// Specifies the actual number of seconds required by the device to resume its normal operational state from
        /// the current power saving mode. This value is zero if either the power saving mode has not been activated or
        /// no power save control is supported.
        /// </summary>
        [Obsolete("This method is no longer used by the common interface. Migrate power saving control to PowerManagement interface. this interface will be removed after version 4.")]
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

        /// <summary>
        /// Specifies the state of the anti-fraud module. Following values are possible:
        /// 
        /// * ```notSupported``` - No anti-fraud module is available.
        /// * ```ok``` - Anti-fraud module is in a good state and no foreign device is detected.
        /// * ```inoperable``` - Anti-fraud module is inoperable.
        /// * ```deviceDetected``` - Anti-fraud module detected the presence of a foreign device.
        /// * ```unknown``` - The state of the anti-fraud module cannot be determined.
        /// </summary>
        public AntiFraudModuleEnum AntiFraudModule 
        {
            get { return antiFraudModule; }
            set
            {
                if (antiFraudModule != value)
                {
                    antiFraudModule = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private AntiFraudModuleEnum antiFraudModule = AntiFraudModule;

        /// <summary>
        /// Exchange status for storage
        /// </summary>
        public ExchangeEnum Exchange 
        {
            get { return exchange; }
            set
            {
                if (exchange != value)
                {
                    exchange = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ExchangeEnum exchange = Exchange;

        /// <summary>
        /// Specifies the status of end to end security support on this device. 
        /// 
        /// Also see [Common.CapabilityProperties.endToEndSecurity](#common.capabilities.completion.properties.common.endtoendsecurity). 
        /// 
        /// * ```notSupported``` - E2E security is not supported by this hardware. Any command can be called without a 
        /// token. 
        /// * ```notEnforced``` - E2E security is supported by this hardware but it is not currently enforced, for 
        /// example because required keys aren't loaded. It's currently possible to perform E2E commands without a 
        /// token. 
        /// * ```notConfigured``` - E2E security is supported but not correctly configured, for example because required
        /// keys aren't loaded. Any attempt to perform any command protected by E2E security will fail.
        /// * ```enforced``` - E2E security is supported and correctly configured. E2E security will be enforced. 
        /// Calling E2E protected commands will only be possible if a valid token is given.
        /// </summary>
        public EndToEndSecurityEnum EndToEndSecurity 
        { 
            get { return endToEndSecurity; } 
            set
            {
                if (endToEndSecurity != value)
                {
                    endToEndSecurity = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private EndToEndSecurityEnum endToEndSecurity = EndToEndSecurity;
    }
}
