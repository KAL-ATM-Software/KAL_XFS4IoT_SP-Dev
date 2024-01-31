/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * SetMediaParameters_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Check.Commands
{
    //Original name = SetMediaParameters
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Check.SetMediaParameters")]
    public sealed class SetMediaParametersCommand : Command<SetMediaParametersCommand.PayloadData>
    {
        public SetMediaParametersCommand(int RequestId, SetMediaParametersCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int? MediaID = null, string Destination = null, bool? Stamp = null, string PrintData = null, List<ImageRequestClass> Image = null)
                : base()
            {
                this.MediaID = MediaID;
                this.Destination = Destination;
                this.Stamp = Stamp;
                this.PrintData = PrintData;
                this.Image = Image;
            }

            /// <summary>
            /// Specifies the sequence number of a media item. Valid IDs are 1 to the maximum media ID assigned within the
            /// transaction - see [mediaID](#check.mediadataevent.event.description.mediaid). If null, all media items are
            /// selected.
            /// <example>4</example>
            /// </summary>
            [DataMember(Name = "mediaID")]
            [DataTypes(Minimum = 1)]
            public int? MediaID { get; init; }

            /// <summary>
            /// Specifies where the item(s) specified by *mediaID* are to be moved to. Following values are possible:
            /// 
            /// * ```customer``` - The item or items are to be returned to the customer.
            /// * ```&lt;storage unit identifier&gt;``` - The item or items are to be sored in a storage unit with matching
            /// [identifier](#storage.getstorage.completion.properties.storage.unit1).
            /// <example>unit1</example>
            /// </summary>
            [DataMember(Name = "destination")]
            [DataTypes(Pattern = @"^customer$|^unit[0-9A-Za-z]+$")]
            public string Destination { get; init; }

            /// <summary>
            /// Specifies whether the media will be stamped. If not specified, the media will not be stamped.
            /// </summary>
            [DataMember(Name = "stamp")]
            public bool? Stamp { get; init; }

            /// <summary>
            /// Specifies the data that will be printed on the media item that is entered by the customer. If a
            /// character is not supported by the device it will be replaced by a vendor dependent substitution
            /// character. If not specified, no text will be printed.
            /// 
            /// For devices that can print multiple lines, each line is separated by a Carriage Return (Unicode 0x000D) and Line
            /// Feed (Unicode 0x000A) sequence. For devices that can print on both sides, the front and back print data are
            /// separated by a Carriage Return (Unicode 0x000D) and a Form Feed (Unicode 0x000C) sequence. In this case the data
            /// to be printed on the back is the first set of data, and the front is the second set of data.
            /// <example>Text to print on the check.</example>
            /// </summary>
            [DataMember(Name = "printData")]
            public string PrintData { get; init; }

            /// <summary>
            /// Specifies the images required. May be null if no images are required.
            /// </summary>
            [DataMember(Name = "image")]
            public List<ImageRequestClass> Image { get; init; }

        }
    }
}
