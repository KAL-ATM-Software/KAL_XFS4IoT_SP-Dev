/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * Read_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.TextTerminal.Commands
{
    //Original name = Read
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "TextTerminal.Read")]
    public sealed class ReadCommand : Command<ReadCommand.PayloadData>
    {
        public ReadCommand()
            : base()
        { }

        public ReadCommand(int RequestId, ReadCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int? NumOfChars = null, ModesEnum? Mode = null, int? PosX = null, int? PosY = null, EchoModeEnum? EchoMode = null, EchoAttrClass EchoAttr = null, bool? Visible = null, bool? Flush = null, bool? AutoEnd = null, List<string> ActiveKeys = null, Dictionary<string, KeyClass> ActiveCommandKeys = null)
                : base()
            {
                this.NumOfChars = NumOfChars;
                this.Mode = Mode;
                this.PosX = PosX;
                this.PosY = PosY;
                this.EchoMode = EchoMode;
                this.EchoAttr = EchoAttr;
                this.Visible = Visible;
                this.Flush = Flush;
                this.AutoEnd = AutoEnd;
                this.ActiveKeys = ActiveKeys;
                this.ActiveCommandKeys = ActiveCommandKeys;
            }

            /// <summary>
            /// Specifies the number of printable characters (numeric and alphanumeric keys) that will be read from the
            /// Text Terminal Unit keypad. All command keys like 'enter', and 'fdk01' will not be counted.
            /// </summary>
            [DataMember(Name = "numOfChars")]
            [DataTypes(Minimum = 0)]
            public int? NumOfChars { get; init; }

            [DataMember(Name = "mode")]
            public ModesEnum? Mode { get; init; }

            /// <summary>
            /// If mode is *absolute*, this specifies the absolute horizontal position.
            /// If mode is *relative*, this specifies a horizontal offset relative to the
            /// current cursor position as a 0 based value.
            /// </summary>
            [DataMember(Name = "posX")]
            [DataTypes(Minimum = 0)]
            public int? PosX { get; init; }

            /// <summary>
            /// If mode is *absolute*, this specifies the absolute vertical position.
            /// If mode is *relative* this specifies a vertical offset relative to the
            /// current cursor position as a 0 based value.
            /// </summary>
            [DataMember(Name = "posY")]
            [DataTypes(Minimum = 0)]
            public int? PosY { get; init; }

            public enum EchoModeEnum
            {
                Text,
                Invisible,
                Password
            }

            /// <summary>
            /// Specifies how the user input is echoed to the screen as one of the following:
            /// * ```text``` - The user input is echoed to the screen.
            /// * ```invisible``` - The user input is not echoed to the screen.
            /// * ```password``` - The keys entered by the user are echoed as the replace character on the screen.
            /// </summary>
            [DataMember(Name = "echoMode")]
            public EchoModeEnum? EchoMode { get; init; }

            [DataContract]
            public sealed class EchoAttrClass
            {
                public EchoAttrClass(bool? Underline = null, bool? Inverted = null, bool? Flash = null)
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
            /// Specifies the text attributes with which the user input is echoed to the screen.
            /// If this property is null then the text will be displayed as normal text.
            /// </summary>
            [DataMember(Name = "echoAttr")]
            public EchoAttrClass EchoAttr { get; init; }

            /// <summary>
            /// Specifies whether the cursor is visible.
            /// </summary>
            [DataMember(Name = "visible")]
            public bool? Visible { get; init; }

            /// <summary>
            /// Specifies whether the keyboard input buffer is cleared before allowing for user input(true) or not (false).
            /// </summary>
            [DataMember(Name = "flush")]
            public bool? Flush { get; init; }

            /// <summary>
            /// Specifies whether the command input is automatically ended by the Service if the maximum number
            /// of printable characters as specified with _numOfChars_ is entered.
            /// </summary>
            [DataMember(Name = "autoEnd")]
            public bool? AutoEnd { get; init; }

            /// <summary>
            /// Specifying the numeric and alphanumeric keys on the Text Terminal Unit,
            /// e.g. ["one", "two", "A", "B", "a", "b"] to be active during the execution of the command.
            /// Devices having a shift key interpret this parameter differently from those that do not have a shift key.
            /// 
            /// For devices having a shift key, specifying only the upper case of a particular letter enables both the upper
            /// and lower case of that key, but the device converts lower case letters to upper case in the output
            /// parameter. To enable both upper and lower case keys, and have both upper and lower case letters returned,
            /// specify both the upper and lower case of the letter (e.g. ["one", "two", "A", "a", "B", "b"]).
            /// 
            /// For devices not having a shift key, specifying either the upper case only (e.g. ["one", "two", "A", "B"]),
            /// or specifying both the upper and lower case of a particular letter
            /// (e.g. ["one", "two", "A", "a", "B", "b"]), enables that key and causes the device to return the upper
            /// case of the letter in the output parameter.
            /// 
            /// For both types of device, specifying only lower case letters (e.g. ["one", "two", "a", "b"]) produces a key
            /// invalid error.
            /// This property is null if no keys of this type are active keys.
            /// 
            /// See predefined [keys](#textterminal.getkeydetail.completion.properties.keys).
            /// <example>["one", "nine"]</example>
            /// </summary>
            [DataMember(Name = "activeKeys")]
            [DataTypes(Pattern = @"^(zero|one|two|three|four|five|six|seven|eight|nine|\\D)$")]
            public List<string> ActiveKeys { get; init; }

            /// <summary>
            /// Specifying the command keys which are active during the execution of the command.
            /// This property is null if no keys of this type are active keys.
            /// </summary>
            [DataMember(Name = "activeCommandKeys")]
            public Dictionary<string, KeyClass> ActiveCommandKeys { get; init; }

        }
    }
}
