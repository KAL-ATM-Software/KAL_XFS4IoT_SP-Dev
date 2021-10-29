/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
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
        private async Task<SetStorageCompletion.PayloadData> HandleSetStorage(ISetStorageEvents events, SetStorageCommand setStorage, CancellationToken cancel)
        {
            MessagePayload.CompletionCodeEnum completionCode = MessagePayload.CompletionCodeEnum.InternalError;
            string errorDescription;
            SetStorageCompletion.PayloadData.ErrorCodeEnum? errorCode;

            if (setStorage.Payload.Storage is null ||
                setStorage.Payload.Storage.Count == 0)
            {
                return new SetStorageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                            $"No storage information is specified.");
            }

            if (Storage.StorageType == StorageTypeEnum.Card)
            {
                Dictionary<string, SetCardUnitStorage> cardStorageToSet = new();

                foreach (var storage in setStorage.Payload.Storage)
                {
                    if (!Storage.CardUnits.ContainsKey(storage.Key))
                    {
                        return new SetStorageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
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
                        return new SetStorageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                    $"Invalid threshold specified for card unit. The value must be smaller than capacity or positive value. {storage.Value.Card.Configuration.Threshold}");
                    }

                    cardStorageToSet.Add(storage.Key, new SetCardUnitStorage(storage.Value.Card.Configuration is null ? null : new SetCardConfiguration(storage.Value.Card.Configuration.Threshold,
                                                                                                                                                        storage.Value.Card.Configuration.CardID),
                                                                             storage.Value.Card.Status.InitialCount));
                }

                if (cardStorageToSet.Count == 0)
                {
                    return new SetStorageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                $"No card configuration is set to all card units.");
                }

                Logger.Log(Constants.DeviceClass, "StorageDev.SetCardStorageAsync()");

                var result = await Device.SetCardStorageAsync(new(cardStorageToSet), cancel);
                
                Logger.Log(Constants.DeviceClass, $"StorageDev.SetCardStorageAsync() -> {result.CompletionCode}, {result.ErrorCode}");

                if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
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
                Dictionary<string, string> preserved = new();
                foreach (var unit in Storage.CashUnits)
                {
                    preserved.Add(unit.Key, JsonSerializer.Serialize(unit.Value));
                }

                Dictionary<string, SetCashUnitStorage> cashStorageToSet = new();
                foreach (var storage in setStorage.Payload.Storage)
                {
                    if (!Storage.CashUnits.ContainsKey(storage.Key))
                    {
                        return new SetStorageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                    $"Unrecognised storage ID for cash unit specified. {storage.Key}");
                    }

                    if (storage.Value.Cash is null ||
                        (storage.Value.Cash.Configuration is null ||
                         (string.IsNullOrEmpty(storage.Value.Cash.Configuration.Currency) &&
                          storage.Value.Cash.Configuration.AppLockIn is null &&
                          storage.Value.Cash.Configuration.AppLockOut is null &&
                          (storage.Value.Cash.Configuration.CashItems is null ||
                           storage.Value.Cash.Configuration.CashItems.Count == 0) &&
                          storage.Value.Cash.Configuration.HighThreshold is null &&
                          storage.Value.Cash.Configuration.LowThreshold is null &&
                          storage.Value.Cash.Configuration.MaxRetracts is null &&
                          string.IsNullOrEmpty(storage.Value.Cash.Configuration.Name))) &&
                        (storage.Value.Cash.Status is null ||
                         (storage.Value.Cash.Status.Initial is null ||
                          storage.Value.Cash.Status.Initial.Cash is null ||
                          storage.Value.Cash.Status.Initial.Cash.Count == 0)))
                    {
                        // No configuration or count specified and just ingore this unit
                        continue;
                    }

                    if (storage.Value.Cash.Configuration?.HighThreshold is not null &&
                        (storage.Value.Cash.Configuration?.HighThreshold >= Storage.CashUnits[storage.Key].Capacity ||
                         storage.Value.Cash.Configuration?.HighThreshold < 0))
                    {
                        return new SetStorageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                    $"Invalid threshold specified for cash unit. The value must be smaller than capacity or positive value. {storage.Value.Cash.Configuration.HighThreshold}");
                    }

                    if (storage.Value.Cash.Configuration?.LowThreshold is not null &&
                        storage.Value.Cash.Configuration?.LowThreshold < 0)
                    {
                        return new SetStorageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                    $"Invalid low threshold specified for cash unit. The value must be positive value. {storage.Value.Cash.Configuration.LowThreshold}");
                    }

                    if (storage.Value.Cash.Configuration?.MaxRetracts is not null &&
                        storage.Value.Cash.Configuration?.MaxRetracts < 0)
                    {
                        return new SetStorageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                    $"Invalid max retracts specified for cash unit. The value must be positive value. {storage.Value.Cash.Configuration.MaxRetracts}");
                    }

                    SetCashConfiguration newConfig = null;
                    if (storage.Value.Cash.Configuration is not null)
                    {
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
                            items |= CashCapabilitiesClass.ItemsEnum.Conterfeit;
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

                        Dictionary<string, BanknoteItem> banknoteItems = null;
                        if (storage.Value.Cash.Configuration.CashItems is not null &&
                            storage.Value.Cash.Configuration.CashItems.Count > 0)
                        {
                            banknoteItems = new();
                            foreach (var item in storage.Value.Cash.Configuration.CashItems)
                            {
                                if (item.Value.NoteID is null)
                                {
                                    return new SetStorageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                $"No note ID is supplied in the CashItems. {item.Key}");
                                }
                                if (item.Value.Value is null)
                                {
                                    return new SetStorageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                $"No value is supplied in the CashItems. {item.Key}");
                                }
                                if (item.Value.Release is null)
                                {
                                    return new SetStorageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                $"No release is supplied in the CashItems. {item.Key}");
                                }

                                banknoteItems.Add(item.Key, new BanknoteItem((int)item.Value.NoteID,
                                                                             item.Value.Currency,
                                                                             (double)item.Value.Value,
                                                                             (int)item.Value.Release));
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
                                                             banknoteItems,
                                                             storage.Value.Cash.Configuration.Name,
                                                             storage.Value.Cash.Configuration.MaxRetracts);
                    }

                    StorageCashCountClass newInitialCounts = null;
                    if (storage.Value.Cash.Status is not null &&
                        storage.Value.Cash.Status.Initial is not null)
                    {
                        Dictionary<string, CashItemCountClass> itemCounts = null;
                        if (storage.Value.Cash.Status.Initial.Cash is not null &&
                            storage.Value.Cash.Status.Initial.Cash.Count > 0)
                        {
                            itemCounts = new();
                            foreach (var item in storage.Value.Cash.Status.Initial.Cash)
                            {
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

                if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
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
                            if (storageToUpdate.Value.InitialCounts.ItemCounts is not null &&
                                storageToUpdate.Value.InitialCounts.ItemCounts.Count > 0)
                            {
                                Storage.CashUnits[storageToUpdate.Key].Unit.Status.InitialCounts.ItemCounts = storageToUpdate.Value.InitialCounts.ItemCounts;
                                Storage.CashUnits[storageToUpdate.Key].Unit.Status.Count = Storage.CashUnits[storageToUpdate.Key].Unit.Status.InitialCounts.Total;
                                Storage.CashUnits[storageToUpdate.Key].Unit.Status.StorageCashInCount = new StorageCashInCountClass();
                                Storage.CashUnits[storageToUpdate.Key].Unit.Status.StorageCashOutCount = new StorageCashOutCountClass();
                            }
                        }
                    }

                    await Storage.UpdateCashAccounting(preservedStorage: preserved);
                }

                completionCode = result.CompletionCode;
                errorDescription = result.ErrorDescription;
                errorCode = result.ErrorCode;
            }

            // Keep updated storage information on the hard disk
            Storage.StorePersistent();

            return new SetStorageCompletion.PayloadData(completionCode, errorDescription, errorCode);
        }
    }
}
