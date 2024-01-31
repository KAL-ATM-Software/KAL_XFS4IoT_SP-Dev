/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Keyboard;

namespace XFS4IoTFramework.Keyboard
{
    public sealed class KeyboardCommandEvents
    {
        public KeyboardCommandEvents(IPinEntryEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(KeyboardCommandEvents));
            events.IsA<IPinEntryEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(KeyboardCommandEvents));
            PinEntryEvents = events;
        }
        public KeyboardCommandEvents(IDataEntryEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(KeyboardCommandEvents));
            events.IsA<IDataEntryEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(KeyboardCommandEvents));
            DataEntryEvents = events;
        }
        public KeyboardCommandEvents(ISecureKeyEntryEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(KeyboardCommandEvents));
            events.IsA<ISecureKeyEntryEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(KeyboardCommandEvents));
            SecureKeyEntryEvents = events;
        }

        public Task KeyEvent(EntryCompletionEnum? Completion = null, string Digit = null)
        {
            XFS4IoT.Keyboard.Events.KeyEvent.PayloadData payload = new(Completion switch
                                                                       {
                                                                           EntryCompletionEnum.Auto => XFS4IoT.Keyboard.EntryCompletionEnum.Auto,
                                                                           EntryCompletionEnum.Enter => XFS4IoT.Keyboard.EntryCompletionEnum.Enter,
                                                                           EntryCompletionEnum.Cancel => XFS4IoT.Keyboard.EntryCompletionEnum.Cancel,
                                                                           EntryCompletionEnum.Continue => XFS4IoT.Keyboard.EntryCompletionEnum.Continue,
                                                                           EntryCompletionEnum.Clear => XFS4IoT.Keyboard.EntryCompletionEnum.Clear,
                                                                           EntryCompletionEnum.Backspace => XFS4IoT.Keyboard.EntryCompletionEnum.Backspace,
                                                                           EntryCompletionEnum.FDK => XFS4IoT.Keyboard.EntryCompletionEnum.Fdk,
                                                                           EntryCompletionEnum.Help => XFS4IoT.Keyboard.EntryCompletionEnum.Help,
                                                                           EntryCompletionEnum.FK => XFS4IoT.Keyboard.EntryCompletionEnum.Fk,
                                                                           EntryCompletionEnum.ContinueFDK => XFS4IoT.Keyboard.EntryCompletionEnum.ContFdk,
                                                                           _ => null,
                                                                       }, Digit);

            if (PinEntryEvents is not null)
            {
                return PinEntryEvents.KeyEvent(payload);
            }
            if (DataEntryEvents is not null)
            {
                return DataEntryEvents.KeyEvent(payload);
            }
            if (SecureKeyEntryEvents is not null)
            {
                return SecureKeyEntryEvents.KeyEvent(payload);
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(KeyEvent));
        }

        public Task EnterDataEvent()
        {
            if (PinEntryEvents is not null)
            {
                return PinEntryEvents.EnterDataEvent();
            }
            if (DataEntryEvents is not null)
            {
                return DataEntryEvents.EnterDataEvent();
            }
            if (SecureKeyEntryEvents is not null)
            {
                return SecureKeyEntryEvents.EnterDataEvent();
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(EnterDataEvent));
        }

        public Task LayoutEvent(Dictionary<EntryModeEnum, List<FrameClass>> KeyboardLayouts)
        {
            List<LayoutFrameClass> data = null;
            List<LayoutFrameClass> pin = null;
            List<LayoutFrameClass> secure = null;

            foreach (var entryType in KeyboardLayouts)
            {
                List<LayoutFrameClass> resultFrames = new();
                foreach (var frame in entryType.Value)
                {
                    List<LayoutFrameClass.KeysClass> functionKeys = new();
                    foreach (var functionKey in frame.FunctionKeys)
                    {
                        functionKeys.Add(new LayoutFrameClass.KeysClass(XPos: functionKey.XPos,
                                                                        YPos: functionKey.YPos,
                                                                        XSize: functionKey.XSize,
                                                                        YSize:functionKey.YSize,
                                                                        Key: functionKey.Key,
                                                                        ShiftKey: functionKey.ShiftKey));
                    }

                    resultFrames.Add(new(frame.XPos,
                                         frame.YPos,
                                         frame.XSize,
                                         frame.YSize,
                                         frame.FloatAction != FrameClass.FloatEnum.NotSupported ? new LayoutFrameClass.FloatClass(frame.FloatAction.HasFlag(FrameClass.FloatEnum.X), frame.FloatAction.HasFlag(FrameClass.FloatEnum.Y)) : null,
                                         functionKeys));
                }

                if (entryType.Key == EntryModeEnum.Data)
                {
                    data = resultFrames;
                }
                if (entryType.Key == EntryModeEnum.Pin)
                {
                    pin = resultFrames;
                }
                if (entryType.Key == EntryModeEnum.Secure)
                {
                    secure = resultFrames;
                }
            }

            XFS4IoT.Keyboard.Events.LayoutEvent.PayloadData payload = new(data, pin, secure);

            if (PinEntryEvents is not null)
            {
                return PinEntryEvents.LayoutEvent(payload);
            }
            if (DataEntryEvents is not null)
            {
                return DataEntryEvents.LayoutEvent(payload);
            }
            if (SecureKeyEntryEvents is not null)
            {
                return SecureKeyEntryEvents.LayoutEvent(payload);
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(LayoutEvent));
        }

        private IPinEntryEvents PinEntryEvents { get; init; } = null;
        private IDataEntryEvents DataEntryEvents { get; init; } = null;
        private ISecureKeyEntryEvents SecureKeyEntryEvents { get; init; } = null;
    }
}
