/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using XFS4IoT;
using XFS4IoTFramework.Storage;
using XFS4IoTFramework.Common;
using XFS4IoT.Storage.Events;
using XFS4IoT.Storage;
using XFS4IoT.CashManagement;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace XFS4IoTServer
{
    public partial class StorageServiceClass
    {
        public StorageServiceClass(IServiceProvider ServiceProvider,
                                   ILogger logger, 
                                   IPersistentData PersistentData, 
                                   StorageTypeEnum StorageType)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(StorageServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IStorageDevice>();

            RegisterFactory(ServiceProvider);

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(StorageServiceClass)}");

            this.PersistentData = PersistentData;
            this.StorageType = StorageType;

            // Load persistent data
            CardUnits = PersistentData.Load<Dictionary<string, CardUnitStorage>>(ServiceProvider.Name + typeof(CardUnitStorage).FullName);
            if (CardUnits is null)
            {
                Logger.Warning(Constants.Framework, "Failed to load persistent data for card units. It could be a first run, SP type is not CardReader or no persistent exists on the file system.");
                CardUnits = [];
            }
            CashUnits = PersistentData.Load<Dictionary<string, CashUnitStorage>>(ServiceProvider.Name + typeof(CashUnitStorage).FullName);
            if (CashUnits is null)
            {
                Logger.Warning(Constants.Framework, "Failed to load persistent data for cash units. It could be a first run, service is not Cash Recycler/Dispenser/Acceptor or no persistent exists on the file system.");
                CashUnits = [];
            }
            CheckUnits = PersistentData.Load<Dictionary<string, CheckUnitStorage>>(ServiceProvider.Name + typeof(CheckUnitStorage).FullName);
            if (CheckUnits is null)
            {
                Logger.Warning(Constants.Framework, "Failed to load persistent data for check units. It could be a first run, service is not Check or no persistent exists on the file system.");
                CheckUnits = [];
            }
            PrinterUnits = PersistentData.Load<Dictionary<string, PrinterUnitStorage>>(ServiceProvider.Name + typeof(PrinterUnitStorage).FullName);
            if (PrinterUnits is null)
            {
                Logger.Warning(Constants.Framework, "Failed to load persistent data for printer units. It could be a first run, service is not Printer or no persistent exists on the file system.");
                PrinterUnits = [];
            }
            IBNSUnits = PersistentData.Load<Dictionary<string, IBNSUnitStorage>>(ServiceProvider.Name + typeof(IBNSUnitStorage).FullName);
            if (IBNSUnits is null)
            {
                Logger.Warning(Constants.Framework, "Failed to load persistent data for IBNS units. It could be a first run, service is not IBNS or no persistent exists on the file system.");
                IBNSUnits = [];
            }
            DepositUnits = PersistentData.Load<Dictionary<string, DepositUnitStorage>>(ServiceProvider.Name + typeof(DepositUnitStorage).FullName);
            if (DepositUnits is null)
            {
                Logger.Warning(Constants.Framework, "Failed to load persistent data for Deposit units. It could be a first run, service is not Deposit or no persistent exists on the file system.");
                DepositUnits = [];
            }

            // Get unit information from the device class
            if (StorageType.HasFlag(StorageTypeEnum.Card))
            {
                ConstructCardStorage();
            }
            if (StorageType.HasFlag(StorageTypeEnum.Printer))
            {
                ConstructPrinterStorage();
            }
            if (StorageType.HasFlag(StorageTypeEnum.Cash))
            {
                ConstructCashUnits();
            }
            if (StorageType.HasFlag(StorageTypeEnum.Check))
            {
                ConstructCheckUnits();
            }
            if (StorageType.HasFlag(StorageTypeEnum.IBNS))
            {
                ConstructIBNSUnits();
            }
            if (StorageType.HasFlag(StorageTypeEnum.Deposit))
            {
                ConstructDepositUnits();
            }

            // Register events
            RegisterStorageChangedEvents();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }

        #region Card
        private void ConstructCardStorage()
        {
            Logger.Log(Constants.DeviceClass, "StorageDev.GetCardStorageConfiguration()");

            bool newConfiguration = Device.GetCardStorageConfiguration(out Dictionary<string, CardUnitStorageConfiguration> newCardUnits);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCardStorageConfiguration()-> {newConfiguration}");

            // first to update capabilites and configuration part of storage information
            if (newConfiguration)
            {
                if (newCardUnits is null ||
                    newCardUnits?.Count == 0)
                {
                    Logger.Warning(Constants.Framework, $"The function GetCardStorageConfiguration returned true. however, there is not output data supplied.");
                    newConfiguration = false;
                }
                else
                {
                    CardUnits.Clear();
                    foreach (var unit in newCardUnits)
                    {
                        CardUnits.Add(unit.Key, new CardUnitStorage(unit.Value));
                    }
                }
            }

            if (!newConfiguration)
            {
                bool identical = newCardUnits?.Count == CardUnits.Count;
                if (newCardUnits is not null)
                {
                    foreach (var unit in newCardUnits)
                    {
                        identical = CardUnits.ContainsKey(unit.Key);
                        if (!identical)
                        {
                            Logger.Warning(Constants.Framework, $"Existing card unit information doesn't contain key specified by the device class. {unit.Key}. Construct new card unit infomation.");
                            break;
                        }

                        identical = CardUnits[unit.Key].Unit.Configuration == unit.Value.Configuration &&
                                    CardUnits[unit.Key].Unit.Capabilities == unit.Value.Capabilities &&
                                    CardUnits[unit.Key].Capacity == unit.Value.Capacity &&
                                    CardUnits[unit.Key].PositionName == unit.Value.PositionName &&
                                    CardUnits[unit.Key].SerialNumber == unit.Value.SerialNumber;

                        if (!identical)
                        {
                            Logger.Warning(Constants.Framework, $"Existing card unit information doesn't have an identical storage structure information specified by the device class. {unit.Key}. Construct new card unit infomation.");
                            break;
                        }
                    }
                }

                if (!identical)
                {
                    CardUnits.Clear();
                    if (newCardUnits is not null)
                    {
                        foreach (var unit in newCardUnits)
                        {
                            CardUnits.Add(unit.Key, new CardUnitStorage(unit.Value));
                        }
                    }
                }
            }

            // Update count from device
            Logger.Log(Constants.DeviceClass, "StorageDev.GetCardUnitCounts()");

            bool updateCounts = Device.GetCardUnitCounts(out Dictionary<string, CardUnitCount> unitCounts);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCardUnitCounts()-> {updateCounts}");

            if (updateCounts &&
                unitCounts is not null)
            {
                foreach (var status in unitCounts)
                {
                    if (!CardUnits.ContainsKey(status.Key))
                    {
                        Logger.Warning(Constants.Framework, $"Specified card unit ID is not found on GetCardUnitCounts. {status.Key}");
                        continue;
                    }
                    CardUnits[status.Key].Unit.Status.InitialCount = status.Value.InitialCount;
                    CardUnits[status.Key].Unit.Status.Count = status.Value.Count;
                    CardUnits[status.Key].Unit.Status.RetainCount = status.Value.RetainCount;
                }
            }

            foreach (var unit in CardUnits)
            {
                // update status logically first and overwrite status if the device class requires.
                if (unit.Value.Unit.Status.Count >= unit.Value.Capacity)
                {
                    unit.Value.Unit.Status.ReplenishmentStatus = CardStatusClass.ReplenishmentStatusEnum.Full;
                }
                else if (unit.Value.Unit.Status.Count == 0)
                {
                    unit.Value.Unit.Status.ReplenishmentStatus = CardStatusClass.ReplenishmentStatusEnum.Empty;
                }
                else if (unit.Value.Unit.Configuration.Threshold != 0)
                {
                    if (unit.Value.Unit.Capabilities.Type == CardCapabilitiesClass.TypeEnum.Retain &&
                        unit.Value.Unit.Status.Count > unit.Value.Unit.Configuration.Threshold)
                    {
                        unit.Value.Unit.Status.ReplenishmentStatus = CardStatusClass.ReplenishmentStatusEnum.High;
                    }
                    else if (unit.Value.Unit.Capabilities.Type == CardCapabilitiesClass.TypeEnum.Dispense &&
                             unit.Value.Unit.Status.Count < unit.Value.Unit.Configuration.Threshold)
                    {
                        unit.Value.Unit.Status.ReplenishmentStatus = CardStatusClass.ReplenishmentStatusEnum.Low;
                    }
                    else
                    {
                        unit.Value.Unit.Status.ReplenishmentStatus = CardStatusClass.ReplenishmentStatusEnum.Healthy;
                    }
                }
                else
                {
                    unit.Value.Unit.Status.ReplenishmentStatus = CardStatusClass.ReplenishmentStatusEnum.Healthy;
                }
            }

            // Update hardware storage status
            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCardStorageStatus()");

            bool updateStatus = Device.GetCardStorageStatus(out Dictionary<string, CardUnitStorage.StatusEnum> storageStatus);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCardStorageStatus()-> {updateStatus}");

            if (updateStatus &&
                storageStatus is not null)
            {
                foreach (var unit in storageStatus)
                {
                    if (!CardUnits.ContainsKey(unit.Key))
                    {
                        Logger.Warning(Constants.Framework, $"Specified card unit ID is not found on GetCardStorageStatus. {unit.Key}");
                        continue;
                    }
                    CardUnits[unit.Key].Status = unit.Value;
                }
            }

            // Update hardware card unit status
            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCardUnitStatus()");

            updateStatus = Device.GetCardUnitStatus(out Dictionary<string, CardStatusClass.ReplenishmentStatusEnum> unitStatus);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCardUnitStatus()-> {updateStatus}");

            if (updateStatus &&
                unitStatus is not null)
            {
                foreach (var unit in unitStatus)
                {
                    if (!CardUnits.ContainsKey(unit.Key))
                    {
                        Logger.Warning(Constants.Framework, $"Specified card unit ID is not found on GetCardUnitStatus. {unit.Key}");
                        continue;
                    }
                    CardUnits[unit.Key].Unit.Status.ReplenishmentStatus = unit.Value;
                }
            }

            // Save card units info persistently
            bool success = PersistentData.Store(ServiceProvider.Name + typeof(CardUnitStorage).FullName, CardUnits);
            if (!success)
            {
                Logger.Warning(Constants.Framework, $"Failed to save card unit counts.");
            }
        }

        /// <summary>
        /// Update storage count from the framework after media movement command is processed.
        /// i.e. Reset, Move commands.
        /// </summary>
        public async Task UpdateCardStorageCount(string storageId, int countDelta)
        {
            CardUnits.ContainsKey(storageId).IsTrue($"Unexpected storageId is passed in before updating card unit counters. {storageId}");

            // Update counts first by framework
            if (CardUnits[storageId].Unit.Capabilities.Type == CardCapabilitiesClass.TypeEnum.Retain ||
                CardUnits[storageId].Unit.Capabilities.Type == CardCapabilitiesClass.TypeEnum.Park)
            {
                CardUnits[storageId].Unit.Status.Count += countDelta;
            }
            else
            {
                CardUnits[storageId].Unit.Status.Count -= countDelta;
            }

            if (CardUnits[storageId].Unit.Status.Count < 0)
            {
                CardUnits[storageId].Unit.Status.Count = 0;
            }

            // Update counts from device
            Logger.Log(Constants.DeviceClass, "StorageDev.GetCardUnitCounts()");

            bool updateCounts = Device.GetCardUnitCounts(out Dictionary<string, CardUnitCount> unitCounts);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCardUnitCounts()-> {updateCounts}");

            int beforeCountUpdate = CardUnits[storageId].Unit.Status.Count;

            if (updateCounts &&
                unitCounts is not null)
            {
                if (!unitCounts.ContainsKey(storageId))
                {
                    Logger.Warning(Constants.Framework, $"The device class returned count to update storage, but no information supplied for storage id {storageId}");
                }
                else
                {
                    // overwrite counts
                    CardUnits[storageId].Unit.Status.Count = unitCounts[storageId].Count;
                }  
            }

            // Update hardware storage status
            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCardStorageStatus()");

            bool updateStatus = Device.GetCardStorageStatus(out Dictionary<string, CardUnitStorage.StatusEnum> storageStatus);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCardStorageStatus()-> {updateStatus}");

            if (updateStatus &&
                storageStatus is not null)
            {
                if (!storageStatus.ContainsKey(storageId))
                {
                    Logger.Warning(Constants.Framework, $"The device class returned to update storage status. however, there is no storage status supplied for storage id {storageId}");
                }
                else
                {
                    CardUnits[storageId].Status = storageStatus[storageId];
                }
            }

            // Update hardware card unit status
            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCardUnitStatus()");

            updateStatus = Device.GetCardUnitStatus(out Dictionary<string, CardStatusClass.ReplenishmentStatusEnum> unitStatus);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCardUnitStatus()-> {updateStatus}");

            // Update status from device
            if (updateStatus)
            {
                if (unitStatus is null)
                {
                    Logger.Warning(Constants.Framework, $"The device class returned to update card unit status. however, there is no status supplied.");
                    updateStatus = false;
                }
                else
                {
                    if (!unitStatus.ContainsKey(storageId))
                    {
                        Logger.Warning(Constants.Framework, $"The device class returned to update card unit status. however, there is no status supplied for storage id {storageId}");
                        updateStatus = false;
                    }
                    else
                    {
                        CardUnits[storageId].Unit.Status.ReplenishmentStatus = unitStatus[storageId];
                    }
                }
            }

            if (!updateStatus)
            {
                if (CardUnits[storageId].Unit.Status.Count >= CardUnits[storageId].Capacity)
                {
                    CardUnits[storageId].Unit.Status.ReplenishmentStatus = CardStatusClass.ReplenishmentStatusEnum.Full;
                }
                else if (CardUnits[storageId].Unit.Status.Count == 0)
                {
                    CardUnits[storageId].Unit.Status.ReplenishmentStatus = CardStatusClass.ReplenishmentStatusEnum.Empty;
                }
                else if (CardUnits[storageId].Unit.Configuration.Threshold != 0 &&
                         (CardUnits[storageId].Unit.Capabilities.Type == CardCapabilitiesClass.TypeEnum.Dispense &&
                          beforeCountUpdate < CardUnits[storageId].Unit.Configuration.Threshold &&
                          CardUnits[storageId].Unit.Status.Count != 0) ||
                         (CardUnits[storageId].Unit.Capabilities.Type == CardCapabilitiesClass.TypeEnum.Retain &&
                          beforeCountUpdate > CardUnits[storageId].Unit.Configuration.Threshold &&
                          CardUnits[storageId].Unit.Status.Count < CardUnits[storageId].Capacity))
                {
                    if (CardUnits[storageId].Unit.Capabilities.Type == CardCapabilitiesClass.TypeEnum.Dispense)
                        CardUnits[storageId].Unit.Status.ReplenishmentStatus = CardStatusClass.ReplenishmentStatusEnum.Low;
                    else if (CardUnits[storageId].Unit.Capabilities.Type == CardCapabilitiesClass.TypeEnum.Retain)
                        CardUnits[storageId].Unit.Status.ReplenishmentStatus = CardStatusClass.ReplenishmentStatusEnum.High;

                    // Park type of storage just ingore
                    StorageUnitClass payload = new(
                        Card: new(
                            Status: new(
                                Count: CardUnits[storageId].Unit.Status.Count,
                                ReplenishmentStatus: CardUnits[storageId].Unit.Status.ReplenishmentStatus switch
                                {
                                    CardStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Empty,
                                    CardStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Full,
                                    CardStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.High,
                                    CardStatusClass.ReplenishmentStatusEnum.Low => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Low,
                                    CardStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Ok,
                                    _ => throw new InternalErrorException($"Unexpected card unit status specified. Unit:{storageId} Status:{CardUnits[storageId].Unit.Status.ReplenishmentStatus}"),
                                }
                                )
                            )
                        );

                    // Device class must fire threshold event if the count is managed.
                    StorageThresholdEvent.PayloadData evPayload = new();
                    Dictionary<string, StorageUnitClass> cardUnits = new()
                    { 
                        { storageId, payload } 
                    };
                    evPayload.ExtendedProperties = cardUnits;
                    await StorageThresholdEvent(evPayload);
                }
                else
                {
                    CardUnits[storageId].Unit.Status.ReplenishmentStatus = CardStatusClass.ReplenishmentStatusEnum.Healthy;
                }
            }

            // Save card units info persistently
            bool success = PersistentData.Store(ServiceProvider.Name + typeof(CardUnitStorage).FullName, CardUnits);
            if (!success)
            {
                Logger.Warning(Constants.Framework, $"Failed to save card unit counts.");
            }
        }
        #endregion

        #region Cash
        /// <summary>
        /// ConstructCashUnits
        /// The method retreive cash unit structures from the device class. 
        /// The device class must provide cash unit structure information.
        /// </summary>
        private void ConstructCashUnits()
        {
            Logger.Log(Constants.DeviceClass, "StorageDev.GetCashStorageConfiguration()");

            bool newConfiguration = Device.GetCashStorageConfiguration(out Dictionary<string, CashUnitStorageConfiguration> newCashUnits);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashStorageConfiguration()-> {newConfiguration}");

            bool updateCashUnitFromDeviceClass = newConfiguration;
            if (newConfiguration)
            {
                if (newCashUnits is null ||
                    newCashUnits?.Count == 0)
                {
                    Logger.Warning(Constants.Framework, $"The function GetCashStorageConfiguration returned true. however, there is not output data supplied.");
                    newConfiguration = false;
                }
                else
                {
                    CashUnits.Clear();
                    foreach (var unit in newCashUnits)
                    {
                        CashUnits.Add(unit.Key, new CashUnitStorage(unit.Value));
                    }
                }
            }
            
            if (!newConfiguration)
            {
                bool identical = newCashUnits?.Count == CashUnits.Count;
                if (newCashUnits is not null)
                {
                    foreach (var unit in newCashUnits)
                    {
                        identical = CashUnits.ContainsKey(unit.Key);
                        if (!identical)
                        {
                            Logger.Warning(Constants.Framework, $"Existing cash unit information doesn't contain key specified by the device class. {unit.Key}. Construct new cash unit infomation.");
                            break;
                        }

                        identical = CashUnits[unit.Key].Unit.Configuration == unit.Value.Configuration &&
                                    CashUnits[unit.Key].Unit.Capabilities == unit.Value.Capabilities &&
                                    CashUnits[unit.Key].Capacity == unit.Value.Capacity &&
                                    CashUnits[unit.Key].PositionName == unit.Value.PositionName &&
                                    CashUnits[unit.Key].SerialNumber == unit.Value.SerialNumber &&
                                    CashUnits[unit.Key].Unit.Status.Index == unit.Value.CashUnitAdditionalInfo.Index &&
                                    unit.Value.CashUnitAdditionalInfo.AccuracySupported ? CashUnits[unit.Key].Unit.Status.Accuracy == CashStatusClass.AccuracyEnum.NotSupported : CashUnits[unit.Key].Unit.Status.Accuracy != CashStatusClass.AccuracyEnum.NotSupported;

                        if (!identical)
                        {
                            Logger.Warning(Constants.Framework, $"Existing cash unit information doesn't have an identical cash unit structure information specified by the device class. {unit.Key}. Construct new cash unit infomation.");
                            break;
                        }
                    }
                }

                if (!identical)
                {
                    CashUnits.Clear();
                    if (newCashUnits is not null)
                    {
                        foreach (var unit in newCashUnits)
                        {
                            CashUnits.Add(unit.Key, new CashUnitStorage(unit.Value));
                        }
                    }
                    updateCashUnitFromDeviceClass = true;
                }
            }

            if (updateCashUnitFromDeviceClass)
            {
                // Use hardware status and check the device maintains status or not
                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashUnitCounts()");

                bool updateCounts = Device.GetCashUnitCounts(out Dictionary<string, CashUnitCountClass> unitCounts);

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashUnitCounts()-> {updateCounts}");

                if (updateCounts &&
                    unitCounts is not null)
                {
                    foreach (var unit in unitCounts)
                    {
                        if (!CashUnits.ContainsKey(unit.Key))
                        {
                            Logger.Warning(Constants.Framework, $"Specified storage ID is not found. {unit.Key}");
                            continue;
                        }
                        CashUnits[unit.Key].Unit.Status.Count = unit.Value.Count;
                        if (unit.Value.StorageCashInCount is not null)
                        {
                            CashUnits[unit.Key].Unit.Status.StorageCashInCount = new(unit.Value.StorageCashInCount);
                        }
                        if (unit.Value.StorageCashOutCount is not null)
                        {
                            CashUnits[unit.Key].Unit.Status.StorageCashOutCount = new(unit.Value.StorageCashOutCount);
                        }
                    }
                }

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashUnitInitialCounts()");

                bool updateInitialCounts = Device.GetCashUnitInitialCounts(out Dictionary<string, XFS4IoTFramework.Storage.StorageCashCountClass> initialCounts);

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashUnitInitialCounts()-> {updateInitialCounts}");

                if (updateInitialCounts &&
                    initialCounts is not null)
                {
                    foreach (var unit in initialCounts)
                    {
                        if (!CashUnits.ContainsKey(unit.Key))
                        {
                            Logger.Warning(Constants.Framework, $"Specified storage ID is not found. {unit.Key}");
                            continue;
                        }
                        CashUnits[unit.Key].Unit.Status.InitialCounts = new(unit.Value);
                    }
                }

                // Update status from count
                foreach (var unit in CashUnits)
                {
                    // update status logically first and overwrite status if the device class requires.
                    if (unit.Value.Unit.Status.Count >= unit.Value.Capacity)
                    {
                        unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.Full;
                    }
                    else if (unit.Value.Unit.Status.Count == 0)
                    {
                        unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.Empty;
                    }
                    else if (unit.Value.Unit.Configuration.LowThreshold != 0)
                    {
                        if (unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut) &&
                            unit.Value.Unit.Status.Count < unit.Value.Unit.Configuration.LowThreshold)
                        {
                            unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.Low;
                        }
                        else if ((unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn) ||
                                  unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)) &&
                                 unit.Value.Unit.Status.Count > unit.Value.Unit.Configuration.HighThreshold)
                        {
                            unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.High;
                        }
                        else if (unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract) ||
                                 unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract))
                        {
                            if (unit.Value.Unit.Capabilities.RetractThresholds)
                            {
                                if (unit.Value.Unit.Status.Count > unit.Value.Unit.Configuration.HighThreshold)
                                {
                                    unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.High;
                                }
                            }
                            else
                            {
                                if (unit.Value.Unit.Status.StorageCashInCount is not null &&
                                    unit.Value.Unit.Status.StorageCashInCount.RetractOperations > unit.Value.Unit.Configuration.HighThreshold)
                                {
                                    unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.High;
                                }
                            }
                        }
                    }
                    else
                    {
                        unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.Healthy;
                    }
                }

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashStorageStatus()");

                bool updateStatus = Device.GetCashStorageStatus(out Dictionary<string, CashUnitStorage.StatusEnum> storageStatus);

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashStorageStatus()-> {updateStatus}");

                if (updateCounts &&
                    storageStatus is not null)
                {
                    foreach (var unit in storageStatus)
                    {
                        if (!CashUnits.ContainsKey(unit.Key))
                        {
                            Logger.Warning(Constants.Framework, $"Specified storage ID is not found. {unit.Key}");
                            continue;
                        }
                        CashUnits[unit.Key].Status = unit.Value;
                    }
                }

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashUnitStatus()");

                updateStatus = Device.GetCashUnitStatus(out Dictionary<string, CashStatusClass.ReplenishmentStatusEnum> unitStatus);

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashUnitStatus()-> {updateStatus}");

                if (updateCounts &&
                    unitStatus is not null)
                {
                    foreach (var unit in unitStatus)
                    {
                        if (!CashUnits.ContainsKey(unit.Key))
                        {
                            Logger.Warning(Constants.Framework, $"Specified storage ID is not found. {unit.Key}");
                            continue;
                        }
                        CashUnits[unit.Key].Unit.Status.ReplenishmentStatus = unit.Value;
                    }
                }

                foreach (var unit in from unit in CashUnits
                                     where unit.Value.Unit.Status.Accuracy != CashStatusClass.AccuracyEnum.NotSupported
                                     select unit)
                {
                    Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashUnitAccuray({unit.Key})");

                    Device.GetCashUnitAccuray(unit.Key, out CashStatusClass.AccuracyEnum unitAccuracy);

                    Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashUnitAccuray()-> void");

                    unit.Value.Unit.Status.Accuracy = unitAccuracy;
                }
            }

            if (!PersistentData.Store(ServiceProvider.Name + typeof(CashUnitStorage).FullName, CashUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data.");
            }
        }

        /// <summary>
        /// UpdateCashAccounting
        /// Update cash unit status and counts managed by the device specific class.
        /// </summary>
        public async Task UpdateCashAccounting(Dictionary<string, CashUnitCountClass> countDelta)
        {
            if (countDelta is not null)
            {
                // First to update item movement reported by the device class, then update entire counts if the device class maintains cash unit counts.
                foreach (var delta in countDelta)
                {
                    if (!CashUnits.ContainsKey(delta.Key) ||
                        delta.Value == null)
                    {
                        continue;
                    }

                    // update counts
                    CashUnits[delta.Key].Unit.Status.Count += delta.Value.Count;

                    if (delta.Value.StorageCashOutCount is not null)
                    {
                        UpdateDeltaStorageCashCount(delta.Key, CashUnits[delta.Key].Unit.Status.StorageCashOutCount.Distributed, delta.Value.StorageCashOutCount.Distributed);
                        UpdateDeltaStorageCashCount(delta.Key, CashUnits[delta.Key].Unit.Status.StorageCashOutCount.Diverted, delta.Value.StorageCashOutCount.Diverted);
                        UpdateDeltaStorageCashCount(delta.Key, CashUnits[delta.Key].Unit.Status.StorageCashOutCount.Presented, delta.Value.StorageCashOutCount.Presented);
                        UpdateDeltaStorageCashCount(delta.Key, CashUnits[delta.Key].Unit.Status.StorageCashOutCount.Rejected, delta.Value.StorageCashOutCount.Rejected);
                        UpdateDeltaStorageCashCount(delta.Key, CashUnits[delta.Key].Unit.Status.StorageCashOutCount.Stacked, delta.Value.StorageCashOutCount.Stacked);
                        UpdateDeltaStorageCashCount(delta.Key, CashUnits[delta.Key].Unit.Status.StorageCashOutCount.Transport, delta.Value.StorageCashOutCount.Transport);
                        UpdateDeltaStorageCashCount(delta.Key, CashUnits[delta.Key].Unit.Status.StorageCashOutCount.Unknown, delta.Value.StorageCashOutCount.Unknown);
                    }

                    if (delta.Value.StorageCashInCount is not null)
                    {
                        CashUnits[delta.Key].Unit.Status.StorageCashInCount.RetractOperations += delta.Value.StorageCashInCount.RetractOperations;
                        UpdateDeltaStorageCashCount(delta.Key, CashUnits[delta.Key].Unit.Status.StorageCashInCount.Deposited, delta.Value.StorageCashInCount.Deposited);
                        UpdateDeltaStorageCashCount(delta.Key, CashUnits[delta.Key].Unit.Status.StorageCashInCount.Distributed, delta.Value.StorageCashInCount.Distributed);
                        UpdateDeltaStorageCashCount(delta.Key, CashUnits[delta.Key].Unit.Status.StorageCashInCount.Rejected, delta.Value.StorageCashInCount.Rejected);
                        UpdateDeltaStorageCashCount(delta.Key, CashUnits[delta.Key].Unit.Status.StorageCashInCount.Retracted, delta.Value.StorageCashInCount.Retracted);
                        UpdateDeltaStorageCashCount(delta.Key, CashUnits[delta.Key].Unit.Status.StorageCashInCount.Transport, delta.Value.StorageCashInCount.Transport);
                    }
                }
            }

            Logger.Log(Constants.DeviceClass, "StoragetDev.GetCashUnitCounts()");

            bool updateCounts = Device.GetCashUnitCounts(out Dictionary<string, CashUnitCountClass> unitCounts);

            Logger.Log(Constants.DeviceClass, $"StoragetDev.GetCashUnitCounts()-> {updateCounts}");

            if (updateCounts &&
                unitCounts is not null)
            {
                // overwrite counts updated by the device class
                foreach (var unitCount in unitCounts)
                {
                    if (!CashUnits.ContainsKey(unitCount.Key))
                    {
                        Logger.Warning(Constants.Framework, $"Unknown storage ID supplied on updating counts. {unitCount.Key}");
                        continue;
                    }

                    CashUnits[unitCount.Key].Unit.Status.Count = unitCount.Value.Count;
                    CashUnits[unitCount.Key].Unit.Status.StorageCashOutCount = unitCount.Value.StorageCashOutCount is null ? new() : new(unitCount.Value.StorageCashOutCount);
                    CashUnits[unitCount.Key].Unit.Status.StorageCashInCount = unitCount.Value.StorageCashInCount is null ? new() : new(unitCount.Value.StorageCashInCount);
                }
            }

            Logger.Log(Constants.DeviceClass, "StoragetDev.GetCashStorageStatus()");

            bool updateStorageStatus = Device.GetCashStorageStatus(out Dictionary<string, CashUnitStorage.StatusEnum> storageStatus);

            Logger.Log(Constants.DeviceClass, $"StoragetDev.GetCashStorageStatus()-> {updateStorageStatus}");

            
            if (updateStorageStatus &&
                storageStatus is not null)
            {
                foreach (var unit in storageStatus)
                {
                    if (!CashUnits.ContainsKey(unit.Key))
                    {
                        Logger.Warning(Constants.Framework, $"Unknown storage ID supplied on updating storage status. {unit.Key}");
                        continue;
                    }
                    CashUnits[unit.Key].Status = unit.Value;
                }
            }

            List<string> sendThresholdEvent = [];

            foreach (var unit in CashUnits)
            {
                // update status logically first and overwrite status if the device class requires.
                if (unit.Value.Unit.Status.Count >= unit.Value.Capacity)
                {
                    unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.Full;
                }
                else if (unit.Value.Unit.Status.Count == 0)
                {
                    unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.Empty;
                }
                else if (unit.Value.Unit.Configuration.LowThreshold != 0)
                {
                    if (unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut) &&
                        unit.Value.Unit.Status.Count < unit.Value.Unit.Configuration.LowThreshold)
                    {
                        unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.Low;
                    }
                    else if ((unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn) ||
                              unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)) &&
                             unit.Value.Unit.Status.Count > unit.Value.Unit.Configuration.HighThreshold)
                    {
                        if (!sendThresholdEvent.Contains(unit.Key))
                            sendThresholdEvent.Add(unit.Key);
                        unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.High;
                    }
                    else if (unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract) ||
                             unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract))
                    {
                        if (unit.Value.Unit.Capabilities.RetractThresholds)
                        {
                            if (unit.Value.Unit.Status.Count > unit.Value.Unit.Configuration.HighThreshold)
                            {
                                if (!sendThresholdEvent.Contains(unit.Key))
                                    sendThresholdEvent.Add(unit.Key);
                                unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.High;
                            }
                            else
                            {
                                unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.Healthy;
                            }
                        }
                        else
                        {
                            if (unit.Value.Unit.Status.StorageCashInCount is not null &&
                                unit.Value.Unit.Status.StorageCashInCount.RetractOperations > unit.Value.Unit.Configuration.HighThreshold)
                            {
                                if (!sendThresholdEvent.Contains(unit.Key))
                                    sendThresholdEvent.Add(unit.Key);
                                unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.High;
                            }
                            else
                            {
                                unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.Healthy;
                            }
                        }
                    }
                    else
                    {
                        unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.Healthy;
                    }
                }
                else
                {
                    unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.Healthy;
                }
            }

            Logger.Log(Constants.DeviceClass, "StoragetDev.GetCashUnitStatus()");

            bool updateunitStatus = Device.GetCashUnitStatus(out Dictionary<string, CashStatusClass.ReplenishmentStatusEnum> unitStatus);

            Logger.Log(Constants.DeviceClass, $"StoragetDev.GetCashUnitStatus()-> {updateunitStatus}");

            if (updateunitStatus &&
                unitStatus is not null)
            {
                foreach (var unit in unitStatus)
                {
                    if (!CashUnits.ContainsKey(unit.Key))
                    {
                        Logger.Warning(Constants.Framework, $"Unknown storage ID supplied on updating unit status. {unit.Key}");
                        continue;
                    }
                    CashUnits[unit.Key].Unit.Status.ReplenishmentStatus = unit.Value;
                }
                // status is maintained by the device class and expected threshold event is being sent
                sendThresholdEvent.Clear();
            }

            foreach (var unit in from unit in CashUnits
                                 where unit.Value.Unit.Status.Accuracy != CashStatusClass.AccuracyEnum.NotSupported
                                 select unit)
            {
                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashUnitAccuray({unit.Key})");

                Device.GetCashUnitAccuray(unit.Key, out CashStatusClass.AccuracyEnum unitAccuracy);

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashUnitAccuray()-> void");

                unit.Value.Unit.Status.Accuracy = unitAccuracy;
            }

            // Send threshold event
            Dictionary<string, StorageUnitClass> thresholdUnits = [];
            foreach (var unitId in sendThresholdEvent)
            {
                if (!CashUnits.ContainsKey(unitId))
                {
                    continue;
                }

                StorageUnitClass payload = 
                    new(Cash: new(
                        Status: new(
                            Out: CashUnits[unitId].Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.Low ?
                            null :
                            new(
                                Presented: CashUnits[unitId].Unit.Status.StorageCashOutCount?.Presented.CopyTo(),
                                Rejected: CashUnits[unitId].Unit.Status.StorageCashOutCount?.Rejected.CopyTo(),
                                Distributed: CashUnits[unitId].Unit.Status.StorageCashOutCount?.Distributed.CopyTo(),
                                Unknown: CashUnits[unitId].Unit.Status.StorageCashOutCount?.Unknown.CopyTo(),
                                Stacked: CashUnits[unitId].Unit.Status.StorageCashOutCount?.Stacked.CopyTo(),
                                Diverted: CashUnits[unitId].Unit.Status.StorageCashOutCount?.Diverted.CopyTo(),
                                Transport: CashUnits[unitId].Unit.Status.StorageCashOutCount?.Transport.CopyTo()
                                ),
                            In: CashUnits[unitId].Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.High ?
                            null :
                            new(
                                RetractOperations: CashUnits[unitId].Unit.Status.StorageCashInCount?.RetractOperations,
                                Deposited: CashUnits[unitId].Unit.Status.StorageCashInCount?.Deposited.CopyTo(),
                                Retracted: CashUnits[unitId].Unit.Status.StorageCashInCount?.Retracted.CopyTo(),
                                Rejected: CashUnits[unitId].Unit.Status.StorageCashInCount?.Rejected.CopyTo(),
                                Distributed: CashUnits[unitId].Unit.Status.StorageCashInCount?.Distributed.CopyTo(),
                                Transport: CashUnits[unitId].Unit.Status.StorageCashInCount?.Transport.CopyTo()
                                ),
                            ReplenishmentStatus: CashUnits[unitId].Unit.Status.ReplenishmentStatus switch
                            {
                                CashStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Empty,
                                CashStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Full,
                                CashStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Ok,
                                CashStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.CashManagement.ReplenishmentStatusEnum.High,
                                CashStatusClass.ReplenishmentStatusEnum.Low => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Low,
                                _ => throw new InternalErrorException($"Unexpected cash unit status specified. Unit:{unitId} Status:{CashUnits[unitId].Unit.Status.ReplenishmentStatus}"),
                            }
                            )
                        ));

                thresholdUnits.Add(unitId, payload);
            }

            // Device class must fire threshold event if the threshold count is managed by the device class.
            if (thresholdUnits.Count > 0)
            {
                StorageThresholdEvent.PayloadData evPayload = new()
                {
                    ExtendedProperties = thresholdUnits
                };
                await StorageThresholdEvent(evPayload);
            }

            if (!PersistentData.Store(ServiceProvider.Name + typeof(CashUnitStorage).FullName, CashUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data.");
            }
        }

        /// <summary>
        /// Update delta counts to the destination of the StorageCashCountClass object and return total amount increased or decreased.
        /// </summary>
        /// <param name="storageId">Storage ID to update cash counts</param>
        /// <param name="storageCashCount">Storage count to update</param>
        /// <param name="storageDeltaCount">Delta count to set the storage</param>
        private void UpdateDeltaStorageCashCount(string storageId, XFS4IoTFramework.Storage.StorageCashCountClass storageCashCount, XFS4IoTFramework.Storage.StorageCashCountClass storageDeltaCount)
        {
            if (storageDeltaCount is null)
                return;

            if (storageCashCount is null)
            {
                Logger.Warning(Constants.Framework, $"The target cash-in unit object is not set for Storage ID:{storageId}. The storage count won't be updated as expected.");
                return;
            }

            // update counts
            if (storageDeltaCount.Unrecognized >= 0 ||
                storageDeltaCount.ItemCounts?.Count > 0)
            {
                storageCashCount.Unrecognized += storageDeltaCount.Unrecognized;
                foreach (var item in storageDeltaCount?.ItemCounts)
                {
                    if (!storageCashCount.ItemCounts.ContainsKey(item.Key))
                    {
                        CashUnits.ContainsKey(storageId).IsTrue($"Invalid storage Id passed in {item.Key} " + nameof(UpdateDeltaStorageCashCount));

                        if (CashUnits[storageId].Unit.Configuration.BanknoteItems.Contains(item.Key))
                        {
                            storageCashCount.ItemCounts.Add(item.Key, new());
                        }
                        else
                        {
                            Contracts.Assert(false, $"Unexpected banknote ID provided by the device class. {item.Key} for the storage {storageId}");
                        }
                    }

                    storageCashCount.ItemCounts[item.Key].Counterfeit += item.Value.Counterfeit;
                    storageCashCount.ItemCounts[item.Key].Fit += item.Value.Fit;
                    storageCashCount.ItemCounts[item.Key].Inked += item.Value.Inked;
                    storageCashCount.ItemCounts[item.Key].Suspect += item.Value.Suspect;
                    storageCashCount.ItemCounts[item.Key].Unfit += item.Value.Unfit;
                }
            }

            return;
        }
        #endregion

        #region Check
        /// <summary>
        /// ConstructCheckUnits
        /// The method retreive check unit structures from the device class. 
        /// The device class must provide check unit structure information
        /// </summary>
        private void ConstructCheckUnits()
        {
            Logger.Log(Constants.DeviceClass, "StorageDev.GetCheckStorageConfiguration()");

            bool newConfiguration = Device.GetCheckStorageConfiguration(out Dictionary<string, CheckUnitStorageConfiguration> newCheckUnits);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCheckStorageConfiguration()-> {newConfiguration}");

            bool updateCheckUnitFromDeviceClass = newConfiguration;
            if (newConfiguration)
            {
                if (newCheckUnits is null ||
                    newCheckUnits?.Count == 0)
                {
                    Logger.Warning(Constants.Framework, $"The function GetCheckStorageConfiguration returned true. however, there is not output data supplied.");
                    newConfiguration = false;
                }
                else
                {
                    CheckUnits.Clear();
                    foreach (var unit in newCheckUnits)
                    {
                        CheckUnits.Add(unit.Key, new CheckUnitStorage(unit.Value));
                    }
                }
            }

            if (!newConfiguration)
            {
                bool identical = newCheckUnits?.Count == CheckUnits.Count;
                if (newCheckUnits is not null)
                {
                    foreach (var unit in newCheckUnits)
                    {
                        identical = CheckUnits.ContainsKey(unit.Key);
                        if (!identical)
                        {
                            Logger.Warning(Constants.Framework, $"Existing check unit information doesn't contain key specified by the device class. {unit.Key}. Construct new check unit infomation.");
                            break;
                        }

                        identical = CheckUnits[unit.Key].Unit.Configuration == unit.Value.Configuration &&
                                    CheckUnits[unit.Key].Unit.Capabilities == unit.Value.Capabilities &&
                                    CheckUnits[unit.Key].Capacity == unit.Value.Capacity &&
                                    CheckUnits[unit.Key].PositionName == unit.Value.PositionName &&
                                    CheckUnits[unit.Key].SerialNumber == unit.Value.SerialNumber;

                        if (!identical)
                        {
                            Logger.Warning(Constants.Framework, $"Existing check unit information doesn't have an identical storage structure information specified by the device class. {unit.Key}. Construct new check unit infomation.");
                            break;
                        }
                    }
                }

                if (!identical)
                {
                    CheckUnits.Clear();
                    if (newCheckUnits is not null)
                    {
                        foreach (var unit in newCheckUnits)
                        {
                            CheckUnits.Add(unit.Key, new CheckUnitStorage(unit.Value));
                        }
                    }
                    updateCheckUnitFromDeviceClass = true;
                }
            }

            if (updateCheckUnitFromDeviceClass)
            {
                // Use hardware status and check the device maintains status or not
                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCheckUnitCounts()");

                bool updateCounts = Device.GetCheckUnitCounts(out Dictionary<string, StorageCheckCountClass> unitCounts);

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCheckUnitCounts()-> {updateCounts}");

                if (updateCounts &&
                    unitCounts is not null)
                {
                    foreach (var unit in unitCounts)
                    {
                        if (!CheckUnits.ContainsKey(unit.Key))
                        {
                            Logger.Warning(Constants.Framework, $"Specified storage ID is not found. {unit.Key}");
                            continue;
                        }
                        CheckUnits[unit.Key].Unit.Status.CheckInCounts.Count = unit.Value.Count;
                        CheckUnits[unit.Key].Unit.Status.CheckInCounts.MediaInCount = unit.Value.MediaInCount;
                        CheckUnits[unit.Key].Unit.Status.CheckInCounts.RetractOperations = unit.Value.RetractOperations;
                    }
                }

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCheckUnitInitialCounts()");

                bool updateInitialCounts = Device.GetCheckUnitInitialCounts(out Dictionary<string, StorageCheckCountClass> initialCounts);

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCheckUnitInitialCounts()-> {updateInitialCounts}");

                if (updateInitialCounts &&
                    initialCounts is not null)
                {
                    foreach (var unit in initialCounts)
                    {
                        if (!CheckUnits.ContainsKey(unit.Key))
                        {
                            Logger.Warning(Constants.Framework, $"Specified storage ID is not found. {unit.Key}");
                            continue;
                        }
                        CheckUnits[unit.Key].Unit.Status.InitialCounts.Count = unit.Value.Count;
                        CheckUnits[unit.Key].Unit.Status.InitialCounts.MediaInCount = unit.Value.MediaInCount;
                        CheckUnits[unit.Key].Unit.Status.InitialCounts.RetractOperations = unit.Value.RetractOperations;
                    }
                }

                // Update status from count
                foreach (var unit in CheckUnits)
                {
                    // update status logically first and overwrite status if the device class requires.
                    if (unit.Value.Unit.Status.CheckInCounts.Count >= unit.Value.Capacity)
                    {
                        unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.Full;
                    }
                    else if (unit.Value.Unit.Status.CheckInCounts.Count == 0)
                    {
                        unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.Empty;
                    }
                    else if (unit.Value.Unit.Configuration.HighThreshold != 0)
                    {
                        if (unit.Value.Unit.Status.CheckInCounts.Count < unit.Value.Unit.Configuration.HighThreshold)
                        {
                            unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.Healthy;
                        }
                        else if (unit.Value.Unit.Status.CheckInCounts.Count > unit.Value.Unit.Configuration.HighThreshold)
                        {
                            unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.High;
                        }
                        else
                        {
                            unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.Healthy;
                        }
                    }
                    else
                    {
                        unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.Healthy;
                    }
                }

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCheckStorageStatus()");

                bool updateStatus = Device.GetCheckStorageStatus(out Dictionary<string, CheckUnitStorage.StatusEnum> storageStatus);

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCheckStorageStatus()-> {updateStatus}");

                if (updateCounts &&
                    storageStatus is not null)
                {
                    foreach (var unit in storageStatus)
                    {
                        if (!CheckUnits.ContainsKey(unit.Key))
                        {
                            Logger.Warning(Constants.Framework, $"Specified storage ID is not found. {unit.Key}");
                            continue;
                        }
                        CheckUnits[unit.Key].Status = unit.Value;
                    }
                }

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCheckUnitStatus()");

                updateStatus = Device.GetCheckUnitStatus(out Dictionary<string, CheckStatusClass.ReplenishmentStatusEnum> unitStatus);

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCheckUnitStatus()-> {updateStatus}");

                if (updateCounts &&
                    unitStatus is not null)
                {
                    foreach (var unit in unitStatus)
                    {
                        if (!CheckUnits.ContainsKey(unit.Key))
                        {
                            Logger.Warning(Constants.Framework, $"Specified storage ID is not found. {unit.Key}");
                            continue;
                        }
                        CheckUnits[unit.Key].Unit.Status.ReplenishmentStatus = unit.Value;
                    }
                }
            }

            if (!PersistentData.Store(ServiceProvider.Name + typeof(CheckUnitStorage).FullName, CheckUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data.");
            }
        }

        /// <summary>
        /// UpdateCheckStorageCount
        /// Update check unit status and counts managed by the device specific class.
        /// </summary>
        public async Task UpdateCheckStorageCount(Dictionary<string, StorageCheckCountClass> countDelta)
        {
            if (countDelta is not null)
            {
                // First to update item movement reported by the device class, then update entire counts if the device class maintains check unit counts.
                foreach (var delta in countDelta)
                {
                    if (!CheckUnits.ContainsKey(delta.Key) ||
                        delta.Value == null)
                    {
                        continue;
                    }

                    // update counts
                    CheckUnits[delta.Key].Unit.Status.CheckInCounts.Count += delta.Value.Count;
                    CheckUnits[delta.Key].Unit.Status.CheckInCounts.MediaInCount += delta.Value.MediaInCount;
                    CheckUnits[delta.Key].Unit.Status.CheckInCounts.RetractOperations += delta.Value.RetractOperations;
                }
            }

            Logger.Log(Constants.DeviceClass, "StoragetDev.GetCheckUnitCounts()");

            bool updateCounts = Device.GetCheckUnitCounts(out Dictionary<string, StorageCheckCountClass> unitCounts);

            Logger.Log(Constants.DeviceClass, $"StoragetDev.GetCheckUnitCounts()-> {updateCounts}");

            if (updateCounts &&
                unitCounts is not null)
            {
                // overwrite counts updated by the device class
                foreach (var unitCount in unitCounts)
                {
                    if (!CheckUnits.ContainsKey(unitCount.Key))
                    {
                        Logger.Warning(Constants.Framework, $"Unknown storage ID supplied on updating counts. {unitCount.Key}");
                        continue;
                    }

                    CheckUnits[unitCount.Key].Unit.Status.CheckInCounts.Count = unitCount.Value.Count;
                    CheckUnits[unitCount.Key].Unit.Status.CheckInCounts.MediaInCount = unitCount.Value.MediaInCount;
                    CheckUnits[unitCount.Key].Unit.Status.CheckInCounts.RetractOperations = unitCount.Value.RetractOperations;
                }
            }

            Logger.Log(Constants.DeviceClass, "StoragetDev.GetCheckStorageStatus()");

            bool updateStorageStatus = Device.GetCheckStorageStatus(out Dictionary<string, CheckUnitStorage.StatusEnum> storageStatus);

            Logger.Log(Constants.DeviceClass, $"StoragetDev.GetCheckStorageStatus()-> {updateStorageStatus}");


            if (updateStorageStatus &&
                storageStatus is not null)
            {
                foreach (var unit in storageStatus)
                {
                    if (!CheckUnits.ContainsKey(unit.Key))
                    {
                        Logger.Warning(Constants.Framework, $"Unknown storage ID supplied on updating storage status. {unit.Key}");
                        continue;
                    }
                    CheckUnits[unit.Key].Status = unit.Value;
                }
            }

            Dictionary<string, bool> sendThresholdEvent = [];

            foreach (var unit in CheckUnits)
            {
                // update status logically first and overwrite status if the device class requires.
                if (unit.Value.Unit.Status.CheckInCounts.Count >= unit.Value.Capacity)
                {
                    unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.Full;
                }
                else if (unit.Value.Unit.Status.CheckInCounts.Count == 0)
                {
                    unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.Empty;
                }
                else if (unit.Value.Unit.Configuration.HighThreshold != 0)
                {
                    if (unit.Value.Unit.Status.CheckInCounts.Count < unit.Value.Unit.Configuration.HighThreshold)
                    {
                        unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.Healthy;
                    }
                    else if (unit.Value.Unit.Status.CheckInCounts.Count > unit.Value.Unit.Configuration.HighThreshold)
                    {
                        if (!sendThresholdEvent.ContainsKey(unit.Key))
                        {
                            sendThresholdEvent.Add(unit.Key, false);
                        }
                        unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.High;
                    }
                    else
                    {
                        unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.Healthy;
                    }
                }
                else if (unit.Value.Unit.Configuration.RetractHighThreshold != 0)
                {
                    if (unit.Value.Unit.Status.CheckInCounts.RetractOperations > unit.Value.Unit.Configuration.RetractHighThreshold)
                    {
                        if (!sendThresholdEvent.ContainsKey(unit.Key))
                        {
                            sendThresholdEvent.Add(unit.Key, true);
                        }
                        unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.High;
                    }
                    else
                    {
                        unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.Healthy;
                    }
                }
                else
                {
                    unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.Healthy;
                }
            }

            Logger.Log(Constants.DeviceClass, "StoragetDev.GetCheckUnitStatus()");

            bool updateunitStatus = Device.GetCheckUnitStatus(out Dictionary<string, CheckStatusClass.ReplenishmentStatusEnum> unitStatus);

            Logger.Log(Constants.DeviceClass, $"StoragetDev.GetCheckUnitStatus()-> {updateunitStatus}");

            if (updateunitStatus &&
                unitStatus is not null)
            {
                foreach (var unit in unitStatus)
                {
                    if (!CheckUnits.ContainsKey(unit.Key))
                    {
                        Logger.Warning(Constants.Framework, $"Unknown storage ID supplied on updating unit status. {unit.Key}");
                        continue;
                    }
                    CheckUnits[unit.Key].Unit.Status.ReplenishmentStatus = unit.Value;
                }
                // status is maintained by the device class and expected threshold event is being sent
                sendThresholdEvent.Clear();
            }

            Dictionary<string, StorageUnitClass> thresholdUnits = [];
            foreach (var unitId in sendThresholdEvent)
            {
                if (!CheckUnits.ContainsKey(unitId.Key))
                {
                    continue;
                }

                StorageUnitClass payload =
                    new(Check: 
                        new(
                            Status: new(
                                In: unitId.Value ? new XFS4IoT.Check.StorageStatusClass.InClass(
                                        RetractOperations: CheckUnits[unitId.Key].Unit.Status.CheckInCounts.RetractOperations
                                    ) :
                                    new XFS4IoT.Check.StorageStatusClass.InClass(
                                        Count: CheckUnits[unitId.Key].Unit.Status.CheckInCounts.Count
                                    ),
                                ReplenishmentStatus: unitId.Value ? null :
                                CheckUnits[unitId.Key].Unit.Status.ReplenishmentStatus switch
                                {
                                    CheckStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.Check.ReplenishmentStatusEnum.Empty,
                                    CheckStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.Check.ReplenishmentStatusEnum.Full,
                                    CheckStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.Check.ReplenishmentStatusEnum.Ok,
                                    CheckStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.Check.ReplenishmentStatusEnum.High,
                                    _ => throw new InternalErrorException($"Unexpected status received before sending threshold event. Unit:{unitId.Key} Status:{CheckUnits[unitId.Key].Unit.Status.ReplenishmentStatus}"),
                                })
                            )
                        );

                thresholdUnits.Add(unitId.Key, payload);
            }

            // Device class must fire threshold event if the threshold count is managed by the device class.
            if (thresholdUnits.Count > 0)
            {
                StorageThresholdEvent.PayloadData evPayload = new()
                {
                    ExtendedProperties = thresholdUnits
                };
                await StorageThresholdEvent(evPayload);
            }

            if (!PersistentData.Store(ServiceProvider.Name + typeof(CheckUnitStorage).FullName, CheckUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data.");
            }
        }
        #endregion

        #region Printer
        /// <summary>
        /// ConstructPrinterStorage
        /// The method retreive printer unit structures from the device class. 
        /// The device class must provide printer unit structure information
        /// </summary>
        private void ConstructPrinterStorage()
        {
            Logger.Log(Constants.DeviceClass, "StorageDev.GetPrinterStorageConfiguration()");

            bool newConfiguration = Device.GetPrinterStorageConfiguration(out Dictionary<string, PrinterUnitStorageConfiguration> newPrinterUnits);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetPrinterStorageConfiguration()-> {newConfiguration}");

            if (newConfiguration)
            {
                if (newPrinterUnits is null ||
                    newPrinterUnits?.Count == 0)
                {
                    Logger.Warning(Constants.Framework, $"The function GetPrinterStorageConfiguration returned true. however, there is not output data supplied.");
                    newConfiguration = false;
                }
                else
                {
                    PrinterUnits.Clear();
                    int index = 0;
                    foreach (var unit in newPrinterUnits)
                    {
                        PrinterUnits.Add(unit.Key, new PrinterUnitStorage(index++, unit.Value));
                    }
                }
            }

            if (!newConfiguration)
            {
                bool identical = newPrinterUnits?.Count == PrinterUnits.Count;
                if (newPrinterUnits is not null)
                {
                    foreach (var unit in newPrinterUnits)
                    {
                        identical = PrinterUnits.ContainsKey(unit.Key);
                        if (!identical)
                        {
                            Logger.Warning(Constants.Framework, $"Existing printer unit information doesn't contain key specified by the device class. {unit.Key}. Construct new printer unit infomation.");
                            break;
                        }

                        identical = PrinterUnits[unit.Key].Unit.Configuration == unit.Value.Configuration &&
                                    PrinterUnits[unit.Key].Unit.Capabilities == unit.Value.Capabilities &&
                                    PrinterUnits[unit.Key].Capacity == unit.Value.Capacity &&
                                    PrinterUnits[unit.Key].PositionName == unit.Value.PositionName &&
                                    PrinterUnits[unit.Key].SerialNumber == unit.Value.SerialNumber;

                        if (!identical)
                        {
                            Logger.Warning(Constants.Framework, $"Existing printer unit information doesn't have an identical storage structure information specified by the device class. {unit.Key}. Construct new printer unit infomation.");
                            break;
                        }
                    }
                }

                if (!identical)
                {
                    PrinterUnits.Clear();
                    if (newPrinterUnits is not null)
                    {
                        int index = 0;
                        foreach (var unit in newPrinterUnits)
                        {
                            PrinterUnits.Add(unit.Key, new PrinterUnitStorage(index++, unit.Value));
                        }
                    }
                }
            }

            // Update count from device
            Logger.Log(Constants.DeviceClass, "StorageDev.GetPrinterUnitCounts()");

            bool updateCounts = Device.GetPrinterUnitCounts(out Dictionary<string, PrinterUnitCount> unitCounts);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetPrinterUnitCounts()-> {updateCounts}");

            if (updateCounts &&
                unitCounts is not null)
            {
                foreach (var status in unitCounts)
                {
                    if (!PrinterUnits.ContainsKey(status.Key))
                    {
                        Logger.Warning(Constants.Framework, $"Specified printer unit ID is not found on GetPrinterUnitCounts. {status.Key}");
                        continue;
                    }
                    PrinterUnits[status.Key].Unit.Status.InitialCount = status.Value.InitialCount;
                    PrinterUnits[status.Key].Unit.Status.InCount = status.Value.InCount;
                }
            }

            foreach (var unit in PrinterUnits)
            {
                // update status logically first and overwrite status if the device class requires.
                unit.Value.Unit.Status.ReplenishmentStatus = XFS4IoTFramework.Storage.PrinterStatusClass.ReplenishmentStatusEnum.Healthy;

                if (unit.Value.Unit.Status.InCount >= unit.Value.Capacity)
                {
                    unit.Value.Unit.Status.ReplenishmentStatus = XFS4IoTFramework.Storage.PrinterStatusClass.ReplenishmentStatusEnum.Full;
                }
            }

            // Update hardware storage status
            Logger.Log(Constants.DeviceClass, $"StorageDev.GetPrinterStorageStatus()");

            bool updateStatus = Device.GetPrinterStorageStatus(out Dictionary<string, PrinterUnitStorage.StatusEnum> storageStatus);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetPrinterStorageStatus()-> {updateStatus}");

            if (updateStatus &&
                storageStatus is not null)
            {
                foreach (var unit in storageStatus)
                {
                    if (!PrinterUnits.ContainsKey(unit.Key))
                    {
                        Logger.Warning(Constants.Framework, $"Specified printer unit ID is not found on GetPrinterStorageStatus. {unit.Key}");
                        continue;
                    }
                    PrinterUnits[unit.Key].Status = unit.Value;
                }
            }

            // Update hardware printer unit status
            Logger.Log(Constants.DeviceClass, $"StorageDev.GetPrinterUnitStatus()");

            updateStatus = Device.GetPrinterUnitStatus(out Dictionary<string, XFS4IoTFramework.Storage.PrinterStatusClass.ReplenishmentStatusEnum> unitStatus);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetPrinterUnitStatus()-> {updateStatus}");

            if (updateStatus &&
                unitStatus is not null)
            {
                foreach (var unit in unitStatus)
                {
                    if (!PrinterUnits.ContainsKey(unit.Key))
                    {
                        Logger.Warning(Constants.Framework, $"Specified printer unit ID is not found on GetPrinterUnitStatus. {unit.Key}");
                        continue;
                    }
                    PrinterUnits[unit.Key].Unit.Status.ReplenishmentStatus = unit.Value;
                }
            }

            // Save printer units info persistently
            bool success = PersistentData.Store(ServiceProvider.Name + typeof(PrinterUnitStorage).FullName, PrinterUnits);
            if (!success)
            {
                Logger.Warning(Constants.Framework, $"Failed to save printer unit counts.");
            }
        }

        /// <summary>
        /// UpdatePrinterStorageCount
        /// Update storage count from the framework after media movement command is processed for printer interface.
        /// i.e. ControlMedia, Retract, Reset.
        /// </summary>
        public Task UpdatePrinterStorageCount(string storageId, int countDelta)
        {
            PrinterUnits.ContainsKey(storageId).IsTrue($"Unexpected storageId is passed in before updating printer unit counters. {storageId}");

            // Update counts first by framework
            PrinterUnits[storageId].Unit.Status.InCount += countDelta;

            if (PrinterUnits[storageId].Unit.Status.InCount < 0)
            {
                PrinterUnits[storageId].Unit.Status.InCount = 0;
            }

            // Update counts from device
            Logger.Log(Constants.DeviceClass, "StorageDev.GetPrinterUnitCounts()");

            bool updateCounts = Device.GetPrinterUnitCounts(out Dictionary<string, PrinterUnitCount> unitCounts);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetPrinterUnitCounts()-> {updateCounts}");

            int beforeCountUpdate = PrinterUnits[storageId].Unit.Status.InCount;

            if (updateCounts &&
                unitCounts is not null)
            {
                if (!unitCounts.ContainsKey(storageId))
                {
                    Logger.Warning(Constants.Framework, $"The device class returned count to update storage, but no information supplied for storage id {storageId}");
                }
                else
                {
                    // overwrite counts
                    PrinterUnits[storageId].Unit.Status.InCount = unitCounts[storageId].InCount;
                }
            }

            // Update hardware storage status
            Logger.Log(Constants.DeviceClass, $"StorageDev.GetPrinterStorageStatus()");

            bool updateStatus = Device.GetPrinterStorageStatus(out Dictionary<string, UnitStorageBase.StatusEnum> storageStatus);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetPrinterStorageStatus()-> {updateStatus}");

            if (updateStatus &&
                storageStatus is not null)
            {
                if (!storageStatus.ContainsKey(storageId))
                {
                    Logger.Warning(Constants.Framework, $"The device class returned to update storage status. however, there is no storage status supplied for storage id {storageId}");
                }
                else
                {
                    PrinterUnits[storageId].Status = storageStatus[storageId];
                }
            }

            // Update hardware printer unit status
            Logger.Log(Constants.DeviceClass, $"StorageDev.GetPrinterUnitStatus()");

            updateStatus = Device.GetPrinterUnitStatus(out Dictionary<string, XFS4IoTFramework.Storage.PrinterStatusClass.ReplenishmentStatusEnum> unitStatus);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetPrinterUnitStatus()-> {updateStatus}");

            // Update status from device
            if (updateStatus)
            {
                if (unitStatus is null)
                {
                    Logger.Warning(Constants.Framework, $"The device class returned to update printer unit status. however, there is no status supplied.");
                    updateStatus = false;
                }
                else
                {
                    if (!unitStatus.ContainsKey(storageId))
                    {
                        Logger.Warning(Constants.Framework, $"The device class returned to update printer unit status. however, there is no status supplied for storage id {storageId}");
                        updateStatus = false;
                    }
                    else
                    {
                        PrinterUnits[storageId].Unit.Status.ReplenishmentStatus = unitStatus[storageId];
                    }
                }
            }

            if (!updateStatus)
            {
                if (PrinterUnits[storageId].Unit.Status.InCount >= CardUnits[storageId].Capacity)
                {
                    PrinterUnits[storageId].Unit.Status.ReplenishmentStatus = XFS4IoTFramework.Storage.PrinterStatusClass.ReplenishmentStatusEnum.Full;
                }
                else
                {
                    PrinterUnits[storageId].Unit.Status.ReplenishmentStatus = XFS4IoTFramework.Storage.PrinterStatusClass.ReplenishmentStatusEnum.Healthy;
                }
            }

            // Save card units info persistently
            bool success = PersistentData.Store(ServiceProvider.Name + typeof(PrinterUnitStorage).FullName, PrinterUnits);
            if (!success)
            {
                Logger.Warning(Constants.Framework, $"Failed to save printer unit counts.");
            }

            return Task.CompletedTask;
        }
        #endregion

        #region IBNS
        /// <summary>
        /// ConstructIBNSUnits
        /// The method retreive IBNS unit structures from the device class. 
        /// The device class must provide IBNS unit structure information.
        /// </summary>
        private void ConstructIBNSUnits()
        {
            Logger.Log(Constants.DeviceClass, "StorageDev.GetIBNSStorageConfiguration()");

            bool newConfiguration = Device.GetIBNSStorageInfo(out Dictionary<string, IBNSStorageInfo> newIBNSUnits);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetIBNSStorageConfiguration()-> {newConfiguration}");

            if (newConfiguration)
            {
                if (newIBNSUnits is null ||
                    newIBNSUnits?.Count == 0)
                {
                    Logger.Warning(Constants.Framework, $"The function GetIBNSStorageConfiguration returned true. however, there is not output data supplied.");
                    newConfiguration = false;
                }
                else
                {
                    IBNSUnits.Clear();
                    foreach (var unit in newIBNSUnits)
                    {
                        IBNSUnits.Add(unit.Key, new IBNSUnitStorage(unit.Value.StorageConfiguration, unit.Value.StorageStatus));
                    }
                }
            }

            if (!newConfiguration)
            {
                bool identical = newIBNSUnits?.Count == IBNSUnits.Count;
                if (newIBNSUnits is not null)
                {
                    foreach (var unit in newIBNSUnits)
                    {
                        identical = IBNSUnits.ContainsKey(unit.Key);
                        if (!identical)
                        {
                            Logger.Warning(Constants.Framework, $"Existing IBNS unit information doesn't contain key specified by the device class. {unit.Key}. Construct new IBNS unit infomation.");
                            break;
                        }

                        identical = IBNSUnits[unit.Key].Capacity == unit.Value.StorageConfiguration.Capacity &&
                                    IBNSUnits[unit.Key].PositionName == unit.Value.StorageConfiguration.PositionName &&
                                    IBNSUnits[unit.Key].SerialNumber == unit.Value.StorageConfiguration.SerialNumber;

                        if (!identical)
                        {
                            Logger.Warning(Constants.Framework, $"Existing IBNS unit information doesn't have an identical storage structure information specified by the device class. {unit.Key}. Construct new IBNS unit infomation.");
                            break;
                        }
                    }
                }

                if (!identical)
                {
                    IBNSUnits.Clear();
                    if (newIBNSUnits is not null)
                    {
                        foreach (var unit in newIBNSUnits)
                        {
                            IBNSUnits.Add(unit.Key, new IBNSUnitStorage(unit.Value.StorageConfiguration, unit.Value.StorageStatus));
                        }
                    }
                }
            }
        }
        #endregion

        #region Deposit
        /// <summary>
        /// ConstructDepositUnits
        /// The method retreive Deposit unit structures from the device class. 
        /// The device class must provide Deposit unit structure information.
        /// </summary>
        private void ConstructDepositUnits()
        {
            Logger.Log(Constants.DeviceClass, "StorageDev.GetDepositStorageConfiguration()");

            bool newConfiguration = Device.GetDepositStorageConfiguration(out Dictionary<string, DepositUnitStorageConfiguration> newDepositUnits);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetDepositStorageConfiguration()-> {newConfiguration}");

            if (newConfiguration)
            {
                if (newDepositUnits is null ||
                    newDepositUnits?.Count == 0)
                {
                    Logger.Warning(Constants.Framework, $"The function GetPrinterStorageConfiguration returned true. however, there is not output data supplied.");
                    newConfiguration = false;
                }
                else
                {
                    PrinterUnits.Clear();
                    foreach (var unit in newDepositUnits)
                    {
                        DepositUnits.Add(unit.Key, new DepositUnitStorage(unit.Value));
                    }
                }
            }

            if (!newConfiguration)
            {
                bool identical = newDepositUnits?.Count == DepositUnits.Count;
                if (newDepositUnits is not null)
                {
                    foreach (var unit in newDepositUnits)
                    {
                        identical = DepositUnits.ContainsKey(unit.Key);
                        if (!identical)
                        {
                            Logger.Warning(Constants.Framework, $"Existing deposit unit information doesn't contain key specified by the device class. {unit.Key}. Construct new printer unit infomation.");
                            break;
                        }

                        identical = DepositUnits[unit.Key].Unit.Configuration == unit.Value.Configuration &&
                                    DepositUnits[unit.Key].Unit.Capabilities == unit.Value.Capabilities &&
                                    DepositUnits[unit.Key].Capacity == unit.Value.Capacity &&
                                    DepositUnits[unit.Key].PositionName == unit.Value.PositionName &&
                                    DepositUnits[unit.Key].SerialNumber == unit.Value.SerialNumber;

                        if (!identical)
                        {
                            Logger.Warning(Constants.Framework, $"Existing deposit unit information doesn't have an identical storage structure information specified by the device class. {unit.Key}. Construct new printer unit infomation.");
                            break;
                        }
                    }
                }

                if (!identical)
                {
                    DepositUnits.Clear();
                    if (newDepositUnits is not null)
                    {
                        foreach (var unit in newDepositUnits)
                        {
                            DepositUnits.Add(unit.Key, new DepositUnitStorage(unit.Value));
                        }
                    }
                }
            }

            // Update count from device
            Logger.Log(Constants.DeviceClass, "StorageDev.GetDepositUnitInfo()");

            bool updateCounts = Device.GetDepositUnitInfo(out Dictionary<string, DepositUnitInfo> unitInfo);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetDepositUnitInfo()-> {updateCounts}");

            if (updateCounts &&
                unitInfo is not null)
            {
                foreach (var status in unitInfo)
                {
                    if (!CardUnits.ContainsKey(status.Key))
                    {
                        Logger.Warning(Constants.Framework, $"Specified deposit unit ID is not found on GetPrinterUnitCounts. {status.Key}");
                        continue;
                    }
                    DepositUnits[status.Key].Unit.Status.NumberOfDeposits = status.Value.NumberOfDeposits;
                    DepositUnits[status.Key].Unit.Status.DepositoryContainerStatus = status.Value.DepositoryContainerStatus;
                    DepositUnits[status.Key].Unit.Status.EnvelopSupplyStatus = status.Value.EnvelopSupplyStatus;
                    DepositUnits[status.Key].Status = status.Value.StorageStatus;
                }
            }

            // Save printer units info persistently
            bool success = PersistentData.Store(ServiceProvider.Name + typeof(DepositUnitStorage).FullName, DepositUnits);
            if (!success)
            {
                Logger.Warning(Constants.Framework, $"Failed to save deposit unit counts.");
            }
        }

        /// <summary>
        /// UpdatePrinterStorageCount
        /// Update storage count from the framework after media movement command is processed for deposit interface.
        /// </summary>
        public Task UpdateDepositStorageCount(string storageId, int countDelta)
        {
            // Not supported yet
            return Task.CompletedTask;
        }
        #endregion

        /// <summary>
        /// Store all unit information persistently
        /// </summary>
        public void StorePersistent()
        {
            if (CardUnits is not null &&
                !PersistentData.Store(ServiceProvider.Name + typeof(CardUnitStorage).FullName, CardUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data for card units.");
            }
            if (CashUnits is not null &&
                !PersistentData.Store(ServiceProvider.Name + typeof(CashUnitStorage).FullName, CashUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data for cash units.");
            }
            if (CheckUnits is not null &&
                !PersistentData.Store(ServiceProvider.Name + typeof(CheckUnitStorage).FullName, CheckUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data for check units.");
            }
            if (PrinterUnits is not null &&
                !PersistentData.Store(ServiceProvider.Name + typeof(PrinterUnitStorage).FullName, PrinterUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data for printer units.");
            }
            if (IBNSUnits is not null &&
                !PersistentData.Store(ServiceProvider.Name + typeof(IBNSUnitStorage).FullName, IBNSUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data for IBNS units.");
            }
            if (DepositUnits is not null &&
                !PersistentData.Store(ServiceProvider.Name + typeof(IBNSUnitStorage).FullName, IBNSUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data for IBNS units.");
            }
        }

        /// <summary>
        /// Persistent data storage access
        /// </summary>
        private readonly IPersistentData PersistentData;

        /// <summary>
        /// Type of storage
        /// </summary>
        public StorageTypeEnum StorageType { get; init; }

        /// <summary>
        /// Card storage structure information of this device
        /// </summary>
        public Dictionary<string, CardUnitStorage> CardUnits { get; init; }

        /// <summary>
        /// Cash storage structure information of this device
        /// </summary>
        public Dictionary<string, CashUnitStorage> CashUnits { get; init; }

        /// <summary>
        /// Check storage structure information of this device
        /// </summary>
        public Dictionary<string, CheckUnitStorage> CheckUnits { get; init; }

        /// <summary>
        /// Printer storage structure information of this device
        /// </summary>
        public Dictionary<string, PrinterUnitStorage> PrinterUnits { get; init; }

        /// <summary>
        /// IBNS storage structure information of this device
        /// </summary>
        public Dictionary<string, IBNSUnitStorage> IBNSUnits { get; init; }

        /// <summary>
        /// Depository storage structure information of this device
        /// </summary>
        public Dictionary<string, DepositUnitStorage> DepositUnits { get; init; }

        /// <summary>
        /// Return XFS4IoT storage structured object.
        /// </summary>
        public Dictionary<string, XFS4IoT.Storage.StorageUnitClass> GetStorages(List<string> UnitIds)
        {
            Dictionary<string, UnitStorageBase> allStorage = [];
            Dictionary<string, StorageCashClass> cashStorage = [];
            Dictionary<string, XFS4IoT.CardReader.StorageClass> cardStorage = [];
            Dictionary<string, XFS4IoT.Check.StorageClass> checkStorage = [];
            Dictionary<string, XFS4IoT.Deposit.StorageClass> depositStorage = [];
            Dictionary<string, XFS4IoT.Printer.StorageClass> printerStorage = [];
            Dictionary<string, XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass> ibnsStorage = [];

            if (StorageType.HasFlag(StorageTypeEnum.Card))
            {
                foreach (var storage in CardUnits)
                {
                    if (!allStorage.ContainsKey(storage.Key))
                    {
                        allStorage.Add(storage.Key, storage.Value);
                    }

                    cardStorage.Add(
                        storage.Key,
                        new(
                            Capabilities: new(
                                Type: storage.Value.Unit.Capabilities.Type switch
                                {
                                    CardCapabilitiesClass.TypeEnum.Dispense => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Dispense,
                                    CardCapabilitiesClass.TypeEnum.Retain => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Retain,
                                    _ => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Park,
                                },
                                HardwareSensors: storage.Value.Unit.Capabilities.HardwareSensors),
                             Configuration: new(
                                 CardID: storage.Value.Unit.Configuration.CardId,
                                 Threshold: storage.Value.Unit.Configuration.Threshold),
                             Status: new(
                                 InitialCount: storage.Value.Unit.Status.InitialCount,
                                 Count: storage.Value.Unit.Status.Count,
                                 RetainCount: storage.Value.Unit.Status.RetainCount,
                                 ReplenishmentStatus: storage.Value.Unit.Status.ReplenishmentStatus switch
                                 {
                                     CardStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Empty,
                                     CardStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Full,
                                     CardStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.High,
                                     CardStatusClass.ReplenishmentStatusEnum.Low => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Low,
                                     CardStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Ok,
                                     _ => throw new InternalErrorException($"Unexpected card bin status specified. Unit:{storage.Key} Status:{storage.Value.Unit.Status.ReplenishmentStatus}"),
                                 })
                             )
                        );
                }
            }
            if (StorageType.HasFlag(StorageTypeEnum.Cash))
            {
                foreach (var storage in CashUnits)
                {
                    if (!allStorage.ContainsKey(storage.Key))
                    {
                        allStorage.Add(storage.Key, storage.Value);
                    }

                    cashStorage.Add(
                        storage.Key,
                        new(
                            Capabilities: new(
                                Types: new(
                                    CashIn: storage.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                    CashOut: storage.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                    Replenishment: storage.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                    CashInRetract: storage.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                    CashOutRetract: storage.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                    Reject: storage.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)
                                    ),
                                Items: new(
                                    Fit: storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                    Unfit: storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                    Unrecognized: storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                    Counterfeit: storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Counterfeit),
                                    Suspect: storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                    Inked: storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                    Coupon: storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                    Document: storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)
                                    ),
                                HardwareSensors: storage.Value.Unit.Capabilities.HardwareSensors,
                                RetractAreas: storage.Value.Unit.Capabilities.RetractAreas,
                                RetractThresholds: storage.Value.Unit.Capabilities.RetractThresholds,
                                CashItems: storage.Value.Unit.Configuration.BanknoteItems
                                ),
                            Configuration: new(
                                Types: new(
                                    CashIn: storage.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                    CashOut: storage.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                    Replenishment: storage.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                    CashInRetract: storage.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                    CashOutRetract: storage.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                    Reject: storage.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)
                                    ),
                                Items: new(
                                    Fit: storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                    Unfit: storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                    Unrecognized: storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                    Counterfeit: storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Counterfeit),
                                    Suspect: storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                    Inked: storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                    Coupon: storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                    Document: storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)
                                    ),
                                Currency: storage.Value.Unit.Configuration.Currency,
                                Value: storage.Value.Unit.Configuration.Value,
                                HighThreshold: storage.Value.Unit.Configuration.HighThreshold,
                                LowThreshold: storage.Value.Unit.Configuration.LowThreshold,
                                AppLockIn: storage.Value.Unit.Configuration.AppLockIn,
                                AppLockOut: storage.Value.Unit.Configuration.AppLockOut,
                                CashItems: storage.Value.Unit.Configuration.BanknoteItems
                                ),
                                Status: new(
                                    Index: storage.Value.Unit.Status.Index,
                                    Initial: storage.Value.Unit.Status.InitialCounts?.CopyTo(),
                                    Out: new(
                                        Presented: storage.Value.Unit.Status.StorageCashOutCount?.Presented?.CopyTo(),
                                        Rejected: storage.Value.Unit.Status.StorageCashOutCount?.Rejected?.CopyTo(),
                                        Distributed: storage.Value.Unit.Status.StorageCashOutCount?.Distributed?.CopyTo(),
                                        Unknown: storage.Value.Unit.Status.StorageCashOutCount?.Unknown?.CopyTo(),
                                        Stacked: storage.Value.Unit.Status.StorageCashOutCount?.Stacked?.CopyTo(),
                                        Diverted: storage.Value.Unit.Status.StorageCashOutCount?.Diverted?.CopyTo(),
                                        Transport: storage.Value.Unit.Status.StorageCashOutCount?.Transport?.CopyTo()
                                        ),
                                    In: new(
                                        RetractOperations: storage.Value.Unit.Status.StorageCashInCount?.RetractOperations,
                                        Deposited: storage.Value.Unit.Status.StorageCashInCount?.Deposited?.CopyTo(),
                                        Retracted: storage.Value.Unit.Status.StorageCashInCount?.Retracted?.CopyTo(),
                                        Rejected: storage.Value.Unit.Status.StorageCashInCount?.Rejected?.CopyTo(),
                                        Distributed: storage.Value.Unit.Status.StorageCashInCount?.Distributed?.CopyTo(),
                                        Transport: storage.Value.Unit.Status.StorageCashInCount?.Transport?.CopyTo()
                                        ),
                                    Accuracy: storage.Value.Unit.Status.Accuracy switch
                                    {
                                        CashStatusClass.AccuracyEnum.Accurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Accurate,
                                        CashStatusClass.AccuracyEnum.AccurateSet => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.AccurateSet,
                                        CashStatusClass.AccuracyEnum.Inaccurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Inaccurate,
                                        CashStatusClass.AccuracyEnum.Unknown => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Unknown,
                                        _ => null,
                                    },
                                    ReplenishmentStatus: storage.Value.Unit.Status.ReplenishmentStatus switch
                                    {
                                        CashStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Empty,
                                        CashStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Full,
                                        CashStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Ok,
                                        CashStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.CashManagement.ReplenishmentStatusEnum.High,
                                        CashStatusClass.ReplenishmentStatusEnum.Low => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Low,
                                        _ => throw new InternalErrorException($"Unexpected cash unit status specified. Unit:{storage.Key} Status:{storage.Value.Unit.Status.ReplenishmentStatus}"),
                                    }
                                    )
                                )
                            );
                }
            }
            if (StorageType.HasFlag(StorageTypeEnum.Check))
            {
                foreach (var storage in CheckUnits)
                {
                    if (!allStorage.ContainsKey(storage.Key))
                    {
                        allStorage.Add(storage.Key, storage.Value);
                    }

                    checkStorage.Add(
                        storage.Key,
                        new(
                            Capabilities: new(
                                Types: new(
                                    MediaIn: storage.Value.Unit.Capabilities.Types.HasFlag(CheckCapabilitiesClass.TypesEnum.MediaIn),
                                    Retract: storage.Value.Unit.Capabilities.Types.HasFlag(CheckCapabilitiesClass.TypesEnum.Retract)
                                    ),
                                Sensors: new(
                                    Empty: storage.Value.Unit.Capabilities.Sensors.HasFlag(CheckCapabilitiesClass.SensorEnum.Empty),
                                    High: storage.Value.Unit.Capabilities.Sensors.HasFlag(CheckCapabilitiesClass.SensorEnum.High),
                                    Full: storage.Value.Unit.Capabilities.Sensors.HasFlag(CheckCapabilitiesClass.SensorEnum.Full)
                                    )
                                ),
                            Configuration: new(
                                Types: new(
                                    MediaIn: storage.Value.Unit.Configuration.Types.HasFlag(CheckCapabilitiesClass.TypesEnum.MediaIn),
                                    Retract: storage.Value.Unit.Configuration.Types.HasFlag(CheckCapabilitiesClass.TypesEnum.Retract)
                                    ),
                                BinID: storage.Value.Unit.Configuration.Id,
                                HighThreshold: storage.Value.Unit.Configuration.HighThreshold <= 0 ?
                                null :
                                storage.Value.Unit.Configuration.HighThreshold,
                                RetractHighThreshold: storage.Value.Unit.Configuration.RetractHighThreshold <= 0 ?
                                null :
                                storage.Value.Unit.Configuration.RetractHighThreshold),
                            Status: new(
                                Index: storage.Value.Unit.Status.Index,
                                Initial: new(
                                    MediaInCount: storage.Value.Unit.Status.InitialCounts.MediaInCount,
                                    Count: storage.Value.Unit.Status.InitialCounts.Count,
                                    RetractOperations: storage.Value.Unit.Status.InitialCounts.RetractOperations
                                    ),
                                In: new(
                                    MediaInCount: storage.Value.Unit.Status.CheckInCounts.MediaInCount,
                                    Count: storage.Value.Unit.Status.CheckInCounts.Count,
                                    RetractOperations: storage.Value.Unit.Status.CheckInCounts.RetractOperations
                                    ),
                                ReplenishmentStatus: storage.Value.Unit.Status.ReplenishmentStatus switch
                                {
                                    CheckStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.Check.ReplenishmentStatusEnum.Empty,
                                    CheckStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.Check.ReplenishmentStatusEnum.Full,
                                    CheckStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.Check.ReplenishmentStatusEnum.Ok,
                                    CheckStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.Check.ReplenishmentStatusEnum.High,
                                    _ => throw new InternalErrorException($"Unexpected check status specified. Unit:{storage.Key} Stauts:{storage.Value.Unit.Status.ReplenishmentStatus}"),
                                }
                                )
                            )
                        );
                }
            }
            if (StorageType.HasFlag(StorageTypeEnum.Printer))
            {
                foreach (var storage in PrinterUnits)
                {
                    if (!allStorage.ContainsKey(storage.Key))
                    {
                        allStorage.Add(storage.Key, storage.Value);
                    }

                    printerStorage.Add(
                        storage.Key,
                        new(
                            Capabilities: new(
                                MaxRetracts: storage.Value.Unit.Capabilities.MaxRetracts),
                            Status: new(
                                Index: storage.Value.Unit.Status.Index,
                                Initial: storage.Value.Unit.Status.InitialCount,
                                In: storage.Value.Unit.Status.InCount,
                                ReplenishmentStatus: storage.Value.Unit.Status.ReplenishmentStatus switch
                                {
                                    XFS4IoTFramework.Storage.PrinterStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.Printer.ReplenishmentStatusEnum.Full,
                                    XFS4IoTFramework.Storage.PrinterStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.Printer.ReplenishmentStatusEnum.Ok,
                                    XFS4IoTFramework.Storage.PrinterStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.Printer.ReplenishmentStatusEnum.High,
                                    XFS4IoTFramework.Storage.PrinterStatusClass.ReplenishmentStatusEnum.Unknown => XFS4IoT.Printer.ReplenishmentStatusEnum.Unknown,
                                    _ => throw new InternalErrorException($"Unexpected printer status specified. Unit:{storage.Key} Status:{storage.Value.Unit.Status.ReplenishmentStatus}"),
                                })
                            )
                        );
                }
            }
            if (StorageType.HasFlag(StorageTypeEnum.IBNS))
            {
                foreach (var storage in IBNSUnits)
                {
                    if (!allStorage.ContainsKey(storage.Key))
                    {
                        allStorage.Add(storage.Key, storage.Value);
                    }

                    ibnsStorage.Add(
                        storage.Key,
                        new(
                            Identifier: storage.Value.Unit.Status.Identifier,
                            Protection: storage.Value.Unit.Status.Protection switch
                            {
                                XFS4IoTFramework.Storage.IBNSStatusClass.ProtectionEnum.NeutralizationTriggered => XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass.ProtectionEnum.NeutralizationTriggered,
                                XFS4IoTFramework.Storage.IBNSStatusClass.ProtectionEnum.Fault => XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass.ProtectionEnum.Fault,
                                XFS4IoTFramework.Storage.IBNSStatusClass.ProtectionEnum.Disarmed => XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass.ProtectionEnum.Disarmed,
                                XFS4IoTFramework.Storage.IBNSStatusClass.ProtectionEnum.Armed => XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass.ProtectionEnum.Armed,
                                _ => throw new InternalErrorException($"Unexpected IBNS protection status specified. Unit:{storage.Key} Status:{storage.Value.Unit.Status.Protection}"),
                            },
                            Warning: storage.Value.Unit.Status.Warning switch
                            {
                                XFS4IoTFramework.Storage.IBNSStatusClass.WarningEnum.CassetteRunsAutonomously => XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass.WarningEnum.CassetteRunsAutonomously,
                                XFS4IoTFramework.Storage.IBNSStatusClass.WarningEnum.Alarm => XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass.WarningEnum.Alarm,
                                _ => null,
                            },
                            PowerSupply: storage.Value.Unit.Status.PowerInfo is null ? null :
                            new XFS4IoT.PowerManagement.PowerInfoClass(
                                PowerInStatus: storage.Value.Unit.Status.PowerInfo.PowerInStatus switch
                                {
                                    PowerInfoClass.PoweringStatusEnum.Powering => XFS4IoT.PowerManagement.PowerInfoClass.PowerInStatusEnum.Powering,
                                    PowerInfoClass.PoweringStatusEnum.NotPower => XFS4IoT.PowerManagement.PowerInfoClass.PowerInStatusEnum.NoPower,
                                    _ => throw new InternalErrorException($"Unexpected IBNS power in status specified. Unit:{storage.Key} Status:{storage.Value.Unit.Status.PowerInfo.PowerInStatus}"),
                                },
                                PowerOutStatus: storage.Value.Unit.Status.PowerInfo.PowerOutStatus switch
                                {
                                    PowerInfoClass.PoweringStatusEnum.Powering => XFS4IoT.PowerManagement.PowerInfoClass.PowerOutStatusEnum.Powering,
                                    PowerInfoClass.PoweringStatusEnum.NotPower => XFS4IoT.PowerManagement.PowerInfoClass.PowerOutStatusEnum.NoPower,
                                    _ => throw new InternalErrorException($"Unexpected IBNS power out status specified. Unit:{storage.Key} Status:{storage.Value.Unit.Status.PowerInfo.PowerOutStatus}"),
                                },
                                BatteryStatus: storage.Value.Unit.Status.PowerInfo.BatteryStatus switch
                                {
                                    PowerInfoClass.BatteryStatusEnum.Full => XFS4IoT.PowerManagement.BatteryStatusEnum.Full,
                                    PowerInfoClass.BatteryStatusEnum.Low => XFS4IoT.PowerManagement.BatteryStatusEnum.Low,
                                    PowerInfoClass.BatteryStatusEnum.Failure => XFS4IoT.PowerManagement.BatteryStatusEnum.Failure,
                                    PowerInfoClass.BatteryStatusEnum.Operational => XFS4IoT.PowerManagement.BatteryStatusEnum.Operational,
                                    PowerInfoClass.BatteryStatusEnum.Critical => XFS4IoT.PowerManagement.BatteryStatusEnum.Critical,
                                    _ => null,
                                },
                                BatteryChargingStatus: storage.Value.Unit.Status.PowerInfo.BatteryChargingStatus switch
                                {
                                    PowerInfoClass.BatteryChargingStatusEnum.Charging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.Charging,
                                    PowerInfoClass.BatteryChargingStatusEnum.NotCharging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.NotCharging,
                                    PowerInfoClass.BatteryChargingStatusEnum.Discharging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.Discharging,
                                    _ => null,
                                }),
                            Tilt: storage.Value.Unit.Status.TiltState switch
                            {
                                XFS4IoTFramework.Common.IBNSStatusClass.TiltStateEnum.Fault => XFS4IoT.BanknoteNeutralization.TiltStateEnum.Fault,
                                XFS4IoTFramework.Common.IBNSStatusClass.TiltStateEnum.Tilted => XFS4IoT.BanknoteNeutralization.TiltStateEnum.Tilted,
                                XFS4IoTFramework.Common.IBNSStatusClass.TiltStateEnum.NotTilted => XFS4IoT.BanknoteNeutralization.TiltStateEnum.NotTilted,
                                XFS4IoTFramework.Common.IBNSStatusClass.TiltStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.TiltStateEnum.Disabled,
                                _ => null,
                            },
                            Temperature: storage.Value.Unit.Status.TemperatureState switch
                            {
                                XFS4IoTFramework.Common.IBNSStatusClass.TemperatureStateEnum.Fault => XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.Fault,
                                XFS4IoTFramework.Common.IBNSStatusClass.TemperatureStateEnum.TooCold => XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.TooCold,
                                XFS4IoTFramework.Common.IBNSStatusClass.TemperatureStateEnum.Healthy => XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.Ok,
                                XFS4IoTFramework.Common.IBNSStatusClass.TemperatureStateEnum.TooHot => XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.TooHot,
                                XFS4IoTFramework.Common.IBNSStatusClass.TemperatureStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.Disabled,
                                _ => null,
                            },
                            Lid: storage.Value.Unit.Status.LidStatus switch
                            {
                                XFS4IoTFramework.Storage.IBNSStatusClass.LidStatusEnum.Fault => XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass.LidEnum.Fault,
                                XFS4IoTFramework.Storage.IBNSStatusClass.LidStatusEnum.Opened => XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass.LidEnum.Opened,
                                XFS4IoTFramework.Storage.IBNSStatusClass.LidStatusEnum.Closed => XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass.LidEnum.Closed,
                                XFS4IoTFramework.Storage.IBNSStatusClass.LidStatusEnum.Disabled => XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass.LidEnum.Disabled,
                                _ => null,
                            },
                            NeutralizationTrigger: storage.Value.Unit.Status.NeutralizationTrigger switch
                            {
                                XFS4IoTFramework.Storage.IBNSStatusClass.NeutralizationTriggerEnum.Initializing => XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass.NeutralizationTriggerEnum.Initializing,
                                XFS4IoTFramework.Storage.IBNSStatusClass.NeutralizationTriggerEnum.Ready => XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass.NeutralizationTriggerEnum.Ready,
                                XFS4IoTFramework.Storage.IBNSStatusClass.NeutralizationTriggerEnum.Disabled => XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass.NeutralizationTriggerEnum.Disabled,
                                XFS4IoTFramework.Storage.IBNSStatusClass.NeutralizationTriggerEnum.Fault => XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass.NeutralizationTriggerEnum.Fault,
                                _ => null,
                            },
                            StorageUnitIdentifier: storage.Value.Unit.Status.StorageUnitIdentifier
                            )
                        );
                }
            }
            if (StorageType.HasFlag(StorageTypeEnum.Deposit))
            {
                foreach (var storage in DepositUnits)
                {
                    if (!allStorage.ContainsKey(storage.Key))
                    {
                        allStorage.Add(storage.Key, storage.Value);
                    }

                    depositStorage.Add(
                        storage.Key,
                        new(
                            Capabilities: new(
                                EnvSupply: storage.Value.Unit.Capabilities.EnvelpeSupply switch
                                {
                                    XFS4IoTFramework.Storage.DepositCapabilitiesClass.EnvelopeSupplyEnum.Motorized => XFS4IoT.Deposit.StorageCapabilitiesClass.EnvSupplyEnum.Motorized,
                                    XFS4IoTFramework.Storage.DepositCapabilitiesClass.EnvelopeSupplyEnum.Manual => XFS4IoT.Deposit.StorageCapabilitiesClass.EnvSupplyEnum.Manual,
                                _ => null
                                }),
                            Status: new(
                                DepContainer: storage.Value.Unit.Status.DepositoryContainerStatus switch
                                {
                                    XFS4IoTFramework.Storage.DepositStatusClass.DepositoryContainerStatusEnum.Healthy => XFS4IoT.Deposit.StorageStatusClass.DepContainerEnum.Ok,
                                    XFS4IoTFramework.Storage.DepositStatusClass.DepositoryContainerStatusEnum.Missing => XFS4IoT.Deposit.StorageStatusClass.DepContainerEnum.Missing,
                                    XFS4IoTFramework.Storage.DepositStatusClass.DepositoryContainerStatusEnum.Inoperative => XFS4IoT.Deposit.StorageStatusClass.DepContainerEnum.Inoperative,
                                    XFS4IoTFramework.Storage.DepositStatusClass.DepositoryContainerStatusEnum.Unknown => XFS4IoT.Deposit.StorageStatusClass.DepContainerEnum.Unknown,
                                    XFS4IoTFramework.Storage.DepositStatusClass.DepositoryContainerStatusEnum.High => XFS4IoT.Deposit.StorageStatusClass.DepContainerEnum.High,
                                    XFS4IoTFramework.Storage.DepositStatusClass.DepositoryContainerStatusEnum.Full => XFS4IoT.Deposit.StorageStatusClass.DepContainerEnum.Full,
                                    _ => null
                                },
                                EnvSupply: storage.Value.Unit.Status.EnvelopSupplyStatus switch
                                {
                                    XFS4IoTFramework.Storage.DepositStatusClass.EnvelopSupplyStatusEnum.Healthy => XFS4IoT.Deposit.StorageStatusClass.EnvSupplyEnum.Ok,
                                    XFS4IoTFramework.Storage.DepositStatusClass.EnvelopSupplyStatusEnum.Missing => XFS4IoT.Deposit.StorageStatusClass.EnvSupplyEnum.Missing,
                                    XFS4IoTFramework.Storage.DepositStatusClass.EnvelopSupplyStatusEnum.Inoperative => XFS4IoT.Deposit.StorageStatusClass.EnvSupplyEnum.Inoperative,
                                    XFS4IoTFramework.Storage.DepositStatusClass.EnvelopSupplyStatusEnum.Unknown => XFS4IoT.Deposit.StorageStatusClass.EnvSupplyEnum.Unknown,
                                    XFS4IoTFramework.Storage.DepositStatusClass.EnvelopSupplyStatusEnum.Low => XFS4IoT.Deposit.StorageStatusClass.EnvSupplyEnum.Low,
                                    XFS4IoTFramework.Storage.DepositStatusClass.EnvelopSupplyStatusEnum.Empty => XFS4IoT.Deposit.StorageStatusClass.EnvSupplyEnum.Empty,
                                    _ => null,
                                },
                                NumOfDeposits: storage.Value.Unit.Status.NumberOfDeposits
                                )
                            )
                        );
                }
            }

            // Build output response
            Dictionary<string, StorageUnitClass> storageResponse = [];
            foreach (var storage in allStorage)
            {
                if (!UnitIds.Contains(storage.Key))
                {
                    continue;
                }

                StorageCashClass thisCashStorage = null;
                XFS4IoT.CardReader.StorageClass thisCardStorage = null;
                XFS4IoT.Check.StorageClass thisCheckStorage = null;
                XFS4IoT.Deposit.StorageClass thisDepositStorage = null;
                XFS4IoT.Printer.StorageClass thisPrinterStorage = null;
                XFS4IoT.BanknoteNeutralization.StorageUnitStatusClass thisIBNSStorage = null;

                if (cashStorage.ContainsKey(storage.Key))
                {
                    thisCashStorage = cashStorage[storage.Key];
                }
                if (cardStorage.ContainsKey(storage.Key))
                {
                    thisCardStorage = cardStorage[storage.Key];
                }
                if (checkStorage.ContainsKey(storage.Key))
                {
                    thisCheckStorage = checkStorage[storage.Key];
                }
                if (depositStorage.ContainsKey(storage.Key))
                {
                    thisDepositStorage = depositStorage[storage.Key];
                }
                if (printerStorage.ContainsKey(storage.Key))
                {
                    thisPrinterStorage = printerStorage[storage.Key];
                }
                if (ibnsStorage.ContainsKey(storage.Key))
                {
                    thisIBNSStorage = ibnsStorage[storage.Key];
                }

                StorageUnitClass thisStorage = new(
                    PositionName: storage.Value.PositionName,
                    Capacity: storage.Value.Capacity,
                    Status: storage.Value.Status switch
                    {
                        CashUnitStorage.StatusEnum.Good => StatusEnum.Ok,
                        CashUnitStorage.StatusEnum.Inoperative => StatusEnum.Inoperative,
                        CashUnitStorage.StatusEnum.Manipulated => StatusEnum.Manipulated,
                        CashUnitStorage.StatusEnum.Missing => StatusEnum.Missing,
                        _ => StatusEnum.NotConfigured,
                    },
                    SerialNumber: storage.Value.SerialNumber,
                    Card: thisCardStorage,
                    Cash: thisCashStorage,
                    Check: thisCheckStorage,
                    Deposit: thisDepositStorage,
                    BanknoteNeutralization: thisIBNSStorage,
                    Printer: thisPrinterStorage
                    );

                storageResponse.Add(storage.Key, thisStorage);
            }

            return storageResponse;
        }

        /// <summary>
        /// Register status changed event to all units supported
        /// </summary>
        private void RegisterStorageChangedEvents()
        {
            foreach (var unit in CardUnits)
            {
                unit.Value.StorageId = unit.Key;
                unit.Value.Unit.Status.StorageId = unit.Key;
                unit.Value.PropertyChanged += StorageChangedEventFowarder;
                unit.Value.Unit.Status.PropertyChanged += StorageChangedEventFowarder;
            }
            foreach (var unit in CashUnits)
            {
                unit.Value.StorageId = unit.Key;
                unit.Value.PropertyChanged += StorageChangedEventFowarder;
                unit.Value.Unit.Status.StorageId = unit.Key;
                unit.Value.Unit.Status.PropertyChanged += StorageChangedEventFowarder;
            }
            foreach (var unit in CheckUnits)
            {
                unit.Value.StorageId = unit.Key;
                unit.Value.PropertyChanged += StorageChangedEventFowarder;
                unit.Value.Unit.Status.StorageId = unit.Key;
                unit.Value.Unit.Status.PropertyChanged += StorageChangedEventFowarder;
                unit.Value.Unit.Status.CheckInCounts.StorageId = unit.Key;
                unit.Value.Unit.Status.CheckInCounts.PropertyChanged += StorageChangedEventFowarder;
                unit.Value.Unit.Status.InitialCounts.StorageId = unit.Key;
                unit.Value.Unit.Status.InitialCounts.PropertyChanged += StorageChangedEventFowarder;
            }
            foreach (var unit in PrinterUnits)
            {
                unit.Value.Unit.Status.StorageId = unit.Key;
                unit.Value.PropertyChanged += StorageChangedEventFowarder;
                unit.Value.Unit.Status.PropertyChanged += StorageChangedEventFowarder;
            }
            foreach (var unit in IBNSUnits)
            {
                unit.Value.StorageId = unit.Key;
                unit.Value.PropertyChanged += StorageChangedEventFowarder;
                unit.Value.Unit.Status.StorageId = unit.Key;
                unit.Value.Unit.Status.PropertyChanged += StorageChangedEventFowarder;
                unit.Value.Unit.Status.PowerInfo.StorageId = unit.Key;
                unit.Value.Unit.Status.PowerInfo.PropertyChanged += StorageChangedEventFowarder;
            }
            foreach (var unit in DepositUnits)
            {
                unit.Value.StorageId = unit.Key;
                unit.Value.PropertyChanged += StorageChangedEventFowarder;
                unit.Value.Unit.Status.StorageId = unit.Key;
                unit.Value.Unit.Status.PropertyChanged += StorageChangedEventFowarder;
            }
        }
        #region Events

        public async Task StorageChangedEvent(object sender, PropertyChangedEventArgs propertyInfo)
        {
            await StorageChangedEventHander(sender, propertyInfo);
        }

        #endregion

        /// <summary>
        /// Storage or Count changed event handler
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StorageChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await StorageChangedEvent(sender, propertyInfo);
    }
}
