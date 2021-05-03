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
        public SetGuidanceLightCommand(string RequestId, SetGuidanceLightCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public class CommandClass
            {
                public enum FlashRateEnum
                {
                    Off,
                    Slow,
                    Medium,
                    Quick,
                    Continuous,
                }
                [DataMember(Name = "flashRate")] 
                public FlashRateEnum? FlashRate { get; private set; }
                public enum ColorEnum
                {
                    Default,
                    Red,
                    Green,
                    Yellow,
                    Blue,
                    Cyan,
                    Magenta,
                    White,
                }
                [DataMember(Name = "color")] 
                public ColorEnum? Color { get; private set; }
                public enum DirectionEnum
                {
                    Entry,
                    Exit,
                }
                [DataMember(Name = "direction")] 
                public DirectionEnum? Direction { get; private set; }

                public CommandClass (FlashRateEnum? FlashRate, ColorEnum? Color, DirectionEnum? Direction)
                {
                    this.FlashRate = FlashRate;
                    this.Color = Color;
                    this.Direction = Direction;
                }


            }


            public PayloadData(int Timeout, int? GuidLight = null, object Command = null)
                : base(Timeout)
            {
                this.GuidLight = GuidLight;
                this.Command = Command;
            }

            /// <summary>
            /// Specifies the index of the guidance light to set as one of the values defined within the capabilities section: 
            /// </summary>
            [DataMember(Name = "guidLight")] 
            public int? GuidLight { get; private set; }

            [DataMember(Name = "command")] 
            public object Command { get; private set; }

        }
    }
}
