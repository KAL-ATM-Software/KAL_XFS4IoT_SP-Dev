/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * Denominate_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Dispenser.Commands
{
    //Original name = Denominate
    [DataContract]
    [Command(Name = "Dispenser.Denominate")]
    public sealed class DenominateCommand : Command<DenominateCommand.PayloadData>
    {
        public DenominateCommand(int RequestId, DenominateCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? TellerID = null, int? MixNumber = null, DenominationClass Denomination = null)
                : base(Timeout)
            {
                this.TellerID = TellerID;
                this.MixNumber = MixNumber;
                this.Denomination = Denomination;
            }

            /// <summary>
            /// Identification of teller. This field is ignored if the device is a Self-Service Dispenser.
            /// </summary>
            [DataMember(Name = "tellerID")]
            public int? TellerID { get; private set; }

            /// <summary>
            /// Mix algorithm or house mix table to be used.
            /// </summary>
            [DataMember(Name = "mixNumber")]
            public int? MixNumber { get; private set; }

            [DataContract]
            public sealed class DenominationClass
            {
                public DenominationClass(Dictionary<string, double> Currencies = null, Dictionary<string, int> Values = null, int? CashBox = null)
                {
                    this.Currencies = Currencies;
                    this.Values = Values;
                    this.CashBox = CashBox;
                }

                /// <summary>
                /// \"List of currency and amount combinations for denomination. There will be one entry for each currency
                /// in the denomination. The property name is the currency name in ISO format (e.g. \"EUR\").
                /// </summary>
                [DataMember(Name = "currencies")]
                public Dictionary<string, double> Currencies { get; private set; }

                /// <summary>
                /// This list specifies the number of items to take from the cash units. 
                /// Each entry uses a cashunit object name as stated by the 
                /// [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) command. The value of the entry is the 
                /// number of items to take from that unit.
                /// If the application does not wish to specify a denomination, it should omit the values property.
                /// </summary>
                [DataMember(Name = "values")]
                public Dictionary<string, int> Values { get; private set; }

                /// <summary>
                /// Only applies to Teller Dispensers. Amount to be paid from the teller’s cash box.
                /// </summary>
                [DataMember(Name = "cashBox")]
                public int? CashBox { get; private set; }

            }

            /// <summary>
            /// Denomination object describing the contents of the denomination operation.
            /// </summary>
            [DataMember(Name = "denomination")]
            public DenominationClass Denomination { get; private set; }

        }
    }
}
