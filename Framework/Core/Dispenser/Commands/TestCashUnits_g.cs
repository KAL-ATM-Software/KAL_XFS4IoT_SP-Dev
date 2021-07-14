/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * TestCashUnits_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Dispenser.Commands
{
    //Original name = TestCashUnits
    [DataContract]
    [Command(Name = "Dispenser.TestCashUnits")]
    public sealed class TestCashUnitsCommand : Command<TestCashUnitsCommand.PayloadData>
    {
        public TestCashUnitsCommand(int RequestId, TestCashUnitsCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string Cashunit = null, RetractAreaClass RetractArea = null, OutputPositionEnum? OutputPosition = null)
                : base(Timeout)
            {
                this.Cashunit = Cashunit;
                this.RetractArea = RetractArea;
                this.OutputPosition = OutputPosition;
            }

            /// <summary>
            /// If defined, this value specifies the object name (as stated by the 
            /// [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) command) of the single cash unit to 
            /// be used for the storage of any items found.
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
                /// Output position from which to retract the items. Following values are possible:
                /// 
                /// * ```default``` - The default configuration information should be used.
                /// * ```left``` - Retract items from the left output position.
                /// * ```right``` - Retract items from the right output position.
                /// * ```center``` - Retract items from the center output position.
                /// * ```top``` - Retract items from the top output position.
                /// * ```bottom``` - Retract items from the bottom output position.
                /// * ```front``` - Retract items from the front output position.
                /// * ```rear``` - Retract items from the rear output position.
                /// </summary>
                [DataMember(Name = "outputPosition")]
                public OutputPositionEnum? OutputPosition { get; init; }

                public enum RetractAreaEnum
                {
                    Retract,
                    Transport,
                    Stacker,
                    Reject,
                    ItemCassette
                }

                /// <summary>
                /// This value specifies the area to which the items are to be retracted. Following values are possible:
                /// 
                /// * ```retract``` - Retract the items to a retract cash unit.
                /// * ```transport``` - Retract the items to the transport.
                /// * ```stacker``` - Retract the items to the intermediate stacker area.
                /// * ```reject``` - Retract the items to a reject cash unit.
                /// * ```itemCassette``` - Retract the items to the item cassettes, i.e. cassettes that can be dispensed from.
                /// </summary>
                [DataMember(Name = "retractArea")]
                public RetractAreaEnum? RetractArea { get; init; }

                /// <summary>
                /// If *retractArea* is set to "retract" this field defines the position inside the retract cash units into 
                /// which the cash is to be retracted. *index* starts with a value of one (1) for the first retract position 
                /// and increments by one for each subsequent position. If there are several retract cash units 
                /// (of type "retractCassette" in command CashManagement.CashUnitInfo), *index* would be incremented from the 
                /// first position of the first retract cash unit to the last position of the last retract cash unit. 
                /// The maximum value of *index* is the sum of *maximum* of each retract cash unit. If *retractArea* is not 
                /// set to "retract" the value of this field is ignored.
                /// </summary>
                [DataMember(Name = "index")]
                public int? Index { get; init; }

            }

            /// <summary>
            /// This field is used if items are to be moved to internal areas of the device, including cash units, the intermediate stacker, or the transport.
            /// </summary>
            [DataMember(Name = "retractArea")]
            public RetractAreaClass RetractArea { get; init; }

            public enum OutputPositionEnum
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
            /// The output position to which items are to be moved. This field is only used if *number* is zero and retractArea is omitted.
            /// Following values are possible:
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
            [DataMember(Name = "outputPosition")]
            public OutputPositionEnum? OutputPosition { get; init; }

        }
    }
}
