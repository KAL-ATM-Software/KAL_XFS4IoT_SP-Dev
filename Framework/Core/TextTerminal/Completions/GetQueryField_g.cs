/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * GetQueryField_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.TextTerminal.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "TextTerminal.GetQueryField")]
    public sealed class GetQueryFieldCompletion : Completion<GetQueryFieldCompletion.PayloadData>
    {
        public GetQueryFieldCompletion(int RequestId, GetQueryFieldCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, Dictionary<string, FieldDetailsClass> Fields = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Fields = Fields;
            }

            public enum ErrorCodeEnum
            {
                FormNotFound,
                FormInvalid,
                FieldNotFound,
                FieldInvalid
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```formNotFound``` - The specified form cannot be found.
            /// * ```formInvalid``` - The specified form is invalid.
            /// * ```fieldNotFound``` - The specified field cannot be found.
            /// * ```fieldInvalid``` - The specified field is invalid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Details of the field(s) requested. The key is the field name and the value contains the details of the fields.
            /// This property is null if the specified form is not loaded.
            /// </summary>
            [DataMember(Name = "fields")]
            public Dictionary<string, FieldDetailsClass> Fields { get; init; }

        }
    }
}
