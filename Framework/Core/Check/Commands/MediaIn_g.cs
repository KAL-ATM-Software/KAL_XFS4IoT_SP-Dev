/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * MediaIn_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Check.Commands
{
    //Original name = MediaIn
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Check.MediaIn")]
    public sealed class MediaInCommand : Command<MediaInCommand.PayloadData>
    {
        public MediaInCommand(int RequestId, MediaInCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CodelineFormatEnum? CodelineFormat = null, List<ImageRequestClass> Image = null, int? MaxMediaOnStacker = null, bool? ApplicationRefuse = null)
                : base()
            {
                this.CodelineFormat = CodelineFormat;
                this.Image = Image;
                this.MaxMediaOnStacker = MaxMediaOnStacker;
                this.ApplicationRefuse = ApplicationRefuse;
            }

            [DataMember(Name = "codelineFormat")]
            public CodelineFormatEnum? CodelineFormat { get; init; }

            /// <summary>
            /// An array specifying the images to be read for each item. May be null if no images are required.
            /// </summary>
            [DataMember(Name = "image")]
            public List<ImageRequestClass> Image { get; init; }

            /// <summary>
            /// Maximum number of media items allowed on the stacker during the media-in transaction. This value is used to
            /// limit the total number of media items on the stacker. When this limit is reached all further media items
            /// will be refused and a [Check.MediaRefusedEvent](#check.mediarefusedevent) message will be generated
            /// reporting *stackerFull*.
            /// - This value cannot exceed [maxMediaOnStacker](#common.capabilities.completion.description.check.maxmediaonstacker)
            /// or the Service will return an *invalidData* error.
            /// - If 0 then the maximum number of items allowed on the stacker reported in
            /// [maxMediaOnStacker](#common.capabilities.completion.description.check.maxmediaonstacker) will be used.
            /// - Ignored unless specified on the first [Check.MediaIn](#check.mediain) command within a single
            /// media-in transaction.
            /// - Ignored on devices without stackers.
            /// <example>10</example>
            /// </summary>
            [DataMember(Name = "maxMediaOnStacker")]
            [DataTypes(Minimum = 0)]
            public int? MaxMediaOnStacker { get; init; }

            /// <summary>
            /// Specifies if the application wants to make the decision to accept or refuse each media item that has
            /// successfully been accepted by the device.
            /// - If true, then the application must decide to accept or refuse each item. The application must use the
            /// [Check.AcceptItem](#check.acceptitem) and [Check.GetNextItem](#check.getnextitem) commands in a sequential
            /// manner to process the bunch of media inserted during the [Check.MediaIn](#check.mediain) command.
            /// - If false, then any decision on whether an item should be refused is left to the device/Service.
            /// - Ignored unless specified on the first [Check.MediaIn](#check.mediain) command within a single
            /// media-in transaction.
            /// - Ignored if [applicationRefuse](#common.capabilities.completion.description.check.applicationrefuse)
            /// is false.
            /// <example>true</example>
            /// </summary>
            [DataMember(Name = "applicationRefuse")]
            public bool? ApplicationRefuse { get; init; }

        }
    }
}
