/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * ClearScreen_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.TextTerminal.Commands
{
    //Original name = ClearScreen
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "TextTerminal.ClearScreen")]
    public sealed class ClearScreenCommand : Command<ClearScreenCommand.PayloadData>
    {
        public ClearScreenCommand()
            : base()
        { }

        public ClearScreenCommand(int RequestId, ClearScreenCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ScreenClass Screen = null)
                : base()
            {
                this.Screen = Screen;
            }

            [DataContract]
            public sealed class ScreenClass
            {
                public ScreenClass(int? PositionX = null, int? PositionY = null, int? Width = null, int? Height = null)
                {
                    this.PositionX = PositionX;
                    this.PositionY = PositionY;
                    this.Width = Width;
                    this.Height = Height;
                }

                /// <summary>
                /// Specifies the horizontal position of the area to be cleared.
                /// </summary>
                [DataMember(Name = "positionX")]
                [DataTypes(Minimum = 0)]
                public int? PositionX { get; init; }

                /// <summary>
                /// Specifies the vertical position of the area to be cleared.
                /// </summary>
                [DataMember(Name = "positionY")]
                [DataTypes(Minimum = 0)]
                public int? PositionY { get; init; }

                /// <summary>
                /// Specifies the width position of the area to be cleared.
                /// </summary>
                [DataMember(Name = "width")]
                [DataTypes(Minimum = 1)]
                public int? Width { get; init; }

                /// <summary>
                /// Specifies the height position of the area to be cleared.
                /// </summary>
                [DataMember(Name = "height")]
                [DataTypes(Minimum = 1)]
                public int? Height { get; init; }

            }

            /// <summary>
            /// Specifies the area of the Text Terminal Unit screen to clear.
            /// If this property is null, the whole screen will be cleared.
            /// </summary>
            [DataMember(Name = "screen")]
            public ScreenClass Screen { get; init; }

        }
    }
}
