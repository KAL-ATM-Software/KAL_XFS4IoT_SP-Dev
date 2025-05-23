/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Keyboard.Commands;
using XFS4IoT.Keyboard.Completions;
using XFS4IoT.Completions;
using XFS4IoT.Keyboard;
using static XFS4IoTFramework.Keyboard.FrameClass;

namespace XFS4IoTFramework.Keyboard
{
    [CommandHandlerAsync]
    public partial class GetLayoutHandler
    {
        private Task<CommandResult<GetLayoutCompletion.PayloadData>> HandleGetLayout(IGetLayoutEvents events, GetLayoutCommand getLayout, CancellationToken cancel)
        {
            if (Keyboard.KeyboardLayouts is null)
            {
                // nothing to report, not keys for the keyboard
                Task.FromResult(
                    new CommandResult<GetLayoutCompletion.PayloadData>(MessageHeader.CompletionCodeEnum.Success)
                    );
            }
         
            EntryModeEnum? inquiry = null;
            if (getLayout.Payload?.EntryMode is not null)
            {
                inquiry = getLayout.Payload.EntryMode switch
                {
                    GetLayoutCommand.PayloadData.EntryModeEnum.Data => EntryModeEnum.Data,
                    GetLayoutCommand.PayloadData.EntryModeEnum.Pin => EntryModeEnum.Pin,
                    _ => EntryModeEnum.Secure,
                };

                if (!Keyboard.KeyboardLayouts.ContainsKey((EntryModeEnum)inquiry))
                {
                    Task.FromResult(
                        new CommandResult<GetLayoutCompletion.PayloadData>(
                            new(GetLayoutCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode, 
                            $"Specified mode is not supported by the device. {getLayout.Payload.EntryMode}")
                        );
                }
            }

            List<LayoutFrameClass> data = null;
            List<LayoutFrameClass> pin = null;
            List<LayoutFrameClass> secure = null;

            foreach (var entryType in Keyboard.KeyboardLayouts)
            {
                List<LayoutFrameClass> resultFrames = [];
                foreach (var frame in entryType.Value)
                {
                    List<KeyClass> functionKeys = [];
                    foreach (var functionKey in frame.FunctionKeys)
                    {
                        functionKeys.Add(
                            new KeyClass(
                                XPos:functionKey.XPos,
                                YPos: functionKey.YPos,
                                XSize:functionKey.XSize,
                                YSize: functionKey.YSize,
                                Key: functionKey.Key,
                                ShiftKey: functionKey.ShiftKey)
                            );
                    }

                    resultFrames.Add(
                        new(
                            frame.XPos, 
                            frame.YPos, 
                            frame.XSize, 
                            frame.YSize,
                            frame.FloatAction != FrameClass.FloatEnum.NotSupported ? new LayoutFrameClass.FloatClass(frame.FloatAction.HasFlag(FrameClass.FloatEnum.X), frame.FloatAction.HasFlag(FrameClass.FloatEnum.Y)) : null,
                            functionKeys)
                        );
                }

                if (inquiry is null or EntryModeEnum.Data &&
                    entryType.Key == EntryModeEnum.Data)
                {
                    data = resultFrames;
                }
                if (inquiry is null or EntryModeEnum.Pin &&
                    entryType.Key == EntryModeEnum.Pin)
                {
                    pin = resultFrames;
                }
                if (inquiry is null or EntryModeEnum.Secure &&
                    entryType.Key == EntryModeEnum.Secure)
                {
                    secure = resultFrames;
                }

                if (inquiry is not null)
                    break;
            }

            GetLayoutCompletion.PayloadData payload = null;
            if (data is not null ||
                pin is not null ||
                secure is not null)
            {
                payload = new(
                    Layout: new LayoutNullableClass(
                        Data: data,
                        Pin: pin,
                        Secure: secure)
                    );
            }

            return Task.FromResult(
                new CommandResult<GetLayoutCompletion.PayloadData>(
                    payload,
                    CompletionCode: MessageHeader.CompletionCodeEnum.Success)
                );
        }
    }
}
