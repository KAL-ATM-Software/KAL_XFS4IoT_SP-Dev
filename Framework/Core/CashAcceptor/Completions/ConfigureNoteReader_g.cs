/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * ConfigureNoteReader_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashAcceptor.ConfigureNoteReader")]
    public sealed class ConfigureNoteReaderCompletion : Completion<ConfigureNoteReaderCompletion.PayloadData>
    {
        public ConfigureNoteReaderCompletion(int RequestId, ConfigureNoteReaderCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, bool? RebootNecessary = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.RebootNecessary = RebootNecessary;
            }

            public enum ErrorCodeEnum
            {
                ExchangeActive,
                CashInActive,
                LoadFailed
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```exchangeActive``` - The device is in the exchange state.
            /// * ```cashInActive``` - A cash-in transaction is active.
            /// * ```loadFailed``` - The load failed because the device is in a state that will not allow the
            /// configuration data to be loaded at this time, for example on some devices there may be notes present
            /// in the storage units when they should not be.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// If set to true, the machine needs a reboot before the note reader can be accessed again.
            /// </summary>
            [DataMember(Name = "rebootNecessary")]
            public bool? RebootNecessary { get; init; }

        }
    }
}
