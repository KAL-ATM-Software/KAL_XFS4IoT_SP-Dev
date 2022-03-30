/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * CryptoData_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Crypto.Completions
{
    [DataContract]
    [Completion(Name = "Crypto.CryptoData")]
    public sealed class CryptoDataCompletion : Completion<CryptoDataCompletion.PayloadData>
    {
        public CryptoDataCompletion(int RequestId, CryptoDataCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, List<byte> Data = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Data = Data;
            }

            public enum ErrorCodeEnum
            {
                AccessDenied,
                KeyNotFound,
                KeyNoValue,
                UseViolation,
                ModeOfUseNotSupported,
                InvalidKeyLength,
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
            /// * ```modeOfUseNotSupported``` - The *key* Mode of Use or the *modeOfUse* qualifier is not supported.
            /// * ```invalidKeyLength``` - The length of *iv* is not supported or the length of an encryption key is
            /// not compatible with the encryption operation required. 
            /// * ```cryptoMethodNotSupported``` - The cryptographic method specified by *cryptoMethod* is not
            /// supported.
            /// * ```noChipTransactionActive``` - A chipcard key is used as encryption key and there is no chip
            /// transaction active. 
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// The encrypted or decrypted data.
            /// <example>U2FtcGxlIERhdGE=</example>
            /// </summary>
            [DataMember(Name = "data")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> Data { get; init; }

        }
    }
}
