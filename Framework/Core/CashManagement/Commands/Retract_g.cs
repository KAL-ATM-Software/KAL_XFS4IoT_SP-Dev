/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * Retract_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashManagement.Commands
{
    //Original name = Retract
    [DataContract]
    [Command(Name = "CashManagement.Retract")]
    public sealed class RetractCommand : Command<RetractCommand.PayloadData>
    {
        public RetractCommand(int RequestId, RetractCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, OutputPositionEnum? OutputPosition = null, RetractAreaEnum? RetractArea = null, int? Index = null)
                : base(Timeout)
            {
                this.OutputPosition = OutputPosition;
                this.RetractArea = RetractArea;
                this.Index = Index;
            }

            [DataMember(Name = "outputPosition")]
            public OutputPositionEnum? OutputPosition { get; init; }

            [DataMember(Name = "retractArea")]
            public RetractAreaEnum? RetractArea { get; init; }

            /// <summary>
            /// If _retractArea_ is set to _retract_ this field defines the position inside the retract storage units into 
            /// which the cash is to be retracted. _index_ starts with a value of one (1) for the first retract position 
            /// and increments by one for each subsequent position. If there are several retract storage units 
            /// (of type _retractCassette_ in [Storage.GetStorage](#storage.getstorage)), _index_ would be incremented from the 
            /// first position of the first retract storage unit to the last position of the last retract storage unit. 
            /// The maximum value of _index_ is the sum of _maximum_ of each retract storage unit. If _retractArea_ is not 
            /// set to _retract_ the value of this field is ignored.
            /// </summary>
            [DataMember(Name = "index")]
            public int? Index { get; init; }

        }
    }
}
