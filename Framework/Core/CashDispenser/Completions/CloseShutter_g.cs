/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * CloseShutter_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashDispenser.Completions
{
    [DataContract]
    [Completion(Name = "CashDispenser.CloseShutter")]
    public sealed class CloseShutterCompletion : Completion<CloseShutterCompletion.PayloadData>
    {
        public CloseShutterCompletion(int RequestId, CloseShutterCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
            }

            public enum ErrorCodeEnum
            {
                UnsupportedPosition,
                ShutterClosed,
                ShutterNotClosed,
                ExchangeActive
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```unsupportedPosition``` - The position specified is not supported.
            /// * ```shutterClosed``` - The shutter was already closed.
            /// * ```shutterNotClosed``` - The shutter failed to close.
            /// * ```exchangeActive``` - The device is in an exchange state (see 
            /// [CashManagement.StartExchange](#cashmanagement.startexchange)).
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
