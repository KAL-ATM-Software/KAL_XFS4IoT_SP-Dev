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
    public partial class ControlMediaHandler
    {
        private async Task<ControlMediaCompletion.PayloadData> HandleControlMedia(IControlMediaEvents events, ControlMediaCommand controlMedia, CancellationToken cancel)
        {
            
            if (controlMedia.Payload.MediaControl is null)
            {
                return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"No media control is set. the device doesn't take a default action.");
            }

            PrinterCapabilitiesClass.ControlEnum controls = PrinterCapabilitiesClass.ControlEnum.NotSupported;

            // Capability check
            if (controlMedia.Payload.MediaControl.Alarm is not null &&
                (bool)controlMedia.Payload.MediaControl.Alarm)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Alarm))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Alarm}");
                }
                controls |= PrinterCapabilitiesClass.ControlEnum.Alarm;
            }
            if (controlMedia.Payload.MediaControl.Backward is not null &&
                (bool)controlMedia.Payload.MediaControl.Backward)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Backward))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Backward}");
                }
                controls |= PrinterCapabilitiesClass.ControlEnum.Backward;
            }
            if (controlMedia.Payload.MediaControl.ClearBuffer is not null &&
                (bool)controlMedia.Payload.MediaControl.ClearBuffer)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.ClearBuffer))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.ClearBuffer}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.ClearBuffer;
            }
            if (controlMedia.Payload.MediaControl.Cut is not null &&
                (bool)controlMedia.Payload.MediaControl.Cut)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Cut))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Cut}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Cut;
            }
            if (controlMedia.Payload.MediaControl.Eject is not null &&
                (bool)controlMedia.Payload.MediaControl.Eject)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Eject))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Eject}");
                }

                if ((controlMedia.Payload.MediaControl.EjectToTransport is not null &&
                    (bool)controlMedia.Payload.MediaControl.EjectToTransport) ||
                    (controlMedia.Payload.MediaControl.Retract is not null &&
                    (bool)controlMedia.Payload.MediaControl.Retract) ||
                    (controlMedia.Payload.MediaControl.Park is not null &&
                    (bool)controlMedia.Payload.MediaControl.Park) ||
                    (controlMedia.Payload.MediaControl.Expel is not null &&
                    (bool)controlMedia.Payload.MediaControl.Expel))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Eject control can't combined with other actions, EjectToTransport, Retract, Park or Expel. {controlMedia.Payload.MediaControl.Eject}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Eject;
            }
            if (controlMedia.Payload.MediaControl.EjectToTransport is not null &&
                (bool)controlMedia.Payload.MediaControl.EjectToTransport)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.EjectToTransport))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.EjectToTransport}");
                }

                if ((controlMedia.Payload.MediaControl.Eject is not null &&
                    (bool)controlMedia.Payload.MediaControl.Eject) ||
                    (controlMedia.Payload.MediaControl.Retract is not null &&
                    (bool)controlMedia.Payload.MediaControl.Retract) ||
                    (controlMedia.Payload.MediaControl.Park is not null &&
                    (bool)controlMedia.Payload.MediaControl.Park) ||
                    (controlMedia.Payload.MediaControl.Expel is not null &&
                    (bool)controlMedia.Payload.MediaControl.Expel))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"EjectToTransport control can't combined with other actions, Eject, Retract, Park or Expel. {controlMedia.Payload.MediaControl.EjectToTransport}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.EjectToTransport;
            }
            if (controlMedia.Payload.MediaControl.Flush is not null &&
                (bool)controlMedia.Payload.MediaControl.Flush)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Flush))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Flush}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Flush;
            }
            if (controlMedia.Payload.MediaControl.Forward is not null &&
                (bool)controlMedia.Payload.MediaControl.Forward)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Forward))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Forward}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Forward;
            }
            if (controlMedia.Payload.MediaControl.Park is not null &&
                (bool)controlMedia.Payload.MediaControl.Park)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Park))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Park}");
                }

                if ((controlMedia.Payload.MediaControl.Eject is not null &&
                    (bool)controlMedia.Payload.MediaControl.Eject) ||
                    (controlMedia.Payload.MediaControl.Retract is not null &&
                    (bool)controlMedia.Payload.MediaControl.Retract) ||
                    (controlMedia.Payload.MediaControl.EjectToTransport is not null &&
                    (bool)controlMedia.Payload.MediaControl.EjectToTransport) ||
                    (controlMedia.Payload.MediaControl.Expel is not null &&
                    (bool)controlMedia.Payload.MediaControl.Expel))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Park control can't combined with other actions, Eject, Retract, EjectToTransport or Expel. {controlMedia.Payload.MediaControl.Park}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Park;
            }
            if (controlMedia.Payload.MediaControl.PartialCut is not null &&
                (bool)controlMedia.Payload.MediaControl.PartialCut)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.PartialCut))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.PartialCut}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.PartialCut;
            }
            if (controlMedia.Payload.MediaControl.Perforate is not null &&
                (bool)controlMedia.Payload.MediaControl.Perforate)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Perforate))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Perforate}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Perforate;
            }
            if (controlMedia.Payload.MediaControl.Retract is not null &&
                (bool)controlMedia.Payload.MediaControl.Retract)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Retract))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Retract}");
                }

                if ((controlMedia.Payload.MediaControl.Eject is not null &&
                    (bool)controlMedia.Payload.MediaControl.Eject) ||
                    (controlMedia.Payload.MediaControl.Park is not null &&
                    (bool)controlMedia.Payload.MediaControl.Park) ||
                    (controlMedia.Payload.MediaControl.EjectToTransport is not null &&
                    (bool)controlMedia.Payload.MediaControl.EjectToTransport) ||
                    (controlMedia.Payload.MediaControl.Expel is not null &&
                    (bool)controlMedia.Payload.MediaControl.Expel))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Retract control can't combined with other actions, Eject, Park, EjectToTransport or Expel. {controlMedia.Payload.MediaControl.Retract}");
                }
                controls |= PrinterCapabilitiesClass.ControlEnum.Retract;
            }
            if (controlMedia.Payload.MediaControl.Rotate180 is not null &&
                (bool)controlMedia.Payload.MediaControl.Rotate180)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Rotate180))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Rotate180}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Rotate180;
            }
            if (controlMedia.Payload.MediaControl.Skip is not null &&
                (bool)controlMedia.Payload.MediaControl.Skip)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Skip))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Skip}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Skip;
            }
            if (controlMedia.Payload.MediaControl.Stack is not null &&
                (bool)controlMedia.Payload.MediaControl.Stack)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Stack))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Stack}");
                }
                controls |= PrinterCapabilitiesClass.ControlEnum.Stack;
            }
            if (controlMedia.Payload.MediaControl.Stamp is not null &&
                (bool)controlMedia.Payload.MediaControl.Stamp)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Stamp))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Stamp}");
                }
                controls |= PrinterCapabilitiesClass.ControlEnum.Stamp;
            
            }
            if (controlMedia.Payload.MediaControl.TurnMedia is not null &&
                (bool)controlMedia.Payload.MediaControl.TurnMedia)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.TurnMedia))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.TurnMedia}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.TurnMedia;
            }

            if (controlMedia.Payload.MediaControl.Expel is not null &&
                (bool)controlMedia.Payload.MediaControl.Expel)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Expel))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Expel}");
                }

                if ((controlMedia.Payload.MediaControl.EjectToTransport is not null &&
                    (bool)controlMedia.Payload.MediaControl.EjectToTransport) ||
                    (controlMedia.Payload.MediaControl.Retract is not null &&
                    (bool)controlMedia.Payload.MediaControl.Retract) ||
                    (controlMedia.Payload.MediaControl.Park is not null &&
                    (bool)controlMedia.Payload.MediaControl.Park) ||
                    (controlMedia.Payload.MediaControl.Retract is not null &&
                    (bool)controlMedia.Payload.MediaControl.Retract))
                {
                    return new ControlMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Expel control can't combined with other actions, EjectToTransport, Retract, Park or Retract. {controlMedia.Payload.MediaControl.Expel}");
                }
                controls |= PrinterCapabilitiesClass.ControlEnum.Expel;
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.ControlMediaAsync()");
            var result = await Device.ControlMediaAsync(new ControlMediaEvent(events),
                                                        new ControlMediaRequest(controls),
                                                        cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.ControlMediaAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new ControlMediaCompletion.PayloadData(result.CompletionCode,
                                                          result.ErrorDescription,
                                                          result.ErrorCode switch
                                                          {
                                                              ControlMediaResult.ErrorCodeEnum.BlackMark => ControlMediaCompletion.PayloadData.ErrorCodeEnum.BlackMark,
                                                              ControlMediaResult.ErrorCodeEnum.FlushFail => ControlMediaCompletion.PayloadData.ErrorCodeEnum.FlushFail,
                                                              ControlMediaResult.ErrorCodeEnum.InkOut => ControlMediaCompletion.PayloadData.ErrorCodeEnum.InkOut,
                                                              ControlMediaResult.ErrorCodeEnum.MediaJammed => ControlMediaCompletion.PayloadData.ErrorCodeEnum.MediaJammed,
                                                              ControlMediaResult.ErrorCodeEnum.MediaRetained => ControlMediaCompletion.PayloadData.ErrorCodeEnum.MediaRetained,
                                                              ControlMediaResult.ErrorCodeEnum.MediaRetracted => ControlMediaCompletion.PayloadData.ErrorCodeEnum.MediaRetracted,
                                                              ControlMediaResult.ErrorCodeEnum.MediaTurnFail => ControlMediaCompletion.PayloadData.ErrorCodeEnum.MediaTurnFail,
                                                              ControlMediaResult.ErrorCodeEnum.NoMediaPresent => ControlMediaCompletion.PayloadData.ErrorCodeEnum.NoMediaPresent,
                                                              ControlMediaResult.ErrorCodeEnum.PageTurnFail => ControlMediaCompletion.PayloadData.ErrorCodeEnum.PageTurnFail,
                                                              ControlMediaResult.ErrorCodeEnum.PaperJammed => ControlMediaCompletion.PayloadData.ErrorCodeEnum.PaperJammed,
                                                              ControlMediaResult.ErrorCodeEnum.PaperOut => ControlMediaCompletion.PayloadData.ErrorCodeEnum.PaperOut,
                                                              ControlMediaResult.ErrorCodeEnum.RetractBinFull => ControlMediaCompletion.PayloadData.ErrorCodeEnum.RetractBinFull,
                                                              ControlMediaResult.ErrorCodeEnum.SequenceInvalid => ControlMediaCompletion.PayloadData.ErrorCodeEnum.SequenceInvalid,
                                                              ControlMediaResult.ErrorCodeEnum.ShutterFail => ControlMediaCompletion.PayloadData.ErrorCodeEnum.ShutterFail,
                                                              ControlMediaResult.ErrorCodeEnum.StackerFull => ControlMediaCompletion.PayloadData.ErrorCodeEnum.StackerFull,
                                                              ControlMediaResult.ErrorCodeEnum.TonerOut => ControlMediaCompletion.PayloadData.ErrorCodeEnum.TonerOut,
                                                              _ => null,
                                                          });

        }
    }
}
