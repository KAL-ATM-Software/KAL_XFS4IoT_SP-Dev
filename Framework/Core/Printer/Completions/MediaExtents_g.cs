/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * MediaExtents_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.MediaExtents")]
    public sealed class MediaExtentsCompletion : Completion<MediaExtentsCompletion.PayloadData>
    {
        public MediaExtentsCompletion(string RequestId, MediaExtentsCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                ExtentNotSupported,
                MediaJammed,
                LampInoperative,
                MediaSize,
                MediaRejected,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, int? SizeX = null, int? SizeY = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(MediaExtentsCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
                this.SizeX = SizeX;
                this.SizeY = SizeY;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:**extentNotSupported**
            ////  The device cannot report extent(s).**mediaJammed**
            ////  The media is jammed.**lampInoperative**
            ////  Imaging lamp is inoperative.**mediaSize**
            ////  The media entered has an incorrect size and the media remains inside the device.**mediaRejected**
            ////  The media was rejected during the insertion phase. The  [Printer.MediaRejectedEvent](#message-Printer.MediaRejectedEvent) event is posted with the details.  The device is still operational.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            ///Specifies the width of the media in terms of the base horizontal resolution.
            /// </summary>
            [DataMember(Name = "sizeX")] 
            public int? SizeX { get; private set; }
            /// <summary>
            ///Specifies the height of the media in terms of the base vertical resolution.
            /// </summary>
            [DataMember(Name = "sizeY")] 
            public int? SizeY { get; private set; }

        }
    }
}
