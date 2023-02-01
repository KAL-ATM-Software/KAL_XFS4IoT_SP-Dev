/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * StatusChangedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Common.Events
{

    [DataContract]
    [Event(Name = "Common.StatusChangedEvent")]
    public sealed class StatusChangedEvent : UnsolicitedEvent<StatusChangedEvent.PayloadData>
    {

        public StatusChangedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(DeviceEnum? Device = null, PositionStatusEnum? DevicePosition = null, int? PowerSaveRecoveryTime = null, AntiFraudModuleEnum? AntiFraudModule = null, ExchangeEnum? Exchange = null, EndToEndSecurityEnum? EndToEndSecurity = null)
                : base()
            {
                this.Device = Device;
                this.DevicePosition = DevicePosition;
                this.PowerSaveRecoveryTime = PowerSaveRecoveryTime;
                this.AntiFraudModule = AntiFraudModule;
                this.Exchange = Exchange;
                this.EndToEndSecurity = EndToEndSecurity;
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
                PotentialFraud,
                Starting
            }

            /// <summary>
            /// Specifies the state of the device. Following values are possible:
            /// 
            /// * ```online``` - The device is online. This is returned when the device is present and operational.
            /// * ```offline``` - The device is offline (e.g., the operator has taken the device offline by turning a switch or breaking an interlock).
            /// * ```powerOff``` - The device is powered off or physically not connected.
            /// * ```noDevice``` - The device is not intended to be there, e.g. this type of self service machine does not contain such a device or it is internally not configured.
            /// * ```hardwareError``` - The device is inoperable due to a hardware error.
            /// * ```userError``` - The device is present but a person is preventing proper device operation.
            /// * ```deviceBusy``` - The device is busy and unable to process a command at this time.
            /// * ```fraudAttempt``` - The device is present but is inoperable because it has detected a fraud attempt.
            /// * ```potentialFraud``` - The device has detected a potential fraud attempt and is capable of remaining in service. In this case the application should make the decision as to whether to take the device offline.
            /// * ```starting``` - The device is starting and performing whatever initialization is necessary. This can be
            /// reported after the connection is made but before the device is ready to accept commands. This must only be a
            /// temporary state, the Service must report a different state as soon as possible. If an error causes
            /// initialization to fail then the state should change to *hardwareError*.
            /// </summary>
            [DataMember(Name = "device")]
            public DeviceEnum? Device { get; init; }

            /// <summary>
            /// Position of the device. Following values are possible:
            /// 
            /// * ```inPosition``` - The device is in its normal operating position, or is fixed in place and cannot be moved.
            /// * ```notInPosition``` - The device has been removed from its normal operating position.
            /// * ```unknown``` - Due to a hardware error or other condition, the position of the device cannot be determined.
            /// </summary>
            [DataMember(Name = "devicePosition")]
            public PositionStatusEnum? DevicePosition { get; init; }

            /// <summary>
            /// Specifies the actual number of seconds required by the device to resume its normal operational state from
            /// the current power saving mode. This value is 0 if either the power saving mode has not been activated or
            /// no power save control is supported.
            /// </summary>
            [DataMember(Name = "powerSaveRecoveryTime")]
            public int? PowerSaveRecoveryTime { get; init; }

            public enum AntiFraudModuleEnum
            {
                Ok,
                Inoperable,
                DeviceDetected,
                Unknown
            }

            /// <summary>
            /// Specifies the state of the anti-fraud module if available. Following values are possible:
            /// 
            /// * ```ok``` - Anti-fraud module is in a good state and no foreign device is detected.
            /// * ```inoperable``` - Anti-fraud module is inoperable.
            /// * ```deviceDetected``` - Anti-fraud module detected the presence of a foreign device.
            /// * ```unknown``` - The state of the anti-fraud module cannot be determined.
            /// </summary>
            [DataMember(Name = "antiFraudModule")]
            public AntiFraudModuleEnum? AntiFraudModule { get; init; }

            [DataMember(Name = "exchange")]
            public ExchangeEnum? Exchange { get; init; }

            public enum EndToEndSecurityEnum
            {
                NotSupported,
                NotEnforced,
                NotConfigured,
                Enforced
            }

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
            /// <example>enforced</example>
            /// </summary>
            [DataMember(Name = "endToEndSecurity")]
            public EndToEndSecurityEnum? EndToEndSecurity { get; init; }

        }

    }
}
