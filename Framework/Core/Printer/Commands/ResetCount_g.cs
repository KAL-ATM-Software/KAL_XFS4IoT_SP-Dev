/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ResetCount_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = ResetCount
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Printer.ResetCount")]
    public sealed class ResetCountCommand : Command<ResetCountCommand.PayloadData>
    {
        public ResetCountCommand(int RequestId, ResetCountCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int? BinNumber = null)
                : base()
            {
                this.BinNumber = BinNumber;
            }

            /// <summary>
            /// The number of the retract bin for which the retract count should be reset to 0. If
            /// 0, all bin counts will be set to 0. See
            /// [retractBins](#common.capabilities.completion.properties.printer.retractbins).
            /// <example>1</example>
            /// </summary>
            [DataMember(Name = "binNumber")]
            [DataTypes(Minimum = 0)]
            public int? BinNumber { get; init; }

        }
    }
}
