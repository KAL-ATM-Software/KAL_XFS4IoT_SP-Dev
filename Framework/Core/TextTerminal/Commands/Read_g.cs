/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * Read_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
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
        public ReadCommand(string RequestId, ReadCommand.PayloadData Payload)
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

            public enum EchoModeEnum
            {
                Text,
                Invisible,
                Password,
            }

            /// <summary>
            ///Specifies the text attributes with which the user input is echoed to the screen. If none of the following attribute flags are selected then the text will be displayed as normal text.
            /// </summary>
            public class EchoAttrClass
            {
                [DataMember(Name = "underline")] 
                public bool? Underline { get; private set; }
                [DataMember(Name = "inverted")] 
                public bool? Inverted { get; private set; }
                [DataMember(Name = "flash")] 
                public bool? Flash { get; private set; }

                public EchoAttrClass (bool? Underline, bool? Inverted, bool? Flash)
                {
                    this.Underline = Underline;
                    this.Inverted = Inverted;
                    this.Flash = Flash;
                }


            }


            public PayloadData(int Timeout, int? NumOfChars = null, ModeEnum? Mode = null, int? PosX = null, int? PosY = null, EchoModeEnum? EchoMode = null, object EchoAttr = null, bool? Echo = null, bool? Flush = null, bool? AutoEnd = null, string ActiveKeys = null, List<string> ActiveCommandKeys = null, List<string> TerminateCommandKeys = null)
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
            ///Specifies the number of printable characters (numeric and alphanumeric keys) that will be read from the text terminal unit key pad.All command keys like ckEnter, ckFDK01 will not be counted.
            /// </summary>
            [DataMember(Name = "numOfChars")] 
            public int? NumOfChars { get; private set; }
            /// <summary>
            ///Specifies where the cursor is positioned for the read operation.
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
            ///Specifies how the user input is echoed to the screen.
            /// </summary>
            [DataMember(Name = "echoMode")] 
            public EchoModeEnum? EchoMode { get; private set; }
            /// <summary>
            ///Specifies the text attributes with which the user input is echoed to the screen. If none of the following attribute flags are selected then the text will be displayed as normal text.
            /// </summary>
            [DataMember(Name = "echoAttr")] 
            public object EchoAttr { get; private set; }
            /// <summary>
            ///Specifies whether the cursor is visible(TRUE) or invisible(FALSE).
            /// </summary>
            [DataMember(Name = "echo")] 
            public bool? Echo { get; private set; }
            /// <summary>
            ///Specifies whether the keyboard input buffer is cleared before allowing for user input(TRUE) or not (FALSE).
            /// </summary>
            [DataMember(Name = "flush")] 
            public bool? Flush { get; private set; }
            /// <summary>
            ///Specifies whether the command input is automatically ended by Service Provider if the maximum number of printable characters as specified with numOfChars is entered.
            /// </summary>
            [DataMember(Name = "autoEnd")] 
            public bool? AutoEnd { get; private set; }
            /// <summary>
            ///String which specifies the numeric and alphanumeric keys on the Text Terminal Unit,e.g. \"12ABab\", to be active during the execution of the command. Devices having a shift key interpret this parameter differently from those that do not have a shift key. For devices having a shift key, specifying only the upper case of a particular letter enables both upper and lower case of that key, but the device converts lower case letters to upper case in the output parameter. To enable both upper and lower case keys, and have both upper and lower case letters returned, specify both the upper and lower case of the letter (e.g. \"12AaBb\"). For devices not having a shift key, specifying either the upper case only (e.g. \"12AB\"), or specifying both the upper and lower case of a particular letter (e.g. \"12AaBb\"), enables that key and causes the device to return the upper case of the letter in the output parameter. For both types of device, specifying only lower case letters (e.g. \"12ab\") produces a key invalid error. This parameter is a NULL if no keys of this type are active keys. activeKeys and activeUnicodeKeys are mutually exclusive, so activeKeys field must not be set  if activeUnicodeKeys field is not set.
            /// </summary>
            [DataMember(Name = "activeKeys")] 
            public string ActiveKeys { get; private set; }
            /// <summary>
            ///Array specifying the command keys which are active during the execution of the command. The array is terminated with a zero value and this array is not set if no keys of this type are active keys.                      
            /// </summary>
            [DataMember(Name = "activeCommandKeys")] 
            public List<string> ActiveCommandKeys{ get; private set; }
            /// <summary>
            ///Array specifying the command keys which must terminate the execution of the command. The array is terminated with a zero value and this array is not set if no keys of this type are terminate keys.
            /// </summary>
            [DataMember(Name = "terminateCommandKeys")] 
            public List<string> TerminateCommandKeys{ get; private set; }

        }
    }
}
