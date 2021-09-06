/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * DataEntry_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Keyboard.Commands
{
    //Original name = DataEntry
    [DataContract]
    [Command(Name = "Keyboard.DataEntry")]
    public sealed class DataEntryCommand : Command<DataEntryCommand.PayloadData>
    {
        public DataEntryCommand(int RequestId, DataEntryCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? MaxLen = null, bool? AutoEnd = null, string ActiveFDKs = null, string ActiveKeys = null, string TerminateFDKs = null, string TerminateKeys = null)
                : base(Timeout)
            {
                this.MaxLen = MaxLen;
                this.AutoEnd = AutoEnd;
                this.ActiveFDKs = ActiveFDKs;
                this.ActiveKeys = ActiveKeys;
                this.TerminateFDKs = TerminateFDKs;
                this.TerminateKeys = TerminateKeys;
            }

            /// <summary>
            /// Specifies the maximum number of digits which can be returned to the application in the output parameter. 
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
