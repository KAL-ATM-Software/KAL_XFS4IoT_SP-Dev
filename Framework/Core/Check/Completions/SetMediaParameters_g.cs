/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * SetMediaParameters_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Check.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Check.SetMediaParameters")]
    public sealed class SetMediaParametersCompletion : Completion<SetMediaParametersCompletion.PayloadData>
    {
        public SetMediaParametersCompletion()
            : base()
        { }

        public SetMediaParametersCompletion(int RequestId, SetMediaParametersCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
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
                InvalidMediaID,
                InvalidBin,
                NoBin,
                MediaBinFull,
                MediaJammed,
                ScannerInop,
                NoItems,
                TonerOut,
                InkOut
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```invalidMediaID``` - The requested media ID does not exist.
            /// * ```invalidBin``` - The specified storage unit cannot accept items.
            /// * ```noBin``` - The specified storage unit does not exist.
            /// * ```mediaBinFull``` - The storage unit is already full and no media can be placed in the specified storage unit.
            /// * ```mediaJammed``` - The media is jammed.
            /// * ```scannerInop``` - Only images were requested by the application and these cannot be obtained because the image scanner is inoperative.
            /// * ```noItems``` - There were no items present in the device.
            /// * ```tonerOut``` - Toner or ink supply is empty or printing contrast with ribbon is not sufficient.
            /// * ```inkOut``` - No stamping possible, stamping ink supply empty.
            /// <example>invalidMediaID</example>
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
