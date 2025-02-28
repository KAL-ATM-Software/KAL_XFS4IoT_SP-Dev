/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Crypto.Commands;
using XFS4IoT.Crypto.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.KeyManagement;
using XFS4IoT;

namespace XFS4IoTFramework.Crypto
{
    public enum HashAlgorithmEnum
    {
        SHA1,
        SHA256,
    }

    public sealed class GenerateRandomNumberResult : DeviceResult
    {
        public GenerateRandomNumberResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription,
            GenerateRandomCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.RandomNumber = null;
        }

        public GenerateRandomNumberResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            List<byte> RandomNumber)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.RandomNumber = RandomNumber;
        }

        public GenerateRandomCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        public List<byte> RandomNumber { get; init; }
    }

    public abstract class RequestBase(
        string KeyName,
        int KeySlot,
        List<byte> Data,
        byte Padding)
    {

        /// <summary>
        /// Specifies the name of the stored key
        /// </summary>
        public string KeyName { get; init; } = KeyName;

        /// <summary>
        /// Key slot to use
        /// </summary>
        public int KeySlot { get; init; } = KeySlot;

        /// <summary>
        /// Data to encrypt or decrypt
        /// </summary>
        public List<byte> Data { get; init; } = Data;

        /// <summary>
        /// Padding data
        /// </summary>
        public byte Padding { get; init; } = Padding;
    }

    public sealed class CryptoDataRequest(
        CryptoDataRequest.CryptoModeEnum Mode,
        CryptoDataRequest.CryptoAlgorithmEnum CryptoAlgorithm,
        string KeyName,
        int KeySlot,
        List<byte> Data,
        byte Padding,
        string IVKey = null,
        int IVKeySlot = -1,
        List<byte> IVData = null) : RequestBase(KeyName, KeySlot, Data, Padding)
    {
        public enum CryptoModeEnum
        {
            Encrypt,
            Decrypt,
        }

        public enum CryptoAlgorithmEnum
        {
            ECB,
            CBC,
            CFB,
            OFB,
            CTR,
            XTS,
            RSAES_PKCS1_V1_5,
            RSAES_OAEP
        }

        /// <summary>
        /// Data to encrypt or decrypt
        /// </summary>
        public CryptoModeEnum Mode { get; init; } = Mode;

        /// <summary>
        /// Crypto algorithm to use
        /// </summary>
        public CryptoAlgorithmEnum CryptoAlgorithm { get; init; } = CryptoAlgorithm;

        /// <summary>
        /// Data of the initialization vector
        /// </summary>
        public List<byte> IVData { get; init; } = IVData;

        /// <summary>
        /// The key name of the initialization vector
        /// </summary>
        public string IVKey { get; init; } = IVKey;

        /// <summary>
        /// The key slot of the initialization vector
        /// </summary>
        public int IVKeySlot { get; init; } = IVKeySlot;
    }

    public sealed class CryptoDataResult : DeviceResult
    {
        public CryptoDataResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            CryptoDataCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.CryptoData = null;
        }

        public CryptoDataResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            List<byte> CryptoData)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.CryptoData = CryptoData;
        }

        public CryptoDataCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        public List<byte> CryptoData { get; init; }
    }

    public sealed class GenerateSignatureRequest(
        string KeyName,
        int KeySlot,
        List<byte> Data,
        byte Padding,
        GenerateSignatureRequest.RSASignatureAlgorithmEnum SignatureAlgorithm) 
        : RequestBase(KeyName, KeySlot, Data, Padding)
    {
        public enum RSASignatureAlgorithmEnum
        {
            RSASSA_PKCS1_V1_5,     // SSA_PKCS_V1_5 Signatures supported
            RSASSA_PSS,            // SSA_PSS Signatures supported
        }

        /// <summary>
        /// Signature algorithm
        /// </summary>
        public RSASignatureAlgorithmEnum SignatureAlgorithm { get; init; } = SignatureAlgorithm;

    }

    public sealed class GenerateMACRequest(
        string KeyName,
        int KeySlot,
        List<byte> Data,
        byte Padding,
        string IVKey = null,
        int IVKeySlot = -1,
        List<byte> IVData = null)
        : RequestBase(KeyName, KeySlot, Data, Padding)
    {

        /// <summary>
        /// Data of the initialization vector
        /// </summary>
        public List<byte> IVData { get; init; } = IVData;

        /// <summary>
        /// The key name of the initialization vector
        /// </summary>
        public string IVKey { get; init; } = IVKey;

        /// <summary>
        /// The key slot of the initialization vector
        /// </summary>
        public int IVKeySlot { get; init; } = IVKeySlot;
    }

    public sealed class GenerateAuthenticationDataResult : DeviceResult
    {
        public GenerateAuthenticationDataResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription,
            GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.AuthenticationData = null;
        }

        public GenerateAuthenticationDataResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            List<byte> AuthenticationData)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.AuthenticationData = AuthenticationData;
        }

        public GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        public List<byte> AuthenticationData { get; init; }
    }

    public sealed class VerifySignatureRequest(
        string KeyName,
        int KeySlot,
        List<byte> Data,
        List<byte> VerificationData,
        VerifySignatureRequest.RSASignatureAlgorithmEnum SignatureAlgorithm,
        byte Padding) 
        : RequestBase(KeyName, KeySlot, Data, Padding)
    {
        public enum RSASignatureAlgorithmEnum
        {
            RSASSA_PKCS1_V1_5,     // SSA_PKCS_V1_5 Signatures supported
            RSASSA_PSS,            // SSA_PSS Signatures supported
        }

        /// <summary>
        /// Signature algorithm
        /// </summary>
        public RSASignatureAlgorithmEnum SignatureAlgorithm { get; init; } = SignatureAlgorithm;

        /// <summary>
        /// Data to verify signature
        /// </summary>
        public List<byte> VerificationData { get; init; } = VerificationData;
    }

    public sealed class VerifyMACRequest(
        string KeyName,
        int KeySlot,
        List<byte> Data,
        List<byte> VerificationData,
        byte Padding,
        string IVKey = null,
        int IVKeySlot = -1,
        List<byte> IVData = null) 
        : RequestBase(KeyName, KeySlot, Data, Padding)
    {

        /// <summary>
        /// Data to verify MAC
        /// </summary>
        public List<byte> VerificationData { get; init; } = VerificationData;

        /// <summary>
        /// Data of the initialization vector
        /// </summary>
        public List<byte> IVData { get; init; } = IVData;
        /// <summary>
        /// The key name of the initialization vector
        /// </summary>
        public string IVKey { get; init; } = IVKey;

        /// <summary>
        /// The key slot of the initialization vector
        /// </summary>
        public int IVKeySlot { get; init; } = IVKeySlot;
    }

    public sealed class VerifyAuthenticationDataResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) 
        : DeviceResult(CompletionCode, ErrorDescription)
    {
        public VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }

    public sealed class GenerateDigestRequest
        (HashAlgorithmEnum Hash,
        List<byte> DataToHash)
    {
        public HashAlgorithmEnum Hash { get; init; } = Hash;

        public List<byte> DataToHash { get; init; } = DataToHash;
    }

    public sealed class GenerateDigestResult : DeviceResult
    {
        public GenerateDigestResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription,
            DigestCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Digest = null;
        }

        public GenerateDigestResult(MessageHeader.CompletionCodeEnum CompletionCode,
                                    List<byte> Digest)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Digest = Digest;
        }

        public DigestCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        public List<byte> Digest { get; init; }
    }
}
