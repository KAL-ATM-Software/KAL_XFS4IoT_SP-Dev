/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using XFS4IoTFramework.KeyManagement;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public interface IKeyManagementService : ICommonService
    {
        /// <summary>
        /// Find keyslot available or being used
        /// </summary>
        public int FindKeySlot(string KeyName);

        /// <summary>
        /// Return collection of the detailed key information
        /// </summary>
        /// <returns></returns>
        List<KeyDetail> GetKeyTable();

        /// <summary>
        /// Return key information
        /// </summary>
        KeyDetail GetKeyDetail(string KeyName);

        /// <summary>
        /// Add new key into the collection and return key slot
        /// </summary>
        void AddKey(string KeyName,
                    int KeySlot,
                    string KeyUsage,
                    string Algorithm,
                    string ModeOfUse,
                    int KeyLength,
                    KeyDetail.KeyStatusEnum KeyStatus,
                    bool Preloaded = false,
                    string RestrictedKeyUsage = null,
                    string KeyVersionNumber = null,
                    string Exportability = null,
                    List<byte> OptionalKeyBlockHeader = null,
                    int? Generation = null,
                    DateTime? ActivatingDate = null,
                    DateTime? ExpiryDate = null,
                    int? Version = null);


        /// <summary>
        /// Delete specified key from the collection and return key slot
        /// </summary>
        void DeleteKey(string KeyName);

        /// <summary>
        /// Update key status
        /// </summary>
        void UpdateKeyStatus(string KeyName, KeyDetail.KeyStatusEnum Status);


        /// <summary>
        /// Return secure key entry component status
        /// </summary>
        /// <returns></returns>
        public SecureKeyEntryStatusClass GetSecureKeyEntryStatus();
    }

    public interface IKeyManagementServiceClass : IKeyManagementService, IKeyManagementUnsolicitedEvents
    {
    }
}
