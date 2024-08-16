/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * GetFormList_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.TextTerminal.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "TextTerminal.GetFormList")]
    public sealed class GetFormListCompletion : Completion<GetFormListCompletion.PayloadData>
    {
        public GetFormListCompletion(int RequestId, GetFormListCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<string> FormList = null)
                : base()
            {
                this.FormList = FormList;
            }

            /// <summary>
            /// Array of the form names. This property is null if no forms were loaded.
            /// <example>["Example form1", "Example form2"]</example>
            /// </summary>
            [DataMember(Name = "formList")]
            public List<string> FormList { get; init; }

        }
    }
}
