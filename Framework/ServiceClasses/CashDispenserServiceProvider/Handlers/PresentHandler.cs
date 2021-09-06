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
                    PresentCommand.PayloadData.PositionEnum.Bottom => CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom,
                    PresentCommand.PayloadData.PositionEnum.Center => CashDispenserCapabilitiesClass.OutputPositionEnum.Center,
                    PresentCommand.PayloadData.PositionEnum.Default => CashDispenserCapabilitiesClass.OutputPositionEnum.Default,
                    PresentCommand.PayloadData.PositionEnum.Front => CashDispenserCapabilitiesClass.OutputPositionEnum.Front,
                    PresentCommand.PayloadData.PositionEnum.Left => CashDispenserCapabilitiesClass.OutputPositionEnum.Left,
                    PresentCommand.PayloadData.PositionEnum.Rear => CashDispenserCapabilitiesClass.OutputPositionEnum.Rear,
                    PresentCommand.PayloadData.PositionEnum.Right => CashDispenserCapabilitiesClass.OutputPositionEnum.Right,
                    PresentCommand.PayloadData.PositionEnum.Top => CashDispenserCapabilitiesClass.OutputPositionEnum.Top,
                    _ => CashDispenserCapabilitiesClass.OutputPositionEnum.Default
                };
            }

            CashDispenser.CashDispenserCapabilities.OutputPositons.ContainsKey(position).IsTrue($"Unsupported position specified. {position}");

            if (!CashDispenser.CashDispenserCapabilities.OutputPositons[position])
            {
                return new PresentCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                         $"Unsupported position. {position}");
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

            CashDispenser.UpdateCashUnitAccounting(result.MovementResult);

            PresentCompletion.PayloadData.PositionEnum resPosition = position switch
            {
                CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom => PresentCompletion.PayloadData.PositionEnum.Bottom,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Center => PresentCompletion.PayloadData.PositionEnum.Center,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Front => PresentCompletion.PayloadData.PositionEnum.Front,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Left => PresentCompletion.PayloadData.PositionEnum.Left,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Rear => PresentCompletion.PayloadData.PositionEnum.Rear,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Right => PresentCompletion.PayloadData.PositionEnum.Right,
                CashDispenserCapabilitiesClass.OutputPositionEnum.Top => PresentCompletion.PayloadData.PositionEnum.Top,
                _ => PresentCompletion.PayloadData.PositionEnum.Default
            };

            PresentCompletion.PayloadData.AdditionalBunchesEnum additionalBunches = result.NumBunchesRemaining switch
            {
                0 => PresentCompletion.PayloadData.AdditionalBunchesEnum.None,
                < 0 => PresentCompletion.PayloadData.AdditionalBunchesEnum.Unknown,
                _ => PresentCompletion.PayloadData.AdditionalBunchesEnum.OneMore
            };

            return new PresentCompletion.PayloadData(result.CompletionCode,
                                                     result.ErrorDescription,
                                                     result.ErrorCode, 
                                                     resPosition, 
                                                     additionalBunches, 
                                                     result.NumBunchesRemaining >=0 ? result.NumBunchesRemaining : null);
        }
    }
}
