/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * PresentMedia_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = PresentMedia
    [DataContract]
    [Command(Name = "CashAcceptor.PresentMedia")]
    public sealed class PresentMediaCommand : Command<PresentMediaCommand.PayloadData>
    {
        public PresentMediaCommand(int RequestId, PresentMediaCommand.PayloadData Payload)
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
                Null,
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
            /// Describes the position where the media is to be presented. Following values are possible:
            /// 
            /// "null": The default configuration information should be used.
            /// 
            /// "inLeft": Present items to the left input position.
            /// 
            /// "inRight": Present items to the right input position.
            /// 
            /// "inCenter": Present items to of the center input position.
            /// 
            /// "inTop": Present items to the top input position.
            /// 
            /// "inBottom": Present items to the bottom input position.
            /// 
            /// "inFront": Present items to the front input position.
            /// 
            /// "inRear": Present items to the rear input position.
            /// 
            /// "outLeft": Present items to the left output position.
            /// 
            /// "outRight": Present items to the right output position.
            /// 
            /// "outCenter": Present items to the center output position.
            /// 
            /// "outTop": Present items to the top output position.
            /// 
            /// "outBottom": Present items to the bottom output position.
            /// 
            /// "outFront": Present items to the front output position.
            /// 
            /// "outRear": Present items to of the rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

        }
    }
}
