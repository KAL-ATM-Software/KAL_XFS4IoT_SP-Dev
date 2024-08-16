/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.

\***********************************************************************************************/
using System;
using System.Collections.Generic;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Lights.Completions;
using XFS4IoTFramework.Common;
using XFS4IoT.Camera.Completions;
using XFS4IoT;

namespace XFS4IoTFramework.Camera
{
    /// <summary>
    /// TakePicture request.
    /// Only one type of camera will be supplied, either a <see cref="CameraCapabilitiesClass.CameraEnum"/> or a <see cref="string"/> for a vendor specific camera.
    /// </summary>
    /// <param name="StdCamera">A standard camera to take a picture with. Will be <see langword="null"/> if <paramref name="CustomCamera"/> is set.</param>
    /// <param name="CustomCamera">A vendor specific camera to take a picture with. Will be <see langword="null"/> if <paramref name="StdCamera"/> is set.</param>
    /// <param name="CamData">Any text string to be displayed on the image. The value may be updated depending on the device capabilities <see cref="XFS4IoTFramework.Common.CameraCapabilitiesClass.MaxDataLength"/> and <see cref="XFS4IoTFramework.Common.CameraCapabilitiesClass.CamData"/>.</param>
    public sealed record TakePictureRequest(CameraCapabilitiesClass.CameraEnum? StdCamera = null, string CustomCamera = null, string CamData = null);

    public sealed class TakePictureResponse : DeviceResult
    {

        public TakePictureResponse(MessageHeader.CompletionCodeEnum CompletionCode,
                              List<byte> PictureData = null)
            : base(CompletionCode, null)
        {
            this.PictureData = PictureData;
            this.ErrorCode = null;
        }

        public TakePictureResponse(MessageHeader.CompletionCodeEnum CompletionCode,
                              string ErrorDescription = null,
                              TakePictureCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.PictureData = PictureData;
            this.ErrorCode = ErrorCode;
        }

        [Obsolete("This constructor is obsolete, use constructor has a first parameter MessageHeader." +
            "CompletionCodeEnum. This class will not be supported in the package version 3.0. " +
            "Please migrate changes in the device class before applying 3.0 package.", false)]
        public TakePictureResponse(MessagePayload.CompletionCodeEnum CompletionCode,
                              List<byte> PictureData = null)
            : base(CompletionCode, null)
        {
            this.PictureData = PictureData;
            this.ErrorCode = null;
        }
        [Obsolete("This constructor is obsolete, use constructor has a first parameter MessageHeader." +
            "CompletionCodeEnum. This class will not be supported in the package version 3.0. " +
            "Please migrate changes in the device class before applying 3.0 package.", false)]
        public TakePictureResponse(MessagePayload.CompletionCodeEnum CompletionCode,
                              string ErrorDescription = null,
                              TakePictureCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.PictureData = PictureData;
            this.ErrorCode = ErrorCode;
        }

        public TakePictureCompletion.PayloadData.ErrorCodeEnum? ErrorCode;

        public List<byte> PictureData { get; init; }
    }
}
