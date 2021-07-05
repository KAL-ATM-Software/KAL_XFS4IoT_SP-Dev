/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * ShutterStatusChangedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Dispenser.Events
{

    [DataContract]
    [Event(Name = "Dispenser.ShutterStatusChangedEvent")]
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
                Left,
                Right,
                Center,
                Top,
                Bottom,
                Front,
                Rear
            }

            /// <summary>
            /// Specifies one of the Dispenser output positions whose shutter status has changed. Following values are possible:
            /// 
            /// * ```left``` - Left output position.
            /// * ```right``` - Right output position.
            /// * ```center``` - Center output position.
            /// * ```top``` - Top output position.
            /// * ```bottom``` - Bottom output position.
            /// * ```front``` - Front output position.
            /// * ```rear``` - Rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; private set; }

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
            /// * ```closed``` - The shutter is closed.
            /// * ```open``` - The shutter is opened.
            /// * ```jammed``` - The shutter is jammed.
            /// * ```unknown``` - Due to a hardware error or other condition, the state of the shutter cannot be determined.
            /// </summary>
            [DataMember(Name = "shutter")]
            public ShutterEnum? Shutter { get; private set; }

        }

    }
}
