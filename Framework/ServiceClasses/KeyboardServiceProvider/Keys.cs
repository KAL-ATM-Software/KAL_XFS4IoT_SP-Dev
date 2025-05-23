﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Keyboard
{
    /// <summary>
    /// Specifies entry mode to which the layout applies. The following values are possible:
    /// * ```Data``` - Specifies that the layout be applied to the DataEntry method.
    /// * ```Pin``` - Specifies that the layout be applied to the PinEntry method.
    /// * ```Secure``` - Specifies that the layout be applied to the SecureKeyEntry method.
    /// </summary>
    public enum EntryModeEnum
    {
        Data,
        Pin,
        Secure
    }

    public sealed class FrameClass(
        int XPos,
        int YPos,
        int XSize,
        int YSize,
        FrameClass.FloatEnum FloatAction,
        List<FrameClass.FunctionKeyClass> FunctionKeys)
    {
        [Flags]
        public enum FloatEnum
        {
            NotSupported = 0,
            X = 1 << 0,
            Y = 1 << 1,
        }

        /// <summary>
        /// For [ETS](#keyboard.generalinformation.ets), specifies the left coordinate of the frame as an offset from the left edge of the screen. 
        /// For all other device types, this value is ignored
        /// </summary>
        public int XPos { get; init; } = XPos;

        /// <summary>
        /// For [ETS](#keyboard.generalinformation.ets), specifies the top coordinate of the frame as an offset from the top edge of the screen. 
        /// For all other device types, this value is ignored
        /// </summary>
        public int YPos { get; init; } = YPos;

        /// <summary>
        /// For [ETS](#keyboard.generalinformation.ets), specifies the width of the frame. For all other device types, this value is ignored
        /// </summary>
        public int XSize { get; init; } = XSize;

        /// <summary>
        /// For [ETS](#keyboard.generalinformation.ets), specifies the height of the frame. For all other device types, this value is ignored
        /// </summary>
        public int YSize { get; init; } = YSize;

        /// <summary>
        /// Specifies if the device can float the touch keyboards
        /// </summary>
        public FloatEnum FloatAction { get; init; } = FloatAction;

        public sealed class FunctionKeyClass(
            int XPos,
            int YPos,
            int XSize,
            int YSize,
            string Key,
            string ShiftKey)
        {

            /// <summary>
            /// Specifies the position of the top left corner of the FK relative to the left hand side of the layout.
            /// For [ETS](#keyboard.generalinformation.ets) devices, must be in the range defined in the frame. 
            /// For non-ETS devices, must be a value between 0 and 999, where 0 is the left edge and 999 is the right edge.
            /// </summary>
            public int XPos { get; init; } = XPos;

            /// <summary>
            /// Specifies the position of the top left corner of the Function Key (FK) relative to the left hand side of the layout.
            /// For [ETS](#keyboard.generalinformation.ets) devices, must be in the range defined in the frame. 
            /// For non-ETS devices, must be a value between 0 and 999, where 0 is the top edge and 999 is the bottom edge.
            /// </summary>
            public int YPos { get; init; } = YPos;

            /// <summary>
            /// Specifies the Function Key (FK) width. 
            /// For [ETS](#keyboard.generalinformation.ets), width is measured in pixels. For non-ETS devices, width is expressed as a value between 
            /// 1 and 1000, where 1 is the smallest possible size and 1000 is the full width of the layout.
            /// </summary>
            public int XSize { get; init; } = XSize;

            /// <summary>
            /// Specifies the Function Key (FK) height.
            /// For [ETS](#keyboard.generalinformation.ets), height is measured in pixels. 
            /// For non-ETS devices, height is expressed as a value between 1 and 1000, where 1 is the smallest 
            /// possible size and 1000 is the full height of the layout.
            /// </summary>
            public int YSize { get; init; } = YSize;

            /// <summary>
            /// Specifies the Function Key associated with the physical area in non-shifted mode.
            /// This property is not required if the *keyType* is omitted.
            /// </summary>
            public string Key { get; init; } = Key;

            /// <summary>
            /// Specifies the Function Key associated with the physical area in shifted mode.
            /// This property is not required if the *keyType* is omitted.
            /// </summary>
            public string ShiftKey { get; init; } = ShiftKey;
        }

        /// <summary>
        /// Defining details of the keys in the keyboard.
        /// </summary>
        public List<FunctionKeyClass> FunctionKeys { get; init; } = FunctionKeys;
    }
}
