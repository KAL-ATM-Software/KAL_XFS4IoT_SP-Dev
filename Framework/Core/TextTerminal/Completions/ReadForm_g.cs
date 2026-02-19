/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * ReadForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.TextTerminal.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "TextTerminal.ReadForm")]
    public sealed class ReadFormCompletion : Completion<ReadFormCompletion.PayloadData>
    {
        public ReadFormCompletion()
            : base()
        { }

        public ReadFormCompletion(int RequestId, ReadFormCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, Dictionary<string, string> Fields = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Fields = Fields;
            }

            public enum ErrorCodeEnum
            {
                FormNotFound,
                FormInvalid,
                FieldSpecFailure,
                KeyCanceled,
                FieldError
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```formNotFound``` - The specified form definition cannot be found.
            /// * ```formInvalid``` - The specified form definition is invalid.
            /// * ```fieldSpecFailure``` - The syntax of *fields* is invalid.
            /// * ```keyCanceled``` - The read operation was terminated by pressing the cancel key.
            /// * ```fieldError``` - An error occurred while processing a field.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Details of the field(s) requested. Each property's name is the field name and the value is the field value containing
            /// all the printable characters (numeric and alphanumeric)
            /// read from the Text Terminal Unit keypad for this field. An example shows two fields read.
            /// This property is null if no fields were read.
            /// </summary>
            [DataMember(Name = "fields")]
            public Dictionary<string, string> Fields { get; init; }

        }
    }
}
