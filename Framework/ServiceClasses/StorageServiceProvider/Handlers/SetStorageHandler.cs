/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using XFS4IoT.Storage.Commands;
using XFS4IoT.Storage.Completions;
using XFS4IoT;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Storage
{
    public partial class SetStorageHandler
    {
        private async Task<CommandResult<SetStorageCompletion.PayloadData>> HandleSetStorage(ISetStorageEvents events, SetStorageCommand setStorage, CancellationToken cancel)
        {
            MessageHeader.CompletionCodeEnum completionCode = MessageHeader.CompletionCodeEnum.InternalError;
            string errorDescription = string.Empty;
            SetStorageCompletion.PayloadData.ErrorCodeEnum? errorCode = null;

            if (setStorage.Payload.Storage is null ||
                setStorage.Payload.Storage.Count == 0)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No storage information is specified.");
            }

            if (Storage.StorageType.HasFlag(StorageTypeEnum.Card))
            {
                Dictionary<string, SetCardUnitStorage> cardStorageToSet = new();

                foreach (var storage in setStorage.Payload.Storage)
                {
                    if (!Storage.CardUnits.ContainsKey(storage.Key))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"Unrecognised storage ID for card unit specified. {storage.Key}");
                    }

                    if (storage.Value.Card is null ||
                        (storage.Value.Card.Configuration is null ||
                         (string.IsNullOrEmpty(storage.Value.Card.Configuration.CardID) &&
                          storage.Value.Card.Configuration.Threshold is null)) &&
                        (storage.Value.Card.Status is null ||
                         storage.Value.Card.Status.InitialCount is null))
                    {
                        // No configuration or count specified and just ingore this unit
                        continue;
                    }

                    if (storage.Value.Card.Configuration?.Threshold is not null &&
                        (storage.Value.Card.Configuration?.Threshold >= Storage.CardUnits[storage.Key].Capacity ||
                         storage.Value.Card.Configuration?.Threshold < 0))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"Invalid threshold specified for card unit. The value must be smaller than capacity or positive value. {storage.Value.Card.Configuration.Threshold}");
                    }

                    cardStorageToSet.Add(storage.Key, new SetCardUnitStorage(storage.Value.Card.Configuration is null ? null : new SetCardConfiguration(storage.Value.Card.Configuration.Threshold,
                                                                                                                                                        storage.Value.Card.Configuration.CardID),
                                                                             storage.Value.Card.Status.InitialCount));
                }

                if (cardStorageToSet.Count == 0)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"No card configuration is set to all card units.");
                }

                Logger.Log(Constants.DeviceClass, "StorageDev.SetCardStorageAsync()");

                var result = await Device.SetCardStorageAsync(new(cardStorageToSet), cancel);

                Logger.Log(Constants.DeviceClass, $"StorageDev.SetCardStorageAsync() -> {result.CompletionCode}, {result.ErrorCode}");

                if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
                {
                    foreach (var storageToUpdate in cardStorageToSet)
                    {
                        Storage.CardUnits.ContainsKey(storageToUpdate.Key).IsTrue($"Unexpected storage ID for card unit found. {storageToUpdate.Key}");

                        string preservedStorage = JsonSerializer.Serialize(Storage.CardUnits[storageToUpdate.Key]);

                        if (storageToUpdate.Value.Configuration is not null)
                        {
                            Storage.CardUnits[storageToUpdate.Key].Unit.Configuration.CardId = storageToUpdate.Value.Configuration.CardId;

                            if (storageToUpdate.Value.Configuration.Threshold is not null)
                            {
                                Storage.CardUnits[storageToUpdate.Key].Unit.Configuration.Threshold = (int)storageToUpdate.Value.Configuration.Threshold;
                            }
                        }

                        if (storageToUpdate.Value.InitialCount is not null)
                        {
                            Storage.CardUnits[storageToUpdate.Key].Unit.Status.InitialCount = (int)storageToUpdate.Value.InitialCount;
                            Storage.CardUnits[storageToUpdate.Key].Unit.Status.Count = (int)storageToUpdate.Value.InitialCount;
                            Storage.CardUnits[storageToUpdate.Key].Unit.Status.RetainCount = 0;
                        }

                        await Storage.UpdateCardStorageCount(storageToUpdate.Key, 0, preservedStorage);
                    }
                }

                completionCode = result.CompletionCode;
                errorDescription = result.ErrorDescription;
                errorCode = result.ErrorCode;
            }
            else
            {
                if (Storage.StorageType.HasFlag(StorageTypeEnum.Cash))
                {
                    Dictionary<string, string> preserved = [];
                    foreach (var unit in Storage.CashUnits)
                    {
                        preserved.Add(unit.Key, JsonSerializer.Serialize(unit.Value));
                    }

                    Dictionary<string, SetCashUnitStorage> cashStorageToSet = [];
                    foreach (var storage in setStorage.Payload.Storage)
                    {
                        if (!Storage.CashUnits.ContainsKey(storage.Key))
                        {
                            return new(
                                MessageHeader.CompletionCodeEnum.InvalidData,
                                $"Unrecognised storage ID for cash unit specified. {storage.Key}");
                        }

                        if (storage.Value.Cash is null ||
                            (storage.Value.Cash.Configuration is null ||
                             (string.IsNullOrEmpty(storage.Value.Cash.Configuration?.Currency) &&
                              storage.Value.Cash.Configuration?.AppLockIn is null &&
                              storage.Value.Cash.Configuration?.AppLockOut is null &&
                              (storage.Value.Cash.Configuration?.CashItems is null ||
                               storage.Value.Cash.Configuration?.CashItems.Count == 0) &&
                              storage.Value.Cash.Configuration?.HighThreshold is null &&
                              storage.Value.Cash.Configuration?.LowThreshold is null &&
                              storage.Value.Cash.Configuration?.MaxRetracts is null &&
                              string.IsNullOrEmpty(storage.Value.Cash.Configuration?.Name))) &&
                            (storage.Value.Cash.Status is null ||
                             (storage.Value.Cash.Status.Initial is null ||
                              storage.Value.Cash.Status.Initial?.ExtendedProperties is null ||
                              storage.Value.Cash.Status.Initial?.ExtendedProperties?.Count == 0)))
                        {
                            // No configuration or count specified and just ingore this unit
                            continue;
                        }

                        SetCashConfiguration newConfig = null;
                        if (storage.Value.Cash.Configuration is not null)
                        {
                            if (storage.Value.Cash.Configuration.HighThreshold is not null &&
                            (storage.Value.Cash.Configuration.HighThreshold >= Storage.CashUnits[storage.Key].Capacity ||
                             storage.Value.Cash.Configuration.HighThreshold < 0))
                            {
                                return new(
                                    MessageHeader.CompletionCodeEnum.InvalidData,
                                    $"Invalid threshold specified for cash unit. The value must be smaller than capacity or positive value. {storage.Value.Cash.Configuration.HighThreshold}");
                            }

                            if (storage.Value.Cash.Configuration.LowThreshold is not null &&
                                storage.Value.Cash.Configuration.LowThreshold < 0)
                            {
                                return new(
                                    MessageHeader.CompletionCodeEnum.InvalidData,
                                    $"Invalid low threshold specified for cash unit. The value must be positive value. {storage.Value.Cash.Configuration.LowThreshold}");
                            }

                            if (storage.Value.Cash.Configuration.MaxRetracts is not null &&
                                storage.Value.Cash.Configuration.MaxRetracts < 0)
                            {
                                return new(
                                    MessageHeader.CompletionCodeEnum.InvalidData,
                                    $"Invalid max retracts specified for cash unit. The value must be positive value. {storage.Value.Cash.Configuration.MaxRetracts}");
                            }

                            CashCapabilitiesClass.TypesEnum? types = null;
                            if (storage.Value.Cash.Configuration.Types?.CashIn is not null && (bool)storage.Value.Cash.Configuration.Types?.CashIn)
                                types |= CashCapabilitiesClass.TypesEnum.CashIn;
                            if (storage.Value.Cash.Configuration.Types?.CashInRetract is not null && (bool)storage.Value.Cash.Configuration.Types?.CashInRetract)
                                types |= CashCapabilitiesClass.TypesEnum.CashInRetract;
                            if (storage.Value.Cash.Configuration.Types?.CashOut is not null && (bool)storage.Value.Cash.Configuration.Types?.CashOut)
                                types |= CashCapabilitiesClass.TypesEnum.CashOut;
                            if (storage.Value.Cash.Configuration.Types?.CashOutRetract is not null && (bool)storage.Value.Cash.Configuration.Types?.CashOutRetract)
                                types |= CashCapabilitiesClass.TypesEnum.CashOutRetract;
                            if (storage.Value.Cash.Configuration.Types?.Reject is not null && (bool)storage.Value.Cash.Configuration.Types?.Reject)
                                types |= CashCapabilitiesClass.TypesEnum.Reject;
                            if (storage.Value.Cash.Configuration.Types?.Replenishment is not null && (bool)storage.Value.Cash.Configuration.Types?.Replenishment)
                                types |= CashCapabilitiesClass.TypesEnum.Replenishment;

                            CashCapabilitiesClass.ItemsEnum? items = null;
                            if (storage.Value.Cash.Configuration.Items?.Counterfeit is not null && (bool)storage.Value.Cash.Configuration.Items?.Counterfeit)
                                items |= CashCapabilitiesClass.ItemsEnum.Counterfeit;
                            if (storage.Value.Cash.Configuration.Items?.Coupon is not null && (bool)storage.Value.Cash.Configuration.Items?.Coupon)
                                items |= CashCapabilitiesClass.ItemsEnum.Coupon;
                            if (storage.Value.Cash.Configuration.Items?.Document is not null && (bool)storage.Value.Cash.Configuration.Items?.Document)
                                items |= CashCapabilitiesClass.ItemsEnum.Document;
                            if (storage.Value.Cash.Configuration.Items?.Fit is not null && (bool)storage.Value.Cash.Configuration.Items?.Fit)
                                items |= CashCapabilitiesClass.ItemsEnum.Fit;
                            if (storage.Value.Cash.Configuration.Items?.Inked is not null && (bool)storage.Value.Cash.Configuration.Items?.Inked)
                                items |= CashCapabilitiesClass.ItemsEnum.Inked;
                            if (storage.Value.Cash.Configuration.Items?.Suspect is not null && (bool)storage.Value.Cash.Configuration.Items?.Suspect)
                                items |= CashCapabilitiesClass.ItemsEnum.Suspect;
                            if (storage.Value.Cash.Configuration.Items?.Unfit is not null && (bool)storage.Value.Cash.Configuration.Items?.Unfit)
                                items |= CashCapabilitiesClass.ItemsEnum.Unfit;
                            if (storage.Value.Cash.Configuration.Items?.Unrecognized is not null && (bool)storage.Value.Cash.Configuration.Items?.Unrecognized)
                                items |= CashCapabilitiesClass.ItemsEnum.Unrecognized;

                            if (storage.Value.Cash.Configuration.CashItems?.Count > 0)
                            {
                                foreach (var item in from item in storage.Value.Cash.Configuration.CashItems
                                                     where !Common.CashManagementCapabilities.AllBanknoteItems.ContainsKey(item)
                                                     select item)
                                {
                                    return new(
                                        MessageHeader.CompletionCodeEnum.InvalidData,
                                        $"Invalid banknote item specified. Unit: {storage.Key}, Invalid item: {item}");
                                }
                            }

                            newConfig = new SetCashConfiguration(types,
                                                                 items,
                                                                 storage.Value.Cash.Configuration.Currency,
                                                                 storage.Value.Cash.Configuration.Value,
                                                                 storage.Value.Cash.Configuration.HighThreshold,
                                                                 storage.Value.Cash.Configuration.LowThreshold,
                                                                 storage.Value.Cash.Configuration.AppLockIn,
                                                                 storage.Value.Cash.Configuration.AppLockOut,
                                                                 storage.Value.Cash.Configuration.CashItems,
                                                                 storage.Value.Cash.Configuration.Name,
                                                                 storage.Value.Cash.Configuration.MaxRetracts);
                        }

                        StorageCashCountClass newInitialCounts = null;
                        if (storage.Value.Cash.Status?.Initial is not null)
                        {
                            Dictionary<string, CashItemCountClass> itemCounts = null;
                            if (storage.Value.Cash.Status.Initial?.ExtendedProperties?.Count > 0)
                            {
                                itemCounts = [];
                                foreach (var item in storage.Value.Cash.Status.Initial.ExtendedProperties)
                                {
                                    if (storage.Value.Cash.Configuration?.CashItems is not null)
                                    {
                                        if (!storage.Value.Cash.Configuration.CashItems.Contains(item.Key))
                                        {
                                            return new(
                                                MessageHeader.CompletionCodeEnum.InvalidData,
                                                $"Invalid banknote item specified for an initial counts. Unit: {storage.Key}, Invalid item: {item.Key}");
                                        }
                                    }

                                    if (Storage.CashUnits[storage.Key].Unit.Status.InitialCounts?.ItemCounts?.Count > 0)
                                    {
                                        // Initial count is setup already
                                        if (!Storage.CashUnits[storage.Key].Unit.Status.InitialCounts.ItemCounts.ContainsKey(item.Key))
                                        {
                                            return new(
                                                MessageHeader.CompletionCodeEnum.InvalidData,
                                                $"Invalid banknote item specified for an initial counts. Unit: {storage.Key}, Invalid item: {item.Key}");
                                        }
                                    }
                                    else
                                    {
                                        if (Storage.CashUnits[storage.Key].Unit.Configuration.BanknoteItems is null ||
                                            Storage.CashUnits[storage.Key].Unit.Configuration.BanknoteItems.Count == 0)
                                        {
                                            // New banknote types should be assigned to this unit
                                            if (storage.Value.Cash.Configuration?.CashItems is not null &&
                                                !storage.Value.Cash.Configuration.CashItems.Contains(item.Key))
                                            {
                                                return new(
                                                    MessageHeader.CompletionCodeEnum.InvalidData,
                                                    $"Invalid banknote item specified for an initial counts. Unit: {storage.Key}, Invalid item: {item.Key}");
                                            }
                                        }
                                        else
                                        {
                                            // No initial counts are set in an internal structure and check denomination assiged to this unit
                                            if (!Storage.CashUnits[storage.Key].Unit.Configuration.BanknoteItems.Contains(item.Key))
                                            {
                                                return new(
                                                    MessageHeader.CompletionCodeEnum.InvalidData,
                                                    $"Invalid banknote item specified for an initial counts. Unit: {storage.Key}, Invalid item: {item.Key}");
                                            }
                                        }
                                    }

                                    itemCounts.Add(item.Key, new CashItemCountClass(item.Value.Fit is null ? 0 : (int)item.Value.Fit,
                                                                                    item.Value.Unfit is null ? 0 : (int)item.Value.Unfit,
                                                                                    item.Value.Suspect is null ? 0 : (int)item.Value.Suspect,
                                                                                    item.Value.Counterfeit is null ? 0 : (int)item.Value.Counterfeit,
                                                                                    item.Value.Inked is null ? 0 : (int)item.Value.Inked));
                                }
                            }
                            newInitialCounts = new StorageCashCountClass(storage.Value.Cash.Status.Initial.Unrecognized is null ? 0 : (int)storage.Value.Cash.Status.Initial.Unrecognized,
                                                                         itemCounts);
                        }

                        cashStorageToSet.Add(storage.Key, new SetCashUnitStorage(newConfig, newInitialCounts));
                    }

                    Logger.Log(Constants.DeviceClass, "StorageDev.SetCashStorageAsync()");

                    var result = await Device.SetCashStorageAsync(new(cashStorageToSet), cancel);

                    Logger.Log(Constants.DeviceClass, $"StorageDev.SetCashStorageAsync() -> {result.CompletionCode}, {result.ErrorCode}");

                    if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
                    {
                        foreach (var storageToUpdate in cashStorageToSet)
                        {
                            Storage.CashUnits.ContainsKey(storageToUpdate.Key).IsTrue($"Unexpected storage ID for cash unit found. {storageToUpdate.Key}");

                            if (storageToUpdate.Value.Configuration is not null)
                            {
                                if (storageToUpdate.Value.Configuration.AppLockIn is not null)
                                {
                                    Storage.CashUnits[storageToUpdate.Key].Unit.Configuration.AppLockIn = (bool)storageToUpdate.Value.Configuration.AppLockIn;
                                }
                                if (storageToUpdate.Value.Configuration.AppLockOut is not null)
                                {
                                    Storage.CashUnits[storageToUpdate.Key].Unit.Configuration.AppLockOut = (bool)storageToUpdate.Value.Configuration.AppLockOut;
                                }
                                if (storageToUpdate.Value.Configuration.BanknoteItems is not null &&
                                    storageToUpdate.Value.Configuration.BanknoteItems.Count > 0)
                                {
                                    Storage.CashUnits[storageToUpdate.Key].Unit.Configuration.BanknoteItems = storageToUpdate.Value.Configuration.BanknoteItems;
                                }
                                if (string.IsNullOrEmpty(storageToUpdate.Value.Configuration.Currency))
                                {
                                    Storage.CashUnits[storageToUpdate.Key].Unit.Configuration.Currency = storageToUpdate.Value.Configuration.Currency;
                                }
                                if (storageToUpdate.Value.Configuration.HighThreshold is not null)
                                {
                                    Storage.CashUnits[storageToUpdate.Key].Unit.Configuration.HighThreshold = (int)storageToUpdate.Value.Configuration.HighThreshold;
                                }
                                if (storageToUpdate.Value.Configuration.LowThreshold is not null)
                                {
                                    Storage.CashUnits[storageToUpdate.Key].Unit.Configuration.LowThreshold = (int)storageToUpdate.Value.Configuration.LowThreshold;
                                }
                                if (storageToUpdate.Value.Configuration.Types is not null)
                                {
                                    Storage.CashUnits[storageToUpdate.Key].Unit.Configuration.Types = (CashCapabilitiesClass.TypesEnum)storageToUpdate.Value.Configuration.Types;
                                }
                                if (storageToUpdate.Value.Configuration.Items is not null)
                                {
                                    Storage.CashUnits[storageToUpdate.Key].Unit.Configuration.Items = (CashCapabilitiesClass.ItemsEnum)storageToUpdate.Value.Configuration.Items;
                                }
                                if (storageToUpdate.Value.Configuration.Value is not null)
                                {
                                    Storage.CashUnits[storageToUpdate.Key].Unit.Configuration.Value = (int)storageToUpdate.Value.Configuration.Value;
                                }
                            }

                            if (storageToUpdate.Value.InitialCounts is not null)
                            {
                                Storage.CashUnits[storageToUpdate.Key].Unit.Status.InitialCounts.Unrecognized = storageToUpdate.Value.InitialCounts.Unrecognized;
                                if (storageToUpdate.Value.InitialCounts.ItemCounts?.Count > 0)
                                {
                                    Storage.CashUnits[storageToUpdate.Key].Unit.Status.InitialCounts.ItemCounts = storageToUpdate.Value.InitialCounts.ItemCounts;
                                    Storage.CashUnits[storageToUpdate.Key].Unit.Status.Count = Storage.CashUnits[storageToUpdate.Key].Unit.Status.InitialCounts.Total;
                                    Storage.CashUnits[storageToUpdate.Key].Unit.Status.StorageCashInCount = new();
                                    Storage.CashUnits[storageToUpdate.Key].Unit.Status.StorageCashOutCount = new();
                                }
                            }
                        }

                        await Storage.UpdateCashAccounting(preservedStorage: preserved);
                    }
                    completionCode = result.CompletionCode;
                    errorDescription = result.ErrorDescription;
                    errorCode = result.ErrorCode;
                }

                if (Storage.StorageType.HasFlag(StorageTypeEnum.Check))
                {
                    Dictionary<string, string> preserved = [];
                    foreach (var unit in Storage.CheckUnits)
                    {
                        preserved.Add(unit.Key, JsonSerializer.Serialize(unit.Value));
                    }

                    Dictionary<string, SetCheckUnitStorage> checkStorageToSet = [];
                    foreach (var storage in setStorage.Payload.Storage)
                    {
                        if (!Storage.CheckUnits.ContainsKey(storage.Key))
                        {
                            return new(
                                MessageHeader.CompletionCodeEnum.InvalidData,
                                $"Unrecognised storage ID for check unit specified. {storage.Key}");
                        }

                        if (storage.Value.Check is null &&
                            (storage.Value.Check.Configuration is null ||
                             (storage.Value.Check.Configuration?.Types is null &&
                              string.IsNullOrEmpty(storage.Value.Check.Configuration?.BinID) &&
                              storage.Value.Check.Configuration?.HighThreshold is null &&
                              storage.Value.Check.Configuration?.RetractHighThreshold is null)) &&
                            (storage.Value.Check.Status is null ||
                             storage.Value.Cash.Status?.Initial is null))
                        {
                            // No configuration or count specified and just ingore this unit
                            continue;
                        }

                        SetCheckConfiguration newConfig = null;
                        if (storage.Value.Check.Configuration is not null)
                        {
                            if (storage.Value.Check.Configuration.HighThreshold is not null &&
                                (storage.Value.Check.Configuration.HighThreshold >= Storage.CashUnits[storage.Key].Capacity ||
                                 storage.Value.Check.Configuration.HighThreshold <= 0))
                            {
                                return new(
                                    MessageHeader.CompletionCodeEnum.InvalidData,
                                    $"Invalid threshold specified for check unit. The value must be smaller than capacity or positive value. {storage.Value.Cash.Configuration.HighThreshold}");
                            }

                            if (storage.Value.Check.Configuration.RetractHighThreshold is not null &&
                                storage.Value.Check.Configuration.RetractHighThreshold <= 0)
                            {
                                return new(
                                    MessageHeader.CompletionCodeEnum.InvalidData,
                                    $"Invalid retract high threshold specified for check unit. The value must be positive value. {storage.Value.Cash.Configuration.MaxRetracts}");
                            }

                            CheckCapabilitiesClass.TypesEnum? types = null;
                            if (storage.Value.Check.Configuration.Types?.MediaIn is not null && (bool)storage.Value.Check.Configuration.Types.MediaIn)
                                types |= CheckCapabilitiesClass.TypesEnum.MediaIn;
                            if (storage.Value.Check.Configuration.Types?.Retract is not null && (bool)storage.Value.Check.Configuration.Types.Retract)
                                types |= CheckCapabilitiesClass.TypesEnum.Retract;

                            newConfig = new SetCheckConfiguration(
                                Types: types,
                                Id: storage.Value.Check.Configuration.BinID,
                                HighThreshold: storage.Value.Check.Configuration.HighThreshold,
                                RetractHighThreshold: storage.Value.Check.Configuration.RetractHighThreshold);
                        }

                        checkStorageToSet.Add(
                            storage.Key,
                            new(
                                Configuration: newConfig,
                                MediaInCount: storage.Value.Check.Status?.Initial?.MediaInCount,
                                Count: storage.Value.Check.Status?.Initial?.Count,
                                RetractOperations: storage.Value.Check.Status?.Initial?.RetractOperations)
                            );
                    }

                    Logger.Log(Constants.DeviceClass, "StorageDev.SetCheckStorageAsync()");

                    var result = await Device.SetCheckStorageAsync(new(checkStorageToSet), cancel);

                    Logger.Log(Constants.DeviceClass, $"StorageDev.SetCheckStorageAsync() -> {result.CompletionCode}, {result.ErrorCode}");

                    if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
                    {
                        foreach (var storageToUpdate in checkStorageToSet)
                        {
                            Storage.CheckUnits.ContainsKey(storageToUpdate.Key).IsTrue($"Unexpected storage ID for cash unit found. {storageToUpdate.Key}");

                            if (storageToUpdate.Value.Configuration is not null)
                            {
                                if (storageToUpdate.Value.Configuration.Types is not null)
                                {
                                    Storage.CheckUnits[storageToUpdate.Key].Unit.Configuration.Types = (CheckCapabilitiesClass.TypesEnum)storageToUpdate.Value.Configuration.Types;
                                }
                                if (string.IsNullOrEmpty(storageToUpdate.Value.Configuration.Id))
                                {
                                    Storage.CheckUnits[storageToUpdate.Key].Unit.Configuration.Id = storageToUpdate.Value.Configuration.Id;
                                }
                                if (storageToUpdate.Value.Configuration.HighThreshold is not null)
                                {
                                    Storage.CheckUnits[storageToUpdate.Key].Unit.Configuration.HighThreshold = (int)storageToUpdate.Value.Configuration.HighThreshold;
                                }
                                if (storageToUpdate.Value.Configuration.RetractHighThreshold is not null)
                                {
                                    Storage.CheckUnits[storageToUpdate.Key].Unit.Configuration.RetractHighThreshold = (int)storageToUpdate.Value.Configuration.RetractHighThreshold;
                                }
                            }

                            if (storageToUpdate.Value.MediaInCount is not null)
                            {
                                Storage.CheckUnits[storageToUpdate.Key].Unit.Status.InitialCounts.MediaInCount = (int)storageToUpdate.Value.MediaInCount;
                                Storage.CheckUnits[storageToUpdate.Key].Unit.Status.CheckInCounts.MediaInCount = Storage.CheckUnits[storageToUpdate.Key].Unit.Status.InitialCounts.MediaInCount;
                            }
                            if (storageToUpdate.Value.Count is not null)
                            {
                                Storage.CheckUnits[storageToUpdate.Key].Unit.Status.InitialCounts.Count = (int)storageToUpdate.Value.Count;
                                Storage.CheckUnits[storageToUpdate.Key].Unit.Status.CheckInCounts.Count = Storage.CheckUnits[storageToUpdate.Key].Unit.Status.InitialCounts.Count;
                            }
                            if (storageToUpdate.Value.RetractOperations is not null)
                            {
                                Storage.CheckUnits[storageToUpdate.Key].Unit.Status.InitialCounts.RetractOperations = (int)storageToUpdate.Value.RetractOperations;
                                Storage.CheckUnits[storageToUpdate.Key].Unit.Status.CheckInCounts.RetractOperations = Storage.CheckUnits[storageToUpdate.Key].Unit.Status.InitialCounts.RetractOperations;
                            }
                        }

                        await Storage.UpdateCheckStorageCount(preservedStorage: preserved);

                    }
                    completionCode = result.CompletionCode;
                    errorDescription = result.ErrorDescription;
                    errorCode = result.ErrorCode;
                }
            }

            // Keep updated storage information on the hard disk
            Storage.StorePersistent();

            return new(
                errorCode is not null ? new(errorCode) : null,
                completionCode, 
                errorDescription);
        }
    }
}
