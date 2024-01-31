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
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashDispenser;
using XFS4IoTFramework.CashAcceptor;

namespace XFS4IoTFramework.CashManagement
{
    public abstract class ItemErrorCommandEvents : ItemInfoAvailableCommandEvent
    {
        public enum ItemErrorReasonEnum
        {
            DoubleNote,
            LongNote,
            SkewedNote,
            IncorrectCount,
            NotesTooClose,
            OtherNoteError,
            ShortNote
        }

        public ItemErrorCommandEvents(IDispenseEvents events) :
            base(events)
        { }
        public ItemErrorCommandEvents(ITestCashUnitsEvents events) :
            base(events)
        { }
        public ItemErrorCommandEvents(ICountEvents events) :
            base(events)
        { }
        public ItemErrorCommandEvents(ICalibrateCashUnitEvents events) :
            base(events)
        { }
        public ItemErrorCommandEvents(IRetractEvents events) :
            base(events)
        { }
        public ItemErrorCommandEvents(ICashInEvents events) :
            base(events)
        { }
        public ItemErrorCommandEvents(ICashInEndEvents events) :
            base(events)
        { }
        public ItemErrorCommandEvents(ICreateSignatureEvents events) :
            base(events)
        { }
        public ItemErrorCommandEvents(ICashUnitCountEvents events) :
            base(events)
        { }
        public ItemErrorCommandEvents(IDepleteEvents events) :
            base(events)
        { }
        public ItemErrorCommandEvents(IReplenishEvents events) :
            base(events)
        { }
        public Task NoteErrorEvent(ItemErrorReasonEnum Reason)
        {
            XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData payload = new(
                    Reason switch
                    {
                        ItemErrorReasonEnum.DoubleNote => XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData.ReasonEnum.DoubleNote,
                        ItemErrorReasonEnum.IncorrectCount => XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData.ReasonEnum.IncorrectCount,
                        ItemErrorReasonEnum.LongNote => XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData.ReasonEnum.LongNote,
                        ItemErrorReasonEnum.NotesTooClose => XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData.ReasonEnum.NotesTooClose,
                        ItemErrorReasonEnum.OtherNoteError => XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData.ReasonEnum.OtherNoteError,
                        ItemErrorReasonEnum.ShortNote => XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData.ReasonEnum.ShortNote,
                        ItemErrorReasonEnum.SkewedNote => XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData.ReasonEnum.SkewedNote,
                        _ => null,
                    }
                );

            if (DispenseEvents is not null)
            {
                return DispenseEvents.NoteErrorEvent(payload);
            }
            if (TestCashUnitsEvents is not null)
            {
                return TestCashUnitsEvents.NoteErrorEvent(payload);
            }
            if (CountEvents is not null)
            {
                return CountEvents.NoteErrorEvent(payload);
            }
            if (CalibrateCashUnitEvents is not null)
            {
                return CalibrateCashUnitEvents.NoteErrorEvent(payload);
            }
            if (RetractEvents is not null)
            {
                return RetractEvents.NoteErrorEvent(payload);
            }
            if (CashInEvents is not null)
            {
                return CashInEvents.NoteErrorEvent(payload);
            }
            if (CreateSignatureEvents is not null)
            {
                return CreateSignatureEvents.NoteErrorEvent(payload);
            }
            if (CashUnitCountEvents is not null)
            {
                return CashUnitCountEvents.NoteErrorEvent(payload);
            }
            if (DepleteEvents is not null)
            {
                return DepleteEvents.NoteErrorEvent(payload);
            }
            if (ReplenishEvents is not null)
            {
                return ReplenishEvents.NoteErrorEvent(payload);
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(NoteErrorEvent));
        }
    }
}
