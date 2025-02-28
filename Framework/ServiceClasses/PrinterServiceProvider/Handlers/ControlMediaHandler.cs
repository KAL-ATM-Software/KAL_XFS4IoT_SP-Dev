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

namespace XFS4IoTFramework.Printer
{
    public partial class ControlMediaHandler
    {
        private async Task<CommandResult<ControlMediaCompletion.PayloadData>> HandleControlMedia(IControlMediaEvents events, ControlMediaCommand controlMedia, CancellationToken cancel)
        {
            
            if (controlMedia.Payload?.MediaControl is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No media control is set. the device doesn't take a default action.");
            }

            PrinterCapabilitiesClass.ControlEnum controls = PrinterCapabilitiesClass.ControlEnum.NotSupported;

            // Capability check
            if (controlMedia.Payload.MediaControl.Alarm is not null &&
                (bool)controlMedia.Payload.MediaControl.Alarm)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Alarm))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Alarm}");
                }
                controls |= PrinterCapabilitiesClass.ControlEnum.Alarm;
            }
            if (controlMedia.Payload.MediaControl.TurnPage is not null)
            {
                if (controlMedia.Payload.MediaControl.TurnPage == XFS4IoT.Printer.MediaControlClass.TurnPageEnum.Backward)
                {
                    if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Backward))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"Unsupported control media specified, check capabilities. {controlMedia.Payload.MediaControl.TurnPage}");
                    }
                    controls |= PrinterCapabilitiesClass.ControlEnum.Backward;
                }
                else
                {
                    if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Forward))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"Unsupported control media specified, check capabilities. {controlMedia.Payload.MediaControl.TurnPage}");
                    }
                    controls |= PrinterCapabilitiesClass.ControlEnum.Forward;
                }
            }
            if (controlMedia.Payload.MediaControl.Move is not null)
            {
                switch (controlMedia.Payload.MediaControl.Move)
                {
                    case XFS4IoT.Printer.MediaControlClass.MoveEnum.Retract:
                        if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Retract))
                        {
                            return new(
                                MessageHeader.CompletionCodeEnum.InvalidData,
                                $"Unsupported control media specified, check capabilities. {controlMedia.Payload.MediaControl.Move}");
                        }
                        controls |= PrinterCapabilitiesClass.ControlEnum.Retract;
                        break;
                    case XFS4IoT.Printer.MediaControlClass.MoveEnum.Eject:
                        if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Eject))
                        {
                            return new(
                                MessageHeader.CompletionCodeEnum.InvalidData,
                                $"Unsupported control media specified, check capabilities. {controlMedia.Payload.MediaControl.Move}");
                        }
                        controls |= PrinterCapabilitiesClass.ControlEnum.Eject;
                        break;
                    case XFS4IoT.Printer.MediaControlClass.MoveEnum.EjectToTransport:
                        if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.EjectToTransport))
                        {
                            return new(
                                MessageHeader.CompletionCodeEnum.InvalidData,
                                $"Unsupported control media specified, check capabilities. {controlMedia.Payload.MediaControl.Move}");
                        }
                        controls |= PrinterCapabilitiesClass.ControlEnum.EjectToTransport;
                        break;
                    case XFS4IoT.Printer.MediaControlClass.MoveEnum.Park:
                        if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Park))
                        {
                            return new(
                                MessageHeader.CompletionCodeEnum.InvalidData,
                                $"Unsupported control media specified, check capabilities. {controlMedia.Payload.MediaControl.Move}");
                        }
                        controls |= PrinterCapabilitiesClass.ControlEnum.Park;
                        break;
                    case XFS4IoT.Printer.MediaControlClass.MoveEnum.Stack:
                        if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Stack))
                        {
                            return new(
                                MessageHeader.CompletionCodeEnum.InvalidData,
                                $"Unsupported control media specified, check capabilities. {controlMedia.Payload.MediaControl.Move}");
                        }
                        controls |= PrinterCapabilitiesClass.ControlEnum.Stack;
                        break;
                    case XFS4IoT.Printer.MediaControlClass.MoveEnum.Expel:
                        if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Expel))
                        {
                            return new(
                                MessageHeader.CompletionCodeEnum.InvalidData,
                                $"Unsupported control media specified, check capabilities. {controlMedia.Payload.MediaControl.Move}");
                        }
                        controls |= PrinterCapabilitiesClass.ControlEnum.Expel;
                        break;
                    default:
                        throw new InvalidDataException(
                            $"Unsupported control media type option specified. {controlMedia.Payload.MediaControl.Move}"
                            );
                }
            }
            if (controlMedia.Payload.MediaControl.Cut is not null &&
                (bool)controlMedia.Payload.MediaControl.Cut)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Cut))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Cut}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Cut;
            }
            if (controlMedia.Payload.MediaControl.Flush is not null &&
                (bool)controlMedia.Payload.MediaControl.Flush)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Flush))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Flush}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Flush;
            }
            if (controlMedia.Payload.MediaControl.PartialCut is not null &&
                (bool)controlMedia.Payload.MediaControl.PartialCut)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.PartialCut))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported control media specified. {controlMedia.Payload.MediaControl.PartialCut}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.PartialCut;
            }
            if (controlMedia.Payload.MediaControl.Perforate is not null &&
                (bool)controlMedia.Payload.MediaControl.Perforate)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Perforate))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Perforate}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Perforate;
            }
            if (controlMedia.Payload.MediaControl.Rotate180 is not null &&
                (bool)controlMedia.Payload.MediaControl.Rotate180)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Rotate180))
                {
                    return new
                        (MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Rotate180}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Rotate180;
            }
            if (controlMedia.Payload.MediaControl.Skip is not null &&
                (bool)controlMedia.Payload.MediaControl.Skip)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Skip))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Skip}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.Skip;
            }
            if (controlMedia.Payload.MediaControl.Stamp is not null &&
                (bool)controlMedia.Payload.MediaControl.Stamp)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Stamp))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported control media specified. {controlMedia.Payload.MediaControl.Stamp}");
                }
                controls |= PrinterCapabilitiesClass.ControlEnum.Stamp;
            
            }
            if (controlMedia.Payload.MediaControl.TurnMedia is not null &&
                (bool)controlMedia.Payload.MediaControl.TurnMedia)
            {
                if (!Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.TurnMedia))
                {
                    return new(MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported control media specified. {controlMedia.Payload.MediaControl.TurnMedia}");
                }

                controls |= PrinterCapabilitiesClass.ControlEnum.TurnMedia;
            }
            
            Logger.Log(Constants.DeviceClass, "PrinterDev.ControlMediaAsync()");
            var result = await Device.ControlMediaAsync(
                new ControlMediaEvent(events),
                new ControlMediaRequest(controls),
                cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.ControlMediaAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            if (controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Retract))
            {
                if (!string.IsNullOrEmpty(result.StorageId))
                {
                    await Storage.UpdateCardStorageCount(result.StorageId, result.MediaInCount);
                }
            }

            return new(
                result.ErrorCode is null ? null : new(
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
                    }),
                result.CompletionCode,
                result.ErrorDescription);

        }

        private XFS4IoTFramework.Storage.IStorageService Storage { get => Provider.IsA<XFS4IoTFramework.Storage.IStorageService>(); }
    }
}
