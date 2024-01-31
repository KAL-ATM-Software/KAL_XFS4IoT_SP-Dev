/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * GenerateKCV_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = GenerateKCV
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "KeyManagement.GenerateKCV")]
    public sealed class GenerateKCVCommand : Command<GenerateKCVCommand.PayloadData>
    {
        public GenerateKCVCommand(int RequestId, GenerateKCVCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string Key = null, KeyCheckModeEnum? KeyCheckMode = null)
                : base()
            {
                this.Key = Key;
                this.KeyCheckMode = KeyCheckMode;
            }

            /// <summary>
            /// Specifies the name of key that should be used to generate the KCV.
            /// <example>Key01</example>
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            public enum KeyCheckModeEnum
            {
                Self,
                Zero
            }

            /// <summary>
            /// Specifies the mode that is used to create the key check value. The following values are possible:
            /// 
            /// * ```self``` - The key check value (KCV) is created by an encryption of the key with itself. For the
            /// description refer to the *self* literal described in the
            /// [keyCheckModes](#common.capabilities.completion.properties.keymanagement.keycheckmodes).
            /// * ```zero``` - The key check value (KCV) is created by encrypting a zero value with the key. Unless
            /// otherwise specified, ECB encryption is used The encryption algorithm used (i.e. DES, 3DES, AES) is
            /// determined by the type of key used to generate the KCV.
            /// </summary>
            [DataMember(Name = "keyCheckMode")]
            public KeyCheckModeEnum? KeyCheckMode { get; init; }

        }
    }
}
