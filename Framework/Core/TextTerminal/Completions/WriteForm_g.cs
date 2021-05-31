/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * WriteForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.TextTerminal.Completions
{
    [DataContract]
    [Completion(Name = "TextTerminal.WriteForm")]
    public sealed class WriteFormCompletion : Completion<WriteFormCompletion.PayloadData>
    {
        public WriteFormCompletion(int RequestId, WriteFormCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
            }

            public enum ErrorCodeEnum
            {
                FormNotFound,
                FormInvalid,
                MediaOverflow,
                FieldSpecFailure,
                CharacterSetsData,
                FieldError
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```formNotFound``` - The specified form definition cannot be found.
            /// * ```formInvalid``` - The specified form definition is invalid.
            /// * ```mediaOverflow``` - The form overflowed the media.
            /// * ```fieldSpecFailure``` - The syntax of the lpszFields member is invalid.
            /// * ```characterSetsData``` - Character set(s) supported by Service Provider is inconsistent with use of fields value.
            /// * ```fieldError``` - An error occurred while processing a field.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; private set; }

        }
    }
}
