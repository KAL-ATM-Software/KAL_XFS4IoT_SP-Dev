/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.0")]
    [Command(Name = "TextTerminal.Write")]
    public sealed class WriteCommand : Command<WriteCommand.PayloadData>
    {
        public WriteCommand()
            : base()
        { }

        public WriteCommand(int RequestId, WriteCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ModesEnum? Mode = null, int? PosX = null, int? PosY = null, TextAttrClass TextAttr = null, string Text = null)
                : base()
            {
                this.Mode = Mode;
                this.PosX = PosX;
                this.PosY = PosY;
                this.TextAttr = TextAttr;
                this.Text = Text;
            }

            [DataMember(Name = "mode")]
            public ModesEnum? Mode { get; init; }

            /// <summary>
            /// If mode is set to absolute, this specifies the absolute horizontal position. If mode is *relative*,
            /// this specifies a horizontal offset relative to the current cursor position as a 0 based value.
            /// </summary>
            [DataMember(Name = "posX")]
            [DataTypes(Minimum = 0)]
            public int? PosX { get; init; }

            /// <summary>
            /// If mode is set to absolute, this specifies the absolute vertical position. If mode is *relative*,
            /// this specifies a vertical offset relative to the current cursor position as a 0 based value.
            /// </summary>
            [DataMember(Name = "posY")]
            [DataTypes(Minimum = 0)]
            public int? PosY { get; init; }

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
                /// The displayed text will be underlined.
                /// </summary>
                [DataMember(Name = "underline")]
                public bool? Underline { get; init; }

                /// <summary>
                /// The displayed text will be inverted.
                /// </summary>
                [DataMember(Name = "inverted")]
                public bool? Inverted { get; init; }

                /// <summary>
                /// The displayed text will be flashing.
                /// </summary>
                [DataMember(Name = "flash")]
                public bool? Flash { get; init; }

            }

            /// <summary>
            /// Specifies the text attributes used for displaying the text.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "textAttr")]
            public TextAttrClass TextAttr { get; init; }

            /// <summary>
            /// Specifies the text that will be displayed.
            /// <example>Text to display</example>
            /// </summary>
            [DataMember(Name = "text")]
            public string Text { get; init; }

        }
    }
}
