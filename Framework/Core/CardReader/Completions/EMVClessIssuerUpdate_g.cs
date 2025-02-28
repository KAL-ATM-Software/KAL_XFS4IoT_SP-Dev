/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessIssuerUpdate_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "CardReader.EMVClessIssuerUpdate")]
    public sealed class EMVClessIssuerUpdateCompletion : Completion<EMVClessIssuerUpdateCompletion.PayloadData>
    {
        public EMVClessIssuerUpdateCompletion(int RequestId, EMVClessIssuerUpdateCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, EMVClessIssuerUpdateEMVClessTxOutputDataClass Chip = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Chip = Chip;
            }

            public enum ErrorCodeEnum
            {
                NoMedia,
                InvalidMedia,
                TransactionNotInitiated
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```noMedia``` - The card was removed before completion of the read action.
            /// * ```invalidMedia``` - No track or chip found or card tapped cannot be used with this command (e.g.,
            ///   contactless storage cards or a different card than what was used to complete the
            ///   [CardReader.EMVClessPerformTransaction](#cardreader.emvclessperformtransaction) command).
            /// * ```transactionNotInitiated``` - This command was issued before calling the
            ///   *CardReader.EMVClessPerformTransaction* command.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataMember(Name = "chip")]
            public EMVClessIssuerUpdateEMVClessTxOutputDataClass Chip { get; init; }

        }
    }
}
