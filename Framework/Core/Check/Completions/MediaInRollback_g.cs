/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * MediaInRollback_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Check.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.1")]
    [Completion(Name = "Check.MediaInRollback")]
    public sealed class MediaInRollbackCompletion : Completion<MediaInRollbackCompletion.PayloadData>
    {
        public MediaInRollbackCompletion()
            : base()
        { }

        public MediaInRollbackCompletion(int RequestId, MediaInRollbackCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, MediaInEndDataClass MediaInRollback = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.MediaInRollback = MediaInRollback;
            }

            public enum ErrorCodeEnum
            {
                NoMedia,
                ShutterFail,
                MediaJammed,
                PositionNotEmpty,
                RefusedItems
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```noMedia``` - No media is present in the device.
            /// * ```shutterFail``` - Open or close of the shutter failed due to manipulation or hardware error.
            /// * ```mediaJammed``` - The media is jammed.
            /// * ```positionNotEmpty``` - One of the input/output/refused positions is not empty and items cannot be inserted until the media items in the position are removed.
            /// * ```refusedItems``` - Programming error: refused items that must be returned via the [Check.PresentMedia](#check.presentmedia) command have not been presented (see [presentRequired](#check.mediarefusedevent.event.description.presentrequired)).
            /// <example>mediaJammed</example>
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataMember(Name = "mediaInRollback")]
            public MediaInEndDataClass MediaInRollback { get; init; }

        }
    }
}
