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

namespace XFS4IoTFramework.Camera
{
    /// <summary>
    /// TakePicture request.
    /// Only one type of camera will be supplied, either a <see cref="CameraCapabilitiesClass.CameraEnum"/> or a <see cref="string"/> for a vendor specific camera.
    /// </summary>
    /// <param name="StdCamera">A standard camera to take a picture with. Will be <see langword="null"/> if <paramref name="CustomCamera"/> is set.</param>
    /// <param name="CustomCamera">A vendor specific camera to take a picture with. Will be <see langword="null"/> if <paramref name="StdCamera"/> is set.</param>
    public sealed record TakePictureRequest(CameraCapabilitiesClass.CameraEnum? StdCamera = null, string CustomCamera = null);

    public sealed class TakePictureResponse : DeviceResult
    {

        public TakePictureResponse(MessagePayload.CompletionCodeEnum CompletionCode,
                              string ErrorDescription = null,
                              List<byte> PictureData = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.PictureData = PictureData;
        }

        public List<byte> PictureData { get; init; }
    }
}
