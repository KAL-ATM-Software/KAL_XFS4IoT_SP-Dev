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
    public partial class DispensePaperHandler
    {
        private async Task<DispensePaperCompletion.PayloadData> HandleDispensePaper(IDispensePaperEvents events, DispensePaperCommand dispensePaper, CancellationToken cancel)
        {
            if (!Common.PrinterCapabilities.DispensePaper)
            {
                return new DispensePaperCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"The device doesn't support dispensing paper capability.");
            }

            PaperSourceEnum? source = null;
            string customSource = null;

            // Capability check
            if (dispensePaper.Payload.PaperSource is null)
            {
                source = PaperSourceEnum.Default;
            }
            else
            {
                if (dispensePaper.Payload.PaperSource == "aux" ||
                    dispensePaper.Payload.PaperSource == "aux2" ||
                    dispensePaper.Payload.PaperSource == "external" ||
                    dispensePaper.Payload.PaperSource == "lower" ||
                    dispensePaper.Payload.PaperSource == "upper" ||
                    dispensePaper.Payload.PaperSource == "park")
                {
                    if ((dispensePaper.Payload.PaperSource == "aux" &&
                        !Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.AUX)) ||
                        (dispensePaper.Payload.PaperSource == "aux2" &&
                        !Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.AUX2)) ||
                        (dispensePaper.Payload.PaperSource == "external" &&
                        !Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.External)) ||
                        (dispensePaper.Payload.PaperSource == "lower" &&
                        !Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.Lower)) ||
                        (dispensePaper.Payload.PaperSource == "upper" &&
                        !Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.Upper)) ||
                        (dispensePaper.Payload.PaperSource == "park" &&
                        !Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.Park)))
                    {
                        return new DispensePaperCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                       $"Specified paper source is not supported by the device. {dispensePaper.Payload.PaperSource}");
                    }

                    source = dispensePaper.Payload.PaperSource switch
                    {
                        "aux" => PaperSourceEnum.AUX,
                        "aux2" => PaperSourceEnum.AUX2,
                        "external" => PaperSourceEnum.External,
                        "lower" => PaperSourceEnum.Lower,
                        "upper" => PaperSourceEnum.Upper,
                        _ => PaperSourceEnum.Park,
                    };
                }
                else
                {
                    if (!Common.PrinterCapabilities.CustomPaperSources.ContainsKey(dispensePaper.Payload.PaperSource))
                    {
                        return new DispensePaperCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                       $"Specified paper source is not supported by the device. {dispensePaper.Payload.PaperSource}");
                    }
                    customSource = dispensePaper.Payload.PaperSource;
                }
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.DispensePaperAsync()");
            var result = await Device.DispensePaperAsync(new DispensePaperCommandEvents(events),
                                                         new DispensePaperRequest(source, customSource),
                                                         cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.DispensePaperAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new DispensePaperCompletion.PayloadData(result.CompletionCode,
                                                           result.ErrorDescription,
                                                           result.ErrorCode);
        }
    }
}
