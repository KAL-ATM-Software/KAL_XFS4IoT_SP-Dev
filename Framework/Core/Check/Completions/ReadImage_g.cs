/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * ReadImage_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Check.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Check.ReadImage")]
    public sealed class ReadImageCompletion : Completion<ReadImageCompletion.PayloadData>
    {
        public ReadImageCompletion(int RequestId, ReadImageCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, MediaDataClass Data = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Data = Data;
            }

            public enum ErrorCodeEnum
            {
                MediaJammed,
                ScannerInop,
                MicrInop,
                NoMedia,
                InvalidMediaID
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```mediaJammed``` - The media is jammed.
            /// * ```scannerInop``` - Only images were requested by the application and these cannot be obtained because the image scanner is inoperative.
            /// * ```micrInop``` - Only MICR data was requested by the application and it cannot be obtained because the MICR reader is inoperative.
            /// * ```noMedia``` - No media is present in the device.
            /// * ```invalidMediaID``` - The requested media ID does not exist.
            /// <example>invalidMediaID</example>
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Image data. May be null if an error occurred.
            /// </summary>
            [DataMember(Name = "data")]
            public MediaDataClass Data { get; init; }

        }
    }
}
