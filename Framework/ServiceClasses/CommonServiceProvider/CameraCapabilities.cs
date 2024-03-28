/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// CameraCapabilitiesClass
    /// Store device capabilites for the camera device
    /// </summary>
    public sealed class CameraCapabilitiesClass(
        CameraCapabilitiesClass.CameraTypeEnum Type = CameraCapabilitiesClass.CameraTypeEnum.Cam,
        Dictionary<CameraCapabilitiesClass.CameraEnum, bool> Cameras = null,
        Dictionary<string, bool> CustomCameras = null,
        int MaxPictures = 0,
        CameraCapabilitiesClass.CamDataMethodsEnum CamData = CameraCapabilitiesClass.CamDataMethodsEnum.None,
        int? MaxDataLength = null)
    {
        public enum CameraTypeEnum
        {
            Cam
        }

        public enum CameraEnum
        {
            Room,
            Person,
            ExitSlot
        }

        [Flags]
        public enum CamDataMethodsEnum
        {
            None = 0,
            AutoAdd = 1 << 0,
            ManualAdd = 1 << 2,
        }

        /// <summary>
        /// Specifies the type of the camera device.
        /// </summary>
        public CameraTypeEnum Type { get; init; } = Type;

        /// <summary>
        /// Specifies whether standard cameras are available.
        /// </summary>
        public Dictionary<CameraEnum, bool> Cameras { get; init; } = Cameras;

        /// <summary>
        /// Allows any vendor specific cameras to be reported.
        /// </summary>
        public Dictionary<string, bool> CustomCameras { get; init; } = CustomCameras;

        /// <summary>
        /// Specifies the maximum number of pictures that can be stored on the recording media.
        /// </summary>
        public int MaxPictures { get; init; } = MaxPictures;

        /// <summary>
        /// Specifies whether the methods are supported for adding data to the picture.
        /// </summary>
        public CamDataMethodsEnum CamData { get; init; } = CamData;

        /// <summary>
        /// Specifies the maximum length of the data that is displayed on the photo.
        /// Omitted if data cannot be manually added to the picture.
        /// </summary>
        public int? MaxDataLength { get; init; } = MaxDataLength;
    }
}
