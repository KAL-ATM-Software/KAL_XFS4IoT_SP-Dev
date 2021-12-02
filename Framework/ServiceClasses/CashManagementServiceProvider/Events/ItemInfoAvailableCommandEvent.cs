/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.CashDispenser;

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
    }
}
