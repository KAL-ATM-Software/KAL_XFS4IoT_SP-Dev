/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Common
{
    public sealed class KeyboardCapabilitiesClass(
        KeyboardCapabilitiesClass.KeyboardBeepEnum AutoBeep, 
        KeyboardCapabilitiesClass.ETSCap ETSCaps)
    {
        [Flags]
        public enum KeyboardBeepEnum
        {
            NotSupported = 0,
            ActiveAvailable = 1 << 0,
            ActiveSelectable = 1 << 1,
            InActiveAvailable = 1 << 2,
            InActiveSelectable = 1 << 3,
        }

        public sealed class ETSCap(
            int XPos,
            int YPos,
            int XSize,
            int YSize,
            int MaximumTouchFrames,
            int MaximumTouchKeys,
            ETSCap.FloatPositionEnum FloatFlags)
        {
            [Flags]
            public enum FloatPositionEnum
            {
                NotSupported = 0,
                FloatX = 1 << 0,
                FloatY = 1 << 1,
            }

            public int XPos { get; init; } = XPos;
            public int YPos { get; init; } = YPos;
            public int XSize { get; init; } = XSize;
            public int YSize { get; init; } = YSize;
            public int MaximumTouchFrames { get; init; } = MaximumTouchFrames;
            public int MaximumTouchKeys { get; init; } = MaximumTouchKeys;
            public FloatPositionEnum FloatFlags { get; init; } = FloatFlags;
        }

        public KeyboardBeepEnum AutoBeep { get; init; } = AutoBeep;

        public ETSCap ETSCaps = ETSCaps;
    }
}
