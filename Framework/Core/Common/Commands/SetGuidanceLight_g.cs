/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * SetGuidanceLight_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Common.Commands
{
    //Original name = SetGuidanceLight
    [DataContract]
    [Command(Name = "Common.SetGuidanceLight")]
    public sealed class SetGuidanceLightCommand : Command<SetGuidanceLightCommand.PayloadData>
    {
        public SetGuidanceLightCommand(int RequestId, SetGuidanceLightCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? GuidLight = null, CommandClass Command = null)
                : base(Timeout)
            {
                this.GuidLight = GuidLight;
                this.Command = Command;
            }

            /// <summary>
            /// Specifies the index of the guidance light to set as one of the values defined within the capabilities section: 
            /// </summary>
            [DataMember(Name = "guidLight")]
            public int? GuidLight { get; init; }

            [DataContract]
            public sealed class CommandClass
            {
                public CommandClass(FlashRateEnum? FlashRate = null, ColorEnum? Color = null, DirectionEnum? Direction = null)
                {
                    this.FlashRate = FlashRate;
                    this.Color = Color;
                    this.Direction = Direction;
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
                /// Indicates which flash rates are supported by the guidelight.
                /// </summary>
                [DataMember(Name = "flashRate")]
                public FlashRateEnum? FlashRate { get; init; }

                public enum ColorEnum
                {
                    Default,
                    Red,
                    Green,
                    Yellow,
                    Blue,
                    Cyan,
                    Magenta,
                    White
                }

                /// <summary>
                /// Indicates which colors are supported by the guidelight.
                /// </summary>
                [DataMember(Name = "color")]
                public ColorEnum? Color { get; init; }

                public enum DirectionEnum
                {
                    Entry,
                    Exit
                }

                /// <summary>
                /// Indicates which directions are supported by the guidelight. and it'an optional field
                /// </summary>
                [DataMember(Name = "direction")]
                public DirectionEnum? Direction { get; init; }

            }


            [DataMember(Name = "command")]
            public CommandClass Command { get; init; }

        }
    }
}
