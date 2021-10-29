/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.

\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Storage.Commands;
using XFS4IoT.Storage.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Storage
{
    public sealed class StartExchangeResult : DeviceResult
    {
        public StartExchangeResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                   string ErrorDescription = null,
                                   StartExchangeCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public StartExchangeCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }

    public sealed class EndExchangeResult : DeviceResult
    {
        public EndExchangeResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                 string ErrorDescription = null,
                                 EndExchangeCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public EndExchangeCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }

    public sealed record SetCardConfiguration
    {
        public SetCardConfiguration(int? Threshold,
                                    string CardId = null)
        {
            this.CardId = CardId;
            this.Threshold = Threshold;
        }

        public string CardId { get; set; }

        public int? Threshold { get; set; }
    }

    public sealed class SetCardUnitStorage
    {
        public SetCardUnitStorage(SetCardConfiguration Configuration,
                                  int? InitialCount)
        {
            this.Configuration = Configuration;
            this.InitialCount = InitialCount;
        }

        /// <summary>
        /// If this property is null, no need to change card unit configuration
        /// </summary>
        public SetCardConfiguration Configuration { get; init; }

        /// <summary>
        /// Set to InitialCount and Count, reset to RetainCount to zero
        /// If this property is not set, no need to update in the device class
        /// </summary>
        public int? InitialCount { get; init; }
    }

    public sealed class SetCardStorageRequest
    {
        public SetCardStorageRequest(Dictionary<string, SetCardUnitStorage> CardStorageToSet)
        {
            this.CardStorageToSet = CardStorageToSet;
        }

        public Dictionary<string, SetCardUnitStorage> CardStorageToSet { get; init; }
    }

    public sealed class SetCardStorageResult : DeviceResult
    {
        public SetCardStorageResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                    string ErrorDescription = null,
                                    SetStorageCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            NewCardStorage = null;
        }

        public SetCardStorageResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                    Dictionary<string, SetCardUnitStorage> NewCardStorage)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.NewCardStorage = NewCardStorage;
        }

        public SetStorageCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        public Dictionary<string, SetCardUnitStorage> NewCardStorage { get; init; }
    }

    public sealed record SetCashConfiguration
    {
        public SetCashConfiguration(CashCapabilitiesClass.TypesEnum? Types,
                                    CashCapabilitiesClass.ItemsEnum? Items,
                                    string Currency,
                                    double? Value,
                                    int? HighThreshold,
                                    int? LowThreshold,
                                    bool? AppLockIn,
                                    bool? AppLockOut,
                                    Dictionary<string, BanknoteItem> BanknoteItems,
                                    string Name,
                                    int? MaxRetracts)
        {
            this.Types = Types;
            this.Items = Items;
            this.Currency = Currency;
            this.Value = Value;
            this.HighThreshold = HighThreshold;
            this.LowThreshold = LowThreshold;
            this.AppLockIn = AppLockIn;
            this.AppLockOut = AppLockOut;
            this.BanknoteItems = BanknoteItems;
            this.Name = Name;
            this.MaxRetracts = MaxRetracts;
        }

        /// <summary>
        /// The types of operation the unit is capable of configured to perform. This is a combination of one or 
        /// more operations
        /// </summary>
        public CashCapabilitiesClass.TypesEnum? Types { get; set; }

        /// <summary>
        /// The types of cash media the unit is configured to store. This is a combination of
        /// one or more item types.May only be modified in an exchange state if applicable.
        /// </summary>
        public CashCapabilitiesClass.ItemsEnum? Items { get; set; }

        /// <summary>
        /// ISO 4217 currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Absolute value of all contents, 0 if mixed. May only be modified in an exchange state if applicable. May be 
        /// a floating point value to allow for coins and notes which have a value which is not a whole multiple
        /// of the currency unit.
        /// </summary>
        public double? Value { get; set; }

        /// <summary>
        /// If specified, ReplenishmentStatus is set to High if the count is greater than this number.
        /// </summary>
        public int? HighThreshold { get; set; }

        /// <summary>
        /// IIf specified, ReplenishmentStatus is set to Low if the count is lower than this number.
        /// </summary>
        public int? LowThreshold { get; set; }

        /// <summary>
        /// If true, items cannot be accepted into the storage unit in Cash In operations.
        /// </summary>
        public bool? AppLockIn { get; set; }

        /// <summary>
        /// If true, items cannot be dispensed from the storage unit in Cash Out operations.
        /// </summary>
        public bool? AppLockOut { get; set; }

        /// <summary>
        /// Lists the cash items which are configured to this unit.
        /// </summary>
        public Dictionary<string, BanknoteItem> BanknoteItems { get; set; }

        /// <summary>
        /// Application configured name of the unit.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If specified, this is the number of retract operations allowed into the unit.
        /// If not specified, the maximum number is not limited by counts.
        /// </summary>
        public int? MaxRetracts { get; set; }
    }

    public sealed class SetCashUnitStorage
    {
        public SetCashUnitStorage(SetCashConfiguration Configuration,
                                  StorageCashCountClass InitialCounts)
        {
            this.Configuration = Configuration;
            this.InitialCounts = InitialCounts;
        }

        /// <summary>
        /// If this property is null, no need to change card unit configuration
        /// </summary>
        public SetCashConfiguration Configuration { get; init; }

        /// <summary>
        /// Set to InitialCounts
        /// </summary>
        public StorageCashCountClass InitialCounts { get; init; }
    }

    public sealed class SetCashStorageRequest
    {
        public SetCashStorageRequest(Dictionary<string, SetCashUnitStorage> CashStorageToSet)
        {
            this.CashStorageToSet = CashStorageToSet;
        }

        public Dictionary<string, SetCashUnitStorage> CashStorageToSet { get; init; }
    }

    public sealed class SetCashStorageResult : DeviceResult
    {
        public SetCashStorageResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                    string ErrorDescription = null,
                                    SetStorageCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            NewCashStorage = null;
        }

        public SetCashStorageResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                    Dictionary<string, SetCashUnitStorage> NewCashStorage)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.NewCashStorage = NewCashStorage;
        }

        public SetStorageCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        public Dictionary<string, SetCashUnitStorage> NewCashStorage { get; init; }
    }
}