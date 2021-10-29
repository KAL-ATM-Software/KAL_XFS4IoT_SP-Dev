/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CashInStart_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = CashInStart
    [DataContract]
    [Command(Name = "CashAcceptor.CashInStart")]
    public sealed class CashInStartCommand : Command<CashInStartCommand.PayloadData>
    {
        public CashInStartCommand(int RequestId, CashInStartCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? TellerID = null, bool? UseRecycleUnits = null, CashManagement.OutputPositionEnum? OutputPosition = null, CashManagement.InputPositionEnum? InputPosition = null, int? TotalItemsLimit = null, List<AmountLimitClass> AmountLimit = null)
                : base(Timeout)
            {
                this.TellerID = TellerID;
                this.UseRecycleUnits = UseRecycleUnits;
                this.OutputPosition = OutputPosition;
                this.InputPosition = InputPosition;
                this.TotalItemsLimit = TotalItemsLimit;
                this.AmountLimit = AmountLimit;
            }

            /// <summary>
            /// Identification of teller. This field is not applicable to Self-Service devices and should be omitted.
            /// </summary>
            [DataMember(Name = "tellerID")]
            [DataTypes(Minimum = 0)]
            public int? TellerID { get; init; }

            /// <summary>
            /// Specifies whether or not the recycle cash units should be used when items are cashed in on a 
            /// successful [CashAcceptor.CashInEnd](#cashacceptor.cashinend) command. This parameter will be ignored if 
            /// there are no recycle cash units or the hardware does not support this.
            /// </summary>
            [DataMember(Name = "useRecycleUnits")]
            public bool? UseRecycleUnits { get; init; }

            [DataMember(Name = "outputPosition")]
            public CashManagement.OutputPositionEnum? OutputPosition { get; init; }

            [DataMember(Name = "inputPosition")]
            public CashManagement.InputPositionEnum? InputPosition { get; init; }

            /// <summary>
            /// If set to a non-zero value, specifies a limit on the total number of items to be accepted during the cash-in
            /// transaction. If set to a zero value, this limitation will not be performed. This limitation can only be used
            /// if [byTotalItems](#common.capabilities.completion.properties.cashacceptor.cashinlimit.bytotalitems) is true.
            /// </summary>
            [DataMember(Name = "totalItemsLimit")]
            [DataTypes(Minimum = 0)]
            public int? TotalItemsLimit { get; init; }

            [DataContract]
            public sealed class AmountLimitClass
            {
                public AmountLimitClass(string Currency = null, double? Value = null)
                {
                    this.Currency = Currency;
                    this.Value = Value;
                }

                /// <summary>
                /// ISO 4217 currency.
                /// <example>USD</example>
                /// </summary>
                [DataMember(Name = "currency")]
                public string Currency { get; init; }

                /// <summary>
                /// The maximum absolute value of the specified currency which can be accepted in the cash-in transaction.
                /// If 0, there is no amount limit applied to the currency.
                /// <example>20</example>
                /// </summary>
                [DataMember(Name = "value")]
                public double? Value { get; init; }

            }

            /// <summary>
            /// If specified, provides a list of the maximum amount of one or more currencies to be accepted during the 
            /// cash-in transaction. This limitation can only be used if
            /// [byAmount](#common.capabilities.completion.properties.cashacceptor.cashinlimit.byamount) is true.
            /// 
            /// If not specified, no currency specific limit is placed on the transaction.
            /// 
            /// If specified for one currency and the device can handle multiple currencies in a single cash-in transaction, 
            /// any currencies not defined in this array are refused.
            /// </summary>
            [DataMember(Name = "amountLimit")]
            public List<AmountLimitClass> AmountLimit { get; init; }

        }
    }
}
