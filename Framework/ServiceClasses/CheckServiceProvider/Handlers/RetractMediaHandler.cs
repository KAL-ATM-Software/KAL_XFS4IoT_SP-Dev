/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Check.Commands;
using XFS4IoT.Check.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.Check
{
    public partial class RetractMediaHandler
    {
        private async Task<CommandResult<RetractMediaCompletion.PayloadData>> HandleRetractMedia(IRetractMediaEvents events, RetractMediaCommand retractMedia, CancellationToken cancel)
        {
            RetractMediaRequest.LocationEnum location = RetractMediaRequest.LocationEnum.Default;
            string storageId = string.Empty;

            if (!string.IsNullOrEmpty(retractMedia.Payload.RetractLocation))
            {
                if (!Regex.IsMatch(retractMedia.Payload.RetractLocation, "^stacker$|^transport$|^rebuncher$|^unit[0-9A-Za-z]+$"))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified media control is not supported by the device.{retractMedia.Payload.RetractLocation}");
                }
                switch (retractMedia.Payload.RetractLocation)
                {
                    case "stacker":
                        location = RetractMediaRequest.LocationEnum.Stacker;
                        break;
                    case "transport":
                        location = RetractMediaRequest.LocationEnum.Transport;
                        break;
                    case "rebuncher":
                        location = RetractMediaRequest.LocationEnum.ReBuncher;
                        break;
                    default:
                        location = RetractMediaRequest.LocationEnum.Unit;
                        storageId = retractMedia.Payload.RetractLocation;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(storageId))
            {
                if (!Storage.CheckUnits.ContainsKey(storageId))
                {
                    return new(
                        new(RetractMediaCompletion.PayloadData.ErrorCodeEnum.InvalidBin),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified storage unit doesn't exist. {storageId}");
                }
            }

            if (location == RetractMediaRequest.LocationEnum.Stacker &&
                !Common.CheckScannerCapabilities.RetractLocations.HasFlag(CheckScannerCapabilitiesClass.RetractLocationEnum.Stacker))
            {
                return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified unsupported media control. Check ResetControls capability reported. {location}");
            }
            if (location == RetractMediaRequest.LocationEnum.Unit &&
                !Common.CheckScannerCapabilities.RetractLocations.HasFlag(CheckScannerCapabilitiesClass.RetractLocationEnum.Storage))
            {
                return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified unsupported media control. Check ResetControls capability reported. {location}");
            }
            if (location == RetractMediaRequest.LocationEnum.Transport &&
                !Common.CheckScannerCapabilities.RetractLocations.HasFlag(CheckScannerCapabilitiesClass.RetractLocationEnum.Transport))
            {
                return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified unsupported media control. Check ResetControls capability reported. {location}");
            }
            if (location == RetractMediaRequest.LocationEnum.ReBuncher &&
                !Common.CheckScannerCapabilities.RetractLocations.HasFlag(CheckScannerCapabilitiesClass.RetractLocationEnum.ReBuncher))
            {
                return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified unsupported media control. Check ResetControls capability reported. {location}");
            }

            Logger.Log(Constants.DeviceClass, "CheckDev.RetractMediaAsync()");

            var result = await Device.RetractMediaAsync(
                events: new(Storage, events),
                request: new(location, storageId),
                cancellation: cancel);

            Logger.Log(Constants.DeviceClass, $"CheckDev.RetractMediaAsync() -> {result.CompletionCode}");

            if (Check.LastTransactionStatus.MediaInTransactionState == TransactionStatus.MediaInTransactionStateEnum.Active)
            {
                if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
                {
                    Check.LastTransactionStatus.MediaInTransactionState = TransactionStatus.MediaInTransactionStateEnum.Retract;
                }
                else
                {
                    Check.LastTransactionStatus.MediaInTransactionState = TransactionStatus.MediaInTransactionStateEnum.Failure;
                }
                Check.StoreTransactionStatus();
            }

            Dictionary<string, StorageCheckCountClass> countDelta = [];
            if (result.MovementResult is not null)
            {
                foreach (var storage in result.MovementResult)
                {
                    countDelta.Add(storage.Key, new(0, storage.Value.Count, storage.Value.MediaRetracted ? 1 : 0));
                }
            }

            // Update internal check counts and send associated events.
            if (countDelta.Count > 0)
            {
                await Storage.UpdateCheckStorageCount(countDelta);
            }

            return new(
                result.ErrorCode is not null ? new(ErrorCode: result.ErrorCode) : null,
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
