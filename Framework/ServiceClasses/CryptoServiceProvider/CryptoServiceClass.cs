/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;

using XFS4IoTFramework.Common;
using XFS4IoTFramework.KeyManagement;
using XFS4IoTFramework.Crypto;

namespace XFS4IoTServer
{
    public partial class CryptoServiceClass
    {
        public CryptoServiceClass(IServiceProvider ServiceProvider,
                                  IKeyManagementService KeyManagementService,
                                  ICommonService CommonService,
                                  ILogger logger)
        : this(ServiceProvider, logger)
        {
            KeyManagementService.IsNotNull($"Unexpected parameter set for key management service in the " + nameof(CryptoServiceClass));
            this.KeyManagementService = KeyManagementService.IsA<IKeyManagementService>($"Invalid interface parameter specified for key management service. " + nameof(CryptoServiceClass));

            CommonService.IsNotNull($"Unexpected parameter set for common service in the " + nameof(CryptoServiceClass));
            this.CommonService = CommonService.IsA<ICommonService>($"Invalid interface parameter specified for common service. " + nameof(CryptoServiceClass));

            GetCapabilities();
        }

        #region KeyManagement unsolicited events
        public Task IllegalKeyAccessEvent(string KeyName, KeyAccessErrorCodeEnum ErrorCode) => KeyManagementService.IllegalKeyAccessEvent(KeyName, ErrorCode);

        public Task CertificateChangeEvent(CertificateChangeEnum CertificateChange) => KeyManagementService.CertificateChangeEvent(CertificateChange);

        #endregion

        #region Common unsolicited events
        public Task StatusChangedEvent(CommonStatusClass.DeviceEnum? Device,
                                       CommonStatusClass.PositionStatusEnum? Position,
                                       int? PowerSaveRecoveryTime,
                                       CommonStatusClass.AntiFraudModuleEnum? AntiFraudModule,
                                       CommonStatusClass.ExchangeEnum? Exchange,
                                       CommonStatusClass.EndToEndSecurityEnum? EndToEndSecurity) => CommonService.StatusChangedEvent(Device,
                                                                                                                                     Position,
                                                                                                                                     PowerSaveRecoveryTime,
                                                                                                                                     AntiFraudModule,
                                                                                                                                     Exchange,
                                                                                                                                     EndToEndSecurity);
        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the Crypto Service.");

        public Task ErrorEvent(CommonStatusClass.ErrorEventIdEnum EventId,
                               CommonStatusClass.ErrorActionEnum Action,
                               string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);

        #endregion

        #region Common Service
        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }

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

        #region Key Management Service
        /// <summary>
        /// KeyManagement service interface
        /// </summary>
        private IKeyManagementService KeyManagementService { get; init; }

        /// <summary>
        /// Find keyslot available or being used
        /// </summary>
        public int FindKeySlot(string KeyName) => throw new NotSupportedException("The UpdateKeyStatus method is not supported in the Crypto interface.");

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
                           string RestrictedKeyUsage,
                           string KeyVersionNumber,
                           string Exportability,
                           List<byte> OptionalKeyBlockHeader,
                           int? Generation,
                           DateTime? ActivatingDate,
                           DateTime? ExpiryDate,
                           int? Version) => throw new NotSupportedException("The AddKey method is not supported in the Crypto interface.");

        /// <summary>
        /// Delete specified key from the collection and return key slot
        /// </summary>
        public void DeleteKey(string KeyName) => throw new NotSupportedException("The DeleteKey method is not supported in the Crypto interface.");

        /// <summary>
        /// Update key status
        /// </summary>
        public void UpdateKeyStatus(string KeyName, KeyDetail.KeyStatusEnum Status) => throw new NotSupportedException("The UpdateKeyStatus method is not supported in the Crypto interface.");

        /// <summary>
        /// Return secure key entry component status
        /// </summary>
        /// <returns></returns>
        public SecureKeyEntryStatusClass GetSecureKeyEntryStatus() => throw new NotSupportedException("The GetSecureKeyEntryStatus method is not supported in the Crypto interface.");

        #endregion

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "CryptoDev.CryptoCapabilities");
            CryptoCapabilities = Device.CryptoCapabilities;
            Logger.Log(Constants.DeviceClass, "CryptoDev.CryptoCapabilities=");

            CryptoCapabilities.IsNotNull($"The device class set CryptoCapabilities property to null. The device class must report device capabilities.");
        }
    }
}
