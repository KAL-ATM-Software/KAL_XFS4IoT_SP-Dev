/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * ShutterStatusChangedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashAcceptor.Events
{

    [DataContract]
    [Event(Name = "CashAcceptor.ShutterStatusChangedEvent")]
    public sealed class ShutterStatusChangedEvent : UnsolicitedEvent<ShutterStatusChangedEvent.PayloadData>
    {

        public ShutterStatusChangedEvent(PayloadData Payload)
            : base(Payload)
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

            public enum PositionEnum
            {
                InLeft,
                InRight,
                InCenter,
                InTop,
                InBottom,
                InFront,
                InRear,
                OutLeft,
                OutRight,
                OutCenter,
                OutTop,
                OutBottom,
                OutFront,
                OutRear
            }

            /// <summary>
            /// Specifies one of the input or output positions whose shutter status has changed. Following values are possible:
            /// 
            /// "inLeft": Left input position.
            /// 
            /// "inRight": Right input position.
            /// 
            /// "inCenter": Center input position.
            /// 
            /// "inTop": Top input position.
            /// 
            /// "inBottom": Bottom input position.
            /// 
            /// "inFront": Front input position.
            /// 
            /// "inRear": Rear input position.
            /// 
            /// "outLeft": Left output position.
            /// 
            /// "outRight": Right output position.
            /// 
            /// "outCenter": Center output position.
            /// 
            /// "outTop": Top output position.
            /// 
            /// "outBottom": Bottom output position.
            /// 
            /// "outFront": Front output position.
            /// 
            /// "outRear": Rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

            public enum ShutterEnum
            {
                Closed,
                Open,
                Jammed,
                Unknown
            }

            /// <summary>
            /// Specifies the new state of the shutter. Following values are possible:
            /// 
            /// "closed": The shutter is closed.
            /// 
            /// "open": The shutter is opened.
            /// 
            /// "jammed": The shutter is jammed.
            /// 
            /// "unknown": Due to a hardware error or other condition, the state of the shutter cannot be determined.
            /// </summary>
            [DataMember(Name = "shutter")]
            public ShutterEnum? Shutter { get; init; }

        }

    }
}
