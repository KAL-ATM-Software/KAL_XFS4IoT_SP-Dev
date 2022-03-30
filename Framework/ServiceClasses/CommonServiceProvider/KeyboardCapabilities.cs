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
    public sealed class KeyboardCapabilitiesClass
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

        public sealed class ETSCap
        {
            [Flags]
            public enum FloatPositionEnum
            {
                NotSupported = 0,
                FloatX = 1 << 0,
                FloatY = 1 << 1,
            }

            public ETSCap(int XPos,
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

        public KeyboardCapabilitiesClass(KeyboardBeepEnum AutoBeep, List<ETSCap> ETSCaps)
        {
            this.AutoBeep = AutoBeep;
            this.ETSCaps = ETSCaps;
        }

        public KeyboardBeepEnum AutoBeep { get; init; }

        public List<ETSCap> ETSCaps;
    }
}
