/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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

namespace XFS4IoTFramework.Common
{
    public sealed class CameraStatusClass
    {
        public CameraStatusClass(Dictionary<CameraLocationStatusClass.CameraLocationEnum, CameraLocationStatusClass> CameraLocationStatus,
                                 Dictionary<string, CameraLocationStatusClass> CustomCameraLocationStatus)
        {
            this.CameraLocationStatus = CameraLocationStatus;
            this.CustomCameraLocationStatus = CustomCameraLocationStatus;
        }

        public sealed class CameraLocationStatusClass : StatusBase
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

            public CameraLocationStatusClass(MediaStateEnum MediaState,
                                             int? NumberOfPictures)
            {
                mediaState = MediaState;
                numberOfPictures = NumberOfPictures;
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
            public MediaStateEnum mediaState = MediaStateEnum.Unknown;

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
            private int? numberOfPictures = -1;
        }

        public Dictionary<CameraLocationStatusClass.CameraLocationEnum, CameraLocationStatusClass> CameraLocationStatus { get; init; }

        public Dictionary<string, CameraLocationStatusClass> CustomCameraLocationStatus { get; init; }
    }
}
