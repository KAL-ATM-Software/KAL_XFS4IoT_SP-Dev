/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Storage.Events;

namespace XFS4IoTFramework.Storage
{
    public class StorageErrorCommandEvent
    {
        #region CashManagement
        public StorageErrorCommandEvent(IStorageService StorageService, CashManagement.ICalibrateCashUnitEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<CashManagement.ICalibrateCashUnitEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            CalibrateCashUnitEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, CashManagement.IResetEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<CashManagement.IResetEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            CashManagementResetEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, CashManagement.IRetractEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<CashManagement.IRetractEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            RetractEvents = events;
        }
        #endregion

        #region CashAcceptor
        public StorageErrorCommandEvent(IStorageService StorageService, CashAcceptor.ICashInEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<CashAcceptor.ICashInEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            CashInEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, CashAcceptor.IReplenishEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<CashAcceptor.IReplenishEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            ReplenishEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, CashAcceptor.ICashInEndEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<CashAcceptor.ICashInEndEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            CashInEndEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, CashAcceptor.ICashInRollbackEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<CashAcceptor.ICashInRollbackEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            CashInRollbackEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, CashAcceptor.ICashUnitCountEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<CashAcceptor.ICashUnitCountEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            CashUnitCountEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, CashAcceptor.IPreparePresentEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<CashAcceptor.IPreparePresentEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            PreparePresentEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, CashAcceptor.IDepleteEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<CashAcceptor.IDepleteEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            DepleteEvents = events;
        }

        #endregion

        #region CashDispenser
        public StorageErrorCommandEvent(IStorageService StorageService, CashDispenser.IDispenseEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<CashDispenser.IDispenseEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            DispenseEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, CashDispenser.IRejectEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<CashDispenser.IRejectEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            RejectEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, CashDispenser.ITestCashUnitsEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<CashDispenser.ITestCashUnitsEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            TestCashUnitsEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, CashDispenser.ICountEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<CashDispenser.ICountEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            CountEvents = events;
        }

        #endregion

        #region Check
        public StorageErrorCommandEvent(IStorageService StorageService, Check.IResetEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<Check.IResetEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            CheckResetEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, Check.IRetractMediaEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<Check.IRetractMediaEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            RetractMediaEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, Check.IMediaInEndEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<Check.IMediaInEndEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            MediaInEndEvents = events;
        }

        public StorageErrorCommandEvent(IStorageService StorageService, Check.IActionItemEvents events)
        {
            StorageService.IsA<IStorageService>($"Invalid storage interface passed in. {StorageService.GetType()} " + nameof(StorageErrorCommandEvent));
            this.StorageService = StorageService;
            events.IsNotNull($"Invalid parameter passed in. " + nameof(StorageErrorCommandEvent));
            events.IsA<Check.IActionItemEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(StorageErrorCommandEvent));
            ActionItemEvents = events;
        }
        
        #endregion

        public Task StorageErrorEvent(FailureEnum Failure, List<string> UnitIds)
        {
            Dictionary<string, XFS4IoT.Storage.StorageUnitClass> storages = StorageService.GetStorages(UnitIds);
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

            #region CashManagement
            if (CalibrateCashUnitEvents is not null)
            {
                return CalibrateCashUnitEvents.StorageErrorEvent(payload);
            }
            if (CashManagementResetEvents is not null)
            {
                return CashManagementResetEvents.StorageErrorEvent(payload);
            }
            if (RetractEvents is not null)
            {
                return RetractEvents.StorageErrorEvent(payload);
            }
            #endregion

            #region CashAcceptor
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
            #endregion

            #region CashDispenser
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
            #endregion

            #region Check
            if (CheckResetEvents is not null)
            {
                return CheckResetEvents.StorageErrorEvent(payload);
            }
            if (RetractMediaEvents is not null)
            {
                return RetractMediaEvents.StorageErrorEvent(payload);
            }
            if (MediaInEndEvents is not null)
            {
                return MediaInEndEvents.StorageErrorEvent(payload);
            }
            if (ActionItemEvents is not null)
            {
                return ActionItemEvents.StorageErrorEvent(payload);
            }
            #endregion 

            throw new InvalidOperationException($"Unreachable code. " + nameof(StorageErrorEvent));
        }

        private IStorageService StorageService { get; init; }

        #region CashManagement
        protected CashManagement.IRetractEvents RetractEvents { get; init; } = null;
        protected CashManagement.IResetEvents CashManagementResetEvents { get; init; } = null;
        protected CashManagement.ICalibrateCashUnitEvents CalibrateCashUnitEvents { get; init; } = null;
        #endregion

        #region CashDispenser
        protected CashDispenser.IDispenseEvents DispenseEvents { get; init; } = null;
        protected CashDispenser.IRejectEvents RejectEvents { get; init; } = null;
        protected CashDispenser.ITestCashUnitsEvents TestCashUnitsEvents { get; init; } = null;
        protected CashDispenser.ICountEvents CountEvents { get; init; } = null;
        #endregion

        #region CashAcceptor
        protected CashAcceptor.ICashInEvents CashInEvents { get; init; } = null;
        protected CashAcceptor.ICashInEndEvents CashInEndEvents { get; init; } = null;
        protected CashAcceptor.ICashInRollbackEvents CashInRollbackEvents { get; init; } = null;
        protected CashAcceptor.IPreparePresentEvents PreparePresentEvents { get; init; } = null;
        protected CashAcceptor.ICashUnitCountEvents CashUnitCountEvents { get; init; } = null;
        protected CashAcceptor.IDepleteEvents DepleteEvents { get; init; } = null;
        protected CashAcceptor.IReplenishEvents ReplenishEvents { get; init; } = null;
        #endregion

        #region Check
        protected Check.IResetEvents CheckResetEvents { get; init; } = null;
        protected Check.IRetractMediaEvents RetractMediaEvents { get; init; } = null;
        protected Check.IMediaInEndEvents MediaInEndEvents { get; init;} = null;
        protected Check.IActionItemEvents ActionItemEvents { get; init; } = null;
        #endregion
    }
}
