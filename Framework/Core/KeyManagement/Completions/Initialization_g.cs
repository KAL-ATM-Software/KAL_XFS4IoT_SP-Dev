/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * Initialization_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "KeyManagement.Initialization")]
    public sealed class InitializationCompletion : Completion<InitializationCompletion.PayloadData>
    {
        public InitializationCompletion(int RequestId, InitializationCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
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
                RandomInvalid,
                KeyNoValue,
                KeyNotFound,
                UseViolation,
                ModeOfUseNotSupported,
                MacInvalid,
                SignatureInvalid
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor
            ///   specific reason.
            /// * ```randomInvalid``` - The encrypted random number in *authentication/data* does not match the one
            ///   previously provided by the device.
            /// * ```keyNoValue``` - A required key was not specified in *authentication.key*.
            /// * ```keyNotFound``` - The key specified in *authentication.key* was not found.
            /// * ```useViolation``` - The key specified in *authentication.key* cannot be used for the specified
            /// *authentication.method*.
            /// * ```modeOfUseNotSupported``` - The key specified in *authentication.key* cannot be used for
            ///   authentication.
            /// * ```macInvalid``` - The MAC included in *authentication/data* is invalid.
            /// * ```signatureInvalid``` - The signature included in *authentication/data* is invalid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
