/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.Completions;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using XFS4IoTFramework.Common;
using XFS4IoT;

namespace XFS4IoTFramework.CardReader
{
    public partial class ChipPowerHandler
    {
        private async Task<CommandResult<ChipPowerCompletion.PayloadData>> HandleChipPower(IChipPowerEvents events, ChipPowerCommand chipPower, CancellationToken cancel)
        {
            if (chipPower.Payload.ChipPower is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    "No chip power action supplied.");
            }

            // check capability
            if (chipPower.Payload.ChipPower == ChipPowerCommand.PayloadData.ChipPowerEnum.Cold &&
                !Common.CardReaderCapabilities.ChipPower.HasFlag(CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Cold) ||
                chipPower.Payload.ChipPower == ChipPowerCommand.PayloadData.ChipPowerEnum.Warm &&
                !Common.CardReaderCapabilities.ChipPower.HasFlag(CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Warm) ||
                chipPower.Payload.ChipPower == ChipPowerCommand.PayloadData.ChipPowerEnum.Off &&
                !Common.CardReaderCapabilities.ChipPower.HasFlag(CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Off))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Supplied chip power action supported. {chipPower.Payload.ChipPower}");
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.ChipPowerAsync()");
            var result = await Device.ChipPowerAsync(new ChipPowerRequest((ChipPowerCommand.PayloadData.ChipPowerEnum)chipPower.Payload.ChipPower),
                                                     cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ChipPowerAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            ChipPowerCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.ChipATRData?.Count > 0)
            {
                payload = new(
                    ErrorCode: result.ErrorCode,
                    ChipData: result.ChipATRData);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }

    }
}
