/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * SetTransactionState_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Common.Commands
{
    //Original name = SetTransactionState
    [DataContract]
    [Command(Name = "Common.SetTransactionState")]
    public sealed class SetTransactionStateCommand : Command<SetTransactionStateCommand.PayloadData>
    {
        public SetTransactionStateCommand(int RequestId, SetTransactionStateCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, StateEnum? State = null, string TransactionID = null)
                : base(Timeout)
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
            /// <example>Example transaction ID</example>
            /// </summary>
            [DataMember(Name = "transactionID")]
            public string TransactionID { get; init; }

        }
    }
}
