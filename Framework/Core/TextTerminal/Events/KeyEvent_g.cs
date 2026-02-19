/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * KeyEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.TextTerminal.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "TextTerminal.KeyEvent")]
    public sealed class KeyEvent : Event<KeyEvent.PayloadData>
    {

        public KeyEvent()
            : base()
        { }

        public KeyEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string Key = null, string CommandKey = null)
                : base()
            {
                this.Key = Key;
                this.CommandKey = CommandKey;
            }

            /// <summary>
            /// Specifies the command key supported.
            /// 
            /// See predefined [keys](#textterminal.getkeydetail.completion.properties.keys).
            /// </summary>
            [DataMember(Name = "key")]
            [DataTypes(Pattern = @"^(zero|one|two|three|four|five|six|seven|eight|nine|\\D)$")]
            public string Key { get; init; }

            /// <summary>
            /// Specifies the command key supported.
            /// 
            /// See predefined [keys](#textterminal.getkeydetail.completion.properties.commandkeys).
            /// </summary>
            [DataMember(Name = "commandKey")]
            [DataTypes(Pattern = @"^(enter|cancel|clear|backspace|help|doubleZero|tripleZero|arrowUp|arrowDown|arrowLeft|arrowRight|fdk(0[1-9]|[12][0-9]|3[0-2])|oem[A-Za-z0-9]*)$")]
            public string CommandKey { get; init; }

        }

    }
}
