/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CloseShutter_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.CloseShutter")]
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
                ExchangeActive,
                ShutterNotClosed,
                TooManyItems,
                ForeignItemsDetected
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// "unsupportedPosition": The position specified is not supported.
            /// 
            /// "shutterClosed": Shutter was already closed.
            /// 
            /// "exchangeActive": The device is in an exchange state. Note that this would not apply during an Exchange (*exchangeType* == "depositInto").
            /// 
            /// "shutterNotClosed": Shutter failed to close.
            /// 
            /// "tooManyItems": There were too many items inserted for the shutter to close.
            /// 
            /// "foreignItemsDetected": Foreign items have been detected in the input position. The shutter is open.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
