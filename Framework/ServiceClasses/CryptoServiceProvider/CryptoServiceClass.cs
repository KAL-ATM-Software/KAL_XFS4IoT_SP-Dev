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
using XFS4IoT.Crypto.Events;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.KeyManagement;

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
        }

        #region Common Service
        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }

        /// <summary>
        /// Stores KeyManagement interface capabilites internally
        /// </summary>
        public KeyManagementCapabilitiesClass KeyManagementCapabilities { get => CommonService.KeyManagementCapabilities; set { } }

        /// <summary>
        /// Stores Crypto interface capabilites internally
        /// </summary>
        public CryptoCapabilitiesClass CryptoCapabilities { get => CommonService.CryptoCapabilities; set { } }

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
    }
}
