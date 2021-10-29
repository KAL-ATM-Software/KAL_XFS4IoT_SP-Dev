/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * KeyEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Keyboard.Events
{

    [DataContract]
    [Event(Name = "Keyboard.KeyEvent")]
    public sealed class KeyEvent : Event<KeyEvent.PayloadData>
    {

        public KeyEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(EntryCompletionEnum? Completion = null, string Digit = null)
                : base()
            {
                this.Completion = Completion;
                this.Digit = Digit;
            }

            [DataMember(Name = "completion")]
            public EntryCompletionEnum? Completion { get; init; }

            /// <summary>
            /// Specifies the digit entered by the user. When working in encryption mode or secure key entry mode ([Keyboard.PinEntry](#keyboard.pinentry) and [Keyboard.SecureKeyEntry](#keyboard.securekeyentry)), this property is omitted for the 
            /// function keys 'one' to 'nine' and 'a' to 'f'. Otherwise, for each key pressed, the corresponding key value is stored in this property. 
            /// <example>five</example>
            /// </summary>
            [DataMember(Name = "digit")]
            [DataTypes(Pattern = @"^(one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|doubleZero|tripleZero)$|^fdk(0[1-9]|[12][0-9]|3[0-2])$|.+")]
            public string Digit { get; init; }

        }

    }
}
