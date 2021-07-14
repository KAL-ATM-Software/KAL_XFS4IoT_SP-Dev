/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * GetTellerInfo_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashManagement.Commands
{
    //Original name = GetTellerInfo
    [DataContract]
    [Command(Name = "CashManagement.GetTellerInfo")]
    public sealed class GetTellerInfoCommand : Command<GetTellerInfoCommand.PayloadData>
    {
        public GetTellerInfoCommand(int RequestId, GetTellerInfoCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? TellerID = null, string CurrencyID = null)
                : base(Timeout)
            {
                this.TellerID = TellerID;
                this.CurrencyID = CurrencyID;
            }

            /// <summary>
            /// Identification of the teller. If the value of *tellerID* is not valid the error *invalidTellerID* is reported.
            /// </summary>
            [DataMember(Name = "tellerID")]
            public int? TellerID { get; init; }

            /// <summary>
            /// Three character ISO format currency identifier [Ref 2].
            /// </summary>
            [DataMember(Name = "currencyID")]
            public string CurrencyID { get; init; }

        }
    }
}
