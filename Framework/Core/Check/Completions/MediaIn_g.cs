/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * MediaIn_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Check.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Check.MediaIn")]
    public sealed class MediaInCompletion : Completion<MediaInCompletion.PayloadData>
    {
        public MediaInCompletion()
            : base()
        { }

        public MediaInCompletion(int RequestId, MediaInCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, MediainClass MediaIn = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.MediaIn = MediaIn;
            }

            public enum ErrorCodeEnum
            {
                StackerFull,
                ShutterFail,
                MediaJammed,
                RefusedItems,
                AllBinsFull,
                ScannerInop,
                MicrInop,
                PositionNotEmpty,
                FeederNotEmpty,
                MediaRejected,
                FeederInop,
                MediaPresent
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```stackerFull``` - The internal stacker is already full or has already reached the limit specified as an input parameter. No media items can be accepted.
            /// * ```shutterFail``` - Open or close of the shutter failed due to manipulation or hardware error.
            /// * ```mediaJammed``` - The media is jammed.
            /// * ```refusedItems``` - Programming error: refused items that must be returned via the [Check.PresentMedia](#check.presentmedia) command have not been presented (see [presentRequired](#check.mediarefusedevent.event.description.presentrequired)).
            /// * ```allBinsFull``` - All storage units are unusable due to being full, missing or inoperative, so no further items can be accepted.
            /// * ```scannerInop``` - Only images were requested by the application and these cannot be obtained because the image scanner is inoperative.
            /// * ```micrInop``` - Only MICR data was requested by the application and it cannot be obtained because the MICR reader is inoperative.
            /// * ```positionNotEmpty``` - One of the input/output/refused positions is not empty and items cannot be inserted until the media items in the position are removed.
            /// * ```feederNotEmpty``` - The media feeder is not empty. This only applies when the [Check.GetNextItem](#check.getnextitem) command should be used to retrieve the next media item.
            /// * ```mediaRejected``` - The media was rejected before it was fully inserted within the device. The [Check.MediaRejectedEvent](#check.mediarejectedevent) is posted with the details. The device is still operational.
            /// * ```feederInop``` - The media feeder is inoperative.
            /// * ```mediaPresent``` - Media from a previous transaction is present in the device when an attempt to start a new media-in transaction was made. The media must be cleared before a new transaction can be started.
            /// <example>mediaRejected</example>
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataMember(Name = "mediaIn")]
            public MediainClass MediaIn { get; init; }

        }
    }
}
