/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Camera interface.
 * CameraSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.Camera
{

    public enum MediaStateEnum
    {
        Ok,
        High,
        Full,
        NotSupp,
        Unknown
    }


    public enum CamerasStateEnum
    {
        NotSupp,
        Ok,
        Inop,
        Unknown
    }


    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(Dictionary<string, MediaStateEnum> Media = null, Dictionary<string, CamerasStateEnum> Cameras = null, Dictionary<string, int> Pictures = null)
        {
            this.Media = Media;
            this.Cameras = Cameras;
            this.Pictures = Pictures;
        }

        /// <summary>
        /// Specifies the state of the recording media of the cameras as one of the following. For a device which 
        /// stores pictures on a hard disk drive or other general-purpose storage, the value of the media field should 
        /// be notSupp.
        /// 
        /// * ```ok``` - The media is in a good state.
        /// * ```high``` - The media is almost full (threshold).
        /// * ```full``` - The media is full.
        /// * ```notSupp``` - The device does not support sensing the media level.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the media cannot be determined.
        /// </summary>
        [DataMember(Name = "media")]
        public Dictionary<string, MediaStateEnum> Media { get; init; }

        /// <summary>
        /// Specifies the state of the cameras as one of the following.
        /// 
        /// * ```notSupp``` - The camera is not supported.
        /// * ```ok``` - The camera is in a good state.
        /// * ```inop``` - The camera is inoperative.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the camera cannot be determined.
        /// </summary>
        [DataMember(Name = "cameras")]
        public Dictionary<string, CamerasStateEnum> Cameras { get; init; }

        /// <summary>
        /// Specifies the number of pictures stored on the recording media of the cameras. For a device which 
        /// stores pictures on a hard disk drive or other general-purpose storage, the value of the pictures 
        /// field should be zero.
        /// </summary>
        [DataMember(Name = "pictures")]
        public Dictionary<string, int> Pictures { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(TypeEnum? Type = null, Dictionary<string, bool> Cameras = null, int? MaxPictures = null, CamDataClass CamData = null, int? MaxDataLength = null, bool? PictureFile = null)
        {
            this.Type = Type;
            this.Cameras = Cameras;
            this.MaxPictures = MaxPictures;
            this.CamData = CamData;
            this.MaxDataLength = MaxDataLength;
            this.PictureFile = PictureFile;
        }

        public enum TypeEnum
        {
            Cam
        }

        /// <summary>
        /// Specifies the type of the camera device; only current value is
        /// 
        /// * ```cam``` - Camera system.
        /// </summary>
        [DataMember(Name = "type")]
        public TypeEnum? Type { get; init; }

        /// <summary>
        /// Specifies whether cameras are available. The value of this parameter is either TRUE or FALSE. 
        /// TRUE is available.
        /// </summary>
        [DataMember(Name = "cameras")]
        public Dictionary<string, bool> Cameras { get; init; }

        /// <summary>
        /// Specifies the maximum number of pictures that can be stored on the recording media.
        /// </summary>
        [DataMember(Name = "maxPictures")]
        [DataTypes(Minimum = 0)]
        public int? MaxPictures { get; init; }

        [DataContract]
        public sealed class CamDataClass
        {
            public CamDataClass(bool? AutoAdd = null, bool? ManAdd = null)
            {
                this.AutoAdd = AutoAdd;
                this.ManAdd = ManAdd;
            }

            /// <summary>
            /// Specifies whether data can be added automatically to the picture.
            /// </summary>
            [DataMember(Name = "autoAdd")]
            public bool? AutoAdd { get; init; }

            /// <summary>
            /// Specifies whether data can be added manually to the picture using the parameter 
            /// [Camera.TakePicture.camData](#camera.takepicture.command.properties.camdata).
            /// </summary>
            [DataMember(Name = "manAdd")]
            public bool? ManAdd { get; init; }

        }

        /// <summary>
        /// Specifies whether the methods are supported for adding data to the picture. Ture means the method is 
        /// supported. False indicates that the method is not supported. If all methods are false, No data can be 
        /// added to the picture.
        /// </summary>
        [DataMember(Name = "camData")]
        public CamDataClass CamData { get; init; }

        /// <summary>
        /// Specifies the maximum length of the data that is displayed on the photo. Zero, if data cannot be manually 
        /// added to the picture.
        /// </summary>
        [DataMember(Name = "maxDataLength")]
        [DataTypes(Minimum = 0)]
        public int? MaxDataLength { get; init; }

        /// <summary>
        /// Specifies whether the parameter [Camera.TakePicture.pictureFile](#camera.takepicture.command.properties.
        /// picturefile) is supported. Ture means the parameter is supported. False indicates that the parameter is not 
        /// supported.
        /// </summary>
        [DataMember(Name = "pictureFile")]
        public bool? PictureFile { get; init; }

    }


}
