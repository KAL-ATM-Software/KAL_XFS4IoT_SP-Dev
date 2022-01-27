/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    public partial class RetractMediaHandler
    {
        private async Task<RetractMediaCompletion.PayloadData> HandleRetractMedia(IRetractMediaEvents events, RetractMediaCommand retractMedia, CancellationToken cancel)
        {
            if (Common.PrinterCapabilities.RetractBins == 0)
            {
                return new RetractMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Invalid bin number specifid.");
            }

            if (retractMedia.Payload.BinNumber is not null &&
                retractMedia.Payload.BinNumber > Common.PrinterCapabilities.RetractBins)
            {
                return new RetractMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                            $"Specified an invalid retract bin number.{retractMedia.Payload.BinNumber}");
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.RetractAsync()");
            var result = await Device.RetractAsync(retractMedia.Payload.BinNumber is null ? -1 : (int)retractMedia.Payload.BinNumber, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.RetractAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new RetractMediaCompletion.PayloadData(result.CompletionCode,
                                                          result.ErrorDescription,
                                                          result.ErrorCode,
                                                          result.BinNumber);
        }
    }
}
