/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "Printer.ResetCount")]
    public sealed class ResetCountCommand : Command<ResetCountCommand.PayloadData>
    {
        public ResetCountCommand(int RequestId, ResetCountCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? BinNumber = null)
                : base(Timeout)
            {
                this.BinNumber = BinNumber;
            }

            /// <summary>
            /// Specifies the height of the media in terms of the base vertical resolution.
            /// </summary>
            [DataMember(Name = "binNumber")]
            public int? BinNumber { get; private set; }

        }
    }
}
