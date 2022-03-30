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
    public partial class ResetCountHandler
    {
        private async Task<ResetCountCompletion.PayloadData> HandleResetCount(IResetCountEvents events, ResetCountCommand resetCount, CancellationToken cancel)
        {
            if (Common.PrinterCapabilities.RetractBins == 0)
            {
                return new ResetCountCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                            $"Invalid bin number specifid.");
            }

            if (resetCount.Payload.BinNumber is not null &&
                resetCount.Payload.BinNumber > Common.PrinterCapabilities.RetractBins)
            {
                return new ResetCountCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                            $"Specified an invalid retract bin number.{resetCount.Payload.BinNumber}");
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.ResetBinCounterAsync()");
            var result = await Device.ResetBinCounterAsync(resetCount.Payload.BinNumber is null ? -1 : (int)resetCount.Payload.BinNumber, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.ResetBinCounterAsync() -> {result.CompletionCode}");

            return new ResetCountCompletion.PayloadData(result.CompletionCode,
                                                        result.ErrorDescription);
        }
    }
}
