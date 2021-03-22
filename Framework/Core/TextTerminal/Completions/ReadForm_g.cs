/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * ReadForm_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
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
        public ReadFormCompletion(string RequestId, ReadFormCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                FormNotFound,
                FormInvalid,
                FieldSpecFailure,
                KeyCanceled,
                FieldError,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, List<string> Fields = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(ReadFormCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
                this.Fields = Fields;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:\"formNotFound\": The specified form definition cannot be found.\"formInvalid\": The specified form definition is invalid.\"fieldSpecFailure\": The syntax of the lpszFields member is invalid.\"keyCanceled\": The read operation was terminated by pressing the <CANCEL> key.\"fieldError\": An error occurred while processing a field.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            ///Specifies \"<fieldName>=<fieldValue>\" string. e.g. Field1=123. The <fieldValue> stands for a string containing all the printable characters (numeric and alphanumeric) read from the text terminal unit key pad for this field.
            /// </summary>
            [DataMember(Name = "fields")] 
            public List<string> Fields{ get; private set; }

        }
    }
}
