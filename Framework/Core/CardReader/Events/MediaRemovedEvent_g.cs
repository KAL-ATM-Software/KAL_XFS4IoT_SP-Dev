/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * MediaRemovedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CardReader.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "CardReader.MediaRemovedEvent")]
    public sealed class MediaRemovedEvent : UnsolicitedEvent<MessagePayloadBase>
    {

        public MediaRemovedEvent()
            : base()
        { }

    }
}
