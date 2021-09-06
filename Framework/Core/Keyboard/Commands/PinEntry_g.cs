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

            public PayloadData(int Timeout, int? MinLen = null, int? MaxLen = null, bool? AutoEnd = null, int? Echo = null, string ActiveFDKs = null, string ActiveKeys = null, string TerminateFDKs = null, string TerminateKeys = null)
                : base(Timeout)
            {
                this.MinLen = MinLen;
                this.MaxLen = MaxLen;
                this.AutoEnd = AutoEnd;
                this.Echo = Echo;
                this.ActiveFDKs = ActiveFDKs;
                this.ActiveKeys = ActiveKeys;
                this.TerminateFDKs = TerminateFDKs;
                this.TerminateKeys = TerminateKeys;
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
            public int? Echo { get; init; }

            /// <summary>
            /// Specifies a mask of those FDKs which are active during the execution of the command.
            /// </summary>
            [DataMember(Name = "activeFDKs")]
            [DataTypes(Pattern = "^(one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|res0[1-8]|oem0[1-6]|doubleZero|tripleZero)$|^fdk(0[1-9]|[12][0-9]|3[0-2])$")]
            public string ActiveFDKs { get; init; }

            /// <summary>
            /// Specifies a mask of those (other) Function Keys which are active during the execution of the command.
            /// </summary>
            [DataMember(Name = "activeKeys")]
            [DataTypes(Pattern = "^(one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|res0[1-8]|oem0[1-6]|doubleZero|tripleZero)$|^fdk(0[1-9]|[12][0-9]|3[0-2])$")]
            public string ActiveKeys { get; init; }

            /// <summary>
            /// Specifies a mask of those FDKs which must terminate the execution of the command.
            /// </summary>
            [DataMember(Name = "terminateFDKs")]
            [DataTypes(Pattern = "^(one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|res0[1-8]|oem0[1-6]|doubleZero|tripleZero)$|^fdk(0[1-9]|[12][0-9]|3[0-2])$")]
            public string TerminateFDKs { get; init; }

            /// <summary>
            /// Specifies a mask of those (other) Function Keys which must terminate the execution of the command.
            /// </summary>
            [DataMember(Name = "terminateKeys")]
            [DataTypes(Pattern = "^(one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|res0[1-8]|oem0[1-6]|doubleZero|tripleZero)$|^fdk(0[1-9]|[12][0-9]|3[0-2])$")]
            public string TerminateKeys { get; init; }

        }
    }
}
