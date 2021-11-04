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
    public sealed class CommonCapabilitiesClass
    {
        public CommonCapabilitiesClass(List<InterfaceClass> Interfaces = null,
                                       string ServiceVersion = null, 
                                       List<DeviceInformationClass> DeviceInformation = null, 
                                       VendorModeInfoClass VendorModeIformation = null, 
                                       bool? PowerSaveControl = null, 
                                       bool? AntiFraudModule = null, 
                                       List<string> SynchronizableCommands = null, 
                                       bool? EndToEndSecurity = null, 
                                       bool? HardwareSecurityElement = null, 
                                       bool? ResponseSecurityEnabled = null, 
                                       int? CommandNonceTimeout = null)
        {
            this.Interfaces = Interfaces;
            this.ServiceVersion = ServiceVersion;
            this.DeviceInformation = DeviceInformation;
            this.VendorModeIformation = VendorModeIformation;
            this.PowerSaveControl = PowerSaveControl;
            this.AntiFraudModule = AntiFraudModule;
            this.SynchronizableCommands = SynchronizableCommands;
            this.EndToEndSecurity = EndToEndSecurity;
            this.HardwareSecurityElement = HardwareSecurityElement;
            this.ResponseSecurityEnabled = ResponseSecurityEnabled;
            this.CommandNonceTimeout = CommandNonceTimeout;
        }

        /// <summary>
        /// Array of interfaces supported by this XFS4IoT service.
        /// </summary>
        public List<InterfaceClass> Interfaces { get; init; }

        /// <summary>
        /// Specifies the Service Version.
        /// </summary>
        public string ServiceVersion { get; init; }

        /// <summary>
        /// Array of deviceInformation structures. If the service uses more than one device there will be on array
        /// element for each device.
        /// </summary>
        public List<DeviceInformationClass> DeviceInformation { get; init; }

        /// <summary>
        /// Specifies additional information about the Service while in Vendor Dependent Mode. If omitted, all sessions
        /// must be closed before entry to VDM.
        /// </summary>
        public VendorModeInfoClass VendorModeIformation { get; init; }

        /// <summary>
        /// Specifies whether power saving control is available.
        /// </summary>
        public bool? PowerSaveControl { get; init; }

        /// <summary>
        /// Specifies whether the anti-fraud module is available.
        /// </summary>
        public bool? AntiFraudModule { get; init; }

        /// <summary>
        /// List of commands that support synchronization.
        /// </summary>
        public List<string> SynchronizableCommands { get; init; }

        /// <summary>
        /// True if this hardware supports End to End security, and requires security tokens as part of the 
        /// data to secured operations. If true then operations may fail if a valid security token is not 
        /// supplied. 
        /// 
        /// If false then all operations can be performed without a security token.
        /// <example>true</example>
        /// </summary>
        public bool? EndToEndSecurity { get; init; }

        /// <summary>
        /// True if this hardware supports End to End security and has a Hardware Security Element which
        /// validates the security token. Otherwise false.
        /// If this valid is false it may mean that validation is performed in software, or that the
        /// device doesn't support End to End security.
        /// <example>true</example>
        /// </summary>
        public bool? HardwareSecurityElement { get; init; }

        /// <summary>
        /// True if this device will return a security token as part of the response data to commands that
        /// support End to End security, for example, to validate the result of a dispense operation.
        /// <example>true</example>
        /// </summary>
        public bool? ResponseSecurityEnabled { get; init; }

        /// <summary>
        /// If this device supports end to end security and can return a command nonce with the command 
        /// [Common.GetCommandNonce](#common.getcommandnonce), and the hardware automatically clears the command 
        /// nonce after a fixed length of time, this property will report the number of seconds between returning
        /// the command nonce and clearing it. 
        /// 
        /// The value is given in seconds but it should not be assumed that the timeout will be accurate to the nearest 
        /// second. The nonce may also become invalid before the timeout, for example because of a power failure. 
        /// 
        /// Hardware may impose a timeout to reduce the chance of an attacker re-using a nonce value or a token. This 
        /// timeout will be long enough to support normal operations such as dispense and present including creating 
        /// the required token on the host and passing it to the hardware. For example, a command nonce might time out
        /// after one hour (that is, 3600 seconds).
        /// 
        /// If commandNonceTimeout is not reported, or it has a value of zero, then the command nonce will never 
        /// timeout. It may still become invalid, for example because of a power failure or when explicitly cleared.
        /// <example>3600</example>
        /// </summary>
        public int? CommandNonceTimeout { get; init; }


        public sealed class InterfaceClass
        {
            public InterfaceClass(NameEnum Name,
                                  Dictionary<string, CommandsClass> Commands = null,
                                  Dictionary<string, EventsClass> Events = null,
                                  int? MaximumRequests = null,
                                  List<string> AuthenticationRequired = null)
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
                CardEmbosser,
                BarcodeReader,
                Camera,
                Lights,
                Auxiliaries,
                VendorMode,
                VendorApplication,
                Storage
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
            /// * ```CardEmbosser``` - CardEmbosser interface.
            /// * ```BarcodeReader``` - BarcodeReader interface.
            /// * ```Lights``` - Lights interface.
            /// * ```Auxiliaries``` - Auxiliaries interface.
            /// * ```VendorMode``` - VendorMode interface.
            /// * ```VendorApplication``` - VendorApplication interface.
            /// * ```Storage``` - Storage interface
            /// </summary>
            public NameEnum Name { get; init; }

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
                public List<string> Versions { get; init; }

            }

            /// <summary>
            /// The commands supported by the service.
            /// </summary>
            public Dictionary<string, CommandsClass> Commands { get; init; }

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
                public List<string> Versions { get; init; }

            }

            /// <summary>
            /// The events (both event and unsolicited) supported by the service.
            /// </summary>
            public Dictionary<string, EventsClass> Events { get; init; }

            /// <summary>
            /// Specifies the maximum number of requests which can be queued by the Service. This will be omitted if not reported.
            /// This will be zero if the maximum number of requests is unlimited.
            /// 
            /// </summary>
            public int? MaximumRequests { get; init; }

            /// <summary>
            /// Array of commands, which need to be authenticated using the security interface.
            /// </summary>
            public List<string> AuthenticationRequired { get; init; }

        }

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
            public string FirmwareName { get; init; }

            /// <summary>
            /// Specifies the firmware version. The property is omitted, if the firmware version is unknown.
            /// </summary>
            public string FirmwareVersion { get; init; }

            /// <summary>
            /// Specifies the hardware revision. The property is omitted, if the hardware revision is unknown.
            /// </summary>
            public string HardwareRevision { get; init; }

        }

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
            public string SoftwareName { get; init; }

            /// <summary>
            /// Specifies the software version. The property is omitted, if the software version is unknown.
            /// </summary>
            public string SoftwareVersion { get; init; }

        }

        public sealed class DeviceInformationClass
        {
            public DeviceInformationClass(string ModelName = null,
                                          string SerialNumber = null,
                                          string RevisionNumber = null,
                                          string ModelDescription = null,
                                          List<FirmwareClass> Firmware = null,
                                          List<SoftwareClass> Software = null)
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
            public string ModelName { get; init; }

            /// <summary>
            /// Specifies the unique serial number of the device. The property is omitted, if the serial number is unknown.
            /// </summary>
            public string SerialNumber { get; init; }

            /// <summary>
            /// Specifies the device revision number. The property is omitted, if the device revision number is unknown.
            /// </summary>
            public string RevisionNumber { get; init; }

            /// <summary>
            /// Contains a description of the device. The property is omitted, if the model description is unknown.
            /// </summary>
            public string ModelDescription { get; init; }

            /// <summary>
            /// Array of firmware structures specifying the names and version numbers of the firmware that is present.
            /// Single or multiple firmware versions can be reported. If the firmware versions are not reported, then this property is omitted.
            /// </summary>
            public List<FirmwareClass> Firmware { get; init; }

            /// <summary>
            /// Array of software structures specifying the names and version numbers of the software components that are present.
            /// Single or multiple software versions can be reported. If the software versions are not reported, then this property is omitted.
            /// </summary>
            public List<SoftwareClass> Software { get; init; }
        }

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
            /// </summary>
            public bool? AllowOpenSessions { get; init; }

            /// <summary>
            /// Array of commands which can be accepted while in Vendor Dependent Mode.
            /// Any Execute command which is not included in this list will be rejected with a SequenceError as control of the
            /// device has been handed to the Vendor Dependent Application. If omitted, no Execute commands can be accepted.
            /// </summary>
            public List<string> AllowedExecuteCommands { get; init; }
        }
    }
}
