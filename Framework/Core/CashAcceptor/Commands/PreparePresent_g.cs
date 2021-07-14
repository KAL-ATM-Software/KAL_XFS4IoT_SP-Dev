/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * PreparePresent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = PreparePresent
    [DataContract]
    [Command(Name = "CashAcceptor.PreparePresent")]
    public sealed class PreparePresentCommand : Command<PreparePresentCommand.PayloadData>
    {
        public PreparePresentCommand(int RequestId, PreparePresentCommand.PayloadData Payload)
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
                OutLeft,
                OutRight,
                OutCenter,
                OutTop,
                OutBottom,
                OutFront,
                OutRear
            }

            /// <summary>
            /// Describes the position where the items are to be moved. Following values are possible:
            /// 
            /// "null": The default configuration information should be used.
            /// 
            /// "outLeft": Move items to the left output position.
            /// 
            /// "outRight": Move items to the right output position.
            /// 
            /// "outCenter": Move items to the center output position.
            /// 
            /// "outTop": Move items to the top output position.
            /// 
            /// "outBottom": Move items to the bottom output position.
            /// 
            /// "outFront": Move items to the front output position.
            /// 
            /// "outRear": Move items to the rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

        }
    }
}
