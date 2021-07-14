/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * Dispense_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Dispenser.Commands
{
    //Original name = Dispense
    [DataContract]
    [Command(Name = "Dispenser.Dispense")]
    public sealed class DispenseCommand : Command<DispenseCommand.PayloadData>
    {
        public DispenseCommand(int RequestId, DispenseCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? TellerID = null, int? MixNumber = null, PositionEnum? Position = null, DenominationClass Denomination = null, string Token = null)
                : base(Timeout)
            {
                this.TellerID = TellerID;
                this.MixNumber = MixNumber;
                this.Position = Position;
                this.Denomination = Denomination;
                this.Token = Token;
            }

            /// <summary>
            /// Identifies the teller. This field is ignored if the device is a Self-Service Dispenser.
            /// </summary>
            [DataMember(Name = "tellerID")]
            public int? TellerID { get; init; }

            /// <summary>
            /// Mix algorithm or house mix table to be used to create a denomination of the supplied amount. 
            /// If the value is 0 ("individual"), the denomination supplied in the *denomination* field is 
            /// validated prior to the dispense operation. If it is found to be invalid no alternative denomination will be calculated.
            /// </summary>
            [DataMember(Name = "mixNumber")]
            public int? MixNumber { get; init; }

            public enum PositionEnum
            {
                Default,
                Left,
                Right,
                Center,
                Top,
                Bottom,
                Front,
                Rear
            }

            /// <summary>
            /// Required output position. Following values are possible:
            /// 
            /// * ```default``` - The default configuration information is used. This can be either position dependent or teller dependent.
            /// * ```left``` - Present items to left side of device.
            /// * ```right``` - Present items to right side of device.
            /// * ```center``` - Present items to center output position.
            /// * ```top``` - Present items to the top output position.
            /// * ```bottom``` - Present items to the bottom output position.
            /// * ```front``` - Present items to the front output position.
            /// * ```rear``` - Present items to the rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

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
                /// "List of currency and amount combinations for denomination. There will be one entry for each currency
                /// in the denomination. The property name is the currency name in ISO format (e.g. "EUR").
                /// </summary>
                [DataMember(Name = "currencies")]
                public Dictionary<string, double> Currencies { get; init; }

                /// <summary>
                /// This list specifies the number of items to take from the cash units. 
                /// Each entry uses a cashunit object name as stated by the 
                /// [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) command. The value of the entry is the 
                /// number of items to take from that unit.
                /// If the application does not wish to specify a denomination, it should omit the values property.
                /// </summary>
                [DataMember(Name = "values")]
                public Dictionary<string, int> Values { get; init; }

                /// <summary>
                /// Only applies to Teller Dispensers. Amount to be paid from the teller’s cash box.
                /// </summary>
                [DataMember(Name = "cashBox")]
                public int? CashBox { get; init; }

            }

            /// <summary>
            /// Denomination object describing the denominations used for the dispense operation.
            /// </summary>
            [DataMember(Name = "denomination")]
            public DenominationClass Denomination { get; init; }

            /// <summary>
            /// The dispense token that authorises the dispense operation, as created by the authorising host. See 
            /// the section on end to end security for more information. 
            /// 
            /// The dispense token will follow the standard token format, and will contain the following key: 
            /// 
            /// "DISPENSE1": The maximum value to be dispensed. This will be a number string that may contain a fractional 
            /// part. The decimal character will be ".". The value, including the fractional part, will be 
            /// defined by the ISO currency. The number will be followed by the ISO currency code. The currency 
            /// code will be upper case. 
            /// 
            /// For example, "123.45EUR" will be €123 and 45 cents.
            /// 
            /// The "DISPENSE" key may appear multiple times with a number suffix. For example, DISPENSE1, DISPENSE2, 
            /// DISPENSE3. The number will start at 1 and increment. Each key can only be given once. Each key must 
            /// have a value in a different currency. For example, DISPENSE1=100.00EUR,DISPENSE2=200.00USD   
            /// 
            /// The actual amount dispensed will be given by the denomination. The value in the token MUST be
            /// greater or equal to the amount in the denomination parameter. If the Token has a lower value, 
            /// or the Token is invalid for any reason, then the command will fail with an invalid data error code.
            /// </summary>
            [DataMember(Name = "token")]
            public string Token { get; init; }

        }
    }
}
