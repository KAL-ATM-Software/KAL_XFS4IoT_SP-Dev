/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.KeyManagement;
using XFS4IoTFramework.PinPad;
using XFS4IoTFramework.Keyboard;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a pinpad service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical pinpad, which implements the PinPad, KeyManagement, Keyboard, Crypto and Common interfaces. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class PinPadServiceProvider : ServiceProvider, IPinPadService, IKeyManagementService, IKeyboardService, ICryptoService, ICommonService, ILightsService
    {
        public PinPadServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger, IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.Crypto, XFSConstants.ServiceClass.Keyboard, XFSConstants.ServiceClass.KeyManagement, XFSConstants.ServiceClass.PinPad },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            KeyManagementService = new KeyManagementServiceClass(this, CommonService, logger, persistentData);
            CryptoService = new CryptoServiceClass(this, KeyManagementService, CommonService, logger);
            KeyboardService = new KeyboardServiceClass(this, KeyManagementService, CommonService, logger);
            PinPadService = new PinPadServiceClass(this, KeyManagementService, CommonService, logger);
        }

        private readonly PinPadServiceClass PinPadService;
        private readonly KeyManagementServiceClass KeyManagementService;
        private readonly KeyboardServiceClass KeyboardService;
        private readonly CryptoServiceClass CryptoService;
        private readonly CommonServiceClass CommonService;


        #region KeyManagement unsolicited events
        public Task InitializedEvent() => KeyManagementService.InitializedEvent();

        public Task IllegalKeyAccessEvent(string KeyName, KeyAccessErrorCodeEnum ErrorCode) => KeyManagementService.IllegalKeyAccessEvent(new XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData(KeyName, ErrorCode switch
        {
            KeyAccessErrorCodeEnum.AlgorithmNotSupp => XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData.ErrorCodeEnum.AlgorithmNotSupp,
            KeyAccessErrorCodeEnum.KeyNotFound => XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData.ErrorCodeEnum.KeyNotFound,
            KeyAccessErrorCodeEnum.KeyNoValue => XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData.ErrorCodeEnum.KeyNoValue,
            _ => XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData.ErrorCodeEnum.UseViolation,
        }));

        public Task CertificateChangeEvent(CertificateChangeEnum CertificateChange) => KeyManagementService.CertificateChangeEvent(new XFS4IoT.KeyManagement.Events.CertificateChangeEvent.PayloadData(CertificateChange switch
                                                                                                                                                                                                       {
                                                                                                                                                                                                           _ => XFS4IoT.KeyManagement.Events.CertificateChangeEvent.PayloadData.CertificateChangeEnum.Secondary
                                                                                                                                                                                                       }));
        #endregion


        #region Common unsolicited events
        public Task PowerSaveChangeEvent(int PowerSaveRecoveryTime) => CommonService.PowerSaveChangeEvent(new PowerSaveChangeEvent.PayloadData(PowerSaveRecoveryTime));

        public Task DevicePositionEvent(CommonStatusClass.PositionStatusEnum Position) => CommonService.DevicePositionEvent(
                                                                                                        new DevicePositionEvent.PayloadData(Position switch
                                                                                                        {
                                                                                                            CommonStatusClass.PositionStatusEnum.InPosition => XFS4IoT.Common.PositionStatusEnum.InPosition,
                                                                                                            CommonStatusClass.PositionStatusEnum.NotInPosition => XFS4IoT.Common.PositionStatusEnum.NotInPosition,
                                                                                                            _ => XFS4IoT.Common.PositionStatusEnum.Unknown,
                                                                                                        }
                                                                                                    ));

        public Task NonceClearedEvent(string ReasonDescription) => CommonService.NonceClearedEvent(new NonceClearedEvent.PayloadData(ReasonDescription));

        public Task ExchangeStateChangedEvent(CommonStatusClass.ExchangeEnum Exchange) => CommonService.ExchangeStateChangedEvent(
                                                                                                        new ExchangeStateChangedEvent.PayloadData(Exchange switch
                                                                                                        {
                                                                                                            CommonStatusClass.ExchangeEnum.Active => XFS4IoT.Common.ExchangeEnum.Active,
                                                                                                            CommonStatusClass.ExchangeEnum.Inactive => XFS4IoT.Common.ExchangeEnum.Inactive,
                                                                                                            _ => XFS4IoT.Common.ExchangeEnum.NotSupported,
                                                                                                        }
                                                                                                    ));
        #endregion

        #region Common Service
        /// <summary>
        /// Stores Common interface capabilites internally
        /// </summary>
        public CommonCapabilitiesClass CommonCapabilities { get => CommonService.CommonCapabilities; set => CommonService.CommonCapabilities = value; }

        /// <summary>
        /// Common Status
        /// </summary>
        public CommonStatusClass CommonStatus { get => CommonService.CommonStatus; set => CommonService.CommonStatus = value; }

        #endregion

        #region Key Management Service
        /// <summary>
        /// Stores KeyManagement interface capabilites internally
        /// </summary>
        public KeyManagementCapabilitiesClass KeyManagementCapabilities { get => CommonService.KeyManagementCapabilities; set => CommonService.KeyManagementCapabilities = value; }

        /// <summary>
        /// Find keyslot available or being used
        /// </summary>
        public int FindKeySlot(string KeyName) => KeyManagementService.FindKeySlot(KeyName);

        /// <summary>
        /// Stored key information of this device
        /// </summary>
        public List<KeyDetail> GetKeyTable() => KeyManagementService.GetKeyTable();

        /// <summary>
        /// Return detailed stored key information
        /// </summary>
        public KeyDetail GetKeyDetail(string KeyName) => KeyManagementService.GetKeyDetail(KeyName);

        /// <summary>
        /// Add new key into the collection and return key slot
        /// </summary>
        public void AddKey(string KeyName,
                           int KeySlot,
                           string KeyUsage,
                           string Algorithm,
                           string ModeOfUse,
                           int KeyLength,
                           KeyDetail.KeyStatusEnum KeyStatus,
                           bool Preloaded,
                           string RestrictedKeyUsage = null,
                           string KeyVersionNumber = null,
                           string Exportability = null,
                           List<byte> OptionalKeyBlockHeader = null,
                           int? Generation = null,
                           DateTime? ActivatingDate = null,
                           DateTime? ExpiryDate = null,
                           int? Version = null) => KeyManagementService.AddKey(KeyName,
                                                                               KeySlot,
                                                                               KeyUsage,
                                                                               Algorithm,
                                                                               ModeOfUse,
                                                                               KeyLength,
                                                                               KeyStatus,
                                                                               Preloaded,
                                                                               RestrictedKeyUsage,
                                                                               KeyVersionNumber,
                                                                               Exportability,
                                                                               OptionalKeyBlockHeader,
                                                                               Generation,
                                                                               ActivatingDate,
                                                                               ExpiryDate,
                                                                               Version);

        /// <summary>
        /// Delete specified key from the collection and return key slot
        /// </summary>
        public void DeleteKey(string KeyName) => KeyManagementService.DeleteKey(KeyName);

        /// <summary>
        /// Update key status
        /// </summary>
        public void UpdateKeyStatus(string KeyName, KeyDetail.KeyStatusEnum Status) => KeyManagementService.UpdateKeyStatus(KeyName, Status);

        /// <summary>
        /// Return secure key entry component status
        /// </summary>
        /// <returns></returns>
        public SecureKeyEntryStatusClass GetSecureKeyEntryStatus() => KeyManagementService.GetSecureKeyEntryStatus();

        #endregion

        #region PinPad Service

        /// <summary>
        /// Return list of PCI Security Standards Council PIN transaction security (PTS) certification held by the PIN device
        /// </summary>
        public PCIPTSDeviceIdClass PCIPTSDeviceId { get => PinPadService.PCIPTSDeviceId; set { } }

        #endregion

        #region Keyboard Service

        /// <summary>
        /// Function keys device supported
        /// </summary>
        public Dictionary<EntryModeEnum, List<string>> SupportedFunctionKeys { get => KeyboardService.SupportedFunctionKeys; set { } }

        /// <summary>
        /// Function keys device supported with shift key
        /// </summary>
        public Dictionary<EntryModeEnum, List<string>> SupportedFunctionKeysWithShift { get => KeyboardService.SupportedFunctionKeysWithShift; set { } }

        /// <summary>
        /// Keyboard layout device supported
        /// </summary>
        public Dictionary<EntryModeEnum, List<FrameClass>> KeyboardLayouts { get => KeyboardService.KeyboardLayouts; set { } }

        #endregion
    }
}
