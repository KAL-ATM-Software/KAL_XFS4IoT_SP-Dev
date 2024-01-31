/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetFormList_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Printer.GetFormList")]
    public sealed class GetFormListCompletion : Completion<GetFormListCompletion.PayloadData>
    {
        public GetFormListCompletion(int RequestId, GetFormListCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<string> FormList = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.FormList = FormList;
            }

            /// <summary>
            /// The list of form names. This will be null if no forms are available.
            /// <example>["Form1", "Form2"]</example>
            /// </summary>
            [DataMember(Name = "formList")]
            public List<string> FormList { get; init; }

        }
    }
}
