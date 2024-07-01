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
            if (reset.Payload?.To is not null)
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

            string storageId = string.Empty;
            // Check storage ID with capabilities
            if (to == ResetDeviceRequest.ToEnum.Retain)
            {
                if (!string.IsNullOrEmpty(reset.Payload?.StorageId))
                {
                    if (Common.CardReaderCapabilities.Type != CardReaderCapabilitiesClass.DeviceTypeEnum.Motor)
                    {
                        return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Card reader type {Common.CardReaderCapabilities.Type} is not supporting storage.");
                    }

                    storageId = reset.Payload.StorageId;
                    if (!Storage.CardUnits.ContainsKey(storageId))
                    {
                        return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Invalid StorageId supplied. {reset.Payload.StorageId}");
                    }
                }
                // if the storage id is null, device class can decide the location
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.ResetDeviceAsync()");
            
            var result = await Device.ResetDeviceAsync(new ResetDeviceRequest(to, storageId),
                                                       cancel);
            
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ResetDeviceAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                string storageIdMediaMoved = storageId;
                if (string.IsNullOrEmpty(storageIdMediaMoved))
                {
                    // Use one storage id reported by the SP
                    storageIdMediaMoved = result.StorageId;
                }

                if (!string.IsNullOrEmpty(storageIdMediaMoved))
                {
                    if (!Storage.CardUnits.ContainsKey(storageIdMediaMoved))
                    {
                        Logger.Warning(Constants.Framework, $"The device class reported invalid storage ID. {storageIdMediaMoved}");
                    }
                    else
                    {
                        if (to == ResetDeviceRequest.ToEnum.Retain &&
                            Storage.CardUnits[storageIdMediaMoved].Unit.Capabilities.Type == CardCapabilitiesClass.TypeEnum.Retain)
                        {
                            Logger.Warning(Constants.Framework, $"The client asked to move media in the retain bin. but the device class stored media to different type of bin. {Storage.CardUnits[storageIdMediaMoved].Unit.Capabilities.Type}");
                        }
                        await Storage.UpdateCardStorageCount(storageIdMediaMoved, result.CountMoved);
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
