/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * Reset_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.Reset")]
    public sealed class ResetCompletion : Completion<ResetCompletion.PayloadData>
    {
        public ResetCompletion(int RequestId, ResetCompletion.PayloadData Payload)
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
                CashUnitError,
                UnsupportedPosition,
                InvalidCashUnit,
                InvalidRetractPosition,
                NotRetractArea,
                ForeignItemsDetected
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// "cashUnitError": A cash unit caused an error. A CashManagement.CashUnitErrorEvent will be sent with the details.
            /// 
            /// "unsupportedPosition": The position specified is not supported.
            /// 
            /// "invalidCashUnit": The cash unit number specified is not valid.
            /// 
            /// "invalidRetractPosition": The *index* is not supported.
            /// 
            /// "notRetractArea": The retract area specified in *retractArea* is not supported.
            /// 
            /// "foreignItemsDetected": Foreign items have been detected in the input position.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
