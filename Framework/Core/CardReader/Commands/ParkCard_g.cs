/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ParkCard_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = ParkCard
    [DataContract]
    [Command(Name = "CardReader.ParkCard")]
    public sealed class ParkCardCommand : Command<ParkCardCommand.PayloadData>
    {
        public ParkCardCommand(string RequestId, ParkCardCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum DirectionEnum
            {
                In,
                Out,
            }


            public PayloadData(int Timeout, DirectionEnum? Direction = null, int? ParkingStation = null)
                : base(Timeout)
            {
                this.Direction = Direction;
                this.ParkingStation = ParkingStation;
            }

            /// <summary>
            /// Specifies which way to move the card as one of the following values:
            /// 
            /// * ```in``` - The card is moved to the parking station from the read/write, chip I/O or transport
            ///   position.
            /// * ```out``` - The card is moved from the parking station to the read/write, chip I/O or transport
            ///   position. Once the card has been moved any command can be executed e.g.
            ///   [CardReader.EjectCard](#cardreader.ejectcard) or [CardReader.ReadRawData](#cardreader.readrawdata).
            /// </summary>
            [DataMember(Name = "direction")] 
            public DirectionEnum? Direction { get; private set; }
            /// <summary>
            /// Specifies which which parking station should be used for this command. This value is the same index as
            /// is identified in the
            /// [parkingStationMedia](#common.status.completion.properties.cardreader.parkingstationmedia) array of
            /// [Common.Status](#common.status).
            /// </summary>
            [DataMember(Name = "parkingStation")] 
            public int? ParkingStation { get; private set; }

        }
    }
}
