/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * Reset_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashDispenser.Completions
{
    [DataContract]
    [Completion(Name = "CashDispenser.Reset")]
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
                PositionNotEmpty,
                IncompleteRetract
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```cashUnitError``` - There is a problem with a cash unit. A CashManagement.CashUnitErrorEvent will be posted with the details.
            /// * ```unsupportedPosition``` - The position specified is not supported.
            /// * ```invalidCashUnit``` - The cash unit number specified is not valid.
            /// * ```invalidRetractPosition``` - The *index* is not supported.
            /// * ```notRetractArea``` - The retract area specified in *retractArea* is not supported.
            /// * ```positionNotEmpty``` - The retract area specified in *retractArea* is not empty so the moving of items was not possible.
            /// * ```incompleteRetract``` - Some or all of the items were not retracted for a reason not covered by other error codes. The detail will be reported with the CashDispenser.IncompleteRetractEvent.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
