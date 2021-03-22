/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EjectCard_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = EjectCard
    [DataContract]
    [Command(Name = "CardReader.EjectCard")]
    public sealed class EjectCardCommand : Command<EjectCardCommand.PayloadData>
    {
        public EjectCardCommand(string RequestId, EjectCardCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum EjectPositionEnum
            {
                ExitPosition,
                TransportPosition,
            }


            public PayloadData(int Timeout, EjectPositionEnum? EjectPosition = null)
                : base(Timeout)
            {
                this.EjectPosition = EjectPosition;
            }

            /// <summary>
            ///Specifies the destination of the card ejection for motorized card readers. Possible values are one of the following:**exitPosition**
            ////The card will be transferred to the exit slot from where the user can remove it. In the case of a latched dip the card will be unlatched, enabling removal.**transportPosition**
            ////The card will be transferred to the transport just behind the exit slot. If a card is already at this position then **success** will be returned. Another CardReader.EjectCard command is required with the *ejectPosition* set to **exitPosition** in order to present the card to the user for removal.
            /// </summary>
            [DataMember(Name = "ejectPosition")] 
            public EjectPositionEnum? EjectPosition { get; private set; }

        }
    }
}
