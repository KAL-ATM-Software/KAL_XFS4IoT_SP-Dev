/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.Completions;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;
using XFS4IoT;

namespace XFS4IoTFramework.CardReader
{
    public partial class ResetHandler
    {
        private async Task<ResetCompletion.PayloadData> HandleReset(IResetEvents events, ResetCommand reset, CancellationToken cancel)
        {
            ResetDeviceRequest.ToEnum to = ResetDeviceRequest.ToEnum.Default;
            if (reset.Payload.To is not null)
            {
                to = reset.Payload.To switch
                {
                    ResetCommand.PayloadData.ToEnum.Exit => ResetDeviceRequest.ToEnum.Exit,
                    ResetCommand.PayloadData.ToEnum.Retain => ResetDeviceRequest.ToEnum.Retain,
                    ResetCommand.PayloadData.ToEnum.CurrentPosition => ResetDeviceRequest.ToEnum.CurrentPosition,
                    _ => ResetDeviceRequest.ToEnum.Default
                };
            }
            // Check to parameter with capabilities
            if (Common.CardReaderCapabilities.Type != CardReaderCapabilitiesClass.DeviceTypeEnum.Motor)
            {
                if (to != ResetDeviceRequest.ToEnum.Default &&
                    to != ResetDeviceRequest.ToEnum.CurrentPosition)
                {
                    return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"Invalid location specified for card reader type. The DIP, swipe or permanent card can't control media to move.  {Common.CardReaderCapabilities.Type} To:{reset.Payload.To}");
                }
            }

            // Check storage ID with capabilities
            if (!string.IsNullOrEmpty(reset.Payload.StorageId))
            {
                if (!Storage.CardUnits.ContainsKey(reset.Payload.StorageId))
                {
                    return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"Invalid StorageId supplied. {reset.Payload.StorageId}");
                }
            }
            else
            {
                if (Common.CardReaderCapabilities.Type != CardReaderCapabilitiesClass.DeviceTypeEnum.Motor)
                {
                     return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                            $"Card reader type {reset.Payload.StorageId} is not supporting storage.");
                }
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.ResetDeviceAsync()");
            
            var result = await Device.ResetDeviceAsync(new ResetCommandEvents(events),
                                                       new ResetDeviceRequest(to, reset.Payload.StorageId),
                                                       cancel);
            
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ResetDeviceAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                if (!string.IsNullOrEmpty(reset.Payload.StorageId))
                {
                    if (to == ResetDeviceRequest.ToEnum.Retain &&
                        Storage.CardUnits.ContainsKey(reset.Payload.StorageId) &&
                        Storage.CardUnits[reset.Payload.StorageId].Unit.Capabilities.Type == CardCapabilitiesClass.TypeEnum.Retain)
                    {
                        // Update counters and save persistently
                        await Storage.UpdateCardStorageCount(reset.Payload.StorageId, result.CountMoved);
                    }
                    else if (to == ResetDeviceRequest.ToEnum.Default)
                    {
                        if (string.IsNullOrEmpty(result.StorageId))
                        {
                            Logger.Warning(Constants.Framework, $"The device class returned an empty storage ID for the default position. the counter for the storage won't be updated.");
                        }
                        else
                        {
                            // Update counters and save persistently
                            await Storage.UpdateCardStorageCount(result.StorageId, result.CountMoved);
                        }
                    }
                }
            }

            return new ResetCompletion.PayloadData(result.CompletionCode,
                                                   result.ErrorDescription,
                                                   result.ErrorCode);
        }
        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
