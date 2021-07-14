/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ReadRawData_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = ReadRawData
    [DataContract]
    [Command(Name = "CardReader.ReadRawData")]
    public sealed class ReadRawDataCommand : Command<ReadRawDataCommand.PayloadData>
    {
        public ReadRawDataCommand(int RequestId, ReadRawDataCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, bool? Track1 = null, bool? Track2 = null, bool? Track3 = null, bool? Chip = null, bool? Security = null, bool? FluxInactive = null, bool? Watermark = null, bool? MemoryChip = null, bool? Track1Front = null, bool? FrontImage = null, bool? BackImage = null, bool? Track1JIS = null, bool? Track3JIS = null, bool? Ddi = null)
                : base(Timeout)
            {
                this.Track1 = Track1;
                this.Track2 = Track2;
                this.Track3 = Track3;
                this.Chip = Chip;
                this.Security = Security;
                this.FluxInactive = FluxInactive;
                this.Watermark = Watermark;
                this.MemoryChip = MemoryChip;
                this.Track1Front = Track1Front;
                this.FrontImage = FrontImage;
                this.BackImage = BackImage;
                this.Track1JIS = Track1JIS;
                this.Track3JIS = Track3JIS;
                this.Ddi = Ddi;
            }

            /// <summary>
            /// Track 1 of the magnetic stripe will be read.
            /// </summary>
            [DataMember(Name = "track1")]
            public bool? Track1 { get; init; }

            /// <summary>
            /// Track 2 of the magnetic stripe will be read.
            /// </summary>
            [DataMember(Name = "track2")]
            public bool? Track2 { get; init; }

            /// <summary>
            /// Track 3 of the magnetic stripe will be read.
            /// </summary>
            [DataMember(Name = "track3")]
            public bool? Track3 { get; init; }

            /// <summary>
            /// The chip will be read.
            /// </summary>
            [DataMember(Name = "chip")]
            public bool? Chip { get; init; }

            /// <summary>
            /// A security check will be performed.
            /// </summary>
            [DataMember(Name = "security")]
            public bool? Security { get; init; }

            /// <summary>
            /// If the Flux Sensor is programmable it will be disabled in order to allow chip data to be read on cards
            /// which have no magnetic stripes.
            /// </summary>
            [DataMember(Name = "fluxInactive")]
            public bool? FluxInactive { get; init; }

            /// <summary>
            /// The Swedish Watermark track will be read.
            /// </summary>
            [DataMember(Name = "watermark")]
            public bool? Watermark { get; init; }

            /// <summary>
            /// The memory chip will be read.
            /// </summary>
            [DataMember(Name = "memoryChip")]
            public bool? MemoryChip { get; init; }

            /// <summary>
            /// Track 1 data is read from the magnetic stripe located on the front of the card. In some countries this
            /// track is known as JIS II track.
            /// </summary>
            [DataMember(Name = "track1Front")]
            public bool? Track1Front { get; init; }

            /// <summary>
            /// The front image of the card will be read in Base64 PNG format.
            /// </summary>
            [DataMember(Name = "frontImage")]
            public bool? FrontImage { get; init; }

            /// <summary>
            /// The back image of the card will be read in Base64 PNG format.
            /// </summary>
            [DataMember(Name = "backImage")]
            public bool? BackImage { get; init; }

            /// <summary>
            /// Track 1 of Japanese cash transfer card will be read. In some countries this track is known as JIS I
            /// track 1 (8bits/char).
            /// </summary>
            [DataMember(Name = "track1JIS")]
            public bool? Track1JIS { get; init; }

            /// <summary>
            /// Track 3 of Japanese cash transfer card will be read. In some countries this track is known as JIS I
            /// track 1 (8bits/char).
            /// </summary>
            [DataMember(Name = "track3JIS")]
            public bool? Track3JIS { get; init; }

            /// <summary>
            /// Dynamic Digital Identification data of the magnetic stripe will be read.
            /// </summary>
            [DataMember(Name = "ddi")]
            public bool? Ddi { get; init; }

        }
    }
}
