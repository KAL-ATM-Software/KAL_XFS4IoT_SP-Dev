/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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

            public PayloadData(int Timeout, string Source = null, List<ReplenishTargetsClass> ReplenishTargets = null)
                : base(Timeout)
            {
                this.Source = Source;
                this.ReplenishTargets = ReplenishTargets;
            }

            /// <summary>
            /// Name of the storage unit (as stated by the [Storage.GetStorage](#storage.getstorage) 
            /// command) from which items are to be removed.
            /// <example>unit1</example>
            /// </summary>
            [DataMember(Name = "source")]
            [DataTypes(Pattern = @"^unit[0-9A-Za-z]+$")]
            public string Source { get; init; }

            [DataContract]
            public sealed class ReplenishTargetsClass
            {
                public ReplenishTargetsClass(string Target = null, int? NumberOfItemsToMove = null)
                {
                    this.Target = Target;
                    this.NumberOfItemsToMove = NumberOfItemsToMove;
                }

                /// <summary>
                /// Object name of the storage unit (as stated by the [Storage.GetStorage](#storage.getstorage) 
                /// command) to which items are to be moved.
                /// <example>unit1</example>
                /// </summary>
                [DataMember(Name = "target")]
                [DataTypes(Pattern = @"^unit[0-9A-Za-z]+$")]
                public string Target { get; init; }

                /// <summary>
                /// The number of items to be moved to the target storage unit. If 0, all items will be moved.
                /// Any items which are removed from the 
                /// source storage unit that are not of the correct currency and value for the target storage unit 
                /// during execution of this command will be returned to the source storage unit.
                /// <example>100</example>
                /// </summary>
                [DataMember(Name = "numberOfItemsToMove")]
                [DataTypes(Minimum = 0)]
                public int? NumberOfItemsToMove { get; init; }

            }

            /// <summary>
            /// Array of target elements specifying how many items are to be moved and to where. There must be at least one
            /// array element.
            /// </summary>
            [DataMember(Name = "replenishTargets")]
            public List<ReplenishTargetsClass> ReplenishTargets { get; init; }

        }
    }
}
