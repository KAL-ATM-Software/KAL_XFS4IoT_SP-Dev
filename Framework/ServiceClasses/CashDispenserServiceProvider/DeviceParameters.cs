/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Storage;
using XFS4IoT;

namespace XFS4IoTFramework.CashDispenser
{
    /// <summary>
    /// Denomination
    /// Representing output data of the Denominate and PresentStatus
    /// </summary>
    public sealed class Denominate
    {
        private readonly ILogger Logger;

        public enum DispensableResultEnum
        {
            Good,
            CashUnitError,
            CashUnitNotEnough,
            CashUnitLocked,
            InvalidCurrency,
            InvalidDenomination,
        }

        public Denominate(Dictionary<string, double> CurrencyAmounts,
                          ILogger Logger) : this(CurrencyAmounts, null, Logger) { }

        public Denominate(Dictionary<string, double> CurrencyAmounts,
                          Dictionary<string, int> Values, 
                          ILogger Logger )
        {
            this.CurrencyAmounts = CurrencyAmounts;
            this.Logger = Logger.IsNotNull();
            if (Values is not null)
            {
                // Copy only value positive
                this.Values = Values.Where(v => v.Value > 0).ToDictionary(v => v.Key, v => v.Value);
            }
        }

        /// <summary>
        /// Currencies to use for dispensing cash
        /// </summary>
        public Dictionary<string, double> CurrencyAmounts { get; set; }

        /// <summary>
        /// Key is cash unit name and the value is the number of cash to be used
        /// </summary>
        public Dictionary<string, int> Values { get; set; }

        public Denomination Denomination 
        { 
            get => new(CurrencyAmounts, Values); 
            set 
            { 
                CurrencyAmounts = value.CurrencyAmounts;
                Values = value.Values;
            }
        }
        /// <summary>
        /// Check there are enough notes to be dispensed
        /// </summary>
        public DispensableResultEnum IsDispensable(Dictionary<string, CashUnitStorage> CashUnits)
        {
            if (Values is null ||
                Values.Count == 0)
            {
                Logger.Warning(Constants.Framework, "The Value of the cash units are empty to check amount is dispensable.");
                return DispensableResultEnum.InvalidDenomination;
            }

            double internalAmount = 0;

            foreach (var unit in Values)
            {
                if (unit.Value <= 0)
                    continue;

                if (!CashUnits.ContainsKey(unit.Key))
                {
                    Logger.Warning(Constants.Framework, $"No cash unit key name find. {unit.Key} " + nameof(IsDispensable));
                    return DispensableResultEnum.CashUnitError;
                }

                if (!CashUnits[unit.Key].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut))
                {
                    Logger.Warning(Constants.Framework, $"Invalid counts to pick from none dispensable unit. {unit.Key}, {CashUnits[unit.Key].Unit.Configuration.Types}" + nameof(IsDispensable));
                    return DispensableResultEnum.CashUnitError;
                }

                // Check counts first
                if (CashUnits[unit.Key].Unit.Status.Count < unit.Value)
                {
                    Logger.Warning(Constants.Framework, $"Not enough cash to dispense item. {unit.Key}, {CashUnits[unit.Key].Unit.Status.Count}" + nameof(IsDispensable));
                    return DispensableResultEnum.CashUnitNotEnough;
                }

                // Check status
                if (CashUnits[unit.Key].Status != CashUnitStorage.StatusEnum.Good ||
                    (CashUnits[unit.Key].Status == CashUnitStorage.StatusEnum.Good &&
                     CashUnits[unit.Key].Unit.Status.ReplenishmentStatus == CashStatusClass.ReplenishmentStatusEnum.Empty))
                {
                    Logger.Warning(Constants.Framework, $"Not good cash unit status to dispense item. {unit.Key}, {CashUnits[unit.Key].Unit.Status.Count}, {CashUnits[unit.Key].Status}" + nameof(IsDispensable));
                    return DispensableResultEnum.CashUnitError;
                }

                // No need to check amounts if CurrencyAmounts is not set
                if (CurrencyAmounts is not null)
                {
                    // Check currency
                    bool currencyOK = false;
                    foreach (var currency in CurrencyAmounts)
                    {
                        if (CashUnits[unit.Key].Unit.Configuration.Currency == currency.Key)
                        {
                            currencyOK = true;
                            break;
                        }
                    }

                    if (!currencyOK)
                    {
                        Logger.Warning(Constants.Framework, $"Specified currency ID not found to dispense. {unit.Key}, {CashUnits[unit.Key].Unit.Status.Count}, {CashUnits[unit.Key].Unit.Configuration.Currency}" + nameof(IsDispensable));
                        return DispensableResultEnum.InvalidCurrency;
                    }

                    internalAmount += CashUnits[unit.Key].Unit.Configuration.Value * unit.Value;
                }
            }

