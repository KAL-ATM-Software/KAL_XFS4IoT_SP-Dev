/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * Digest_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Crypto.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Crypto.Digest")]
    public sealed class DigestCompletion : Completion<DigestCompletion.PayloadData>
    {
        public DigestCompletion(int RequestId, DigestCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, List<byte> Digest = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Digest = Digest;
            }

            public enum ErrorCodeEnum
            {
                AccessDenied
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for
            ///                         any vendor specific reason.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Contains the generated digest. If the command fails, this will be null.
            /// <example>OTNjYzE2Y2FkNzYwMTY3 ...</example>
            /// </summary>
            [DataMember(Name = "digest")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> Digest { get; init; }

        }
    }
}
