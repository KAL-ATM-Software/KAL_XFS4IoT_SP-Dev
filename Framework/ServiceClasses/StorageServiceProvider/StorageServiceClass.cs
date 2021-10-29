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
using System.Text.Json;
using XFS4IoT;
using XFS4IoTFramework.Storage;
using XFS4IoTFramework.Common;
using XFS4IoT.Storage.Events;
using XFS4IoT.Storage;

namespace XFS4IoTServer
{
    public partial class StorageServiceClass
    {
        public StorageServiceClass(IServiceProvider ServiceProvider,
                                   ICommonService CommonService,
                                   ILogger logger, 
                                   IPersistentData PersistentData, 
                                   StorageTypeEnum StorageType)
            : this(ServiceProvider, logger)
        {
            CommonService.IsNotNull($"Unexpected parameter set for common service in the " + nameof(StorageServiceClass));
            this.CommonService = CommonService.IsA<ICommonService>($"Invalid interface parameter specified for common service. " + nameof(StorageServiceClass));

            this.PersistentData = PersistentData;
            this.StorageType = StorageType;

            // Load persistent data
            CardUnits = PersistentData.Load<Dictionary<string, CardUnitStorage>>(ServiceProvider.Name + typeof(CardUnitStorage).FullName);
            if (CardUnits is null)
            {
                Logger.Warning(Constants.Framework, "Failed to load persistent data for card units. It could be a first run, SP type is CashDispenser/Recycler/Acceptor or no persistent exists on the file system.");
                CardUnits = new();
            }
            CashUnits = PersistentData.Load<Dictionary<string, CashUnitStorage>>(ServiceProvider.Name + typeof(CashUnitStorage).FullName);
            if (CashUnits is null)
            {
                Logger.Warning(Constants.Framework, "Failed to load persistent data for cash units. It could be a first run, SP type CardReader/CardDispenser or no persistent exists on the file system.");
                CashUnits = new();
            }

            if (StorageType == StorageTypeEnum.Card)
            {
                ConstructCardStorage();
            }
            else
            {
                ConstructCashUnits();
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
                (newCardUnits is not null &&
                 newCardUnits.Count > 0))
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
            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCardStorageStatus()");

            updateStatus = Device.GetCardUnitStatus(out Dictionary<string, CardStatusClass.ReplenishmentStatusEnum> unitStatus);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCardStorageStatus()-> {updateStatus}");

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
                CardUnits[storageId].Unit.Capabilities.Type == CardCapabilitiesClass.TypeEnum.Pard)
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
                    StorageUnitClass payload = new(CardUnits[storageId].PositionName,
                                                   CardUnits[storageId].Capacity,
                                                   CardUnits[storageId].Status switch
                                                   {
                                                       CardUnitStorage.StatusEnum.Good => StatusEnum.Ok,
                                                       CardUnitStorage.StatusEnum.Inoperative => StatusEnum.Inoperative,
                                                       CardUnitStorage.StatusEnum.Manipulated => StatusEnum.Manipulated,
                                                       CardUnitStorage.StatusEnum.Missing => StatusEnum.Missing,
                                                       _ => StatusEnum.NotConfigured,
                                                   },
                                                   CardUnits[storageId].SerialNumber,
                                                   Cash: null,
                                                   Card: new XFS4IoT.CardReader.StorageClass(
                                                           new XFS4IoT.CardReader.StorageCapabilitiesClass(CardUnits[storageId].Unit.Capabilities.Type switch
                                                                                                           {
                                                                                                               CardCapabilitiesClass.TypeEnum.Dispense => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Dispense,
                                                                                                               CardCapabilitiesClass.TypeEnum.Retain => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Retain,
                                                                                                               _ => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Park,
                                                                                                           },
                                                                                                           CardUnits[storageId].Unit.Capabilities.HardwareSensors),
                                                           new XFS4IoT.CardReader.StorageConfigurationClass(CardUnits[storageId].Unit.Configuration.CardId,
                                                                                                            CardUnits[storageId].Unit.Configuration.Threshold),
                                                           new XFS4IoT.CardReader.StorageStatusClass(CardUnits[storageId].Unit.Status.InitialCount,
                                                                                                     CardUnits[storageId].Unit.Status.Count,
                                                                                                     CardUnits[storageId].Unit.Status.RetainCount,
                                                                                                     CardUnits[storageId].Unit.Status.ReplenishmentStatus switch
                                                                                                     {
                                                                                                         CardStatusClass.ReplenishmentStatusEnum.Empty => ReplenishmentStatusEnumEnum.Empty,
                                                                                                         CardStatusClass.ReplenishmentStatusEnum.Full => ReplenishmentStatusEnumEnum.Full,
                                                                                                         CardStatusClass.ReplenishmentStatusEnum.High => ReplenishmentStatusEnumEnum.High,
                                                                                                         CardStatusClass.ReplenishmentStatusEnum.Low => ReplenishmentStatusEnumEnum.Low,
                                                                                                         _ => ReplenishmentStatusEnumEnum.Ok,
                                                                                                     })));

