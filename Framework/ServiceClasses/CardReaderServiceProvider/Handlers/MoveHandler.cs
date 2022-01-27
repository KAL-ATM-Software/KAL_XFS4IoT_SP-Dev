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
using XFS4IoTFramework.Storage;
using XFS4IoTFramework.Common;

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

            MovePosition.MovePositionEnum fromPos = MovePosition.MovePositionEnum.Storage;

            // First to check the specified storage is valid
            if (move.Payload.From != "exit" &&
                move.Payload.From != "transport")
            {
                if (!Storage.CardUnits.ContainsKey(move.Payload.From))
                {
                    return new MoveCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"Invalid StorageId supplied for From property. {move.Payload.From}");
                }
            }
            else
            {
                if (move.Payload.From == "exit")
                    fromPos = MovePosition.MovePositionEnum.Exit;
                else
                    fromPos = MovePosition.MovePositionEnum.Transport;
            }

            if (string.IsNullOrEmpty(move.Payload.To))
            {
                return new MoveCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                      "The property To is not specified.");
            }

            MovePosition.MovePositionEnum toPos = MovePosition.MovePositionEnum.Storage;
            if (move.Payload.To != "exit" &&
                move.Payload.To != "transport")
            {
                if (!Storage.CardUnits.ContainsKey(move.Payload.To))
                {
                    return new MoveCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"Invalid StorageId supplied for To property. {move.Payload.To}");
                }
            }
            else
            {
                if (move.Payload.To == "exit")
                    toPos = MovePosition.MovePositionEnum.Exit;
                else
                    toPos = MovePosition.MovePositionEnum.Transport;
            }

            // Check parameters for the type of card reader
            if (Common.CardReaderCapabilities.Type == CardReaderCapabilitiesClass.DeviceTypeEnum.Contactless ||
                Common.CardReaderCapabilities.Type == CardReaderCapabilitiesClass.DeviceTypeEnum.IntelligentContactless ||
                Common.CardReaderCapabilities.Type == CardReaderCapabilitiesClass.DeviceTypeEnum.LatchedDip)
            {
                if (toPos == MovePosition.MovePositionEnum.Storage ||
                    fromPos == MovePosition.MovePositionEnum.Storage)
                {
                    return new MoveCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"Invalid location specified to or from move mdeia as the contractless cardreader won't have a retain bin or dispensing unit. {move.Payload.To}");
                }
                // passing other cases to the device class as may need to turn off RFID or unlatch card
            }
            else if (Common.CardReaderCapabilities.Type == CardReaderCapabilitiesClass.DeviceTypeEnum.Swipe ||
                     Common.CardReaderCapabilities.Type == CardReaderCapabilitiesClass.DeviceTypeEnum.Dip ||
                     Common.CardReaderCapabilities.Type == CardReaderCapabilitiesClass.DeviceTypeEnum.Permanent)
            {
                if (!string.IsNullOrEmpty(move.Payload.To) ||
                    !string.IsNullOrEmpty(move.Payload.From))
                {
                    return new MoveCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"Invalid location specified for card reader type. The DIP, swipe or permanent card can't control media to move.  {Common.CardReaderCapabilities.Type} To:{move.Payload.To} From:{move.Payload.From}");
                }
            }

            MovePosition from = new (fromPos, 
                                     fromPos == MovePosition.MovePositionEnum.Storage ? move.Payload.From : null);
            
            MovePosition to = new (toPos, 
                                   toPos == MovePosition.MovePositionEnum.Storage ? move.Payload.To : null);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.MoveCardAsync()");
            var result = await Device.MoveCardAsync(new MoveCardRequest(from, to),
                                                    cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.MoveCardAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                // Update counters and save persistently
                if (to.Position == MovePosition.MovePositionEnum.Storage)
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
                        await Storage.UpdateCardStorageCount(result.StorageId, result.CountMoved);
                    }
                }
            }

            return new MoveCompletion.PayloadData(result.CompletionCode,
                                                  result.ErrorDescription,
                                                  result.ErrorCode);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
