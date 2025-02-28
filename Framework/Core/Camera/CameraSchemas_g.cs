/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
        Unknown
    }


    public enum CamerasStateEnum
    {
        Ok,
        Inop,
        Unknown
    }


    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(MediaClass Media = null, CamerasClass Cameras = null, PicturesClass Pictures = null)
        {
            this.Media = Media;
            this.Cameras = Cameras;
            this.Pictures = Pictures;
        }

        [DataContract]
        public sealed class MediaClass
        {
            public MediaClass(MediaStateEnum? Room = null, MediaStateEnum? Person = null, MediaStateEnum? ExitSlot = null)
            {
                this.Room = Room;
                this.Person = Person;
                this.ExitSlot = ExitSlot;
            }

            /// <summary>
            /// Specifies the state of the recording media of the camera that monitors the whole self-service area.
            /// See media for valid values.
            /// </summary>
            [DataMember(Name = "room")]
            public MediaStateEnum? Room { get; init; }

            /// <summary>
            /// Specifies the state of the recording media of the camera that monitors the person standing in front of 
            /// the self-service machine.
            /// See media for valid values.
            /// </summary>
            [DataMember(Name = "person")]
            public MediaStateEnum? Person { get; init; }

            /// <summary>
            /// Specifies the state of the recording media of the camera that monitors the exit slot(s) of the 
            /// self-service machine.
            /// See media for valid values.
            /// </summary>
            [DataMember(Name = "exitSlot")]
            public MediaStateEnum? ExitSlot { get; init; }

            [System.Text.Json.Serialization.JsonExtensionData]
            public Dictionary<string, System.Text.Json.JsonElement> ExtensionData { get; set; } = new();

            [System.Text.Json.Serialization.JsonIgnore]
            public Dictionary<string, MediaStateEnum> ExtendedProperties
            {
                get => MessageBase.ParseExtendedProperties<MediaStateEnum>(ExtensionData);
                set => ExtensionData = MessageBase.CreateExtensionData<MediaStateEnum>(value);
            }

        }

        /// <summary>
        /// Specifies the state of the recording media of the cameras as one of the following. For a device which 
        /// stores pictures on a hard disk drive or other general-purpose storage, the relevant property will be null.
        /// This property may be null in [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
        /// 
        /// * ```ok``` - The media is in a good state.
        /// * ```high``` - The media is almost full (threshold).
        /// * ```full``` - The media is full.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the media cannot be determined.
        /// </summary>
        [DataMember(Name = "media")]
        public MediaClass Media { get; init; }

        [DataContract]
        public sealed class CamerasClass
        {
            public CamerasClass(CamerasStateEnum? Room = null, CamerasStateEnum? Person = null, CamerasStateEnum? ExitSlot = null)
            {
                this.Room = Room;
                this.Person = Person;
                this.ExitSlot = ExitSlot;
            }

            /// <summary>
            /// Specifies the state of the camera that monitors the whole self-service area. See cameras for valid values.
            /// </summary>
            [DataMember(Name = "room")]
            public CamerasStateEnum? Room { get; init; }

            /// <summary>
            /// Specifies the state of the camera that monitors the person standing in front of the 
            /// self-service machine. See cameras for valid values.
            /// </summary>
            [DataMember(Name = "person")]
            public CamerasStateEnum? Person { get; init; }

            /// <summary>
            /// Specifies the state of the camera that monitors the exit slot(s) of the self-service machine. See
            /// cameras for valid values.
            /// </summary>
            [DataMember(Name = "exitSlot")]
            public CamerasStateEnum? ExitSlot { get; init; }

            [System.Text.Json.Serialization.JsonExtensionData]
            public Dictionary<string, System.Text.Json.JsonElement> ExtensionData { get; set; } = new();

            [System.Text.Json.Serialization.JsonIgnore]
            public Dictionary<string, CamerasStateEnum> ExtendedProperties
            {
                get => MessageBase.ParseExtendedProperties<CamerasStateEnum>(ExtensionData);
                set => ExtensionData = MessageBase.CreateExtensionData<CamerasStateEnum>(value);
            }

        }

        /// <summary>
        /// Specifies the state of the cameras as one of the following. The relevant property will be null if not
        /// supported and this property may be null in [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
        /// 
        /// * ```ok``` - The camera is in a good state.
        /// * ```inoperative``` - The camera is inoperative.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the camera cannot be determined.
        /// </summary>
        [DataMember(Name = "cameras")]
        public CamerasClass Cameras { get; init; }

        [DataContract]
        public sealed class PicturesClass
        {
            public PicturesClass(int? Room = null, int? Person = null, int? ExitSlot = null)
            {
                this.Room = Room;
                this.Person = Person;
                this.ExitSlot = ExitSlot;
            }

            /// <summary>
            /// Specifies the number of pictures stored on the recording media of the room camera.
            /// </summary>
            [DataMember(Name = "room")]
            [DataTypes(Minimum = 0)]
            public int? Room { get; init; }

            /// <summary>
            /// Specifies the number of pictures stored on the recording media of the person camera.
            /// </summary>
            [DataMember(Name = "person")]
            [DataTypes(Minimum = 0)]
            public int? Person { get; init; }

            /// <summary>
            /// Specifies the number of pictures stored on the recording media of the exit slot camera.
            /// </summary>
            [DataMember(Name = "exitSlot")]
            [DataTypes(Minimum = 0)]
            public int? ExitSlot { get; init; }

            [System.Text.Json.Serialization.JsonExtensionData]
            public Dictionary<string, System.Text.Json.JsonElement> ExtensionData { get; set; } = new();

            [System.Text.Json.Serialization.JsonIgnore]
            public Dictionary<string, int> ExtendedProperties
            {
                get => MessageBase.ParseExtendedProperties<int>(ExtensionData);
                set => ExtensionData = MessageBase.CreateExtensionData<int>(value);
            }

        }

        /// <summary>
        /// Specifies the number of pictures stored on the recording media of the cameras. For a device which 
        /// stores pictures on a hard disk drive or other general-purpose storage, the value of the relevant camera's
        /// property is 0.
        /// Properties may be null in [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
        /// </summary>
        [DataMember(Name = "pictures")]
        public PicturesClass Pictures { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(CamerasClass Cameras = null, int? MaxPictures = null, CamDataClass CamData = null, int? MaxDataLength = null)
        {
            this.Cameras = Cameras;
            this.MaxPictures = MaxPictures;
            this.CamData = CamData;
            this.MaxDataLength = MaxDataLength;
        }

        [DataContract]
        public sealed class CamerasClass
        {
            public CamerasClass(bool? Room = null, bool? Person = null, bool? ExitSlot = null)
            {
                this.Room = Room;
                this.Person = Person;
                this.ExitSlot = ExitSlot;
            }

            /// <summary>
            /// Specifies whether the camera that monitors the whole self-service area is available.
            /// </summary>
            [DataMember(Name = "room")]
            public bool? Room { get; init; }

            /// <summary>
            /// Specifies whether the camera that monitors the person standing in front of the self-service is 
            /// available.
            /// </summary>
            [DataMember(Name = "person")]
            public bool? Person { get; init; }

            /// <summary>
            /// Specifies whether the camera that monitors the exit slot(s) of the self-service machine is available.
            /// </summary>
            [DataMember(Name = "exitSlot")]
            public bool? ExitSlot { get; init; }

            [System.Text.Json.Serialization.JsonExtensionData]
            public Dictionary<string, System.Text.Json.JsonElement> ExtensionData { get; set; } = new();

            [System.Text.Json.Serialization.JsonIgnore]
            public Dictionary<string, bool> ExtendedProperties
            {
                get => MessageBase.ParseExtendedProperties<bool>(ExtensionData);
                set => ExtensionData = MessageBase.CreateExtensionData<bool>(value);
            }

        }

        /// <summary>
        /// Specifies whether cameras are available.
        /// </summary>
        [DataMember(Name = "cameras")]
        public CamerasClass Cameras { get; init; }

        /// <summary>
        /// Specifies the maximum number of pictures that can be stored on the recording media. This property is null if not 
        /// applicable.
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
            /// Specifies whether data can be added manually to the picture using 
            /// [Camera.TakePicture.camData](#camera.takepicture.command.properties.camdata).
            /// </summary>
            [DataMember(Name = "manAdd")]
            public bool? ManAdd { get; init; }

        }

        /// <summary>
        /// Specifies whether the methods are supported for adding data to the picture. If null, no data can be 
        /// added to the picture.
        /// </summary>
        [DataMember(Name = "camData")]
        public CamDataClass CamData { get; init; }

        /// <summary>
        /// Specifies the maximum length of the data that is displayed on the photo. This property is null if not 
        /// applicable.
        /// </summary>
        [DataMember(Name = "maxDataLength")]
        [DataTypes(Minimum = 0)]
        public int? MaxDataLength { get; init; }

    }


}
