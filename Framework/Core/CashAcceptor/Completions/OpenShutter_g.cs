/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * OpenShutter_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.OpenShutter")]
    public sealed class OpenShutterCompletion : Completion<OpenShutterCompletion.PayloadData>
    {
        public OpenShutterCompletion(int RequestId, OpenShutterCompletion.PayloadData Payload)
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
                ShutterNotOpen,
                ShutterOpen,
                ExchangeActive,
                ForeignItemsDetected
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// \"unsupportedPosition\": The position specified is not supported.
            /// 
            /// \"shutterNotOpen\": Shutter failed to open.
            /// 
            /// \"shutterOpen\": Shutter was already open.
            /// 
            /// \"exchangeActive\": The device is in an exchange state. Note that this would not apply during an Exchange (*exchangeType* == \"depositInto\").
            /// 
            /// \"foreignItemsDetected\": Foreign items have been detected in the input position. 
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; private set; }

        }
    }
}
