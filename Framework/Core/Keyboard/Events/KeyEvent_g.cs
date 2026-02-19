/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.0")]
    [Event(Name = "Keyboard.KeyEvent")]
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

            public PayloadData(EntryCompletionEnum? Completion = null, string Digit = null)
                : base()
            {
                this.Completion = Completion;
                this.Digit = Digit;
            }

            [DataMember(Name = "completion")]
            public EntryCompletionEnum? Completion { get; init; }

            /// <summary>
            /// Specifies the digit entered by the user. When working in encryption mode or secure key entry mode
            /// ([Keyboard.PinEntry](#keyboard.pinentry) and [Keyboard.SecureKeyEntry](#keyboard.securekeyentry)), this
            /// property is null for the function keys 'one' to 'nine' and 'a' to 'f'. Otherwise, for each key pressed,
            /// the corresponding key value is stored in this property.
            /// 
            /// The following standard values are defined:
            /// 
            /// * ```zero``` - Numeric digit 0
            /// * ```one``` - Numeric digit 1
            /// * ```two``` - Numeric digit 2
            /// * ```three``` - Numeric digit 3
            /// * ```four``` - Numeric digit 4
            /// * ```five``` - Numeric digit 5
            /// * ```six``` - Numeric digit 6
            /// * ```seven``` - Numeric digit 7
            /// * ```eight``` - Numeric digit 8
            /// * ```nine``` - Numeric digit 9
            /// * ```[a-f]``` - Hex digit A to F for secure key entry
            /// * ```enter``` - Enter
            /// * ```cancel``` - Cancel
            /// * ```clear``` - Clear
            /// * ```backspace``` - Backspace
            /// * ```help``` - Help
            /// * ```decPoint``` - Decimal point
            /// * ```shift``` - Shift key used during hex entry
            /// * ```doubleZero``` - 00
            /// * ```tripleZero``` - 000
            /// * ```fdk[01-32]``` - 32 FDK keys
            /// 
            /// Additional non-standard values are also allowed:
            /// 
            /// * ```oem[a-zA-Z0-9]*``` - A non-standard value
            /// <example>five</example>
            /// </summary>
            [DataMember(Name = "digit")]
            [DataTypes(Pattern = @"^(zero|one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|doubleZero|tripleZero|fdk(0[1-9]|[12][0-9]|3[0-2])|oem[a-zA-Z0-9]*)$")]
            public string Digit { get; init; }

        }

    }
}
