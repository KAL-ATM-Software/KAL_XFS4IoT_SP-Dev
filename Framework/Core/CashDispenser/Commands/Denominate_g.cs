/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * Denominate_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashDispenser.Commands
{
    //Original name = Denominate
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "CashDispenser.Denominate")]
    public sealed class DenominateCommand : Command<DenominateCommand.PayloadData>
    {
        public DenominateCommand(int RequestId, DenominateCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(DenominateRequestClass Request = null)
                : base()
            {
                this.Request = Request;
            }

            /// <summary>
            /// The request to be denominated.
            /// </summary>
            [DataMember(Name = "request")]
            public DenominateRequestClass Request { get; init; }

        }
    }
}
