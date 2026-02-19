/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Auxiliaries;
using XFS4IoT.CashManagement;
using XFS4IoT.Commands;
using XFS4IoT.Common;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public partial class CommonServiceClass
    {
        public CommonServiceClass(IServiceProvider ServiceProvider, ILogger logger, string ServiceName)
        {
            RegisterFactory(ServiceProvider);

            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CommonServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICommonDevice>();
            GetCapabilities();
            GetStatus();
        }

        #region Device Capabilities
        /// <summary>
        /// Stores Common Capabilities
        /// </summary>
        public CommonCapabilitiesClass CommonCapabilities { get; set; } = null;

        /// <summary>
        /// Stores CashDispenser capabilites
        /// </summary>
        public CashDispenserCapabilitiesClass CashDispenserCapabilities { get; set; } = null;

        /// <summary>
        /// Stores CashManagement capabilites
        /// </summary>
        public CashManagementCapabilitiesClass CashManagementCapabilities { get; set; } = null;

        /// <summary>
        /// Stores CardReader capabilites
        /// </summary>
        public CardReaderCapabilitiesClass CardReaderCapabilities { get; set; } = null;

        /// <summary>
        /// Stores TextTerminal capabilites
        /// </summary>
        public TextTerminalCapabilitiesClass TextTerminalCapabilities { get; set; } = null;

        /// <summary>
        /// Stores KeyManagement capabilites
        /// </summary>
        public KeyManagementCapabilitiesClass KeyManagementCapabilities { get; set; } = null;

        /// <summary>
        /// Stores Crypto capabilites
        /// </summary>
        public CryptoCapabilitiesClass CryptoCapabilities { get; set; } = null;

        /// <summary>
        /// Stores PinPad capabilites
        /// </summary>
        public PinPadCapabilitiesClass PinPadCapabilities { get; set; } = null;

        /// <summary>
        /// Stores Keyboard capabilites
        /// </summary>
        public KeyboardCapabilitiesClass KeyboardCapabilities { get; set; } = null;

        /// <summary>
        /// Stores Lights capabilities for an internal use
        /// </summary>
        public LightsCapabilitiesClass LightsCapabilities { get; set; } = null;

        /// <summary>
        /// Stores printer capabilites
        /// </summary>
        public PrinterCapabilitiesClass PrinterCapabilities { get; set; } = null;

        /// <summary>
        /// Stores auxiliaries capabilites
        /// </summary>
        public AuxiliariesCapabilitiesClass AuxiliariesCapabilities { get; set; } = null;

        /// <summary>
        /// Stores vendor application capabilites
        /// </summary>
        public VendorApplicationCapabilitiesClass VendorApplicationCapabilities { get; set; } = null;

        /// <summary>
        /// Stores BarcodeReader capabilites
        /// </summary>
        public BarcodeReaderCapabilitiesClass BarcodeReaderCapabilities { get; set; } = null;

        /// <summary>
        /// Stores Biometric capabilites
        /// </summary>
        public BiometricCapabilitiesClass BiometricCapabilities { get; set; } = null;

        /// <summary>
        /// Stores Camera capabilites
        /// </summary>
        public CameraCapabilitiesClass CameraCapabilities { get; set; } = null;

        /// <summary>
        /// Stores CashAcceptor capabilites
        /// </summary>
        public CashAcceptorCapabilitiesClass CashAcceptorCapabilities { get; set; } = null;

        /// <summary>
        /// Stores Check scanner capabilites
        /// </summary>
        public CheckScannerCapabilitiesClass CheckScannerCapabilities { get; set; } = null;

        /// <summary>
        /// Stores Mixed media capabilites
        /// </summary>
        public MixedMediaCapabilitiesClass MixedMediaCapabilities { get; set; } = null;

        /// <summary>
        /// Stores Power management capabilities
        /// </summary>
        public PowerManagementCapabilitiesClass PowerManagementCapabilities { get; set; } = null;

        /// <summary>
        /// Stores IBNS capabilities
        /// </summary>
        public IBNSCapabilitiesClass IBNSCapabilities { get; set; } = null;

        /// <summary>
        /// Stores German DK capabilities
        /// </summary>
        public GermanCapabilitiesClass GermanCapabilities { get; set; } = null;

        /// <summary>
        /// Stores Deposit capabilities
        /// </summary>
        public DepositCapabilitiesClass DepositCapabilities { get; set; } = null;

        private void GetCapabilities()
        {
            if (CommonCapabilities is not null)
            {
                return;
            }

            Logger.Log(Constants.DeviceClass, "CommonDev.CommonCapabilities");
            CommonCapabilities = Device.CommonCapabilities;
            Logger.Log(Constants.DeviceClass, "CommonDev.CommonCapabilities=");

            CommonCapabilities.IsNotNull($"The device class set CommonCapabilities property to null. The device class must report device capabilities.");

            // Store supported command and event version in the framework.
            Dictionary<string, MessageTypeInfo> supportedFrameworkMessages = null;

            foreach (var assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assem.IsDynamic == true)
                {
                    continue;
                }

                foreach (var type in assem.ExportedTypes)
                {
                    if (Attribute.GetCustomAttribute(type, typeof(XFS4VersionAttribute)) is not XFS4VersionAttribute versionAttrib)
                    {
                        continue;
                    }
                    CommandAttribute commandAttrib = Attribute.GetCustomAttribute(type, typeof(CommandAttribute)) as CommandAttribute;
                    if (commandAttrib is not null)
                    {
                        (supportedFrameworkMessages ??= []).Add(commandAttrib.Name, new(MessageTypeInfo.MessageTypeEnum.Command, new([versionAttrib.Version])));
                        continue;
                    }
                    EventAttribute ceventAttrib = Attribute.GetCustomAttribute(type, typeof(EventAttribute)) as EventAttribute;
                    if (ceventAttrib is not null)
                    {
                        (supportedFrameworkMessages ??= []).Add(ceventAttrib.Name, new(MessageTypeInfo.MessageTypeEnum.Event, new([versionAttrib.Version])));
                        continue;
                    }
                }
            }

            if (supportedFrameworkMessages is null ||
                supportedFrameworkMessages.Count == 0)
            {
                return;
            }

            Dictionary<string, MessageTypeInfo> supportedServiceMessages = [];
            // Common interface supported
            if (CommonCapabilities.CommonInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Common,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.CommonInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Common,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.CommonInterface.Events);
            }

            // Camera interface supported
            if (CommonCapabilities.CameraInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Camera,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.CameraInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Camera,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.CameraInterface.Events);
            }
            // CardReader interface supported
            if (CommonCapabilities.CardReaderInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.CardReader,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.CardReaderInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.CardReader,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.CardReaderInterface.Events);
            }
            // CashDispenser interface supported
            if (CommonCapabilities.CashDispenserInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.CashDispenser,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.CashDispenserInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.CashDispenser,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.CashDispenserInterface.Events);
            }
            // CashAcceptor interface supported
            if (CommonCapabilities.CashAcceptorInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.CashAcceptor,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.CashAcceptorInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.CashAcceptor,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.CashAcceptorInterface.Events);
            }
            // CashManagement interface supported
            if (CommonCapabilities.CashManagementInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.CashManagement,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.CashManagementInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.CashManagement,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.CashManagementInterface.Events);
            }
            // Crypto interface supported
            if (CommonCapabilities.CryptoInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Crypto,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.CryptoInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Crypto,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.CryptoInterface.Events);
            }
            // Keyboard interface supported
            if (CommonCapabilities.KeyboardInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Keyboard,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.KeyboardInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Keyboard,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.KeyboardInterface.Events);
            }
            // KeyManagement interface supported
            if (CommonCapabilities.KeyManagementInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.KeyManagement,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.KeyManagementInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.KeyManagement,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.KeyManagementInterface.Events);
            }
            // PinPad interface supported
            if (CommonCapabilities.PinPadInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.PinPad,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.PinPadInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.PinPad,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.PinPadInterface.Events);
            }
            // Barcode interface supported
            if (CommonCapabilities.BarcodeReaderInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.BarcodeReader,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.BarcodeReaderInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.BarcodeReader,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.BarcodeReaderInterface.Events);
            }
            // Biometrics interface supported
            if (CommonCapabilities.BiometricInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Biometric,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.BiometricInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Biometric,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.BiometricInterface.Events);

            }
            // Auxiliaries interface supported
            if (CommonCapabilities.AuxiliariesInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Auxiliaries,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.AuxiliariesInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Auxiliaries,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.AuxiliariesInterface.Events);
            }
            // Printer interface supported
            if (CommonCapabilities.PrinterInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Printer,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.PrinterInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Printer,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.PrinterInterface.Events);
            }
            // TextTerminal interface supported
            if (CommonCapabilities.TextTerminalInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.TextTerminal,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.TextTerminalInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.TextTerminal,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.TextTerminalInterface.Events);
            }
            // Lights interface supported
            if (CommonCapabilities.LightsInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Lights,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.LightsInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Lights,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.LightsInterface.Events);
            }
            // Storage interface supported
            if (CommonCapabilities.StorageInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Storage,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.StorageInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Storage,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.StorageInterface.Events);
            }
            // VendorApplication interface supported
            if (CommonCapabilities.VendorApplicationInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.VendorApplication,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.VendorApplicationInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.VendorApplication,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.VendorApplicationInterface.Events);
            }
            // VendorMode interface supported
            if (CommonCapabilities.VendorModeInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.VendorMode,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.VendorModeInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.VendorMode,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.VendorModeInterface.Events);
            }
            // Check interface supported
            if (CommonCapabilities.CheckScannerInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Check,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.CheckScannerInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Check,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.CheckScannerInterface.Events);
            }
            // MixedMedia interface supported
            if (CommonCapabilities.MixedMediaInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.MixedMedia,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.MixedMediaInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.MixedMedia,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.MixedMediaInterface.Events);
            }
            // PowerManagement interface supported
            if (CommonCapabilities.PowerManagementInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.PowerManagement,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.PowerManagementInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.PowerManagement,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.PowerManagementInterface.Events);
            }
            // IntelligentBanknoteNeutralization interface supported (IBNS)
            if (CommonCapabilities.BanknoteNeutralizationInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.BanknoteNeutralization,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.BanknoteNeutralizationInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.BanknoteNeutralization,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.BanknoteNeutralizationInterface.Events);
            }
            if (CommonCapabilities.GermanInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.German,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.GermanInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.German,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.GermanInterface.Events);
            }
            if (CommonCapabilities.DepositInterface is not null)
            {
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Deposit,
                    MessageTypeInfo.MessageTypeEnum.Command,
                    supportedFrameworkMessages,
                    CommonCapabilities.DepositInterface.Commands);
                supportedServiceMessages.AddMatches(
                    InterfaceClass.NameEnum.Deposit,
                    MessageTypeInfo.MessageTypeEnum.Event,
                    supportedFrameworkMessages,
                    CommonCapabilities.DepositInterface.Events);
            }

            ServiceProvider.SetMessagesSupported(supportedServiceMessages);
        }

        #endregion

        #region Device Status
        /// <summary>
        /// Stores Commons status
        /// </summary>
        public CommonStatusClass CommonStatus { get; set; } = null;

        /// <summary>
        /// Stores CardReader status
        /// </summary>
        public CardReaderStatusClass CardReaderStatus { get; set; } = null;

        /// <summary>
        /// Stores CashDispenser status
        /// </summary>
        public CashDispenserStatusClass CashDispenserStatus { get; set; } = null;

        /// <summary>
        /// Stores CashManagement status
        /// </summary>
        public CashManagementStatusClass CashManagementStatus { get; set; } = null;

        /// <summary>
        /// Stores KeyManagement status
        /// </summary>
        public KeyManagementStatusClass KeyManagementStatus { get; set; } = null;

        /// <summary>
        /// Stores Keyboard status
        /// </summary>
        public KeyboardStatusClass KeyboardStatus { get; set; } = null;

        /// <summary>
        /// Stores CardReader status
        /// </summary>
        public TextTerminalStatusClass TextTerminalStatus { get; set; } = null;

        /// <summary>
        /// Stores light status
        /// </summary>
        public LightsStatusClass LightsStatus { get; set; } = null;

        /// <summary>
        /// Stores printer status
        /// </summary>
        public PrinterStatusClass PrinterStatus { get; set; } = null;

        /// <summary>
        /// Stores auxiliaries status
        /// </summary>
        public AuxiliariesStatusClass AuxiliariesStatus { get; set; } = null;

        /// <summary>
        /// Stores vendor mode status
        /// </summary>
        public VendorModeStatusClass VendorModeStatus { get; set; } = null;

        /// <summary>
        /// Stores vendor application status
        /// </summary>
        public VendorApplicationStatusClass VendorApplicationStatus { get; set; } = null;

        /// <summary>
        /// Stores BarcodeReader status
        /// </summary>
        public BarcodeReaderStatusClass BarcodeReaderStatus { get; set; } = null;

        /// <summary>
        /// Stores Biometric status
        /// </summary>
        public BiometricStatusClass BiometricStatus { get; set; } = null;

        /// <summary>
        /// Stores Camera status
        /// </summary>
        public CameraStatusClass CameraStatus { get; set; } = null;

        /// <summary>
        /// Stores CashAcceptor status
        /// </summary>
        public CashAcceptorStatusClass CashAcceptorStatus { get; set; } = null;

        /// <summary>
        /// Stores Check scanner status
        /// </summary>
        public CheckScannerStatusClass CheckScannerStatus { get; set; } = null;

        /// <summary>
        /// Stores Mixed media status
        /// </summary>
        public MixedMediaStatusClass MixedMediaStatus { get; set; } = null;

        /// <summary>
        /// Stores Power management status
        /// </summary>
        public PowerManagementStatusClass PowerManagementStatus { get; set; } = null;

        /// <summary>
        /// Stores IBNS status
        /// </summary>
        public IBNSStatusClass IBNSStatus { get; set; } = null;


        /// <summary>
        /// Stores Deposit status
        /// </summary>
        public DepositStatusClass DepostStatus { get; set; } = null;

        private void GetStatus()
        {
            if (CommonStatus is null)
            {
                Logger.Log(Constants.DeviceClass, "CommonDev.CommonStatus");
                CommonStatus = Device.CommonStatus;
                Logger.Log(Constants.DeviceClass, "CommonDev.CommonStatus=");

                CommonStatus.IsNotNull($"The device class set CommonStatus property to null. The device class must report device status.");
                CommonStatus.PropertyChanged += StatusChangedEventFowarder;
            }
        }

        #endregion

        #region Events
        public async Task StatusChangedEvent(object sender, PropertyChangedEventArgs propertyInfo)
        {
            await StatusChangedEventHander(sender, propertyInfo);
        }

        public Task NonceClearedEvent(string ReasonDescription) => NonceClearedEvent(new NonceClearedEvent.PayloadData(ReasonDescription));

        public Task ErrorEvent(CommonStatusClass.ErrorEventIdEnum EventId,
                               CommonStatusClass.ErrorActionEnum Action,
                               string VendorDescription) => ErrorEvent(new ErrorEvent.PayloadData(EventId: EventId switch
                               {
                                   CommonStatusClass.ErrorEventIdEnum.FraudAttempt => XFS4IoT.Common.Events.ErrorEvent.PayloadData.EventIdEnum.FraudAttempt,
                                   CommonStatusClass.ErrorEventIdEnum.Hardware => XFS4IoT.Common.Events.ErrorEvent.PayloadData.EventIdEnum.Hardware,
                                   CommonStatusClass.ErrorEventIdEnum.Software => XFS4IoT.Common.Events.ErrorEvent.PayloadData.EventIdEnum.Software,
                                   _ => XFS4IoT.Common.Events.ErrorEvent.PayloadData.EventIdEnum.User,
                               },
                               Action: Action switch
                               {
                                   CommonStatusClass.ErrorActionEnum.Clear => XFS4IoT.Common.Events.ErrorEvent.PayloadData.ActionEnum.Clear,
                                   CommonStatusClass.ErrorActionEnum.Configuration => XFS4IoT.Common.Events.ErrorEvent.PayloadData.ActionEnum.Configuration,
                                   CommonStatusClass.ErrorActionEnum.Maintenance => XFS4IoT.Common.Events.ErrorEvent.PayloadData.ActionEnum.Maintenance,
                                   CommonStatusClass.ErrorActionEnum.Reset => XFS4IoT.Common.Events.ErrorEvent.PayloadData.ActionEnum.Reset,
                                   CommonStatusClass.ErrorActionEnum.SoftwareError => XFS4IoT.Common.Events.ErrorEvent.PayloadData.ActionEnum.SoftwareError,
                                   _ => XFS4IoT.Common.Events.ErrorEvent.PayloadData.ActionEnum.Suspend,
                               },
                               VendorDescription: VendorDescription));

        #endregion

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await StatusChangedEvent(sender, propertyInfo);
    }
}
