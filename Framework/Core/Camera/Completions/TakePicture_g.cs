/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Camera interface.
 * TakePicture_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Camera.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "Camera.TakePicture")]
    public sealed class TakePictureCompletion : Completion<TakePictureCompletion.PayloadData>
    {
        public TakePictureCompletion(int RequestId, TakePictureCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, List<byte> PictureFile = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.PictureFile = PictureFile;
            }

            public enum ErrorCodeEnum
            {
                CameraNotSupported,
                MediaFull,
                CameraInoperable
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```cameraNotSupported``` - The specified camera is not supported.
            /// * ```mediaFull``` - The recording media is full.
            /// * ```cameraInoperable``` - The specified camera is inoperable.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// The base64 encoded data representing the picture. This may be null if there is no picture.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "pictureFile")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> PictureFile { get; init; }

        }
    }
}
