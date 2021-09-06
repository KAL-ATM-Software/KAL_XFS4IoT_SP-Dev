/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * GetPresentStatus_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashDispenser.Commands
{
    //Original name = GetPresentStatus
    [DataContract]
    [Command(Name = "CashDispenser.GetPresentStatus")]
    public sealed class GetPresentStatusCommand : Command<GetPresentStatusCommand.PayloadData>
    {
        public GetPresentStatusCommand(int RequestId, GetPresentStatusCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, PositionEnum? Position = null, string Nonce = null)
                : base(Timeout)
            {
                this.Position = Position;
                this.Nonce = Nonce;
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
            public PositionEnum? Position { get; init; }

            /// <summary>
            /// A nonce value to be used when creating the end to end security token in the 
            /// response. See the API documentation on end to end security for more details.
            /// </summary>
            [DataMember(Name = "nonce")]
            [DataTypes(Pattern = "^[0-9A-F]{32}$|^[0-9]*$")]
            public string Nonce { get; init; }

        }
    }
}
