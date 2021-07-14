/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * MediaDetectedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashAcceptor.Events
{

    [DataContract]
    [Event(Name = "CashAcceptor.MediaDetectedEvent")]
    public sealed class MediaDetectedEvent : UnsolicitedEvent<MediaDetectedEvent.PayloadData>
    {

        public MediaDetectedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string Cashunit = null, RetractAreaClass RetractArea = null, OutputPositionEnum? OutputPosition = null)
                : base()
            {
                this.Cashunit = Cashunit;
                this.RetractArea = RetractArea;
                this.OutputPosition = OutputPosition;
            }

            /// <summary>
            /// If defined, this value specifies the object name (as stated by the 
            /// [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) command) of the single cash unit to 
            /// be used for the storage of any items found.
            /// 
            /// If items are to be moved to an output position, this value must be omitted, 
            /// [retractArea](#cashacceptor.reset.command.properties.retractarea) must be omitted and 
            /// [outputPosition](#cashacceptor.reset.command.properties.outputposition) specifies where items are to be 
            /// moved to.
            /// If this value is omitted and items are to be moved to internal areas of the device, *retractArea* specifies 
            /// where items are to be moved to or stored.
            /// </summary>
            [DataMember(Name = "cashunit")]
            public string Cashunit { get; init; }

            [DataContract]
            public sealed class RetractAreaClass
            {
                public RetractAreaClass(OutputPositionEnum? OutputPosition = null, RetractAreaEnum? RetractArea = null, int? Index = null)
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

            /// <summary>
            /// This field is used if items are to be moved to internal areas of the device, including cash units, the intermediate stacker or the transport. 
            /// The field is only relevant if [cashunit](#cashacceptor.reset.command.properties.cashunit) is not defined.
            /// </summary>
            [DataMember(Name = "retractArea")]
            public RetractAreaClass RetractArea { get; init; }

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
            /// The output position to which items are to be moved. This field is only used if *number* is zero and *netractArea* is omitted. Following values are possible:
            /// 
            /// "null": Take the default configuration.
            /// 
            /// "left": Move items to the left output position.
            /// 
            /// "right": Move items to the right output position.
            /// 
            /// "center": Move items to the center output position.
            /// 
            /// "top": Move items to the top output position.
            /// 
            /// "bottom": Move items to the bottom output position.
            /// 
            /// "front": Move items to the front output position.
            /// 
            /// "rear": Move items to the rear output position.
            /// </summary>
            [DataMember(Name = "outputPosition")]
            public OutputPositionEnum? OutputPosition { get; init; }

        }

    }
}
