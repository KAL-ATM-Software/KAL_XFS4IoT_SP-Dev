/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * MediaDataEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Check.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "Check.MediaDataEvent")]
    public sealed class MediaDataEvent : Event<MediaDataEvent.PayloadData>
    {

        public MediaDataEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(int? MediaID = null, string CodelineData = null, MagneticReadIndicatorEnum? MagneticReadIndicator = null, List<ImageDataClass> Image = null, InsertOrientationClass InsertOrientation = null, MediaSizeClass MediaSize = null, MediaValidityEnum? MediaValidity = null)
                : base()
            {
                this.MediaID = MediaID;
                this.CodelineData = CodelineData;
                this.MagneticReadIndicator = MagneticReadIndicator;
                this.Image = Image;
                this.InsertOrientation = InsertOrientation;
                this.MediaSize = MediaSize;
                this.MediaValidity = MediaValidity;
            }

            /// <summary>
            /// Specifies the sequence number (starting from 1) of a media item.
            /// <example>4</example>
            /// </summary>
            [DataMember(Name = "mediaID")]
            [DataTypes(Minimum = 1)]
            public int? MediaID { get; init; }

            /// <summary>
            /// Specifies the code line data. See [Code line Characters](#check.generalinformation.codelinecharacters).
            /// <example>⑈22222⑈⑆123456⑆</example>
            /// </summary>
            [DataMember(Name = "codelineData")]
            public string CodelineData { get; init; }

            [DataMember(Name = "magneticReadIndicator")]
            public MagneticReadIndicatorEnum? MagneticReadIndicator { get; init; }

            /// <summary>
            /// Array of image data. If the Device has
            /// determined the orientation of the media (i.e. *insertOrientation* is defined and not set to \\"unknown\\"),
            /// then all images returned are in the standard orientation and the images will match the image source
            /// requested by the application. This means that images will be returned with the code line at the bottom,
            /// and the image of the front and rear of the media item will be returned in the structures associated with
            /// the \\"front\\" and \\"back\\" image sources respectively.
            /// </summary>
            [DataMember(Name = "image")]
            public List<ImageDataClass> Image { get; init; }

            [DataMember(Name = "insertOrientation")]
            public InsertOrientationClass InsertOrientation { get; init; }

            [DataMember(Name = "mediaSize")]
            public MediaSizeClass MediaSize { get; init; }

            [DataMember(Name = "mediaValidity")]
            public MediaValidityEnum? MediaValidity { get; init; }

        }

    }
}
