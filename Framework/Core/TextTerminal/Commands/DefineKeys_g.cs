/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * DefineKeys_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.TextTerminal.Commands
{
    //Original name = DefineKeys
    [DataContract]
    [Command(Name = "TextTerminal.DefineKeys")]
    public sealed class DefineKeysCommand : Command<DefineKeysCommand.PayloadData>
    {
        public DefineKeysCommand(int RequestId, DefineKeysCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string ActiveKeys = null, Dictionary<string, KeyClass> ActiveCommandKeys = null)
                : base(Timeout)
            {
                this.ActiveKeys = ActiveKeys;
                this.ActiveCommandKeys = ActiveCommandKeys;
            }

            /// <summary>
            /// String which specifies the alphanumeric keys on the Text Terminal Unit, e.g. "12ABab", to be active during the execution 
            /// of the next [TextTerminal.ReadForm](#textterminal.readform) command. Devices having a shift key interpret this parameter differently from those that 
            /// do not have a shift key. For devices having a shift key, specifying only the upper case of a particular letter enables both 
            /// upper and lower case of that key, but the device converts lower case letters to upper case in the output parameter. To enable 
            /// both upper and lower case keys, and have both upper and lower case letters returned, specify both the upper and lower case of 
            /// the letter (e.g. "12AaBb"). For devices not having a shift key, specifying either the upper case only (e.g. "12AB"), or specifying 
            /// both the upper and lower case of a particular letter (e.g. "12AaBb"), enables that key and causes the device to return the upper 
            /// case of the letter in the output parameter. For both types of device, specifying only lower case letters (e.g. "12ab") produces a 
            /// key invalid error.
            /// <example>0123456789ABCabc</example>
            /// </summary>
            [DataMember(Name = "activeKeys")]
            public string ActiveKeys { get; init; }

            /// <summary>
            /// Array specifying the command keys which are active during the execution of the next [TextTerminal.ReadForm](#textterminal.readform) command.                       
            /// The following standard names are defined:
            /// 
            ///   * ```enter``` - Enter
            ///   * ```cancel``` - Cancel
            ///   * ```clear``` - Clear
            ///   * ```backspace``` - Backspace
            ///   * ```help``` - Help
            ///   * ```doubleZero``` - 00
            ///   * ```tripleZero``` - 000
            ///   * ```arrowUp``` - up arrow
            ///   * ```arrowDown``` - down arrow
            ///   * ```arrowLeft``` - left arrow
            ///   * ```arrowRight``` - right arrow
            ///   * ```fdk[01-32]``` - 32 FDK keys
            /// 
            ///   Additional non standard key names are also allowed.
            /// </summary>
            [DataMember(Name = "activeCommandKeys")]
            public Dictionary<string, KeyClass> ActiveCommandKeys { get; init; }

        }
    }
}
