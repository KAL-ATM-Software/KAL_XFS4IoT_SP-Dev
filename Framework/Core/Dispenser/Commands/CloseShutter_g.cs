/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * CloseShutter_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Dispenser.Commands
{
    //Original name = CloseShutter
    [DataContract]
    [Command(Name = "Dispenser.CloseShutter")]
    public sealed class CloseShutterCommand : Command<CloseShutterCommand.PayloadData>
    {
        public CloseShutterCommand(int RequestId, CloseShutterCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, PositionEnum? Position = null)
                : base(Timeout)
            {
                this.Position = Position;
            }

            public enum PositionEnum
            {
                Default,
                Left,
                Right,
                Center,
                Top,
                Bottom,
                Front,
                Rear
            }

            /// <summary>
            /// The output position where the shutter is to be closed. 
            /// If the application does not need to specify a shutter, this field can be omitted or its contents set to "default".
            /// Following values are possible:
            /// 
            /// * ```default``` - The default configuration information should be used.
            /// * ```left``` - Close the shutter at the left output position.
            /// * ```right``` - Close the shutter at the right output position.
            /// * ```center``` - Close the shutter at the center output position.
            /// * ```top``` - Close the shutter at the top output position.
            /// * ```bottom``` - Close the shutter at the bottom output position.
            /// * ```front``` - Close the shutter at the front output position.
            /// * ```rear``` - Close the shutter at the rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

        }
    }
}
