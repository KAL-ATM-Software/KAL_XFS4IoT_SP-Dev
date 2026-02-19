/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * SetKey_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = SetKey
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "CardReader.SetKey")]
    public sealed class SetKeyCommand : Command<SetKeyCommand.PayloadData>
    {
        public SetKeyCommand()
            : base()
        { }

        public SetKeyCommand(int RequestId, SetKeyCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<byte> KeyValue = null)
                : base()
            {
                this.KeyValue = KeyValue;
            }

            /// <summary>
            /// Contains the Base64 encoded payment containing the CIM86 DES key. This key is supplied by the vendor
            /// of the CIM86 module.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "keyValue")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> KeyValue { get; init; }

        }
    }
}
