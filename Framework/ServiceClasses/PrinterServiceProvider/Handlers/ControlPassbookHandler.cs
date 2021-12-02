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
    public partial class ControlPassbookHandler
    {
        private async Task<ControlPassbookCompletion.PayloadData> HandleControlPassbook(IControlPassbookEvents events, ControlPassbookCommand controlPassbook, CancellationToken cancel)
        {
            if (controlPassbook.Payload.Action is null)
            {
                return new ControlPassbookCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                 $"no action specified.");

            }

            // Capability check
            if ((controlPassbook.Payload.Action == ControlPassbookCommand.PayloadData.ActionEnum.Backward &&
                !Printer.PrinterCapabilities.ControlPassbook.HasFlag(PrinterCapabilitiesClass.ControlPassbookEnum.TurnBackward)) ||
                (controlPassbook.Payload.Action == ControlPassbookCommand.PayloadData.ActionEnum.Forward &&
                !Printer.PrinterCapabilities.ControlPassbook.HasFlag(PrinterCapabilitiesClass.ControlPassbookEnum.TurnForward)) ||
                (controlPassbook.Payload.Action == ControlPassbookCommand.PayloadData.ActionEnum.CloseBackward &&
                !Printer.PrinterCapabilities.ControlPassbook.HasFlag(PrinterCapabilitiesClass.ControlPassbookEnum.CloseBackward)) ||
                (controlPassbook.Payload.Action == ControlPassbookCommand.PayloadData.ActionEnum.CloseForward &&
                !Printer.PrinterCapabilities.ControlPassbook.HasFlag(PrinterCapabilitiesClass.ControlPassbookEnum.CloseForward)))
            {
                return new ControlPassbookCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                 $"Specified action is not supported by the device. {controlPassbook.Payload.Action}");
            }

            int count = 0;
            if (controlPassbook.Payload.Action == ControlPassbookCommand.PayloadData.ActionEnum.Backward ||
                controlPassbook.Payload.Action == ControlPassbookCommand.PayloadData.ActionEnum.Forward)
            {
                if (controlPassbook.Payload.Count is null)
                {
                    return new ControlPassbookCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                     $"No count is specified.");
                }
                count = (int)controlPassbook.Payload.Count;
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.ControlPassbookAsync()");
            var result = await Device.ControlPassbookAsync(new ControlPassbookRequest(controlPassbook.Payload.Action switch
                                                                                      {
                                                                                          ControlPassbookCommand.PayloadData.ActionEnum.Backward => ControlPassbookRequest.ActionEnum.TurnBackward,
                                                                                          ControlPassbookCommand.PayloadData.ActionEnum.Forward => ControlPassbookRequest.ActionEnum.TurnForward,
                                                                                          ControlPassbookCommand.PayloadData.ActionEnum.CloseBackward => ControlPassbookRequest.ActionEnum.CloseBackward,
                                                                                          _ => ControlPassbookRequest.ActionEnum.CloseForward,
                                                                                      },
                                                                                      count),
                                                           cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.ControlPassbookAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new ControlPassbookCompletion.PayloadData(result.CompletionCode,
                                                             result.ErrorDescription,
                                                             result.ErrorCode);
        }
    }
}
