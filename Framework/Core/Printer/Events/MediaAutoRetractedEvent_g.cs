/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * MediaAutoRetractedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Printer.Events
{

    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Event(Name = "Printer.MediaAutoRetractedEvent")]
    public sealed class MediaAutoRetractedEvent : UnsolicitedEvent<MediaAutoRetractedEvent.PayloadData>
    {

        public MediaAutoRetractedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string Result = null)
                : base()
            {
                this.Result = Result;
            }

            /// <summary>
            /// Specifies where the media has actually been deposited, as one of the following:
            /// 
            /// * ```transport``` - Media was retracted to the transport.
            /// * ```jammed``` - The media is jammed.
            /// * ```[storage unit identifier]``` - A storage unit as specified by
            ///   [identifier](#storage.getstorage.completion.properties.storage.unit1).
            /// <example>unit1</example>
            /// </summary>
            [DataMember(Name = "result")]
            [DataTypes(Pattern = @"^transport$|^jammed$|^unit[0-9A-Za-z]+$")]
            public string Result { get; init; }

        }

    }
}
