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
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Storage;
using XFS4IoT.CashAcceptor.Events;

namespace XFS4IoTFramework.CashAcceptor
{
    public abstract class CashInCommonCommandEvents : ItemErrorCommandEvents
    {
        public enum RefusedReasonEnum
        {
            CashInUnitFull,
            InvalidBill,
            NoBillsToDeposit,
            DepositFailure,
            CommonInputComponentFailure,
            StackerFull,
            ForeignItemsDetected,
            InvalidBunch,
            Counterfeit,
            LimitOverTotalItems,
            LimitOverAmount
        }

        public CashInCommonCommandEvents(ICashInEvents events) :
            base(events)
        { }
        public CashInCommonCommandEvents(ICreateSignatureEvents events) :
            base(events)
        { }

        public Task InsertItemsEvent()
        {
            if (CashInEvents is not null)
            {
                return CashInEvents.InsertItemsEvent();
            }
            if (CreateSignatureEvents is not null)
            {
                return CreateSignatureEvents.InsertItemsEvent();
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(InsertItemsEvent));
        }

        public Task InputRefuseEvent(RefusedReasonEnum reason)
        {
            InputRefuseEvent.PayloadData payload = new InputRefuseEvent.PayloadData(reason switch
            {
                RefusedReasonEnum.CashInUnitFull => XFS4IoT.CashAcceptor.Events.InputRefuseEvent.PayloadData.ReasonEnum.CashInUnitFull,
                RefusedReasonEnum.InvalidBill => XFS4IoT.CashAcceptor.Events.InputRefuseEvent.PayloadData.ReasonEnum.InvalidBill,
                RefusedReasonEnum.NoBillsToDeposit => XFS4IoT.CashAcceptor.Events.InputRefuseEvent.PayloadData.ReasonEnum.NoBillsToDeposit,
                RefusedReasonEnum.DepositFailure => XFS4IoT.CashAcceptor.Events.InputRefuseEvent.PayloadData.ReasonEnum.DepositFailure,
                RefusedReasonEnum.CommonInputComponentFailure => XFS4IoT.CashAcceptor.Events.InputRefuseEvent.PayloadData.ReasonEnum.CommonInputComponentFailure,
                RefusedReasonEnum.StackerFull => XFS4IoT.CashAcceptor.Events.InputRefuseEvent.PayloadData.ReasonEnum.StackerFull,
                RefusedReasonEnum.ForeignItemsDetected => XFS4IoT.CashAcceptor.Events.InputRefuseEvent.PayloadData.ReasonEnum.ForeignItemsDetected,
                RefusedReasonEnum.InvalidBunch => XFS4IoT.CashAcceptor.Events.InputRefuseEvent.PayloadData.ReasonEnum.InvalidBunch,
                RefusedReasonEnum.Counterfeit => XFS4IoT.CashAcceptor.Events.InputRefuseEvent.PayloadData.ReasonEnum.Counterfeit,
                RefusedReasonEnum.LimitOverTotalItems => XFS4IoT.CashAcceptor.Events.InputRefuseEvent.PayloadData.ReasonEnum.LimitOverTotalItems,
                _ => XFS4IoT.CashAcceptor.Events.InputRefuseEvent.PayloadData.ReasonEnum.LimitOverAmount,
            });

            if (CashInEvents is not null)
            {
                return CashInEvents.InputRefuseEvent(payload);
            }
            if (CreateSignatureEvents is not null)
            {
                return CreateSignatureEvents.InputRefuseEvent(payload);
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(InputRefuseEvent));
        }
    }
}