                    // Device class must fire threshold event if the count is managed.
                    await StorageThresholdEvent(new StorageThresholdEvent.PayloadData(new() { { storageId, payload } }));
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
                StorageUnitClass payload = new(CardUnits[storageId].PositionName,
                                               CardUnits[storageId].Capacity,
                                               CardUnits[storageId].Status switch
                                               {
                                                   CardUnitStorage.StatusEnum.Good => StatusEnum.Ok,
                                                   CardUnitStorage.StatusEnum.Inoperative => StatusEnum.Inoperative,
                                                   CardUnitStorage.StatusEnum.Manipulated => StatusEnum.Manipulated,
                                                   CardUnitStorage.StatusEnum.Missing => StatusEnum.Missing,
                                                   _ => StatusEnum.NotConfigured,
                                               },
                                               CardUnits[storageId].SerialNumber,
                                               Cash: null,
                                               Card: new XFS4IoT.CardReader.StorageClass(
                                                        new XFS4IoT.CardReader.StorageCapabilitiesClass(CardUnits[storageId].Unit.Capabilities.Type switch
                                                                                                        {
                                                                                                            CardCapabilitiesClass.TypeEnum.Dispense => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Dispense,
                                                                                                            CardCapabilitiesClass.TypeEnum.Retain => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Retain,
                                                                                                            _ => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Park,
                                                                                                        },
                                                                                                        CardUnits[storageId].Unit.Capabilities.HardwareSensors),
                                                        new XFS4IoT.CardReader.StorageConfigurationClass(CardUnits[storageId].Unit.Configuration.CardId,
                                                                                                         CardUnits[storageId].Unit.Configuration.Threshold),
                                                        new XFS4IoT.CardReader.StorageStatusClass(CardUnits[storageId].Unit.Status.InitialCount,
                                                                                                  CardUnits[storageId].Unit.Status.Count,
                                                                                                  CardUnits[storageId].Unit.Status.RetainCount,
                                                                                                  CardUnits[storageId].Unit.Status.ReplenishmentStatus switch
                                                                                                  {
                                                                                                      CardStatusClass.ReplenishmentStatusEnum.Empty => ReplenishmentStatusEnumEnum.Empty,
                                                                                                      CardStatusClass.ReplenishmentStatusEnum.Full => ReplenishmentStatusEnumEnum.Full,
                                                                                                      CardStatusClass.ReplenishmentStatusEnum.High => ReplenishmentStatusEnumEnum.High,
                                                                                                      CardStatusClass.ReplenishmentStatusEnum.Low => ReplenishmentStatusEnumEnum.Low,
                                                                                                      _ => ReplenishmentStatusEnumEnum.Ok,
                                                                                                  })));

