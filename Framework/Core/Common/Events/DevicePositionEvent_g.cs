/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * DevicePositionEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Common.Events
{

    [DataContract]
    [Event(Name = "Common.DevicePositionEvent")]
    public sealed class DevicePositionEvent : UnsolicitedEvent<DevicePositionEvent.PayloadData>
    {

        public DevicePositionEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(PositionStatusEnum? Position = null)
                : base()
            {
                this.Position = Position;
            }

            /// <summary>
            /// Position of the device
            /// </summary>
            [DataMember(Name = "Position")]
            public PositionStatusEnum? Position { get; init; }

        }

    }
}
