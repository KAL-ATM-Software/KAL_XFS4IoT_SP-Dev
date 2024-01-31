/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashAcceptor.CashInStart")]
    public sealed class CashInStartCommand : Command<CashInStartCommand.PayloadData>
    {
        public CashInStartCommand(int RequestId, CashInStartCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int? TellerID = null, bool? UseRecycleUnits = null, CashManagement.OutputPositionEnum? OutputPosition = null, CashManagement.InputPositionEnum? InputPosition = null, int? TotalItemsLimit = null, List<AmountLimitClass> AmountLimit = null)
                : base()
            {
                this.TellerID = TellerID;
                this.UseRecycleUnits = UseRecycleUnits;
                this.OutputPosition = OutputPosition;
                this.InputPosition = InputPosition;
                this.TotalItemsLimit = TotalItemsLimit;
                this.AmountLimit = AmountLimit;
            }

            /// <summary>
            /// Identification of teller. This property is not applicable to Self-Service devices and can therefore be null.
            /// </summary>
            [DataMember(Name = "tellerID")]
            [DataTypes(Minimum = 0)]
            public int? TellerID { get; init; }

            /// <summary>
            /// Specifies whether or not the recycle storage units should be used when items are cashed in on a
            /// successful [CashAcceptor.CashInEnd](#cashacceptor.cashinend) command. This property will be ignored if
            /// there are no recycle storage units or the hardware does not support this.
            /// </summary>
            [DataMember(Name = "useRecycleUnits")]
            public bool? UseRecycleUnits { get; init; }

            [DataMember(Name = "outputPosition")]
            public CashManagement.OutputPositionEnum? OutputPosition { get; init; }

            [DataMember(Name = "inputPosition")]
            public CashManagement.InputPositionEnum? InputPosition { get; init; }

            /// <summary>
            /// If set to a non-zero value, specifies a limit on the total number of items to be accepted during the cash-in
            /// transaction. If set to 0, there will be no limit on the number of items. This limitation can only be used
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
                /// ISO 4217 currency identifier [[Ref. cashmanagement-1](#ref-cashmanagement-1)].
                /// <example>USD</example>
                /// </summary>
                [DataMember(Name = "currency")]
                [DataTypes(Pattern = @"^[A-Z]{3}$")]
                public string Currency { get; init; }

                /// <summary>
                /// Absolute value of a cash item or items. May be a floating point value to allow for coins and notes which have
                /// a value which is not a whole multiple of the currency unit.
                /// 
                /// If applied to a storage unit, this applies to all contents, may be 0 if mixed and may only be modified in
                /// an exchange state if applicable.
                /// 
                /// May be null in command data or events if not being modified.
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
            /// 
            /// If a value of null is specified for a currency, there is no amount limit applied to the currency.
            /// </summary>
            [DataMember(Name = "amountLimit")]
            public List<AmountLimitClass> AmountLimit { get; init; }

        }
    }
}
