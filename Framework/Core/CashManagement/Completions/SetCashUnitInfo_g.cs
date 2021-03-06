/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * SetCashUnitInfo_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashManagement.Completions
{
    [DataContract]
    [Completion(Name = "CashManagement.SetCashUnitInfo")]
    public sealed class SetCashUnitInfoCompletion : Completion<SetCashUnitInfoCompletion.PayloadData>
    {
        public SetCashUnitInfoCompletion(int RequestId, SetCashUnitInfoCompletion.PayloadData Payload)
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
                InvalidTellerId,
                InvalidCashUnit,
                NoExchangeActive,
                CashUnitError
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```invalidTellerId``` - Invalid teller ID. This error will never be generated by a Self-Service device.
            /// * ```invalidCashUnit``` - Invalid cash unit.
            /// * ```noExchangeActive``` - The device is not in an exchange state. The command can only be completed in 
            /// exchange state.
            /// * ```cashUnitError``` - A problem occurred with a cash unit. A CashManagement.CashUnitErrorEvent will 
            /// be posted with the details.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
