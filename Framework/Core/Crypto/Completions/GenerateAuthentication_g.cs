/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * GenerateAuthentication_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Crypto.Completions
{
    [DataContract]
    [Completion(Name = "Crypto.GenerateAuthentication")]
    public sealed class GenerateAuthenticationCompletion : Completion<GenerateAuthenticationCompletion.PayloadData>
    {
        public GenerateAuthenticationCompletion(int RequestId, GenerateAuthenticationCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, List<byte> AuthenticationData = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.AuthenticationData = AuthenticationData;
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
                NoChipTransactionActive
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor
            /// specific reason. 
            /// * ```keyNotFound``` - The *key* name does not exist.
            /// * ```keyNoValue``` - The *key* name exists but the key is not loaded.
            /// * ```useViolation``` - The *key* usage is not supported. 
            /// * ```modeOfUseNotSupported``` - The *key* Mode of Use is not supported.
            /// * ```invalidKeyLength``` - The length of *iv* is not supported or the length of an encryption key is
            /// not compatible with the encryption operation required.
            /// * ```algorithmNotSupported``` - The hash algorithm ins not supported.
            /// * ```cryptoMethodNotSupported``` - The cryptographic method specified by *cryptoMethod* is not
            /// supported.
            /// * ```noChipTransactionActive``` - A chipcard key is used as encryption key and there is no chip
            /// transaction active. 
            /// active.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// The generated authentication data.
            /// <example>VGhlIG1hYyB2YWx1ZSBv ...</example>
            /// </summary>
            [DataMember(Name = "authenticationData")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> AuthenticationData { get; init; }

        }
    }
}
