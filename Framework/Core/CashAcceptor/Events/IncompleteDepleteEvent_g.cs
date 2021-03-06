/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * IncompleteDepleteEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashAcceptor.Events
{

    [DataContract]
    [Event(Name = "CashAcceptor.IncompleteDepleteEvent")]
    public sealed class IncompleteDepleteEvent : Event<IncompleteDepleteEvent.PayloadData>
    {

        public IncompleteDepleteEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(DepleteClass Deplete = null)
                : base()
            {
                this.Deplete = Deplete;
            }

            [DataContract]
            public sealed class DepleteClass
            {
                public DepleteClass(int? NumberOfItemsReceived = null, int? NumberOfItemsRejected = null, List<DepleteSourceResultsClass> DepleteSourceResults = null)
                {
                    this.NumberOfItemsReceived = NumberOfItemsReceived;
                    this.NumberOfItemsRejected = NumberOfItemsRejected;
                    this.DepleteSourceResults = DepleteSourceResults;
                }

                /// <summary>
                /// Total number of items received in the target cash unit during execution of this command.
                /// </summary>
                [DataMember(Name = "numberOfItemsReceived")]
                public int? NumberOfItemsReceived { get; init; }

                /// <summary>
                /// Total number of items rejected during execution of this command.
                /// </summary>
                [DataMember(Name = "numberOfItemsRejected")]
                public int? NumberOfItemsRejected { get; init; }

                [DataContract]
                public sealed class DepleteSourceResultsClass
                {
                    public DepleteSourceResultsClass(string CashunitSource = null, int? NoteID = null, int? NumberOfItemsRemoved = null)
                    {
                        this.CashunitSource = CashunitSource;
                        this.NoteID = NoteID;
                        this.NumberOfItemsRemoved = NumberOfItemsRemoved;
                    }

                    /// <summary>
                    /// Object name of the cash unit (as stated by the [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) 
                    /// command) from which items have been removed.
                    /// </summary>
                    [DataMember(Name = "cashunitSource")]
                    public string CashunitSource { get; init; }

                    /// <summary>
                    /// Identification of item type. The note ID represents the item identifiers reported by the CashAcceptor.BanknoteTypes command.
                    /// </summary>
                    [DataMember(Name = "noteID")]
                    public int? NoteID { get; init; }

                    /// <summary>
                    /// Total number of items removed from this source cash unit of the *noteID* item type. 
                    /// A zero value will be returned if this source cash unit did not move any items of this item type, for example due to a cash unit or transport jam.
                    /// </summary>
                    [DataMember(Name = "numberOfItemsRemoved")]
                    public int? NumberOfItemsRemoved { get; init; }

                }

                /// <summary>
                /// Array of DepleteSpourceResult structures. In the case where one item type has several releases and these are moved, 
                /// or where items are moved from a multi denomination cash unit to a multi denomination cash unit, each source can move several *noteID* item types. 
                /// 
                /// For example: If one single source was specified with the *depleteSources* input structure, and this source moved two different *noteID* item types, 
                /// then the *depleteSourceResults* array will have two elements. Or if two sources were specified and the 
                /// first source moved two different *noteID* item types and the second source moved three different *noteID* item types, then the *depleteSourceResults* array will have five elements.
                /// </summary>
                [DataMember(Name = "depleteSourceResults")]
                public List<DepleteSourceResultsClass> DepleteSourceResults { get; init; }

            }

            /// <summary>
            /// Note that in this case the values in this structure report the amount and number of each denomination that have actually been moved during the depletion command.
            /// </summary>
            [DataMember(Name = "deplete")]
            public DepleteClass Deplete { get; init; }

        }

    }
}
