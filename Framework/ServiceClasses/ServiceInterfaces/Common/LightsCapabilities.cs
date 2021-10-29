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
    /// <summary>
    /// LightsCapabilitiesClass
    /// Store device capabilites for the lights supported by the device
    /// </summary>
    public sealed class LightsCapabilitiesClass
    {
        [Flags]
        public enum FlashRateEnum
        {
            Off = 0x0001,
            Slow = 0x0002,
            Medium = 0x0004,
            Quick = 0x0008,
            Continuous = 0x0010,
        }

        [Flags]
        public enum ColorEnum
        {
            Default = 0,
            Red = 0x0001,
            Green = 0x0002,
            Yellow = 0x0004,
            Blue = 0x0008,
            Cyan = 0x0010,
            Magenta = 0x0020,
            White = 0x0040,
        }

        [Flags]
        public enum DirectionEnum
        {
            NotSupported = 0,
            Entry = 0x0001,
            Exit = 0x0002,
        }

        [Flags]
        public enum LightPostionEnum
        {
            Default = 0,
            Left = 0x0001,
            Right = 0x0002,
            Center = 00004,
            Top = 0x0008,
            Bottom = 0x0010,
            Front = 0x0020,
            Rear = 0x0040,
        }

        public LightsCapabilitiesClass(Dictionary<string, Light> Lights)
        {
            this.Lights = Lights;
        }

        /// <summary>
        /// Light capabilities
        /// </summary>
        public sealed class Light
        {
            public Light(FlashRateEnum FlashRate,
                         ColorEnum Color,
                         DirectionEnum Direction,
                         LightPostionEnum Position)
            {
                this.FlashRate = FlashRate;
                this.Color = Color;
                this.Direction = Direction;
                this.Position = Position;
            }

            public FlashRateEnum FlashRate { get; init; }

            public ColorEnum Color { get; init; }

            public DirectionEnum Direction { get; init; }

            public LightPostionEnum Position { get; init; }
        }

        public Dictionary<string, Light> Lights { get; init; }
    }
}
