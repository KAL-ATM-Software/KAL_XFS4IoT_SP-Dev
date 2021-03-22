/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * Write_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
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
        public WriteCommand(string RequestId, WriteCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ModeEnum
            {
                Relative,
                Absolute,
            }

            /// <summary>
            ///Specifies the text attributes used for displaying the text. If none of the following attribute flags are selected then the text will be displayed as normal text.
            /// </summary>
            public class TextAttrClass
            {
                [DataMember(Name = "underline")] 
                public bool? Underline { get; private set; }
                [DataMember(Name = "inverted")] 
                public bool? Inverted { get; private set; }
                [DataMember(Name = "flash")] 
                public bool? Flash { get; private set; }

                public TextAttrClass (bool? Underline, bool? Inverted, bool? Flash)
                {
                    this.Underline = Underline;
                    this.Inverted = Inverted;
                    this.Flash = Flash;
                }


            }


            public PayloadData(int Timeout, ModeEnum? Mode = null, int? PosX = null, int? PosY = null, object TextAttr = null, string Text = null)
                : base(Timeout)
            {
                this.Mode = Mode;
                this.PosX = PosX;
                this.PosY = PosY;
                this.TextAttr = TextAttr;
                this.Text = Text;
            }

            /// <summary>
            ///Specifies whether the position of the output is absolute or relative to the current cursor position.
            /// </summary>
            [DataMember(Name = "mode")] 
            public ModeEnum? Mode { get; private set; }
            /// <summary>
            ///If mode is set to absolute, this specifies the absolute horizontal position. If mode is set to relative this specifies a horizontal offset relative to the current cursor position as a zero (0) based value.
            /// </summary>
            [DataMember(Name = "posX")] 
            public int? PosX { get; private set; }
            /// <summary>
            ///If mode is set to absolute, this specifies the absolute vertical position. If mode is set to relative this specifies a vertical offset relative to the current cursor position as a zero (0) based value.
            /// </summary>
            [DataMember(Name = "posY")] 
            public int? PosY { get; private set; }
            /// <summary>
            ///Specifies the text attributes used for displaying the text. If none of the following attribute flags are selected then the text will be displayed as normal text.
            /// </summary>
            [DataMember(Name = "textAttr")] 
            public object TextAttr { get; private set; }
            /// <summary>
            ///Specifies the text that will be displayed.
            /// </summary>
            [DataMember(Name = "text")] 
            public string Text { get; private set; }

        }
    }
}
