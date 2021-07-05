/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * GetPresentStatus_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Dispenser.Completions
{
    [DataContract]
    [Completion(Name = "Dispenser.GetPresentStatus")]
    public sealed class GetPresentStatusCompletion : Completion<GetPresentStatusCompletion.PayloadData>
    {
        public GetPresentStatusCompletion(int RequestId, GetPresentStatusCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, DenominationClass Denomination = null, PresentStateEnum? PresentState = null, List<string> Extra = null, string Token = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Denomination = Denomination;
                this.PresentState = PresentState;
                this.Extra = Extra;
                this.Token = Token;
            }

            public enum ErrorCodeEnum
            {
                UnsupportedPosition
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```unsupportedPosition``` - The specified output position is not supported.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; private set; }

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
            /// Denomination structure which contains the amount dispensed 
            /// from the specified output position and the number of items dispensed from each cash unit. 
            /// Where the capability *moveItems* reports *toStacker* this 
            /// value is cumulative across a series of Dispenser.Dispense calls that add additional items to the stacker.
            /// Where mixed currencies were dispensed the *amount* field in the returned denomination structure will be 
            /// zero and the *currencyID* field will be omitted.
            /// </summary>
            [DataMember(Name = "denomination")]
            public DenominationClass Denomination { get; private set; }

            public enum PresentStateEnum
            {
                Presented,
                NotPresented,
                Unknown
            }

            /// <summary>
            /// Supplies the status of the last dispense or present operation. Following values are possible:
            /// 
            /// * ```presented``` - The items were presented. This status is set as soon as the customer has access to the items.
            /// * ```notPresented``` - The customer has not had access to the items.
            /// * ```unknown``` - It is not known if the customer had access to the items.
            /// </summary>
            [DataMember(Name = "presentState")]
            public PresentStateEnum? PresentState { get; private set; }

            /// <summary>
            /// Pointer to a list of vendor-specific, or any other extended, information. 
            /// The information is returned as a series of “key=value” strings so that it is easily extensible by Service Providers.
            /// </summary>
            [DataMember(Name = "extra")]
            public List<string> Extra { get; private set; }

            /// <summary>
            /// $ref: ../Docs/PresentStatusToken.md
            /// </summary>
            [DataMember(Name = "token")]
            public string Token { get; private set; }

        }
    }
}
