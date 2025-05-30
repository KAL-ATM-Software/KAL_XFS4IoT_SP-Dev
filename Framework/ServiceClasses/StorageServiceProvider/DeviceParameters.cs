﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.

\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoTServer;
using XFS4IoT;
using XFS4IoT.Completions;
using XFS4IoT.Storage.Commands;
using XFS4IoT.Storage.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Storage
{
    public enum FailureEnum
    {
        Empty,
        Error,
        Full,
        Locked,
        Invalid,
        Config,
        NotConfigured
    }

    public sealed class StartExchangeResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        StartExchangeCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
        : DeviceResult(CompletionCode, ErrorDescription)
    {
        public StartExchangeCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }

    public sealed class EndExchangeResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        EndExchangeCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)
    {
        public EndExchangeCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }

    public sealed record SetCardConfiguration
    {
        public SetCardConfiguration(
            int? Threshold,
            string CardId = null)
        {
            this.CardId = CardId;
            this.Threshold = Threshold;
        }

        public string CardId { get; set; }

        public int? Threshold { get; set; }
    }

    public sealed class SetCardUnitStorage(
        SetCardConfiguration Configuration,
        int? InitialCount)
    {

        /// <summary>
        /// If this property is null, no need to change card unit configuration
        /// </summary>
        public SetCardConfiguration Configuration { get; init; } = Configuration;

        /// <summary>
        /// Set to InitialCount and Count, reset to RetainCount to zero
        /// If this property is not set, no need to update in the device class
        /// </summary>
        public int? InitialCount { get; init; } = InitialCount;
    }

    public sealed class SetCardStorageRequest(Dictionary<string, SetCardUnitStorage> CardStorageToSet)
    {
        public Dictionary<string, SetCardUnitStorage> CardStorageToSet { get; init; } = CardStorageToSet;
    }

    public sealed class SetCardStorageResult : DeviceResult
    {
        public SetCardStorageResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            SetStorageCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            NewCardStorage = null;
        }

        public SetCardStorageResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            Dictionary<string, SetCardUnitStorage> NewCardStorage)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.NewCardStorage = NewCardStorage;
        }

        public SetStorageCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        public Dictionary<string, SetCardUnitStorage> NewCardStorage { get; init; }
    }

    /// <summary>
    /// Configuration of the cash unit
    /// </summary>
    public sealed record SetCashConfiguration
    {
        public SetCashConfiguration(
            CashCapabilitiesClass.TypesEnum? Types,
            CashCapabilitiesClass.ItemsEnum? Items,
            string Currency,
            double? Value,
            int? HighThreshold,
            int? LowThreshold,
            bool? AppLockIn,
            bool? AppLockOut,
            List<string> BanknoteItems,
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
        /// If specified, ReplenishmentStatus is set to Low if the count is lower than this number.
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
        public List<string> BanknoteItems { get; set; }

        /// <summary>
        /// Application configured name of the unit.
        /// Null if the application doesn't set to change
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If specified, this is the number of retract operations allowed into the unit.
        /// If not specified, the maximum number is not limited by counts.
        /// </summary>
        public int? MaxRetracts { get; set; }
    }

    public sealed class SetCashUnitStorage(
        SetCashConfiguration Configuration,
        StorageCashCountClass InitialCounts)
    {

        /// <summary>
        /// If this property is null, no need to change card unit configuration
        /// </summary>
        public SetCashConfiguration Configuration { get; init; } = Configuration;

        /// <summary>
        /// Set to InitialCounts
        /// </summary>
        public StorageCashCountClass InitialCounts { get; init; } = InitialCounts;
    }

    public sealed class SetCashStorageRequest(Dictionary<string, SetCashUnitStorage> CashStorageToSet)
    {
        public Dictionary<string, SetCashUnitStorage> CashStorageToSet { get; init; } = CashStorageToSet;
    }

    public sealed class SetCashStorageResult : DeviceResult
    {
        public SetCashStorageResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            SetStorageCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            NewCashStorage = null;
        }

        public SetCashStorageResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            Dictionary<string, SetCashUnitStorage> NewCashStorage)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.NewCashStorage = NewCashStorage;
        }

        public SetStorageCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        public Dictionary<string, SetCashUnitStorage> NewCashStorage { get; init; }
    }

    /// <summary>
    /// Configuration of the check unit
    /// </summary>
    public sealed record SetCheckConfiguration
    {
        public SetCheckConfiguration(
            CheckCapabilitiesClass.TypesEnum? Types,
            string Id,
            int? HighThreshold,
            int? RetractHighThreshold)
        {
            this.Types = Types;
            this.Id = Id;
            this.HighThreshold = HighThreshold;
            this.RetractHighThreshold = RetractHighThreshold;
            this.HighThreshold = HighThreshold;
        }

        /// <summary>
        /// If specified, The types of operation the unit is capable of or configured to perform. 
        /// This is a combination of one or  more operations.
        /// </summary>
        public CheckCapabilitiesClass.TypesEnum? Types { get; set; }

        /// <summary>
        /// An application defined Storage Unit Identifier.
        /// Null if the application doesn't set to change.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// If specified, ReplenishmentStatus is set to High if the total number of items
        /// in the storage unit is greater than this number.
        /// </summary>
        public int? HighThreshold { get; set; }

        /// <summary>
        /// If specified and the storage unit is configured as Retract,
        /// ReplenishmentStatus is set to High if the total number of retract operations 
        /// in the storage unit is greater than this number.
        /// </summary>
        public int? RetractHighThreshold { get; set; }
    }

    public sealed class SetCheckUnitStorage(
        SetCheckConfiguration Configuration,
        int? MediaInCount,
        int? Count,
        int? RetractOperations)
    {

        /// <summary>
        /// If this property is null, no need to change check unit configuration
        /// </summary>
        public SetCheckConfiguration Configuration { get; init; } = Configuration;

        /// <summary>
        /// If specified, Count of items added to the storage unit due to Check operations. 
        /// If the number of items is not counted this is not reported and RetractOperations
        /// is incremented as items are added to the unit.
        /// </summary>
        public int? MediaInCount { get; init; } = MediaInCount;

        /// <summary>
        /// If specified, Total number of items added to the storage unit due to any operations. 
        /// If the number of items is not counted this is not reported and RetractOperations is 
        /// incremented as items are added to the unit.
        /// </summary>
        public int? Count { get; init; } = Count;

        /// <summary>
        /// If specified, Total number of operations which resulted in items being retracted to the storage unit.
        /// </summary>
        public int? RetractOperations { get; init; } = RetractOperations;
    }

    public sealed class SetCheckStorageRequest(Dictionary<string, SetCheckUnitStorage> CheckStorageToSet)
    {
        public Dictionary<string, SetCheckUnitStorage> CheckStorageToSet { get; init; } = CheckStorageToSet;
    }

    public sealed class SetCheckStorageResult : DeviceResult
    {
        public SetCheckStorageResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            SetStorageCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            NewCheckStorage = null;
        }

        public SetCheckStorageResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            Dictionary<string, SetCheckUnitStorage> NewCheckStorage)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.NewCheckStorage = NewCheckStorage;
        }

        public SetStorageCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        public Dictionary<string, SetCheckUnitStorage> NewCheckStorage { get; init; }
    }

    /// <summary>
    /// Configuration of the printer unit
    /// </summary>
    public sealed record SetPrinterConfiguration
    {
        public SetPrinterConfiguration()
        { }
    }

    public sealed class SetPrinterUnitStorage(
        SetPrinterConfiguration Configuration = null,
        int? InitialCount = null)
    {

        /// <summary>
        /// Printer configuration is not supported in the specification -3.
        /// </summary>
        public SetPrinterConfiguration Configuration { get; init; } = Configuration;

        /// <summary>
        /// If specified, the printer related count (retract paper, passbook) as set at the last replenishment.
        /// </summary>
        public int? InitialCount { get; init; } = InitialCount;
    }

    public sealed class SetPrinterStorageRequest(Dictionary<string, SetPrinterUnitStorage> PrinterStorageToSet)
    {
        public Dictionary<string, SetPrinterUnitStorage> PrinterStorageToSet { get; init; } = PrinterStorageToSet;
    }

    public sealed class SetPrinterStorageResult : DeviceResult
    {
        public SetPrinterStorageResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            SetStorageCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            NewPrinterStorage = null;
        }

        public SetPrinterStorageResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            Dictionary<string, SetPrinterUnitStorage> NewPrinterStorage)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.NewPrinterStorage = NewPrinterStorage;
        }

        public SetStorageCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        public Dictionary<string, SetPrinterUnitStorage> NewPrinterStorage { get; init; }
    }

    /// <summary>
    /// Configuration of the deposit unit
    /// </summary>
    public sealed record SetDepositConfiguration
    {
        public SetDepositConfiguration()
        { }
    }

    public sealed class SetDepositUnitStorage(
        SetDepositConfiguration Configuration = null,
        int? InitialCount = null)
    {

        /// <summary>
        /// Deposit configuration is not supported in the specification -3.
        /// </summary>
        public SetDepositConfiguration Configuration { get; init; } = Configuration;

        /// <summary>
        /// If specified, the deposit related count (box or bag) as set at the last replenishment.
        /// </summary>
        public int? InitialCount { get; init; } = InitialCount;
    }

    public sealed class SetDepositStorageRequest(Dictionary<string, SetDepositUnitStorage> DepositStorageToSet)
    {
        public Dictionary<string, SetDepositUnitStorage> PrinterStorageToSet { get; init; } = DepositStorageToSet;
    }

    public sealed class SetDepositStorageResult : DeviceResult
    {
        public SetDepositStorageResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            SetStorageCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            NewDepositStorage = null;
        }

        public SetDepositStorageResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            Dictionary<string, SetDepositUnitStorage> NewDepositStorage)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.NewDepositStorage = NewDepositStorage;
        }

        public SetStorageCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        public Dictionary<string, SetDepositUnitStorage> NewDepositStorage { get; init; }
    }
}