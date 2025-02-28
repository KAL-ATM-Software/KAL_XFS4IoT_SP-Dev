/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "CashManagement.GetItemInfo")]
    public sealed class GetItemInfoCompletion : Completion<GetItemInfoCompletion.PayloadData>
    {
        public GetItemInfoCompletion(int RequestId, GetItemInfoCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<ItemInfoClass> ItemsList = null)
                : base()
            {
                this.ItemsList = ItemsList;
            }

            /// <summary>
            /// Array of objects listing the item information. May be null, if empty.
            /// </summary>
            [DataMember(Name = "itemsList")]
            public List<ItemInfoClass> ItemsList { get; init; }

        }
    }
}
