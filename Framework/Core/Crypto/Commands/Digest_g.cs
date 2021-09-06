/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * Digest_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Crypto.Commands
{
    //Original name = Digest
    [DataContract]
    [Command(Name = "Crypto.Digest")]
    public sealed class DigestCommand : Command<DigestCommand.PayloadData>
    {
        public DigestCommand(int RequestId, DigestCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, HashAlgorithmEnum? HashAlgorithm = null, string DigestInput = null)
                : base(Timeout)
            {
                this.HashAlgorithm = HashAlgorithm;
                this.DigestInput = DigestInput;
            }

            public enum HashAlgorithmEnum
            {
                Sha1,
                Sha256
            }

            /// <summary>
            /// Specifies which hash algorithm should be used to calculate the hash. 
            /// See the [verifyAttributes](#common.capabilities.completion.properties.crypto.verifyattributes) capability for valid algorithms.
            /// The following values are possible:
            /// 
            /// * ```sha1``` - The SHA-1 digest algorithm.
            /// * ```sha256``` - The SHA-256 digest algorithm, as defined in ISO/IEC 10118-3:2004 and FIPS 180-2.
            /// </summary>
            [DataMember(Name = "hashAlgorithm")]
            public HashAlgorithmEnum? HashAlgorithm { get; init; }

            /// <summary>
            /// Contains Base64 encoded the length and the data to be hashed.
            /// </summary>
            [DataMember(Name = "digestInput")]
            public string DigestInput { get; init; }

        }
    }
}
