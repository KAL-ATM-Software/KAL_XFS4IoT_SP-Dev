/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    /// <summary>
    /// Common capabilities class for reporting available services
    /// </summary>
    public sealed class CommonCapabilitiesClass(
        CommonCapabilitiesClass.CommonInterfaceClass CommonInterface,
        CommonCapabilitiesClass.CardReaderInterfaceClass CardReaderInterface = null,
        CommonCapabilitiesClass.CashDispenserInterfaceClass CashDispenserInterface = null,
        CommonCapabilitiesClass.CashManagementInterfaceClass CashManagementInterface = null,
        CommonCapabilitiesClass.CryptoInterfaceClass CryptoInterface = null,
        CommonCapabilitiesClass.KeyManagementInterfaceClass KeyManagementInterface = null,
        CommonCapabilitiesClass.KeyboardInterfaceClass KeyboardInterface = null,
        CommonCapabilitiesClass.PinPadInterfaceClass PinPadInterface = null,
        CommonCapabilitiesClass.TextTerminalInterfaceClass TextTerminalInterface = null,
        CommonCapabilitiesClass.StorageInterfaceClass StorageInterface = null,
        CommonCapabilitiesClass.LightsInterfaceClass LightsInterface = null,
        CommonCapabilitiesClass.AuxiliariesInterfaceClass AuxiliariesInterface = null,
        CommonCapabilitiesClass.PrinterInterfaceClass PrinterInterface = null,
        CommonCapabilitiesClass.VendorModeInterfaceClass VendorModeInterface = null,
        CommonCapabilitiesClass.VendorApplicationInterfaceClass VendorApplicationInterface = null,
        CommonCapabilitiesClass.BarcodeReaderInterfaceClass BarcodeReaderInterface = null,
        CommonCapabilitiesClass.BiometricInterfaceClass BiometricInterface = null,
        CommonCapabilitiesClass.CashAcceptorInterfaceClass CashAcceptorInterface = null,
        CommonCapabilitiesClass.CameraInterfaceClass CameraInterface = null,
        CommonCapabilitiesClass.CheckScannerInterfaceClass CheckScannerInterface = null,
        CommonCapabilitiesClass.MixedMediaInterfaceClass MixedMediaInterface = null,
        CommonCapabilitiesClass.IntelligentBanknoteNeutralizationClass IntelligentBanknoteNeutralizationInterface = null,
        CommonCapabilitiesClass.DepositInterfaceClass DepositInterface = null,
        CommonCapabilitiesClass.PowerManagementInterfaceClass PowerManagementInterface = null,
        CommonCapabilitiesClass.GermanSpecificInterfaceClass GermanSpecificInterface = null,
        List<CommonCapabilitiesClass.DeviceInformationClass> DeviceInformation = null,
        bool? PowerSaveControl = null, // Obsolete property
        bool? AntiFraudModule = null,
        CommonCapabilitiesClass.EndToEndSecurityClass EndToEndSecurity = null)
    {

        /// <summary>
        /// Command, Events supported by the service interface
        /// </summary>
        public sealed class CommonInterfaceClass(
            List<CommonInterfaceClass.CommandEnum> Commands = null,
            List<CommonInterfaceClass.EventEnum> Events = null)
        {
            public enum CommandEnum
            {
                Capabilities,
                Status,
                ClearCommandNonce,
                GetCommandNonce,
                GetTransactionState,
                PowerSaveControl, // Obsolete property
                SetTransactionState,
                SetVersions,
                Cancel,
            }

            public enum EventEnum
            {
                ErrorEvent,
                NonceClearedEvent,
                StatusChangedEvent,
            }

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class CardReaderInterfaceClass(
            List<CardReaderInterfaceClass.CommandEnum> Commands = null,
            List<CardReaderInterfaceClass.EventEnum> Events = null)
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

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class CashDispenserInterfaceClass(
            List<CashDispenserInterfaceClass.CommandEnum> Commands = null,
            List<CashDispenserInterfaceClass.EventEnum> Events = null)
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

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class CashManagementInterfaceClass(
            List<CashManagementInterfaceClass.CommandEnum> Commands = null,
            List<CashManagementInterfaceClass.EventEnum> Events = null)
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

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class CryptoInterfaceClass(
            List<CryptoInterfaceClass.CommandEnum> Commands = null,
            List<CryptoInterfaceClass.EventEnum> Events = null)
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
            { }

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class KeyManagementInterfaceClass(
            List<KeyManagementInterfaceClass.CommandEnum> Commands = null,
            List<KeyManagementInterfaceClass.EventEnum> Events = null)
        {
            public enum CommandEnum
            {
                DeleteKey,
                DeriveKey,
                ExportRSADeviceSignedItem,
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

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }
        public sealed class KeyboardInterfaceClass(
            List<KeyboardInterfaceClass.CommandEnum> Commands = null,
            List<KeyboardInterfaceClass.EventEnum> Events = null)
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

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }
        public sealed class PinPadInterfaceClass(
            List<PinPadInterfaceClass.CommandEnum> Commands = null,
            List<PinPadInterfaceClass.EventEnum> Events = null)
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
            { }

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }
        public sealed class TextTerminalInterfaceClass(
            List<TextTerminalInterfaceClass.CommandEnum> Commands = null,
            List<TextTerminalInterfaceClass.EventEnum> Events = null)
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

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }
        public sealed class StorageInterfaceClass(
            List<StorageInterfaceClass.CommandEnum> Commands = null,
            List<StorageInterfaceClass.EventEnum> Events = null)
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
                CountsChangedEvent,
            }

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class LightsInterfaceClass(List<LightsInterfaceClass.CommandEnum> Commands = null)
        {
            public enum CommandEnum
            {
                SetLight
            }
            public enum EventEnum
            { }

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; } = null;
        }

        public sealed class AuxiliariesInterfaceClass(
            List<AuxiliariesInterfaceClass.CommandEnum> Commands = null,
            List<AuxiliariesInterfaceClass.EventEnum> Events = null)
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
            { }

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }
        public sealed class PrinterInterfaceClass(
            List<PrinterInterfaceClass.CommandEnum> Commands = null,
            List<PrinterInterfaceClass.EventEnum> Events = null)
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
                SetForm,
                SetMedia,
                ClearBuffer,
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

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class VendorModeInterfaceClass(
            List<VendorModeInterfaceClass.CommandEnum> Commands = null,
            List<VendorModeInterfaceClass.EventEnum> Events = null)
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

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class VendorApplicationInterfaceClass(
            List<VendorApplicationInterfaceClass.CommandEnum> Commands = null,
            List<VendorApplicationInterfaceClass.EventEnum> Events = null)
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

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class BarcodeReaderInterfaceClass(
            List<BarcodeReaderInterfaceClass.CommandEnum> Commands = null,
            List<BarcodeReaderInterfaceClass.EventEnum> Events = null)
        {
            public enum CommandEnum
            {
                Read,
                Reset,
            }

            public enum EventEnum
            { }

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class BiometricInterfaceClass(
            List<BiometricInterfaceClass.CommandEnum> Commands = null,
            List<BiometricInterfaceClass.EventEnum> Events = null)
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

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class CashAcceptorInterfaceClass(
            List<CashAcceptorInterfaceClass.CommandEnum> Commands = null,
            List<CashAcceptorInterfaceClass.EventEnum> Events = null)
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

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class CameraInterfaceClass(
            List<CameraInterfaceClass.CommandEnum> Commands = null,
            List<CameraInterfaceClass.EventEnum> Events = null)
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

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class CheckScannerInterfaceClass(
            List<CheckScannerInterfaceClass.CommandEnum> Commands = null,
            List<CheckScannerInterfaceClass.EventEnum> Events = null)
        {
            public enum CommandEnum
            {
                AcceptItem,
                ActionItem,
                ExpelMedia,
                GetNextItem,
                GetTransactionStatus,
                MediaIn,
                MediaInEnd,
                MediaInRollback,
                PresentMedia,
                ReadImage,
                Reset,
                RetractMedia,
                SetMediaParameters,
                SupplyReplenish,
            }

            public enum EventEnum
            {
                MediaDataEvent,
                MediaDetectedEvent,
                MediaInsertedEvent,
                MediaPresentedEvent,
                MediaRefusedEvent,
                MediaRejectedEvent,
                MediaTakenEvent,
                NoMediaEvent,
            }

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class MixedMediaInterfaceClass(
            List<MixedMediaInterfaceClass.CommandEnum> Commands = null,
            List<MixedMediaInterfaceClass.EventEnum> Events = null)
        {
            public enum CommandEnum
            {
                SetMode,
            }

            public enum EventEnum
            { }

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class DepositInterfaceClass(
            List<DepositInterfaceClass.CommandEnum> Commands = null,
            List<DepositInterfaceClass.EventEnum> Events = null)
        {
            public enum CommandEnum
            {
                Dispense,
                Entry,
                Reset,
                Retract,
                SupplyReplenish,
            }

            public enum EventEnum
            {
                DepositErrorEvent,
                EnvDepositedEvent,
                EnvInsertedEvent,
                EnvTakenEvent,
                InsertDepositEvent,
                MediaDetectedEvent,
            }

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class PowerManagementInterfaceClass(
            List<PowerManagementInterfaceClass.CommandEnum> Commands = null,
            List<PowerManagementInterfaceClass.EventEnum> Events = null)
        {
            public enum CommandEnum
            {
                PowerSaveControlCommand,
            }

            public enum EventEnum
            { }

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class GermanSpecificInterfaceClass(
            List<GermanSpecificInterfaceClass.CommandEnum> Commands = null,
            List<GermanSpecificInterfaceClass.EventEnum> Events = null)
        {
            public enum CommandEnum
            {
                GetHSMTData,
                HSMInit,
                SecureMsgReceive,
                SecureMsgSend,
                SetHSMTData,
            }

            public enum EventEnum
            {
                HSMTDataChangedEvent,
                OPTRequiredEvent,
            }

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class IntelligentBanknoteNeutralizationClass(
            List<IntelligentBanknoteNeutralizationClass.CommandEnum> Commands = null,
            List<IntelligentBanknoteNeutralizationClass.EventEnum> Events = null)
        {
            public enum CommandEnum
            {
                SetProtection,
                TriggerNeutralization,
            }

            public enum EventEnum
            { }

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        public sealed class DepositClass(
            List<DepositClass.CommandEnum> Commands = null,
            List<DepositClass.EventEnum> Events = null)
        {
            public enum CommandEnum
            {
                SetProtection,
                TriggerNeutralization,
            }

            public enum EventEnum
            { }

            public List<CommandEnum> Commands { get; init; } = Commands;
            public List<EventEnum> Events { get; init; } = Events;
        }

        /// <summary>
        /// Device interfaces supported by this XFS4IoT service.
        /// </summary>
        public CommonInterfaceClass CommonInterface { get; init; } = CommonInterface;
        public CardReaderInterfaceClass CardReaderInterface { get; init; } = CardReaderInterface;
        public CashDispenserInterfaceClass CashDispenserInterface { get; init; } = CashDispenserInterface;
        public CashManagementInterfaceClass CashManagementInterface { get; init; } = CashManagementInterface;
        public CryptoInterfaceClass CryptoInterface { get; init; } = CryptoInterface;
        public KeyManagementInterfaceClass KeyManagementInterface { get; init; } = KeyManagementInterface;
        public KeyboardInterfaceClass KeyboardInterface { get; init; } = KeyboardInterface;
        public PinPadInterfaceClass PinPadInterface { get; init; } = PinPadInterface;
        public TextTerminalInterfaceClass TextTerminalInterface { get; init; } = TextTerminalInterface;
        public StorageInterfaceClass StorageInterface { get; init; } = StorageInterface;
        public LightsInterfaceClass LightsInterface { get; init; } = LightsInterface;
        public AuxiliariesInterfaceClass AuxiliariesInterface { get; init; } = AuxiliariesInterface;
        public PrinterInterfaceClass PrinterInterface { get; init; } = PrinterInterface;
        public VendorModeInterfaceClass VendorModeInterface { get; init; } = VendorModeInterface;
        public VendorApplicationInterfaceClass VendorApplicationInterface { get; init; } = VendorApplicationInterface;
        public BarcodeReaderInterfaceClass BarcodeReaderInterface { get; init; } = BarcodeReaderInterface;
        public BiometricInterfaceClass BiometricInterface { get; init; } = BiometricInterface;
        public CashAcceptorInterfaceClass CashAcceptorInterface { get; init; } = CashAcceptorInterface;
        public CameraInterfaceClass CameraInterface { get; init; } = CameraInterface;
        public CheckScannerInterfaceClass CheckScannerInterface { get; init; } = CheckScannerInterface;
        public MixedMediaInterfaceClass MixedMediaInterface { get; init; } = MixedMediaInterface;
        public IntelligentBanknoteNeutralizationClass IntelligentBanknoteNeutralizationInterface { get; init; } = IntelligentBanknoteNeutralizationInterface;
        public DepositInterfaceClass DepositInterface { get; init; } = DepositInterface;
        public PowerManagementInterfaceClass PowerManagementInterface { get; init; } = PowerManagementInterface;
        public GermanSpecificInterfaceClass GermanSpecificInterface { get; init; } = GermanSpecificInterface;

        /// <summary>
        /// Array of deviceInformation structures. If the service uses more than one device there will be on array
        /// element for each device.
        /// </summary>
        public List<DeviceInformationClass> DeviceInformation { get; init; } = DeviceInformation;

        /// <summary>
        /// Specifies whether power saving control is available.
        /// </summary>
        [Obsolete("This method is no longer used by the common interface. Migrate power saving control to PowerManagement interface. this interface will be removed after version 4.")]
        public bool? PowerSaveControl { get; init; } = PowerSaveControl;

        /// <summary>
        /// Specifies whether the anti-fraud module is available.
        /// </summary>
        public bool? AntiFraudModule { get; init; } = AntiFraudModule;

        /// <summary>
        /// True if this hardware supports End to End security, and requires security tokens as part of the 
        /// data to secured operations. If true then operations may fail if a valid security token is not 
        /// supplied. 
        /// 
        /// If false then all operations can be performed without a security token.
        /// <example>true</example>
        /// </summary>
        public EndToEndSecurityClass EndToEndSecurity { get; init; } = EndToEndSecurity;


        public sealed class FirmwareClass(string FirmwareName = null, string FirmwareVersion = null, string HardwareRevision = null)
        {

            /// <summary>
            /// Specifies the firmware name. The property is omitted, if the firmware name is unknown.
            /// </summary>
            public string FirmwareName { get; init; } = FirmwareName;

            /// <summary>
            /// Specifies the firmware version. The property is omitted, if the firmware version is unknown.
            /// </summary>
            public string FirmwareVersion { get; init; } = FirmwareVersion;

            /// <summary>
            /// Specifies the hardware revision. The property is omitted, if the hardware revision is unknown.
            /// </summary>
            public string HardwareRevision { get; init; } = HardwareRevision;

        }

        public sealed class SoftwareClass(string SoftwareName = null, string SoftwareVersion = null)
        {

            /// <summary>
            /// Specifies the software name. The property is omitted, if the software name is unknown.
            /// </summary>
            public string SoftwareName { get; init; } = SoftwareName;

            /// <summary>
            /// Specifies the software version. The property is omitted, if the software version is unknown.
            /// </summary>
            public string SoftwareVersion { get; init; } = SoftwareVersion;

        }

        public sealed class DeviceInformationClass(
            string ModelName = null,
            string SerialNumber = null,
            string RevisionNumber = null,
            string ModelDescription = null,
            List<CommonCapabilitiesClass.FirmwareClass> Firmware = null,
            List<CommonCapabilitiesClass.SoftwareClass> Software = null)
        {

            /// <summary>
            /// Specifies the device model name. The property is omitted, if the device model name is unknown.
            /// </summary>
            public string ModelName { get; init; } = ModelName;

            /// <summary>
            /// Specifies the unique serial number of the device. The property is omitted, if the serial number is unknown.
            /// </summary>
            public string SerialNumber { get; init; } = SerialNumber;

            /// <summary>
            /// Specifies the device revision number. The property is omitted, if the device revision number is unknown.
            /// </summary>
            public string RevisionNumber { get; init; } = RevisionNumber;

            /// <summary>
            /// Contains a description of the device. The property is omitted, if the model description is unknown.
            /// </summary>
            public string ModelDescription { get; init; } = ModelDescription;

            /// <summary>
            /// Array of firmware structures specifying the names and version numbers of the firmware that is present.
            /// Single or multiple firmware versions can be reported. If the firmware versions are not reported, then this property is omitted.
            /// </summary>
            public List<FirmwareClass> Firmware { get; init; } = Firmware;

            /// <summary>
            /// Array of software structures specifying the names and version numbers of the software components that are present.
            /// Single or multiple software versions can be reported. If the software versions are not reported, then this property is omitted.
            /// </summary>
            public List<SoftwareClass> Software { get; init; } = Software;
        }

        public sealed class EndToEndSecurityClass(
            EndToEndSecurityClass.RequiredEnum? Required = null,
            bool? HardwareSecurityElement = null,
            EndToEndSecurityClass.ResponseSecurityEnabledEnum? ResponseSecurityEnabled = null,
            int? CommandNonceTimeout = null)
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
            public RequiredEnum? Required { get; init; } = Required;

            /// <summary>
            /// Specifies if this device has a Hardware Security Element (HSE) which validates the security token. 
            /// If this property is false it means that validation is performed in software.
            /// <example>true</example>
            /// </summary>
            public bool? HardwareSecurityElement { get; init; } = HardwareSecurityElement;

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
            public ResponseSecurityEnabledEnum? ResponseSecurityEnabled { get; init; } = ResponseSecurityEnabled;

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
            public int? CommandNonceTimeout { get; init; } = CommandNonceTimeout;

        }
    }
}
