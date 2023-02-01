/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Camera interface.
 * TakePicture_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Camera.Commands
{
    //Original name = TakePicture
    [DataContract]
    [Command(Name = "Camera.TakePicture")]
    public sealed class TakePictureCommand : Command<TakePictureCommand.PayloadData>
    {
        public TakePictureCommand(int RequestId, TakePictureCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, CameraEnum? Camera = null, string CamData = null, List<byte> PictureFile = null)
                : base(Timeout)
            {
                this.Camera = Camera;
                this.CamData = CamData;
                this.PictureFile = PictureFile;
            }

            public enum CameraEnum
            {
                Room,
                Person,
                ExitSlot
            }

            /// <summary>
            /// Specifies the camera that should take the photo as one of the following values:
            /// 
            /// * ```room``` - Monitors the whole self-service area.
            /// * ```person``` - Monitors the person standing in front of the self-service machine.
            /// * ```exitSlot``` - Monitors the exit slot(s) of the self-service machine.
            /// </summary>
            [DataMember(Name = "camera")]
            public CameraEnum? Camera { get; init; }

            /// <summary>
            /// Specifies the text string to be displayed on the photo if supported by 
            /// [manAdd](#common.capabilities.completion.properties.camera.camdata.manadd). If the 
            /// [maximum text length](#common.capabilities.completion.properties.camera.maxdatalength) is
            /// exceeded it will be truncated. In this case or if the text given is invalid, a 
            /// [Camera.InvalidDataEvent](#camera.invaliddataevent) event will be generated. Nevertheless 
            /// the picture is taken.
            /// <example>Camera 1 Text</example>
            /// </summary>
            [DataMember(Name = "camData")]
            public string CamData { get; init; }

            /// <summary>
            /// The base64 encoded data representing the picture. This is only supported if
            /// [pictureFile](#common.capabilities.completion.properties.camera.picturefile) 
            /// is true.
            /// <example>Xhdjyedh736ydw7hdi</example>
            /// </summary>
            [DataMember(Name = "pictureFile")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> PictureFile { get; init; }

        }
    }
}
