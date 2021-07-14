/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * Replenish_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = Replenish
    [DataContract]
    [Command(Name = "CashAcceptor.Replenish")]
    public sealed class ReplenishCommand : Command<ReplenishCommand.PayloadData>
    {
        public ReplenishCommand(int RequestId, ReplenishCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string CashunitSource = null, List<ReplenishTargetsClass> ReplenishTargets = null)
                : base(Timeout)
            {
                this.CashunitSource = CashunitSource;
                this.ReplenishTargets = ReplenishTargets;
            }

            /// <summary>
            /// Object name of the cash unit (as stated by the [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) 
            /// command) from which items are to be removed.
            /// </summary>
            [DataMember(Name = "cashunitSource")]
            public string CashunitSource { get; init; }

            [DataContract]
            public sealed class ReplenishTargetsClass
            {
                public ReplenishTargetsClass(string CashunitTarget = null, int? NumberOfItemsToMove = null, bool? RemoveAll = null)
                {
                    this.CashunitTarget = CashunitTarget;
                    this.NumberOfItemsToMove = NumberOfItemsToMove;
                    this.RemoveAll = RemoveAll;
                }

                /// <summary>
                /// Object name of the cash unit (as stated by the [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) 
                /// command) to which items are to be moved.
                /// </summary>
                [DataMember(Name = "cashunitTarget")]
                public string CashunitTarget { get; init; }

                /// <summary>
                /// The number of items to be moved to the target cash unit. Any items which are removed from the 
                /// source cash unit that are not of the correct currency ID and value for the target cash unit 
                /// during execution of this command will be returned to the source cash unit. This field will be 
                /// ignored if the [removeAll](#cashacceptor.replenish.command.properties.replenishtargets.removeall)
                /// parameter is set to TRUE.
                /// </summary>
                [DataMember(Name = "numberOfItemsToMove")]
                public int? NumberOfItemsToMove { get; init; }

                /// <summary>
                /// Specifies if all items are to be moved to the target cash unit. Any items which are removed from the source 
                /// cash unit that are not of the correct currency ID and 
                /// value for the target cash unit during execution of this command will be returned to the source cash unit. 
                /// If TRUE all items in the source will be moved, regardless of the *numberOfItemsToMove* field value. If 
                /// FALSE the number of items specified with 
                /// [numberOfItemsToMove](#cashacceptor.replenish.command.properties.replenishtargets.numberofitemstomove) will be moved.
                /// </summary>
                [DataMember(Name = "removeAll")]
                public bool? RemoveAll { get; init; }

            }

            /// <summary>
            /// Array of replenish Target elements. There must be at least one array element.
            /// </summary>
            [DataMember(Name = "replenishTargets")]
            public List<ReplenishTargetsClass> ReplenishTargets { get; init; }

        }
    }
}
