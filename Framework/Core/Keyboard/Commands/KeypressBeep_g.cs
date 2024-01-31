/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * KeypressBeep_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Keyboard.Commands
{
    //Original name = KeypressBeep
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Keyboard.KeypressBeep")]
    public sealed class KeypressBeepCommand : Command<KeypressBeepCommand.PayloadData>
    {
        public KeypressBeepCommand(int RequestId, KeypressBeepCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ModeClass Mode = null)
                : base()
            {
                this.Mode = Mode;
            }

            [DataContract]
            public sealed class ModeClass
            {
                public ModeClass(bool? Active = null, bool? Inactive = null)
                {
                    this.Active = Active;
                    this.Inactive = Inactive;
                }

                /// <summary>
                /// Specifies whether beeping should be enabled for active keys.
                /// </summary>
                [DataMember(Name = "active")]
                public bool? Active { get; init; }

                /// <summary>
                /// Specifies whether beeping should be enabled for inactive keys.
                /// </summary>
                [DataMember(Name = "inactive")]
                public bool? Inactive { get; init; }

            }

            /// <summary>
            /// Specifies whether automatic generation of key press beep tones should be activated for any active or
            /// inactive key subsequently pressed on the PIN. This selectively turns beeping on and off for active,
            /// inactive or both types of keys.
            /// </summary>
            [DataMember(Name = "mode")]
            public ModeClass Mode { get; init; }

        }
    }
}
