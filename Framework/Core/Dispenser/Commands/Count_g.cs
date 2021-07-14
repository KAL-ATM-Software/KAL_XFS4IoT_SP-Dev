/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * Count_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Dispenser.Commands
{
    //Original name = Count
    [DataContract]
    [Command(Name = "Dispenser.Count")]
    public sealed class CountCommand : Command<CountCommand.PayloadData>
    {
        public CountCommand(int RequestId, CountCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, bool? EmptyAll = null, PositionEnum? Position = null, string PhysicalPositionName = null)
                : base(Timeout)
            {
                this.EmptyAll = EmptyAll;
                this.Position = Position;
                this.PhysicalPositionName = PhysicalPositionName;
            }

            /// <summary>
            /// Specifies whether all cash units are to be emptied. If this value is TRUE then physicalPositionName is ignored.
            /// </summary>
            [DataMember(Name = "emptyAll")]
            public bool? EmptyAll { get; init; }

            public enum PositionEnum
            {
                Default,
                Left,
                Right,
                Center,
                Top,
                Bottom,
                Front,
                Rear,
                Reject
            }

            /// <summary>
            /// Specifies the location to which items should be moved. Following values are possible:
            /// 
            /// * ```default``` - Output location is determined by Service.
            /// * ```left``` - Present items to left side of device.
            /// * ```right``` - Present items to right side of device.
            /// * ```center``` - Present items to center output position.
            /// * ```top``` - Present items to the top output position.
            /// * ```bottom``` - Present items to the bottom output position.
            /// * ```front``` - Present items to the front output position.
            /// *  ```rear``` - Present items to the rear output position.
            /// * ```reject``` - Reject bin is used as output location.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

            /// <summary>
            /// Specifies which cash unit to empty and count. This name is the same as the 
            /// *physicalPositionName* in the [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) completion message.
            /// </summary>
            [DataMember(Name = "physicalPositionName")]
            public string PhysicalPositionName { get; init; }

        }
    }
}
