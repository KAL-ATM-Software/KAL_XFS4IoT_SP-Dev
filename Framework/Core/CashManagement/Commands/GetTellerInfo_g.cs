/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashManagement.GetTellerInfo")]
    public sealed class GetTellerInfoCommand : Command<GetTellerInfoCommand.PayloadData>
    {
        public GetTellerInfoCommand()
            : base()
        { }

        public GetTellerInfoCommand(int RequestId, GetTellerInfoCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int? TellerID = null, string Currency = null)
                : base()
            {
                this.TellerID = TellerID;
                this.Currency = Currency;
            }

            /// <summary>
            /// Identification of the teller. If invalid the error *invalidTellerId* is reported. If null, all
            /// tellers are reported.
            /// </summary>
            [DataMember(Name = "tellerID")]
            [DataTypes(Minimum = 0)]
            public int? TellerID { get; init; }

            /// <summary>
            /// ISO 4217 format currency identifier [[Ref. cashmanagement-1](#ref-cashmanagement-1)]. If null, all currencies are reported for *tellerID*.
            /// <example>USD</example>
            /// </summary>
            [DataMember(Name = "currency")]
            [DataTypes(Pattern = @"^[A-Z]{3}$")]
            public string Currency { get; init; }

        }
    }
}
