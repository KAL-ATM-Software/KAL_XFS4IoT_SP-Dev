/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * ShutterStatusChangedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashManagement.Events
{

    [DataContract]
    [Event(Name = "CashManagement.ShutterStatusChangedEvent")]
    public sealed class ShutterStatusChangedEvent : Event<ShutterStatusChangedEvent.PayloadData>
    {

        public ShutterStatusChangedEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(PositionEnum? Position = null, ShutterEnum? Shutter = null)
                : base()
            {
                this.Position = Position;
                this.Shutter = Shutter;
            }

            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

            [DataMember(Name = "shutter")]
            public ShutterEnum? Shutter { get; init; }

        }

    }
}
