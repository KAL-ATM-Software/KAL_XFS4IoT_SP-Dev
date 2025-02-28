/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * VerifyAuthentication_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Crypto.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Crypto.VerifyAuthentication")]
    public sealed class VerifyAuthenticationCompletion : Completion<VerifyAuthenticationCompletion.PayloadData>
    {
        public VerifyAuthenticationCompletion(int RequestId, VerifyAuthenticationCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
            }

            public enum ErrorCodeEnum
            {
                AccessDenied,
                KeyNotFound,
                KeyNoValue,
                UseViolation,
                ModeOfUseNotSupported,
                InvalidKeyLength,
                AlgorithmNotSupported,
                CryptoMethodNotSupported,
                NoChipTransactionActive,
                MacInvalid,
                SignatureInvalid
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor
            ///   specific reason.
            /// * ```keyNotFound``` - The *key* name does not exist.
            /// * ```keyNoValue``` - The *key* name exists but the key is not loaded.
            /// * ```useViolation``` - The *key* usage is not supported.
            /// * ```modeOfUseNotSupported``` - The *key* Mode of Use is not supported.
            /// * ```invalidKeyLength``` - The length of *iv* is not supported or the length of an encryption key is
            ///   not compatible with the encryption operation required.
            /// * ```algorithmNotSupported``` - The hash algorithm ins not supported.
            /// * ```cryptoMethodNotSupported``` - The cryptographic method specified by *cryptoMethod* is not
            ///   supported.
            /// * ```noChipTransactionActive``` - A chipcard key is used as encryption key and there is no chip
            ///   transaction active.
            /// * ```macInvalid``` - The MAC verification failed.
            /// * ```signatureInvalid``` - The signature verification failed.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
