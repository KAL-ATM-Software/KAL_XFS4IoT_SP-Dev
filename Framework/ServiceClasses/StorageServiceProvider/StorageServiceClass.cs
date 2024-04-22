/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using System.ComponentModel;

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

            // Get unit information from the device class
            if (StorageType.HasFlag(StorageTypeEnum.Card))
            {
                ConstructCardStorage();
            }
            else
            {
                // Support cash and check compound units
                if (StorageType.HasFlag(StorageTypeEnum.Cash))
                {
                    ConstructCashUnits();
                }
                if (StorageType.HasFlag(StorageTypeEnum.Check))
                {
                    ConstructCheckUnits();
                }
            }
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
            if (newConfiguration &&
                newCardUnits?.Count > 0)
            {
                CardUnits.Clear();
                foreach (var unit in newCardUnits)
                {
                    CardUnits.Add(unit.Key, new CardUnitStorage(unit.Value));
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
                unit.Value.Unit.Status.ReplenishmentStatus = CardStatusClass.ReplenishmentStatusEnum.Healthy;

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
        /// Update storage count from the framework after media movement command is processed
        /// </summary>
        public async Task UpdateCardStorageCount(string storageId, int countDelta, string preservedStorage)
        {
            CardUnits.ContainsKey(storageId).IsTrue($"Unexpected storageId is passed in before updating card unit counters. {storageId}");

            string preserved = JsonSerializer.Serialize(CardUnits[storageId]);
            if (string.IsNullOrEmpty(preservedStorage))
            {
                preserved = preservedStorage;
            }

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

            // Send changed event
            if (preserved != JsonSerializer.Serialize(CardUnits[storageId]))
            {
                StorageUnitClass payload = new(
                    PositionName: CardUnits[storageId].PositionName,
                    Capacity: CardUnits[storageId].Capacity,
                    Status: CardUnits[storageId].Status switch
                    {
                        CardUnitStorage.StatusEnum.Good => StatusEnum.Ok,
                        CardUnitStorage.StatusEnum.Inoperative => StatusEnum.Inoperative,
                        CardUnitStorage.StatusEnum.Manipulated => StatusEnum.Manipulated,
                        CardUnitStorage.StatusEnum.Missing => StatusEnum.Missing,
                        _ => StatusEnum.NotConfigured,
                    },
                    SerialNumber: CardUnits[storageId].SerialNumber,
                    Cash: null,
                    Card: new(
                            Capabilities: new(
                                Type: CardUnits[storageId].Unit.Capabilities.Type switch
                                {
                                    CardCapabilitiesClass.TypeEnum.Dispense => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Dispense,
                                    CardCapabilitiesClass.TypeEnum.Retain => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Retain,
                                    _ => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Park,
                                },
                                HardwareSensors: CardUnits[storageId].Unit.Capabilities.HardwareSensors),
                            Configuration: new(
                                CardID: CardUnits[storageId].Unit.Configuration.CardId,
                                Threshold: CardUnits[storageId].Unit.Configuration.Threshold),
                            Status: new(
                                InitialCount: CardUnits[storageId].Unit.Status.InitialCount,
                                Count: CardUnits[storageId].Unit.Status.Count,
                                RetainCount: CardUnits[storageId].Unit.Status.RetainCount,
                                ReplenishmentStatus: CardUnits[storageId].Unit.Status.ReplenishmentStatus switch
                                {
                                    CardStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Empty,
                                    CardStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Full,
                                    CardStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.High,
                                    CardStatusClass.ReplenishmentStatusEnum.Low => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Low,
                                    _ => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Ok,
                                }
                                )
                            )
                    );

                // Device class must fire threshold event if the count is managed.
                StorageChangedEvent.PayloadData evPayload = new();
                Dictionary<string, StorageUnitClass> storageUnits = new()
                { 
                    { storageId, payload } 
                };
                evPayload.ExtendedProperties = storageUnits;
                await StorageChangedEvent(evPayload);
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

            Contracts.Assert(newCashUnits is not null && newCashUnits.Count != 0, "The device class returned an empty cash unit structure information on the GetCashStorageConfiguration.");

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
                    foreach (var unit in newCashUnits)
                    {
                        CashUnits.Add(unit.Key, new CashUnitStorage(unit.Value));
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

                bool updateInitialCounts = Device.GetCashUnitInitialCounts(out Dictionary<string, StorageCashCountClass> initialCounts);

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

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashStorageStatus()");

                updateStatus = Device.GetCashUnitStatus(out Dictionary<string, CashStatusClass.ReplenishmentStatusEnum> unitStatus);

                Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashStorageStatus()-> {updateStatus}");

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
        public async Task UpdateCashAccounting(Dictionary<string, CashUnitCountClass> countDelta, Dictionary<string, string> preservedStorage)
        {
            Dictionary<string, string> preserved = [];
            foreach (var unit in CashUnits)
            {
                preserved.Add(unit.Key, JsonSerializer.Serialize(unit.Value));
            }
            if (preservedStorage is not null)
                preserved = preservedStorage;

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
                unit.Value.Unit.Status.ReplenishmentStatus = CashStatusClass.ReplenishmentStatusEnum.Healthy;

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
                        }
                    }
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

            // Send changed event
            Dictionary<string, StorageUnitClass> statusChangedUnits = [];
            foreach (var unit in CashUnits)
            {
                if (JsonSerializer.Serialize(unit.Value) ==
                    preserved[unit.Key])
                {
                    continue;
                }

                StorageUnitClass payload = new(
                    Id: unit.Value.Id,
                    PositionName: unit.Value.PositionName,
                    Capacity: unit.Value.Capacity,
                    Status: unit.Value.Status switch
                    {
                        CashUnitStorage.StatusEnum.Good => StatusEnum.Ok,
                        CashUnitStorage.StatusEnum.Inoperative => StatusEnum.Inoperative,
                        CashUnitStorage.StatusEnum.Manipulated => StatusEnum.Manipulated,
                        CashUnitStorage.StatusEnum.Missing => StatusEnum.Missing,
                        _ => StatusEnum.NotConfigured,
                    },
                    SerialNumber: unit.Value.SerialNumber,
                    Cash: new(
                        Capabilities: new(
                            Types: new(
                                CashIn: unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                CashOut: unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                Replenishment: unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                CashInRetract: unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                CashOutRetract: unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                Reject: unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)
                                ),
                            Items: new(
                                Fit: unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                Unfit: unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                Unrecognized: unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                Counterfeit: unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Counterfeit),
                                Suspect: unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                Inked: unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                Coupon: unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                Document: unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)
                                ),
                            HardwareSensors: unit.Value.Unit.Capabilities.HardwareSensors,
                            RetractAreas: unit.Value.Unit.Capabilities.RetractAreas,
                            RetractThresholds: unit.Value.Unit.Capabilities.RetractThresholds,
                            CashItems: unit.Value.Unit.Capabilities.BanknoteItems),
                        Configuration: new(
                            Types: new(
                                CashIn: unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                CashOut: unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                Replenishment: unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                CashInRetract: unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                CashOutRetract: unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                Reject: unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)
                                ),
                            Items: new(
                                Fit: unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                Unfit: unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                Unrecognized: unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                Counterfeit: unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Counterfeit),
                                Suspect: unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                Inked: unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                Coupon: unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                Document: unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)
                                ),
                            Currency: unit.Value.Unit.Configuration.Currency,
                            Value: unit.Value.Unit.Configuration.Value,
                            HighThreshold: unit.Value.Unit.Configuration.HighThreshold,
                            LowThreshold: unit.Value.Unit.Configuration.LowThreshold,
                            AppLockIn: unit.Value.Unit.Configuration.AppLockIn,
                            AppLockOut: unit.Value.Unit.Configuration.AppLockOut,
                            CashItems: unit.Value.Unit.Configuration.BanknoteItems),
                        Status: new(
                            Index: unit.Value.Unit.Status.Index,
                            Initial: unit.Value.Unit.Status.InitialCounts?.CopyTo(),
                            Out: new(
                                Presented: unit.Value.Unit.Status.StorageCashOutCount?.Presented.CopyTo(),
                                Rejected: unit.Value.Unit.Status.StorageCashOutCount?.Rejected.CopyTo(),
                                Distributed: unit.Value.Unit.Status.StorageCashOutCount?.Distributed.CopyTo(),
                                Unknown: unit.Value.Unit.Status.StorageCashOutCount?.Unknown.CopyTo(),
                                Stacked: unit.Value.Unit.Status.StorageCashOutCount?.Stacked.CopyTo(),
                                Diverted: unit.Value.Unit.Status.StorageCashOutCount?.Diverted.CopyTo(),
                                Transport: unit.Value.Unit.Status.StorageCashOutCount?.Transport.CopyTo()),
                            In: new(
                                RetractOperations: unit.Value.Unit.Status.StorageCashInCount?.RetractOperations,
                                Deposited: unit.Value.Unit.Status.StorageCashInCount?.Deposited.CopyTo(),
                                Retracted: unit.Value.Unit.Status.StorageCashInCount?.Retracted.CopyTo(),
                                Rejected: unit.Value.Unit.Status.StorageCashInCount?.Rejected.CopyTo(),
                                Distributed: unit.Value.Unit.Status.StorageCashInCount?.Distributed.CopyTo(),
                                Transport: unit.Value.Unit.Status.StorageCashInCount?.Transport.CopyTo()),
                            Accuracy: unit.Value.Unit.Status.Accuracy switch 
                            {
                                CashStatusClass.AccuracyEnum.Accurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Accurate,
                                CashStatusClass.AccuracyEnum.AccurateSet => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.AccurateSet,
                                CashStatusClass.AccuracyEnum.Inaccurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Inaccurate,
                                CashStatusClass.AccuracyEnum.Unknown => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Unknown,
                                _ => null,
                            },
                            ReplenishmentStatus: unit.Value.Unit.Status.ReplenishmentStatus switch
                            {
                                CashStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Empty,
                                CashStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Full,
                                CashStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Ok,
                                CashStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.CashManagement.ReplenishmentStatusEnum.High,
                                CashStatusClass.ReplenishmentStatusEnum.Low => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Low,
                                _ => throw new InternalErrorException($"Unexpected cash unit status specified. Unit:{unit.Key} Status:{unit.Value.Unit.Status.ReplenishmentStatus}"),
                            }
                            )
                        ),
                    Card: null);

                statusChangedUnits.Add(unit.Key, payload);
            }

            // Device class must fire status event if the count is managed by the device class.
            if (statusChangedUnits.Count > 0)
            {
                StorageChangedEvent.PayloadData evPayload = new()
                {
                    ExtendedProperties = statusChangedUnits
                };
                await StorageChangedEvent(evPayload);
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
        private void UpdateDeltaStorageCashCount(string storageId, StorageCashCountClass storageCashCount, StorageCashCountClass storageDeltaCount)
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

            Contracts.Assert(newCheckUnits is not null && newCheckUnits.Count != 0, "The device class returned an empty cash unit structure information on the GetCheckStorageConfiguration.");
            
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
                            Logger.Warning(Constants.Framework, $"Existing check unit information doesn't have an identical cash unit structure information specified by the device class. {unit.Key}. Construct new cash unit infomation.");
                            break;
                        }
                    }
                }

                if (!identical)
                {
                    CheckUnits.Clear();
                    foreach (var unit in newCheckUnits)
                    {
                        CheckUnits.Add(unit.Key, new CheckUnitStorage(unit.Value));
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
                        CheckUnits[unit.Key].Unit.Status.CheckInCounts = new(unit.Value);
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
                        CheckUnits[unit.Key].Unit.Status.InitialCounts = new(unit.Value);
                    }
                }

                // Update status from count
                foreach (var unit in CheckUnits)
                {
                    // update status logically first and overwrite status if the device class requires.
                    unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.Healthy;

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
        public async Task UpdateCheckStorageCount(Dictionary<string, StorageCheckCountClass> countDelta, Dictionary<string, string> preservedStorage)
        {
            Dictionary<string, string> preserved = [];
            foreach (var unit in CheckUnits)
            {
                preserved.Add(unit.Key, JsonSerializer.Serialize(unit.Value));
            }
            if (preservedStorage is not null)
                preserved = preservedStorage;

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
                unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.Healthy;

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
                            sendThresholdEvent.Add(unit.Key, false);
                        {
                            unit.Value.Unit.Status.ReplenishmentStatus = CheckStatusClass.ReplenishmentStatusEnum.High;
                        }
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
                    }
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

            // Send changed event
            Dictionary<string, StorageUnitClass> statusChangedUnits = [];
            foreach (var unit in CheckUnits)
            {
                if (JsonSerializer.Serialize(unit.Value) ==
                    preserved[unit.Key])
                {
                    continue;
                }

                StorageUnitClass payload = new(
                    PositionName: unit.Value.PositionName,
                    Capacity: unit.Value.Capacity,
                    Status: unit.Value.Status switch
                    {
                        CashUnitStorage.StatusEnum.Good => StatusEnum.Ok,
                        CashUnitStorage.StatusEnum.Inoperative => StatusEnum.Inoperative,
                        CashUnitStorage.StatusEnum.Manipulated => StatusEnum.Manipulated,
                        CashUnitStorage.StatusEnum.Missing => StatusEnum.Missing,
                        _ => StatusEnum.NotConfigured,
                    },
                    SerialNumber: unit.Value.SerialNumber,
                    Check: new(
                        Capabilities: new(
                            Types: new(
                                MediaIn: unit.Value.Unit.Capabilities.Types.HasFlag(CheckCapabilitiesClass.TypesEnum.MediaIn),
                                Retract: unit.Value.Unit.Capabilities.Types.HasFlag(CheckCapabilitiesClass.TypesEnum.Retract)
                                ),
                            Sensors: new(
                                Empty: unit.Value.Unit.Capabilities.Sensors.HasFlag(CheckCapabilitiesClass.SensorEnum.Empty),
                                High: unit.Value.Unit.Capabilities.Sensors.HasFlag(CheckCapabilitiesClass.SensorEnum.High),
                                Full: unit.Value.Unit.Capabilities.Sensors.HasFlag(CheckCapabilitiesClass.SensorEnum.Full)
                                )
                            ),
                        Configuration: new(
                            Types: new(
                                MediaIn: unit.Value.Unit.Configuration.Types.HasFlag(CheckCapabilitiesClass.TypesEnum.MediaIn),
                                Retract: unit.Value.Unit.Configuration.Types.HasFlag(CheckCapabilitiesClass.TypesEnum.Retract)
                                ),
                            BinID: unit.Value.Unit.Configuration.Id,
                            HighThreshold: unit.Value.Unit.Configuration.HighThreshold,
                            RetractHighThreshold: unit.Value.Unit.Configuration.RetractHighThreshold),
                        Status: new(
                            Index: unit.Value.Unit.Status.Index,
                            Initial: new(
                                MediaInCount: unit.Value.Unit.Status.InitialCounts.MediaInCount,
                                Count: unit.Value.Unit.Status.InitialCounts.Count,
                                RetractOperations: unit.Value.Unit.Status.InitialCounts.RetractOperations),
                            In: new(
                                MediaInCount: unit.Value.Unit.Status.CheckInCounts.MediaInCount,
                                Count: unit.Value.Unit.Status.CheckInCounts.Count,
                                RetractOperations: unit.Value.Unit.Status.CheckInCounts.RetractOperations),
                            ReplenishmentStatus: unit.Value.Unit.Status.ReplenishmentStatus switch
                            {
                                CheckStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.Check.ReplenishmentStatusEnum.Empty,
                                CheckStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.Check.ReplenishmentStatusEnum.Full,
                                CheckStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.Check.ReplenishmentStatusEnum.Ok,
                                CheckStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.Check.ReplenishmentStatusEnum.High,
                                _ => throw new InternalErrorException($"Unexpected check status specified. Unit:{unit.Key} Stauts:{unit.Value.Unit.Status.ReplenishmentStatus}"),
                            }
                            )
                        )
                    );

                statusChangedUnits.Add(unit.Key, payload);
            }

            // Device class must fire status event if the count is managed by the device class.
            if (statusChangedUnits.Count > 0)
            {
                StorageChangedEvent.PayloadData evPayload = new()
                {
                    ExtendedProperties = statusChangedUnits
                };
                await StorageChangedEvent(evPayload);
            }

            if (!PersistentData.Store(ServiceProvider.Name + typeof(CheckUnitStorage).FullName, CheckUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data.");
            }
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
        /// Return XFS4IoT storage structured object.
        /// </summary>
        public Dictionary<string, XFS4IoT.Storage.StorageUnitClass> GetStorages(List<string> UnitIds)
        {
            Dictionary<string, XFS4IoT.Storage.StorageUnitClass> storageUnits = null;

            foreach (var storageId in UnitIds)
            {
                if (StorageType == StorageTypeEnum.Card)
                {
                    if (!CardUnits.ContainsKey(storageId))
                        continue;

                    StorageUnitClass thisStorage = new(
                        PositionName: CardUnits[storageId].PositionName,
                        Capacity: CardUnits[storageId].Capacity,
                        Status: CardUnits[storageId].Status switch
                        {
                            CashUnitStorage.StatusEnum.Good => StatusEnum.Ok,
                            CashUnitStorage.StatusEnum.Inoperative => StatusEnum.Inoperative,
                            CashUnitStorage.StatusEnum.Manipulated => StatusEnum.Manipulated,
                            CashUnitStorage.StatusEnum.Missing => StatusEnum.Missing,
                            _ => StatusEnum.NotConfigured,
                        },
                        SerialNumber: CardUnits[storageId].SerialNumber,
                        Card: new(
                            Capabilities: new(
                                Type: CardUnits[storageId].Unit.Capabilities.Type switch
                                {
                                    CardCapabilitiesClass.TypeEnum.Dispense => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Dispense,
                                    CardCapabilitiesClass.TypeEnum.Retain => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Retain,
                                    _ => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Park,
                                },
                                HardwareSensors: CardUnits[storageId].Unit.Capabilities.HardwareSensors),
                            Configuration: new(
                                CardID: CardUnits[storageId].Unit.Configuration.CardId,
                                Threshold: CardUnits[storageId].Unit.Configuration.Threshold),
                            Status: new(
                                InitialCount: CardUnits[storageId].Unit.Status.InitialCount,
                                Count: CardUnits[storageId].Unit.Status.Count,
                                RetainCount: CardUnits[storageId].Unit.Status.RetainCount,
                                ReplenishmentStatus: CardUnits[storageId].Unit.Status.ReplenishmentStatus switch
                                {
                                    CardStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Empty,
                                    CardStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Full,
                                    CardStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.High,
                                    CardStatusClass.ReplenishmentStatusEnum.Low => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Low,
                                    _ => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Ok,
                                })
                            )
                        );

                    (storageUnits ??= []).Add(storageId, thisStorage);
                    // Card unit is not supporting combined storage with other types.
                    continue;
                }

                if (StorageType.HasFlag(StorageTypeEnum.Cash) ||
                    StorageType.HasFlag(StorageTypeEnum.Check))
                {
                    // The device could be mixed media cash and check

                    if (StorageType.HasFlag(StorageTypeEnum.Cash) &&
                        CashUnits.ContainsKey(storageId))
                    {
                        XFS4IoT.Storage.StorageUnitClass thisStorage = new(
                            Id: CashUnits[storageId].Id,
                            PositionName: CashUnits[storageId].PositionName,
                            Capacity: CashUnits[storageId].Capacity,
                            Status: CashUnits[storageId].Status switch
                            {
                                CashUnitStorage.StatusEnum.Good => StatusEnum.Ok,
                                CashUnitStorage.StatusEnum.Inoperative => StatusEnum.Inoperative,
                                CashUnitStorage.StatusEnum.Manipulated => StatusEnum.Manipulated,
                                CashUnitStorage.StatusEnum.Missing => StatusEnum.Missing,
                                _ => StatusEnum.NotConfigured,
                            },
                            SerialNumber: CashUnits[storageId].SerialNumber,
                            Cash: new(
                                Capabilities: new(
                                    Types: new(
                                        CashIn: CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                        CashOut: CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                        Replenishment: CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                        CashInRetract: CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                        CashOutRetract: CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                        Reject: CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)
                                        ),
                                    Items: new(
                                        Fit: CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                        Unfit: CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                        Unrecognized: CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                        Counterfeit: CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Counterfeit),
                                        Suspect: CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                        Inked: CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                        Coupon: CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                        Document: CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)
                                        ),
                                    HardwareSensors: CashUnits[storageId].Unit.Capabilities.HardwareSensors,
                                    RetractAreas: CashUnits[storageId].Unit.Capabilities.RetractAreas,
                                    RetractThresholds: CashUnits[storageId].Unit.Capabilities.RetractThresholds,
                                    CashItems: CashUnits[storageId].Unit.Configuration.BanknoteItems
                                    ),
                                Configuration: new(
                                    Types: new(
                                        CashIn: CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                        CashOut: CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                        Replenishment: CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                        CashInRetract: CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                        CashOutRetract: CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                        Reject: CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)
                                        ),
                                    Items: new(
                                        Fit: CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                        Unfit: CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                        Unrecognized: CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                        Counterfeit: CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Counterfeit),
                                        Suspect: CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                        Inked: CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                        Coupon: CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                        Document: CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)
                                        ),
                                    Currency: CashUnits[storageId].Unit.Configuration.Currency,
                                    Value: CashUnits[storageId].Unit.Configuration.Value,
                                    HighThreshold: CashUnits[storageId].Unit.Configuration.HighThreshold,
                                    LowThreshold: CashUnits[storageId].Unit.Configuration.LowThreshold,
                                    AppLockIn: CashUnits[storageId].Unit.Configuration.AppLockIn,
                                    AppLockOut: CashUnits[storageId].Unit.Configuration.AppLockOut,
                                    CashItems: CashUnits[storageId].Unit.Configuration.BanknoteItems
                                    ),
                                    Status: new(
                                        Index: CashUnits[storageId].Unit.Status.Index,
                                        Initial: CashUnits[storageId].Unit.Status.InitialCounts?.CopyTo(),
                                        Out: new(
                                            Presented: CashUnits[storageId].Unit.Status.StorageCashOutCount?.Presented?.CopyTo(),
                                            Rejected: CashUnits[storageId].Unit.Status.StorageCashOutCount?.Rejected?.CopyTo(),
                                            Distributed: CashUnits[storageId].Unit.Status.StorageCashOutCount?.Distributed?.CopyTo(),
                                            Unknown: CashUnits[storageId].Unit.Status.StorageCashOutCount?.Unknown?.CopyTo(),
                                            Stacked: CashUnits[storageId].Unit.Status.StorageCashOutCount?.Stacked?.CopyTo(),
                                            Diverted: CashUnits[storageId].Unit.Status.StorageCashOutCount?.Diverted?.CopyTo(),
                                            Transport: CashUnits[storageId].Unit.Status.StorageCashOutCount?.Transport?.CopyTo()
                                            ),
                                        In: new(
                                            RetractOperations: CashUnits[storageId].Unit.Status.StorageCashInCount?.RetractOperations,
                                            Deposited: CashUnits[storageId].Unit.Status.StorageCashInCount?.Deposited?.CopyTo(),
                                            Retracted: CashUnits[storageId].Unit.Status.StorageCashInCount?.Retracted?.CopyTo(),
                                            Rejected: CashUnits[storageId].Unit.Status.StorageCashInCount?.Rejected?.CopyTo(),
                                            Distributed: CashUnits[storageId].Unit.Status.StorageCashInCount?.Distributed?.CopyTo(),
                                            Transport: CashUnits[storageId].Unit.Status.StorageCashInCount?.Transport?.CopyTo()
                                            ),
                                        Accuracy: CashUnits[storageId].Unit.Status.Accuracy switch
                                        {
                                            CashStatusClass.AccuracyEnum.Accurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Accurate,
                                            CashStatusClass.AccuracyEnum.AccurateSet => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.AccurateSet,
                                            CashStatusClass.AccuracyEnum.Inaccurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Inaccurate,
                                            CashStatusClass.AccuracyEnum.Unknown => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Unknown,
                                            _ => null,
                                        },
                                        ReplenishmentStatus: CashUnits[storageId].Unit.Status.ReplenishmentStatus switch
                                        {
                                            CashStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Empty,
                                            CashStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Full,
                                            CashStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Ok,
                                            CashStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.CashManagement.ReplenishmentStatusEnum.High,
                                            CashStatusClass.ReplenishmentStatusEnum.Low => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Low,
                                            _ => throw new InternalErrorException($"Unexpected cash unit status specified. Unit:{storageId} Status:{CashUnits[storageId].Unit.Status.ReplenishmentStatus}"),
                                        }
                                        )
                                    )
                                );

                        (storageUnits ??= []).Add(storageId, thisStorage);
                    }
                    if (StorageType.HasFlag(StorageTypeEnum.Check))
                    {
                        if (!CheckUnits.ContainsKey(storageId))
                            continue;

                        XFS4IoT.CashManagement.StorageCashClass cashStorage = null;

                        if (storageUnits is not null &&
                            storageUnits.ContainsKey(storageId))
                        {
                            // Check and Cash combined unit
                            cashStorage = storageUnits[storageId].Cash;
                            storageUnits.Remove(storageId);
                        }
                        StorageUnitClass thisStorage = new(
                            PositionName: CheckUnits[storageId].PositionName,
                            Capacity: CheckUnits[storageId].Capacity,
                            Status: CheckUnits[storageId].Status switch
                            {
                                CashUnitStorage.StatusEnum.Good => StatusEnum.Ok,
                                CashUnitStorage.StatusEnum.Inoperative => StatusEnum.Inoperative,
                                CashUnitStorage.StatusEnum.Manipulated => StatusEnum.Manipulated,
                                CashUnitStorage.StatusEnum.Missing => StatusEnum.Missing,
                                _ => StatusEnum.NotConfigured,
                            },
                            SerialNumber: CheckUnits[storageId].SerialNumber,
                            Cash: cashStorage,
                            Check: new(
                                Capabilities: new(
                                    Types: new(
                                        MediaIn: CheckUnits[storageId].Unit.Capabilities.Types.HasFlag(CheckCapabilitiesClass.TypesEnum.MediaIn),
                                        Retract: CheckUnits[storageId].Unit.Capabilities.Types.HasFlag(CheckCapabilitiesClass.TypesEnum.Retract)
                                        ),
                                    Sensors: new(
                                        Empty: CheckUnits[storageId].Unit.Capabilities.Sensors.HasFlag(CheckCapabilitiesClass.SensorEnum.Empty),
                                        High: CheckUnits[storageId].Unit.Capabilities.Sensors.HasFlag(CheckCapabilitiesClass.SensorEnum.High),
                                        Full: CheckUnits[storageId].Unit.Capabilities.Sensors.HasFlag(CheckCapabilitiesClass.SensorEnum.Full)
                                        )
                                    ),
                                Configuration: new(
                                    Types: new(
                                        MediaIn: CheckUnits[storageId].Unit.Configuration.Types.HasFlag(CheckCapabilitiesClass.TypesEnum.MediaIn),
                                        Retract: CheckUnits[storageId].Unit.Configuration.Types.HasFlag(CheckCapabilitiesClass.TypesEnum.Retract)
                                        ),
                                    BinID: CheckUnits[storageId].Unit.Configuration.Id,
                                    HighThreshold: CheckUnits[storageId].Unit.Configuration.HighThreshold > 0 ?
                                    CheckUnits[storageId].Unit.Configuration.HighThreshold :
                                    null,
                                    RetractHighThreshold: CheckUnits[storageId].Unit.Configuration.RetractHighThreshold > 0 ?
                                    CheckUnits[storageId].Unit.Configuration.RetractHighThreshold :
                                    null),
                                Status: new(
                                    Index: CheckUnits[storageId].Unit.Status.Index,
                                    Initial: new(
                                        MediaInCount: CheckUnits[storageId].Unit.Status.InitialCounts.MediaInCount,
                                        Count: CheckUnits[storageId].Unit.Status.InitialCounts.Count,
                                        RetractOperations: CheckUnits[storageId].Unit.Status.InitialCounts.RetractOperations
                                        ),
                                    In: new(
                                        MediaInCount: CheckUnits[storageId].Unit.Status.CheckInCounts.MediaInCount,
                                        Count: CheckUnits[storageId].Unit.Status.CheckInCounts.Count,
                                        RetractOperations: CheckUnits[storageId].Unit.Status.CheckInCounts.RetractOperations
                                        ),
                                    ReplenishmentStatus: CheckUnits[storageId].Unit.Status.ReplenishmentStatus switch
                                    {
                                        CheckStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.Check.ReplenishmentStatusEnum.Empty,
                                        CheckStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.Check.ReplenishmentStatusEnum.Full,
                                        CheckStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.Check.ReplenishmentStatusEnum.Ok,
                                        CheckStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.Check.ReplenishmentStatusEnum.High,
                                        _ => throw new InternalErrorException($"Unexpected check status specified. Unit:{storageId} Stauts:{CheckUnits[storageId].Unit.Status.ReplenishmentStatus}"),
                                    }
                                    )
                                )
                            );

                        (storageUnits ??= []).Add(storageId, thisStorage);
                    }
                }
            }

            return storageUnits;
        }
    }
}
