﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoTServer;
using XFS4IoT;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// GetCommandRandomNumberResult
    /// Return authorisation token for a command
    /// </summary>
    public sealed class GetCommandNonceResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        string Nonce = null) 
        : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// A nonce that should be included in the authorisation token in a command used to provide 
        /// end to end protection.
        /// 
        /// The nonce will be given as HEX (upper case.)
        /// </summary>
        public string Nonce { get; private set; } = Nonce;
    }

    /// <summary>
    /// Transaction state
    /// </summary>
    public enum TransactionStateEnum
    {
        /// <summary>
        /// A customer transaction is in progress.
        /// </summary>
        Active,
        /// <summary>
        /// No customer transaction is in progress.
        /// </summary>
        Inactive,
    }

    /// <summary>
    /// GetCommandRandomNumberResult
    /// Return transaction state
    /// </summary>
    public sealed class GetTransactionStateResult : DeviceResult
    {
        public GetTransactionStateResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription)
            : base(CompletionCode, ErrorDescription)
        {
            this.State = null;
            this.TransactionID = null;
        }
        public GetTransactionStateResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            TransactionStateEnum State,
            string TransactionID)
            : base(CompletionCode, null)
        {
            this.State = State;
            this.TransactionID = TransactionID;
        }

        /// <summary>
        /// Specifies the transaction state. Following values are possible:
        /// 
        /// "active": A customer transaction is in progress.
        /// 
        /// "inactive": No customer transaction is in progress.
        /// </summary>
        public TransactionStateEnum? State { get; init; }

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
    public sealed class SetTransactionStateRequest(
        TransactionStateEnum State,
        string TransactionID)
    {

        /// <summary>
        /// Specifies the transaction state. Following values are possible:
        /// 
        /// "active": A customer transaction is in progress.
        /// 
        /// "inactive": No customer transaction is in progress.
        /// </summary>
        public TransactionStateEnum State { get; init; } = State;

        /// <summary>
        /// Specifies a string which identifies the transaction ID. The value returned in this 
        /// parameter is an application defined customer transaction identifier, which was previously set in the Common.SetTransactionState command
        /// </summary>
        public string TransactionID { get; init; } = TransactionID;
    }
}