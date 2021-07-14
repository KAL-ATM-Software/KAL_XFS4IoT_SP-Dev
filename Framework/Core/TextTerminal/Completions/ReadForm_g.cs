/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Completion(Name = "TextTerminal.ReadForm")]
    public sealed class ReadFormCompletion : Completion<ReadFormCompletion.PayloadData>
    {
        public ReadFormCompletion(int RequestId, ReadFormCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, Dictionary<string, string> Fields = null)
                : base(CompletionCode, ErrorDescription)
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
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```formNotFound``` - The specified form definition cannot be found.
            /// * ```formInvalid``` - The specified form definition is invalid.
            /// * ```fieldSpecFailure``` - The syntax of the lpszFields member is invalid.
            /// * ```keyCanceled``` - The read operation was terminated by pressing the &lt;CANCEL&gt; key.
            /// * ```fieldError``` - An error occurred while processing a field.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Details of the field(s) requested. The key is the field name and value is file value containing all the printable characters (numeric and alphanumeric) 
            /// read from the text terminal unit key pad for this field.
            /// </summary>
            [DataMember(Name = "fields")]
            public Dictionary<string, string> Fields { get; init; }

        }
    }
}
