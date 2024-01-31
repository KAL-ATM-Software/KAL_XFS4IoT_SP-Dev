/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CashUnitCount_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = CashUnitCount
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashAcceptor.CashUnitCount")]
    public sealed class CashUnitCountCommand : Command<CashUnitCountCommand.PayloadData>
    {
        public CashUnitCountCommand(int RequestId, CashUnitCountCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<string> Units = null)
                : base()
            {
                this.Units = Units;
            }

            /// <summary>
            /// Array containing the [identifiers](#storage.getstorage.completion.properties.storage) of the individual
            /// storage units to be counted.
            /// If an invalid storage unit is contained in this list, the command will fail with a *cashUnitError*
            /// *errorCode*.
            /// 
            /// <example>["unit1", "unit2"]</example>
            /// </summary>
            [DataMember(Name = "units")]
            public List<string> Units { get; init; }

        }
    }
}
