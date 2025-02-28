/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoTFramework.Common;
using System.Text.RegularExpressions;

namespace XFS4IoTFramework.Printer
{
    public partial class ResetHandler
    {
        private async Task<CommandResult<ResetCompletion.PayloadData>> HandleReset(IResetEvents events, ResetCommand reset, CancellationToken cancel)
        {
            ResetDeviceRequest.MediaControlEnum mediaControl = ResetDeviceRequest.MediaControlEnum.Default;
            int binNumber = -1;
            if (!string.IsNullOrEmpty(reset.Payload.MediaControl))
            {
                if (!Regex.IsMatch(reset.Payload.MediaControl, "^eject$|^expel$|^unit[0-9]+$"))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified media control is not supported by the device.{reset.Payload.MediaControl}");
                }
                switch (reset.Payload.MediaControl)
                {
                    case "eject":
                        mediaControl = ResetDeviceRequest.MediaControlEnum.Eject;
                        break;
                    case "expel":
                        mediaControl = ResetDeviceRequest.MediaControlEnum.Expel;
                        break;
                    default:
                        mediaControl = ResetDeviceRequest.MediaControlEnum.Retract;
                        (reset.Payload.MediaControl.Length == "unit0".Length).IsTrue($"Invalid retract unit specified.");
                        char bin = reset.Payload.MediaControl[4];
                        binNumber = bin - 0x30;
                        break;
                }
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.ResetDeviceAsync()");
            var result = await Device.ResetDeviceAsync(
                new ResetDeviceRequest(
                    mediaControl,
                    binNumber),
                cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.ResetDeviceAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            if (!string.IsNullOrEmpty(result.StorageId))
            {
                await Storage.UpdateCardStorageCount(result.StorageId, result.MediaInCount);
            }

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private XFS4IoTFramework.Storage.IStorageService Storage { get => Provider.IsA<XFS4IoTFramework.Storage.IStorageService>(); }
    }
}
