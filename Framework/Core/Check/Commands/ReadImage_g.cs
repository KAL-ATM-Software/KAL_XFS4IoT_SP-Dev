/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * ReadImage_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Check.Commands
{
    //Original name = ReadImage
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Check.ReadImage")]
    public sealed class ReadImageCommand : Command<ReadImageCommand.PayloadData>
    {
        public ReadImageCommand(int RequestId, ReadImageCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int? MediaID = null, CodelineFormatEnum? CodelineFormat = null, List<ImageRequestClass> Image = null)
                : base()
            {
                this.MediaID = MediaID;
                this.CodelineFormat = CodelineFormat;
                this.Image = Image;
            }

            /// <summary>
            /// Specifies the sequence number (starting from 1) of a media item.
            /// <example>4</example>
            /// </summary>
            [DataMember(Name = "mediaID")]
            [DataTypes(Minimum = 1)]
            public int? MediaID { get; init; }

            [DataMember(Name = "codelineFormat")]
            public CodelineFormatEnum? CodelineFormat { get; init; }

            /// <summary>
            /// An array specifying the images to be read for each item. May be null if no images are required.
            /// </summary>
            [DataMember(Name = "image")]
            public List<ImageRequestClass> Image { get; init; }

        }
    }
}
