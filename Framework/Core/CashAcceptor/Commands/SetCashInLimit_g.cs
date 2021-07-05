/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * SetCashInLimit_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = SetCashInLimit
    [DataContract]
    [Command(Name = "CashAcceptor.SetCashInLimit")]
    public sealed class SetCashInLimitCommand : Command<SetCashInLimitCommand.PayloadData>
    {
        public SetCashInLimitCommand(int RequestId, SetCashInLimitCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? TotalItemsLimit = null, AmountLimitClass AmountLimit = null)
                : base(Timeout)
            {
                this.TotalItemsLimit = TotalItemsLimit;
                this.AmountLimit = AmountLimit;
            }

            /// <summary>
            /// If set to a non-zero value, specifies a limit on the total number of items to be accepted during the cash-in transaction. If set to a zero value, this limitation will not be performed.
            /// This limitation can only be used if \"limitByTotalItems\" is specified in the *cashInLimit* field of the Common.Capabilities command. 
            /// If *totalItemsLimit* is non-zero but not supported the UnsupportedData error will be returned and no limit will be set.
            /// This parameter overrides any previously set limit on the total number of items.
            /// </summary>
            [DataMember(Name = "totalItemsLimit")]
            public int? TotalItemsLimit { get; private set; }

            [DataContract]
            public sealed class AmountLimitClass
            {
                public AmountLimitClass(string CurrencyID = null, double? Amount = null)
                {
                    this.CurrencyID = CurrencyID;
                    this.Amount = Amount;
                }

                /// <summary>
                /// Currency identifier in ISO 4217 format [Ref. 2]. This must not be three ASCII 0x20 characters.
                /// </summary>
                [DataMember(Name = "currencyID")]
                public string CurrencyID { get; private set; }

                /// <summary>
                /// If set to a non-zero value, specifies a limit on the total amount of the cash-in transaction for the specified cCurrencyID. 
                /// If set to a zero value, no amount limit will apply to the specified currency.
                /// </summary>
                [DataMember(Name = "amount")]
                public double? Amount { get; private set; }

            }

            /// <summary>
            /// Array of amountLimit structures. 
            /// This limitation can only be used if \"limitByAmount\" is reported in the *cashInLimit* field of the Common.Capabilities command. If *amountLimit* is not empty but 
            /// not supported the UnsupportedData error will be returned and no limit will be set.
            /// If *amountLimit* is emtpy or omitted, this has no impact.
            /// If *amountLimit* is not empty, this specifies the maximum amount of the currency specified by *currencyID* which can be accepted in the current cash-in transaction. 
            /// If the currency has already been specified for the current cash-in transaction, the maximum amount is overridden for that currency. 
            /// If the currency has not already been specified, it is added to a set of currency specific limits to apply to the cash-in transaction. 
            /// If any currency limits are specified for the current cash-in transaction, the handling of other currencies is dependent on whether the \"refuseOther\" flag is 
            /// reported in the *cashInLimit* field of the Common.Capabilites command.
            /// </summary>
            [DataMember(Name = "amountLimit")]
            public AmountLimitClass AmountLimit { get; private set; }

        }
    }
}