            double total = 0;
            if (CurrencyAmounts is not null)
                total = CurrencyAmounts.Select(c => c.Value).Sum();
            if (total != 0)
            {
                // If amount is zero, no need to check total amount of cash units
                bool invalidAmount = internalAmount != total;
                if (invalidAmount)
                {
                    Logger.Warning(Constants.Framework, $"Total amount to dispanse doesn't match with requested. Amount specified to dispense each units {internalAmount}, Amount to dispense {total} " + nameof(IsDispensable));
                    return DispensableResultEnum.InvalidDenomination;
                }
            }

            return DispensableResultEnum.Good;
        }

        /// <summary>
        /// Check given amount and total of cash unit denomination is equal.
        /// </summary>
        public bool CheckTotalAmount(Dictionary<string, CashUnitStorage> CashUnits)
        {
            if (CurrencyAmounts is null &&
                CashUnits is null)
            {
                // Not sure why this method is called
                return true;
            }

            if (CurrencyAmounts is null ||
                CashUnits is null)
            {
                return false;
            }

            double total = 0;
            foreach (var currency in CurrencyAmounts)
            {
                total += GetTotalAmount(currency.Key, Values, CashUnits);
            }

            return (total == CurrencyAmounts.Select(c =>c.Value).Sum());
        }

