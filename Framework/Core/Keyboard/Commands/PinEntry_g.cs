/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * PinEntry_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Keyboard.Commands
{
    //Original name = PinEntry
    [DataContract]
    [Command(Name = "Keyboard.PinEntry")]
    public sealed class PinEntryCommand : Command<PinEntryCommand.PayloadData>
    {
        public PinEntryCommand(int RequestId, PinEntryCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? MinLen = null, int? MaxLen = null, bool? AutoEnd = null, string Echo = null, Dictionary<string, KeyClass> ActiveKeys = null)
                : base(Timeout)
            {
                this.MinLen = MinLen;
                this.MaxLen = MaxLen;
                this.AutoEnd = AutoEnd;
                this.Echo = Echo;
                this.ActiveKeys = ActiveKeys;
            }

            /// <summary>
            /// Specifies the minimum number of digits which must be entered for the PIN. 
            /// A value of zero indicates no minimum PIN length verification.
            /// </summary>
            [DataMember(Name = "minLen")]
            public int? MinLen { get; init; }

            /// <summary>
            /// Specifies the maximum number of digits which can be entered for the PIN.
            /// A value of zero indicates no maximum PIN length verification.
            /// </summary>
            [DataMember(Name = "maxLen")]
            public int? MaxLen { get; init; }

            /// <summary>
            /// If autoEnd is set to true, the Service Provider terminates the command when the maximum number of digits are entered. 
            /// Otherwise, the input is terminated by the user using one of the termination keys. 
            /// autoEnd is ignored when maxLen is set to zero.
            /// </summary>
            [DataMember(Name = "autoEnd")]
            public bool? AutoEnd { get; init; }

            /// <summary>
            /// Specifies the replace character to be echoed on a local display for the PIN digit. 
            /// </summary>
            [DataMember(Name = "echo")]
            public string Echo { get; init; }

            /// <summary>
            /// Specifies all Function Keys which are active during the execution of the command.
            /// This should be the complete set or a subset of the keys returned in the payload of the 
            /// [Keyboard.GetLayout](#keyboard.getlayout) command.
            /// 
            /// The following standard names are defined:
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
            /// Additional non standard key names are also allowed.
            /// </summary>
            [DataMember(Name = "activeKeys")]
            public Dictionary<string, KeyClass> ActiveKeys { get; init; }

        }
    }
}
