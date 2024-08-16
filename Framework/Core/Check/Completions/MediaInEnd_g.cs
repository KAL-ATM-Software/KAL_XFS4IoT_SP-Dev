/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * MediaInEnd_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Check.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Check.MediaInEnd")]
    public sealed class MediaInEndCompletion : Completion<MediaInEndCompletion.PayloadData>
    {
        public MediaInEndCompletion(int RequestId, MediaInEndCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, MediaInEndDataClass MediaInEnd = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.MediaInEnd = MediaInEnd;
            }

            public enum ErrorCodeEnum
            {
                NoMedia,
                ShutterFail,
                MediaJammed,
                MediaBinError,
                PositionNotEmpty,
                RefusedItems,
                FeederNotEmpty
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```noMedia``` - No media is present in the device.
            /// * ```shutterFail``` - Open or close of the shutter failed due to manipulation or hardware error.
            /// * ```mediaJammed``` - The media is jammed.
            /// * ```mediaBinError``` - A storage unit caused a problem. A [Storage.StorageErrorEvent](#storage.storageerrorevent) will be posted with the details.
            /// * ```positionNotEmpty``` - One of the input/output/refused positions is not empty and items cannot be inserted until the media items in the position are removed.
            /// * ```refusedItems``` - Programming error: refused items that must be returned via the [Check.PresentMedia](#check.presentmedia) command have not been presented (see [presentRequired](#check.mediarefusedevent.event.description.presentrequired)).
            /// * ```feederNotEmpty``` - The media feeder is not empty.
            /// <example>refusedItems</example>
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataMember(Name = "mediaInEnd")]
            public MediaInEndDataClass MediaInEnd { get; init; }

        }
    }
}
