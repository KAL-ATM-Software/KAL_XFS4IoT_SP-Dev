/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Printer
{
    public partial class ResetHandler
    {
        private async Task<ResetCompletion.PayloadData> HandleReset(IResetEvents events, ResetCommand reset, CancellationToken cancel)
        {
            ResetDeviceRequest.MediaControlEnum mediaControl = ResetDeviceRequest.MediaControlEnum.Default;
            if (reset.Payload.MediaControl is not null)
            {
                if ((reset.Payload.MediaControl == ResetCommand.PayloadData.MediaControlEnum.Eject &&
                    !Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Eject)) ||
                    (reset.Payload.MediaControl == ResetCommand.PayloadData.MediaControlEnum.Retract &&
                    !Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Retract)) ||
                    (reset.Payload.MediaControl == ResetCommand.PayloadData.MediaControlEnum.Expel &&
                    !Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Expel)))
                {
                    return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"Specified media control is not supported by the device.{reset.Payload.MediaControl}");
                }

                if (reset.Payload.MediaControl == ResetCommand.PayloadData.MediaControlEnum.Retract &&
                    reset.Payload.RetractBinNumber is not null &&
                    reset.Payload.RetractBinNumber > Common.PrinterCapabilities.RetractBins)
                {
                    return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"Specified an invalid retract bin number.{reset.Payload.RetractBinNumber}");
                }
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.ResetDeviceAsync()");
            var result = await Device.ResetDeviceAsync(new ResetDeviceRequest(mediaControl,
                                                                              reset.Payload.RetractBinNumber is null ? -1 : (int)reset.Payload.RetractBinNumber),
                                                       cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.ResetDeviceAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new ResetCompletion.PayloadData(result.CompletionCode,
                                                   result.ErrorDescription,
                                                   result.ErrorCode);
        }
    }
}
