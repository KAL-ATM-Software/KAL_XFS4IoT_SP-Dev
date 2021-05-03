/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetQueryField_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.GetQueryField")]
    public sealed class GetQueryFieldCompletion : Completion<GetQueryFieldCompletion.PayloadData>
    {
        public GetQueryFieldCompletion(string RequestId, GetQueryFieldCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                FormNotFound,
                FieldNotFound,
                FormInvalid,
                FieldInvalid,
            }

            /// <summary>
            /// Details of the field(s) requested. For each object, the key is the field name.
            /// </summary>
            public class FieldsClass
            {

                public FieldsClass ()
                {
                }


            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, FieldsClass Fields = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Fields = Fields;
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```formNotFound``` - The specified form cannot be found.
            /// * ```fieldNotFound``` - The specified field cannot be found.
            /// * ```formInvalid``` - The specified form is invalid.
            /// * ```fieldInvalid``` - The specified field is invalid.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            /// Details of the field(s) requested. For each object, the key is the field name.
            /// </summary>
            [DataMember(Name = "fields")] 
            public FieldsClass Fields { get; private set; }

        }
    }
}
