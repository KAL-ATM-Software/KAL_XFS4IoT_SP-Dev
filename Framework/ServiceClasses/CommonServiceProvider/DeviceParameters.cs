/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// GetCommandRandomNumberResult
    /// Return authorisation token for a command
    /// </summary>
    public sealed class GetCommandNonceResult : DeviceResult
    {
        public GetCommandNonceResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                            string ErrorDescription = null,
                                            string Nonce = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.Nonce = Nonce;
        }

        public GetCommandNonceResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                     string Nonce = null)
            : base(CompletionCode, null)
        {
            this.Nonce = Nonce;
        }

        /// <summary>
        /// A nonce that should be included in the authorisation token in a command used to provide 
        /// end to end protection.
        /// 
        /// The nonce will be given as HEX (upper case.)
        /// </summary>
        public string Nonce { get; private set; }
    }

    /// <summary>
    /// GetCommandRandomNumberResult
    /// Return transaction state
    /// </summary>
    public sealed class GetTransactionStateResult : DeviceResult
    {
        public GetTransactionStateResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                         string ErrorDescription)
            : base(CompletionCode, ErrorDescription)
        {
            this.State = StateEnum.None;
            this.TransactionID = string.Empty;
        }
        public GetTransactionStateResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                         StateEnum State,
                                         string TransactionID)
            : base(CompletionCode, string.Empty)
        {
            this.State = State;
            this.TransactionID = TransactionID;
        }

        public enum StateEnum
        {
            None,
            Active,
            Inactive,
        }

        /// <summary>
        /// Specifies the transaction state. Following values are possible:
        /// 
        /// "active": A customer transaction is in progress.
        /// 
        /// "inactive": No customer transaction is in progress.
        /// </summary>
        public StateEnum State { get; init; }

        /// <summary>
        /// Specifies a string which identifies the transaction ID. The value returned in this 
        /// parameter is an application defined customer transaction identifier, which was previously set in the Common.SetTransactionState command
        /// </summary>
        public string TransactionID { get; init; }
    }

    /// <summary>
    /// SetTransactionStateRequest
    /// Set transaction information to the device
    /// </summary>
    public sealed class SetTransactionStateRequest
    {
        public SetTransactionStateRequest(StateEnum State,
                                          string TransactionID)
        {
            this.State = State;
            this.TransactionID = TransactionID;
        }

        public enum StateEnum
        {
            Active,
            Inactive,
        }

        /// <summary>
        /// Specifies the transaction state. Following values are possible:
        /// 
        /// "active": A customer transaction is in progress.
        /// 
        /// "inactive": No customer transaction is in progress.
        /// </summary>
        public StateEnum State { get; init; }

        /// <summary>
        /// Specifies a string which identifies the transaction ID. The value returned in this 
        /// parameter is an application defined customer transaction identifier, which was previously set in the Common.SetTransactionState command
        /// </summary>
        public string TransactionID { get; init; }
    }
}