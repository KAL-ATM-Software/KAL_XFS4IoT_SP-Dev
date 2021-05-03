/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.Completions;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;

namespace XFS4IoTFramework.CardReader
{
    public partial class ChipPowerHandler
    {
        private async Task<ChipPowerCompletion.PayloadData> HandleChipPower(IChipPowerEvents events, ChipPowerCommand chipPower, CancellationToken cancel)
        {
            if (chipPower.Payload.ChipPower is null)
            {
                return new ChipPowerCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           "No chip power action supplied.");
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.ChipPowerAsync()");
            var result = await Device.ChipPowerAsync(events,
                                                     new ChipPowerRequest((ChipPowerCommand.PayloadData.ChipPowerEnum)chipPower.Payload.ChipPower),
                                                     cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ChipPowerAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new ChipPowerCompletion.PayloadData(result.CompletionCode,
                                                       result.ErrorDescription,
                                                       result.ErrorCode);
        }

    }
}
