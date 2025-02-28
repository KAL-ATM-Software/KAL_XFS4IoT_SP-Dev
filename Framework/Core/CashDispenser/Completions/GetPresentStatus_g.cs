/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * GetPresentStatus_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashDispenser.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "CashDispenser.GetPresentStatus")]
    public sealed class GetPresentStatusCompletion : Completion<GetPresentStatusCompletion.PayloadData>
    {
        public GetPresentStatusCompletion(int RequestId, GetPresentStatusCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, DenominationClass Denomination = null, PresentStateEnum? PresentState = null, string Token = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Denomination = Denomination;
                this.PresentState = PresentState;
                this.Token = Token;
            }

            public enum ErrorCodeEnum
            {
                UnsupportedPosition
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```unsupportedPosition``` - The specified output position is not supported.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Denomination structure which contains the amount dispensed from the specified output position and the number
            /// of items dispensed from each storage unit.
            /// This is cumulative across a series of [CashDispenser.Dispense](#cashdispenser.dispense) calls that add
            /// additional items to the stacker.
            /// 
            /// May be null where no items were dispensed.
            /// </summary>
            [DataMember(Name = "denomination")]
            public DenominationClass Denomination { get; init; }

            public enum PresentStateEnum
            {
                Presented,
                NotPresented,
                Unknown
            }

            /// <summary>
            /// Supplies the status of the last dispense or present operation. Following values are possible:
            /// 
            /// * ```presented``` - The items were presented. This status is set as soon as the customer has access to the items.
            /// * ```notPresented``` - The customer has not had access to the items.
            /// * ```unknown``` - It is not known if the customer had access to the items.
            /// </summary>
            [DataMember(Name = "presentState")]
            public PresentStateEnum? PresentState { get; init; }

            /// <summary>
            /// The present status token that protects the present status. Only provided if the
            /// command message contained the nonce property. See
            /// [end-to-end security](#api.e2esecurity) for more information.
            /// <example>NONCE=1414,TOKENFORMAT=1,TOKENLENGTH=0268,DISPENSEID=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2,DISPENSED1=50.00EUR,PRESENTED1=YES,PRESENTEDAMOUNT1=50.00EUR,RETRACTED1=NO,HMACSHA256=55D123E9EE64F0CC3D1CD4F953348B441E521BBACCD6998C6F51D645D71E6C83</example>
            /// </summary>
            [DataMember(Name = "token")]
            [DataTypes(Pattern = @"^(?=[!-~]{0,1024}$)NONCE=[0-9A-F]+,TOKENFORMAT=1,TOKENLENGTH=[0-9]{4},(?:[A-Z0-9]+=[^,=]+?,)+HMACSHA256=[0-9A-F]{64}$")]
            public string Token { get; init; }

        }
    }
}
