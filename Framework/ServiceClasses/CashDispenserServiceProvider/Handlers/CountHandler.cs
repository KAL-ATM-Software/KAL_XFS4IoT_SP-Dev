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
    public partial class CountHandler
    {
        private async Task<CountCompletion.PayloadData> HandleCount(ICountEvents events, CountCommand count, CancellationToken cancel)
        {
            CashDispenserCapabilitiesClass.OutputPositionEnum position = CashDispenserCapabilitiesClass.OutputPositionEnum.Default;
            if (count.Payload.Position is not null)
            {
                position = count.Payload.Position switch
                {
                    CountCommand.PayloadData.PositionEnum.Bottom => CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom,
                    CountCommand.PayloadData.PositionEnum.Center => CashDispenserCapabilitiesClass.OutputPositionEnum.Center,
                    CountCommand.PayloadData.PositionEnum.Default => CashDispenserCapabilitiesClass.OutputPositionEnum.Default,
                    CountCommand.PayloadData.PositionEnum.Front => CashDispenserCapabilitiesClass.OutputPositionEnum.Front,
                    CountCommand.PayloadData.PositionEnum.Left => CashDispenserCapabilitiesClass.OutputPositionEnum.Left,
                    CountCommand.PayloadData.PositionEnum.Rear => CashDispenserCapabilitiesClass.OutputPositionEnum.Rear,
                    CountCommand.PayloadData.PositionEnum.Right => CashDispenserCapabilitiesClass.OutputPositionEnum.Right,
                    CountCommand.PayloadData.PositionEnum.Top => CashDispenserCapabilitiesClass.OutputPositionEnum.Top,
                    _ => CashDispenserCapabilitiesClass.OutputPositionEnum.Default
                };
            }

            CashDispenser.CashDispenserCapabilities.OutputPositons.ContainsKey(position).IsTrue($"Unsupported position specified. {position}");

            if (!CashDispenser.CashDispenserCapabilities.OutputPositons[position])
            {
                return new CountCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                       $"Unsupported position. {position}");
            }

            CountRequest request = new (position);
            if (!string.IsNullOrEmpty(count.Payload.PhysicalPositionName))
            {
                request = new CountRequest(position, count.Payload.PhysicalPositionName);
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.CountAsync()");

            var result = await Device.CountAsync(events, request, cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.CountAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            CashDispenser.UpdateCashUnitAccounting(result.MovementResult);

            return new CountCompletion.PayloadData(result.CompletionCode, 
                                                   result.ErrorDescription, 
                                                   result.ErrorCode);
        }
    }
}
