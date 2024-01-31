/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * GetNextItem_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Check.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Check.GetNextItem")]
    public sealed class GetNextItemCompletion : Completion<GetNextItemCompletion.PayloadData>
    {
        public GetNextItemCompletion(int RequestId, GetNextItemCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, MediaFeederEnum? MediaFeeder = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.MediaFeeder = MediaFeeder;
            }

            public enum ErrorCodeEnum
            {
                NoItems,
                MediaJammed,
                RefusedItems,
                PositionNotEmpty,
                ScannerInop,
                MicrInop,
                FeederInop
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```noItems``` - There were no items present in the device.
            /// * ```mediaJammed``` - The media is jammed.
            /// * ```refusedItems``` - Programming error, refused items that must be returned have not been presented.
            /// * ```positionNotEmpty``` - One of the input/output/refused positions is not empty.
            /// * ```scannerInop``` - Only images were requested by the application and these cannot be obtained because the image scanner is inoperative.
            /// * ```micrInop``` - Only MICR data was requested by the application and it cannot be obtained because the MICR reader is inoperative.
            /// * ```feederInop``` - The media feeder is inoperative.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataMember(Name = "mediaFeeder")]
            public MediaFeederEnum? MediaFeeder { get; init; }

        }
    }
}
