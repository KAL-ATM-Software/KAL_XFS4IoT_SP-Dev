/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * Retract_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = Retract
    [DataContract]
    [Command(Name = "CashAcceptor.Retract")]
    public sealed class RetractCommand : Command<RetractCommand.PayloadData>
    {
        public RetractCommand(int RequestId, RetractCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, OutputPositionEnum? OutputPosition = null, RetractAreaEnum? RetractArea = null, int? Index = null)
                : base(Timeout)
            {
                this.OutputPosition = OutputPosition;
                this.RetractArea = RetractArea;
                this.Index = Index;
            }

            public enum OutputPositionEnum
            {
                Null,
                Left,
                Right,
                Center,
                Top,
                Bottom,
                Front,
                Rear
            }

            /// <summary>
            /// Specifies the output position from which to retract the bills. Following values are possible:
            /// 
            /// "null": The default configuration information should be used. This value is also used to retract items from internal device locations.
            /// 
            /// "left": Retract items from the left output position.
            /// 
            /// "right": Retract items from the right output position.
            /// 
            /// "center": Retract items from the center output position.
            /// 
            /// "top": Retract items from the top output position.
            /// 
            /// "bottom": Retract items from the bottom output position.
            /// 
            /// "front": Retract items from the front output position.
            /// 
            /// "rear": Retract items from the rear output position.
            /// </summary>
            [DataMember(Name = "outputPosition")]
            public OutputPositionEnum? OutputPosition { get; init; }

            public enum RetractAreaEnum
            {
                Retract,
                Reject,
                Transport,
                Stacker,
                BillCassettes,
                CashIn
            }

            /// <summary>
            /// This value specifies the area to which the items are to be retracted. Following values are possible:
            /// 
            /// "retract": Retract the items to a retract cash unit.
            /// 
            /// "reject": Retract the items to a reject cash unit.
            /// 
            /// "transport": Retract the items to the transport.
            /// 
            /// "stacker": Retract the items to the intermediate stacker area.
            /// 
            /// "billCassettes": Retract the items to item cassettes, i.e. cash-in and recycle cash units.
            /// 
            /// "cashIn": Retract the items to a cash-in cash unit. The *itemType* of the cash-in cash unit defined in 
            /// CashManagement.CashUnitInfo must include "all" and "unfit".
            /// </summary>
            [DataMember(Name = "retractArea")]
            public RetractAreaEnum? RetractArea { get; init; }

            /// <summary>
            /// If *retractArea* is set to "retract" this field defines the position inside the retract cash units into which the cash is to be retracted. 
            /// *index* starts with a value of one (1) for the first retract position and increments by one for each subsequent position. The maximum value of *index* is the sum of 
            /// the *maximum* of each retract cash unit.
            /// 
            /// If *retractArea* is set to "cashIn" this field defines the cash unit under the "cashIn" 
            /// cash units into which the cash is to be retracted. *index* corresponds to the cash unit *number* 
            /// defined in CashManagement.CashUnitInfo.
            /// 
            /// If *retractArea* is not set to "retract" or "cashIn" then the value of this field is ignored.
            /// </summary>
            [DataMember(Name = "index")]
            public int? Index { get; init; }

        }
    }
}
