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
    public class PinPadServiceProvider : ServiceProvider, IPinPadServiceClass, IKeyManagementServiceClass, IKeyboardServiceClass, ICryptoServiceClass, ICommonServiceClass
    {
        public PinPadServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger, IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.Crypto, XFSConstants.ServiceClass.Keyboard, XFSConstants.ServiceClass.KeyManagement, XFSConstants.ServiceClass.PinPad },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger);
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


        #region PinPad unsolicited events
        public Task IllegalKeyAccessEvent(XFS4IoT.PinPad.Events.IllegalKeyAccessEvent.PayloadData Payload) => PinPadService.IllegalKeyAccessEvent(Payload);
        #endregion

        #region KeyManagement unsolicited events
        public Task InitializedEvent() => KeyManagementService.InitializedEvent();

        public Task IllegalKeyAccessEvent(XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData Payload) => KeyManagementService.IllegalKeyAccessEvent(Payload);

        public Task CertificateChangeEvent(XFS4IoT.KeyManagement.Events.CertificateChangeEvent.PayloadData Payload) => KeyManagementService.CertificateChangeEvent(Payload);
        #endregion

        #region Crypto unsolicited events
        public Task IllegalKeyAccessEvent(XFS4IoT.Crypto.Events.IllegalKeyAccessEvent.PayloadData Payload) => CryptoService.IllegalKeyAccessEvent(Payload);
        #endregion

        #region Common unsolicited events
        public Task PowerSaveChangeEvent(PowerSaveChangeEvent.PayloadData Payload) => CommonService.PowerSaveChangeEvent(Payload);

        public Task DevicePositionEvent(DevicePositionEvent.PayloadData Payload) => CommonService.DevicePositionEvent(Payload);

        public Task NonceClearedEvent(NonceClearedEvent.PayloadData Payload) => CommonService.NonceClearedEvent(Payload);

        public Task ExchangeStateChangedEvent(ExchangeStateChangedEvent.PayloadData Payload) => CommonService.ExchangeStateChangedEvent(Payload);
        #endregion

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

        /// <summary>
        /// Return list of PCI Security Standards Council PIN transaction security (PTS) certification held by the PIN device
        /// </summary>
        public PCIPTSDeviceIdClass PCIPTSDeviceId { get => PinPadService.PCIPTSDeviceId; set { } }

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
    }
}
