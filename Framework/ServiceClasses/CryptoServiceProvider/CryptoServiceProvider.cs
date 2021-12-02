/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
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
    public class CryptoServiceProvider : ServiceProvider, ICryptoService, IKeyManagementService, ICommonService
    {
        public CryptoServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger, IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.Crypto, XFSConstants.ServiceClass.KeyManagement },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            KeyManagementService = new KeyManagementServiceClass(this, CommonService, logger, persistentData);
            CryptoService = new CryptoServiceClass(this, KeyManagementService, CommonService, logger);
        }

        private readonly CryptoServiceClass CryptoService;
        private readonly KeyManagementServiceClass KeyManagementService;
        private readonly CommonServiceClass CommonService;


        #region KeyManagement unsolicited events
        public Task InitializedEvent() => KeyManagementService.InitializedEvent();

        public Task IllegalKeyAccessEvent(string KeyName, KeyAccessErrorCodeEnum ErrorCode) => KeyManagementService.IllegalKeyAccessEvent(new IllegalKeyAccessEvent.PayloadData(KeyName, ErrorCode switch
                                                                                                                                                                                         {
                                                                                                                                                                                             KeyAccessErrorCodeEnum.AlgorithmNotSupp => XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData.ErrorCodeEnum.AlgorithmNotSupp,
                                                                                                                                                                                             KeyAccessErrorCodeEnum.KeyNotFound => XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData.ErrorCodeEnum.KeyNotFound,
                                                                                                                                                                                             KeyAccessErrorCodeEnum.KeyNoValue => XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData.ErrorCodeEnum.KeyNoValue,
                                                                                                                                                                                             _ => XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData.ErrorCodeEnum.UseViolation,
                                                                                                                                                                                         }));

        public Task CertificateChangeEvent(CertificateChangeEnum CertificateChange) => KeyManagementService.CertificateChangeEvent(new CertificateChangeEvent.PayloadData(CertificateChange switch
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

        /// <summary>
        /// Stores KeyManagement interface capabilites internally
        /// </summary>
        public KeyManagementCapabilitiesClass KeyManagementCapabilities { get => CommonService.KeyManagementCapabilities; set => CommonService.KeyManagementCapabilities = value; }

        /// <summary>
        /// Stores Crypto interface capabilites internally
        /// </summary>
        public CryptoCapabilitiesClass CryptoCapabilities { get => CommonService.CryptoCapabilities; set => CommonService.CryptoCapabilities = value; }

        #endregion

        #region KeyManagement Service

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
    }
}
