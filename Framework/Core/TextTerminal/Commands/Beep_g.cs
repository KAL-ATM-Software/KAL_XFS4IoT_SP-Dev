/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * Beep_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.TextTerminal.Commands
{
    //Original name = Beep
    [DataContract]
    [Command(Name = "TextTerminal.Beep")]
    public sealed class BeepCommand : Command<BeepCommand.PayloadData>
    {
        public BeepCommand(int RequestId, BeepCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, BeepClass Beep = null)
                : base(Timeout)
            {
                this.Beep = Beep;
            }

            [DataContract]
            public sealed class BeepClass
            {
                public BeepClass(bool? Off = null, bool? KeyPress = null, bool? Exclamation = null, bool? Warning = null, bool? Error = null, bool? Critical = null, bool? Continuous = null)
                {
                    this.Off = Off;
                    this.KeyPress = KeyPress;
                    this.Exclamation = Exclamation;
                    this.Warning = Warning;
                    this.Error = Error;
                    this.Critical = Critical;
                    this.Continuous = Continuous;
                }

                /// <summary>
                /// The beeper is turned off.
                /// </summary>
                [DataMember(Name = "off")]
                public bool? Off { get; init; }

                /// <summary>
                /// The beeper sounds a key click signal.
                /// </summary>
                [DataMember(Name = "keyPress")]
                public bool? KeyPress { get; init; }

                /// <summary>
                /// The beeper sounds an exclamation signal.
                /// </summary>
                [DataMember(Name = "exclamation")]
                public bool? Exclamation { get; init; }

                /// <summary>
                /// The beeper sounds a warning signal.
                /// </summary>
                [DataMember(Name = "warning")]
                public bool? Warning { get; init; }

                /// <summary>
                /// The beeper sounds a error signal.
                /// </summary>
                [DataMember(Name = "error")]
                public bool? Error { get; init; }

                /// <summary>
                /// The beeper sounds a critical error signal.
                /// </summary>
                [DataMember(Name = "critical")]
                public bool? Critical { get; init; }

                /// <summary>
                /// The beeper sound is turned on continuously.
                /// </summary>
                [DataMember(Name = "continuous")]
                public bool? Continuous { get; init; }

            }

            /// <summary>
            /// Specifies whether the beeper should be turned on or off.
            /// </summary>
            [DataMember(Name = "beep")]
            public BeepClass Beep { get; init; }

        }
    }
}
