/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * GetPinBlock_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.PinPad.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "PinPad.GetPinBlock")]
    public sealed class GetPinBlockCompletion : Completion<GetPinBlockCompletion.PayloadData>
    {
        public GetPinBlockCompletion(int RequestId, GetPinBlockCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, List<byte> PinBlock = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.PinBlock = PinBlock;
            }

            public enum ErrorCodeEnum
            {
                KeyNotFound,
                AccessDenied,
                KeyNoValue,
                UseViolation,
                NoPin,
                FormatNotSupported,
                InvalidKeyLength,
                AlgorithmNotSupported,
                DukptOverflow,
                CryptoMethodNotSupported
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```keyNotFound``` - The specified key was not found.
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor
            /// specific reason.
            /// * ```keyNoValue``` - The specified key name was found but the corresponding key value has not been
            /// loaded.
            /// * ```useViolation``` - The use specified by
            /// [keyUsage](#common.capabilities.completion.properties.keymanagement.keyattributes.m0) is not
            /// supported.
            /// * ```noPin``` - The PIN has not been entered was not long enough or has been cleared.
            /// * ```formatNotSupported``` - The specified format is not supported.
            /// * ```invalidKeyLength``` - The length of *secondEncKey* or *key* is not supported by this key or the
            /// length of an encryption
            /// key is not compatible with the encryption operation required.
            /// * ```algorithmNotSupported``` - The algorithm specified by algorithm is not supported.
            /// * ```dukptOverflow``` - The DUKPT KSN encryption counter has overflowed to zero. A new IPEK must be
            /// loaded.
            /// * ```cryptoMethodNotSupported``` - The cryptographic method specified by cryptoMethod is not
            /// supported.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// The encrypted PIN block. This value is null if there is no PIN block.
            /// <example>UGluYmxvY2sgZGF0YQ==</example>
            /// </summary>
            [DataMember(Name = "pinBlock")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> PinBlock { get; init; }

        }
    }
}
