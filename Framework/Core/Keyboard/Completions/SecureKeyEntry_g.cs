/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * SecureKeyEntry_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Keyboard.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "Keyboard.SecureKeyEntry")]
    public sealed class SecureKeyEntryCompletion : Completion<SecureKeyEntryCompletion.PayloadData>
    {
        public SecureKeyEntryCompletion(int RequestId, SecureKeyEntryCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, int? Digits = null, EntryCompletionEnum? Completion = null, List<byte> Kcv = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Digits = Digits;
                this.Completion = Completion;
                this.Kcv = Kcv;
            }

            public enum ErrorCodeEnum
            {
                AccessDenied,
                KeyInvalid,
                KeyNotSupported,
                NoActiveKeys,
                NoTerminatekeys,
                InvalidKeyLength,
                ModeNotSupported,
                TooManyFrames,
                PartialFrame,
                MissingKeys,
                EntryTimeout
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any
            ///   vendor-specific reason.
            /// * ```keyInvalid``` - At least one of the specified function keys or FDKs is invalid.
            /// * ```keyNotSupported``` - At least one of the specified function keys or FDKs is not supported by the
            ///   Service Provider.
            /// * ```noActiveKeys``` - There are no active function keys specified, or there is no defined layout
            ///   definition.
            /// * ```noTerminatekeys``` - There are no terminate keys specified and *autoEnd* is false.
            /// * ```invalidKeyLength``` - The keyLen key length is not supported.
            /// * ```modeNotSupported``` - The KCV mode is not supported.
            /// * ```tooManyFrames``` - The device requires that only one frame is used for this command.
            /// * ```partialFrame``` - The single Touch Frame does not cover the entire monitor.
            /// * ```missingKeys``` - The single frame does not contain a full set of hexadecimal key definitions.
            /// * ```entryTimeout``` - The timeout for entering data has been reached. This is a timeout which may be
            ///   due to hardware limitations or legislative requirements (for example PCI).
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Specifies the number of key digits entered. Applications must ensure all required digits have been
            /// entered before trying to store the key.
            /// </summary>
            [DataMember(Name = "digits")]
            [DataTypes(Minimum = 0)]
            public int? Digits { get; init; }

            [DataMember(Name = "completion")]
            public EntryCompletionEnum? Completion { get; init; }

            /// <summary>
            /// Contains the key check value data that can be used for verification of the entered key formatted in
            /// Base64.
            /// This value is null if device does not have this capability, or the key entry was not fully
            /// entered, e.g. the entry was terminated by Enter before the required number of digits was entered.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "kcv")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Kcv { get; init; }

        }
    }
}
