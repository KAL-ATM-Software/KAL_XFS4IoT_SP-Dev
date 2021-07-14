/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Completion(Name = "CardReader.EMVClessIssuerUpdate")]
    public sealed class EMVClessIssuerUpdateCompletion : Completion<EMVClessIssuerUpdateCompletion.PayloadData>
    {
        public EMVClessIssuerUpdateCompletion(int RequestId, EMVClessIssuerUpdateCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, EMVClessTxOutputDataClass Chip = null)
                : base(CompletionCode, ErrorDescription)
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
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```noMedia``` - The card was removed before completion of the read action.
            /// * ```invalidMedia``` - No track or chip found or card tapped cannot be used with this command (e.g.
            ///   contactless storage cards or a different card than what was used to complete the
            ///   [CardReader.EMVClessPerformTransaction](#cardreader.emvclessperformtransaction) command).
            /// * ```transactionNotInitiated``` - This command was issued before calling the
            ///   CardReader.EMVClessPerformTransaction command.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Contains the BER-TLV formatted data read from the chip. This will be omitted if no data has been
            /// returned.
            /// </summary>
            [DataMember(Name = "chip")]
            public EMVClessTxOutputDataClass Chip { get; init; }

        }
    }
}
