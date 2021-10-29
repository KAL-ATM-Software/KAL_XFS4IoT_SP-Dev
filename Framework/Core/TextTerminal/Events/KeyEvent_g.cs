/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Event(Name = "TextTerminal.KeyEvent")]
    public sealed class KeyEvent : UnsolicitedEvent<KeyEvent.PayloadData>
    {

        public KeyEvent(PayloadData Payload)
            : base(Payload)
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
            /// On a numeric or alphanumeric key press this parameter holds the value of the key pressed. 
            /// This property is not required if no numeric or alphanumeric key was pressed.
            /// <example>0</example>
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            /// <summary>
            /// On a Command key press this parameter holds the value of the Command key pressed, e.g. 'enter'.
            /// This property is not required when no command key was pressed.
            /// <example>"enter"</example>
            /// </summary>
            [DataMember(Name = "commandKey")]
            [DataTypes(Pattern = @"^(enter|cancel|clear|backspace|help|doubleZero|tripleZero|arrowUp|arrowDown|arrowLeft|arrowRight)$|^fdk(0[1-9]|[12][0-9]|3[0-2])$|.+")]
            public string CommandKey { get; init; }

        }

    }
}
