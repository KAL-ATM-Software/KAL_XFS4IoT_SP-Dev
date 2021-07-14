/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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

    public enum PositionStatusEnum
    {
        Inposition,
        Notinposition,
        Posunknown
    }


    [DataContract]
    public sealed class StatusPropertiesClass
    {
        public StatusPropertiesClass(DeviceEnum? Device = null, List<string> Extra = null, List<GuideLightsClass> GuideLights = null, PositionStatusEnum? DevicePosition = null, int? PowerSaveRecoveryTime = null, AntiFraudModuleEnum? AntiFraudModule = null)
        {
            this.Device = Device;
            this.Extra = Extra;
            this.GuideLights = GuideLights;
            this.DevicePosition = DevicePosition;
            this.PowerSaveRecoveryTime = PowerSaveRecoveryTime;
            this.AntiFraudModule = AntiFraudModule;
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

        /// <summary>
        /// Specifies the state of the device.
        /// </summary>
        [DataMember(Name = "device")]
        public DeviceEnum? Device { get; init; }

        /// <summary>
        /// Specifies a list of vendor-specific, or any other extended, information. 
        /// The information is returned as a series of "key=value" strings so that it is easily extendable by Service Providers.
        /// 
        /// </summary>
        [DataMember(Name = "extra")]
        public List<string> Extra { get; init; }

        [DataContract]
        public sealed class GuideLightsClass
        {
            public GuideLightsClass(FlashRateEnum? FlashRate = null, ColorEnum? Color = null, DirectionEnum? Direction = null)
            {
                this.FlashRate = FlashRate;
                this.Color = Color;
                this.Direction = Direction;
            }

            public enum FlashRateEnum
            {
                NotSupported,
                Off,
                Slow,
                Medium,
                Quick,
                Continuous
            }

            /// <summary>
            /// Indicates the current flash rate of the guidelight.
            /// </summary>
            [DataMember(Name = "flashRate")]
            public FlashRateEnum? FlashRate { get; init; }

            public enum ColorEnum
            {
                NotSupported,
                Off,
                Red,
                Green,
                Yellow,
                Blue,
                Cyan,
                Magenta,
                White
            }

            /// <summary>
            /// Indicates the current color of the guidelight.
            /// </summary>
            [DataMember(Name = "color")]
            public ColorEnum? Color { get; init; }

            public enum DirectionEnum
            {
                NotSupported,
                Off,
                Entry,
                Exit
            }

            /// <summary>
            /// Indicates the current direction of the guidelight.
            /// </summary>
            [DataMember(Name = "direction")]
            public DirectionEnum? Direction { get; init; }

        }

        /// <summary>
        /// Specifies the state of the guidance light indicators. A number of guidance light types are defined below. Vendor specific guidance lights are defined starting from the end of the array.
        /// </summary>
        [DataMember(Name = "guideLights")]
        public List<GuideLightsClass> GuideLights { get; init; }

        /// <summary>
        /// Position of the device.
        /// </summary>
        [DataMember(Name = "devicePosition")]
        public PositionStatusEnum? DevicePosition { get; init; }

        /// <summary>
        /// Specifies the actual number of seconds required by the device to resume its normal operational state from the current power saving mode. This value is zero if either the power saving mode has not been activated or no power save control is supported
        /// </summary>
        [DataMember(Name = "powerSaveRecoveryTime")]
        public int? PowerSaveRecoveryTime { get; init; }

        public enum AntiFraudModuleEnum
        {
            NotSupp,
            Ok,
            Inop,
            DeviceDetected,
            Unknown
        }

        /// <summary>
        /// Specifies the state of the anti-fraud module
        /// </summary>
        [DataMember(Name = "antiFraudModule")]
        public AntiFraudModuleEnum? AntiFraudModule { get; init; }

    }


    [DataContract]
    public sealed class InterfaceClass
    {
        public InterfaceClass(NameEnum? Name = null, List<string> Commands = null, List<string> Events = null, int? MaximumRequests = null, List<string> AuthenticationRequired = null)
        {
            this.Name = Name;
            this.Commands = Commands;
            this.Events = Events;
            this.MaximumRequests = MaximumRequests;
            this.AuthenticationRequired = AuthenticationRequired;
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
            SensorsAndIndicators,
            CardEmbosser,
            BarcodeReader
        }

        /// <summary>
        /// Name of supported XFS4IoT interface.
        /// </summary>
        [DataMember(Name = "name")]
        public NameEnum? Name { get; init; }

        /// <summary>
        /// Full array of commands supported by this XFS4IoT interface.
        /// </summary>
        [DataMember(Name = "commands")]
        public List<string> Commands { get; init; }

        /// <summary>
        /// Full array of events supported by this XFS4IoT interface.
        /// </summary>
        [DataMember(Name = "events")]
        public List<string> Events { get; init; }

        /// <summary>
        /// Specifies the maximum number of requests which can be queued by the Service. This will be omitted if not reported. 
        /// This will be zero if the maximum number of requests is unlimited.
        /// 
        /// </summary>
        [DataMember(Name = "maximumRequests")]
        public int? MaximumRequests { get; init; }

        /// <summary>
        /// Array of commands, which need to be authenticated using the security interface.
        /// </summary>
        [DataMember(Name = "authenticationRequired")]
        public List<string> AuthenticationRequired { get; init; }

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
        /// Specifies the firmware name. The property is omitted, if the firmware name is unknown.
        /// </summary>
        [DataMember(Name = "firmwareName")]
        public string FirmwareName { get; init; }

        /// <summary>
        /// Specifies the firmware version. The property is omitted, if the firmware version is unknown.
        /// </summary>
        [DataMember(Name = "firmwareVersion")]
        public string FirmwareVersion { get; init; }

        /// <summary>
        /// Specifies the hardware revision. The property is omitted, if the hardware revision is unknown.
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
        /// Specifies the software name. The property is omitted, if the software name is unknown.
        /// </summary>
        [DataMember(Name = "softwareName")]
        public string SoftwareName { get; init; }

        /// <summary>
        /// Specifies the software version. The property is omitted, if the software version is unknown.
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
        /// Specifies the device model name. The property is omitted, if the device model name is unknown.
        /// </summary>
        [DataMember(Name = "modelName")]
        public string ModelName { get; init; }

        /// <summary>
        /// Specifies the unique serial number of the device. The property is omitted, if the serial number is unknown.
        /// </summary>
        [DataMember(Name = "serialNumber")]
        public string SerialNumber { get; init; }

        /// <summary>
        /// Specifies the device revision number. The property is omitted, if the device revision number is unknown.
        /// </summary>
        [DataMember(Name = "revisionNumber")]
        public string RevisionNumber { get; init; }

        /// <summary>
        /// Contains a description of the device. The property is omitted, if the model description is unknown.
        /// </summary>
        [DataMember(Name = "modelDescription")]
        public string ModelDescription { get; init; }

        /// <summary>
        /// Array of firmware structures specifying the names and version numbers of the firmware that is present.
        /// Single or multiple firmware versions can be reported. If the firmware versions are not reported, then this property is omitted.
        /// 
        /// </summary>
        [DataMember(Name = "firmware")]
        public List<FirmwareClass> Firmware { get; init; }

        /// <summary>
        /// Array of software structures specifying the names and version numbers of the software components that are present.
        /// Single or multiple software versions can be reported. If the software versions are not reported, then this property is omitted.
        /// 
        /// </summary>
        [DataMember(Name = "software")]
        public List<SoftwareClass> Software { get; init; }

    }


    [DataContract]
    public sealed class VendorModeInfoClass
    {
        public VendorModeInfoClass(bool? AllowOpenSessions = null, List<string> AllowedExecuteCommands = null)
        {
            this.AllowOpenSessions = AllowOpenSessions;
            this.AllowedExecuteCommands = AllowedExecuteCommands;
        }

        /// <summary>
        /// If TRUE, sessions with this Service may remain open during Vendor Dependent Mode for the purposes of monitoring events, 
        /// sending Info commands, or sending Execute commands listed in lpdwAllowedExecuteCommands. If FALSE, all sessions must be closed before entering Vendor Dependent Mode.
        /// 
        /// </summary>
        [DataMember(Name = "allowOpenSessions")]
        public bool? AllowOpenSessions { get; init; }

        /// <summary>
        /// Array of commands which can be accepted while in Vendor Dependent Mode.
        /// Any Execute command which is not included in this list will be rejected with a SequenceError as control of the 
        /// device has been handed to the Vendor Dependent Application. If omitted, no Execute commands can be accepted.
        /// 
        /// </summary>
        [DataMember(Name = "allowedExecuteCommands")]
        public List<string> AllowedExecuteCommands { get; init; }

    }


    [DataContract]
    public sealed class CapabilityPropertiesClass
    {
        public CapabilityPropertiesClass(string ServiceVersion = null, List<DeviceInformationClass> DeviceInformation = null, VendorModeInfoClass VendorModeIformation = null, List<string> Extra = null, List<GuideLightsClass> GuideLights = null, bool? PowerSaveControl = null, bool? AntiFraudModule = null, List<string> SynchronizableCommands = null, bool? EndToEndSecurity = null, bool? HardwareSecurityElement = null, bool? ResponseSecurityEnabled = null)
        {
            this.ServiceVersion = ServiceVersion;
            this.DeviceInformation = DeviceInformation;
            this.VendorModeIformation = VendorModeIformation;
            this.Extra = Extra;
            this.GuideLights = GuideLights;
            this.PowerSaveControl = PowerSaveControl;
            this.AntiFraudModule = AntiFraudModule;
            this.SynchronizableCommands = SynchronizableCommands;
            this.EndToEndSecurity = EndToEndSecurity;
            this.HardwareSecurityElement = HardwareSecurityElement;
            this.ResponseSecurityEnabled = ResponseSecurityEnabled;
        }

        /// <summary>
        /// Specifies the Service Version.
        /// </summary>
        [DataMember(Name = "serviceVersion")]
        public string ServiceVersion { get; init; }

        /// <summary>
        /// Array of deviceInformation structures. If the service uses more than one device there will be on array element for each device.
        /// </summary>
        [DataMember(Name = "deviceInformation")]
        public List<DeviceInformationClass> DeviceInformation { get; init; }

        /// <summary>
        /// Specifies additional information about the Service while in Vendor Dependent Mode. If omitted, all sessions must be closed before entry to VDM.
        /// </summary>
        [DataMember(Name = "vendorModeIformation")]
        public VendorModeInfoClass VendorModeIformation { get; init; }

        /// <summary>
        /// Specifies a list of vendor-specific, or any other extended, information. 
        /// The information is returned as a series of "key=value" strings so that it is easily extendable by Service Providers
        /// 
        /// </summary>
        [DataMember(Name = "extra")]
        public List<string> Extra { get; init; }

        [DataContract]
        public sealed class GuideLightsClass
        {
            public GuideLightsClass(FlashRateClass FlashRate = null, ColorClass Color = null, DirectionClass Direction = null)
            {
                this.FlashRate = FlashRate;
                this.Color = Color;
                this.Direction = Direction;
            }

            [DataContract]
            public sealed class FlashRateClass
            {
                public FlashRateClass(bool? Slow = null, bool? Medium = null, bool? Quick = null, bool? Continuous = null)
                {
                    this.Slow = Slow;
                    this.Medium = Medium;
                    this.Quick = Quick;
                    this.Continuous = Continuous;
                }

                /// <summary>
                /// The light can blink slowly.
                /// </summary>
                [DataMember(Name = "slow")]
                public bool? Slow { get; init; }

                /// <summary>
                /// The light can blink medium frequency.
                /// </summary>
                [DataMember(Name = "medium")]
                public bool? Medium { get; init; }

                /// <summary>
                /// The light can blink quickly.
                /// </summary>
                [DataMember(Name = "quick")]
                public bool? Quick { get; init; }

                /// <summary>
                /// The light can be continuous (steady).
                /// </summary>
                [DataMember(Name = "continuous")]
                public bool? Continuous { get; init; }

            }

            /// <summary>
            /// Indicates which flash rates are supported by the guidelight.
            /// </summary>
            [DataMember(Name = "flashRate")]
            public FlashRateClass FlashRate { get; init; }

            [DataContract]
            public sealed class ColorClass
            {
                public ColorClass(bool? Red = null, bool? Green = null, bool? Yellow = null, bool? Blue = null, bool? Cyan = null, bool? Magenta = null, bool? White = null)
                {
                    this.Red = Red;
                    this.Green = Green;
                    this.Yellow = Yellow;
                    this.Blue = Blue;
                    this.Cyan = Cyan;
                    this.Magenta = Magenta;
                    this.White = White;
                }

                /// <summary>
                /// The light can be red.
                /// </summary>
                [DataMember(Name = "red")]
                public bool? Red { get; init; }

                /// <summary>
                /// The light can be green.
                /// </summary>
                [DataMember(Name = "green")]
                public bool? Green { get; init; }

                /// <summary>
                /// The light can be yellow.
                /// </summary>
                [DataMember(Name = "yellow")]
                public bool? Yellow { get; init; }

                /// <summary>
                /// The light can be blue.
                /// </summary>
                [DataMember(Name = "blue")]
                public bool? Blue { get; init; }

                /// <summary>
                /// The light can be cyan.
                /// </summary>
                [DataMember(Name = "cyan")]
                public bool? Cyan { get; init; }

                /// <summary>
                /// The light can be magenta.
                /// </summary>
                [DataMember(Name = "magenta")]
                public bool? Magenta { get; init; }

                /// <summary>
                /// The light can be white.
                /// </summary>
                [DataMember(Name = "white")]
                public bool? White { get; init; }

            }

            /// <summary>
            /// Indicates which colors are supported by the guidelight.
            /// </summary>
            [DataMember(Name = "color")]
            public ColorClass Color { get; init; }

            [DataContract]
            public sealed class DirectionClass
            {
                public DirectionClass(bool? Entry = null, bool? Exit = null)
                {
                    this.Entry = Entry;
                    this.Exit = Exit;
                }

                /// <summary>
                /// The light can indicate entry.
                /// </summary>
                [DataMember(Name = "entry")]
                public bool? Entry { get; init; }

                /// <summary>
                /// The light can indicate exit.
                /// </summary>
                [DataMember(Name = "exit")]
                public bool? Exit { get; init; }

            }

            /// <summary>
            /// Indicates which directions are supported by the guidelight.
            /// </summary>
            [DataMember(Name = "direction")]
            public DirectionClass Direction { get; init; }

        }

        /// <summary>
        /// Specifies which guidance lights are available
        /// </summary>
        [DataMember(Name = "guideLights")]
        public List<GuideLightsClass> GuideLights { get; init; }

        /// <summary>
        /// Specifies whether power saving control is available
        /// </summary>
        [DataMember(Name = "powerSaveControl")]
        public bool? PowerSaveControl { get; init; }

        /// <summary>
        /// Specifies whether the anti-fraud module is available
        /// </summary>
        [DataMember(Name = "antiFraudModule")]
        public bool? AntiFraudModule { get; init; }

        /// <summary>
        /// List of commands that support synchronization.
        /// </summary>
        [DataMember(Name = "synchronizableCommands")]
        public List<string> SynchronizableCommands { get; init; }

        /// <summary>
        /// True if this hardware supports End to End security, and requires security tokens as part of the 
        /// data to secured operations. If true then operations may fail if a valid security token is not 
        /// suplied. 
        /// 
        /// If false then all operations can be performed without a security token.
        /// </summary>
        [DataMember(Name = "endToEndSecurity")]
        public bool? EndToEndSecurity { get; init; }

        /// <summary>
        /// True if this hardware supports End to End security and has a Hardware Security Element which 
        /// validates the security token. Otherwise false. 
        /// If this valid is false it may mean that validation is performed in software, or that the 
        /// device doesn't support End to End security.
        /// </summary>
        [DataMember(Name = "hardwareSecurityElement")]
        public bool? HardwareSecurityElement { get; init; }

        /// <summary>
        /// True if this device will return a security token as part of the response data to commands that 
        /// support End to End security, for example, to validate the result of a dispense operation.
        /// </summary>
        [DataMember(Name = "responseSecurityEnabled")]
        public bool? ResponseSecurityEnabled { get; init; }

    }


}
