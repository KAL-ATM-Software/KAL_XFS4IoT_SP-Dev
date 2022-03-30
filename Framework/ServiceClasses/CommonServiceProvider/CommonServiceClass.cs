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
using XFS4IoT;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public partial class CommonServiceClass
    {
        public CommonServiceClass(IServiceProvider ServiceProvider, ILogger logger, string ServiceName)
        {
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
        public AuxiliariesCapabilities AuxiliariesCapabilities { get; set; } = null;

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
        public AuxiliariesStatus AuxiliariesStatus { get; set; } = null;

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

        #endregion

        public Task StatusChangedEvent(CommonStatusClass.DeviceEnum? Device,
                                       CommonStatusClass.PositionStatusEnum? Position,
                                       int? PowerSaveRecoveryTime,
                                       CommonStatusClass.AntiFraudModuleEnum? AntiFraudModule,
                                       CommonStatusClass.ExchangeEnum? Exchange,
                                       CommonStatusClass.EndToEndSecurityEnum? EndToEndSecurity) => StatusChangedEvent(new StatusChangedEvent.PayloadData(Device: Device switch
                                       {
                                           CommonStatusClass.DeviceEnum.DeviceBusy => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.DeviceEnum.DeviceBusy,
                                           CommonStatusClass.DeviceEnum.FraudAttempt => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.DeviceEnum.FraudAttempt,
                                           CommonStatusClass.DeviceEnum.HardwareError => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.DeviceEnum.HardwareError,
                                           CommonStatusClass.DeviceEnum.NoDevice => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.DeviceEnum.NoDevice,
                                           CommonStatusClass.DeviceEnum.Offline => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.DeviceEnum.Offline,
                                           CommonStatusClass.DeviceEnum.Online => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.DeviceEnum.Online,
                                           CommonStatusClass.DeviceEnum.PotentialFraud => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.DeviceEnum.PotentialFraud,
                                           CommonStatusClass.DeviceEnum.PowerOff => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.DeviceEnum.PowerOff,
                                           CommonStatusClass.DeviceEnum.UserError => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.DeviceEnum.UserError,
                                           _ => null,
                                       },
                                       DevicePosition: Position switch
                                       {
                                           CommonStatusClass.PositionStatusEnum.InPosition => XFS4IoT.Common.PositionStatusEnum.InPosition,
                                           CommonStatusClass.PositionStatusEnum.NotInPosition => XFS4IoT.Common.PositionStatusEnum.NotInPosition,
                                           CommonStatusClass.PositionStatusEnum.Unknown => XFS4IoT.Common.PositionStatusEnum.Unknown,
                                           _ => null,
                                       },
                                       PowerSaveRecoveryTime: PowerSaveRecoveryTime,
                                       AntiFraudModule: AntiFraudModule switch
                                       {
                                           CommonStatusClass.AntiFraudModuleEnum.DeviceDetected => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.AntiFraudModuleEnum.DeviceDetected,
                                           CommonStatusClass.AntiFraudModuleEnum.Inoperable => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.AntiFraudModuleEnum.Inoperable,
                                           CommonStatusClass.AntiFraudModuleEnum.Ok => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.AntiFraudModuleEnum.Ok,
                                           CommonStatusClass.AntiFraudModuleEnum.Unknown => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.AntiFraudModuleEnum.Unknown,
                                           _ => null,
                                       },
                                       Exchange: Exchange switch
                                       {
                                           CommonStatusClass.ExchangeEnum.Active => XFS4IoT.Common.ExchangeEnum.Active,
                                           CommonStatusClass.ExchangeEnum.Inactive => XFS4IoT.Common.ExchangeEnum.Inactive,
                                           CommonStatusClass.ExchangeEnum.NotSupported => XFS4IoT.Common.ExchangeEnum.NotSupported,
                                           _ => null,
                                       },
                                       EndToEndSecurity: EndToEndSecurity switch
                                       {
                                           CommonStatusClass.EndToEndSecurityEnum.Enforced => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.EndToEndSecurityEnum.Enforced,
                                           CommonStatusClass.EndToEndSecurityEnum.NotConfigured => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.EndToEndSecurityEnum.NotConfigured,
                                           CommonStatusClass.EndToEndSecurityEnum.NotEnforced => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.EndToEndSecurityEnum.NotEnforced,
                                           CommonStatusClass.EndToEndSecurityEnum.NotSupported => XFS4IoT.Common.Events.StatusChangedEvent.PayloadData.EndToEndSecurityEnum.NotSupported,
                                           _ => null,
                                       }));


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


        private void GetCapabilities()
        {
            if (CommonCapabilities is null)
            {
                Logger.Log(Constants.DeviceClass, "CommonDev.CommonCapabilities");
                CommonCapabilities = Device.CommonCapabilities;
                Logger.Log(Constants.DeviceClass, "CommonDev.CommonCapabilities=");

                CommonCapabilities.IsNotNull($"The device class set CommonCapabilities property to null. The device class must report device capabilities.");
            }
        }

        private void GetStatus()
        {
            if (CommonStatus is null)
            {
                Logger.Log(Constants.DeviceClass, "CommonDev.CommonStatus");
                CommonStatus = Device.CommonStatus;
                Logger.Log(Constants.DeviceClass, "CommonDev.CommonStatus=");

                CommonStatus.IsNotNull($"The device class set CommonStatus property to null. The device class must report device status.");
            }
        }
    }
}
