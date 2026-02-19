/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "3.0")]
    [Command(Name = "Crypto.Digest")]
    public sealed class DigestCommand : Command<DigestCommand.PayloadData>
    {
        public DigestCommand()
            : base()
        { }

        public DigestCommand(int RequestId, DigestCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(HashAlgorithmEnum? HashAlgorithm = null, List<byte> Data = null)
                : base()
            {
                this.HashAlgorithm = HashAlgorithm;
                this.Data = Data;
            }

            public enum HashAlgorithmEnum
            {
                Sha1,
                Sha256
            }

            /// <summary>
            /// Specifies which hash algorithm should be used to calculate the hash. See the
            /// [verifyAttributes](#common.capabilities.completion.properties.crypto.verifyattributes) capability for
            /// valid algorithms. The following values are possible:
            /// 
            /// * ```sha1``` - The SHA-1 digest algorithm.
            /// * ```sha256``` - The SHA-256 digest algorithm, as defined in ISO/IEC 10118-3:2004 and FIPS 180-2.
            /// </summary>
            [DataMember(Name = "hashAlgorithm")]
            public HashAlgorithmEnum? HashAlgorithm { get; init; }

            /// <summary>
            /// The data to be hashed.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "data")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Data { get; init; }

        }
    }
}
