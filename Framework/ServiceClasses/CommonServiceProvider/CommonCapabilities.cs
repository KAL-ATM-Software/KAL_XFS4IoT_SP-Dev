/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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

        /// <summary>
        /// Command, Events supported by the service interface
        /// </summary>
        public sealed class CommonInterfaceClass
        {
            public enum CommandEnum
            {
                Capabilities,
                Status,
                ClearCommandNonce,
                GetCommandNonce,
                GetTransactionState,
                PowerSaveControl,
                SetTransactionState,
                SetVersions,
            }

            public enum EventEnum
            {
                ErrorEvent,
                NonceClearedEvent,
                StatusChangedEvent,
            }

            public CommonInterfaceClass(List<CommandEnum> Commands = null,
                                        List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }
        public sealed class CardReaderInterfaceClass
        {
            public enum CommandEnum
            {
                ChipIO,
                ChipPower,
                EMVClessConfigure,
                EMVClessIssuerUpdate,
                EMVClessPerformTransaction,
                EMVClessQueryApplications,
                Move,
                QueryIFMIdentifier,
                ReadRawData,
                Reset,
                SetKey,
                WriteRawData,
            }

            public enum EventEnum
            {
                CardActionEvent,
                EMVClessReadStatusEvent,
                InsertCardEvent,
                InvalidMediaEvent,
                MediaDetectedEvent,
                MediaInsertedEvent,
                MediaRemovedEvent,
                TrackDetectedEvent,
            }
            public CardReaderInterfaceClass(List<CommandEnum> Commands = null,
                                            List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }

        public sealed class CashDispenserInterfaceClass
        {
            public enum CommandEnum
            {
                Count,
                Denominate,
                Dispense,
                GetMixTable,
                GetMixTypes,
                GetPresentStatus,
                PrepareDispense,
                Present,
                Reject,
                SetMixTable,
                TestCashUnits,
            }

            public enum EventEnum
            {
                IncompleteDispenseEvent,
                DelayedDispenseEvent,
                StartDispenseEvent,
            }
            public CashDispenserInterfaceClass(List<CommandEnum> Commands = null,
                                               List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }

        public sealed class CashManagementInterfaceClass
        {
            public enum CommandEnum
            {
                CalibrateCashUnit,
                CloseShutter,
                GetBankNoteTypes,
                GetClassificationList,
                GetItemInfo,
                GetTellerInfo,
                OpenShutter,
                Reset,
                Retract,
                SetClassificationList,
                SetTellerInfo,
            }

            public enum EventEnum
            {
                IncompleteRetractEvent,
                InfoAvailableEvent,
                ItemsInsertedEvent,
                ItemsPresentedEvent,
                ItemsTakenEvent,
                MediaDetectedEvent,
                NoteErrorEvent,
                ShutterStatusChangedEvent,
                TellerInfoChangedEvent,
            }
            public CashManagementInterfaceClass(List<CommandEnum> Commands = null,
                                                List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }

        public sealed class CryptoInterfaceClass
        {
            public enum CommandEnum
            {
                CryptoData,
                Digest,
                GenerateAuthentication,
                GenerateRandom,
                VerifyAuthentication,
            }

            public enum EventEnum
            {
            }
            public CryptoInterfaceClass(List<CommandEnum> Commands = null,
                                        List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }

        public sealed class KeyManagementInterfaceClass
        {
            public enum CommandEnum
            {
                DeleteKey,
                DeriveKey,
                ExportRSAEPPSignedItem,
                ExportRSAIssuerSignedItem,
                GenerateKCV,
                GenerateRSAKeyPair,
                GetCertificate,
                GetKeyDetail,
                ImportKey,
                Initialization,
                LoadCertificate,
                ReplaceCertificate,
                Reset,
                StartAuthenticate,
                StartKeyExchange,
                ImportKeyToken,
                ImportEmvPublicKey,
            }

            public enum EventEnum
            {
                CertificateChangeEvent,
                DUKPTKSNEvent,
                IllegalKeyAccessEvent,
                InitializedEvent,
            }
            public KeyManagementInterfaceClass(List<CommandEnum> Commands = null,
                                               List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }
        public sealed class KeyboardInterfaceClass
        {
            public enum CommandEnum
            {
                DataEntry,
                DefineLayout,
                GetLayout,
                KeypressBeep,
                PinEntry,
                Reset,
                SecureKeyEntry,
            }

            public enum EventEnum
            {
                EnterDataEvent,
                KeyEvent,
                LayoutEvent,
            }
            public KeyboardInterfaceClass(List<CommandEnum> Commands = null,
                                          List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }
        public sealed class PinPadInterfaceClass
        {
            public enum CommandEnum
            {
                GetPinBlock,
                GetQueryPCIPTSDeviceId,
                LocalDES,
                LocalVisa,
                MaintainPin,
                PresentIDC,
                Reset,
                SetPinBlockData,
            }

            public enum EventEnum
            {
            }
            public PinPadInterfaceClass(List<CommandEnum> Commands = null,
                                        List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }
        public sealed class TextTerminalInterfaceClass
        {
            public enum CommandEnum
            {
                Beep,
                ClearScreen,
                DefineKeys,
                GetFormList,
                GetKeyDetail,
                GetQueryField,
                GetQueryForm,
                Read,
                ReadForm,
                Reset,
                SetResolution,
                Write,
                WriteForm
            }

            public enum EventEnum
            {
                FieldErrorEvent,
                FieldWarningEvent,
                FormLoadedEvent,
                KeyEvent,
            }
            public TextTerminalInterfaceClass(List<CommandEnum> Commands = null,
                                              List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }
        public sealed class StorageInterfaceClass
        {
            public enum CommandEnum
            {
                EndExchange,
                GetStorage,
                SetStorage,
                StartExchange,
            }

            public enum EventEnum
            {
                StorageChangedEvent,
                StorageErrorEvent,
                StorageThresholdEvent,
            }
            public StorageInterfaceClass(List<CommandEnum> Commands = null,
                                         List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }

        public sealed class LightsInterfaceClass
        {
            public enum CommandEnum
            {
                SetLight
            }
            public enum EventEnum
            {
            }
            public LightsInterfaceClass(List<CommandEnum> Commands = null)
            {
                this.Commands = Commands;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; } = null;
        }

        public sealed class AuxiliariesInterfaceClass
        {
            public enum CommandEnum
            {
                ClearAutoStartUpTime,
                GetAutoStartUpTime,
                Register,
                SetAutoStartUpTime,
                SetAuxiliaries,
            }

            public enum EventEnum
            {
            }
            public AuxiliariesInterfaceClass(List<CommandEnum> Commands = null,
                                             List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }
        public sealed class PrinterInterfaceClass
        {
            public enum CommandEnum
            {
                ControlMedia,
                ControlPassbook,
                DispensePaper,
                GetCodelineMapping,
                GetFormList,
                GetMediaList,
                GetQueryField,
                GetQueryForm,
                GetQueryMedia,
                LoadDefinition,
                MediaExtents,
                PrintForm,
                PrintNative,
                PrintRaw,
                ReadForm,
                ReadImage,
                Reset,
                ResetCount,
                RetractMedia,
                SetBlackMarkMode,
                SupplyReplenish,
            }

            public enum EventEnum
            {
                DefinitionLoadedEvent,
                FieldErrorEvent,
                FieldWarningEvent,
                InkThresholdEvent,
                LampThresholdEvent,
                MediaAutoRetractedEvent,
                MediaDetectedEvent,
                MediaInsertedEvent,
                MediaInsertedUnsolicitedEvent,
                MediaPresentedEvent,
                MediaPresentedUnsolicitedEvent,
                MediaRejectedEvent,
                MediaTakenEvent,
                NoMediaEvent,
                PaperThresholdEvent,
                RetractBinStatusEvent,
                RetractBinThresholdEvent,
                TonerThresholdEvent,
            }
            public PrinterInterfaceClass(List<CommandEnum> Commands = null,
                                         List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }

        public sealed class VendorModeInterfaceClass
        {
            public enum CommandEnum
            {
                EnterModeAcknowledge,
                EnterModeRequest,
                ExitModeAcknowledge,
                ExitModeRequest,
                Register,
            }

            public enum EventEnum
            {
                EnterModeRequestEvent,
                ExitModeRequestEvent,
                ModeEnteredEvent,
                ModeExitedEvent,
            }
            public VendorModeInterfaceClass(List<CommandEnum> Commands = null,
                                        List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }

        public sealed class VendorApplicationInterfaceClass
        {
            public enum CommandEnum
            {
                StartLocalApplication,
                GetActiveInterface,
                SetActiveInterface,
            }

            public enum EventEnum
            {
                VendorAppExitedEvent,
                InterfaceChangedEvent,
            }
            public VendorApplicationInterfaceClass(List<CommandEnum> Commands = null,
                                                   List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }

        public sealed class BarcodeReaderInterfaceClass
        {
            public enum CommandEnum
            {
                Read,
                Reset,
            }

            public enum EventEnum
            {
            }

            public BarcodeReaderInterfaceClass(List<CommandEnum> Commands = null,
                                               List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }

        public sealed class BiometricInterfaceClass
        {
            public enum CommandEnum
            {
                Clear,
                GetStorageInfo,
                Import,
                Match,
                Read,
                Reset,
                SetDataPersistence,
                SetMatch,
            }

            public enum EventEnum
            {
                PresentSubjectEvent,
                SubjectDetectedEvent,
                RemoveSubjectEvent,
                SubjectRemovedEvent,
                DataClearedEvent,
                OrientationEvent
            }

            public BiometricInterfaceClass(List<CommandEnum> Commands = null,
                                          List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }

        public sealed class CashAcceptorInterfaceClass
        {
            public enum CommandEnum
            {
                CashIn,
                CashInEnd,
                CashInRollback,
                CashInStart,
                CashUnitCount,
                CompareSignature,
                ConfigureNoteReader,
                ConfigureNoteTypes,
                CreateSignature,
                Deplete,
                DeviceLockControl,
                GetCashInStatus,
                GetDepleteSource,
                GetDeviceLockStatus,
                GetPositionCapabilities,
                GetPresentStatus,
                GetReplenishTarget,
                PreparePresent,
                PresentMedia,
                Replenish,
            }

            public enum EventEnum
            {
                IncompleteDepleteEvent,
                IncompleteReplenishEvent,
                InputRefuseEvent,
                InsertItemsEvent,
                SubCashInEvent,
            }

            public CashAcceptorInterfaceClass(List<CommandEnum> Commands = null,
                                              List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }

        public sealed class CameraInterfaceClass
        {
            public enum CommandEnum
            {
                TakePicture,
                Reset,
            }

            public enum EventEnum
            {
                InvalidDataEvent,
                MediaThresholdEvent,
            }

            public CameraInterfaceClass(List<CommandEnum> Commands = null,
                                        List<EventEnum> Events = null)
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            public List<CommandEnum> Commands { get; init; }
            public List<EventEnum> Events { get; init; }
        }

        public CommonCapabilitiesClass(CommonInterfaceClass CommonInterface,
                                       CardReaderInterfaceClass CardReaderInterface = null,
                                       CashDispenserInterfaceClass CashDispenserInterface = null,
                                       CashManagementInterfaceClass CashManagementInterface = null,
                                       CryptoInterfaceClass CryptoInterface = null,
                                       KeyManagementInterfaceClass KeyManagementInterface = null,
                                       KeyboardInterfaceClass KeyboardInterface = null,
                                       PinPadInterfaceClass PinPadInterface = null,
                                       TextTerminalInterfaceClass TextTerminalInterface = null,
                                       StorageInterfaceClass StorageInterface = null,
                                       LightsInterfaceClass LightsInterface = null,
                                       AuxiliariesInterfaceClass AuxiliariesInterface = null,
                                       PrinterInterfaceClass PrinterInterface = null,
                                       VendorModeInterfaceClass VendorModeInterface = null,
                                       VendorApplicationInterfaceClass VendorApplicationInterface = null,
                                       BarcodeReaderInterfaceClass BarcodeReaderInterface = null,
                                       BiometricInterfaceClass BiometricInterface = null,
                                       CashAcceptorInterfaceClass CashAcceptorInterface = null,
                                       CameraInterfaceClass CameraInterface = null,
                                       List<DeviceInformationClass> DeviceInformation = null, 
                                       bool? PowerSaveControl = null, 
                                       bool? AntiFraudModule = null,
                                       EndToEndSecurityClass EndToEndSecurity = null)
        {
            this.CommonInterface = CommonInterface;
            this.CardReaderInterface = CardReaderInterface; 
            this.CashDispenserInterface = CashDispenserInterface;
            this.CashManagementInterface = CashManagementInterface;
            this.CryptoInterface = CryptoInterface;
            this.KeyManagementInterface = KeyManagementInterface;
            this.KeyboardInterface = KeyboardInterface;
            this.PinPadInterface = PinPadInterface;
            this.TextTerminalInterface = TextTerminalInterface;
            this.StorageInterface = StorageInterface;
            this.LightsInterface = LightsInterface;
            this.AuxiliariesInterface = AuxiliariesInterface;
            this.PrinterInterface = PrinterInterface;
            this.VendorModeInterface = VendorModeInterface;
            this.VendorApplicationInterface = VendorApplicationInterface;
            this.BarcodeReaderInterface = BarcodeReaderInterface;
            this.BiometricInterface = BiometricInterface;
            this.CashAcceptorInterface = CashAcceptorInterface;
            this.CameraInterface = CameraInterface;
            this.DeviceInformation = DeviceInformation;
            this.PowerSaveControl = PowerSaveControl;
            this.AntiFraudModule = AntiFraudModule;
            this.EndToEndSecurity = EndToEndSecurity;
        }

        /// <summary>
        /// Device interfaces supported by this XFS4IoT service.
        /// </summary>
        public CommonInterfaceClass CommonInterface { get; init; }
        public CardReaderInterfaceClass CardReaderInterface { get; init; }
        public CashDispenserInterfaceClass CashDispenserInterface { get; init; }
        public CashManagementInterfaceClass CashManagementInterface { get; init; }
        public CryptoInterfaceClass CryptoInterface { get; init; }
        public KeyManagementInterfaceClass KeyManagementInterface { get; init; }
        public KeyboardInterfaceClass KeyboardInterface { get; init; }
        public PinPadInterfaceClass PinPadInterface { get; init; }
        public TextTerminalInterfaceClass TextTerminalInterface { get; init; }
        public StorageInterfaceClass StorageInterface { get; init; }
        public LightsInterfaceClass LightsInterface { get; init; }
        public AuxiliariesInterfaceClass AuxiliariesInterface { get; init; }
        public PrinterInterfaceClass PrinterInterface { get; init; }
        public VendorModeInterfaceClass VendorModeInterface { get; init; }
        public VendorApplicationInterfaceClass VendorApplicationInterface { get; init; }
        public BarcodeReaderInterfaceClass BarcodeReaderInterface { get; init; }
        public BiometricInterfaceClass BiometricInterface { get; init; }
        public CashAcceptorInterfaceClass CashAcceptorInterface { get; init; }
        public CameraInterfaceClass CameraInterface { get; init; }

        /// <summary>
        /// Array of deviceInformation structures. If the service uses more than one device there will be on array
        /// element for each device.
        /// </summary>
        public List<DeviceInformationClass> DeviceInformation { get; init; }

        /// <summary>
        /// Specifies whether power saving control is available.
        /// </summary>
        public bool? PowerSaveControl { get; init; }

        /// <summary>
        /// Specifies whether the anti-fraud module is available.
        /// </summary>
        public bool? AntiFraudModule { get; init; }

        /// <summary>
        /// True if this hardware supports End to End security, and requires security tokens as part of the 
        /// data to secured operations. If true then operations may fail if a valid security token is not 
        /// supplied. 
        /// 
        /// If false then all operations can be performed without a security token.
        /// <example>true</example>
        /// </summary>
        public EndToEndSecurityClass EndToEndSecurity { get; init; }

        
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

        public sealed class EndToEndSecurityClass
        {
            public enum RequiredEnum
            {
                IfConfigured,
                Always
            }

            public enum ResponseSecurityEnabledEnum
            {
                NotSupported,
                IfConfigured,
                Always
            }

            public EndToEndSecurityClass(RequiredEnum? Required = null, 
                                         bool? HardwareSecurityElement = null, 
                                         ResponseSecurityEnabledEnum? ResponseSecurityEnabled = null, 
                                         int? CommandNonceTimeout = null)
            {
                this.Required = Required;
                this.HardwareSecurityElement = HardwareSecurityElement;
                this.ResponseSecurityEnabled = ResponseSecurityEnabled;
                this.CommandNonceTimeout = CommandNonceTimeout;
            }

            /// <summary>
            /// Specifies the level of support for end to end security
            /// 
            /// * IfConfigured - The device is capable of supporting E2E security, but it will not be 
            ///   enforced if not configured, for example because the required keys are not loaded.
            /// * Always - E2E security is supported and enforced at all times. Failure to supply the required 
            ///   security details will lead to errors. If E2E security is not correctly configured, for example because 
            ///   the required keys are not loaded, all secured commands will fail with an error. 
            /// <example>Required</example>
            /// </summary>
            public RequiredEnum? Required { get; init; }

            /// <summary>
            /// Specifies if this device has a Hardware Security Element (HSE) which validates the security token. 
            /// If this property is false it means that validation is performed in software.
            /// <example>true</example>
            /// </summary>
            public bool? HardwareSecurityElement { get; init; }

            /// <summary>
            /// Specifies if this device will return a security token as part of the response data to commands that 
            /// support end to end security, for example, to validate the result of a dispense operation.
            /// 
            /// * NotSupported -  The device is incapable of returning a response token.
            /// * IfConfigured - The device is capable of supporting E2E security if correctly configured. If E2E 
            ///   security has not been correctly configured, for example because the required keys are not loaded, 
            ///   commands will complete without a security token.
            /// * Always - A security token will be included with command responses. If E2E security is not correctly 
            ///   configured, for example because the required keys are not loaded, the command will complete with an error.
            /// <example>always</example>
            /// </summary>
            public ResponseSecurityEnabledEnum? ResponseSecurityEnabled { get; init; }

            /// <summary>
            /// If this device supports end to end security and can return a command nonce with the command 
            /// Common.GetCommandNonce, and the device automatically clears the command 
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
            /// timeout. It may still become invalid, for example because of a power failure or when explicitly cleared. 
            /// <example>3600</example>
            /// </summary>
            public int? CommandNonceTimeout { get; init; }

        }
    }
}
