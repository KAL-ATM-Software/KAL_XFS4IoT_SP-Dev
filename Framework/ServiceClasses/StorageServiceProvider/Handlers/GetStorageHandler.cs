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
using XFS4IoT.Storage.Commands;
using XFS4IoT.Storage.Completions;
using XFS4IoT.Storage;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Storage
{
    public partial class GetStorageHandler
    { 
        private Task<GetStorageCompletion.PayloadData> HandleGetStorage(IGetStorageEvents events, GetStorageCommand getStorage, CancellationToken cancel)
        {
            Dictionary<string, StorageUnitClass> storageResponse = new();
            if (Storage.StorageType == StorageTypeEnum.Card)
            {
                foreach (var storage in Storage.CardUnits)
                {
                    StorageUnitClass thisStorage = new(storage.Value.PositionName,
                                                       storage.Value.Capacity,
                                                       storage.Value.Status switch
                                                       {
                                                           CardUnitStorage.StatusEnum.Good => StatusEnum.Ok,
                                                           CardUnitStorage.StatusEnum.Inoperative => StatusEnum.Inoperative,
                                                           CardUnitStorage.StatusEnum.Manipulated => StatusEnum.Manipulated,
                                                           CardUnitStorage.StatusEnum.Missing => StatusEnum.Missing,
                                                           _ => StatusEnum.NotConfigured,
                                                       },
                                                       storage.Value.SerialNumber,
                                                       Cash: null,
                                                       Card: new XFS4IoT.CardReader.StorageClass(new XFS4IoT.CardReader.StorageCapabilitiesClass(storage.Value.Unit.Capabilities.Type switch
                                                                                                                                                 {
                                                                                                                                                     CardCapabilitiesClass.TypeEnum.Dispense => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Dispense,
                                                                                                                                                     CardCapabilitiesClass.TypeEnum.Retain => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Retain,
                                                                                                                                                     _ => XFS4IoT.CardReader.StorageCapabilitiesClass.TypeEnum.Park,
                                                                                                                                                 },
                                                                                                                                                 storage.Value.Unit.Capabilities.HardwareSensors),
                                                                                                 new XFS4IoT.CardReader.StorageConfigurationClass(storage.Value.Unit.Configuration.CardId,
                                                                                                                                                  storage.Value.Unit.Configuration.Threshold),
                                                                                                 new XFS4IoT.CardReader.StorageStatusClass(storage.Value.Unit.Status.InitialCount,
                                                                                                                                           storage.Value.Unit.Status.Count,
                                                                                                                                           storage.Value.Unit.Status.RetainCount,
                                                                                                                                           storage.Value.Unit.Status.ReplenishmentStatus switch
                                                                                                                                           {
                                                                                                                                               CardStatusClass.ReplenishmentStatusEnum.Empty => ReplenishmentStatusEnumEnum.Empty,
                                                                                                                                               CardStatusClass.ReplenishmentStatusEnum.Full => ReplenishmentStatusEnumEnum.Full,
                                                                                                                                               CardStatusClass.ReplenishmentStatusEnum.High => ReplenishmentStatusEnumEnum.High,
                                                                                                                                               CardStatusClass.ReplenishmentStatusEnum.Low => ReplenishmentStatusEnumEnum.Low,
                                                                                                                                              _ => ReplenishmentStatusEnumEnum.Ok,
                                                                                                                                           })));
                    storageResponse.Add(storage.Key, thisStorage);
                }
            }
            else
            {
                foreach (var storage in Storage.CashUnits)
                {
                    Dictionary<string, XFS4IoT.CashManagement.CashItemClass> capItems = new();
                    if (storage.Value.Unit.Capabilities.BanknoteItems is not null)
                    {
                        foreach (var item in storage.Value.Unit.Capabilities.BanknoteItems)
                        {
                            capItems.Add(item.Key, new XFS4IoT.CashManagement.CashItemClass(item.Value.NoteId,
                                                                                            item.Value.Currency,
                                                                                            item.Value.Value,
                                                                                            item.Value.Release));
                        }
                    }

                    Dictionary<string, XFS4IoT.CashManagement.CashItemClass> confItems = new();
                    if (storage.Value.Unit.Configuration.BanknoteItems is not null)
                    {
                        foreach (var item in storage.Value.Unit.Configuration.BanknoteItems)
                        {
                            confItems.Add(item.Key, new XFS4IoT.CashManagement.CashItemClass(item.Value.NoteId,
                                                                                             item.Value.Currency,
                                                                                             item.Value.Value,
                                                                                             item.Value.Release));
                        }
                    }

                    StorageUnitClass thisStorage = new(storage.Value.PositionName,
                                                       storage.Value.Capacity,
                                                       storage.Value.Status switch
                                                       {
                                                           CashUnitStorage.StatusEnum.Good => StatusEnum.Ok,
                                                           CashUnitStorage.StatusEnum.Inoperative => StatusEnum.Inoperative,
                                                           CashUnitStorage.StatusEnum.Manipulated => StatusEnum.Manipulated,
                                                           CashUnitStorage.StatusEnum.Missing => StatusEnum.Missing,
                                                           _ => StatusEnum.NotConfigured,
                                                       },
                                                       storage.Value.SerialNumber,
                                                       Cash: new XFS4IoT.CashManagement.StorageCashClass(
                                                                new XFS4IoT.CashManagement.StorageCashCapabilitiesClass(new XFS4IoT.CashManagement.StorageCashTypesClass(storage.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                                                                                                                                                         storage.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                                                                                                                                                         storage.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                                                                                                                                                         storage.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                                                                                                                                                         storage.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                                                                                                                                                         storage.Value.Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)),
                                                                                                                        new XFS4IoT.CashManagement.StorageCashItemTypesClass(storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                                                                                                                                                             storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                                                                                                                                                             storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                                                                                                                                                             storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Conterfeit),
                                                                                                                                                                             storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                                                                                                                                                             storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                                                                                                                                                             storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                                                                                                                                                             storage.Value.Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)),
                                                                                                                        storage.Value.Unit.Capabilities.HardwareSensors,
                                                                                                                        storage.Value.Unit.Capabilities.RetractAreas,
                                                                                                                        storage.Value.Unit.Capabilities.RetractThresholds,
                                                                                                                        capItems),
                                                                new XFS4IoT.CashManagement.StorageCashConfigurationClass(new XFS4IoT.CashManagement.StorageCashTypesClass(storage.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                                                                                                                                                          storage.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                                                                                                                                                          storage.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                                                                                                                                                          storage.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                                                                                                                                                          storage.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                                                                                                                                                          storage.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)),
                                                                                                                        new XFS4IoT.CashManagement.StorageCashItemTypesClass(storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                                                                                                                                                             storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                                                                                                                                                             storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                                                                                                                                                             storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Conterfeit),
                                                                                                                                                                             storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                                                                                                                                                             storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                                                                                                                                                             storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                                                                                                                                                             storage.Value.Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)),
                                                                                                                        storage.Value.Unit.Configuration.Currency,
                                                                                                                        storage.Value.Unit.Configuration.Value,
                                                                                                                        storage.Value.Unit.Configuration.HighThreshold,
                                                                                                                        storage.Value.Unit.Configuration.LowThreshold,
                                                                                                                        storage.Value.Unit.Configuration.AppLockIn,
                                                                                                                        storage.Value.Unit.Configuration.AppLockOut,
                                                                                                                        confItems),
                                                                new XFS4IoT.CashManagement.StorageCashStatusClass(storage.Value.Unit.Status.Index,
                                                                                                                  storage.Value.Unit.Status.InitialCounts?.CopyTo(),
                                                                                                                  new XFS4IoT.CashManagement.StorageCashOutClass(storage.Value.Unit.Status.StorageCashOutCount?.Presented?.CopyTo(),
                                                                                                                                                                 storage.Value.Unit.Status.StorageCashOutCount?.Rejected?.CopyTo(),
                                                                                                                                                                 storage.Value.Unit.Status.StorageCashOutCount?.Distributed?.CopyTo(),
                                                                                                                                                                 storage.Value.Unit.Status.StorageCashOutCount?.Unknown?.CopyTo(),
                                                                                                                                                                 storage.Value.Unit.Status.StorageCashOutCount?.Stacked?.CopyTo(),
                                                                                                                                                                 storage.Value.Unit.Status.StorageCashOutCount?.Diverted?.CopyTo(),
                                                                                                                                                                 storage.Value.Unit.Status.StorageCashOutCount?.Transport?.CopyTo()),
                                                                                                                  new XFS4IoT.CashManagement.StorageCashInClass(storage.Value.Unit.Status.StorageCashInCount?.RetractOperations,
                                                                                                                                                                storage.Value.Unit.Status.StorageCashInCount?.Deposited?.CopyTo(),
                                                                                                                                                                storage.Value.Unit.Status.StorageCashInCount?.Retracted?.CopyTo(),
                                                                                                                                                                storage.Value.Unit.Status.StorageCashInCount?.Rejected?.CopyTo(),
                                                                                                                                                                storage.Value.Unit.Status.StorageCashInCount?.Distributed?.CopyTo(),
                                                                                                                                                                storage.Value.Unit.Status.StorageCashInCount?.Transport?.CopyTo()),
                                                                                                                  storage.Value.Unit.Status.Count,
                                                                                                                  storage.Value.Unit.Status.Accuracy switch
                                                                                                                  {
                                                                                                                      CashStatusClass.AccuracyEnum.Accurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Accurate,
                                                                                                                      CashStatusClass.AccuracyEnum.AccurateSet => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.AccurateSet,
                                                                                                                      CashStatusClass.AccuracyEnum.Inaccurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Inaccurate,
                                                                                                                      CashStatusClass.AccuracyEnum.NotSupported => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.NotSupported,
                                                                                                                      _ => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Unknown,
                                                                                                                  },
                                                                                                                  storage.Value.Unit.Status.ReplenishmentStatus switch
                                                                                                                  {
                                                                                                                      CashStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Empty,
                                                                                                                      CashStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Full,
                                                                                                                      CashStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Ok,
                                                                                                                      CashStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.CashManagement.ReplenishmentStatusEnum.High,
                                                                                                                      _ => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Low,
                                                                                                                  })),
                                                        Card: null);

                    storageResponse.Add(storage.Key, thisStorage);
                }
            }

            return Task.FromResult(new GetStorageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                        null,
                                                                        storageResponse));
        }
    }
}
