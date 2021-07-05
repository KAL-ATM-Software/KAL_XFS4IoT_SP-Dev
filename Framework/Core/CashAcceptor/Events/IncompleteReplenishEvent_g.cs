/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * IncompleteReplenishEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashAcceptor.Events
{

    [DataContract]
    [Event(Name = "CashAcceptor.IncompleteReplenishEvent")]
    public sealed class IncompleteReplenishEvent : Event<IncompleteReplenishEvent.PayloadData>
    {

        public IncompleteReplenishEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(ReplenishClass Replenish = null)
                : base()
            {
                this.Replenish = Replenish;
            }

            [DataContract]
            public sealed class ReplenishClass
            {
                public ReplenishClass(int? NumberOfItemsRemoved = null, int? NumberOfItemsRejected = null, List<ReplenishTargetResultsClass> ReplenishTargetResults = null)
                {
                    this.NumberOfItemsRemoved = NumberOfItemsRemoved;
                    this.NumberOfItemsRejected = NumberOfItemsRejected;
                    this.ReplenishTargetResults = ReplenishTargetResults;
                }

                /// <summary>
                /// Total number of items removed from the source cash unit including rejected items during execution of this command.
                /// </summary>
                [DataMember(Name = "numberOfItemsRemoved")]
                public int? NumberOfItemsRemoved { get; private set; }

                /// <summary>
                /// Total number of items rejected during execution of this command.
                /// </summary>
                [DataMember(Name = "numberOfItemsRejected")]
                public int? NumberOfItemsRejected { get; private set; }

                [DataContract]
                public sealed class ReplenishTargetResultsClass
                {
                    public ReplenishTargetResultsClass(string CashunitTarget = null, int? NoteID = null, int? NumberOfItemsReceived = null)
                    {
                        this.CashunitTarget = CashunitTarget;
                        this.NoteID = NoteID;
                        this.NumberOfItemsReceived = NumberOfItemsReceived;
                    }

                    /// <summary>
                    /// Object name of the cash unit (as stated by the [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) 
                    /// command) to which items have been moved.
                    /// </summary>
                    [DataMember(Name = "cashunitTarget")]
                    public string CashunitTarget { get; private set; }

                    /// <summary>
                    /// Identification of note type. The note ID represents the note identifiers reported by the CashAcceptor.BanknoteTypes command.
                    /// </summary>
                    [DataMember(Name = "noteID")]
                    public int? NoteID { get; private set; }

                    /// <summary>
                    /// Total number of items received in this target cash unit of the *noteID* note type. A zero value will be returned if this target cash unit did not receive any items of this note type, for example due to a cash unit or transport jam.
                    /// </summary>
                    [DataMember(Name = "numberOfItemsReceived")]
                    public int? NumberOfItemsReceived { get; private set; }

                }

                /// <summary>
                /// Array of replenishTargetResult structures. In the case where one note type has several releases and these are moved, 
                /// or where items are moved from a multi denomination cash unit to a multi denomination cash unit, each target can receive several *noteID* note types. 
                /// For example: If one single target was specified with the *replenishTargets* input structure, and this target received two different *noteID* note types, 
                /// then the *replenishTargetResults* array will have two elements. Or if two targets were specified and the first 
                /// target received two different *noteID* note types and the second target received three different *noteID* note types, then the *replenishTargetResults* array will have five elements.
                /// </summary>
                [DataMember(Name = "replenishTargetResults")]
                public List<ReplenishTargetResultsClass> ReplenishTargetResults { get; private set; }

            }

            /// <summary>
            /// Note that in this case the values in this structure report the amount and number of each denomination that have actually been moved during the replenishment command.
            /// </summary>
            [DataMember(Name = "replenish")]
            public ReplenishClass Replenish { get; private set; }

        }

    }
}