                // Device class must fire threshold event if the count is managed.
                await StorageChangedEvent(new  StorageChangedEvent.PayloadData(new() { { storageId, payload } }));
            }
        }
        #endregion

        #region Cash
        /// <summary>
        /// ConstructCashUnits
        /// The method retreive cash unit structures from the device class. The device class must provide cash unit structure info
        /// </summary>
        private void ConstructCashUnits()
        {
            Logger.Log(Constants.DeviceClass, "StorageDev.GetCashUnitConfiguration()");

            bool newConfiguration = Device.GetCashStorageConfiguration(out Dictionary<string, CashUnitStorageConfiguration> newCashUnits);

            Logger.Log(Constants.DeviceClass, $"StorageDev.GetCashUnitConfiguration()-> {newConfiguration}");

            newCashUnits.IsNotNull("The device class returned an empty cash unit structure information on the GetCashUnitStructure.");

            bool updateCashUnitFromDeviceClass = newConfiguration;
            if (newConfiguration)
            {
                if (newCashUnits is null ||
                    newCashUnits.Count == 0)
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
                bool identical = newCashUnits.Count == CashUnits.Count;
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
                        CashConfigurationClass cong = CashUnits[unit.Key].Unit.Configuration;
                        CashUnits[unit.Key].Unit.Status.Count = unit.Value.Count;
                        CashUnits[unit.Key].Unit.Status.StorageCashInCount = unit.Value.StorageCashInCount;
                        CashUnits[unit.Key].Unit.Status.StorageCashOutCount = unit.Value.StorageCashOutCount;
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

            // Update cash unit accounting
            if (!PersistentData.Store(ServiceProvider.Name + typeof(CashUnit).FullName, CashUnits))
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
            Dictionary<string, string> preserved = new();
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
                        UpdateDeltaStorageCashCount(CashUnits[delta.Key].Unit.Status.StorageCashOutCount.Distributed, delta.Value.StorageCashOutCount.Distributed);
                        UpdateDeltaStorageCashCount(CashUnits[delta.Key].Unit.Status.StorageCashOutCount.Diverted, delta.Value.StorageCashOutCount.Diverted);
                        UpdateDeltaStorageCashCount(CashUnits[delta.Key].Unit.Status.StorageCashOutCount.Presented, delta.Value.StorageCashOutCount.Presented);
                        UpdateDeltaStorageCashCount(CashUnits[delta.Key].Unit.Status.StorageCashOutCount.Rejected, delta.Value.StorageCashOutCount.Rejected);
                        UpdateDeltaStorageCashCount(CashUnits[delta.Key].Unit.Status.StorageCashOutCount.Stacked, delta.Value.StorageCashOutCount.Stacked);
                        UpdateDeltaStorageCashCount(CashUnits[delta.Key].Unit.Status.StorageCashOutCount.Transport, delta.Value.StorageCashOutCount.Transport);
                        UpdateDeltaStorageCashCount(CashUnits[delta.Key].Unit.Status.StorageCashOutCount.Unknown, delta.Value.StorageCashOutCount.Unknown);
                    }

                    if (delta.Value.StorageCashInCount is not null)
                    {
                        CashUnits[delta.Key].Unit.Status.StorageCashInCount.RetractOperations += delta.Value.StorageCashInCount.RetractOperations;
                        UpdateDeltaStorageCashCount(CashUnits[delta.Key].Unit.Status.StorageCashInCount.Deposited, delta.Value.StorageCashInCount.Deposited);
                        UpdateDeltaStorageCashCount(CashUnits[delta.Key].Unit.Status.StorageCashInCount.Distributed, delta.Value.StorageCashInCount.Distributed);
                        UpdateDeltaStorageCashCount(CashUnits[delta.Key].Unit.Status.StorageCashInCount.Rejected, delta.Value.StorageCashInCount.Rejected);
                        UpdateDeltaStorageCashCount(CashUnits[delta.Key].Unit.Status.StorageCashInCount.Retracted, delta.Value.StorageCashInCount.Retracted);
                        UpdateDeltaStorageCashCount(CashUnits[delta.Key].Unit.Status.StorageCashInCount.Transport, delta.Value.StorageCashInCount.Transport);
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
                    CashUnits[unitCount.Key].Unit.Status.StorageCashOutCount = unitCount.Value.StorageCashOutCount;
                    CashUnits[unitCount.Key].Unit.Status.StorageCashInCount = unitCount.Value.StorageCashInCount;
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

            List<string> sendThresholdEvent = new();

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
            Dictionary<string, StorageUnitClass> thresholdUnits = new();
            foreach (var unitId in sendThresholdEvent)
            {
                if (!CashUnits.ContainsKey(unitId))
                {
                    continue;
                }

                Dictionary<string, XFS4IoT.CashManagement.CashItemClass> capItems = new();
                if (CashUnits[unitId].Unit.Capabilities.BanknoteItems is not null)
                {
                    foreach (var item in CashUnits[unitId].Unit.Capabilities.BanknoteItems)
                    {
                        capItems.Add(item.Key, new XFS4IoT.CashManagement.CashItemClass(item.Value.NoteId,
                                                                                        item.Value.Currency,
                                                                                        item.Value.Value,
                                                                                        item.Value.Release));
                    }
                }

                Dictionary<string, XFS4IoT.CashManagement.CashItemClass> confItems = new();
                if (CashUnits[unitId].Unit.Configuration.BanknoteItems is not null)
                {
                    foreach (var item in CashUnits[unitId].Unit.Configuration.BanknoteItems)
                    {
                        confItems.Add(item.Key, new XFS4IoT.CashManagement.CashItemClass(item.Value.NoteId,
                                                                                         item.Value.Currency,
                                                                                         item.Value.Value,
                                                                                         item.Value.Release));
                    }
                }

                StorageUnitClass payload = new(CashUnits[unitId].PositionName,
                                               CashUnits[unitId].Capacity,
                                               CashUnits[unitId].Status switch
                                               {
                                                   CashUnitStorage.StatusEnum.Good => StatusEnum.Ok,
                                                   CashUnitStorage.StatusEnum.Inoperative => StatusEnum.Inoperative,
                                                   CashUnitStorage.StatusEnum.Manipulated => StatusEnum.Manipulated,
                                                   CashUnitStorage.StatusEnum.Missing => StatusEnum.Missing,
                                                   _ => StatusEnum.NotConfigured,
                                               },
                                               CashUnits[unitId].SerialNumber,
                                               Cash: new XFS4IoT.CashManagement.StorageCashClass(
                                                        new XFS4IoT.CashManagement.StorageCashCapabilitiesClass(new XFS4IoT.CashManagement.StorageCashTypesClass(CashUnits[unitId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                                                                                                                                                 CashUnits[unitId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                                                                                                                                                 CashUnits[unitId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                                                                                                                                                 CashUnits[unitId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                                                                                                                                                 CashUnits[unitId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                                                                                                                                                 CashUnits[unitId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)),
                                                                                                                new XFS4IoT.CashManagement.StorageCashItemTypesClass(CashUnits[unitId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                                                                                                                                                     CashUnits[unitId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                                                                                                                                                     CashUnits[unitId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                                                                                                                                                     CashUnits[unitId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Conterfeit),
                                                                                                                                                                     CashUnits[unitId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                                                                                                                                                     CashUnits[unitId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                                                                                                                                                     CashUnits[unitId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                                                                                                                                                     CashUnits[unitId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)),
                                                                                                                CashUnits[unitId].Unit.Capabilities.HardwareSensors,
                                                                                                                CashUnits[unitId].Unit.Capabilities.RetractAreas,
                                                                                                                CashUnits[unitId].Unit.Capabilities.RetractThresholds,
                                                                                                                capItems),
                                                        new XFS4IoT.CashManagement.StorageCashConfigurationClass(new XFS4IoT.CashManagement.StorageCashTypesClass(CashUnits[unitId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                                                                                                                                                  CashUnits[unitId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                                                                                                                                                  CashUnits[unitId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                                                                                                                                                  CashUnits[unitId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                                                                                                                                                  CashUnits[unitId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                                                                                                                                                  CashUnits[unitId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)),
                                                                                                                new XFS4IoT.CashManagement.StorageCashItemTypesClass(CashUnits[unitId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                                                                                                                                                     CashUnits[unitId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                                                                                                                                                     CashUnits[unitId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                                                                                                                                                     CashUnits[unitId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Conterfeit),
                                                                                                                                                                     CashUnits[unitId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                                                                                                                                                     CashUnits[unitId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                                                                                                                                                     CashUnits[unitId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                                                                                                                                                     CashUnits[unitId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)),
                                                                                                                CashUnits[unitId].Unit.Configuration.Currency,
                                                                                                                CashUnits[unitId].Unit.Configuration.Value,
                                                                                                                CashUnits[unitId].Unit.Configuration.HighThreshold,
                                                                                                                CashUnits[unitId].Unit.Configuration.LowThreshold,
                                                                                                                CashUnits[unitId].Unit.Configuration.AppLockIn,
                                                                                                                CashUnits[unitId].Unit.Configuration.AppLockOut,
                                                                                                                confItems),
                                                        new XFS4IoT.CashManagement.StorageCashStatusClass(CashUnits[unitId].Unit.Status.Index,
                                                                                                          CashUnits[unitId].Unit.Status.InitialCounts.CopyTo(),
                                                                                                          new XFS4IoT.CashManagement.StorageCashOutClass(CashUnits[unitId].Unit.Status.StorageCashOutCount.Presented.CopyTo(),
                                                                                                                                                         CashUnits[unitId].Unit.Status.StorageCashOutCount.Rejected.CopyTo(),
                                                                                                                                                         CashUnits[unitId].Unit.Status.StorageCashOutCount.Distributed.CopyTo(),
                                                                                                                                                         CashUnits[unitId].Unit.Status.StorageCashOutCount.Unknown.CopyTo(),
                                                                                                                                                         CashUnits[unitId].Unit.Status.StorageCashOutCount.Stacked.CopyTo(),
                                                                                                                                                         CashUnits[unitId].Unit.Status.StorageCashOutCount.Diverted.CopyTo(),
                                                                                                                                                         CashUnits[unitId].Unit.Status.StorageCashOutCount.Transport.CopyTo()),
                                                                                                          new XFS4IoT.CashManagement.StorageCashInClass(CashUnits[unitId].Unit.Status.StorageCashInCount.RetractOperations,
                                                                                                                                                        CashUnits[unitId].Unit.Status.StorageCashInCount.Deposited.CopyTo(),
                                                                                                                                                        CashUnits[unitId].Unit.Status.StorageCashInCount.Retracted.CopyTo(),
                                                                                                                                                        CashUnits[unitId].Unit.Status.StorageCashInCount.Rejected.CopyTo(),
                                                                                                                                                        CashUnits[unitId].Unit.Status.StorageCashInCount.Distributed.CopyTo(),
                                                                                                                                                        CashUnits[unitId].Unit.Status.StorageCashInCount.Transport.CopyTo()),
                                                                                                          CashUnits[unitId].Unit.Status.Count,
                                                                                                          CashUnits[unitId].Unit.Status.Accuracy switch
                                                                                                          {
                                                                                                              CashStatusClass.AccuracyEnum.Accurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Accurate,
                                                                                                              CashStatusClass.AccuracyEnum.AccurateSet => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.AccurateSet,
                                                                                                              CashStatusClass.AccuracyEnum.Inaccurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Inaccurate,
                                                                                                              CashStatusClass.AccuracyEnum.NotSupported => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.NotSupported,
                                                                                                              _ => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Unknown,
                                                                                                          },
                                                                                                          CashUnits[unitId].Unit.Status.ReplenishmentStatus switch
                                                                                                          {
                                                                                                              CashStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Empty,
                                                                                                              CashStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Full,
                                                                                                              CashStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Ok,
                                                                                                              CashStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.CashManagement.ReplenishmentStatusEnum.High,
                                                                                                              _ => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Low,
                                                                                                          })),
                                                Card: null);

                thresholdUnits.Add(unitId, payload);
            }

            // Device class must fire threshold event if the count is managed.
            if (thresholdUnits.Count > 0)
            {
                await StorageThresholdEvent(new StorageThresholdEvent.PayloadData(thresholdUnits));
            }

            // Send changed event
            Dictionary<string, StorageUnitClass> statusChangedUnits = new();
            foreach (var unit in CashUnits)
            {
                if (JsonSerializer.Serialize(unit.Value) == 
                    preserved[unit.Key])
                {
                    continue;
                }

                Dictionary<string, XFS4IoT.CashManagement.CashItemClass> capItems = new();
                if (unit.Value.Unit.Capabilities.BanknoteItems is not null)
                {
                    foreach (var item in unit.Value.Unit.Capabilities.BanknoteItems)
                    {
                        capItems.Add(item.Key, new XFS4IoT.CashManagement.CashItemClass(item.Value.NoteId,
                                                                                        item.Value.Currency,
                                                                                        item.Value.Value,
                                                                                        item.Value.Release));
                    }
                }

                Dictionary<string, XFS4IoT.CashManagement.CashItemClass> confItems = new();
                if (unit.Value.Unit.Configuration.BanknoteItems is not null)
                {
                    foreach (var item in unit.Value.Unit.Configuration.BanknoteItems)
                    {
                        confItems.Add(item.Key, new XFS4IoT.CashManagement.CashItemClass(item.Value.NoteId,
                                                                                         item.Value.Currency,
                                                                                         item.Value.Value,
                                                                                         item.Value.Release));
                    }
                }


                StorageUnitClass payload = new(unit.Value.PositionName,
                                               unit.Value.Capacity,
                                               unit.Value.Status switch
                                               {
                                                   CashUnitStorage.StatusEnum.Good => StatusEnum.Ok,
                                                   CashUnitStorage.StatusEnum.Inoperative => StatusEnum.Inoperative,
                                                   CashUnitStorage.StatusEnum.Manipulated => StatusEnum.Manipulated,
                                                   CashUnitStorage.StatusEnum.Missing => StatusEnum.Missing,
                                                   _ => StatusEnum.NotConfigured,
                                               },
                                               unit.Value.SerialNumber,
                                               Cash: new XFS4IoT.CashManagement.StorageCashClass(
                                                   new XFS4IoT.CashManagement.StorageCashCapabilitiesClass(new XFS4IoT.CashManagement.StorageCashTypesClass(unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                                                                                                                                            unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                                                                                                                                            unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                                                                                                                                            unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                                                                                                                                            unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                                                                                                                                            unit.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)),
                                                                                                            new XFS4IoT.CashManagement.StorageCashItemTypesClass(unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                                                                                                                                                 unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                                                                                                                                                 unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                                                                                                                                                 unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Conterfeit),
                                                                                                                                                                 unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                                                                                                                                                 unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                                                                                                                                                 unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                                                                                                                                                 unit.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)),
                                                                                                            unit.Value.Unit.Capabilities.HardwareSensors,
                                                                                                            unit.Value.Unit.Capabilities.RetractAreas,
                                                                                                            unit.Value.Unit.Capabilities.RetractThresholds,
                                                                                                            capItems),
                                                   new XFS4IoT.CashManagement.StorageCashConfigurationClass(new XFS4IoT.CashManagement.StorageCashTypesClass(unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                                                                                                                                             unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                                                                                                                                             unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                                                                                                                                             unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                                                                                                                                             unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                                                                                                                                             unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)
                                                                                                                                                             ),
                                                                                                            new XFS4IoT.CashManagement.StorageCashItemTypesClass(unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                                                                                                                                                 unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                                                                                                                                                 unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                                                                                                                                                 unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Conterfeit),
                                                                                                                                                                 unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                                                                                                                                                 unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                                                                                                                                                 unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                                                                                                                                                 unit.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)
                                                                                                                                                                 ),
                                                                                                            unit.Value.Unit.Configuration.Currency,
                                                                                                            unit.Value.Unit.Configuration.Value,
                                                                                                            unit.Value.Unit.Configuration.HighThreshold,
                                                                                                            unit.Value.Unit.Configuration.LowThreshold,
                                                                                                            unit.Value.Unit.Configuration.AppLockIn,
                                                                                                            unit.Value.Unit.Configuration.AppLockOut,
                                                                                                            confItems),
                                                   new XFS4IoT.CashManagement.StorageCashStatusClass(unit.Value.Unit.Status.Index,
                                                                                                     unit.Value.Unit.Status.InitialCounts.CopyTo(),
                                                                                                     new XFS4IoT.CashManagement.StorageCashOutClass(unit.Value.Unit.Status.StorageCashOutCount.Presented.CopyTo(),
                                                                                                                                                    unit.Value.Unit.Status.StorageCashOutCount.Rejected.CopyTo(),
                                                                                                                                                    unit.Value.Unit.Status.StorageCashOutCount.Distributed.CopyTo(),
                                                                                                                                                    unit.Value.Unit.Status.StorageCashOutCount.Unknown.CopyTo(),
                                                                                                                                                    unit.Value.Unit.Status.StorageCashOutCount.Stacked.CopyTo(),
                                                                                                                                                    unit.Value.Unit.Status.StorageCashOutCount.Diverted.CopyTo(),
                                                                                                                                                    unit.Value.Unit.Status.StorageCashOutCount.Transport.CopyTo()),
                                                                                                     new XFS4IoT.CashManagement.StorageCashInClass(unit.Value.Unit.Status.StorageCashInCount.RetractOperations,
                                                                                                                                                   unit.Value.Unit.Status.StorageCashInCount.Deposited.CopyTo(),
                                                                                                                                                   unit.Value.Unit.Status.StorageCashInCount.Retracted.CopyTo(),
                                                                                                                                                   unit.Value.Unit.Status.StorageCashInCount.Rejected.CopyTo(),
                                                                                                                                                   unit.Value.Unit.Status.StorageCashInCount.Distributed.CopyTo(),
                                                                                                                                                   unit.Value.Unit.Status.StorageCashInCount.Transport.CopyTo()),
                                                                                                     unit.Value.Unit.Status.Count,
                                                                                                     unit.Value.Unit.Status.Accuracy switch 
                                                                                                     {
                                                                                                         CashStatusClass.AccuracyEnum.Accurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Accurate,
                                                                                                         CashStatusClass.AccuracyEnum.AccurateSet => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.AccurateSet,
                                                                                                         CashStatusClass.AccuracyEnum.Inaccurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Inaccurate,
                                                                                                         CashStatusClass.AccuracyEnum.NotSupported => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.NotSupported,
                                                                                                         _ => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Unknown,
                                                                                                     },
                                                                                                     unit.Value.Unit.Status.ReplenishmentStatus switch
                                                                                                     {
                                                                                                         CashStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Empty,
                                                                                                         CashStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Full,
                                                                                                         CashStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Ok,
                                                                                                         CashStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.CashManagement.ReplenishmentStatusEnum.High,
                                                                                                         _ => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Low,
                                                                                                     })),
                                              Card: null);

                statusChangedUnits.Add(unit.Key, payload);
            }

            // Device class must fire threshold event if the count is managed.
            if (statusChangedUnits.Count > 0)
            {
                await StorageChangedEvent(new StorageChangedEvent.PayloadData(statusChangedUnits));
            }

            if (!PersistentData.Store(ServiceProvider.Name + typeof(CashUnit).FullName, CashUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data.");
            }
        }

        /// <summary>
        /// Update delta counts to the destination of the StorageCashCountClass object and return total amount increased or decreased.
        /// </summary>
        private void UpdateDeltaStorageCashCount(StorageCashCountClass storageCashCount, StorageCashCountClass storageDeltaCount)
        {
            if (storageDeltaCount is null)
                return;

            // update counts
            if (storageDeltaCount.Unrecognized >= 0 ||
                (storageDeltaCount.ItemCounts is not null &&
                 storageDeltaCount.ItemCounts.Count > 0))
            {
                storageCashCount.Unrecognized += storageDeltaCount.Unrecognized;
                if (storageDeltaCount.ItemCounts is not null)
                {
                    foreach (var item in storageDeltaCount.ItemCounts)
                    {
                        if (!storageCashCount.ItemCounts.ContainsKey(item.Key))
                        {
                            Logger.Warning(Constants.Framework, $"Unknown banknote item id supplied in the UpdateDeltaStorageCashCount. {item.Key}");
                            continue;
                        }

                        if (item.Value.Counterfeit > 0)
                        {
                            storageCashCount.ItemCounts[item.Key].Counterfeit += item.Value.Counterfeit;
                        }

                        if (item.Value.Fit > 0)
                        {
                            storageCashCount.ItemCounts[item.Key].Fit += item.Value.Fit;
                        }

                        if (item.Value.Inked > 0)
                        {
                            storageCashCount.ItemCounts[item.Key].Inked += item.Value.Inked;
                        }

                        if (item.Value.Suspect > 0)
                        {
                            storageCashCount.ItemCounts[item.Key].Suspect += item.Value.Suspect;
                        }

                        if (item.Value.Unfit > 0)
                        {
                            storageCashCount.ItemCounts[item.Key].Unfit += item.Value.Unfit;
                        }
                    }
                }
            }

            return;
        }

        #endregion

        /// <summary>
        /// Store CardUnits and CashUnits persistently
        /// </summary>
        public void StorePersistent()
        {
            if (!PersistentData.Store(ServiceProvider.Name + typeof(CashUnit).FullName, CashUnits))
            {
                Logger.Warning(Constants.Framework, "Failed to save persistent data.");
            }
        }

        /// <summary>
        /// Persistent data storage access
        /// </summary>
        private readonly IPersistentData PersistentData;

        /// <summary>
        /// Type of storage
        /// </summary>
        public StorageTypeEnum StorageType { get; set; }

        /// <summary>
        /// Card storage structure information of this device
        /// </summary>
        public Dictionary<string, CardUnitStorage> CardUnits { get; set; }

        /// <summary>
        /// Cash storage structure information of this device
        /// </summary>
        public Dictionary<string, CashUnitStorage> CashUnits { get; set; }
    }
}
