/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ImportKeyToken_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "KeyManagement.ImportKeyToken")]
    public sealed class ImportKeyTokenCompletion : Completion<ImportKeyTokenCompletion.PayloadData>
    {
        public ImportKeyTokenCompletion(int RequestId, ImportKeyTokenCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, int? KeyLength = null, KeyAcceptAlgorithmEnum? KeyAcceptAlgorithm = null, List<byte> KeyAcceptData = null, KeyCheckModeEnum? KeyCheckMode = null, List<byte> KeyCheckValue = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.KeyLength = KeyLength;
                this.KeyAcceptAlgorithm = KeyAcceptAlgorithm;
                this.KeyAcceptData = KeyAcceptData;
                this.KeyCheckMode = KeyCheckMode;
                this.KeyCheckValue = KeyCheckValue;
            }

            public enum ErrorCodeEnum
            {
                AccessDenied,
                DuplicateKey,
                InvalidKeyLength,
                NoKeyRam,
                FormatInvalid,
                ContentInvalid,
                UseViolation,
                RandomInvalid,
                SignatureInvalid,
                InvalidCertState
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor
            ///   specific reason.
            /// * ```duplicateKey``` - A *key* exists with that name and cannot be overwritten.
            /// * ```invalidKeyLength``` - The length of the Key Transport Key is not valid.
            /// * ```noKeyRam``` - There is no space left in the key RAM for a key of the specified type.
            /// * ```formatInvalid``` - The format of the message or key block is invalid.
            /// * ```contentInvalid``` - The content of the message or key block is invalid.
            /// * ```useViolation``` - The specified use is not supported, or if a key with the same name has already
            ///   been loaded, the specified use conflicts with the use of the key previously loaded.
            /// * ```randomInvalid``` - The encrypted random number in the input data does not match the one
            ///   previously provided by the PIN device. Only applies to CRKL load options that use a random number.
            /// * ```signatureInvalid``` - The signature in the input data is invalid.
            /// * ```invalidCertState``` - A Host certificate has not been previously loaded.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Specifies the length, in bits, of the key. Zero if the key length is unknown.
            /// </summary>
            [DataMember(Name = "keyLength")]
            [DataTypes(Minimum = 0)]
            public int? KeyLength { get; init; }

            public enum KeyAcceptAlgorithmEnum
            {
                Sha1,
                Sha256
            }

            /// <summary>
            /// Defines the algorithm used to generate the signature contained in the message *keyAcceptData* sent to the
            /// host. The following values are possible:
            /// 
            /// * ```sha1``` - *keyAcceptData* contains a SHA-1 digest of concatenated data using the device signing key.
            /// * ```sha256``` - *keyAcceptData* contains a SHA-256 digest of concatenated data using the device signing
            /// key.
            /// </summary>
            [DataMember(Name = "keyAcceptAlgorithm")]
            public KeyAcceptAlgorithmEnum? KeyAcceptAlgorithm { get; init; }

            /// <summary>
            /// If *loadOption* is *random* or *randomCrl*, this data is a binary encoded PKCS #7, represented
            /// in DER encoded ASN.1 notation. The message has an outer Signed-data content type with the SignerInfo
            /// encryptedDigest field containing the ATM’s signature. The random numbers are included as
            /// authenticatedAttributes within the SignerInfo. The inner content is a data content type, which contains the
            /// HOST identifier as an issuerAndSerialNumber sequence.
            /// 
            /// If *keyAcceptAlgorithm* is null, then this will also be null.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "keyAcceptData")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> KeyAcceptData { get; init; }

            public enum KeyCheckModeEnum
            {
                KcvSelf,
                KcvZero
            }

            /// <summary>
            /// Specifies the mode that is used to create the key check value. The following values are possible:
            /// 
            /// * ```kcvSelf``` - The key check value (KCV) is created by an encryption of the key with itself.
            /// * ```kcvZero``` - The key check value (KCV) is created by encrypting a zero value with the key.
            /// </summary>
            [DataMember(Name = "keyCheckMode")]
            public KeyCheckModeEnum? KeyCheckMode { get; init; }

            /// <summary>
            /// Contains the key verification code data that can be used for verification of the loaded key. This will be
            /// null if the device does not have that capability.
            /// 
            /// If *keyCheckMode* is null, then this will also be null.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "keyCheckValue")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> KeyCheckValue { get; init; }

        }
    }
}
