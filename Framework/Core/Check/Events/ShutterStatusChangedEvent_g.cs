/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * ShutterStatusChangedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Check.Events
{

    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Event(Name = "Check.ShutterStatusChangedEvent")]
    public sealed class ShutterStatusChangedEvent : UnsolicitedEvent<ShutterStatusChangedEvent.PayloadData>
    {

        public ShutterStatusChangedEvent()
            : base()
        { }

        public ShutterStatusChangedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(PositionEnum? Position = null, ShutterStateEnum? Shutter = null)
                : base()
            {
                this.Position = Position;
                this.Shutter = Shutter;
            }

            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

            [DataMember(Name = "shutter")]
            public ShutterStateEnum? Shutter { get; init; }

        }

    }
}
