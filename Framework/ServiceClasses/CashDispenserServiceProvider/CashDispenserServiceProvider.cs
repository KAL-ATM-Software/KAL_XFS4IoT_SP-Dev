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
    public class CashDispenserServiceProvider : ServiceProvider, ICashDispenserService, ICashManagementService, ICommonService, IStorageService, ILightsService
    {
        public CashDispenserServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger, IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.CashManagement, XFSConstants.ServiceClass.CashDispenser, XFSConstants.ServiceClass.Storage },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            StorageService = new StorageServiceClass(this, CommonService, logger, persistentData, StorageTypeEnum.Cash);
            CashManagementService = new CashManagementServiceClass(this, CommonService, StorageService, logger);
            CashDispenserService = new CashDispenserServiceClass(this, CashManagementService, CommonService, logger, persistentData);
        }

        private readonly CashDispenserServiceClass CashDispenserService;
        private readonly CashManagementServiceClass CashManagementService;
        private readonly CommonServiceClass CommonService;
        private readonly StorageServiceClass StorageService;

        #region CashManagement unsolicited events

        public Task SafeDoorOpenEvent() => CashManagementService.SafeDoorOpenEvent();

        public Task SafeDoorClosedEvent() => CashManagementService.SafeDoorClosedEvent();

        public Task ItemsTakenEvent(CashDispenserCapabilitiesClass.OutputPositionEnum Position, string AdditionalBunches = null) => CashManagementService.ItemsTakenEvent(Position, AdditionalBunches);

        public Task ItemsInsertedEvent(CashDispenserCapabilitiesClass.OutputPositionEnum Postion) => CashManagementService.ItemsInsertedEvent(Postion);

        public Task ItemsPresentedEvent(CashDispenserCapabilitiesClass.OutputPositionEnum Position, string AdditionalBunches) => CashManagementService.ItemsPresentedEvent(Position, AdditionalBunches);

        public Task ShutterStatusChangedEvent(CashManagementCapabilitiesClass.PositionEnum Position, CashManagementStatusClass.ShutterEnum Status) => CashManagementService.ShutterStatusChangedEvent(Position, Status);

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


        public Task NonceClearedEvent(string ReasonDescription) => CommonService.NonceClearedEvent(ReasonDescription);

        public Task ErrorEvent(CommonStatusClass.ErrorEventIdEnum EventId, 
                               CommonStatusClass.ErrorActionEnum Action, 
                               string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);

        #endregion

        #region Storage unsolic events
        public Task StorageThresholdEvent(List<string> CashUnitIds)
        {
            StorageThresholdEvent.PayloadData paylod = new();
            paylod.ExtendedProperties = GetStorages(CashUnitIds);
            return StorageService.StorageThresholdEvent(paylod);
        }

        public Task StorageChangedEvent(List<string> CashUnitIds)
        {
            StorageChangedEvent.PayloadData paylod = new();
            paylod.ExtendedProperties = GetStorages(CashUnitIds);
            return StorageService.StorageChangedEvent(paylod);
        }

        public Task StorageErrorEvent(FailureEnum Failure, List<string> CashUnitIds) 
        {
            Dictionary<string, XFS4IoT.Storage.StorageUnitClass> storages = GetStorages(CashUnitIds);
            return StorageService.StorageErrorEvent(new StorageErrorEvent.PayloadData(Failure switch
            {
                FailureEnum.Config => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Config,
                FailureEnum.Empty => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Empty,
                FailureEnum.Error => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Error,
                FailureEnum.Full => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Full,
                FailureEnum.Invalid => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Invalid,
                FailureEnum.Locked => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Locked,
                _ => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.NotConfigured,
            },
                                                                                     storages));
        }
        private Dictionary<string, XFS4IoT.Storage.StorageUnitClass> GetStorages(List<string> CashUnitIds)
        {
            Dictionary<string, XFS4IoT.Storage.StorageUnitClass> storages = new();

            foreach (var storageId in CashUnitIds)
            {
                if (!StorageService.CashUnits.ContainsKey(storageId))
                    continue;

                storages.Add(storageId,
                             new(Id: CashUnits[storageId].Id,
                                 PositionName: CashUnits[storageId].PositionName,
                                 Capacity: CashUnits[storageId].Capacity,
                                 Status: CashUnits[storageId].Status switch
                                 {
                                     CashUnitStorage.StatusEnum.Good => XFS4IoT.Storage.StatusEnum.Ok,
                                     CashUnitStorage.StatusEnum.Inoperative => XFS4IoT.Storage.StatusEnum.Inoperative,
                                     CashUnitStorage.StatusEnum.Manipulated => XFS4IoT.Storage.StatusEnum.Manipulated,
                                     CashUnitStorage.StatusEnum.Missing => XFS4IoT.Storage.StatusEnum.Missing,
                                     _ => XFS4IoT.Storage.StatusEnum.NotConfigured,
                                 },
                                 SerialNumber: CashUnits[storageId].SerialNumber,
                                 Cash: new XFS4IoT.CashManagement.StorageCashClass(
                                        new XFS4IoT.CashManagement.StorageCashCapabilitiesClass(new XFS4IoT.CashManagement.StorageCashTypesClass(CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                                                                                                                                 CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                                                                                                                                 CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                                                                                                                                 CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                                                                                                                                 CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                                                                                                                                 CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)),
                                                                                                new XFS4IoT.CashManagement.StorageCashItemTypesClass(CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                                                                                                                                     CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                                                                                                                                     CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                                                                                                                                     CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Conterfeit),
                                                                                                                                                     CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                                                                                                                                     CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                                                                                                                                     CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                                                                                                                                     CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)),
                                                                                                CashUnits[storageId].Unit.Capabilities.HardwareSensors,
                                                                                                CashUnits[storageId].Unit.Capabilities.RetractAreas,
                                                                                                CashUnits[storageId].Unit.Capabilities.RetractThresholds,
                                                                                                CashUnits[storageId].Unit.Capabilities.BanknoteItems),
                                        new XFS4IoT.CashManagement.StorageCashConfigurationClass(new XFS4IoT.CashManagement.StorageCashTypesClass(CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                                                                                                                                  CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                                                                                                                                  CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                                                                                                                                  CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                                                                                                                                  CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                                                                                                                                  CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)),
                                                                                                new XFS4IoT.CashManagement.StorageCashItemTypesClass(CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                                                                                                                                     CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                                                                                                                                     CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                                                                                                                                     CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Conterfeit),
                                                                                                                                                     CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                                                                                                                                     CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                                                                                                                                     CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                                                                                                                                     CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)),
                                                                                                CashUnits[storageId].Unit.Configuration.Currency,
                                                                                                CashUnits[storageId].Unit.Configuration.Value,
                                                                                                CashUnits[storageId].Unit.Configuration.HighThreshold,
                                                                                                CashUnits[storageId].Unit.Configuration.LowThreshold,
                                                                                                CashUnits[storageId].Unit.Configuration.AppLockIn,
                                                                                                CashUnits[storageId].Unit.Configuration.AppLockOut,
                                                                                                CashUnits[storageId].Unit.Configuration.BanknoteItems),
                                        new XFS4IoT.CashManagement.StorageCashStatusClass(CashUnits[storageId].Unit.Status.Index,
                                                                                          CashUnits[storageId].Unit.Status.InitialCounts.CopyTo(),
                                                                                            new XFS4IoT.CashManagement.StorageCashOutClass(CashUnits[storageId].Unit.Status.StorageCashOutCount.Presented.CopyTo(),
                                                                                                                                           CashUnits[storageId].Unit.Status.StorageCashOutCount.Rejected.CopyTo(),
                                                                                                                                           CashUnits[storageId].Unit.Status.StorageCashOutCount.Distributed.CopyTo(),
                                                                                                                                           CashUnits[storageId].Unit.Status.StorageCashOutCount.Unknown.CopyTo(),
                                                                                                                                           CashUnits[storageId].Unit.Status.StorageCashOutCount.Stacked.CopyTo(),
                                                                                                                                           CashUnits[storageId].Unit.Status.StorageCashOutCount.Diverted.CopyTo(),
                                                                                                                                           CashUnits[storageId].Unit.Status.StorageCashOutCount.Transport.CopyTo()),
                                                                                            new XFS4IoT.CashManagement.StorageCashInClass(CashUnits[storageId].Unit.Status.StorageCashInCount.RetractOperations,
                                                                                                                                          CashUnits[storageId].Unit.Status.StorageCashInCount.Deposited.CopyTo(),
                                                                                                                                          CashUnits[storageId].Unit.Status.StorageCashInCount.Retracted.CopyTo(),
                                                                                                                                          CashUnits[storageId].Unit.Status.StorageCashInCount.Rejected.CopyTo(),
                                                                                                                                          CashUnits[storageId].Unit.Status.StorageCashInCount.Distributed.CopyTo(),
                                                                                                                                          CashUnits[storageId].Unit.Status.StorageCashInCount.Transport.CopyTo()),
                                                                                            CashUnits[storageId].Unit.Status.Accuracy switch
                                                                                            {
                                                                                                CashStatusClass.AccuracyEnum.Accurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Accurate,
                                                                                                CashStatusClass.AccuracyEnum.AccurateSet => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.AccurateSet,
                                                                                                CashStatusClass.AccuracyEnum.Inaccurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Inaccurate,
                                                                                                CashStatusClass.AccuracyEnum.NotSupported => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.NotSupported,
                                                                                                _ => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Unknown,
                                                                                            },
                                                                                            CashUnits[storageId].Unit.Status.ReplenishmentStatus switch
                                                                                            {
                                                                                                CashStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Empty,
                                                                                                CashStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Full,
                                                                                                CashStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Ok,
                                                                                                CashStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.CashManagement.ReplenishmentStatusEnum.High,
                                                                                                _ => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Low,
                                                                                            })
                                        ),
                                Card: null)
                             );
            }

            return storages;
        }
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
        /// CashDispenser Status
        /// </summary>
        public CashDispenserStatusClass CashDispenserStatus { get => CommonService.CashDispenserStatus; set => CommonService.CashDispenserStatus = value; }

        /// <summary>
        /// CashManagement Status
        /// </summary>
        public CashManagementStatusClass CashManagementStatus { get => CommonService.CashManagementStatus; set => CommonService.CashManagementStatus = value; }

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
