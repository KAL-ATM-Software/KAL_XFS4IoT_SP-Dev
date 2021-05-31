/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "TextTerminal.ClearScreen")]
    public sealed class ClearScreenCommand : Command<ClearScreenCommand.PayloadData>
    {
        public ClearScreenCommand(int RequestId, ClearScreenCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? PositionX = null, int? PositionY = null, int? Width = null, int? Height = null)
                : base(Timeout)
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
            public int? PositionX { get; private set; }

            /// <summary>
            /// Specifies the vertical position of the area to be cleared.
            /// </summary>
            [DataMember(Name = "positionY")]
            public int? PositionY { get; private set; }

            /// <summary>
            /// Specifies the width position of the area to be cleared.
            /// </summary>
            [DataMember(Name = "width")]
            public int? Width { get; private set; }

            /// <summary>
            /// Specifies the height position of the area to be cleared.
            /// </summary>
            [DataMember(Name = "height")]
            public int? Height { get; private set; }

        }
    }
}
