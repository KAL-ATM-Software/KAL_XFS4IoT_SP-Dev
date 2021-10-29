/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "CashDispenser.Denominate")]
    public sealed class DenominateCommand : Command<DenominateCommand.PayloadData>
    {
        public DenominateCommand(int RequestId, DenominateCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, DenominationClass Denomination = null, string Mix = null, int? TellerID = null)
                : base(Timeout)
            {
                this.Denomination = Denomination;
                this.Mix = Mix;
                this.TellerID = TellerID;
            }

            [DataMember(Name = "denomination")]
            public DenominationClass Denomination { get; init; }

            /// <summary>
            /// Mix algorithm or house mix table to be used as defined by mixes reported by
            /// [CashDispenser.GetMixTypes](#cashdispenser.getmixtypes). May be omitted if the request is entirely specified
            /// by _counts_.
            /// <example>mix1</example>
            /// </summary>
            [DataMember(Name = "mix")]
            public string Mix { get; init; }

            /// <summary>
            /// Only applies to Teller Dispensers. Identification of teller.
            /// </summary>
            [DataMember(Name = "tellerID")]
            public int? TellerID { get; init; }

        }
    }
}
