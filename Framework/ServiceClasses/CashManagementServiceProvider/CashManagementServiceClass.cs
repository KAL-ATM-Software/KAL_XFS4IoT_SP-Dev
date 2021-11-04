/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.

\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Storage;

namespace XFS4IoTServer
{
    public partial class CashManagementServiceClass
    {
        public CashManagementServiceClass(IServiceProvider ServiceProvider,
                                          ICommonService CommonService,
                                          IStorageService StorageService,
                                          ILogger logger) 
            : this(ServiceProvider, logger)
        {
            CommonService.IsNotNull($"Unexpected parameter set for common service in the " + nameof(CashManagementServiceClass));
            this.CommonService = CommonService.IsA<ICommonService>($"Invalid interface parameter specified for common service. " + nameof(CashManagementServiceClass));

            StorageService.IsNotNull($"Unexpected parameter set for storage service in the " + nameof(CashManagementServiceClass));
            this.StorageService = StorageService.IsA<IStorageService>($"Invalid interface parameter specified for storage service. " + nameof(CashManagementServiceClass));

            GetStatus();
            GetCapabilities();
        }

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
        /// Capabilities of the CashManagement interface
        /// </summary>
        public CashManagementCapabilitiesClass CashManagementCapabilities { get => CommonService.CashManagementCapabilities; set => CommonService.CashManagementCapabilities = value; }

        /// <summary>
        /// Capabilities of the CashDispenser interface
        /// </summary>
        public CashDispenserCapabilitiesClass CashDispenserCapabilities { get => CommonService.CashDispenserCapabilities; set => CommonService.CashDispenserCapabilities = value; }

        /// <summary>
        /// CashManagement Status
        /// </summary>
        public CashManagementStatusClass CashManagementStatus { get => CommonService.CashManagementStatus; set => CommonService.CashManagementStatus = value; }

        #endregion

        #region Storage Service
        /// <summary>
        /// Storage service interface
        /// </summary>
        private IStorageService StorageService { get; init; }

        /// <summary>
        /// Update storage count from the framework after media movement command is processed
        /// </summary>
        public Task UpdateCardStorageCount(string storageId, int countDelta, string preservedStorage) => throw new NotSupportedException($"The CashManagement interface doesn't aupport card unit information.");

        /// <summary>
        /// UpdateCashAccounting
        /// Update cash unit status and counts managed by the device specific class.
        /// </summary>
        public async Task UpdateCashAccounting(Dictionary<string, CashUnitCountClass> countDelta = null, Dictionary<string, string> preservedStorage = null) => await StorageService.UpdateCashAccounting(countDelta, preservedStorage);

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
        public Dictionary<string, CashUnitStorage> CashUnits { get => StorageService.CashUnits; set { } }

        #endregion

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "CashManagementDev.CashManagementStatus");
            CashManagementStatus = Device.CashManagementStatus;
            Logger.Log(Constants.DeviceClass, "CashManagementDev.CashDispenserStatus=");

            CashManagementStatus.IsNotNull($"The device class set CashManagementStatus property to null. The device class must report device status.");
        }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "CashManagementDev.CashManagementCapabilities");
            CashManagementCapabilities = Device.CashManagementCapabilities;
            Logger.Log(Constants.DeviceClass, "CashManagementDev.CashManagementCapabilities=");

            CashManagementCapabilities.IsNotNull($"The device class set CashManagementCapabilities property to null. The device class must report device capabilities.");
        }
    }
}
