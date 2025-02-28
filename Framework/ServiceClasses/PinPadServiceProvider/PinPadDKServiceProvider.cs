/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
using XFS4IoTFramework.GermanSpecific;
using System.ComponentModel;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a pinpad service provider with DK feature. 
    /// </summary>
    public class PinPadDKServiceProvider : ServiceProvider, IPinPadService, IKeyManagementService, IKeyboardService, ICryptoService, ICommonService, ILightsService, IGermanSpecificService
    {
        public PinPadDKServiceProvider(
            EndpointDetails endpointDetails, 
            string ServiceName, 
            IDevice device, 
            ILogger logger, 
            IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 [XFSConstants.ServiceClass.Common, 
                  XFSConstants.ServiceClass.Crypto, 
                  XFSConstants.ServiceClass.Keyboard, 
                  XFSConstants.ServiceClass.KeyManagement, 
                  XFSConstants.ServiceClass.PinPad, 
                  XFSConstants.ServiceClass.GermanSpecific],
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            KeyManagementService = new KeyManagementServiceClass(this, logger, persistentData);
            CryptoService = new CryptoServiceClass(this, logger);
            KeyboardService = new KeyboardServiceClass(this, logger);
            PinPadService = new PinPadServiceClass(this, logger);
            DKService = new GermanSpecificServiceClass(this, logger);
        }

        private readonly PinPadServiceClass PinPadService;
        private readonly KeyManagementServiceClass KeyManagementService;
        private readonly KeyboardServiceClass KeyboardService;
        private readonly CryptoServiceClass CryptoService;
        private readonly CommonServiceClass CommonService;
        private readonly GermanSpecificServiceClass DKService;

        #region KeyManagement unsolicited events
        public Task InitializedEvent() => KeyManagementService.InitializedEvent();

        public Task IllegalKeyAccessEvent(string KeyName, KeyAccessErrorCodeEnum ErrorCode) => KeyManagementService.IllegalKeyAccessEvent(KeyName, ErrorCode);

        public Task CertificateChangeEvent(CertificateChangeEnum CertificateChange) => KeyManagementService.CertificateChangeEvent(CertificateChange);

        #endregion

        #region Common unsolicited events
        public Task StatusChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => CommonService.StatusChangedEvent(sender, propertyInfo);

        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the PinPad Service.");

        public Task ErrorEvent(
            CommonStatusClass.ErrorEventIdEnum EventId,
            CommonStatusClass.ErrorActionEnum Action,
            string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);
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

        /// <summary>
        /// Stores KeyManagement interface capabilites internally
        /// </summary>
        public KeyManagementCapabilitiesClass KeyManagementCapabilities { get => CommonService.KeyManagementCapabilities; set => CommonService.KeyManagementCapabilities = value; }

        /// <summary>
        /// KeyManagement Status
        /// </summary>
        public KeyManagementStatusClass KeyManagementStatus { get => CommonService.KeyManagementStatus; set => CommonService.KeyManagementStatus = value; }

        /// <summary>
        /// Stores PinPad interface capabilites internally
        /// </summary>
        public PinPadCapabilitiesClass PinPadCapabilities { get => CommonService.PinPadCapabilities; set => CommonService.PinPadCapabilities = value; }

        /// <summary>
        /// Stores Crypto interface capabilites internally
        /// </summary>
        public CryptoCapabilitiesClass CryptoCapabilities { get => CommonService.CryptoCapabilities; set => CommonService.CryptoCapabilities = value; }

        /// <summary>
        /// Stores Keyboard interface capabilites internally
        /// </summary>
        public KeyboardCapabilitiesClass KeyboardCapabilities { get => CommonService.KeyboardCapabilities; set => CommonService.KeyboardCapabilities = value; }

        /// <summary>
        /// Keyboard Status
        /// </summary>
        public KeyboardStatusClass KeyboardStatus { get => CommonService.KeyboardStatus; set => CommonService.KeyboardStatus = value; }

        #endregion

        #region Key Management Service

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
        public void AddKey(
            string KeyName,
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
            int? Version = null) => KeyManagementService.AddKey(
                KeyName,
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
