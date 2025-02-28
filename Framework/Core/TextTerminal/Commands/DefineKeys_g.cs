/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.0")]
    [Command(Name = "TextTerminal.DefineKeys")]
    public sealed class DefineKeysCommand : Command<DefineKeysCommand.PayloadData>
    {
        public DefineKeysCommand(int RequestId, DefineKeysCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<string> ActiveKeys = null, Dictionary<string, KeyClass> ActiveCommandKeys = null)
                : base()
            {
                this.ActiveKeys = ActiveKeys;
                this.ActiveCommandKeys = ActiveCommandKeys;
            }

            /// <summary>
            /// String array which specifies the alphanumeric keys on the Text Terminal Unit, e.g. ["one", "two", "B", "a", "b"],
            /// to be active during the execution of the next [TextTerminal.ReadForm](#textterminal.readform) command.
            /// 
            /// Devices having a shift key interpret this parameter differently from those that do not have a shift key.
            /// For devices having a shift key, specifying only the upper case of a particular letter enables both
            /// the upper and lower case of that key, but the device converts lower case letters to upper case in the output parameter.
            /// To enable both upper and lower case keys, and have both upper and lower case letters returned,
            /// specify both the upper and lower case of the letter (e.g. ["one", "two", "A", "a", "B", "b"]).
            /// 
            /// For devices not having a shift key, specifying either the upper case only (e.g. ["one", "two", "A", "B"]),
            /// or specifying both the upper and lower case of a particular letter (e.g. ["one", "two", "A", "a", "B", "b"]),
            /// enables that key and causes the device to return the upper case of the letter in the output parameter.
            /// 
            /// For both types of device, specifying only lower case letters (e.g. "12ab"["one", "two", "a", "b"]) produces a key invalid error.
            /// 
            /// See predefined [keys](#textterminal.getkeydetail.completion.properties.keys).
            /// <example>["one", "nine"]</example>
            /// </summary>
            [DataMember(Name = "activeKeys")]
            [DataTypes(Pattern = @"^(zero|one|two|three|four|five|six|seven|eight|nine|\\D)$")]
            public List<string> ActiveKeys { get; init; }

            /// <summary>
            /// Array specifying the command keys which are active during the execution of the next [TextTerminal.ReadForm](#textterminal.readform) command.
            /// This property is null if no active command keys are required.
            /// </summary>
            [DataMember(Name = "activeCommandKeys")]
            public Dictionary<string, KeyClass> ActiveCommandKeys { get; init; }

        }
    }
}
