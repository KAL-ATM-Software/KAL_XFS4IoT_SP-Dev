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
using XFS4IoTFramework.CashDispenser;
using XFS4IoTFramework.CashAcceptor;

namespace XFS4IoTFramework.CashManagement
{
    public class ItemInfoAvailableCommandEvent
    {
        public ItemInfoAvailableCommandEvent(IDispenseEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<IDispenseEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            DispenseEvents = events;
        }
        public ItemInfoAvailableCommandEvent(IPresentEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<IPresentEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            PresentEvents = events;
        }
        public ItemInfoAvailableCommandEvent(IRejectEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<IRejectEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            RejectEvents = events;
        }
        public ItemInfoAvailableCommandEvent(ITestCashUnitsEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<ITestCashUnitsEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            TestCashUnitsEvents = events;
        }
        public ItemInfoAvailableCommandEvent(ICountEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<ICountEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            CountEvents = events;
        }
        public ItemInfoAvailableCommandEvent(IRetractEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<IRetractEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            RetractEvents = events;
        }
        public ItemInfoAvailableCommandEvent(IResetEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<IResetEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            ResetEvents = events;
        }
        public ItemInfoAvailableCommandEvent(ICalibrateCashUnitEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<ICalibrateCashUnitEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            CalibrateCashUnitEvents = events;
        }
        public ItemInfoAvailableCommandEvent(ICashInEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<ICashInEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            CashInEvents = events;
        }
        public ItemInfoAvailableCommandEvent(ICashInEndEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<ICashInEndEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            CashInEndEvents = events;
        }
        public ItemInfoAvailableCommandEvent(ICashInRollbackEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<ICashInRollbackEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            CashInRollbackEvents = events;
        }
        public ItemInfoAvailableCommandEvent(ICreateSignatureEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<ICreateSignatureEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            CreateSignatureEvents = events;
        }
        public ItemInfoAvailableCommandEvent(IPreparePresentEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<IPreparePresentEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            PreparePresentEvents = events;
        }
        public ItemInfoAvailableCommandEvent(ICashUnitCountEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<ICashUnitCountEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            CashUnitCountEvents = events;
        }
        public ItemInfoAvailableCommandEvent(IDepleteEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<IDepleteEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            DepleteEvents = events;
        }
        public ItemInfoAvailableCommandEvent(IReplenishEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ItemInfoAvailableCommandEvent));
            events.IsA<IReplenishEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ItemInfoAvailableCommandEvent));
            ReplenishEvents = events;
        }

        public Task InfoAvailableEvent(List<ItemInfoSummary> ItemInfoSummary)
        {
            List <XFS4IoT.CashManagement.Events.InfoAvailableEvent.PayloadData.ItemInfoSummaryClass> itemInfo = new();
            foreach (var item in ItemInfoSummary)
            {
                itemInfo.Add(new (item.Level switch
                                 {
                                     NoteLevelEnum.Counterfeit => XFS4IoT.CashManagement.NoteLevelEnum.Counterfeit,
                                     NoteLevelEnum.Fit => XFS4IoT.CashManagement.NoteLevelEnum.Fit,
                                     NoteLevelEnum.Inked => XFS4IoT.CashManagement.NoteLevelEnum.Inked,
                                     NoteLevelEnum.Suspect => XFS4IoT.CashManagement.NoteLevelEnum.Suspect,
                                     NoteLevelEnum.Unfit => XFS4IoT.CashManagement.NoteLevelEnum.Unfit,
                                     _ => XFS4IoT.CashManagement.NoteLevelEnum.Unrecognized,
                                 },
                                 item.NumOfItems));
            }

            if (DispenseEvents is not null)
            {
                return DispenseEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (PresentEvents is not null)
            {
                return PresentEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (RejectEvents is not null)
            {
                return RejectEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (TestCashUnitsEvents is not null)
            {
                return TestCashUnitsEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (CountEvents is not null)
            {
                return CountEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (RetractEvents is not null)
            {
                return RetractEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (ResetEvents is not null)
            {
                return ResetEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (CalibrateCashUnitEvents is not null)
            {
                return CalibrateCashUnitEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (CashInEvents is not null)
            {
                return CashInEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (CashInEndEvents is not null)
            {
                return CashInEndEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (CashInRollbackEvents is not null)
            {
                return CashInRollbackEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (CreateSignatureEvents is not null)
            {
                return CreateSignatureEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (PreparePresentEvents is not null)
            {
                return PreparePresentEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (CashUnitCountEvents is not null)
            {
                return CashUnitCountEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (DepleteEvents is not null)
            {
                return DepleteEvents.InfoAvailableEvent(new(itemInfo));
            }
            if (ReplenishEvents is not null)
            {
                return ReplenishEvents.InfoAvailableEvent(new(itemInfo));
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(InfoAvailableEvent));
        }

        protected IDispenseEvents DispenseEvents { get; init; } = null;
        protected IPresentEvents PresentEvents { get; init; } = null;
        protected IRejectEvents RejectEvents { get; init; } = null;
        protected ITestCashUnitsEvents TestCashUnitsEvents { get; init; } = null;
        protected ICountEvents CountEvents { get; init; } = null;
        protected IRetractEvents RetractEvents { get; init; } = null;
        protected IResetEvents ResetEvents { get; init; } = null;
        protected ICalibrateCashUnitEvents CalibrateCashUnitEvents { get; init; } = null;
        protected ICashInEvents CashInEvents { get; init; } = null;
        protected ICashInEndEvents CashInEndEvents { get; init; } = null;
        protected ICashInRollbackEvents CashInRollbackEvents { get; init; } = null;
        protected ICreateSignatureEvents CreateSignatureEvents { get; init; } = null;
        protected IPreparePresentEvents PreparePresentEvents { get; init; } = null;
        protected ICashUnitCountEvents CashUnitCountEvents { get; init; } = null;
        protected IDepleteEvents DepleteEvents { get; init; } = null;
        protected IReplenishEvents ReplenishEvents { get; init; } = null;
    }
}