        /// <summary>
        /// Return total amount from given currency and denomination based on the cash unit information
        /// </summary>
        /// <param name="Currency"></param>
        /// <param name="Denom"></param>
        /// <param name="CashUnits"></param>
        /// <returns></returns>
        public static double GetTotalAmount(string Currency, Dictionary<string, int> Denom, Dictionary<string, CashUnitStorage> CashUnits)
        {
            if (Denom is null &&
                CashUnits is null)
            {
                return 0;
            }

            if (Denom is null ||
                CashUnits is null)
            {
                return 0;
            }

            double total = 0;
            foreach (var unit in CashUnits)
            {
                if (unit.Value.Unit.Configuration.Currency == Currency &&
                    Denom.ContainsKey(unit.Key))
                {
                    total += Denom[unit.Key] * unit.Value.Unit.Configuration.Value;
                }
            }

            return total;
        }
    }

    /// <summary>
    /// CountRequest
    /// Count operation and move notes to the specified position
    /// </summary>
    public sealed class CountRequest
    {
        /// <summary>
        /// CountRequest
        /// Count operation to perform
        /// </summary>
        /// <param name="Position">Cash to move</param>
        /// <param name="StorageUnitIds">Count from specified storage id</param>
        public CountRequest(CashDispenserCapabilitiesClass.OutputPositionEnum Position, List<string> StorageUnitIds)
        {
            this.Position = Position;
            this.StorageUnitIds = StorageUnitIds;
        }
        /// <summary>
        /// CountRequest
        /// Count cash from all physical units
        /// </summary>
        /// <param name="Position">Cash to move</param>
        public CountRequest(CashDispenserCapabilitiesClass.OutputPositionEnum Position)
        {
            this.Position = Position;
            this.StorageUnitIds = null;
        }

        /// <summary>
        /// Position of moving notes
        /// </summary>
        public CashDispenserCapabilitiesClass.OutputPositionEnum Position { get; init; }

        /// <summary>
        /// Specifies which storage to empty and count. This identifier reported by the storage GetStorage command.
        /// </summary>
        public List<string> StorageUnitIds { get; init; }
    }

    /// <summary>
    /// CountResult
    /// Return result of counting notes operation.
    /// </summary>
    public sealed class CountResult : DeviceResult
    {
        /// <summary>
        /// CountResult
        /// Return result of counting notes operation.
        /// </summary>
        public CountResult(MessagePayload.CompletionCodeEnum CompletionCode,
                           string ErrorDescription = null,
                           CountCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
                           Dictionary<string, CashUnitCountClass> MovementResult = null) 
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.MovementResult = MovementResult;
        }

        public CountResult(MessagePayload.CompletionCodeEnum CompletionCode,
                           Dictionary<string, CashUnitCountClass> MovementResult = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.MovementResult = MovementResult;
        }

        /// <summary>
        /// Specifies the error code on closing or opening shutter
        /// </summary>
        public CountCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Specifies the detailed note movement while in count operation.
        /// </summary>
        public Dictionary<string, CashUnitCountClass> MovementResult { get; init; }
    }

    /// <summary>
    /// PresentCashRequest
    /// Present operation to perform
    /// </summary>
    public sealed class PresentCashRequest
    {
        /// <summary>
        /// PresentCashRequest
        /// Present operation to perform
        /// </summary>
        /// <param name="Position">Output position where cash are presented.</param>
        public PresentCashRequest(CashDispenserCapabilitiesClass.OutputPositionEnum Position)
        {
            this.Position = Position;
        }

        /// <summary>
        /// Position of moving cash
        /// </summary>
        public CashDispenserCapabilitiesClass.OutputPositionEnum Position { get; init; }
    }

    /// <summary>
    /// PresentCashResult
    /// Return result of presenting cash operation
    /// </summary>
    public sealed class PresentCashResult : DeviceResult
    {
        /// <summary>
        /// PresentCashResult
        /// Return result of presenting cash operation
        /// </summary>
        /// If this value is specified the number of additional bunches of items remaining to be presented as a result of the current operation. 
        /// If the number of additional bunches is at least one, but the precise number is unknown, NumBunchesRemaining will be -1. 
        /// If there are no bunches remaining, set to zero
        public PresentCashResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                 string ErrorDescription = null,
                                 PresentCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
                                 int NumBunchesRemaining = 0,
                                 Dictionary<string, CashUnitCountClass> MovementResult = null) :
            base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.NumBunchesRemaining = NumBunchesRemaining;
            this.MovementResult = MovementResult;
        }
        public PresentCashResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                 int NumBunchesRemaining = 0,
                                 Dictionary<string, CashUnitCountClass> MovementResult = null) 
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.NumBunchesRemaining = NumBunchesRemaining;
            this.MovementResult = MovementResult;
        }

        /// <summary>
        /// Specifies the error code on presenting cash
        /// </summary>
        public PresentCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// If this value is specified the number of additional bunches of items remaining to be presented as a result of the current operation. 
        /// If the number of additional bunches is at least one, but the precise number is unknown, NumBunchesRemaining will be -1. 
        /// If there are no bunches remaining, set to zero
        /// </summary>
        public int NumBunchesRemaining { get; init; }

        /// <summary>
        /// Specifies the detailed note movement while in present operation.
        /// </summary>
        public Dictionary<string, CashUnitCountClass> MovementResult { get; init; }
    }

    /// <summary>
    /// RejectResult
    /// Return result of reject operation
    /// </summary>
    public sealed class RejectResult : DeviceResult
    {
        /// <summary>
        /// RejectResult
        /// Return result of reject operation
        /// </summary>
        public RejectResult(MessagePayload.CompletionCodeEnum CompletionCode,
                            string ErrorDescription = null,
                            RejectCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
                            Dictionary<string, CashUnitCountClass> MovementResult = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.MovementResult = MovementResult;
        }
        public RejectResult(MessagePayload.CompletionCodeEnum CompletionCode,
                            Dictionary<string, CashUnitCountClass> MovementResult = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.MovementResult = MovementResult;
        }

        /// <summary>
        /// Specifies the error code on reset device
        /// </summary>
        public RejectCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Specifies the detailed note movement while in reject operation.
        /// </summary>
        public Dictionary<string, CashUnitCountClass> MovementResult { get; init; }

    }

    /// <summary>
    /// PrepareDispenseRequest
    /// Prepare to get ready to dispense media before dispense operation
    /// </summary>
    public sealed class PrepareDispenseRequest
    {

        /// <summary>
        /// Start - Initiates the action to prepare for the next dispense operation. 
        /// Stop - Stops the previously activated dispense preparation.
        /// </summary>
        public enum ActionEnum
        {
            Start,
            Stop,
        }

        /// <summary>
        /// PrepareDispenseRequest
        /// Prepare to get ready to dispense media before dispense operation
        /// </summary>
        /// <param name="Action">Action to prepare dispense operation</param>
        public PrepareDispenseRequest(ActionEnum Action)
        {
            this.Action = Action;
        }

        public ActionEnum Action { get; init; }
    }

    /// <summary>
    /// PrepareDispenseResult
    /// Return result of preparation of the dispense operation.
    /// </summary>
    public sealed class PrepareDispenseResult : DeviceResult
    {
        public PrepareDispenseResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                     string ErrorDescription = null)
            : base(CompletionCode, ErrorDescription)
        {
        }
    }

    /// <summary>
    /// DenominateRequest
    /// The device class can find out count of items to be picked from the cash units
    /// </summary>
    public sealed class DenominateRequest
    {
        /// <summary>
        /// DenominateRequest
        /// </summary>
        /// <param name="CurrencyAmounts">Currency and amounts to mix</param>
        /// <param name="Values">Items picks from the cash units</param>
        public DenominateRequest(Dictionary<string, double> CurrencyAmounts,
                                 Dictionary<string, int> Values)
        {
            this.CurrencyAmounts = CurrencyAmounts;
            this.Values = Values;
        }

        public Dictionary<string, double> CurrencyAmounts { get; init; }

        public Dictionary<string, int> Values { get; init; }
    }

    /// <summary>
    /// DenominateResult
    /// Return result of denomination how many items to be picked from cash units
    /// </summary>
    public sealed class DenominateResult : DeviceResult
    {
        /// <summary>
        /// Constructor
        /// Return result of denomination how many items to be picked from cash units
        /// </summary>
        public DenominateResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                string ErrorDescription = null,
                                DenominateCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Values = null;
        }

        public DenominateResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                Dictionary<string, int> Values)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Values = Values;
        }

        public DenominateCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }


        public Dictionary<string, int> Values { get; init; }
    }

    /// <summary>
    /// DispenseRequest
    /// Perform dispensing operation
    /// </summary>
    public sealed class DispenseRequest
    {
        /// <summary>
        /// Constructor
        /// Perform dispensing operation
        /// </summary>
        public DispenseRequest(Dictionary<string, int> Values,
                               bool Present,
                               CashDispenserCapabilitiesClass.OutputPositionEnum? OutputPosition = null,
                               string E2EToken = null,
                               int? CashBox = null)
        {
            this.Values = Values;
            this.Present = Present;
            this.E2EToken = E2EToken;
            this.CashBox = CashBox;
            this.OutputPosition = OutputPosition;
        }
        public DispenseRequest(Dictionary<string, int> Values,
                               bool Present,
                               CashDispenserCapabilitiesClass.OutputPositionEnum OutputPosition,
                               string E2EToken = null)
        {
            this.Values = Values;
            this.Present = Present;
            this.E2EToken = E2EToken;
            CashBox = null;
            this.OutputPosition = OutputPosition;
        }

        public Dictionary<string, int> Values { get; init; }

        /// <summary>
        /// if this property is true, defines OutputPosition to which the items are to be dispensed.
        /// Otherwise, one of following location
        /// * teller position if the device is a Teller Dispenser
        /// * intermediate stacker if the device has one
        /// * the default position if there is no intermediate stacker.
        /// </summary>
        public bool Present { get; init; }
        /// <summary>
        /// Specify the output position item to be dispensed. if it is null, device class move items to one of following.
        /// - teller position if the device is a Teller Dispenser
        /// - intermediate stacker if the device has one
        /// - the default position if there is no intermediate stacker.
        /// </summary>
        public CashDispenserCapabilitiesClass.OutputPositionEnum? OutputPosition { get; init; }

        public int? CashBox { get; init; }

        public string E2EToken { get; init; }
    }

    /// <summary>
    /// DispenseResult
    /// Return result of dispensing operation
    /// </summary>
    public sealed class DispenseResult : DeviceResult
    {
        /// <summary>
        /// Constructor
        ///  Return result of dispensing operation
        /// </summary>
        public DispenseResult(MessagePayload.CompletionCodeEnum CompletionCode,
                              string ErrorDescription = null,
                              DispenseCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
                              Dictionary<string, int> Values = null,
                              int? CashBox = null,
                              Dictionary<string, CashUnitCountClass> MovementResult = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Values = Values;
            this.CashBox = CashBox;
            this.MovementResult = MovementResult;
        }

        public DispenseResult(MessagePayload.CompletionCodeEnum CompletionCode,
                              Dictionary<string, int> Values,
                              Dictionary<string, CashUnitCountClass> MovementResult = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Values = Values;
            this.CashBox = null;
            this.MovementResult = MovementResult;
        }

        public DispenseCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        public Dictionary<string, int> Values { get; init; }

        public int? CashBox { get; init; }

        /// <summary>
        /// Specifies the detailed note movement while in present operation.
        /// </summary>
        public Dictionary<string, CashUnitCountClass> MovementResult { get; init; }
    }

    /// <summary>
    /// TestCashUnitsRequest
    /// The parameter class for the test cash units operation
    /// </summary>
    public sealed class TestCashUnitsRequest
    {
        /// <summary>
        /// TestCashUnitsRequest
        /// The parameter class for the test cash units operation
        /// </summary>
        public TestCashUnitsRequest(ItemPosition Position)
        {
            this.Position = Position;
        }

        /// <summary>
        /// Specifies where the dispensed items should be moved to.
        /// If tnis value is null, the retract items to the default position.
        /// </summary>
        public ItemPosition Position { get; init; }
    }

    /// <summary>
    /// TestCashUnitsResult
    /// Return result of the test cash units operation
    /// </summary>
    public sealed class TestCashUnitsResult : DeviceResult
    {
        /// <summary>
        /// TestCashUnitsResult
        /// Return result of the test cash units operation
        /// </summary>
        public TestCashUnitsResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                   string ErrorDescription = null,
                                   TestCashUnitsCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
                                   Dictionary<string, CashUnitCountClass> MovementResult = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.MovementResult = MovementResult;
        }

        public TestCashUnitsResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                   Dictionary<string, CashUnitCountClass> MovementResult = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.MovementResult = MovementResult;
        }

        /// <summary>
        /// Specifies the error code on reset device
        /// </summary>
        public TestCashUnitsCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Specifies the detailed note movement while in reset operation.
        /// </summary>
        public Dictionary<string, CashUnitCountClass> MovementResult { get; init; }
    }
}