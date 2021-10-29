/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashDispenser;
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoT.CashManagement;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashDispenser
{
    public partial class GetPresentStatusHandler
    {
        private Task<GetPresentStatusCompletion.PayloadData> HandleGetPresentStatus(IGetPresentStatusEvents events, GetPresentStatusCommand getPresentStatus, CancellationToken cancel)
        {
            CashDispenserCapabilitiesClass.OutputPositionEnum position = CashDispenserCapabilitiesClass.OutputPositionEnum.Default;
            if (getPresentStatus.Payload.Position is not null)
            {
                position = getPresentStatus.Payload.Position switch
                {
                    OutputPositionEnum.OutBottom => CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom,
                    OutputPositionEnum.OutCenter => CashDispenserCapabilitiesClass.OutputPositionEnum.Center,
                    OutputPositionEnum.OutDefault => CashDispenserCapabilitiesClass.OutputPositionEnum.Default,
                    OutputPositionEnum.OutFront => CashDispenserCapabilitiesClass.OutputPositionEnum.Front,
                    OutputPositionEnum.OutLeft => CashDispenserCapabilitiesClass.OutputPositionEnum.Left,
                    OutputPositionEnum.OutRear => CashDispenserCapabilitiesClass.OutputPositionEnum.Rear,
                    OutputPositionEnum.OutRight => CashDispenserCapabilitiesClass.OutputPositionEnum.Right,
                    OutputPositionEnum.OutTop => CashDispenserCapabilitiesClass.OutputPositionEnum.Top,
                    _ => CashDispenserCapabilitiesClass.OutputPositionEnum.NotSupported
                };
            }

            if (position == CashDispenserCapabilitiesClass.OutputPositionEnum.NotSupported)
            {
                return Task.FromResult(new GetPresentStatusCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                  $"Specified invalid position {position}"));
            }

            if (!CashDispenser.CashDispenserCapabilities.OutputPositions.HasFlag(position))
            {
                return Task.FromResult(new GetPresentStatusCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                                  $"Specified unsupported position {position}",
                                                                                  GetPresentStatusCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition));
            }

            CashDispenser.LastPresentStatus.ContainsKey(position).IsTrue($"Unexpected position is specified. {position}");

            return Task.FromResult(new GetPresentStatusCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                              null,
                                                                              null,
                                                                              new DenominationClass(
                                                                                  CashDispenser.LastPresentStatus[position].LastDenomination?.CurrencyAmounts,
                                                                                  CashDispenser.LastPresentStatus[position].LastDenomination?.Values),
                                                                              CashDispenser.LastPresentStatus[position].Status switch
                                                                              {
                                                                                  PresentStatus.PresentStatusEnum.NotPresented => GetPresentStatusCompletion.PayloadData.PresentStateEnum.NotPresented,
                                                                                  PresentStatus.PresentStatusEnum.Presented => GetPresentStatusCompletion.PayloadData.PresentStateEnum.Presented,
                                                                                  _ => GetPresentStatusCompletion.PayloadData.PresentStateEnum.Unknown
                                                                              },
                                                                              CashDispenser.LastPresentStatus[position].Token));
        }
    }
}
