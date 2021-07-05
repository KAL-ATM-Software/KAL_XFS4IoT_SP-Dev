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
using XFS4IoTFramework.Common;

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

            // check capability
            if (chipPower.Payload.ChipPower == ChipPowerCommand.PayloadData.ChipPowerEnum.Cold &&
                !CardReader.CardReaderCapabilities.ChipPower.HasFlag(CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Cold) ||
                chipPower.Payload.ChipPower == ChipPowerCommand.PayloadData.ChipPowerEnum.Warm &&
                !CardReader.CardReaderCapabilities.ChipPower.HasFlag(CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Warm) ||
                chipPower.Payload.ChipPower == ChipPowerCommand.PayloadData.ChipPowerEnum.Off &&
                !CardReader.CardReaderCapabilities.ChipPower.HasFlag(CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Off))
            {
                return new ChipPowerCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"Supplied chip power action supported. {chipPower.Payload.ChipPower}");
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
