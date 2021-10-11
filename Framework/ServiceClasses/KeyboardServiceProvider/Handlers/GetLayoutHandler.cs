/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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

namespace XFS4IoTFramework.Keyboard
{
    public partial class GetLayoutHandler
    {
        private Task<GetLayoutCompletion.PayloadData> HandleGetLayout(IGetLayoutEvents events, GetLayoutCommand getLayout, CancellationToken cancel)
        {
            if (Keyboard.KeyboardLayouts is null)
            {
                // nothing to report, not keys for the keyboard
                Task.FromResult(new GetLayoutCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, string.Empty));
            }
         
            EntryModeEnum? inquiry = null;
            if (getLayout.Payload.EntryMode is not null)
            {
                inquiry = getLayout.Payload.EntryMode switch
                {
                    GetLayoutCommand.PayloadData.EntryModeEnum.Data => EntryModeEnum.Data,
                    GetLayoutCommand.PayloadData.EntryModeEnum.Pin => EntryModeEnum.Pin,
                    _ => EntryModeEnum.Secure,
                };

                if (!Keyboard.KeyboardLayouts.ContainsKey((EntryModeEnum)inquiry))
                {
                    Task.FromResult(new GetLayoutCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode, 
                                                                        $"Specified mode is not supported by the device. {getLayout.Payload.EntryMode}",
                                                                        GetLayoutCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported));
                }
            }

            List<LayoutFrameClass> data = null;
            List<LayoutFrameClass> pin = null;
            List<LayoutFrameClass> secure = null;

            foreach (var entryType in Keyboard.KeyboardLayouts)
            {
                List<LayoutFrameClass> resultFrames = new();
                foreach (var frame in entryType.Value)
                {
                    List<LayoutFrameClass.KeysClass> functionKeys = new();
                    foreach (var functionKey in frame.FunctionKeys)
                    {
                        functionKeys.Add(new LayoutFrameClass.KeysClass(functionKey.XPos,
                                                                        functionKey.YPos,
                                                                        functionKey.XSize,
                                                                        functionKey.YSize,
                                                                        functionKey.Key,
                                                                        functionKey.ShiftKey));
                    }

                    resultFrames.Add(new(frame.XPos, 
                                         frame.YPos, 
                                         frame.XSize, 
                                         frame.YSize,
                                         frame.FloatAction != FrameClass.FloatEnum.NotSupported ? new LayoutFrameClass.FloatClass(frame.FloatAction.HasFlag(FrameClass.FloatEnum.X), frame.FloatAction.HasFlag(FrameClass.FloatEnum.Y)) : null,
                                         functionKeys));
                }

                if (inquiry is null ||
                    entryType.Key == EntryModeEnum.Data)
                {
                    data = resultFrames;
                }
                if (inquiry is null ||
                    entryType.Key == EntryModeEnum.Pin)
                {
                    pin = resultFrames;
                }
                if (inquiry is null ||
                    entryType.Key == EntryModeEnum.Secure)
                {
                    secure = resultFrames;
                }

                if (inquiry is not null)
                    break;
            }
            
            return Task.FromResult(new GetLayoutCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                       null, 
                                                                       Layout: new LayoutClass(data, pin, secure)));
        }
    }
}
