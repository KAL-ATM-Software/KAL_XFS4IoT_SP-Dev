/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * ConfigureNoteTypes_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashAcceptor.ConfigureNoteTypes")]
    public sealed class ConfigureNoteTypesCompletion : Completion<ConfigureNoteTypesCompletion.PayloadData>
    {
        public ConfigureNoteTypesCompletion(int RequestId, ConfigureNoteTypesCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
            }

            public enum ErrorCodeEnum
            {
                ExchangeActive,
                CashInActive
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```exchangeActive``` - The device is in the exchange state.
            /// * ```cashInActive``` - A cash-in transaction is active. This device requires that no cash-in
            ///   transaction is active in order to perform the command.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
