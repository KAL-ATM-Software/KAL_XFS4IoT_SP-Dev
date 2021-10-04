/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Lights interface.
 * LightsSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.Lights
{

    [DataContract]
    public sealed class LightStateClass
    {
        public LightStateClass(PositionEnum? Position = null, FlashRateEnum? FlashRate = null, ColourEnum? Colour = null, DirectionEnum? Direction = null)
        {
            this.Position = Position;
            this.FlashRate = FlashRate;
            this.Colour = Colour;
            this.Direction = Direction;
        }

        public enum PositionEnum
        {
            Left,
            Right,
            Center,
            Top,
            Bottom,
            Front,
            Rear
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
        [DataMember(Name = "position")]
        public PositionEnum? Position { get; init; }

        public enum FlashRateEnum
        {
            Off,
            Slow,
            Medium,
            Quick,
            Continuous
        }

        /// <summary>
        /// The light flash rate as one of the following values:
        /// * ```off``` -  The light is turned off.
        /// * ```slow``` -  The light is flashing slowly.
        /// * ```medium``` -  The light is flashing medium frequency.
        /// * ```quick``` -  The light is flashing quickly.
        /// * ```continuous``` - The light is continuous (steady).
        /// </summary>
        [DataMember(Name = "flashRate")]
        public FlashRateEnum? FlashRate { get; init; }

        public enum ColourEnum
        {
            Red,
            Green,
            Yellow,
            Blue,
            Cyan,
            Magenta,
            White
        }

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
        [DataMember(Name = "colour")]
        public ColourEnum? Colour { get; init; }

        public enum DirectionEnum
        {
            Entry,
            Exit
        }

        /// <summary>
        /// The light direction as one of the following values:
        /// * ```entry``` -  The light is indicating entry.
        /// * ```exit``` -  The light is indicating exit.
        /// </summary>
        [DataMember(Name = "direction")]
        public DirectionEnum? Direction { get; init; }

    }


    [DataContract]
    public sealed class LightCapabilitiesClass
    {
        public LightCapabilitiesClass(FlashRateEnum? FlashRate = null, ColorEnum? Color = null, DirectionEnum? Direction = null, PositionEnum? Position = null)
        {
            this.FlashRate = FlashRate;
            this.Color = Color;
            this.Direction = Direction;
            this.Position = Position;
        }

        public enum FlashRateEnum
        {
            Off,
            Slow,
            Medium,
            Quick,
            Continuous
        }

        /// <summary>
        /// Indicates the light flash rate. The following values are possible:
        /// * ```off``` -  The light can be turned off.
        /// * ```slow``` -  The light can flash slowly.
        /// * ```medium``` -  The light can flash medium frequency.
        /// * ```quick``` -  The light can flash quickly.
        /// * ```continuous``` - The light can be continuous (steady).
        /// </summary>
        [DataMember(Name = "flashRate")]
        public FlashRateEnum? FlashRate { get; init; }

        public enum ColorEnum
        {
            Red,
            Green,
            Yellow,
            Blue,
            Cyan,
            Magenta,
            White
        }

        /// <summary>
        /// Indicates the light color. If this parameter is omitted the default color will be used. The following 
        /// values are possible:
        /// * ```red``` -  The light can be red.
        /// * ```green``` -  The light can be green.
        /// * ```yellow``` -  The light can be yellow.
        /// * ```blue``` -  The light can be blue.
        /// * ```cyan``` - The light can be cyan.
        /// * ```magenta``` -  The light can be magenta.
        /// * ```white``` - The light can be white.
        /// </summary>
        [DataMember(Name = "color")]
        public ColorEnum? Color { get; init; }

        public enum DirectionEnum
        {
            Entry,
            Exit
        }

        /// <summary>
        /// Indicates the light direction. The following values are possible:
        /// * ```entry``` -  The light can  indicate entry.
        /// * ```exit``` -  The light can indicate exit.
        /// </summary>
        [DataMember(Name = "direction")]
        public DirectionEnum? Direction { get; init; }

        public enum PositionEnum
        {
            Left,
            Right,
            Center,
            Top,
            Bottom,
            Front,
            Rear
        }

        /// <summary>
        /// Indicates the light position. The following values are possible:
        /// * ```left``` -  The left position.
        /// * ```right``` -  The right position.
        /// * ```center``` -  The center position.
        ///  * ```top``` -  The top position.
        /// * ```bottom``` -  The bottom position.
        /// * ```front``` -  The front position.
        /// * ```rear``` -  The rear position.
        /// </summary>
        [DataMember(Name = "position")]
        public PositionEnum? Position { get; init; }

    }


}
