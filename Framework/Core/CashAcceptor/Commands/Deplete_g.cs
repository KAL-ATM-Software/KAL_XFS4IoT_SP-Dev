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

            public PayloadData(int Timeout, List<DepleteSourcesClass> DepleteSources = null, string CashUnitTarget = null)
                : base(Timeout)
            {
                this.DepleteSources = DepleteSources;
                this.CashUnitTarget = CashUnitTarget;
            }

            [DataContract]
            public sealed class DepleteSourcesClass
            {
                public DepleteSourcesClass(string Source = null, int? NumberOfItemsToMove = null)
                {
                    this.Source = Source;
                    this.NumberOfItemsToMove = NumberOfItemsToMove;
                }

                /// <summary>
                /// Object name of the storage unit (as stated by the [Storage.GetStorage](#storage.getstorage) 
                /// command) from which items are to be removed.
                /// <example>unit1</example>
                /// </summary>
                [DataMember(Name = "source")]
                public string Source { get; init; }

                /// <summary>
                /// The number of items to be moved from the source storage unit. If 0, all items will be moved.
                /// If non-zero, this must be equal to or less than the count of items reported for the cash unit specified by 
                /// _cashUnitSource_. This field will be ignored if the _removeAll_ parameter is set to true.
                /// <example>100</example>
                /// </summary>
                [DataMember(Name = "numberOfItemsToMove")]
                [DataTypes(Minimum = 0)]
                public int? NumberOfItemsToMove { get; init; }

            }

            /// <summary>
            /// Array of DepleteSource structures. There must be at least one element in this array.
            /// </summary>
            [DataMember(Name = "depleteSources")]
            public List<DepleteSourcesClass> DepleteSources { get; init; }

            /// <summary>
            /// Object name of the cash unit (as stated by the [Storage.GetStorage](#storage.getstorage) 
            /// command) to which items are to be moved.
            /// <example>unit2</example>
            /// </summary>
            [DataMember(Name = "cashUnitTarget")]
            public string CashUnitTarget { get; init; }

        }
    }
}
