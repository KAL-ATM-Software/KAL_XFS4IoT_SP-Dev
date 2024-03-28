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
    /// <summary>
    /// LightsStatusClass
    /// Store device status for the available lights reported by the capabilities
    /// </summary>
    public sealed class LightsStatusClass : StatusBase
    {
        public sealed class LightOperation(LightOperation.PositionEnum Position,
        LightOperation.FlashRateEnum FlashRate,
        LightOperation.ColourEnum Colour,
        LightOperation.DirectionEnum Direction)
        {
            public enum PositionEnum
            {
                Left,
                Right,
                Center,
                Top,
                Bottom,
                Front,
                Rear,
                Default,
            }

            public enum FlashRateEnum
            {
                Off,
                Slow,
                Medium,
                Quick,
                Continuous
            }

            public enum ColourEnum
            {
                Red,
                Green,
                Yellow,
                Blue,
                Cyan,
                Magenta,
                White,
                Default
            }

            public enum DirectionEnum
            {
                Entry,
                Exit,
                None,
            }

            /// <summary>
            /// The light position. If omitted then the default position is used. One of the following values:
            /// * ```left``` -  The left position.
            /// * ```right``` -  The right position.
            /// * ```center``` -  The center position.
            ///  * ```top``` -  The top position.
            /// * ```bottom``` -  The bottom position.
            /// * ```front``` -  The front position.
            /// * ```rear``` -  The rear position.
            /// </summary>
            public PositionEnum Position { get; set; } = Position;

            /// <summary>
            /// The light flash rate as one of the following values:
            /// * ```off``` -  The light is turned off.
            /// * ```slow``` -  The light is flashing slowly.
            /// * ```medium``` -  The light is flashing medium frequency.
            /// * ```quick``` -  The light is flashing quickly.
            /// * ```continuous``` - The light is continuous (steady).
            /// </summary>
            public FlashRateEnum FlashRate { get; set; } = FlashRate;

            /// <summary>
            /// The light color as one of the following values:
            /// * ```red``` -  The light is red.
            /// * ```green``` -  The light is green.
            /// * ```yellow``` -  The light us yellow.
            /// * ```blue``` -  The light is blue.
            /// * ```cyan``` - The light is cyan.
            /// * ```magenta``` -  The light is magenta.
            /// * ```white``` - The light is white.
            /// </summary>
            public ColourEnum Colour { get; set; } = Colour;

            /// <summary>
            /// The light direction as one of the following values:
            /// * ```entry``` -  The light is indicating entry.
            /// * ```exit``` -  The light is indicating exit.
            /// </summary>
            public DirectionEnum Direction { get; set; } = Direction;
        }

        /// <summary>
        /// Report all available standard light status
        /// </summary>
        public Dictionary<LightsCapabilitiesClass.DeviceEnum, LightOperation> Status { get; set; }

        /// <summary>
        /// Report all available vendor specific light status
        /// </summary>
        public Dictionary<string, LightOperation> CustomStatus { get; set; }
    }
}
