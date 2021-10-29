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
        public LightCapabilitiesClass(FlashRateClass FlashRate = null, ColorClass Color = null, DirectionClass Direction = null, PositionClass Position = null)
        {
            this.FlashRate = FlashRate;
            this.Color = Color;
            this.Direction = Direction;
            this.Position = Position;
        }

        [DataContract]
        public sealed class FlashRateClass
        {
            public FlashRateClass(bool? Off = null, bool? Slow = null, bool? Medium = null, bool? Quick = null, bool? Continuous = null)
            {
                this.Off = Off;
                this.Slow = Slow;
                this.Medium = Medium;
                this.Quick = Quick;
                this.Continuous = Continuous;
            }

            /// <summary>
            /// The light can be turned off.
            /// </summary>
            [DataMember(Name = "off")]
            public bool? Off { get; init; }

            /// <summary>
            /// The light can flash slowly.
            /// </summary>
            [DataMember(Name = "slow")]
            public bool? Slow { get; init; }

            /// <summary>
            /// The light can flash medium frequency.
            /// </summary>
            [DataMember(Name = "medium")]
            public bool? Medium { get; init; }

            /// <summary>
            /// The light can flash quickly.
            /// </summary>
            [DataMember(Name = "quick")]
            public bool? Quick { get; init; }

            /// <summary>
            /// The light can flash continuous (steady).
            /// </summary>
            [DataMember(Name = "continuous")]
            public bool? Continuous { get; init; }

        }

        /// <summary>
        /// Indicates the light flash rate.
        /// </summary>
        [DataMember(Name = "flashRate")]
        public FlashRateClass FlashRate { get; init; }

        [DataContract]
        public sealed class ColorClass
        {
            public ColorClass(bool? Red = null, bool? Green = null, bool? Yellow = null, bool? Blue = null, bool? Cyan = null, bool? Magenta = null, bool? White = null)
            {
                this.Red = Red;
                this.Green = Green;
                this.Yellow = Yellow;
                this.Blue = Blue;
                this.Cyan = Cyan;
                this.Magenta = Magenta;
                this.White = White;
            }

            /// <summary>
            /// The light can be red.
            /// </summary>
            [DataMember(Name = "red")]
            public bool? Red { get; init; }

            /// <summary>
            /// The light can be green.
            /// </summary>
            [DataMember(Name = "green")]
            public bool? Green { get; init; }

            /// <summary>
            /// The light can be yellow.
            /// </summary>
            [DataMember(Name = "yellow")]
            public bool? Yellow { get; init; }

            /// <summary>
            /// The light can be blue.
            /// </summary>
            [DataMember(Name = "blue")]
            public bool? Blue { get; init; }

            /// <summary>
            /// The light can be cyan.
            /// </summary>
            [DataMember(Name = "cyan")]
            public bool? Cyan { get; init; }

            /// <summary>
            /// The light can be magenta.
            /// </summary>
            [DataMember(Name = "magenta")]
            public bool? Magenta { get; init; }

            /// <summary>
            /// The light can be white .
            /// </summary>
            [DataMember(Name = "white")]
            public bool? White { get; init; }

        }

        /// <summary>
        /// Indicates the light color.
        /// </summary>
        [DataMember(Name = "color")]
        public ColorClass Color { get; init; }

        [DataContract]
        public sealed class DirectionClass
        {
            public DirectionClass(bool? Entry = null, bool? Exit = null)
            {
                this.Entry = Entry;
                this.Exit = Exit;
            }

            /// <summary>
            /// The light can  indicate entry.
            /// </summary>
            [DataMember(Name = "entry")]
            public bool? Entry { get; init; }

            /// <summary>
            /// The light can  indicate exit.
            /// </summary>
            [DataMember(Name = "exit")]
            public bool? Exit { get; init; }

        }

        /// <summary>
        /// Indicates the light direction.
        /// </summary>
        [DataMember(Name = "direction")]
        public DirectionClass Direction { get; init; }

        [DataContract]
        public sealed class PositionClass
        {
            public PositionClass(bool? Left = null, bool? Right = null, bool? Center = null, bool? Top = null, bool? Bottom = null, bool? Front = null, bool? Rear = null)
            {
                this.Left = Left;
                this.Right = Right;
                this.Center = Center;
                this.Top = Top;
                this.Bottom = Bottom;
                this.Front = Front;
                this.Rear = Rear;
            }

            /// <summary>
            /// The left position.
            /// </summary>
            [DataMember(Name = "left")]
            public bool? Left { get; init; }

            /// <summary>
            /// The right position.
            /// </summary>
            [DataMember(Name = "right")]
            public bool? Right { get; init; }

            /// <summary>
            /// The center position.
            /// </summary>
            [DataMember(Name = "center")]
            public bool? Center { get; init; }

            /// <summary>
            /// The top position.
            /// </summary>
            [DataMember(Name = "top")]
            public bool? Top { get; init; }

            /// <summary>
            /// The bottom position.
            /// </summary>
            [DataMember(Name = "bottom")]
            public bool? Bottom { get; init; }

            /// <summary>
            /// The front position.
            /// </summary>
            [DataMember(Name = "front")]
            public bool? Front { get; init; }

            /// <summary>
            /// The rear position.
            /// </summary>
            [DataMember(Name = "rear")]
            public bool? Rear { get; init; }

        }

        /// <summary>
        /// Indicates the light position.
        /// </summary>
        [DataMember(Name = "position")]
        public PositionClass Position { get; init; }

    }


}
