/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessPerformTransaction_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.EMVClessPerformTransaction")]
    public sealed class EMVClessPerformTransactionCompletion : Completion<EMVClessPerformTransactionCompletion.PayloadData>
    {
        public EMVClessPerformTransactionCompletion(int RequestId, EMVClessPerformTransactionCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, EMVClessTxOutputDataClass Track1 = null, EMVClessTxOutputDataClass Track2 = null, EMVClessTxOutputDataClass Track3 = null, EMVClessTxOutputDataClass Chip = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Track1 = Track1;
                this.Track2 = Track2;
                this.Track3 = Track3;
                this.Chip = Chip;
            }

            public enum ErrorCodeEnum
            {
                NoMedia,
                InvalidMedia,
                ReaderNotConfigured
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```noMedia``` - The card was removed before completion of the read operation.
            /// * ```invalidMedia``` - No track or chip was found or the card tapped cannot be used with this command
            ///   (e.g. contactless storage cards).
            /// * ```readerNotConfigured``` - This command was issued before calling
            ///   [CardReader.EMVClessConfigure](#cardreader.emvclessconfigure) command.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Contains the chip returned data formatted in as track 1. This value is set after the contactless
            /// transaction has been completed with mag-stripe mode.
            /// </summary>
            [DataMember(Name = "track1")]
            public EMVClessTxOutputDataClass Track1 { get; init; }

            /// <summary>
            /// Contains the chip returned data formatted in as track 2. This value is set after the contactless
            /// transaction has been completed with mag-stripe mode.
            /// </summary>
            [DataMember(Name = "track2")]
            public EMVClessTxOutputDataClass Track2 { get; init; }

            /// <summary>
            /// Contains the chip returned data formatted in as track 3. This value is set after the contactless
            /// transaction has been completed with mag-stripe mode.
            /// </summary>
            [DataMember(Name = "track3")]
            public EMVClessTxOutputDataClass Track3 { get; init; }

            /// <summary>
            /// Contains the BER-TLV formatted data read from the chip. This value is set after the contactless
            /// transaction has been completed with EMV mode or mag-stripe mode.
            /// </summary>
            [DataMember(Name = "chip")]
            public EMVClessTxOutputDataClass Chip { get; init; }

        }
    }
}
