/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * GetTransactionState_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Common.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Common.GetTransactionState")]
    public sealed class GetTransactionStateCompletion : Completion<GetTransactionStateCompletion.PayloadData>
    {
        public GetTransactionStateCompletion(int RequestId, GetTransactionStateCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(StateEnum? State = null, string TransactionID = null)
                : base()
            {
                this.State = State;
                this.TransactionID = TransactionID;
            }

            public enum StateEnum
            {
                Active,
                Inactive
            }

            /// <summary>
            /// Specifies the transaction state. Following values are possible:
            /// 
            /// - ```active``` - A customer transaction is in progress.
            /// - ```inactive``` - No customer transaction is in progress.
            /// </summary>
            [DataMember(Name = "state")]
            public StateEnum? State { get; init; }

            /// <summary>
            /// Specifies a string which identifies the transaction ID.
            /// 
            /// If *state* is *inactive*, this property:
            ///   - Is ignored in [Common.SetTransactionState](#common.settransactionstate)
            ///   - Is null in [Common.GetTransactionState](#common.gettransactionstate).
            /// <example>Example transaction ID</example>
            /// </summary>
            [DataMember(Name = "transactionID")]
            public string TransactionID { get; init; }

        }
    }
}
