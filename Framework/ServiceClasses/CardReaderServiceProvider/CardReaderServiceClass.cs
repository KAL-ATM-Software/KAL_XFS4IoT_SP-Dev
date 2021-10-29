/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
using XFS4IoT.Storage.Events;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTServer
{
    public partial class CardReaderServiceClass
    {
        public CardReaderServiceClass(IServiceProvider ServiceProvider,
                                      ICommonService CommonService,
                                      IStorageService StorageService,
                                      ILogger logger)
            : this(ServiceProvider, logger)
        {
            CommonService.IsNotNull($"Unexpected parameter set for common service in the " + nameof(CardReaderServiceClass));
            this.CommonService = CommonService.IsA<ICommonService>($"Invalid interface parameter specified for common service. " + nameof(CardReaderServiceClass));

            StorageService.IsNotNull($"Unexpected parameter set in the " + nameof(CardReaderServiceClass));
            this.StorageService = StorageService.IsA<IStorageService>($"Invalid interface parameter specified for storage service. " + nameof(CardReaderServiceClass));
        }

        #region Common Service
        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        /// <summary>
        /// Stores CardReader interface capabilites internally
        /// </summary>
        public CardReaderCapabilitiesClass CardReaderCapabilities { get => CommonService.CardReaderCapabilities; set { } }
        #endregion

        #region Storage Service
        /// <summary>
        /// Storage service interface
        /// </summary>
        public IStorageService StorageService { get; init; }

        /// <summary>
        /// Update storage count from the framework after media movement command is processed
        /// </summary>
        public async Task UpdateCardStorageCount(string storageId, int count, string preservedStorage) => await StorageService.UpdateCardStorageCount(storageId, count, preservedStorage);

        /// <summary>
        /// UpdateCashAccounting
        /// Update cash unit status and counts managed by the device specific class.
        /// </summary>
        public Task UpdateCashAccounting(Dictionary<string, CashUnitCountClass> countDelta, Dictionary<string, string> preservedStorage) => throw new NotSupportedException($"CardReader service provider doesn't support cash storage.");

        /// <summary>
        /// Return which type of storage SP is using
        /// </summary>
        public StorageTypeEnum StorageType { get => StorageService.StorageType; set { } }

        /// <summary>
        /// Store CardUnits and CashUnits persistently
        /// </summary>
        public void StorePersistent() => StorageService.StorePersistent();

        /// <summary>
        /// Card storage structure information of this device
        /// </summary>
        public Dictionary<string, CardUnitStorage> CardUnits { get => StorageService.CardUnits; set { } }

        /// <summary>
        /// Cash storage structure information of this device
        /// </summary>
        public Dictionary<string, CashUnitStorage> CashUnits { get => throw new NotSupportedException($"CardReader service provider doesn't support cash storage."); set { } }
        #endregion
    }
}
