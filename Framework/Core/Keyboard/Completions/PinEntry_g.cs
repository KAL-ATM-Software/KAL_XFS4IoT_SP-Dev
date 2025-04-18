/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * PinEntry_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Keyboard.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Keyboard.PinEntry")]
    public sealed class PinEntryCompletion : Completion<PinEntryCompletion.PayloadData>
    {
        public PinEntryCompletion(int RequestId, PinEntryCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, int? Digits = null, EntryCompletionEnum? Completion = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Digits = Digits;
                this.Completion = Completion;
            }

            public enum ErrorCodeEnum
            {
                KeyInvalid,
                KeyNotSupported,
                NoActivekeys,
                NoTerminatekeys,
                MinimumLength,
                TooManyFrames,
                PartialFrame,
                EntryTimeout
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```keyInvalid``` - At least one of the specified function keys or FDKs is invalid.
            /// * ```keyNotSupported``` - At least one of the specified function keys or FDKs is not supported by the
            ///   Service Provider.
            /// * ```noActivekeys``` - There are no active function keys specified, or there is no defined layout
            ///   definition.
            /// * ```noTerminatekeys``` - There are no terminate keys specified and *maxLen* is not set to zero and
            ///   *autoEnd* is false.
            /// * ```minimumLength``` - The minimum PIN length property is invalid or greater than the maximum PIN
            ///   length property when the maximum PIN length is not zero.
            /// * ```tooManyFrames``` - The device requires that only one frame is used for this command.
            /// * ```partialFrame``` - The single Touch Frame does not cover the entire monitor.
            /// * ```entryTimeout``` - The timeout for entering data has been reached. This is a timeout which may be
            ///                         due to hardware limitations or legislative requirements (for example PCI).
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Specifies the number of PIN digits entered.
            /// </summary>
            [DataMember(Name = "digits")]
            [DataTypes(Minimum = 0)]
            public int? Digits { get; init; }

            [DataMember(Name = "completion")]
            public EntryCompletionEnum? Completion { get; init; }

        }
    }
}
