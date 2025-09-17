/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.CashManagement.Events;
using XFS4IoT.Common.Events;
using XFS4IoT.Storage.Events;
using XFS4IoTFramework.BanknoteNeutralization;
using XFS4IoTFramework.CashDispenser;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;
using XFS4IoTFramework.Check;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a dispenser service provider.
    /// This Service allows to combile with IBNS and Check device in one service provider.
    /// </summary>
    public class CashDispenserServiceProvider : ServiceProvider, ICashDispenserService, ICashManagementService, ICommonService, IStorageService, ILightsService, IBanknoteNeutralizationService, ICheckService
    {
        public CashDispenserServiceProvider(
            EndpointDetails endpointDetails,
            string ServiceName,
            IDevice device,
            ILogger logger,
            IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 [XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.CashManagement, XFSConstants.ServiceClass.CashDispenser, XFSConstants.ServiceClass.Storage],
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            StorageService = new StorageServiceClass(this, logger, persistentData, StorageTypeEnum.Cash);
            CashManagementService = new CashManagementServiceClass(this, logger, persistentData);
            CashDispenserService = new CashDispenserServiceClass(this, logger, persistentData);

            List<XFSConstants.ServiceClass> services = [.. ServiceClasses];
            // Check optional services
            if (device as IBanknoteNeutralizationDevice is not null)
            {
                IBNSService = new BanknoteNeutralizationServiceClass(this, logger, persistentData);
                services.Add(XFSConstants.ServiceClass.BanknoteNeutralization);
            }
            if (device as ICheckDevice is not null)
            {
                CheckService = new CheckServiceClass(this, logger, persistentData);
                services.Add(XFSConstants.ServiceClass.Check);
            }

            ServiceClasses = services;
        }

        private readonly CashDispenserServiceClass CashDispenserService;
        private readonly CashManagementServiceClass CashManagementService;
        private readonly CommonServiceClass CommonService;
        private readonly StorageServiceClass StorageService;
        // Optional services
        private readonly BanknoteNeutralizationServiceClass IBNSService = null;
        private readonly CheckServiceClass CheckService = null;

        #region CashManagement unsolicited events

        public Task ItemsTakenEvent(CashManagementCapabilitiesClass.PositionEnum Position, string AdditionalBunches = null) => CashManagementService.ItemsTakenEvent(Position, AdditionalBunches);

        public Task ItemsInsertedEvent(CashManagementCapabilitiesClass.PositionEnum Position) => CashManagementService.ItemsInsertedEvent(Position);

        public Task ItemsPresentedEvent(CashManagementCapabilitiesClass.PositionEnum Position, string AdditionalBunches) => CashManagementService.ItemsPresentedEvent(Position, AdditionalBunches);

        #endregion

        #region Common unsolicited events
        public Task StatusChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => CommonService.StatusChangedEvent(sender, propertyInfo);

        public Task NonceClearedEvent(string ReasonDescription) => CommonService.NonceClearedEvent(ReasonDescription);

        public Task ErrorEvent(CommonStatusClass.ErrorEventIdEnum EventId,
                               CommonStatusClass.ErrorActionEnum Action,
                               string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);

        #endregion

        #region Storage unsolicited events
        public Task StorageThresholdEvent(List<string> CashUnitIds)
        {
            StorageThresholdEvent.PayloadData paylod = new()
            {
                ExtendedProperties = GetStorages(CashUnitIds)
            };
            return StorageService.StorageThresholdEvent(paylod);
        }

        /// <summary>
        /// Sending status changed event.
        /// </summary>
        public Task StorageChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => StorageService.StorageChangedEvent(sender, propertyInfo);

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
        /// Stores CashDispenser interface capabilites internally
        /// </summary>
        public CashDispenserCapabilitiesClass CashDispenserCapabilities { get => CommonService.CashDispenserCapabilities; set => CommonService.CashDispenserCapabilities = value; }

        /// <summary>
        /// Stores CashManagement interface capabilites internally
        /// </summary>
        public CashManagementCapabilitiesClass CashManagementCapabilities { get => CommonService.CashManagementCapabilities; set => CommonService.CashManagementCapabilities = value; }

        /// <summary>
        /// Stores IBNS interface capabilites internally
        /// </summary>
        public XFS4IoTFramework.Common.IBNSCapabilitiesClass IBNSCapabilities { get => CommonService.IBNSCapabilities; set => CommonService.IBNSCapabilities = value; }


        /// <summary>
        /// CashDispenser Status
        /// </summary>
        public CashDispenserStatusClass CashDispenserStatus { get => CommonService.CashDispenserStatus; set => CommonService.CashDispenserStatus = value; }

        /// <summary>
        /// CashManagement Status
        /// </summary>
        public CashManagementStatusClass CashManagementStatus { get => CommonService.CashManagementStatus; set => CommonService.CashManagementStatus = value; }

        /// <summary>
        /// IBNS Status
        /// </summary>
        public XFS4IoTFramework.Common.IBNSStatusClass IBNSStatus { get => CommonService.IBNSStatus; set => CommonService.IBNSStatus = value; }


        #endregion

        #region Storage Service

        /// <summary>
        /// Update storage count from the framework after media movement command is processed
        /// </summary>
        public Task UpdateCardStorageCount(string storageId, int countDelta) => throw new NotSupportedException($"CashDispenser service class doesn't support card storage.");

        /// <summary>
        /// UpdateCashAccounting
        /// Update cash unit status and counts managed by the device specific class.
        /// </summary>
        public async Task UpdateCashAccounting(Dictionary<string, CashUnitCountClass> countDelta = null) => await StorageService.UpdateCashAccounting(countDelta);

        /// <summary>
        /// Update managed check storage information in the framework.
        /// </summary>
        public Task UpdateCheckStorageCount(Dictionary<string, StorageCheckCountClass> countDelta = null) => throw new NotSupportedException($"CashDispenser service class doesn't support check storage.");

        /// <summary>
        /// Update managed printer storage information in the framework.
        /// </summary>
        public Task UpdatePrinterStorageCount(string storageId, int countDelta) => throw new NotSupportedException($"CashDispenser service class doesn't support printer storage.");

        /// <summary>
        /// Update managed deposit storage information in the framework.
        /// </summary>
        public Task UpdateDepositStorageCount(string storageId, int countDelta) => throw new NotSupportedException($"CashDispenser service class doesn't support deposit storage.");

        /// <summary>
        /// Return which type of storage SP is using
        /// </summary>
        public StorageTypeEnum StorageType { get => StorageService.StorageType; init { } }

        /// <summary>
        /// Store CardUnits and CashUnits persistently
        /// </summary>
        public void StorePersistent() => StorageService.StorePersistent();

        /// <summary>
        /// Card storage structure information of this device
        /// </summary>
        public Dictionary<string, CardUnitStorage> CardUnits { get => StorageService.CardUnits; init { } }

        /// <summary>
        /// Cash storage structure information of this device
        /// </summary>
        public Dictionary<string, CashUnitStorage> CashUnits { get => StorageService.CashUnits; init { } }

        /// <summary>
        /// Check storage structure information of this device
        /// </summary>
        public Dictionary<string, CheckUnitStorage> CheckUnits { get => StorageService.CheckUnits; init { } }

        /// <summary>
        /// Printer storage structure information of this device
        /// </summary>
        public Dictionary<string, PrinterUnitStorage> PrinterUnits { get => StorageService.PrinterUnits; init { } }

        /// <summary>
        /// IBNS storage structure information of this device
        /// </summary>
        public Dictionary<string, IBNSUnitStorage> IBNSUnits { get => StorageService.IBNSUnits; init { } }

        /// <summary>
        /// Deposit storage structure information of this device
        /// </summary>
        public Dictionary<string, DepositUnitStorage> DepositUnits { get => StorageService.DepositUnits; init { } }

        /// <summary>
        /// Return XFS4IoT storage structured object.
        /// </summary>
        public Dictionary<string, XFS4IoT.Storage.StorageUnitClass> GetStorages(List<string> UnitIds) => StorageService.GetStorages(UnitIds);

        #endregion

        #region CashDispenser Service
        /// <summary>
        /// Add vendor specific mix algorithm
        /// </summary>
        /// <param name="mixId">ID for the mix</param>
        /// <param name="mix">new mix algorithm to support for a customization</param>
        public void AddMix(string mixId, Mix mix) => CashDispenserService.AddMix(mixId, mix);

        /// <summary>
        /// Return mix algorithm available
        /// </summary>
        public Mix GetMix(string mixId) => CashDispenserService.GetMix(mixId);

        /// <summary>
        /// Return mix algorithm supported by the framework or the application set mix tables
        /// </summary>
        public Dictionary<string, Mix> GetMixAlgorithms() => CashDispenserService.GetMixAlgorithms();

        /// <summary>
        /// Keep last present status
        /// </summary>
        public Dictionary<CashManagementCapabilitiesClass.OutputPositionEnum, CashDispenserPresentStatus> LastCashDispenserPresentStatus { get => CashDispenserService.LastCashDispenserPresentStatus; init { } }

        /// <summary>
        /// Store present status for CashDispenser service persistently
        /// </summary>
        public void StoreCashDispenserPresentStatus() => CashDispenserService.StoreCashDispenserPresentStatus();

        #endregion

        #region CashManagement Service

        /// <summary>
        /// The framework maintains cash-in status
        /// </summary>
        public CashInStatusClass CashInStatusManaged { get => CashManagementService.CashInStatusManaged; init { } }

        /// <summary>
        /// Store cash-in in status persistently
        /// </summary>
        public void StoreCashInStatus() => CashManagementService.StoreCashInStatus();

        /// <summary>
        /// The last status of the most recent attempt to present or return items to the customer. 
        /// </summary>
        public Dictionary<CashManagementCapabilitiesClass.PositionEnum, CashManagementPresentStatus> LastCashManagementPresentStatus { get => CashManagementService.LastCashManagementPresentStatus; init { } }

        /// <summary>
        /// Store present status for CashManagement Service persistently
        /// </summary>
        public void StoreCashManagementPresentStatus() => CashManagementService.StoreCashManagementPresentStatus();

        /// <summary>
        /// This list provides the functionality to blacklist notes and allows additional flexibility, for example to specify that notes can be taken out of circulation
        /// by specifying them as unfit.Any items not returned in this list will be handled according to normal classification rules.
        /// </summary>
        public ItemClassificationListClass ItemClassificationList { get => CashManagementService.ItemClassificationList; init { } }

        /// <summary>
        /// Store classification list persistently
        /// </summary>
        public void StoreItemClassificationList() => CashManagementService.StoreItemClassificationList();

        #endregion

        #region IBNS Service

        /// <summary>
        /// Set storage status from the device class
        /// </summary>
        public void UpdateStorageStatus(string storageId, UnitStorageBase.StatusEnum storageStatus) => IBNSService?.UpdateStorageStatus(storageId, storageStatus);

        #endregion

        #region Check Service

        /// <summary>
        /// Store transaction status
        /// </summary>
        public TransactionStatus LastTransactionStatus { get => CheckService?.LastTransactionStatus; init { } }

        /// <summary>
        /// Store transaction status persistently
        /// </summary>
        public void StoreTransactionStatus() => CheckService?.StoreTransactionStatus();

        #endregion
    }
}
