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
        public enum ReasonEnum
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

        public Task NoteErrorEvent(ReasonEnum Reason)
        {
            XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData payload = new(
                    Reason switch
                    {
                        ReasonEnum.DoubleNote => XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData.ReasonEnum.DoubleNote,
                        ReasonEnum.IncorrectCount => XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData.ReasonEnum.IncorrectCount,
                        ReasonEnum.LongNote => XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData.ReasonEnum.LongNote,
                        ReasonEnum.NotesTooClose => XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData.ReasonEnum.NotesTooClose,
                        ReasonEnum.OtherNoteError => XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData.ReasonEnum.OtherNoteError,
                        ReasonEnum.ShortNote => XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData.ReasonEnum.ShortNote,
                        ReasonEnum.SkewedNote => XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData.ReasonEnum.SkewedNote,
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
