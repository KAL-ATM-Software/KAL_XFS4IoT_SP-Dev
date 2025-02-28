/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * CommonSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.Common
{

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


    public enum PositionStatusEnum
    {
        InPosition,
        NotInPosition,
        Unknown
    }


    public enum AntiFraudModuleEnum
    {
        Ok,
        Inoperable,
        DeviceDetected,
        Unknown
    }


    public enum ExchangeEnum
    {
        Active,
        Inactive
    }


    public enum EndToEndSecurityStatusEnum
    {
        NotEnforced,
        NotConfigured,
        Enforced
    }


    [DataContract]
    public sealed class PersistentDataStoreStatusClass
    {
        public PersistentDataStoreStatusClass(int? Remaining = null)
        {
            this.Remaining = Remaining;
        }

        /// <summary>
        /// Specifies the number of Kilobytes remaining in the persistent data store. This value must be less than or
        /// equal to the persistent data store
        /// [capacity](#common.capabilities.completion.properties.common.persistentdatastore.capacity).
        /// </summary>
        [DataMember(Name = "remaining")]
        [DataTypes(Minimum = 0)]
        public int? Remaining { get; init; }

    }


    [DataContract]
    public sealed class StatusPropertiesClass
    {
        public StatusPropertiesClass(DeviceEnum? Device = null, PositionStatusEnum? DevicePosition = null, int? PowerSaveRecoveryTime = null, AntiFraudModuleEnum? AntiFraudModule = null, ExchangeEnum? Exchange = null, EndToEndSecurityStatusEnum? EndToEndSecurity = null, PersistentDataStoreStatusClass PersistentDataStore = null)
        {
            this.Device = Device;
            this.DevicePosition = DevicePosition;
            this.PowerSaveRecoveryTime = PowerSaveRecoveryTime;
            this.AntiFraudModule = AntiFraudModule;
            this.Exchange = Exchange;
            this.EndToEndSecurity = EndToEndSecurity;
            this.PersistentDataStore = PersistentDataStore;
        }

        [DataMember(Name = "device")]
        public DeviceEnum? Device { get; init; }

        [DataMember(Name = "devicePosition")]
        public PositionStatusEnum? DevicePosition { get; init; }

        /// <summary>
        /// Specifies the actual number of seconds required by the device to resume its normal operational state from
        /// the current power-saving mode. This value is 0 if the power-saving mode has not been activated. This property
        /// is null in [Common.Status](#common.status) if power save control is not supported.
        /// <example>10</example>
        /// </summary>
        [DataMember(Name = "powerSaveRecoveryTime")]
        [DataTypes(Minimum = 0)]
        public int? PowerSaveRecoveryTime { get; init; }

        [DataMember(Name = "antiFraudModule")]
        public AntiFraudModuleEnum? AntiFraudModule { get; init; }

        [DataMember(Name = "exchange")]
        public ExchangeEnum? Exchange { get; init; }

        [DataMember(Name = "endToEndSecurity")]
        public EndToEndSecurityStatusEnum? EndToEndSecurity { get; init; }

        [DataMember(Name = "persistentDataStore")]
        public PersistentDataStoreStatusClass PersistentDataStore { get; init; }

    }


    [DataContract]
    public sealed class InterfaceClass
    {
        public InterfaceClass(NameEnum? Name = null, Dictionary<string, CommandsClass> Commands = null, Dictionary<string, EventsClass> Events = null, int? MaximumRequests = null)
        {
            this.Name = Name;
            this.Commands = Commands;
            this.Events = Events;
            this.MaximumRequests = MaximumRequests;
        }

        public enum NameEnum
        {
            Common,
            CardReader,
            CashAcceptor,
            CashDispenser,
            CashManagement,
            PinPad,
            Crypto,
            KeyManagement,
            Keyboard,
            TextTerminal,
            Printer,
            BarcodeReader,
            Camera,
            Lights,
            Auxiliaries,
            VendorMode,
            VendorApplication,
            Storage,
            Biometric,
            Check,
            MixedMedia,
            Deposit,
            German,
            BanknoteNeutralization,
            PowerManagement
        }

        /// <summary>
        /// Name of supported XFS4IoT interface. Following values are supported:
        /// 
        /// * ```Common``` - Common interface. Every device implements this interface.
        /// * ```CardReader``` - CardReader interface.
        /// * ```CashAcceptor``` - CashAcceptor interface.
        /// * ```CashDispenser``` - CashDispenser interface.
        /// * ```CashManagement``` - CashManagement interface.
        /// * ```PinPad``` - PinPad interface.
        /// * ```Crypto``` - Crypto interface.
        /// * ```KeyManagement``` - KeyManagement interface.
        /// * ```Keyboard``` - Keyboard interface.
        /// * ```TextTerminal``` - TextTerminal interface.
        /// * ```Printer``` - Printer interface.
        /// * ```BarcodeReader``` - BarcodeReader interface.
        /// * ```Camera``` - Camera interface.
        /// * ```Lights``` - Lights interface.
        /// * ```Auxiliaries``` - Auxiliaries interface.
        /// * ```VendorMode``` - VendorMode interface.
        /// * ```VendorApplication``` - VendorApplication interface.
        /// * ```Storage``` - Storage interface.
        /// * ```Biometric``` - Biometric interface.
        /// * ```Check``` - Check interface.
        /// * ```MixedMedia``` - MixedMedia interface.
        /// * ```Deposit``` - Deposit interface.
        /// * ```BanknoteNeutralization``` - Intelligent Banknote Neutralization System interface.
        /// * ```German``` - German country specific interface.
        /// * ```PowerManagement``` - PowerManagement interface.
        /// </summary>
        [DataMember(Name = "name")]
        public NameEnum? Name { get; init; }

        [DataContract]
        public sealed class CommandsClass
        {
            public CommandsClass(List<string> Versions = null)
            {
                this.Versions = Versions;
            }

            /// <summary>
            /// The versions of the command supported by the service. There will be one item for each major version
            /// supported. The minor version number qualifies the exact version of the message the service supports.
            /// <example>["1.3", "2.1", "3.0"]</example>
            /// </summary>
            [DataMember(Name = "versions")]
            [DataTypes(Pattern = @"^[1-9][0-9]*\.([1-9][0-9]*|0)$")]
            public List<string> Versions { get; init; }

        }

        /// <summary>
        /// The commands supported by the service.
        /// </summary>
        [DataMember(Name = "commands")]
        public Dictionary<string, CommandsClass> Commands { get; init; }

        [DataContract]
        public sealed class EventsClass
        {
            public EventsClass(List<string> Versions = null)
            {
                this.Versions = Versions;
            }

            /// <summary>
            /// The versions of the event supported by the service. There will be one item for each major version
            /// supported. The minor version number qualifies the exact version of the message the service supports.
            /// <example>["1.3", "2.1", "3.0"]</example>
            /// </summary>
            [DataMember(Name = "versions")]
            [DataTypes(Pattern = @"^[1-9][0-9]*\.([1-9][0-9]*|0)$")]
            public List<string> Versions { get; init; }

        }

        /// <summary>
        /// The events (both event and unsolicited) supported by the service. May be null if no events are supported.
        /// </summary>
        [DataMember(Name = "events")]
        public Dictionary<string, EventsClass> Events { get; init; }

        /// <summary>
        /// Specifies the maximum number of requests which can be queued by the Service.
        /// This will be 0 if the maximum number of requests is unlimited.
        /// 
        /// </summary>
        [DataMember(Name = "maximumRequests")]
        [DataTypes(Minimum = 0)]
        public int? MaximumRequests { get; init; }

    }


    [DataContract]
    public sealed class FirmwareClass
    {
        public FirmwareClass(string FirmwareName = null, string FirmwareVersion = null, string HardwareRevision = null)
        {
            this.FirmwareName = FirmwareName;
            this.FirmwareVersion = FirmwareVersion;
            this.HardwareRevision = HardwareRevision;
        }

        /// <summary>
        /// Specifies the firmware name. The property is null if the firmware name is unknown.
        /// <example>Acme Firmware</example>
        /// </summary>
        [DataMember(Name = "firmwareName")]
        public string FirmwareName { get; init; }

        /// <summary>
        /// Specifies the firmware version. The property is null if the firmware version is unknown.
        /// <example>1.0.1.2</example>
        /// </summary>
        [DataMember(Name = "firmwareVersion")]
        public string FirmwareVersion { get; init; }

        /// <summary>
        /// Specifies the hardware revision. The property is null if the hardware revision is unknown.
        /// <example>4.3.0.5</example>
        /// </summary>
        [DataMember(Name = "hardwareRevision")]
        public string HardwareRevision { get; init; }

    }


    [DataContract]
    public sealed class SoftwareClass
    {
        public SoftwareClass(string SoftwareName = null, string SoftwareVersion = null)
        {
            this.SoftwareName = SoftwareName;
            this.SoftwareVersion = SoftwareVersion;
        }

        /// <summary>
        /// Specifies the software name. The property is null if the software name is unknown.
        /// <example>Acme Software Name</example>
        /// </summary>
        [DataMember(Name = "softwareName")]
        public string SoftwareName { get; init; }

        /// <summary>
        /// Specifies the software version. The property is null if the software version is unknown.
        /// <example>1.3.0.2</example>
        /// </summary>
        [DataMember(Name = "softwareVersion")]
        public string SoftwareVersion { get; init; }

    }


    [DataContract]
    public sealed class DeviceInformationClass
    {
        public DeviceInformationClass(string ModelName = null, string SerialNumber = null, string RevisionNumber = null, string ModelDescription = null, List<FirmwareClass> Firmware = null, List<SoftwareClass> Software = null)
        {
            this.ModelName = ModelName;
            this.SerialNumber = SerialNumber;
            this.RevisionNumber = RevisionNumber;
            this.ModelDescription = ModelDescription;
            this.Firmware = Firmware;
            this.Software = Software;
        }

        /// <summary>
        /// Specifies the device model name. The property is null if the device model name is unknown.
        /// <example>AcmeModel42</example>
        /// </summary>
        [DataMember(Name = "modelName")]
        public string ModelName { get; init; }

        /// <summary>
        /// Specifies the unique serial number of the device. The property is null if the serial number is unknown.
        /// <example>1.0.12.05</example>
        /// </summary>
        [DataMember(Name = "serialNumber")]
        public string SerialNumber { get; init; }

        /// <summary>
        /// Specifies the device revision number. The property is null if the device revision number is unknown.
        /// <example>1.2.3</example>
        /// </summary>
        [DataMember(Name = "revisionNumber")]
        public string RevisionNumber { get; init; }

        /// <summary>
        /// Contains a description of the device. The property is null if the model description is unknown.
        /// <example>Acme Dispenser Model 3</example>
        /// </summary>
        [DataMember(Name = "modelDescription")]
        public string ModelDescription { get; init; }

        /// <summary>
        /// Array of firmware structures specifying the names and version numbers of the firmware that is present.
        /// Single or multiple firmware versions can be reported. If the firmware versions are not reported, then this property is null.
        /// </summary>
        [DataMember(Name = "firmware")]
        public List<FirmwareClass> Firmware { get; init; }

        /// <summary>
        /// Array of software structures specifying the names and version numbers of the software components that are present.
        /// Single or multiple software versions can be reported. If the software versions are not reported, then this property is null.
        /// </summary>
        [DataMember(Name = "software")]
        public List<SoftwareClass> Software { get; init; }

    }


    [DataContract]
    public sealed class EndToEndSecurityClass
    {
        public EndToEndSecurityClass(RequiredEnum? Required = null, bool? HardwareSecurityElement = null, ResponseSecurityEnabledEnum? ResponseSecurityEnabled = null, List<string> Commands = null, int? CommandNonceTimeout = null)
        {
            this.Required = Required;
            this.HardwareSecurityElement = HardwareSecurityElement;
            this.ResponseSecurityEnabled = ResponseSecurityEnabled;
            this.Commands = Commands;
            this.CommandNonceTimeout = CommandNonceTimeout;
        }

        public enum RequiredEnum
        {
            IfConfigured,
            Always
        }

        /// <summary>
        /// Specifies the level of support for end-to-end security
        /// 
        /// * ```ifConfigured``` - The device is capable of supporting E2E security, but it will not be
        ///   enforced if not configured, for example because the required keys are not loaded.
        /// * ```always``` - E2E security is supported and enforced at all times. Failure to supply the required
        ///   security details will lead to errors. If E2E security is not correctly configured, for example because
        ///   the required keys are not loaded, all secured commands will fail with an error.
        /// <example>always</example>
        /// </summary>
        [DataMember(Name = "required")]
        public RequiredEnum? Required { get; init; }

        /// <summary>
        /// Specifies if this device has a Hardware Security Element (HSE) which validates the security token.
        /// If this property is false it means that validation is performed in software.
        /// <example>true</example>
        /// </summary>
        [DataMember(Name = "hardwareSecurityElement")]
        public bool? HardwareSecurityElement { get; init; }

        public enum ResponseSecurityEnabledEnum
        {
            IfConfigured,
            Always
        }

        /// <summary>
        /// Specifies if this device will return a security token as part of the response data to commands that
        /// support end-to-end security, for example, to validate the result of a dispense operation. This property
        /// is null in [Common.Status](#common.status) if the device is incapable of returning a response token,
        /// otherwise one of the following values:
        /// 
        /// * ```ifConfigured``` - The device is capable of supporting E2E security if correctly configured. If E2E
        ///   security has not been correctly configured, for example because the required keys are not loaded,
        ///   commands will complete without a security token.
        /// * ```always``` - A security token will be included with command responses. If E2E security is not correctly
        ///   configured, for example because the required keys are not loaded, the command will complete with an error.
        /// <example>always</example>
        /// </summary>
        [DataMember(Name = "responseSecurityEnabled")]
        public ResponseSecurityEnabledEnum? ResponseSecurityEnabled { get; init; }

        /// <summary>
        /// Array of commands which require an E2E token to authorize. These commands will fail if called without
        /// a valid token.
        /// 
        /// The commands that can be listed here depend on the XFS4IoT standard, but it's possible that the standard
        /// will change over time, so for maximum compatibility an application should check this property before calling
        /// a command.
        /// 
        /// Note that this only includes commands that _require_ a token. Commands that take a nonce and _return_ a
        /// token will not be listed here. Those commands can be called without a nonce and will continue to operate
        /// in a compatible way.
        /// <example>["CashDispenser.Dispense"]</example>
        /// </summary>
        [DataMember(Name = "commands")]
        [DataTypes(Pattern = @"^[A-Za-z][A-Za-z0-9]*\.[A-Za-z][A-Za-z0-9]*$")]
        public List<string> Commands { get; init; }

        /// <summary>
        /// If this device supports end-to-end security and can return a command nonce with the command
        /// [Common.GetCommandNonce](#common.getcommandnonce), and the device automatically clears the command
        /// nonce after a fixed length of time, this property will report the number of seconds between returning
        /// the command nonce and clearing it.
        /// 
        /// The value is given in seconds but it should not be assumed that the timeout will be accurate to the nearest
        /// second. The nonce may also become invalid before the timeout, for example because of a power failure.
        /// 
        /// The device may impose a timeout to reduce the chance of an attacker re-using a nonce value or a token. This
        /// timeout will be long enough to support normal operations such as dispense and present including creating
        /// the required token on the host and passing it to the device. For example, a command nonce might time out
        /// after one hour (that is, 3600 seconds).
        /// 
        /// In all other cases, commandNonceTimeout will have a value of zero. Any command nonce will never
        /// timeout. It may still become invalid, for example because of a power failure or when explicitly cleared
        /// using the [Common.ClearCommandNonce](#common.clearcommandnonce) command.
        /// <example>3600</example>
        /// </summary>
        [DataMember(Name = "commandNonceTimeout")]
        [DataTypes(Minimum = 0)]
        public int? CommandNonceTimeout { get; init; }

    }


    [DataContract]
    public sealed class PersistentDataStoreCapabilityClass
    {
        public PersistentDataStoreCapabilityClass(int? Capacity = null)
        {
            this.Capacity = Capacity;
        }

        /// <summary>
        /// Specifies the total number of Kilobytes that can be stored.
        /// 
        /// Clients should use this, in conjunction with the
        /// [remaining](#common.status.completion.properties.common.persistentdatastore.remaining) status to
        /// determine how to make best use of the available space, for example, a client may elect
        /// to store all data where the service reports ample capacity or dynamically set and delete
        /// partial data when the available space is small.
        /// 
        /// If the service effectively has no practical persistent storage limit, for example when executing on a
        /// general-purpose operating system with a large drive, the service may report a capacity value less than the
        /// drive size representing the persistent data size it can handle while remaining performant.
        /// </summary>
        [DataMember(Name = "capacity")]
        [DataTypes(Minimum = 1)]
        public int? Capacity { get; init; }

    }


    [DataContract]
    public sealed class CapabilityPropertiesClass
    {
        public CapabilityPropertiesClass(string ServiceVersion = null, List<DeviceInformationClass> DeviceInformation = null, bool? PowerSaveControl = null, bool? AntiFraudModule = null, EndToEndSecurityClass EndToEndSecurity = null, PersistentDataStoreCapabilityClass PersistentDataStore = null)
        {
            this.ServiceVersion = ServiceVersion;
            this.DeviceInformation = DeviceInformation;
            this.PowerSaveControl = PowerSaveControl;
            this.AntiFraudModule = AntiFraudModule;
            this.EndToEndSecurity = EndToEndSecurity;
            this.PersistentDataStore = PersistentDataStore;
        }

        /// <summary>
        /// Specifies the Service Version.
        /// <example>1.3.42</example>
        /// </summary>
        [DataMember(Name = "serviceVersion")]
        public string ServiceVersion { get; init; }

        /// <summary>
        /// Array of deviceInformation structures. If the service uses more than one device there will be one array
        /// item for each device.
        /// </summary>
        [DataMember(Name = "deviceInformation")]
        public List<DeviceInformationClass> DeviceInformation { get; init; }

        /// <summary>
        /// Specifies whether power saving control is available.
        /// </summary>
        [DataMember(Name = "powerSaveControl")]
        public bool? PowerSaveControl { get; init; }

        /// <summary>
        /// Specifies whether the anti-fraud module is available.
        /// </summary>
        [DataMember(Name = "antiFraudModule")]
        public bool? AntiFraudModule { get; init; }

        [DataMember(Name = "endToEndSecurity")]
        public EndToEndSecurityClass EndToEndSecurity { get; init; }

        [DataMember(Name = "persistentDataStore")]
        public PersistentDataStoreCapabilityClass PersistentDataStore { get; init; }

    }


    [DataContract]
    public sealed class StatusPropertiesChangedClass
    {
        public StatusPropertiesChangedClass(DeviceEnum? Device = null, PositionStatusEnum? DevicePosition = null, int? PowerSaveRecoveryTime = null, AntiFraudModuleEnum? AntiFraudModule = null, ExchangeEnum? Exchange = null, EndToEndSecurityStatusEnum? EndToEndSecurity = null, PersistentDataStoreStatusClass PersistentDataStore = null)
        {
            this.Device = Device;
            this.DevicePosition = DevicePosition;
            this.PowerSaveRecoveryTime = PowerSaveRecoveryTime;
            this.AntiFraudModule = AntiFraudModule;
            this.Exchange = Exchange;
            this.EndToEndSecurity = EndToEndSecurity;
            this.PersistentDataStore = PersistentDataStore;
        }

        [DataMember(Name = "device")]
        public DeviceEnum? Device { get; init; }

        [DataMember(Name = "devicePosition")]
        public PositionStatusEnum? DevicePosition { get; init; }

        /// <summary>
        /// Specifies the actual number of seconds required by the device to resume its normal operational state from
        /// the current power-saving mode. This value is 0 if the power-saving mode has not been activated. This property
        /// is null in [Common.Status](#common.status) if power save control is not supported.
        /// <example>10</example>
        /// </summary>
        [DataMember(Name = "powerSaveRecoveryTime")]
        [DataTypes(Minimum = 0)]
        public int? PowerSaveRecoveryTime { get; init; }

        [DataMember(Name = "antiFraudModule")]
        public AntiFraudModuleEnum? AntiFraudModule { get; init; }

        [DataMember(Name = "exchange")]
        public ExchangeEnum? Exchange { get; init; }

        [DataMember(Name = "endToEndSecurity")]
        public EndToEndSecurityStatusEnum? EndToEndSecurity { get; init; }

        [DataMember(Name = "persistentDataStore")]
        public PersistentDataStoreStatusClass PersistentDataStore { get; init; }

    }


}
