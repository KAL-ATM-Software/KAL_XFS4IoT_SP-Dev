/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * CryptoData_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
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
        public CryptoDataCompletion(string RequestId, CryptoDataCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
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
                CryptoMethodNotSupported,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string CryptData = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(CryptoDataCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
                this.CryptData = CryptData;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible: <BR>**keyNotFound**: The specified key was not found.<BR>**modeNotSupported**: The mode specified by modeOfUse is not supported.<BR>**accessDenied**: The encryption module is either not initialized or not ready for any vendor specific reason.<BR>**keyNoValue**: The specified key name was found but the corresponding key value has not been loaded.<BR>**useViolation**: The use specified by keyUsage is not supported.<BR>**invalidKeyLength**: The length of startValue is not supported or the length of an encryption key is not compatible with the encryption operation required.<BR>**noChipTransactionActive**: A chipcard key is used as encryption key and there is no chip transaction active.<BR>**algorithmNotSupported**: The algorithm specified by bAlgorithm is not supported.<BR>**cryptoMethodNotSupported**: The cryptographic method specified by cryptoMethod is not supported.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            ///The encrypted or decrypted data formatted in Base64.
            /// </summary>
            [DataMember(Name = "cryptData")] 
            public string CryptData { get; private set; }

        }
    }
}
