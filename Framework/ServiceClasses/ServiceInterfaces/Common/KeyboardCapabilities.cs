/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    public sealed class KeyboardCapabilitiesClass
    {
        [Flags]
        public enum KeyboardBeepEnum
        {
            NotSupported = 0,
            ActiveAvailable = 0x0001,
            ActiveSelectable = 0x0002,
            InActiveAvailable = 0x0004,
            InActiveSelectable = 0x0010,
        }

        public sealed class ETSCaps
        {
            [Flags]
            public enum FloatPositionEnum
            {
                NotSupported = 0,
                FloatX = 0x0001,
                FloatY = 0x0002,
            }

            public ETSCaps(int XPos,
                           int YPos,
                           int XSize,
                           int YSize,
                           int MaximumTouchFrames,
                           int MaximumTouchKeys,
                           FloatPositionEnum FloatFlags)
            {
                this.XPos = XPos;
                this.YPos = YPos;
                this.XSize = XSize;
                this.YSize = YSize;
                this.MaximumTouchFrames = MaximumTouchFrames;
                this.MaximumTouchKeys = MaximumTouchKeys;
                this.FloatFlags = FloatFlags;
            }

            public int XPos { get; init; }
            public int YPos { get; init; }
            public int XSize { get; init; }
            public int YSize { get; init; }
            public int MaximumTouchFrames { get; init; }
            public int MaximumTouchKeys { get; init;  }
            public FloatPositionEnum FloatFlags { get; init; }
        }

        public KeyboardCapabilitiesClass(KeyboardBeepEnum AutoBeep)
        {
            this.AutoBeep = AutoBeep;
        }

        public KeyboardBeepEnum AutoBeep { get; init; }
    }
}
