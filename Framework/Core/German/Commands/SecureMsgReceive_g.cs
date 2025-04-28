/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT German interface.
 * SecureMsgReceive_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.German.Commands
{
    //Original name = SecureMsgReceive
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Command(Name = "German.SecureMsgReceive")]
    public sealed class SecureMsgReceiveCommand : Command<SecureMsgReceiveCommand.PayloadData>
    {
        public SecureMsgReceiveCommand(int RequestId, SecureMsgReceiveCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ProtocolEnum? Protocol = null, List<byte> Msg = null)
                : base()
            {
                this.Protocol = Protocol;
                this.Msg = Msg;
            }

            public enum ProtocolEnum
            {
                IsoPs,
                RawData,
                GenAs
            }

            /// <summary>
            /// Specifies the protocol the message belongs to. The following values are possible:
            /// 
            /// * ```isoPs``` - ISO 8583 protocol for the personalization system. (see [Protocol isoPs](#german.generalinformation.zka_german.protocolisops))
            /// * ```rawData``` - Raw data protocol. (see [Protocol rawData](#german.generalinformation.zka_german.protocolrawdata))
            /// * ```genAs``` - Generic PAC/MAC for non-ISO 8583 message formats. (see [Protocol genAs](#german.generalinformation.zka_german.protocolgenas))
            /// <example>isoPs</example>
            /// </summary>
            [DataMember(Name = "protocol")]
            public ProtocolEnum? Protocol { get; init; }

            /// <summary>
            /// Specifies the message that was received. This property is null if during a specified time period specified by [timeout](#german.securemsgreceive.command.properties.timeout) no 
            /// response was received from the communication partner 
            /// (necessary to set the internal state machine to the correct state).
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "msg")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Msg { get; init; }

        }
    }
}
