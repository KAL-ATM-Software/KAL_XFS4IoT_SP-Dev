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
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashDispenser;

namespace XFS4IoTFramework.CashManagement
{
    public class ItemErrorCommandEvents : ItemInfoAvailableCommandEvent
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

            throw new InvalidOperationException($"Unreachable code. " + nameof(NoteErrorEvent));
        }
    }
}
