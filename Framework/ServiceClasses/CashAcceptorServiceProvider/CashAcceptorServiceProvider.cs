/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.CashAcceptor;
using XFS4IoTFramework.CashDispenser;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;
using XFS4IoTFramework.BanknoteNeutralization;
using XFS4IoTFramework.Check;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a cash acceptor service provider.
    /// This Service allows to combine CashDispenser for a cash recycler device, or 
    /// CheckScanner for cash and check recycler configuration with IBNS.
    /// </summary>
    public class CashAcceptorServiceProvider : ServiceProvider, ICashAcceptorService, ICashManagementService, ICommonService, IStorageService, ICashDispenserService, IBanknoteNeutralizationService, ICheckService
    {
        public CashAcceptorServiceProvider(
            EndpointDetails endpointDetails, 
            string ServiceName, 
            IDevice device, 
            ILogger logger, 
            IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 [XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.CashAcceptor, XFSConstants.ServiceClass.CashManagement, XFSConstants.ServiceClass.Storage],
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            StorageService = new StorageServiceClass(this, logger, persistentData, StorageTypeEnum.Cash);
            CashManagementService = new CashManagementServiceClass(this, logger, persistentData);
            CashAcceptor = new CashAcceptorServiceClass(this, logger);

            List<XFSConstants.ServiceClass> services = [.. ServiceClasses];
            // Check optional services
            if (device as ICashDispenserDevice is not null)
            {
                CashDispenserService = new CashDispenserServiceClass(this, logger, persistentData);
                services.Add(XFSConstants.ServiceClass.CashDispenser);
            }
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

        private readonly CashAcceptorServiceClass CashAcceptor;
        private readonly CashManagementServiceClass CashManagementService;
        private readonly StorageServiceClass StorageService;
        private readonly CommonServiceClass CommonService;
        // Optional service
        private readonly CashDispenserServiceClass CashDispenserService = null;
        private readonly BanknoteNeutralizationServiceClass IBNSService = null;
        private readonly CheckServiceClass CheckService = null;

        #region CashManagement unsolicited events

        public Task ItemsTakenEvent(CashManagementCapabilitiesClass.PositionEnum Position, string AdditionalBunches = null) => CashManagementService.ItemsTakenEvent(Position, AdditionalBunches);

        public Task ItemsInsertedEvent(CashManagementCapabilitiesClass.PositionEnum Position) => CashManagementService.ItemsInsertedEvent(Position);

        public Task ItemsPresentedEvent(CashManagementCapabilitiesClass.PositionEnum Position, string AdditionalBunches) => CashManagementService.ItemsPresentedEvent(Position, AdditionalBunches);
        
        #endregion

        #region Storage Service

        /// <summary>
        /// Update storage count from the framework after media movement command is processed
        /// </summary>
        public Task UpdateCardStorageCount(string storageId, int countDelta) => throw new NotSupportedException($"CashAcceptor service class doesn't support card storage.");

        /// <summary>
        /// UpdateCashAccounting
        /// Update cash unit status and counts managed by the device specific class.
        /// </summary>
        public async Task UpdateCashAccounting(Dictionary<string, CashUnitCountClass> countDelta = null) => await StorageService.UpdateCashAccounting(countDelta);

        /// <summary>
        /// Update managed check storage information in the framework.
        /// </summary>
        public Task UpdateCheckStorageCount(Dictionary<string, StorageCheckCountClass> countDelta = null) => throw new NotSupportedException($"CashAcceptor service class doesn't support check storage.");

        /// <summary>
        /// Update managed printer storage information in the framework.
        /// </summary>
        public Task UpdatePrinterStorageCount(string storageId, int countDelta) => throw new NotSupportedException($"CashAcceptor service class doesn't support printer storage.");

        /// <summary>
        /// Update managed deposit storage information in the framework.
        /// </summary>
        public Task UpdateDepositStorageCount(string storageId, int countDelta) => throw new NotSupportedException($"CashAcceptor service class doesn't support deposit storage.");


        public void StorePersistent() => StorageService.StorePersistent();

        /// <summary>
        /// Return which type of storage SP is using
        /// </summary>
        public StorageTypeEnum StorageType { get => StorageService.StorageType; init { } }

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

        #region Storage Unsolicsited events 
        /// <summary>
        /// Sending status changed event.
        /// </summary>
        public Task StorageChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => StorageService.StorageChangedEvent(sender, propertyInfo);

        #endregion

        #region Common unsolicited events
        public Task StatusChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => CommonService.StatusChangedEvent(sender, propertyInfo);

        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the CashAcceptor Service.");

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
        /// Stores CashAcceptor interface capabilites internally
        /// </summary>
        public CashAcceptorCapabilitiesClass CashAcceptorCapabilities { get => CommonService.CashAcceptorCapabilities; set => CommonService.CashAcceptorCapabilities = value; }

        /// <summary>
        /// CashAcceptor Status
        /// </summary>
        public CashAcceptorStatusClass CashAcceptorStatus { get => CommonService.CashAcceptorStatus; set => CommonService.CashAcceptorStatus = value; }

        /// <summary>
        /// Stores CashManagement interface capabilites internally
        /// </summary>
        public CashManagementCapabilitiesClass CashManagementCapabilities { get => CommonService.CashManagementCapabilities; set => CommonService.CashManagementCapabilities = value; }

        /// <summary>
        /// CashManagement Status
        /// </summary>
        public CashManagementStatusClass CashManagementStatus { get => CommonService.CashManagementStatus; set => CommonService.CashManagementStatus = value; }

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
        /// Store present status persistently
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

        #region CashAcceptor Service

        /// <summary>
        /// The information about the status of the currently active cash-in transaction or 
        /// in the case where no cash-in transaction is active the status of the most recently ended cash-in transaction. 
        /// </summary>
        public CashInStatusClass CashInStatus { get => CashAcceptor.CashInStatus; init { } }

        /// <summary>
        /// The physical lock/unlock status of the CashAcceptor device and storages.
        /// </summary>
        public DeviceLockStatusClass DeviceLockStatus { get => CashAcceptor.DeviceLockStatus; init { } }

        /// <summary>
        /// The deplete target and destination information
        /// Key - The storage id can be used for target of the depletion operation.
        /// Value - List of storage id can be used for source of the depletion operation
        /// </summary>
        public Dictionary<string, List<string>> DepleteCashUnitSources { get => CashAcceptor.DepleteCashUnitSources; init { } }

        /// <summary>
        /// Which storage units can be specified as targets for a given source storage unit with the CashAcceptor.Replenish command
        /// </summary>
        public List<string> ReplenishTargets { get => CashAcceptor.ReplenishTargets; init { } }

        #endregion

        #region CashDispenser Service

        /// <summary>
        /// Add vendor specific mix algorithm
        /// </summary>
        /// <param name="mixId">ID for the mix</param>
        /// <param name="mix">new mix algorithm to support for a customization</param>
        public void AddMix(string mixId, Mix mix) => CashDispenserService?.AddMix(mixId, mix);

        /// <summary>
        /// Return mix algorithm available
        /// </summary>
        public Mix GetMix(string mixId) => CashDispenserService?.GetMix(mixId);

        /// <summary>
        /// Return mix algorithm supported by the framework or the application set mix tables
        /// </summary>
        public Dictionary<string, Mix> GetMixAlgorithms() => CashDispenserService?.GetMixAlgorithms();

        /// <summary>
        /// Keep last present status
        /// </summary>
        public Dictionary<CashManagementCapabilitiesClass.OutputPositionEnum, CashDispenserPresentStatus> LastCashDispenserPresentStatus { get => CashDispenserService?.LastCashDispenserPresentStatus; init { } }

        /// <summary>
        /// Store present status persistently
        /// </summary>
        public void StoreCashDispenserPresentStatus() => CashDispenserService?.StoreCashDispenserPresentStatus();

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
