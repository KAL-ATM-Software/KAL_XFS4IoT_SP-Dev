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
using XFS4IoT;
using XFS4IoTFramework.Storage;
using XFS4IoT.Storage;
using System.ComponentModel;
using XFS4IoT.CashManagement;
using System.Threading;

namespace XFS4IoTServer
{
    public partial class StorageServiceClass
    {
        private async Task StorageChangedEventHander(object sender, PropertyChangedEventArgs propertyInfo)
        {
            if (sender.GetType() == typeof(CardStatusClass) ||
                sender.GetType() == typeof(CardUnitStorage))
            {
                if (sender.GetType() == typeof(CardStatusClass))
                {
                    CardStatusClass cardStatus = sender as CardStatusClass;
                    cardStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");
                    if (string.IsNullOrEmpty(cardStatus.StorageId))
                    {
                        return;
                    }

                    if (propertyInfo.PropertyName == nameof(cardStatus.Count) ||
                        propertyInfo.PropertyName == nameof(cardStatus.InitialCount) ||
                        propertyInfo.PropertyName == nameof(cardStatus.RetainCount))
                    {
                        await CountsChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    cardStatus.StorageId,
                                    new StorageUnitClass(
                                        Card: new XFS4IoT.CardReader.StorageClass(
                                            Status: new XFS4IoT.CardReader.StorageStatusClass(
                                                InitialCount: (propertyInfo.PropertyName == nameof(cardStatus.InitialCount)) ? cardStatus.InitialCount : null,
                                                Count: (propertyInfo.PropertyName == nameof(cardStatus.Count)) ? cardStatus.Count : null,
                                                RetainCount: (propertyInfo.PropertyName == nameof(cardStatus.RetainCount)) ? cardStatus.RetainCount : null)
                                            )
                                        )
                                }
                                }
                            });
                    }
                    else if (propertyInfo.PropertyName == nameof(cardStatus.ReplenishmentStatus))
                    {
                        await StorageChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    cardStatus.StorageId,
                                    new StorageUnitClass(
                                        Card: new XFS4IoT.CardReader.StorageClass(
                                            Status: new XFS4IoT.CardReader.StorageStatusClass(
                                                ReplenishmentStatus: cardStatus.ReplenishmentStatus switch
                                                {
                                                    CardStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Empty,
                                                    CardStatusClass.ReplenishmentStatusEnum.Low => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Low,
                                                    CardStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Full,
                                                    CardStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.High,
                                                    CardStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.CardReader.StorageStatusClass.ReplenishmentStatusEnum.Ok,
                                                    _ => throw new InternalErrorException($"Unknown replenishment status received while handing StorageChangedEvent {cardStatus.ReplenishmentStatus}")
                                                })
                                            )
                                        )
                                }
                                }
                            });
                    }
                }
                else if (sender.GetType() == typeof(CardUnitStorage))
                {
                    CardUnitStorage cardStatus = sender as CardUnitStorage;
                    cardStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");
                    if (string.IsNullOrEmpty(cardStatus.StorageId))
                    {
                        return;
                    }

                    if (propertyInfo.PropertyName == nameof(cardStatus.Status))
                    {
                        await StorageChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new()
                                {
                                {
                                    cardStatus.StorageId,
                                    new StorageUnitClass(
                                        Status: cardStatus.Status switch
                                        {
                                            UnitStorageBase.StatusEnum.Missing => XFS4IoT.Storage.StatusEnum.Missing,
                                            UnitStorageBase.StatusEnum.Manipulated => XFS4IoT.Storage.StatusEnum.Manipulated,
                                            UnitStorageBase.StatusEnum.Inoperative => XFS4IoT.Storage.StatusEnum.Inoperative,
                                            UnitStorageBase.StatusEnum.NotConfigured => XFS4IoT.Storage.StatusEnum.NotConfigured,
                                            UnitStorageBase.StatusEnum.Good => XFS4IoT.Storage.StatusEnum.Ok,
                                            _ => throw new InternalErrorException($"Unknown storage status received while handing StorageChangedEvent {cardStatus.Status}")
                                        })
                                }
                                }
                            });
                    }
                }
            }
            else if (sender.GetType() == typeof(CashStatusClass) ||
                     sender.GetType() == typeof(CashUnitStorage))
            {
                if (sender.GetType() == typeof(CashStatusClass))
                {
                    CashStatusClass cashStatus = sender as CashStatusClass;
                    cashStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    if (string.IsNullOrEmpty(cashStatus.StorageId))
                    {
                        return;
                    }

                    if (propertyInfo.PropertyName == nameof(cashStatus.StorageCashOutCount) ||
                        propertyInfo.PropertyName == nameof(cashStatus.StorageCashInCount) ||
                        propertyInfo.PropertyName == nameof(cashStatus.InitialCounts))
                    {
                        await CountsChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    cashStatus.StorageId,
                                    new StorageUnitClass(
                                        Cash: new XFS4IoT.CashManagement.StorageCashClass(
                                            Status: new XFS4IoT.CashManagement.StorageCashStatusClass(
                                                Initial: (propertyInfo.PropertyName == nameof(cashStatus.InitialCounts)) ? cashStatus.InitialCounts.CopyTo() : null,
                                                Out: (propertyInfo.PropertyName == nameof(cashStatus.StorageCashOutCount)) ? 
                                                new(
                                                    Presented: cashStatus.StorageCashOutCount.Presented?.CopyTo(),
                                                    Rejected: cashStatus.StorageCashOutCount.Rejected?.CopyTo(),
                                                    Distributed: cashStatus.StorageCashOutCount.Distributed?.CopyTo(),
                                                    Unknown: cashStatus.StorageCashOutCount.Unknown?.CopyTo(),
                                                    Stacked: cashStatus.StorageCashOutCount.Stacked?.CopyTo(),
                                                    Diverted: cashStatus.StorageCashOutCount.Diverted?.CopyTo(),
                                                    Transport: cashStatus.StorageCashOutCount.Transport?.CopyTo()
                                                    ) : null,
                                                In: (propertyInfo.PropertyName == nameof(cashStatus.StorageCashInCount)) ? 
                                                new(
                                                    RetractOperations: cashStatus.StorageCashInCount.RetractOperations,
                                                    Deposited: cashStatus.StorageCashInCount.Deposited?.CopyTo(),
                                                    Retracted: cashStatus.StorageCashInCount.Retracted?.CopyTo(),
                                                    Rejected: cashStatus.StorageCashInCount.Rejected?.CopyTo(),
                                                    Distributed: cashStatus.StorageCashInCount.Distributed?.CopyTo(),
                                                    Transport: cashStatus.StorageCashInCount.Transport?.CopyTo()
                                                    ) : null)
                                            )
                                        )
                                }
                                }
                            });
                    }
                    else if (propertyInfo.PropertyName == nameof(cashStatus.ReplenishmentStatus))
                    {
                        await StorageChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    cashStatus.StorageId,
                                    new StorageUnitClass(
                                        Cash: new XFS4IoT.CashManagement.StorageCashClass(
                                            Status: new XFS4IoT.CashManagement.StorageCashStatusClass(
                                                ReplenishmentStatus: cashStatus.ReplenishmentStatus switch
                                                {
                                                    CashStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Empty,
                                                    CashStatusClass.ReplenishmentStatusEnum.Low => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Low,
                                                    CashStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Full,
                                                    CashStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.CashManagement.ReplenishmentStatusEnum.High,
                                                    CashStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.CashManagement.ReplenishmentStatusEnum.Ok,
                                                    _ => throw new InternalErrorException($"Unknown replenishment status received while handing StorageChangedEvent {cashStatus.ReplenishmentStatus}")
                                                })
                                            )
                                        )
                                }
                                }
                            });
                    }
                }
                else if (sender.GetType() == typeof(CashUnitStorage))
                {
                    CashUnitStorage cashStatus = sender as CashUnitStorage;
                    cashStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    if (string.IsNullOrEmpty(cashStatus.StorageId))
                    {
                        return;
                    }

                    if (propertyInfo.PropertyName == nameof(cashStatus.Status))
                    {
                        await StorageChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    cashStatus.StorageId,
                                    new StorageUnitClass(
                                        Status: cashStatus.Status switch
                                        {
                                            UnitStorageBase.StatusEnum.Missing => XFS4IoT.Storage.StatusEnum.Missing,
                                            UnitStorageBase.StatusEnum.Manipulated => XFS4IoT.Storage.StatusEnum.Manipulated,
                                            UnitStorageBase.StatusEnum.Inoperative => XFS4IoT.Storage.StatusEnum.Inoperative,
                                            UnitStorageBase.StatusEnum.NotConfigured => XFS4IoT.Storage.StatusEnum.NotConfigured,
                                            UnitStorageBase.StatusEnum.Good => XFS4IoT.Storage.StatusEnum.Ok,
                                            _ => throw new InternalErrorException($"Unknown storage status received while handing StorageChangedEvent {cashStatus.Status}")
                                        })
                                }
                                }
                            });
                    }
                }
            }
            else if (sender.GetType() == typeof(CheckStatusClass) ||
                     sender.GetType() == typeof(StorageCheckCountClass) ||
                     sender.GetType() == typeof(CheckUnitStorage))
            {
                if (sender.GetType() == typeof(CheckStatusClass))
                {
                    CheckStatusClass checkStatus = sender as CheckStatusClass;
                    checkStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    if (string.IsNullOrEmpty(checkStatus.StorageId))
                    {
                        return;
                    }

                    if (propertyInfo.PropertyName == nameof(checkStatus.InitialCounts) ||
                        propertyInfo.PropertyName == nameof(checkStatus.CheckInCounts))
                    {
                        await CountsChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    checkStatus.StorageId,
                                    new StorageUnitClass(
                                        Check: new XFS4IoT.Check.StorageClass(
                                            Status: new XFS4IoT.Check.StorageStatusClass(
                                                Initial: (propertyInfo.PropertyName == nameof(checkStatus.InitialCounts)) ? 
                                                new XFS4IoT.Check.StorageStatusClass.InitialClass(
                                                    MediaInCount: checkStatus.InitialCounts.MediaInCount,
                                                    Count: checkStatus.InitialCounts.Count) : null,
                                                In: (propertyInfo.PropertyName == nameof(checkStatus.CheckInCounts)) ? 
                                                new XFS4IoT.Check.StorageStatusClass.InClass(
                                                    MediaInCount: checkStatus.CheckInCounts.MediaInCount,
                                                    Count: checkStatus.CheckInCounts.Count) : null
                                                )
                                            )
                                        )
                                }
                                }
                            });
                    }
                    else if (propertyInfo.PropertyName == nameof(checkStatus.ReplenishmentStatus))
                    {
                        await StorageChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    checkStatus.StorageId,
                                    new StorageUnitClass(
                                        Check: new XFS4IoT.Check.StorageClass(
                                            Status: new XFS4IoT.Check.StorageStatusClass(
                                                ReplenishmentStatus: checkStatus.ReplenishmentStatus switch
                                                {
                                                    CheckStatusClass.ReplenishmentStatusEnum.Empty => XFS4IoT.Check.ReplenishmentStatusEnum.Empty,
                                                    CheckStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.Check.ReplenishmentStatusEnum.Full,
                                                    CheckStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.Check.ReplenishmentStatusEnum.High,
                                                    CheckStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.Check.ReplenishmentStatusEnum.Ok,
                                                    _ => throw new InternalErrorException($"Unknown replenishment status received while handing StorageChangedEvent {checkStatus.ReplenishmentStatus}")
                                                }
                                                )
                                            )
                                        )
                                }
                                }
                            });
                    }
                }
                else if (sender.GetType() == typeof(StorageCheckCountClass))
                {
                    StorageCheckCountClass checkStatus = sender as StorageCheckCountClass;
                    checkStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    if (string.IsNullOrEmpty(checkStatus.StorageId))
                    {
                        return;
                    }

                    if (propertyInfo.PropertyName == nameof(checkStatus.Count) ||
                        propertyInfo.PropertyName == nameof(checkStatus.MediaInCount))
                    {
                        XFS4IoT.Check.StorageStatusClass.InitialClass initialCount = null;
                        XFS4IoT.Check.StorageStatusClass.InClass mediaInCount = null;

                        if (checkStatus.ParentPropertyName == nameof(CheckStatusClass.InitialCounts))
                        {
                            initialCount = new(
                                MediaInCount: (propertyInfo.PropertyName == nameof(checkStatus.MediaInCount)) ? checkStatus.MediaInCount : null,
                                Count: (propertyInfo.PropertyName == nameof(checkStatus.Count)) ? checkStatus.Count : null);
                        }
                        else
                        {
                            mediaInCount = new(
                                MediaInCount: (propertyInfo.PropertyName == nameof(checkStatus.MediaInCount)) ? checkStatus.MediaInCount : null,
                                Count: (propertyInfo.PropertyName == nameof(checkStatus.Count)) ? checkStatus.Count : null);
                        }

                        await CountsChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    checkStatus.StorageId,
                                    new StorageUnitClass(
                                        Check: new XFS4IoT.Check.StorageClass(
                                            Status: new XFS4IoT.Check.StorageStatusClass(
                                                Initial: initialCount,
                                                In: mediaInCount)
                                            )
                                        )
                                }
                                }
                            });
                    }
                    else if (propertyInfo.PropertyName == nameof(checkStatus.RetractOperations))
                    {
                        XFS4IoT.Check.StorageStatusClass.InitialClass initialCount = null;
                        XFS4IoT.Check.StorageStatusClass.InClass mediaInCount = null;

                        if (checkStatus.ParentPropertyName == nameof(CheckStatusClass.InitialCounts))
                        {
                            initialCount = new(RetractOperations: checkStatus.RetractOperations);
                        }
                        else
                        {
                            mediaInCount = new(RetractOperations: checkStatus.RetractOperations);
                        }

                        await StorageChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                    {
                                        checkStatus.StorageId,
                                        new StorageUnitClass(
                                            Check: new XFS4IoT.Check.StorageClass(
                                                Status: new XFS4IoT.Check.StorageStatusClass(
                                                    Initial: initialCount,
                                                    In: mediaInCount
                                                    )
                                                )
                                            )
                                    }
                                }
                            });
                    }
                }
                else if (sender.GetType() == typeof(CheckUnitStorage))
                {
                    CheckUnitStorage checkStatus = sender as CheckUnitStorage;
                    checkStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    if (string.IsNullOrEmpty(checkStatus.StorageId))
                    {
                        return;
                    }

                    if (propertyInfo.PropertyName == nameof(checkStatus.Status))
                    {
                        await StorageChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    checkStatus.StorageId,
                                    new StorageUnitClass(
                                        Status: checkStatus.Status switch
                                        {
                                            UnitStorageBase.StatusEnum.Missing => XFS4IoT.Storage.StatusEnum.Missing,
                                            UnitStorageBase.StatusEnum.Manipulated => XFS4IoT.Storage.StatusEnum.Manipulated,
                                            UnitStorageBase.StatusEnum.Inoperative => XFS4IoT.Storage.StatusEnum.Inoperative,
                                            UnitStorageBase.StatusEnum.NotConfigured => XFS4IoT.Storage.StatusEnum.NotConfigured,
                                            UnitStorageBase.StatusEnum.Good => XFS4IoT.Storage.StatusEnum.Ok,
                                            _ => throw new InternalErrorException($"Unknown storage status received while handing StorageChangedEvent {checkStatus.Status}")
                                        })
                                }
                                }
                            });
                    }
                }
            }
            else if (sender.GetType() == typeof(XFS4IoTFramework.Storage.PrinterStatusClass) ||
                     sender.GetType() == typeof(PrinterUnitStorage))
            {
                if (sender.GetType() == typeof(XFS4IoTFramework.Storage.PrinterStatusClass))
                {
                    XFS4IoTFramework.Storage.PrinterStatusClass printerStatus = sender as XFS4IoTFramework.Storage.PrinterStatusClass;
                    printerStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    if (string.IsNullOrEmpty(printerStatus.StorageId))
                    {
                        return;
                    }

                    if (propertyInfo.PropertyName == nameof(printerStatus.InCount) ||
                        propertyInfo.PropertyName == nameof(printerStatus.InitialCount))
                    {
                        await CountsChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    printerStatus.StorageId,
                                    new StorageUnitClass(
                                        Printer: new XFS4IoT.Printer.StorageClass(
                                            Status: new XFS4IoT.Printer.StorageStatusClass(
                                                Initial: (propertyInfo.PropertyName == nameof(printerStatus.InitialCount)) ? printerStatus.InitialCount : null,
                                                In: (propertyInfo.PropertyName == nameof(printerStatus.InCount)) ? printerStatus.InCount : null)
                                            )
                                        )
                                }
                                }
                            });
                    }
                    else if (propertyInfo.PropertyName == nameof(printerStatus.ReplenishmentStatus))
                    {
                        await StorageChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    printerStatus.StorageId,
                                    new StorageUnitClass(
                                        Printer: new XFS4IoT.Printer.StorageClass(
                                            Status: new XFS4IoT.Printer.StorageStatusClass(
                                                ReplenishmentStatus: printerStatus.ReplenishmentStatus switch
                                                {
                                                    XFS4IoTFramework.Storage.PrinterStatusClass.ReplenishmentStatusEnum.Unknown => XFS4IoT.Printer.ReplenishmentStatusEnum.Unknown,
                                                    XFS4IoTFramework.Storage.PrinterStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.Printer.ReplenishmentStatusEnum.Full,
                                                    XFS4IoTFramework.Storage.PrinterStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.Printer.ReplenishmentStatusEnum.High,
                                                    XFS4IoTFramework.Storage.PrinterStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.Printer.ReplenishmentStatusEnum.Ok,
                                                    _ => throw new InternalErrorException($"Unknown replenishment status received while handing StorageChangedEvent {printerStatus.ReplenishmentStatus}")
                                                }
                                                )
                                            )
                                        )
                                }
                                }
                            });
                    }
                }
                else if (sender.GetType() == typeof(PrinterUnitStorage))
                {
                    PrinterUnitStorage printerStatus = sender as PrinterUnitStorage;
                    printerStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    if (string.IsNullOrEmpty(printerStatus.StorageId))
                    {
                        return;
                    }

                    if (propertyInfo.PropertyName == nameof(printerStatus.Status) ||
                        propertyInfo.PropertyName == nameof(printerStatus.Unit.Status.ReplenishmentStatus))
                    {
                        await StorageChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    printerStatus.StorageId,
                                    new StorageUnitClass(
                                        Status: propertyInfo.PropertyName != nameof(printerStatus.Status) ? 
                                        null : printerStatus.Status switch
                                        {
                                            UnitStorageBase.StatusEnum.Missing => XFS4IoT.Storage.StatusEnum.Missing,
                                            UnitStorageBase.StatusEnum.Manipulated => XFS4IoT.Storage.StatusEnum.Manipulated,
                                            UnitStorageBase.StatusEnum.Inoperative => XFS4IoT.Storage.StatusEnum.Inoperative,
                                            UnitStorageBase.StatusEnum.NotConfigured => XFS4IoT.Storage.StatusEnum.NotConfigured,
                                            UnitStorageBase.StatusEnum.Good => XFS4IoT.Storage.StatusEnum.Ok,
                                            _ => throw new InternalErrorException($"Unknown storage status received while handing StorageChangedEvent {printerStatus.Status}")
                                        },
                                        Printer: new(
                                            Status: new XFS4IoT.Printer.StorageStatusClass(
                                                ReplenishmentStatus: propertyInfo.PropertyName != nameof(printerStatus.Unit.Status.ReplenishmentStatus) ?
                                                null : printerStatus.Unit.Status.ReplenishmentStatus switch
                                                {
                                                    PrinterStatusClass.ReplenishmentStatusEnum.Healthy => XFS4IoT.Printer.ReplenishmentStatusEnum.Ok,
                                                    PrinterStatusClass.ReplenishmentStatusEnum.Full => XFS4IoT.Printer.ReplenishmentStatusEnum.Full,
                                                    PrinterStatusClass.ReplenishmentStatusEnum.High => XFS4IoT.Printer.ReplenishmentStatusEnum.High,
                                                    PrinterStatusClass.ReplenishmentStatusEnum.Unknown => XFS4IoT.Printer.ReplenishmentStatusEnum.Unknown,
                                                    _ => throw new InternalErrorException($"Unknown replenishment status received while handing StorageChangedEvent {printerStatus.Unit.Status.ReplenishmentStatus}")
                                                })
                                            )
                                        )
                                }
                                }
                            });
                    }
                }
            }
            else if (sender.GetType() == typeof(XFS4IoTFramework.Storage.IBNSStatusClass) ||
                     sender.GetType() == typeof(XFS4IoTFramework.Storage.PowerInfoClass) ||
                     sender.GetType() == typeof(IBNSUnitStorage))
            {
                if (sender.GetType() == typeof(XFS4IoTFramework.Storage.IBNSStatusClass))
                {
                    XFS4IoTFramework.Storage.IBNSStatusClass ibnsStatus = sender as XFS4IoTFramework.Storage.IBNSStatusClass;
                    ibnsStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    if (string.IsNullOrEmpty(ibnsStatus.StorageId))
                    {
                        return;
                    }

                    await StorageChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    ibnsStatus.StorageId,
                                    new StorageUnitClass(
                                        BanknoteNeutralization: new XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass(
                                            Identifier: propertyInfo.PropertyName != nameof(ibnsStatus.Identifier) ? null : ibnsStatus.Identifier,
                                            Protection: propertyInfo.PropertyName != nameof(ibnsStatus.Protection) ? null : ibnsStatus.Protection switch
                                            {
                                                XFS4IoTFramework.Storage.IBNSStatusClass.ProtectionEnum.Armed => XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass.ProtectionEnum.Armed,
                                                XFS4IoTFramework.Storage.IBNSStatusClass.ProtectionEnum.NeutralizationTriggered => XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass.ProtectionEnum.NeutralizationTriggered,
                                                XFS4IoTFramework.Storage.IBNSStatusClass.ProtectionEnum.Fault => XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass.ProtectionEnum.Fault,
                                                XFS4IoTFramework.Storage.IBNSStatusClass.ProtectionEnum.Disarmed => XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass.ProtectionEnum.Disarmed,
                                                _ => throw new InternalErrorException($"Unknown protection status received while handing StorageChangedEvent {ibnsStatus.Protection}")
                                            },
                                            Warning: propertyInfo.PropertyName != nameof(ibnsStatus.Warning) ? null : ibnsStatus.Warning switch
                                            {
                                                XFS4IoTFramework.Storage.IBNSStatusClass.WarningEnum.Alarm => XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass.WarningEnum.Alarm,
                                                XFS4IoTFramework.Storage.IBNSStatusClass.WarningEnum.CassetteRunsAutonomously => XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass.WarningEnum.CassetteRunsAutonomously,
                                                _ => throw new InternalErrorException($"Unknown warning status received while handing StorageChangedEvent {ibnsStatus.Warning}")
                                            },
                                            PowerSupply: null,
                                            Tilt: propertyInfo.PropertyName != nameof(ibnsStatus.TiltState) ? null : ibnsStatus.TiltState switch
                                            {
                                                XFS4IoTFramework.Common.IBNSStatusClass.TiltStateEnum.Fault => XFS4IoT.IntelligentBanknoteNeutralization.TiltStateEnum.Fault,
                                                XFS4IoTFramework.Common.IBNSStatusClass.TiltStateEnum.Tilted => XFS4IoT.IntelligentBanknoteNeutralization.TiltStateEnum.Tilted,
                                                XFS4IoTFramework.Common.IBNSStatusClass.TiltStateEnum.NotTilted => XFS4IoT.IntelligentBanknoteNeutralization.TiltStateEnum.NotTilted,
                                                XFS4IoTFramework.Common.IBNSStatusClass.TiltStateEnum.Disabled => XFS4IoT.IntelligentBanknoteNeutralization.TiltStateEnum.Disabled,
                                                _ => throw new InternalErrorException($"Unknown tilt status received while handing StorageChangedEvent {ibnsStatus.TiltState}")
                                            },
                                            Temperature: propertyInfo.PropertyName != nameof(ibnsStatus.TemperatureState) ? null : ibnsStatus.TemperatureState switch
                                            {
                                                XFS4IoTFramework.Common.IBNSStatusClass.TemperatureStateEnum.TooCold => XFS4IoT.IntelligentBanknoteNeutralization.TemperatureStateEnum.TooCold,
                                                XFS4IoTFramework.Common.IBNSStatusClass.TemperatureStateEnum.TooHot => XFS4IoT.IntelligentBanknoteNeutralization.TemperatureStateEnum.TooHot,
                                                XFS4IoTFramework.Common.IBNSStatusClass.TemperatureStateEnum.Fault => XFS4IoT.IntelligentBanknoteNeutralization.TemperatureStateEnum.Fault,
                                                XFS4IoTFramework.Common.IBNSStatusClass.TemperatureStateEnum.Healthy => XFS4IoT.IntelligentBanknoteNeutralization.TemperatureStateEnum.Ok,
                                                XFS4IoTFramework.Common.IBNSStatusClass.TemperatureStateEnum.Disabled => XFS4IoT.IntelligentBanknoteNeutralization.TemperatureStateEnum.Disabled,
                                                _ => throw new InternalErrorException($"Unknown temparature status received while handing StorageChangedEvent {ibnsStatus.TemperatureState}")
                                            },
                                            Lid: propertyInfo.PropertyName != nameof(ibnsStatus.LidStatus) ? null : ibnsStatus.LidStatus switch
                                            {
                                                XFS4IoTFramework.Storage.IBNSStatusClass.LidStatusEnum.Fault => XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass.LidEnum.Fault,
                                                XFS4IoTFramework.Storage.IBNSStatusClass.LidStatusEnum.Closed => XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass.LidEnum.Closed,
                                                XFS4IoTFramework.Storage.IBNSStatusClass.LidStatusEnum.Disabled => XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass.LidEnum.Disabled,
                                                XFS4IoTFramework.Storage.IBNSStatusClass.LidStatusEnum.Opened => XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass.LidEnum.Opened,
                                                _ => throw new InternalErrorException($"Unknown lid status received while handing StorageChangedEvent {ibnsStatus.LidStatus}")
                                            },
                                            NeutralizationTrigger: propertyInfo.PropertyName != nameof(ibnsStatus.NeutralizationTrigger) ? null : ibnsStatus.NeutralizationTrigger switch
                                            {
                                                XFS4IoTFramework.Storage.IBNSStatusClass.NeutralizationTriggerEnum.Initializing => XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass.NeutralizationTriggerEnum.Initializing,
                                                XFS4IoTFramework.Storage.IBNSStatusClass.NeutralizationTriggerEnum.Ready => XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass.NeutralizationTriggerEnum.Ready,
                                                XFS4IoTFramework.Storage.IBNSStatusClass.NeutralizationTriggerEnum.Fault => XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass.NeutralizationTriggerEnum.Fault,
                                                XFS4IoTFramework.Storage.IBNSStatusClass.NeutralizationTriggerEnum.Disabled => XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass.NeutralizationTriggerEnum.Disabled,
                                                _ => throw new InternalErrorException($"Unknown neutralization trigger status received while handing StorageChangedEvent {ibnsStatus.NeutralizationTrigger}")
                                            },
                                            StorageUnitIdentifier: propertyInfo.PropertyName != nameof(ibnsStatus.StorageUnitIdentifier) ? null : ibnsStatus.StorageUnitIdentifier)
                                        )
                                }
                                }
                            });
                }
                else if (sender.GetType() == typeof(XFS4IoTFramework.Storage.PowerInfoClass))
                {
                    XFS4IoTFramework.Storage.PowerInfoClass ibnsStatus = sender as XFS4IoTFramework.Storage.PowerInfoClass;
                    ibnsStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    if (string.IsNullOrEmpty(ibnsStatus.StorageId))
                    {
                        return;
                    }

                    XFS4IoT.PowerManagement.PowerInfoClass powerStatus = new(
                        PowerInStatus: propertyInfo.PropertyName != nameof(ibnsStatus.PowerInStatus) ? null : ibnsStatus.PowerInStatus switch
                        {
                            XFS4IoTFramework.Storage.PowerInfoClass.PoweringStatusEnum.NotPower => XFS4IoT.PowerManagement.PowerInfoClass.PowerInStatusEnum.NoPower,
                            XFS4IoTFramework.Storage.PowerInfoClass.PoweringStatusEnum.Powering => XFS4IoT.PowerManagement.PowerInfoClass.PowerInStatusEnum.Powering,
                            _ => throw new InternalErrorException($"Unknown power-in status received while handing StorageChangedEvent {ibnsStatus.PowerInStatus}")
                        },
                        PowerOutStatus: propertyInfo.PropertyName != nameof(ibnsStatus.PowerOutStatus) ? null : ibnsStatus.PowerOutStatus switch
                        {
                            XFS4IoTFramework.Storage.PowerInfoClass.PoweringStatusEnum.NotPower => XFS4IoT.PowerManagement.PowerInfoClass.PowerOutStatusEnum.NoPower,
                            XFS4IoTFramework.Storage.PowerInfoClass.PoweringStatusEnum.Powering => XFS4IoT.PowerManagement.PowerInfoClass.PowerOutStatusEnum.Powering,
                            _ => throw new InternalErrorException($"Unknown power-out status received while handing StorageChangedEvent {ibnsStatus.PowerOutStatus}")
                        },
                        BatteryStatus: propertyInfo.PropertyName != nameof(ibnsStatus.BatteryStatus) ? null : ibnsStatus.BatteryStatus switch
                        {
                            XFS4IoTFramework.Storage.PowerInfoClass.BatteryStatusEnum.Low => XFS4IoT.PowerManagement.BatteryStatusEnum.Low,
                            XFS4IoTFramework.Storage.PowerInfoClass.BatteryStatusEnum.Failure => XFS4IoT.PowerManagement.BatteryStatusEnum.Failure,
                            XFS4IoTFramework.Storage.PowerInfoClass.BatteryStatusEnum.Critical => XFS4IoT.PowerManagement.BatteryStatusEnum.Critical,
                            XFS4IoTFramework.Storage.PowerInfoClass.BatteryStatusEnum.Operational => XFS4IoT.PowerManagement.BatteryStatusEnum.Operational,
                            XFS4IoTFramework.Storage.PowerInfoClass.BatteryStatusEnum.Full => XFS4IoT.PowerManagement.BatteryStatusEnum.Full,
                            _ => throw new InternalErrorException($"Unknown battery status received while handing StorageChangedEvent {ibnsStatus.BatteryStatus}")
                        },
                        BatteryChargingStatus: propertyInfo.PropertyName != nameof(ibnsStatus.BatteryChargingStatus) ? null : ibnsStatus.BatteryChargingStatus switch
                        {
                            XFS4IoTFramework.Storage.PowerInfoClass.BatteryChargingStatusEnum.Charging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.Charging,
                            XFS4IoTFramework.Storage.PowerInfoClass.BatteryChargingStatusEnum.NotCharging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.NotCharging,
                            XFS4IoTFramework.Storage.PowerInfoClass.BatteryChargingStatusEnum.Discharging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.Discharging,
                            _ => throw new InternalErrorException($"Unknown battery charging status received while handing StorageChangedEvent {ibnsStatus.BatteryChargingStatus}")
                        });

                    await StorageChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    ibnsStatus.StorageId,
                                    new StorageUnitClass(
                                        BanknoteNeutralization: new XFS4IoT.IntelligentBanknoteNeutralization.StorageUnitStatusClass(
                                            PowerSupply: powerStatus)
                                        )
                                }
                                }
                            });
                }
                else if (sender.GetType() == typeof(IBNSUnitStorage))
                {
                    IBNSUnitStorage ibnsStatus = sender as IBNSUnitStorage;
                    ibnsStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                    if (string.IsNullOrEmpty(ibnsStatus.StorageId))
                    {
                        return;
                    }

                    if (propertyInfo.PropertyName == nameof(ibnsStatus.Status))
                    {
                        await StorageChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    ibnsStatus.StorageId,
                                    new StorageUnitClass(
                                        Status: ibnsStatus.Status switch
                                        {
                                            UnitStorageBase.StatusEnum.Missing => XFS4IoT.Storage.StatusEnum.Missing,
                                            UnitStorageBase.StatusEnum.Manipulated => XFS4IoT.Storage.StatusEnum.Manipulated,
                                            UnitStorageBase.StatusEnum.Inoperative => XFS4IoT.Storage.StatusEnum.Inoperative,
                                            UnitStorageBase.StatusEnum.NotConfigured => XFS4IoT.Storage.StatusEnum.NotConfigured,
                                            UnitStorageBase.StatusEnum.Good => XFS4IoT.Storage.StatusEnum.Ok,
                                            _ => throw new InternalErrorException($"Unknown storage status received while handing StorageChangedEvent {ibnsStatus.Status}")
                                        })
                                }
                                }
                            });
                    }
                }
                else if (sender.GetType() == typeof(DepositStatusClass) ||
                         sender.GetType() == typeof(DepositUnitStorage))
                {
                    if (sender.GetType() == typeof(DepositStatusClass))
                    {
                        DepositStatusClass depositStatus = sender as DepositStatusClass;
                        depositStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                        if (string.IsNullOrEmpty(depositStatus.StorageId))
                        {
                            return;
                        }

                        if (propertyInfo.PropertyName == nameof(depositStatus.NumberOfDeposits))
                        {
                            await CountsChangedEvent(
                                Payload: new()
                                {
                                    ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                    {
                                {
                                    depositStatus.StorageId,
                                    new StorageUnitClass(
                                        Deposit: new XFS4IoT.Deposit.StorageClass(
                                            Status: new XFS4IoT.Deposit.StorageStatusClass(
                                                NumOfDeposits: depositStatus.NumberOfDeposits)
                                            )
                                        )
                                }
                                    }
                                });
                        }
                        else
                        {
                            if (propertyInfo.PropertyName == nameof(depositStatus.NumberOfDeposits))
                            {
                                await StorageChangedEvent(
                                    Payload: new()
                                    {
                                        ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                        {
                                    {
                                        depositStatus.StorageId,
                                        new StorageUnitClass(
                                            Deposit: new XFS4IoT.Deposit.StorageClass(
                                                Status: new XFS4IoT.Deposit.StorageStatusClass(
                                                    NumOfDeposits: depositStatus.NumberOfDeposits)
                                                )
                                            )
                                    }
                                        }
                                    });
                            }
                        }
                    }
                    else if (sender.GetType() == typeof(DepositUnitStorage))
                    {
                        DepositUnitStorage depositStatus = sender as DepositUnitStorage;
                        depositStatus.IsNotNull($"Unexpected type received. {sender.GetType()}");

                        if (string.IsNullOrEmpty(depositStatus.StorageId))
                        {
                            return;
                        }

                        await StorageChangedEvent(
                            Payload: new()
                            {
                                ExtendedProperties = new Dictionary<string, StorageUnitClass>()
                                {
                                {
                                    depositStatus.StorageId,
                                    new StorageUnitClass(
                                        Status: depositStatus.Status switch
                                        {
                                            UnitStorageBase.StatusEnum.Missing => XFS4IoT.Storage.StatusEnum.Missing,
                                            UnitStorageBase.StatusEnum.Manipulated => XFS4IoT.Storage.StatusEnum.Manipulated,
                                            UnitStorageBase.StatusEnum.Inoperative => XFS4IoT.Storage.StatusEnum.Inoperative,
                                            UnitStorageBase.StatusEnum.NotConfigured => XFS4IoT.Storage.StatusEnum.NotConfigured,
                                            UnitStorageBase.StatusEnum.Good => XFS4IoT.Storage.StatusEnum.Ok,
                                            _ => throw new InternalErrorException($"Unknown storage status received while handing StorageChangedEvent {depositStatus.Status}")
                                        })
                                }
                                }
                            });
                    }
                }
            }
        }
    }
}