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
                /// <example>100</example>
                /// </summary>
                [DataMember(Name = "numberOfItemsReceived")]
                [DataTypes(Minimum = 0)]
                public int? NumberOfItemsReceived { get; init; }

                /// <summary>
                /// Total number of items rejected during execution of this command.
                /// <example>10</example>
                /// </summary>
                [DataMember(Name = "numberOfItemsRejected")]
                [DataTypes(Minimum = 0)]
                public int? NumberOfItemsRejected { get; init; }

                [DataContract]
                public sealed class DepleteSourceResultsClass
                {
                    public DepleteSourceResultsClass(string CashUnitSource = null, CashManagement.CashItemClass NoteId = null, int? NumberOfItemsRemoved = null)
                    {
                        this.CashUnitSource = CashUnitSource;
                        this.NoteId = NoteId;
                        this.NumberOfItemsRemoved = NumberOfItemsRemoved;
                    }

                    /// <summary>
                    /// Object name of the cash unit (as stated by the [Storage.GetStorage](#storage.getstorage) 
                    /// command) from which items have been removed.
                    /// <example>unit1</example>
                    /// </summary>
                    [DataMember(Name = "cashUnitSource")]
                    public string CashUnitSource { get; init; }

                    [DataMember(Name = "noteId")]
                    public CashManagement.CashItemClass NoteId { get; init; }

                    /// <summary>
                    /// Total number of items removed from this source cash unit of the _noteId_ item type. 
                    /// Zero will be returned if this source cash unit did not move any items of this item type, 
                    /// for example due to a cash unit or transport jam.
                    /// </summary>
                    [DataMember(Name = "numberOfItemsRemoved")]
                    [DataTypes(Minimum = 0)]
                    public int? NumberOfItemsRemoved { get; init; }

                }

                /// <summary>
                /// Breakdown of which notes moved where. In the case where one item type has several releases and these are moved, 
                /// or where items are moved from a multi denomination cash unit to a multi denomination cash unit, each source 
                /// can move several note types. 
                /// 
                /// For example:
                /// * If one single source was specified with the input structure, and this source moved two different 
                /// note types, then this will have two elements. 
                /// * If two sources were specified and the first source moved two different note types and the second source 
                /// moved three different note types, then this will have five elements.
                /// </summary>
                [DataMember(Name = "depleteSourceResults")]
                public List<DepleteSourceResultsClass> DepleteSourceResults { get; init; }

            }

            /// <summary>
            /// Note that in this case the values in this structure report the amount and number of each denomination that 
            /// have actually been moved during the depletion command.
            /// </summary>
            [DataMember(Name = "deplete")]
            public DepleteClass Deplete { get; init; }

        }

    }
}
