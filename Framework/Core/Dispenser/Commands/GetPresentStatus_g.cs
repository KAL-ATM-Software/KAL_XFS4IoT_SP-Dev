/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * GetPresentStatus_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Dispenser.Commands
{
    //Original name = GetPresentStatus
    [DataContract]
    [Command(Name = "Dispenser.GetPresentStatus")]
    public sealed class GetPresentStatusCommand : Command<GetPresentStatusCommand.PayloadData>
    {
        public GetPresentStatusCommand(int RequestId, GetPresentStatusCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, PositionEnum? Position = null, string RandomNumber = null)
                : base(Timeout)
            {
                this.Position = Position;
                this.RandomNumber = RandomNumber;
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
            /// Required output position. Following values are possible:
            /// 
            /// * ```default``` - The default configuration.
            /// * ```left``` - The left output position.
            /// * ```right``` - The right output position.
            /// * ```center``` - The center output position.
            /// * ```top``` - The top output position.
            /// * ```bottom``` - The bottom output position.
            /// * ```front``` - The front output position.
            /// * ```rear``` - The rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; private set; }

            /// <summary>
            /// A random number to be used when creating the end to end security token in the  response. See the API
            /// documentation on end to end security for more details.
            /// </summary>
            [DataMember(Name = "randomNumber")]
            public string RandomNumber { get; private set; }

        }
    }
}
