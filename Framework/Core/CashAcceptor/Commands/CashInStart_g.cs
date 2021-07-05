/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CashInStart_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = CashInStart
    [DataContract]
    [Command(Name = "CashAcceptor.CashInStart")]
    public sealed class CashInStartCommand : Command<CashInStartCommand.PayloadData>
    {
        public CashInStartCommand(int RequestId, CashInStartCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? TellerID = null, bool? UseRecycleUnits = null, OutputPositionEnum? OutputPosition = null, InputPositionEnum? InputPosition = null)
                : base(Timeout)
            {
                this.TellerID = TellerID;
                this.UseRecycleUnits = UseRecycleUnits;
                this.OutputPosition = OutputPosition;
                this.InputPosition = InputPosition;
            }

            /// <summary>
            /// Identification of teller. This field is not applicable to Self-Service devices and should be omitted.
            /// </summary>
            [DataMember(Name = "tellerID")]
            public int? TellerID { get; private set; }

            /// <summary>
            /// Specifies whether or not the recycle cash units should be used when items are cashed in on a 
            /// successful CashAcceptor.CashInEnd command. This parameter will be ignored if there are no recycle cash units or the hardware does not support this.
            /// </summary>
            [DataMember(Name = "useRecycleUnits")]
            public bool? UseRecycleUnits { get; private set; }

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
            /// The output position where the items will be presented to the customer in the case of a rollback. Following values are possible:
            /// 
            /// \"null\": The items will be presented to the default configuration.
            /// 
            /// \"left\": The items will be presented to the left output position.
            /// 
            /// \"right\": The items will be presented to the right output position.
            /// 
            /// \"center\": The items will be presented to the center output position.
            /// 
            /// \"top\": The items will be presented to the top output position.
            /// 
            /// \"bottom\": The items will be presented to the bottom output position.
            /// 
            /// \"front\": The items will be presented to the front output position.
            /// 
            /// \"rear\": The items will be presented to the rear output position.
            /// </summary>
            [DataMember(Name = "outputPosition")]
            public OutputPositionEnum? OutputPosition { get; private set; }

            public enum InputPositionEnum
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
            /// Specifies from which position the cash should be inserted. Following values are possible:
            /// 
            /// \"null\": The cash is inserted from the default configuration.
            /// 
            /// \"left\": The cash is inserted from the left input position.
            /// 
            /// \"right\": The cash is inserted from the right input position.
            /// 
            /// \"center\": The cash is inserted from the center input position.
            /// 
            /// \"top\": The cash is inserted from the top input position.
            /// 
            /// \"bottom\": The cash is inserted from the bottom input position.
            /// 
            /// \"front\": The cash is inserted from the front input position.
            /// 
            /// \"rear\": The cash is inserted from the rear input position.
            /// </summary>
            [DataMember(Name = "inputPosition")]
            public InputPositionEnum? InputPosition { get; private set; }

        }
    }
}
