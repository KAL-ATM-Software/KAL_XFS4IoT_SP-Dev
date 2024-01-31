/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Keyboard.PinEntry")]
    public sealed class PinEntryCommand : Command<PinEntryCommand.PayloadData>
    {
        public PinEntryCommand(int RequestId, PinEntryCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int? MinLen = null, int? MaxLen = null, bool? AutoEnd = null, string Echo = null, Dictionary<string, ActiveKeysClass> ActiveKeys = null)
                : base()
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
            [DataTypes(Minimum = 0)]
            public int? MinLen { get; init; }

            /// <summary>
            /// Specifies the maximum number of digits which can be entered for the PIN.
            /// A value of zero indicates no maximum PIN length verification.
            /// </summary>
            [DataMember(Name = "maxLen")]
            [DataTypes(Minimum = 0)]
            public int? MaxLen { get; init; }

            /// <summary>
            /// If *autoEnd* is set to true, the Service Provider terminates the command when the maximum number of
            /// digits are entered. Otherwise, the input is terminated by the user using one of the termination keys.
            /// *autoEnd* is ignored when *maxLen* is set to zero.
            /// </summary>
            [DataMember(Name = "autoEnd")]
            public bool? AutoEnd { get; init; }

            /// <summary>
            /// Specifies the replace character to be echoed on a local display for the PIN digit.
            /// This property will be ignored by the service if the device doesn't have a local display.
            /// <example>X</example>
            /// </summary>
            [DataMember(Name = "echo")]
            public string Echo { get; init; }

            [DataContract]
            public sealed class ActiveKeysClass
            {
                public ActiveKeysClass(bool? Terminate = null)
                {
                    this.Terminate = Terminate;
                }

                /// <summary>
                /// The key is a terminate key.
                /// </summary>
                [DataMember(Name = "terminate")]
                public bool? Terminate { get; init; }

            }

            /// <summary>
            /// Specifies all Function Keys which are active during the execution of the command.
            /// This should be the complete set or a subset of the keys returned in the payload of the
            /// [Keyboard.GetLayout](#keyboard.getlayout) command.
            /// </summary>
            [DataMember(Name = "activeKeys")]
            public Dictionary<string, ActiveKeysClass> ActiveKeys { get; init; }

        }
    }
}
