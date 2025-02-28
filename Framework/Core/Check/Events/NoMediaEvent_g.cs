/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * NoMediaEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Check.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "Check.NoMediaEvent")]
    public sealed class NoMediaEvent : Event<MessagePayloadBase>
    {

        public NoMediaEvent(int RequestId)
            : base(RequestId)
        { }

    }
}
