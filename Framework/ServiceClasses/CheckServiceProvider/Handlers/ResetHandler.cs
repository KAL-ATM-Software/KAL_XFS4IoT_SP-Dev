/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Check.Commands;
using XFS4IoT.Check.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.Check
{
    public partial class ResetHandler
    {
        private async Task<ResetCompletion.PayloadData> HandleReset(IResetEvents events, ResetCommand reset, CancellationToken cancel)
        {
            ResetDeviceRequest.MediaControlEnum mediaControl = ResetDeviceRequest.MediaControlEnum.Default;
            string storageId = string.Empty;

            if (!string.IsNullOrEmpty(reset.Payload.MediaControl))
            {
                if (!Regex.IsMatch(reset.Payload.MediaControl, "^eject$|^transport$|^rebuncher$|^unit[0-9A-Za-z]+$"))
                {
                    return new ResetCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.InvalidData,
                        $"Specified media control is invalid. {reset.Payload.MediaControl}");
                }
                switch (reset.Payload.MediaControl)
                {
                    case "eject":
                        mediaControl = ResetDeviceRequest.MediaControlEnum.Eject;
                        break;
                    case "transport":
                        mediaControl = ResetDeviceRequest.MediaControlEnum.Transport;
                        break;
                    case "rebuncher":
                        mediaControl = ResetDeviceRequest.MediaControlEnum.ReBuncher;
                        break;
                    default:
                        mediaControl = ResetDeviceRequest.MediaControlEnum.Unit;
                        storageId = reset.Payload.MediaControl;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(storageId))
            {
                if (!Storage.CheckUnits.ContainsKey(storageId))
                {
                    return new ResetCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.CommandErrorCode,
                        $"Specified storage unit doesn't exist. {storageId}",
                        ResetCompletion.PayloadData.ErrorCodeEnum.InvalidBin);
                }
            }

            if (mediaControl == ResetDeviceRequest.MediaControlEnum.Eject &&
                !Common.CheckScannerCapabilities.ResetControls.HasFlag(CheckScannerCapabilitiesClass.ResetControlEnum.Eject))
            {
                return new ResetCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.InvalidData,
                        $"Specified unsupported media control. Check ResetControls capability reported. {mediaControl}");
            }
            if (mediaControl == ResetDeviceRequest.MediaControlEnum.Unit &&
                !Common.CheckScannerCapabilities.ResetControls.HasFlag(CheckScannerCapabilitiesClass.ResetControlEnum.Storage))
            {
                return new ResetCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.InvalidData,
                        $"Specified unsupported media control. Check ResetControls capability reported. {mediaControl}");
            }
            if (mediaControl == ResetDeviceRequest.MediaControlEnum.Transport &&
                !Common.CheckScannerCapabilities.ResetControls.HasFlag(CheckScannerCapabilitiesClass.ResetControlEnum.Transport))
            {
                return new ResetCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.InvalidData,
                        $"Specified unsupported media control. Check ResetControls capability reported. {mediaControl}");
            }
            if (mediaControl == ResetDeviceRequest.MediaControlEnum.ReBuncher &&
                !Common.CheckScannerCapabilities.ResetControls.HasFlag(CheckScannerCapabilitiesClass.ResetControlEnum.ReBuncher))
            {
                return new ResetCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.InvalidData,
                        $"Specified unsupported media control. Check ResetControls capability reported. {mediaControl}");
            }

            Logger.Log(Constants.DeviceClass, "CheckDev.ResetAsync()");

            var result = await Device.ResetAsync(
                events: new(Storage, events), 
                request: new(mediaControl, storageId), 
                cancellation: cancel);

            Logger.Log(Constants.DeviceClass, $"CheckDev.ResetAsync() -> {result.CompletionCode}");

            if (Check.LastTransactionStatus.MediaInTransactionState == TransactionStatus.MediaInTransactionStateEnum.Active)
            {
                if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
                {
                    Check.LastTransactionStatus.MediaInTransactionState = TransactionStatus.MediaInTransactionStateEnum.Reset;
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

            return new ResetCompletion.PayloadData(
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription,
                ErrorCode: result.ErrorCode);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
