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
using XFS4IoT.CashManagement.Events;
using XFS4IoT.Common.Events;
using XFS4IoT.Storage.Events;
using XFS4IoTFramework.CashDispenser;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a dispenser service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical dispenser, which only implements the Dispenser, CashManagement and Common interfaces. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class CashDispenserServiceProvider : ServiceProvider, ICashDispenserServiceClass, ICashManagementServiceClass, ICommonServiceClass, IStorageServiceClass, ILightsServiceClass
    {
        public CashDispenserServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger, IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.CashManagement, XFSConstants.ServiceClass.CashDispenser, XFSConstants.ServiceClass.Storage },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger);
            StorageService = new StorageServiceClass(this, CommonService, logger, persistentData, StorageTypeEnum.Cash);
            CashManagementService = new CashManagementServiceClass(this, CommonService, StorageService, logger);
            CashDispenserService = new CashDispenserServiceClass(this, CashManagementService, CommonService, logger, persistentData);
        }

        private readonly CashDispenserServiceClass CashDispenserService;
        private readonly CashManagementServiceClass CashManagementService;
        private readonly CommonServiceClass CommonService;
        private readonly StorageServiceClass StorageService;

        #region CashManagement unsolicited events
        public Task TellerInfoChangedEvent(TellerInfoChangedEvent.PayloadData Payload) => CashManagementService.TellerInfoChangedEvent(Payload);

        public Task SafeDoorOpenEvent() => CashManagementService.SafeDoorOpenEvent();

        public Task SafeDoorClosedEvent() => CashManagementService.SafeDoorClosedEvent();
        #endregion

        #region Common unsolicited events
        public Task PowerSaveChangeEvent(PowerSaveChangeEvent.PayloadData Payload) => CommonService.PowerSaveChangeEvent(Payload);

        public Task DevicePositionEvent(DevicePositionEvent.PayloadData Payload) => CommonService.DevicePositionEvent(Payload);

        public Task NonceClearedEvent(NonceClearedEvent.PayloadData Payload) => CommonService.NonceClearedEvent(Payload);

        public Task ExchangeStateChangedEvent(ExchangeStateChangedEvent.PayloadData Payload) => CommonService.ExchangeStateChangedEvent(Payload);
        #endregion

        #region Storage unsolic events
        public Task StorageThresholdEvent(StorageThresholdEvent.PayloadData Payload) => StorageService.StorageThresholdEvent(Payload);

        public Task StorageChangedEvent(StorageChangedEvent.PayloadData Payload) => StorageService.StorageChangedEvent(Payload);

        public Task StorageErrorEvent(StorageErrorEvent.PayloadData Payload) => StorageService.StorageErrorEvent(Payload);
        #endregion

        #region Common Service

        /// <summary>
        /// Stores CashDispenser interface capabilites internally
        /// </summary>
        public CashDispenserCapabilitiesClass CashDispenserCapabilities { get => CommonService.CashDispenserCapabilities; set => CommonService.CashDispenserCapabilities = value; }

        /// <summary>
        /// Stores CashManagement interface capabilites internally
        /// </summary>
        public CashManagementCapabilitiesClass CashManagementCapabilities { get => CommonService.CashManagementCapabilities; set => CommonService.CashManagementCapabilities = value; }

        #endregion

        #region Cash Management Service

        /// <summary>
        /// Update storage count from the framework after media movement command is processed
        /// </summary>
        public Task UpdateCardStorageCount(string storageId, int countDelta, string preservedStorage) => throw new NotSupportedException($"CashManagement service class doesn't support card storage.");

        /// <summary>
        /// UpdateCashAccounting
        /// Update cash unit status and counts managed by the device specific class.
        /// </summary>
        public async Task UpdateCashAccounting(Dictionary<string, CashUnitCountClass> countDelta = null, Dictionary<string, string> preservedStorage = null) => await CashManagementService.UpdateCashAccounting(countDelta, preservedStorage);

        /// <summary>
        /// Return which type of storage SP is using
        /// </summary>
        public StorageTypeEnum StorageType { get => CashManagementService.StorageType; set => CashManagementService.StorageType = value; }

        /// <summary>
        /// Store CardUnits and CashUnits persistently
        /// </summary>
        public void StorePersistent() => CashManagementService.StorePersistent();

        /// <summary>
        /// Card storage structure information of this device
        /// </summary>
        public Dictionary<string, CardUnitStorage> CardUnits { get => CashManagementService.CardUnits; set => CashManagementService.CardUnits = value; }

        /// <summary>
        /// Cash storage structure information of this device
        /// </summary>
        public Dictionary<string, CashUnitStorage> CashUnits { get => CashManagementService.CashUnits; set => CashManagementService.CashUnits = value; }

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
        public Dictionary<CashDispenserCapabilitiesClass.OutputPositionEnum, PresentStatus> LastPresentStatus { get => CashDispenserService.LastPresentStatus; set => CashDispenserService.LastPresentStatus = value; }

        #endregion
    }
}
