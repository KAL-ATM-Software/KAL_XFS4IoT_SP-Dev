/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * PresentMedia_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashAcceptor.PresentMedia")]
    public sealed class PresentMediaCompletion : Completion<PresentMediaCompletion.PayloadData>
    {
        public PresentMediaCompletion(int RequestId, PresentMediaCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
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
                UnsupportedPosition,
                ShutterNotOpen,
                NoItems,
                ExchangeActive,
                ForeignItemsDetected
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```unsupportedPosition``` - The position specified is not supported or is not a valid position for
            ///   this command.
            /// * ```shutterNotOpen``` - Shutter failed to open.
            /// * ```noItems``` - There were no items to present at the specified position.
            /// * ```exchangeActive``` - The device is in the exchange state.
            /// * ```foreignItemsDetected``` - Foreign items have been detected in the input position.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
