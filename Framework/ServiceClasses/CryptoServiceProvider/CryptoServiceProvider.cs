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
using XFS4IoT.Crypto.Events;
using XFS4IoT.Common.Events;
using XFS4IoT.KeyManagement.Events;
using XFS4IoTFramework.KeyManagement;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a crypto service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a service provider for the crypto device. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class CryptoServiceProvider : ServiceProvider, ICryptoServiceClass, IKeyManagementServiceClass, ICommonServiceClass
    {
        public CryptoServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger, IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.Crypto, XFSConstants.ServiceClass.KeyManagement },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger);
            KeyManagementService = new KeyManagementServiceClass(this, CommonService, logger, persistentData);
            CryptoService = new CryptoServiceClass(this, KeyManagementService, CommonService, logger);
        }

        private readonly CryptoServiceClass CryptoService;
        private readonly KeyManagementServiceClass KeyManagementService;
        private readonly CommonServiceClass CommonService;


        #region Crypto unsolicited events
        public Task IllegalKeyAccessEvent(XFS4IoT.Crypto.Events.IllegalKeyAccessEvent.PayloadData Payload) => CryptoService.IllegalKeyAccessEvent(Payload);
        #endregion

        #region KeyManagement unsolicited events
        public Task InitializedEvent(InitializedEvent.PayloadData Payload) => KeyManagementService.InitializedEvent(Payload);

        public Task IllegalKeyAccessEvent(XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData Payload) => KeyManagementService.IllegalKeyAccessEvent(Payload);

        public Task CertificateChangeEvent(CertificateChangeEvent.PayloadData Payload) => KeyManagementService.CertificateChangeEvent(Payload);

        #endregion

        #region Common unsolicited events
        public Task PowerSaveChangeEvent(PowerSaveChangeEvent.PayloadData Payload) => CommonService.PowerSaveChangeEvent(Payload);

        public Task DevicePositionEvent(DevicePositionEvent.PayloadData Payload) => CommonService.DevicePositionEvent(Payload);

        public Task NonceClearedEvent(NonceClearedEvent.PayloadData Payload) => CommonService.NonceClearedEvent(Payload);
        #endregion


        /// <summary>
        /// Stores KeyManagement interface capabilites internally
        /// </summary>
        public KeyManagementCapabilitiesClass KeyManagementCapabilities { get => CommonService.KeyManagementCapabilities; set => CommonService.KeyManagementCapabilities = value; }

        /// <summary>
        /// Stores Crypto interface capabilites internally
        /// </summary>
        public CryptoCapabilitiesClass CryptoCapabilities { get => CommonService.CryptoCapabilities; set => CommonService.CryptoCapabilities = value; }

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
    }
}
