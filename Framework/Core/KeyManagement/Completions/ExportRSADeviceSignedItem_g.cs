/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ExportRSADeviceSignedItem_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "KeyManagement.ExportRSADeviceSignedItem")]
    public sealed class ExportRSADeviceSignedItemCompletion : Completion<ExportRSADeviceSignedItemCompletion.PayloadData>
    {
        public ExportRSADeviceSignedItemCompletion(int RequestId, ExportRSADeviceSignedItemCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, List<byte> Value = null, List<byte> SelfSignature = null, List<byte> Signature = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Value = Value;
                this.SelfSignature = SelfSignature;
                this.Signature = Signature;
            }

            public enum ErrorCodeEnum
            {
                NoRSAKeyPair,
                AccessDenied,
                KeyNotFound
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```noRSAKeyPair``` - The device does not have a private key.
            /// * ```accessDenied``` - The device is either not initialized or not ready for any vendor specific reason.
            /// * ```keyNotFound``` - The data item identified by name was not found.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// If a public key was requested then value contains the PKCS#1 formatted RSA Public Key represented in
            /// DER encoded ASN.1 format. If the security item was requested then value contains the device's Security
            /// Item, which may be vendor specific.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "value")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Value { get; init; }

            /// <summary>
            /// If a public key was requested then *selfSignature* contains the RSA signature of the public key
            /// exported, generated with the key-pair's private component.
            /// 
            /// This should be null if not supported or required.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "selfSignature")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> SelfSignature { get; init; }

            /// <summary>
            /// Specifies the RSA signature of the data item exported.
            /// 
            /// This should be null if not supported or required.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "signature")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Signature { get; init; }

        }
    }
}
