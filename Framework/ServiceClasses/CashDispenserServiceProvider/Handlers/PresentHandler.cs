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
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoT.CashManagement;
using XFS4IoTFramework.Common;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.CashDispenser
{
    public partial class PresentHandler
    {
        private async Task<PresentCompletion.PayloadData> HandlePresent(IPresentEvents events, PresentCommand present, CancellationToken cancel)
        {
            CashDispenserCapabilitiesClass.OutputPositionEnum position = CashDispenserCapabilitiesClass.OutputPositionEnum.Default;
            if (present.Payload.Position is not null)
            {
                position = present.Payload.Position switch
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
                return new PresentCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                         $"Invalid position specified. {position}");
            }

            if (!CashDispenser.CashDispenserCapabilities.OutputPositions.HasFlag(position))
            {
                return new PresentCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                         $"Unsupported position specified. {position}",
                                                         PresentCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition);
            }
                
            Logger.Log(Constants.DeviceClass, "CashDispenserDev.PresentCashAsync()");

            var result = await Device.PresentCashAsync(events, new PresentCashRequest(position), cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.PresentCashAsync() -> {result.CompletionCode}, {result.ErrorCode}");


            PresentStatus presentStatus = null;
            try
            {
                Logger.Log(Constants.DeviceClass, "CashDispenserDev.GetPresentStatus()");

                presentStatus = Device.GetPresentStatus(position);

                Logger.Log(Constants.DeviceClass, $"CashDispenserDev.GetPresentStatus() -> {presentStatus}");
            }
            catch (NotImplementedException)
            {
                Logger.Log(Constants.DeviceClass, $"CashDispenserDev.GetPresentStatus() -> Not implemented");
            }
            catch (Exception)
            {
                throw;
            }

            // Update an internal present status
            CashDispenser.LastPresentStatus[position].Status = result.CompletionCode switch
            {
                MessagePayload.CompletionCodeEnum.Success => PresentStatus.PresentStatusEnum.Presented,
                _ => PresentStatus.PresentStatusEnum.Unknown
            };

            if (presentStatus is not null)
            {
                CashDispenser.LastPresentStatus[position].Status = (PresentStatus.PresentStatusEnum)presentStatus.Status;

                if (presentStatus.LastDenomination is not null)
                    CashDispenser.LastPresentStatus[position].LastDenomination = presentStatus.LastDenomination;

                CashDispenser.LastPresentStatus[position].Token = presentStatus.Token;
            }

            await CashDispenser.UpdateCashAccounting(result.MovementResult);

            PositionEnum resPostion = position switch
            {
                CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom => PositionEnum.OutBottom,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Center => PositionEnum.OutCenter,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Front => PositionEnum.OutFront,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Left => PositionEnum.OutLeft,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Rear => PositionEnum.OutRear,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Right => PositionEnum.OutRight,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Top => PositionEnum.OutTop,
                _ => PositionEnum.OutDefault
            };

            return new PresentCompletion.PayloadData(result.CompletionCode,
                                                     result.ErrorDescription,
                                                     result.ErrorCode,
                                                     resPostion,
                                                     result.NumBunchesRemaining < 0 ? "unknown" : result.NumBunchesRemaining.ToString());
        }
    }
}
