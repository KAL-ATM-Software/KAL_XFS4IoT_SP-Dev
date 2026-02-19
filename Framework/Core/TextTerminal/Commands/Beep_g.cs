/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.0")]
    [Command(Name = "TextTerminal.Beep")]
    public sealed class BeepCommand : Command<BeepCommand.PayloadData>
    {
        public BeepCommand()
            : base()
        { }

        public BeepCommand(int RequestId, BeepCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(BeepClass Beep = null)
                : base()
            {
                this.Beep = Beep;
            }

            [DataContract]
            public sealed class BeepClass
            {
                public BeepClass(bool? Continuous = null, BeepTypeEnum? BeepType = null)
                {
                    this.Continuous = Continuous;
                    this.BeepType = BeepType;
                }

                /// <summary>
                /// Specifies whether the beep is continuous.
                /// </summary>
                [DataMember(Name = "continuous")]
                public bool? Continuous { get; init; }

                public enum BeepTypeEnum
                {
                    KeyPress,
                    Exclamation,
                    Warning,
                    Error,
                    Critical
                }

                /// <summary>
                /// Specifies the type of beep as one of the following:
                /// 
                /// * ```keyPress``` - The beeper sounds a key click signal.
                /// * ```exclamation``` - The beeper sounds an exclamation signal.
                /// * ```warning``` - The beeper sounds a warning signal.
                /// * ```error``` - The beeper sounds an error signal.
                /// * ```critical``` - The beeper sounds a critical signal.
                /// </summary>
                [DataMember(Name = "beepType")]
                public BeepTypeEnum? BeepType { get; init; }

            }

            /// <summary>
            /// Specifies whether the beeper should be turned on or off.
            /// </summary>
            [DataMember(Name = "beep")]
            public BeepClass Beep { get; init; }

        }
    }
}
