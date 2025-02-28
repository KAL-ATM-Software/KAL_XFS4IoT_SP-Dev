/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using static XFS4IoTFramework.Common.CameraStatusClass;

namespace XFS4IoTFramework.Common
{
    public sealed class CameraStatusClass(
        Dictionary<CameraLocationStatusClass.CameraLocationEnum, CameraLocationStatusClass> CameraLocationStatus,
        Dictionary<string, CameraLocationStatusClass> CustomCameraLocationStatus)
    {
        public sealed class CameraLocationStatusClass(
            CameraLocationStatusClass.MediaStateEnum MediaState,
            int? NumberOfPictures) : StatusBase
        {
            public enum CameraLocationEnum
            {
                Room,
                Person,
                ExitSlot,
            }

            public enum MediaStateEnum
            {
                Ok,
                High,
                Full,
                Unknown
            }

            public enum CamerasStateEnum
            {
                Ok,
                Inoperable,
                Unknown
            }

            /// <summary>
            /// This property is set by the framework for status changed event
            /// </summary>
            public CameraLocationEnum? Location { get; set; } = null;

            /// <summary>
            /// This property is set by the framework for status changed event
            /// </summary>
            public string CustomLocation { get; set; } = null;

            /// <summary>
            /// Specifies the state of the cameras
            /// </summary>
            public CamerasStateEnum CamerasState
            {
                get
                {
                    return camerasState;
                }
                set
                {
                    if (camerasState != value)
                    {
                        camerasState = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private CamerasStateEnum camerasState = CamerasStateEnum.Unknown;

            /// <summary>
            /// Specifies the state of the recording media of the cameras
            /// </summary>
            public MediaStateEnum MediaState
            {
                get
                {
                    return mediaState;
                }
                set
                {
                    if (mediaState != value)
                    {
                        mediaState = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            public MediaStateEnum mediaState = MediaState;

            /// <summary>
            /// Specifies the number of pictures stored on the recording media of the cameras
            /// </summary>
            public int? NumberOfPictures
            {
                get
                {
                    return numberOfPictures;
                }
                set
                {
                    if (numberOfPictures != value)
                    {
                        numberOfPictures = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private int? numberOfPictures = NumberOfPictures;
        }

        public Dictionary<CameraLocationStatusClass.CameraLocationEnum, CameraLocationStatusClass> CameraLocationStatus { get; init; } = CameraLocationStatus;

        public Dictionary<string, CameraLocationStatusClass> CustomCameraLocationStatus { get; init; } = CustomCameraLocationStatus;
    }
}
