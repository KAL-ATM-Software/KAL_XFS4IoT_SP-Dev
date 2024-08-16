/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoTServer;
using XFS4IoT;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Storage
{
    [CommandHandlerAsync]
    public partial class GetStorageHandler
    { 
        private Task<CommandResult<GetStorageCompletion.PayloadData>> HandleGetStorage(IGetStorageEvents events, GetStorageCommand getStorage, CancellationToken cancel)
        {
            Dictionary<string, StorageUnitClass> storageResponse = [];

            if (Storage.StorageType.HasFlag(StorageTypeEnum.Card))
            {
                foreach (var storage in Storage.CardUnits)
                {
                    StorageUnitClass thisStorage = new(
                        PositionName: storage.Value.PositionName,
                        Capacity: storage.Value.Capacity,
                        Status: storage.Value.Status switch
                        {
                            CardUnitStorage.StatusEnum.Good => StatusEnum.Ok,
                            CardUnitStorage.StatusEnum.Inoperative => StatusEnum.Inoperative,
                            CardUnitStorage.StatusEnum.Manipulated => StatusEnum.Manipulated,
                            CardUnitStorage.StatusEnum.Missing => StatusEnum.Missing,
                            _ => StatusEnum.NotConfigured,
                        },
                        SerialNumber: storage.Value.SerialNumber,
                        Card: new(
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
                    storageResponse.Add(storage.Key, thisStorage);
                }
            }
            else if (Storage.StorageType.HasFlag(StorageTypeEnum.Cash) ||
                     Storage.StorageType.HasFlag(StorageTypeEnum.Check))
            {
                if (Storage.StorageType.HasFlag(StorageTypeEnum.Cash))
                {
                    foreach (var storage in Storage.CashUnits)
                    {
                        StorageUnitClass thisStorage = new(
                            Id: storage.Value.Id,
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
                            Cash: new(
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

                        storageResponse.Add(storage.Key, thisStorage);
                    }
                }

                if (Storage.StorageType.HasFlag(StorageTypeEnum.Check))
                {
                    foreach (var storage in Storage.CheckUnits)
                    {
                        XFS4IoT.CashManagement.StorageCashClass cashStorage = null;

                        if (storageResponse.ContainsKey(storage.Key))
                        {
                            // Check and Cash combined unit
                            cashStorage = storageResponse[storage.Key].Cash;
                            storageResponse.Remove(storage.Key);
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
                            Cash: cashStorage,
                            Check: new(
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

                        storageResponse.Add(storage.Key, thisStorage);
                    }
                }
            }

            return Task.FromResult(
                new CommandResult<GetStorageCompletion.PayloadData>(
                    storageResponse.Count == 0 ? null : new(storageResponse),
                    MessageHeader.CompletionCodeEnum.Success)
                );
        }
    }
}
