/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Printer
{
    public partial class PrintRawHandler
    {
        private async Task<PrintRawCompletion.PayloadData> HandlePrintRaw(IPrintRawEvents events, PrintRawCommand printRaw, CancellationToken cancel)
        {
            if (printRaw.Payload.InputData is null)
            {
                return new PrintRawCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"No InputData is specified.");
            }

            if (string.IsNullOrEmpty(printRaw.Payload.Data))
            {
                return new PrintRawCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"No raw data is specified.");
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.RawPrintAsync()");
            var result = await Device.RawPrintAsync(new MediaPresentedCommandEvent(events),
                                                    new RawPrintRequest(printRaw.Payload.InputData == PrintRawCommand.PayloadData.InputDataEnum.Yes,
                                                                        Convert.FromBase64String(printRaw.Payload.Data).ToList()), 
                                                    cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.RawPrintAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new PrintRawCompletion.PayloadData(result.CompletionCode,
                                                      result.ErrorDescription,
                                                      result.ErrorCode,
                                                      result.Data?.Count > 0 ? Convert.ToBase64String(result.Data.ToArray()) : null);
        }
    }
}
