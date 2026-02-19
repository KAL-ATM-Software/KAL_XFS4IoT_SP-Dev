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
using XFS4IoT.Commands;

namespace XFS4IoT.Camera.Commands
{
    //Original name = TakePicture
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Camera.TakePicture")]
    public sealed class TakePictureCommand : Command<TakePictureCommand.PayloadData>
    {
        public TakePictureCommand()
            : base()
        { }

        public TakePictureCommand(int RequestId, TakePictureCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CameraEnum? Camera = null, string CamData = null)
                : base()
            {
                this.Camera = Camera;
                this.CamData = CamData;
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
            /// [Camera.InvalidDataEvent](#camera.invaliddataevent) event will be generated. Nevertheless,
            /// the picture is taken.
            /// <example>Camera 1 Text</example>
            /// </summary>
            [DataMember(Name = "camData")]
            public string CamData { get; init; }

        }
    }
}
