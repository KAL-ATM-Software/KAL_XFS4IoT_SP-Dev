/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * OpenShutter_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Dispenser.Commands
{
    //Original name = OpenShutter
    [DataContract]
    [Command(Name = "Dispenser.OpenShutter")]
    public sealed class OpenShutterCommand : Command<OpenShutterCommand.PayloadData>
    {
        public OpenShutterCommand(int RequestId, OpenShutterCommand.PayloadData Payload)
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
            /// The output position where the shutter is to be opened. 
            /// If the application does not need to specify a shutter, this field can be omitted or its contents set to \"default\".
            /// Following values are possible:
            /// 
            /// * ```default``` - The default configuration information should be used.
            /// * ```left``` - Open the shutter at the left output position.
            /// * ```right``` - Open the shutter at the right output position.
            /// * ```center``` - Open the shutter at the center output position.
            /// * ```top``` - Open the shutter at the top output position.
            /// * ```bottom``` - Open the shutter at the bottom output position.
            /// * ```front``` - Open the shutter at the front output position.
            /// * ```rear``` - Open the shutter at the rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; private set; }

        }
    }
}
