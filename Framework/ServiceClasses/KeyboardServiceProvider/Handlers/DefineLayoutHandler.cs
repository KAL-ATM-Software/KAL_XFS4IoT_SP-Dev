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
using XFS4IoT.Keyboard;
using XFS4IoT.Keyboard.Commands;
using XFS4IoT.Keyboard.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Keyboard
{
    public partial class DefineLayoutHandler
    {
        private async Task<DefineLayoutCompletion.PayloadData> HandleDefineLayout(IDefineLayoutEvents events, DefineLayoutCommand defineLayout, CancellationToken cancel)
        {
            if (defineLayout.Payload.Layout is null)
            {
                return new DefineLayoutCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"No key layout data specified.");
            }

            if (defineLayout.Payload.Layout is null)
            {
                return new DefineLayoutCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"No key layout data specified.");
            }

            if (defineLayout.Payload.Layout.Data is null &&
                defineLayout.Payload.Layout.Pin is null &&
                defineLayout.Payload.Layout.Secure is null)
            {
                return new DefineLayoutCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                $"No key mode data specified.");
            }

            Dictionary<EntryModeEnum, List<LayoutFrameClass>> updateEntryModes = new();
            if (defineLayout.Payload.Layout.Data is not null)
                updateEntryModes.Add(EntryModeEnum.Data, defineLayout.Payload.Layout.Data);
            if (defineLayout.Payload.Layout.Pin is not null)
                updateEntryModes.Add(EntryModeEnum.Pin, defineLayout.Payload.Layout.Pin);
            if (defineLayout.Payload.Layout.Secure is not null)
                updateEntryModes.Add(EntryModeEnum.Secure, defineLayout.Payload.Layout.Secure);

            Dictionary<EntryModeEnum, List<FrameClass>> request = new();

            foreach (var entryMode in updateEntryModes)
            {

                if (entryMode.Value is null ||
                    entryMode.Value.Count == 0)
                {
                    return new DefineLayoutCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"No frames specified.");
                }

                List<FrameClass> frames = new();
                foreach (var frame in entryMode.Value)
                {
                    FrameClass.FloatEnum floatAction = FrameClass.FloatEnum.NotSupported;
                    if (frame.Float is not null)
                    {
                        if (frame.Float.X is not null && (bool)frame.Float.X)
                            floatAction |= FrameClass.FloatEnum.X;
                        if (frame.Float.Y is not null && (bool)frame.Float.Y)
                            floatAction |= FrameClass.FloatEnum.Y;
                    }

                    if (frame.XPos is null ||
                        frame.YPos is null ||
                        frame.XSize is null ||
                        frame.YSize is null)
                    {
                        return new DefineLayoutCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                      $"XPos, YPos, XSize, YSize are not specified in the frame. {entryMode.Key}");
                    }

                    if (frame.Keys is null ||
                        frame.Keys.Count == 0)
                    {
                        return new DefineLayoutCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                      $"No function keys are specified in the frame. {entryMode.Key}");
                    }

                    List<FrameClass.FunctionKeyClass> functionKeys = new();
                    foreach (var key in frame.Keys)
                    {
                        if (key.XPos is null ||
                            key.YPos is null ||
                            key.XSize is null ||
                            key.YSize is null)
                        {
                            return new DefineLayoutCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                          $"XPos, YPos, XSize, YSize are not specified in function keys. {entryMode.Key}");
                        }

                        if (string.IsNullOrEmpty(key.Key) &&
                            string.IsNullOrEmpty(key.ShiftKey))
                        {
                            return new DefineLayoutCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                          $"No function key with shift and non-shift mode is specified. {entryMode.Key}");
                        }

                        functionKeys.Add(new FrameClass.FunctionKeyClass((int)key.XPos,
                                                                         (int)key.YPos,
                                                                         (int)key.XSize,
                                                                         (int)key.YSize,
                                                                         key.Key,
                                                                         key.ShiftKey));
                    }

                    frames.Add(new FrameClass((int)frame.XPos, 
                                              (int)frame.YPos, 
                                              (int)frame.XSize, 
                                              (int)frame.YSize, 
                                              floatAction, 
                                              functionKeys));
                }

                request.Add(entryMode.Key, frames);
            }

            Logger.Log(Constants.DeviceClass, "KeyboardDev.DefineLayout()");

            var result = await Device.DefineLayout(request, cancel);

            Logger.Log(Constants.DeviceClass, $"KeyboardDev.DefineLayout() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                // Update internal layout
                Logger.Log(Constants.DeviceClass, "KeyboardDev.GetLayoutInfo()");

                Keyboard.KeyboardLayouts = Device.GetLayoutInfo();

                Logger.Log(Constants.DeviceClass, "KeyboardDev.GetLayoutInfo()->");

                Keyboard.KeyboardLayouts.IsNotNull($"The device class must provide keyboard layout information through GetLayoutInfo method call.");

                // Update internal variables
                Keyboard.SupportedFunctionKeys.Clear();
                Keyboard.SupportedFunctionKeysWithShift.Clear();

                foreach (var entryType in Keyboard.KeyboardLayouts)
                {
                    List<string> keys = null;
                    List<string> shiftKeys = null;

                    foreach (var frame in entryType.Value)
                    {
                        foreach (var key in frame.FunctionKeys)
                        {
                            if (!string.IsNullOrEmpty(key.Key))
                                keys.Add(key.Key);
                            if (!string.IsNullOrEmpty(key.ShiftKey))
                                shiftKeys.Add(key.ShiftKey);
                        }
                    }

                    if (keys is not null &&
                        keys.Count != 0)
                    {
                        Keyboard.SupportedFunctionKeys.Add(entryType.Key, keys);
                    }
                    if (shiftKeys is not null &&
                        shiftKeys.Count != 0)
                    {
                        Keyboard.SupportedFunctionKeysWithShift.Add(entryType.Key, shiftKeys);
                    }
                }
            }
            
            return new DefineLayoutCompletion.PayloadData(result.CompletionCode,
                                                          result.ErrorDescription,
                                                          result.ErrorCode);
        }
    }
}
