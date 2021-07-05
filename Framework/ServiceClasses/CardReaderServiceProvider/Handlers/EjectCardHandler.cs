/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CardReader
{
    public partial class EjectCardHandler
    {
        private async Task<EjectCardCompletion.PayloadData> HandleEjectCard(IEjectCardEvents events, EjectCardCommand ejectCard, CancellationToken cancel)
        {
            if (ejectCard.Payload.EjectPosition is not null)
            {
                // check capability
                if (ejectCard.Payload.EjectPosition == EjectCardCommand.PayloadData.EjectPositionEnum.ExitPosition &&
                    !CardReader.CardReaderCapabilities.EjectPosition.HasFlag(CardReaderCapabilitiesClass.EjectPositionsEnum.Exit) ||
                    ejectCard.Payload.EjectPosition == EjectCardCommand.PayloadData.EjectPositionEnum.TransportPosition &&
                    !CardReader.CardReaderCapabilities.EjectPosition.HasFlag(CardReaderCapabilitiesClass.EjectPositionsEnum.Transport))
                {
                    return new EjectCardCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Supplied eject position is not supported. {ejectCard.Payload.EjectPosition}");
                }
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.EjectCardAsync()");
            var result = await Device.EjectCardAsync(new EjectCardRequest(ejectCard.Payload.EjectPosition),
                                                     cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.EjectCardAsync() -> {result.CompletionCode}");

            return new EjectCardCompletion.PayloadData(result.CompletionCode,
                                                       result.ErrorDescription,
                                                       result.ErrorCode);
        }

    }
}
