/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [Command(Name = "CardReader.SetKey")]
    public sealed class SetKeyCommand : Command<SetKeyCommand.PayloadData>
    {
        public SetKeyCommand(int RequestId, SetKeyCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, List<byte> KeyValue = null)
                : base(Timeout)
            {
                this.KeyValue = KeyValue;
            }

            /// <summary>
            /// Contains the Base64 encoded payment containing the CIM86 DES key. This key is supplied by the vendor
            /// of the CIM86 module.
            /// <example>QmFzZTY0IGVuY29kZWQg ...</example>
            /// </summary>
            [DataMember(Name = "keyValue")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> KeyValue { get; init; }

        }
    }
}
