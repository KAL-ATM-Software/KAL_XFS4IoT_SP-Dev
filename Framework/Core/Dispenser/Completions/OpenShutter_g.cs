/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * OpenShutter_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Dispenser.Completions
{
    [DataContract]
    [Completion(Name = "Dispenser.OpenShutter")]
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
                ExchangeActive
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```unsupportedPosition``` - The position specified is not supported.
            /// * ```shutterNotOpen``` - The shutter failed to open.
            /// * ```shutterOpen``` - The shutter was already open.
            /// * ```exchangeActive``` - The device is in an exchange state (see CashManagement.StartExchange).
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; private set; }

        }
    }
}
