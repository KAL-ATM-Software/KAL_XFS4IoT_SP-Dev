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
using XFS4IoT;
using XFS4IoT.Events;
using XFS4IoT.Storage.Events;
using XFS4IoTFramework.CashAcceptor;
using XFS4IoTFramework.CashDispenser;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.Storage
{
    public class StorageErrorCommandEvent
    {
        public StorageErrorCommandEvent(IStorageService StorageService, ICalibrateCashUnitEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<ICalibrateCashUnitEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            CalibrateCashUnitEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, IResetEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<IResetEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            ResetEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, IRetractEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<IRetractEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            RetractEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, ICashInEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<ICashInEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            CashInEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, IReplenishEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<IReplenishEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            ReplenishEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, ICashInEndEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<ICashInEndEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            CashInEndEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, ICashInRollbackEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<ICashInRollbackEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            CashInRollbackEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, ICountEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<ICountEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            CountEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, ICashUnitCountEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<ICashUnitCountEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            CashUnitCountEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, IPreparePresentEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<IPreparePresentEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            PreparePresentEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, IDispenseEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<IDispenseEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            DispenseEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, IRejectEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<IRejectEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            RejectEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, ITestCashUnitsEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<ITestCashUnitsEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            TestCashUnitsEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, IDepleteEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<IDepleteEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            DepleteEvents = events;
        }
        
        public Task StorageErrorEvent(FailureEnum Failure, List<string> CashUnitIds)
        {
            Dictionary<string, XFS4IoT.Storage.StorageUnitClass> storages = GetStorages(CashUnitIds);
            StorageErrorEvent.PayloadData payload = new(
                Failure switch
                {
                    FailureEnum.Config => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Config,
                    FailureEnum.Empty => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Empty,
                    FailureEnum.Error => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Error,
                    FailureEnum.Full => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Full,
                    FailureEnum.Invalid => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Invalid,
                    FailureEnum.Locked => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.Locked,
                    _ => XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData.FailureEnum.NotConfigured,
                },
                storages);

            if (CalibrateCashUnitEvents is not null)
            {
                return CalibrateCashUnitEvents.StorageErrorEvent(payload);
            }
            if (ResetEvents is not null)
            {
                return ResetEvents.StorageErrorEvent(payload);
            }
            if (RetractEvents is not null)
            {
                return RetractEvents.StorageErrorEvent(payload);
            }

            if (CashInEvents is not null)
            {
                return CashInEvents.StorageErrorEvent(payload);
            }
            if (CashInEndEvents is not null)
            {
                return CashInEndEvents.StorageErrorEvent(payload);
            }
            if (CashInRollbackEvents is not null)
            {
                return CashInRollbackEvents.StorageErrorEvent(payload);
            }
            if (CashUnitCountEvents is not null)
            {
                return CashUnitCountEvents.StorageErrorEvent(payload);
            }
            if (PreparePresentEvents is not null)
            {
                return PreparePresentEvents.StorageErrorEvent(payload);
            }
            if (ReplenishEvents is not null)
            {
                return ReplenishEvents.StorageErrorEvent(payload);
            }
            if (DepleteEvents is not null)
            {
                return DepleteEvents.StorageErrorEvent(payload);
            }

            if (DispenseEvents is not null)
            {
                return DispenseEvents.StorageErrorEvent(payload);
            }
            if (RejectEvents is not null)
            {
                return RejectEvents.StorageErrorEvent(payload);
            }
            if (TestCashUnitsEvents is not null)
            {
                return TestCashUnitsEvents.StorageErrorEvent(payload);
            }
            if (CountEvents is not null)
            {
                return CountEvents.StorageErrorEvent(payload);
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(StorageErrorEvent));
        }
        private Dictionary<string, XFS4IoT.Storage.StorageUnitClass> GetStorages(List<string> CashUnitIds)
        {
            Dictionary<string, XFS4IoT.Storage.StorageUnitClass> storages = [];

            foreach (var storageId in CashUnitIds)
            {
                if (!StorageService.CashUnits.ContainsKey(storageId))
                    continue;

                storages.Add(storageId,
                             new(Id: StorageService.CashUnits[storageId].Id,
                                 PositionName: StorageService.CashUnits[storageId].PositionName,
                                 Capacity: StorageService.CashUnits[storageId].Capacity,
                                 Status: StorageService.CashUnits[storageId].Status switch
                                 {
                                     CashUnitStorage.StatusEnum.Good => XFS4IoT.Storage.StatusEnum.Ok,
                                     CashUnitStorage.StatusEnum.Inoperative => XFS4IoT.Storage.StatusEnum.Inoperative,
                                     CashUnitStorage.StatusEnum.Manipulated => XFS4IoT.Storage.StatusEnum.Manipulated,
                                     CashUnitStorage.StatusEnum.Missing => XFS4IoT.Storage.StatusEnum.Missing,
                                     _ => XFS4IoT.Storage.StatusEnum.NotConfigured,
                                 },
                                 SerialNumber: StorageService.CashUnits[storageId].SerialNumber,
                                 Cash: new XFS4IoT.CashManagement.StorageCashClass(
                                        new XFS4IoT.CashManagement.StorageCashCapabilitiesClass(new XFS4IoT.CashManagement.StorageCashTypesClass(StorageService.CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                                                                                                                                 StorageService.CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                                                                                                                                 StorageService.CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                                                                                                                                 StorageService.CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                                                                                                                                 StorageService.CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                                                                                                                                 StorageService.CashUnits[storageId].Unit.Capabilities.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)),
                                                                                                new XFS4IoT.CashManagement.StorageCashItemTypesClass(StorageService.CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                                                                                                                                     StorageService.CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                                                                                                                                     StorageService.CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                                                                                                                                     StorageService.CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Conterfeit),
                                                                                                                                                     StorageService.CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                                                                                                                                     StorageService.CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                                                                                                                                     StorageService.CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                                                                                                                                     StorageService.CashUnits[storageId].Unit.Capabilities.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)),
                                                                                                StorageService.CashUnits[storageId].Unit.Capabilities.HardwareSensors,
                                                                                                StorageService.CashUnits[storageId].Unit.Capabilities.RetractAreas,
                                                                                                StorageService.CashUnits[storageId].Unit.Capabilities.RetractThresholds,
                                                                                                StorageService.CashUnits[storageId].Unit.Capabilities.BanknoteItems),
                                        new XFS4IoT.CashManagement.StorageCashConfigurationClass(new XFS4IoT.CashManagement.StorageCashTypesClass(StorageService.CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashIn),
                                                                                                                                                  StorageService.CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut),
                                                                                                                                                  StorageService.CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Replenishment),
                                                                                                                                                  StorageService.CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract),
                                                                                                                                                  StorageService.CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract),
                                                                                                                                                  StorageService.CashUnits[storageId].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)),
                                                                                                new XFS4IoT.CashManagement.StorageCashItemTypesClass(StorageService.CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Fit),
                                                                                                                                                     StorageService.CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unfit),
                                                                                                                                                     StorageService.CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Unrecognized),
                                                                                                                                                     StorageService.CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Conterfeit),
                                                                                                                                                     StorageService.CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Suspect),
                                                                                                                                                     StorageService.CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Inked),
                                                                                                                                                     StorageService.CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Coupon),
                                                                                                                                                     StorageService.CashUnits[storageId].Unit.Configuration.Items.HasFlag(CashCapabilitiesClass.ItemsEnum.Document)),
                                                                                                StorageService.CashUnits[storageId].Unit.Configuration.Currency,
                                                                                                StorageService.CashUnits[storageId].Unit.Configuration.Value,
                                                                                                StorageService.CashUnits[storageId].Unit.Configuration.HighThreshold,
                                                                                                StorageService.CashUnits[storageId].Unit.Configuration.LowThreshold,
                                                                                                StorageService.CashUnits[storageId].Unit.Configuration.AppLockIn,
                                                                                                StorageService.CashUnits[storageId].Unit.Configuration.AppLockOut,
                                                                                                StorageService.CashUnits[storageId].Unit.Configuration.BanknoteItems),
                                        new XFS4IoT.CashManagement.StorageCashStatusClass(StorageService.CashUnits[storageId].Unit.Status.Index,
                                                                                          StorageService.CashUnits[storageId].Unit.Status.InitialCounts.CopyTo(),
                                                                                            new XFS4IoT.CashManagement.StorageCashOutClass(StorageService.CashUnits[storageId].Unit.Status.StorageCashOutCount?.Presented.CopyTo(),
                                                                                                                                           StorageService.CashUnits[storageId].Unit.Status.StorageCashOutCount?.Rejected.CopyTo(),
                                                                                                                                           StorageService.CashUnits[storageId].Unit.Status.StorageCashOutCount?.Distributed.CopyTo(),
                                                                                                                                           StorageService.CashUnits[storageId].Unit.Status.StorageCashOutCount?.Unknown.CopyTo(),
                                                                                                                                           StorageService.CashUnits[storageId].Unit.Status.StorageCashOutCount?.Stacked.CopyTo(),
                                                                                                                                           StorageService.CashUnits[storageId].Unit.Status.StorageCashOutCount?.Diverted.CopyTo(),
                                                                                                                                           StorageService.CashUnits[storageId].Unit.Status.StorageCashOutCount?.Transport.CopyTo()),
                                                                                            new XFS4IoT.CashManagement.StorageCashInClass(StorageService.CashUnits[storageId].Unit.Status.StorageCashInCount?.RetractOperations,
                                                                                                                                          StorageService.CashUnits[storageId].Unit.Status.StorageCashInCount?.Deposited.CopyTo(),
                                                                                                                                          StorageService.CashUnits[storageId].Unit.Status.StorageCashInCount?.Retracted.CopyTo(),
                                                                                                                                          StorageService.CashUnits[storageId].Unit.Status.StorageCashInCount?.Rejected.CopyTo(),
                                                                                                                                          StorageService.CashUnits[storageId].Unit.Status.StorageCashInCount?.Distributed.CopyTo(),
                                                                                                                                          StorageService.CashUnits[storageId].Unit.Status.StorageCashInCount?.Transport.CopyTo()),
                                                                                            StorageService.CashUnits[storageId].Unit.Status.Accuracy switch
                                                                                            {
                                                                                                CashStatusClass.AccuracyEnum.Accurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Accurate,
                                                                                                CashStatusClass.AccuracyEnum.AccurateSet => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.AccurateSet,
                                                                                                CashStatusClass.AccuracyEnum.Inaccurate => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Inaccurate,
                                                                                                CashStatusClass.AccuracyEnum.Unknown => XFS4IoT.CashManagement.StorageCashStatusClass.AccuracyEnum.Unknown,
                                                                                                _ => null,
                                                                                            },
                                                                                            StorageService.CashUnits[storageId].Unit.Status.ReplenishmentStatus switch
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

        private IStorageService StorageService { get; init; }

        #region CashManagement
        protected IRetractEvents RetractEvents { get; init; } = null;
        protected IResetEvents ResetEvents { get; init; } = null;
        protected ICalibrateCashUnitEvents CalibrateCashUnitEvents { get; init; } = null;
        #endregion

        #region CashDispenser
        protected IDispenseEvents DispenseEvents { get; init; } = null;
        protected IRejectEvents RejectEvents { get; init; } = null;
        protected ITestCashUnitsEvents TestCashUnitsEvents { get; init; } = null;
        protected ICountEvents CountEvents { get; init; } = null;
        #endregion

        #region CashAcceptor
        protected ICashInEvents CashInEvents { get; init; } = null;
        protected ICashInEndEvents CashInEndEvents { get; init; } = null;
        protected ICashInRollbackEvents CashInRollbackEvents { get; init; } = null;
        protected IPreparePresentEvents PreparePresentEvents { get; init; } = null;
        protected ICashUnitCountEvents CashUnitCountEvents { get; init; } = null;
        protected IDepleteEvents DepleteEvents { get; init; } = null;
        protected IReplenishEvents ReplenishEvents { get; init; } = null;
        #endregion
    }
}
