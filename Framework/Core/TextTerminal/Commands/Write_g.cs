/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * Write_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.TextTerminal.Commands
{
    //Original name = Write
    [DataContract]
    [Command(Name = "TextTerminal.Write")]
    public sealed class WriteCommand : Command<WriteCommand.PayloadData>
    {
        public WriteCommand(int RequestId, WriteCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, ModesEnum? Mode = null, int? PosX = null, int? PosY = null, TextAttrClass TextAttr = null, string Text = null)
                : base(Timeout)
            {
                this.Mode = Mode;
                this.PosX = PosX;
                this.PosY = PosY;
                this.TextAttr = TextAttr;
                this.Text = Text;
            }


            [DataMember(Name = "mode")]
            public ModesEnum? Mode { get; private set; }

            /// <summary>
            /// If mode is set to absolute, this specifies the absolute horizontal position. If mode is set to relative 
            /// this specifies a horizontal offset relative to the current cursor position as a zero (0) based value.
            /// </summary>
            [DataMember(Name = "posX")]
            public int? PosX { get; private set; }

            /// <summary>
            /// If mode is set to absolute, this specifies the absolute vertical position. If mode is set to relative 
            /// this specifies a vertical offset relative to the current cursor position as a zero (0) based value.
            /// </summary>
            [DataMember(Name = "posY")]
            public int? PosY { get; private set; }

            [DataContract]
            public sealed class TextAttrClass
            {
                public TextAttrClass(bool? Underline = null, bool? Inverted = null, bool? Flash = null)
                {
                    this.Underline = Underline;
                    this.Inverted = Inverted;
                    this.Flash = Flash;
                }

                /// <summary>
                /// The displayed text will be unerlined.
                /// </summary>
                [DataMember(Name = "underline")]
                public bool? Underline { get; private set; }

                /// <summary>
                /// The displayed text will be inverted.
                /// </summary>
                [DataMember(Name = "inverted")]
                public bool? Inverted { get; private set; }

                /// <summary>
                /// The displayed text will be flashing.
                /// </summary>
                [DataMember(Name = "flash")]
                public bool? Flash { get; private set; }

            }

            /// <summary>
            /// Specifies the text attributes used for displaying the text. If none of the following attribute flags 
            /// are selected then the text will be displayed as normal text.
            /// </summary>
            [DataMember(Name = "textAttr")]
            public TextAttrClass TextAttr { get; private set; }

            /// <summary>
            /// Specifies the text that will be displayed.
            /// </summary>
            [DataMember(Name = "text")]
            public string Text { get; private set; }

        }
    }
}
