/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Completion(Name = "TextTerminal.GetQueryField")]
    public sealed class GetQueryFieldCompletion : Completion<GetQueryFieldCompletion.PayloadData>
    {
        public GetQueryFieldCompletion(int RequestId, GetQueryFieldCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, Dictionary<string, FieldDetailsClass> Fields = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Fields = Fields;
            }

            /// <summary>
            /// Details of the field(s) requested. The key is the field name and the value contains the details of the fields.
            /// </summary>
            [DataMember(Name = "fields")]
            public Dictionary<string, FieldDetailsClass> Fields { get; private set; }

        }
    }
}
