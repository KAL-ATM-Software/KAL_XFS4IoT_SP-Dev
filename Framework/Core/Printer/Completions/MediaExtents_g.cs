/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * MediaExtents_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "Printer.MediaExtents")]
    public sealed class MediaExtentsCompletion : Completion<MediaExtentsCompletion.PayloadData>
    {
        public MediaExtentsCompletion()
            : base()
        { }

        public MediaExtentsCompletion(int RequestId, MediaExtentsCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, int? X = null, int? Y = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.X = X;
                this.Y = Y;
            }

            public enum ErrorCodeEnum
            {
                ExtentNotSupported,
                MediaJammed,
                LampInoperative,
                MediaSize,
                MediaRejected
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```extentNotSupported``` - The device cannot report extent(s).
            /// * ```mediaJammed``` - The media is jammed.
            /// * ```lampInoperative``` - Imaging lamp is inoperative.
            /// * ```mediaSize``` - The media entered has an incorrect size and the media remains inside the device.
            /// * ```mediaRejected``` - The media was rejected during the insertion phase. The
            ///   [Printer.MediaRejectedEvent](#printer.mediarejectedevent) event is posted with the details. The
            ///   device is still operational.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Specifies the width of the media in terms of the base horizontal resolution. May be null if an error
            /// occurred.
            /// </summary>
            [DataMember(Name = "x")]
            [DataTypes(Minimum = 1)]
            public int? X { get; init; }

            /// <summary>
            /// Specifies the height of the media in terms of the base vertical resolution. May be null if an error
            /// occurred.
            /// </summary>
            [DataMember(Name = "y")]
            [DataTypes(Minimum = 1)]
            public int? Y { get; init; }

        }
    }
}
