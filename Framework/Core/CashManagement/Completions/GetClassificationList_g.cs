/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * GetClassificationList_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashManagement.Completions
{
    [DataContract]
    [Completion(Name = "CashManagement.GetClassificationList")]
    public sealed class GetClassificationListCompletion : Completion<GetClassificationListCompletion.PayloadData>
    {
        public GetClassificationListCompletion(int RequestId, GetClassificationListCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, string Version = null, List<ClassificationElementClass> ClassificationElements = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Version = Version;
                this.ClassificationElements = ClassificationElements;
            }

            /// <summary>
            /// This is an application defined string that sets the version identifier of 
            /// the classification list. This property can be omitted if it has no version identifier.
            /// <example>Version 1.2</example>
            /// </summary>
            [DataMember(Name = "version")]
            public string Version { get; init; }

            /// <summary>
            /// Array of classification objects.
            /// </summary>
            [DataMember(Name = "classificationElements")]
            public List<ClassificationElementClass> ClassificationElements { get; init; }

        }
    }
}
