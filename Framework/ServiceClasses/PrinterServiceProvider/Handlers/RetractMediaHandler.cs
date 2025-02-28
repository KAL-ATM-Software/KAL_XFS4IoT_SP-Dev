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
    public partial class RetractMediaHandler
    {
        private async Task<CommandResult<RetractMediaCompletion.PayloadData>> HandleRetractMedia(IRetractMediaEvents events, RetractMediaCommand retractMedia, CancellationToken cancel)
        {
            if (Common.PrinterCapabilities.RetractBins == 0)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Invalid bin number specifid.");
            }

            int binNumber = -1; // default to move transport.
            if (!string.IsNullOrEmpty(retractMedia.Payload.MediaControl))
            {
                if (!Regex.IsMatch(retractMedia.Payload.MediaControl, "^transport$|^unit[0-9]+$"))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified media control is not supported by the device.{retractMedia.Payload.MediaControl}");
                }
            }

            if (retractMedia.Payload.MediaControl != "transport")
            {
                (retractMedia.Payload.MediaControl.Length == "unit0".Length).IsTrue($"Invalid retract unit specified.");
                char bin = retractMedia.Payload.MediaControl[4];
                binNumber = bin - 0x30;
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.RetractAsync()");
            var result = await Device.RetractAsync(binNumber, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.RetractAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            string mediaResult = null;
            if (result.ErrorCode == RetractMediaCompletion.PayloadData.ErrorCodeEnum.NoMediaPresent)
            {
                mediaResult = "nomedia";
            }
            else if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
            {
                mediaResult = retractMedia.Payload.MediaControl;
            }

            if (!string.IsNullOrEmpty(result.StorageId))
            {
                await Storage.UpdateCardStorageCount(result.StorageId, result.MediaInCount);
            }

            RetractMediaCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                !string.IsNullOrEmpty(mediaResult))
            {
                payload = new(
                    ErrorCode: result.ErrorCode,
                    Result: mediaResult);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private XFS4IoTFramework.Storage.IStorageService Storage { get => Provider.IsA<XFS4IoTFramework.Storage.IStorageService>(); }
    }
}
