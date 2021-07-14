/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * Deplete_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = Deplete
    [DataContract]
    [Command(Name = "CashAcceptor.Deplete")]
    public sealed class DepleteCommand : Command<DepleteCommand.PayloadData>
    {
        public DepleteCommand(int RequestId, DepleteCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, List<DepleteSourcesClass> DepleteSources = null, string CashunitTarget = null)
                : base(Timeout)
            {
                this.DepleteSources = DepleteSources;
                this.CashunitTarget = CashunitTarget;
            }

            [DataContract]
            public sealed class DepleteSourcesClass
            {
                public DepleteSourcesClass(string CashunitSource = null, int? NumberOfItemsToMove = null, bool? RemoveAll = null)
                {
                    this.CashunitSource = CashunitSource;
                    this.NumberOfItemsToMove = NumberOfItemsToMove;
                    this.RemoveAll = RemoveAll;
                }

                /// <summary>
                /// Object name of the cash unit (as stated by the [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) 
                /// command) from which items are to be removed.
                /// </summary>
                [DataMember(Name = "cashunitSource")]
                public string CashunitSource { get; init; }

                /// <summary>
                /// The number of items to be moved from the source cash unit. 
                /// This must be equal to or less than the count of items reported for the cash unit specified by *numberSource*. This field will be ignored if the *removeAll* parameter is set to TRUE.
                /// </summary>
                [DataMember(Name = "numberOfItemsToMove")]
                public int? NumberOfItemsToMove { get; init; }

                /// <summary>
                /// Specifies if all items are to be moved from the source cash unit. 
                /// If TRUE all items in the source will be moved, regardless of the *numberOfItemsToMove* field value. If FALSE the number of items specified with *numberOfItemsToMove* will be moved.
                /// </summary>
                [DataMember(Name = "removeAll")]
                public bool? RemoveAll { get; init; }

            }

            /// <summary>
            /// Array of DepleteSource structures. There must be at least one element in this array.
            /// </summary>
            [DataMember(Name = "depleteSources")]
            public List<DepleteSourcesClass> DepleteSources { get; init; }

            /// <summary>
            /// Object name of the cash unit (as stated by the [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) 
            /// command) to which items are to be moved.
            /// </summary>
            [DataMember(Name = "cashunitTarget")]
            public string CashunitTarget { get; init; }

        }
    }
}
