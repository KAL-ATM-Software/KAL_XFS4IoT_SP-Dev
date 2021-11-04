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
    public sealed class CommonStatusClass
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

        public CommonStatusClass(DeviceEnum Device, 
                                 PositionStatusEnum DevicePosition, 
                                 int PowerSaveRecoveryTime, 
                                 AntiFraudModuleEnum AntiFraudModule, 
                                 ExchangeEnum Exchange)
        {
            this.Device = Device;
            this.DevicePosition = DevicePosition;
            this.PowerSaveRecoveryTime = PowerSaveRecoveryTime;
            this.AntiFraudModule = AntiFraudModule;
            this.Exchange = Exchange;
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
        public DeviceEnum Device { get; set; }

        /// <summary>
        /// Position of the device. Following values are possible:
        /// 
        /// * ```inPosition``` - The device is in its normal operating position, or is fixed in place and cannot be moved.
        /// * ```notInPosition``` - The device has been removed from its normal operating position.
        /// * ```unknown``` - Due to a hardware error or other condition, the position of the device cannot be determined.
        /// </summary>
        public PositionStatusEnum DevicePosition { get; set; }

        /// <summary>
        /// Specifies the actual number of seconds required by the device to resume its normal operational state from
        /// the current power saving mode. This value is zero if either the power saving mode has not been activated or
        /// no power save control is supported.
        /// </summary>
        public int PowerSaveRecoveryTime { get; set; }

        /// <summary>
        /// Specifies the state of the anti-fraud module. Following values are possible:
        /// 
        /// * ```notSupported``` - No anti-fraud module is available.
        /// * ```ok``` - Anti-fraud module is in a good state and no foreign device is detected.
        /// * ```inoperable``` - Anti-fraud module is inoperable.
        /// * ```deviceDetected``` - Anti-fraud module detected the presence of a foreign device.
        /// * ```unknown``` - The state of the anti-fraud module cannot be determined.
        /// </summary>
        public AntiFraudModuleEnum AntiFraudModule { get; set; }

        /// <summary>
        /// Exchange status for storage
        /// </summary>
        public ExchangeEnum Exchange { get; set; }
    }
}
