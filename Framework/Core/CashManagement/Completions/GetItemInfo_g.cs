/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * GetItemInfo_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashManagement.Completions
{
    [DataContract]
    [Completion(Name = "CashManagement.GetItemInfo")]
    public sealed class GetItemInfoCompletion : Completion<GetItemInfoCompletion.PayloadData>
    {
        public GetItemInfoCompletion(int RequestId, GetItemInfoCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<ItemInfoClass> ItemsList = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ItemsList = ItemsList;
            }

            /// <summary>
            /// Array of "item info" objects.
            /// </summary>
            [DataMember(Name = "itemsList")]
            public List<ItemInfoClass> ItemsList { get; init; }

        }
    }
}
