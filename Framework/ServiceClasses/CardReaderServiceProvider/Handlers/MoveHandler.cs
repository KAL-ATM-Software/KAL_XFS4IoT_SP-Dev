/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;

namespace XFS4IoTFramework.CardReader
{
    public partial class MoveHandler
    {
        private async Task<MoveCompletion.PayloadData> HandleMove(IMoveEvents events, MoveCommand move, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(move.Payload.From))
            {
                return new MoveCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                      "The property From is not specified.");
            }

            MoveCardRequest.MovePosition.MovePositionEnum fromPos = MoveCardRequest.MovePosition.MovePositionEnum.Storage;

            // First to check the specified storage is valid
            if (move.Payload.From != "exit" &&
                move.Payload.From != "transport")
            {
                if (!CardReader.CardUnits.ContainsKey(move.Payload.From))
                {
                    return new MoveCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"Invalid StorageId supplied for From property. {move.Payload.From}");
                }
            }
            else
            {
                if (move.Payload.From == "exit")
                    fromPos = MoveCardRequest.MovePosition.MovePositionEnum.Exit;
                else
                    fromPos = MoveCardRequest.MovePosition.MovePositionEnum.Transport;
            }

            if (string.IsNullOrEmpty(move.Payload.To))
            {
                return new MoveCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                      "The property To is not specified.");
            }

            MoveCardRequest.MovePosition.MovePositionEnum toPos = MoveCardRequest.MovePosition.MovePositionEnum.Storage;
            if (move.Payload.To != "exit" &&
                move.Payload.To != "transport")
            {
                if (!CardReader.CardUnits.ContainsKey(move.Payload.To))
                {
                    return new MoveCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"Invalid StorageId supplied for To property. {move.Payload.To}");
                }
            }
            else
            {
                if (move.Payload.To == "exit")
                    toPos = MoveCardRequest.MovePosition.MovePositionEnum.Exit;
                else
                    toPos = MoveCardRequest.MovePosition.MovePositionEnum.Transport;
            }

            // Check parameters for the type of card reader
            if (CardReader.CardReaderCapabilities.Type == Common.CardReaderCapabilitiesClass.DeviceTypeEnum.Contactless ||
                CardReader.CardReaderCapabilities.Type == Common.CardReaderCapabilitiesClass.DeviceTypeEnum.IntelligentContactless ||
                CardReader.CardReaderCapabilities.Type == Common.CardReaderCapabilitiesClass.DeviceTypeEnum.LatchedDip)
            {
                if (toPos == MoveCardRequest.MovePosition.MovePositionEnum.Storage ||
                    fromPos == MoveCardRequest.MovePosition.MovePositionEnum.Storage)
                {
                    return new MoveCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"Invalid location specified to or from move mdeia as the contractless cardreader won't have a retain bin or dispensing unit. {move.Payload.To}");
                }
                // passing other cases to the device class as may need to turn off RFID or unlatch card
            }
            else if (CardReader.CardReaderCapabilities.Type == Common.CardReaderCapabilitiesClass.DeviceTypeEnum.Swipe ||
                     CardReader.CardReaderCapabilities.Type == Common.CardReaderCapabilitiesClass.DeviceTypeEnum.Dip ||
                     CardReader.CardReaderCapabilities.Type == Common.CardReaderCapabilitiesClass.DeviceTypeEnum.Permanent)
            {
                if (!string.IsNullOrEmpty(move.Payload.To) ||
                    !string.IsNullOrEmpty(move.Payload.From))
                {
                    return new MoveCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"Invalid location specified for card reader type. The DIP, swipe or permanent card can't control media to move.  {CardReader.CardReaderCapabilities.Type} To:{move.Payload.To} From:{move.Payload.From}");
                }
            }

            MoveCardRequest.MovePosition from = new (fromPos, 
                                                     fromPos == MoveCardRequest.MovePosition.MovePositionEnum.Storage ? move.Payload.From : null);
            
            MoveCardRequest.MovePosition to = new (toPos, 
                                                   toPos == MoveCardRequest.MovePosition.MovePositionEnum.Storage ? move.Payload.To : null);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.MoveCardAsync()");
            var result = await Device.MoveCardAsync(events,
                                                    new MoveCardRequest(from, to),
                                                    cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.MoveCardAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                // Update counters and save persistently
                if (to.Position == MoveCardRequest.MovePosition.MovePositionEnum.Storage)
                {
                    string storageId = to.StorageId;
                    if (string.IsNullOrEmpty(to.StorageId))
                    {
                        // Default position and the device class most report storage id
                        storageId = result.StorageId;
                    }

                    if (string.IsNullOrEmpty(storageId))
                    {
                        Logger.Warning(Constants.Framework, $"The device class returned an empty storage ID for the default position. the counter for the storage won't be updated.");
                    }
                    else
                    {
                        await CardReader.UpdateCardStorageCount(result.StorageId, result.CountMoved);
                    }
                }
            }

            return new MoveCompletion.PayloadData(result.CompletionCode,
                                                  result.ErrorDescription,
                                                  result.ErrorCode);
        }
    }
}
