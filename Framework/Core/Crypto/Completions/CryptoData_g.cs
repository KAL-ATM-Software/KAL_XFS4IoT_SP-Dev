/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string CryptData = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.CryptData = CryptData;
            }

            public enum ErrorCodeEnum
            {
                KeyNotFound,
                ModeNotSupported,
                AccessDenied,
                KeyNoValue,
                UseViolation,
                InvalidKeyLength,
                NoChipTransactionActive,
                AlgorithmNotSupported,
                CryptoMethodNotSupported
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```keyNotFound``` - The specified key was not found.
            /// * ```modeNotSupported``` - The mode specified by modeOfUse is not supported.
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor specific
            /// reason. 
            /// * ```keyNoValue``` - The specified key name was found but the corresponding key value has not been
            /// loaded. 
            /// * ```useViolation``` - The use specified by keyUsage is not supported. 
            /// * ```invalidKeyLength``` - The length of startValue is not supported or the length of an encryption key is
            /// not compatible with the encryption operation required. 
            /// * ```noChipTransactionActive``` - A chipcard key is used as encryption key and there is no chip transaction
            /// active. 
            /// * ```algorithmNotSupported``` - The algorithm specified by bAlgorithm is not supported. 
            /// * ```cryptoMethodNotSupported``` - The cryptographic method specified by cryptoMethod is not supported.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; private set; }

            /// <summary>
            /// The encrypted or decrypted data formatted in Base64.
            /// </summary>
            [DataMember(Name = "cryptData")]
            public string CryptData { get; private set; }

        }
    }
}
