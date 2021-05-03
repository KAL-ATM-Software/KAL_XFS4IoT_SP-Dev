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
    public partial class ParkCardHandler
    {
        private async Task<ParkCardCompletion.PayloadData> HandleParkCard(IParkCardEvents events, ParkCardCommand parkCard, CancellationToken cancel)
        {
            if (parkCard.Payload.Direction is null)
            {
                return new ParkCardCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          "No chip IO data supplied.");
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.ParkCardAsync()");
            var result = await Device.ParkCardAsync(new ParkCardRequest(parkCard.Payload.Direction, parkCard.Payload.ParkingStation),
                                                    cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ParkCardAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new ParkCardCompletion.PayloadData(result.CompletionCode,
                                                      result.ErrorDescription,
                                                      result.ErrorCode);
        }

    }
}
