/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT German interface.
 * SecureMsgSend_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.German.Commands
{
    //Original name = SecureMsgSend
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Command(Name = "German.SecureMsgSend")]
    public sealed class SecureMsgSendCommand : Command<SecureMsgSendCommand.PayloadData>
    {
        public SecureMsgSendCommand()
            : base()
        { }

        public SecureMsgSendCommand(int RequestId, SecureMsgSendCommand.PayloadData Payload, int Timeout)
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
                HsmLdi,
                GenAs,
                PinCmp
            }

            /// <summary>
            /// Specifies the protocol the message belongs to. The following values are possible:
            /// 
            /// * ```isoPs``` - ISO 8583 protocol for the personalization system. (see [Protocol isoPs](#german.generalinformation.zka_german.protocolisops))
            /// * ```rawData``` - Raw data protocol. (see [Protocol rawData](#german.generalinformation.zka_german.protocolrawdata))
            /// * ```hsmLdi``` - HSM LDI protocol. (see [Protocol hsmLdi](#german.generalinformation.zka_german.protocolhsmldi))
            /// * ```genAs``` - Generic PAC/MAC for non-ISO 8583 message formats. (see [Protocol genAs](#german.generalinformation.zka_german.protocolgenas))
            /// * ```pinCmp``` - Protocol for comparing PINs entered in the PIN pad during a PIN Change transaction. (see [Protocol pinCmp](#german.generalinformation.zka_german.protocolpincmp))
            /// </summary>
            [DataMember(Name = "protocol")]
            public ProtocolEnum? Protocol { get; init; }

            /// <summary>
            /// Specifies the message that should be sent. This property is null if the *protocol* property is set to *hsmLdi*.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "msg")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Msg { get; init; }

        }
    }
}
