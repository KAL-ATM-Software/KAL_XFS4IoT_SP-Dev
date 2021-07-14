/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "TextTerminal.Read")]
    public sealed class ReadCommand : Command<ReadCommand.PayloadData>
    {
        public ReadCommand(int RequestId, ReadCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? NumOfChars = null, ModesEnum? Mode = null, int? PosX = null, int? PosY = null, EchoModeEnum? EchoMode = null, EchoAttrClass EchoAttr = null, bool? Echo = null, bool? Flush = null, bool? AutoEnd = null, string ActiveKeys = null, List<string> ActiveCommandKeys = null, List<string> TerminateCommandKeys = null)
                : base(Timeout)
            {
                this.NumOfChars = NumOfChars;
                this.Mode = Mode;
                this.PosX = PosX;
                this.PosY = PosY;
                this.EchoMode = EchoMode;
                this.EchoAttr = EchoAttr;
                this.Echo = Echo;
                this.Flush = Flush;
                this.AutoEnd = AutoEnd;
                this.ActiveKeys = ActiveKeys;
                this.ActiveCommandKeys = ActiveCommandKeys;
                this.TerminateCommandKeys = TerminateCommandKeys;
            }

            /// <summary>
            /// Specifies the number of printable characters (numeric and alphanumeric keys) that will be read from the 
            /// text terminal unit key pad. All command keys like ckEnter, ckFDK01 will not be counted.
            /// </summary>
            [DataMember(Name = "numOfChars")]
            [DataTypes(Minimum = 0)]
            public int? NumOfChars { get; init; }


            [DataMember(Name = "mode")]
            public ModesEnum? Mode { get; init; }

            /// <summary>
            /// If mode is set to absolute, this specifies the absolute horizontal position. 
            /// If mode is set to relative this specifies a horizontal offset relative to the 
            /// current cursor position as a zero (0) based value.
            /// </summary>
            [DataMember(Name = "posX")]
            [DataTypes(Minimum = 0)]
            public int? PosX { get; init; }

            /// <summary>
            /// If mode is set to absolute, this specifies the absolute vertical position. 
            /// If mode is set to relative this specifies a vertical offset relative to the 
            /// current cursor position as a zero (0) based value.
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
            /// Specifies how the user input is echoed to the screen as one of the following flags:
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
            /// If none of the following attribute flags are selected then the text will be displayed as normal text.
            /// </summary>
            [DataMember(Name = "echoAttr")]
            public EchoAttrClass EchoAttr { get; init; }

            /// <summary>
            /// Specifies whether the cursor is visible(true) or invisible(false).
            /// </summary>
            [DataMember(Name = "echo")]
            public bool? Echo { get; init; }

            /// <summary>
            /// Specifies whether the keyboard input buffer is cleared before allowing for user input(true) or not (false).
            /// </summary>
            [DataMember(Name = "flush")]
            public bool? Flush { get; init; }

            /// <summary>
            /// Specifies whether the command input is automatically ended by Service Provider if the maximum number 
            /// of printable characters as specified with numOfChars is entered.
            /// </summary>
            [DataMember(Name = "autoEnd")]
            public bool? AutoEnd { get; init; }

            /// <summary>
            /// String which specifies the numeric and alphanumeric keys on the Text Terminal Unit,
            /// e.g. "12ABab", to be active during the execution of the command. Devices having a shift key interpret 
            /// this parameter differently from those that do not have a shift key. For devices having a shift key, 
            /// specifying only the upper case of a particular letter enables both upper and lower case of that key, 
            /// but the device converts lower case letters to upper case in the output parameter. To enable both 
            /// upper and lower case keys, and have both upper and lower case letters returned, specify both the 
            /// upper and lower case of the letter (e.g. "12AaBb"). For devices not having a shift key, specifying 
            /// either the upper case only (e.g. "12AB"), or specifying both the upper and lower case of a particular letter 
            /// (e.g. "12AaBb"), enables that key and causes the device to return the upper case of the letter in the output parameter. 
            /// For both types of device, specifying only lower case letters (e.g. "12ab") produces a key invalid error. 
            /// This parameter is a NULL if no keys of this type are active keys. activeKeys and activeUnicodeKeys are 
            /// mutually exclusive, so activeKeys field must not be set  if activeUnicodeKeys field is not set.
            /// </summary>
            [DataMember(Name = "activeKeys")]
            public string ActiveKeys { get; init; }

            /// <summary>
            /// Array specifying the command keys which are active during the execution of the command. 
            /// The array is terminated with a zero value and this array is not set if no keys of this type are active keys.                      
            /// </summary>
            [DataMember(Name = "activeCommandKeys")]
            public List<string> ActiveCommandKeys { get; init; }

            /// <summary>
            /// Array specifying the command keys which must terminate the execution of the command. 
            /// The array is terminated with a zero value and this array is not set if no keys of this type are terminate keys.
            /// </summary>
            [DataMember(Name = "terminateCommandKeys")]
            public List<string> TerminateCommandKeys { get; init; }

        }
    }
}
