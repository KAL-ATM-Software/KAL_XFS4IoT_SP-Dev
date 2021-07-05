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
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.CashManagement
{
    /// <summary>
    /// This field is used if items are to be moved to internal areas of the device, including cash units, 
    /// the intermediate stacker, or the transport.
    /// </summary>
    public class Retract
    {
        public CashDispenserCapabilitiesClass.RetractAreaEnum RetractArea { get; init; }

        /// <summary>
        /// Index is valid if the RetractArea is set to Retract, otherwise this value can be omitted
        /// </summary>
        public int? Index { get; init; }

        public Retract(CashDispenserCapabilitiesClass.RetractAreaEnum RetractArea,
                       int? Index = null)
        {
            this.RetractArea = RetractArea;
            this.Index = Index;
        }
    }

    public sealed class ItemPosition
    {
        /// <summary>
        /// ItemPosition
        /// Specifies where the dispensed items should be moved to
        /// </summary>
         public ItemPosition(string CashUnit)
        {
            this.CashUnit = CashUnit;
            this.RetractArea = null;
            this.OutputPosition = null;
        }
        public ItemPosition(Retract RetractArea)
        {
            this.CashUnit = string.Empty;
            this.RetractArea = RetractArea;
            this.OutputPosition = null;
        }
        public ItemPosition(CashDispenserCapabilitiesClass.OutputPositionEnum? OutputPosition)
        {
            this.CashUnit = string.Empty;
            this.RetractArea = null;
            this.OutputPosition = OutputPosition;
        }

        /// <summary>
        /// This value specifies the name of the single cash unit to be used for the storage of any items found.
        /// </summary>
        public string CashUnit { get; init; }
        /// <summary>
        /// This field is used if items are to be moved to internal areas of the device, including cash units, 
        /// the intermediate stacker, or the transport.
        /// </summary>
        public Retract RetractArea { get; init; }
        /// <summary>
        /// The output position to which items are to be moved if the RetractArea is specified to OutputPosition.
        /// Following values are possible:
        /// 
        /// * ```default``` - The default configuration.
        /// * ```left``` - The left output position.
        /// * ```right``` - The right output position.
        /// * ```center``` - The center output position.
        /// * ```top``` - The top output position.
        /// * ```bottom``` - The bottom output position.
        /// * ```front``` - The front output position.
        /// * ```rear``` - The rear output position.
        /// </summary>
        public CashDispenserCapabilitiesClass.OutputPositionEnum? OutputPosition { get; init; }
    }

    public sealed class InitiateExchangeRequest
    {
        public InitiateExchangeRequest(List<string> CashUnits)
        {
            this.CashUnits = CashUnits;
        }

        /// <summary>
        /// Array of strings containing the object names of the cash units to be exchanged
        /// </summary>
        public List<string> CashUnits { get; init; }
    }

    public sealed class InitiateClearRecyclerRequest
    {
        public InitiateClearRecyclerRequest(string CashUnit,
                                            CashDispenserCapabilitiesClass.OutputPositionEnum Position,
                                            string TargetCashUnit)
        {
            this.CashUnit = CashUnit;
            this.Position = Position;
            this.TargetCashUnit = TargetCashUnit;
        }

        /// <summary>
        /// Cash Unit name of recycle cash unit to be emptied
        /// </summary>
        public string CashUnit { get; init; }

        /// <summary>
        /// Determines to which position the cash should be moved
        /// </summary>
        public CashDispenserCapabilitiesClass.OutputPositionEnum Position { get; init; }

        /// <summary>
        /// Object name of the cash unit the items are to be moved
        /// </summary>
        public string TargetCashUnit { get; init; }
    }

    public sealed class InitiateExchangeResult : DeviceResult
    {
        public InitiateExchangeResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                      string ErrorDescription = null,
                                      StartExchangeCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.CashUnits = null;
        }

        public InitiateExchangeResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                      List<string> CashUnits)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.CashUnits = CashUnits;
        }

        /// <summary>
        /// Specifies the error code on start exchange
        /// </summary>
        public StartExchangeCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Cash Unit name of cash units to be exchanged.
        /// </summary>
        public List<string> CashUnits { get; init; }

    }

    public sealed class CompleteExchangeRequest
    {
        public CompleteExchangeRequest()
        {
        }
    }

    public sealed class CompleteExchangeResult : DeviceResult
    {
        public CompleteExchangeResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                      string ErrorDescription = null,
                                      EndExchangeCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary>
        /// Specifies the error code on end exchange
        /// </summary>
        public EndExchangeCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }

    /// <summary>
    /// UnlockSafeResult
    /// Request for unlocking safe door.
    /// </summary>
    public sealed class UnlockSafeResult : DeviceResult
    {
        public UnlockSafeResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                      string ErrorDescription = null,
                                      OpenSafeDoorCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        { }

        /// <summary>
        /// Specifies the error code on end exchange
        /// </summary>
        public OpenSafeDoorCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }

    /// <summary>
    /// CalibrateCashUnitRequest
    /// Request to perform calibration of cash unit
    /// </summary>
    public sealed class CalibrateCashUnitRequest
    {
        public CalibrateCashUnitRequest(string CashUnit,
                                        int NumOfBills,
                                        ItemPosition Position)
        {
            this.CashUnit = CashUnit;
            this.NumOfBills = NumOfBills;
            this.Position = Position;
        }

        /// <summary>
        /// The object name of the cash unit where items to be dispensed
        /// </summary>
        public string CashUnit { get; init; }

        /// <summary>
        /// The number of bills to be dispensed during the calibration process.
        /// </summary>
        public int NumOfBills { get; init; }

        /// <summary>
        /// Specifies where the dispensed items should be moved to
        /// </summary>
        public ItemPosition Position { get; init; }
    }

    /// <summary>
    /// CalibrateCashUnitResult
    /// Return result of calibration for cash unit
    /// </summary>
    public sealed class CalibrateCashUnitResult : DeviceResult
    {
        /// <summary>
        /// CalibrateCashUnitResult
        /// Return result of calibration for cash unit
        /// </summary>
        /// <param name="CompletionCode"></param>
        /// <param name="ErrorDescription"></param>
        /// <param name="ErrorCode"></param>
        public CalibrateCashUnitResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                       string ErrorDescription = null,
                                       CalibrateCashUnitCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Position = null;
            this.MovementResult = null;
        }

        public CalibrateCashUnitResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                       ItemPosition Position,
                                       Dictionary<string, ItemMovement> MovementResult = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Position = Position;
            this.MovementResult = MovementResult;
        }

        /// <summary>
        /// Specifies the error code on calibration
        /// </summary>
        public CalibrateCashUnitCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }


        /// <summary>
        /// Specifies where the dispensed items should be moved to
        /// </summary>
        public ItemPosition Position { get; init; }

        /// <summary>
        /// Specifies the detailed note movement while in calibration.
        /// </summary>
        public Dictionary<string, ItemMovement> MovementResult { get; init; }
    }

    /// <summary>
    /// SetCashUnitInfoRequest
    /// Request to set new counts or changing cash unit configuration
    /// </summary>
    public sealed class SetCashUnitInfoRequest
    {
        /// <summary>
        /// CashUnitAccounting
        /// The object contains update cash unit accounting requested by the application
        /// </summary>
        public sealed class SetCashUnitAccounting
        {
            public SetCashUnitAccounting(int? LogicalCount = null,
                                         int? InitialCount = null,
                                         int? DispensedCount = null,
                                         int? PresentedCount = null,
                                         int? RetractedCount = null,
                                         int? RejectCount = null,
                                         int? Count = null,
                                         int? CashInCount = null,
                                         List<BankNoteNumber> BankNoteNumberList = null)
            {
                this.LogicalCount = LogicalCount;
                this.InitialCount = InitialCount;
                this.DispensedCount = DispensedCount;
                this.PresentedCount = PresentedCount;
                this.RetractedCount = RetractedCount;
                this.RejectCount = RejectCount;
                this.Count = Count;
                this.CashInCount = CashInCount;
                this.BankNoteNumberList = BankNoteNumberList;
            }

            /// <summary>
            /// The meaning of this count depends on the type of cash unit. This value is persistent.
            /// For all cash units except retract cash units (*type* is not *retractCassette*) this value specifies 
            /// the number of items inside the  cash unit.
            /// For all dispensing cash units (*type* is *billCassette*, *coinCylinder*, 
            /// *coinDispenser*, *coupon*, *document* or *recycling*), 
            /// this value includes any items from the cash unit not yet presented to the customer. 
            /// This count is only decremented when the items are either known to be in customer access or successfully rejected.
            /// If the cash unit is usable from the CashAcceptor interface (*type* is *recycling*, *cashIn*, *retractCassette* 
            /// or *rejectCassette*) then this value will be incremented as a result of a cash-in operation.
            /// Note that for a reject cash unit (*type* is *rejectCassette*), this value is unreliable, since 
            /// the typical reason for dumping items to the reject cash unit is a suspected count failure.
            /// For a retract cash unit (*type* is *retractCassette*) this value specifies the number 
            /// of retract operations which result in items entering the cash unit.
            /// </summary>
            public int? LogicalCount { get; init; }

            /// <summary>
            /// Initial number of items contained in the cash unit. This value is persistent.
            /// </summary>
            public int? InitialCount { get; init; }

            /// <summary>
            /// The number of items dispensed from this cash unit. 
            /// This count is incremented when the items are removed from the cash units. 
            /// This count includes any items that were rejected during the dispense operation and are no longer in this cash unit. 
            /// This field is always zero for cash units with a *type* of *rejectCassette* or *retractCassette*. This value is persistent.
            /// </summary>
            public int? DispensedCount { get; init; }

            /// <summary>
            /// The number of items from this cash unit that have been presented to the customer. 
            /// This count is incremented when the items are presented to the customer.
            /// If it is unknown if a customer has been presented with the items, then this count is not updated. 
            /// This field is always zero for cash units with a *type* of *rejectCassette* or *retractCassette*. This value is persistent.
            /// </summary>
            public int? PresentedCount { get; init; }

            /// <summary>
            /// The number of items that have been accessible to a customer and retracted into the 
            /// cash unit. This value is persistent.
            /// </summary>
            public int? RetractedCount { get; init; }

            /// <summary>
            /// The number of items dispensed from this cash unit which have been rejected, are in a cash unit 
            /// other than this cash unit, and which have not been accessible to a customer. This value may be unreliable, 
            /// since a typical reason for rejecting items is a suspected pick failure. Other reasons for rejecting items 
            /// may include incorrect note denominations, classifications not valid for dispensing, or where the transaction 
            /// has been cancelled and a Reject command has been called. For reject and retract cash units 
            /// (*type* is *rejectCassette* or *retractCassette*) this field does not apply and will be reported as zero. This value is persistent.
            /// </summary>
            public int? RejectCount { get; init; }

            /// <summary>
            /// As defined by the *logicalCount* description, but with the following exceptions:
            /// This count does not include items dispensed but not yet presented.
            /// On cash units with *type* set to \"retractCassette\" the count represents 
            /// the number of items, unless the device cannot count items during a retract, in which case this count will be zero.
            /// This value is persistent.
            /// </summary>
            public int? Count { get; init; }

            /// <summary>
            /// Count of items that have entered the cash unit. This counter is incremented whenever an item 
            /// enters a cash unit for any reason, unless it originated 
            /// from this cash unit but was returned without being accessible to a customer. For a retract cash unit this 
            /// value represents the total number of items of all types in the cash unit, or if the device cannot count 
            /// items during a retract operation this value will be zero. This value is persistent.
            /// </summary>
            public int? CashInCount { get; init; }

            /// <summary>
            /// Array of banknote numbers the cash unit contains.
            /// Include all acceptable banknote types and counts here
            /// </summary>
            public List<BankNoteNumber> BankNoteNumberList { get; init; }
        }

        /// <summary>
        /// Cash Unit configuration changes requested by the client application
        /// </summary>
        public sealed class SetCashUnitConfiguration
        {
            public SetCashUnitConfiguration(CashUnit.TypeEnum? Type = null,
                                            string CurrencyID = null,
                                            double? Value = null,
                                            int? Maximum = null,
                                            bool? AppLock = null,
                                            string CashUnitName = null,
                                            int? Minimum = null,
                                            string PhysicalPositionName = null,
                                            string UnitID = null,
                                            int? MaximumCapacity = null,
                                            bool? HardwareSensor = null,
                                            CashUnit.ItemTypesEnum? ItemTypes = null,
                                            List<int> BanknoteIDs = null)
            {
                this.Type = Type;
                this.CurrencyID = CurrencyID;
                this.Value = Value;
                this.Maximum = Maximum;
                this.AppLock = AppLock;
                this.CashUnitName = CashUnitName;
                this.Minimum = Minimum;
                this.PhysicalPositionName = PhysicalPositionName;
                this.UnitID = UnitID;
                this.MaximumCapacity = MaximumCapacity;
                this.HardwareSensor = HardwareSensor;
                this.ItemTypes = ItemTypes;
                this.BanknoteIDs = BanknoteIDs;
            }

            /// <summary>
            /// Type of cash unit. 
            /// Following values are possible:
            /// 
            /// * ```notApplicable``` - Not applicable. Typically means cash unit is missing.
            /// * ```rejectCassette``` - Reject cash unit. This type will also indicate a combined reject/retract cash unit.
            /// * ```billCassette``` - Cash unit containing bills.
            /// * ```coinCylinder``` - Coin cylinder.
            /// * ```coinDispenser``` - Coin dispenser as a whole unit.
            /// * ```retractCassette``` - Retract cash unit.
            /// * ```coupon``` - Cash unit containing coupons or advertising material.
            /// * ```document``` - Cash unit containing documents.
            /// * ```replenishmentContainer``` - Replenishment container. A cash unit can be refilled from a replenishment container.
            /// * ```recycling``` - Recycling cash unit. This unit is only present when the device implements the Dispenser and CashAcceptor interfaces.
            /// * ```cashIn``` - Cash-in cash unit.
            /// </summary>
            public CashUnit.TypeEnum? Type { get; init; }

            /// <summary>
            /// A three character string storing the ISO format [Ref. 2] Currency ID. This value will be omitted for 
            /// cash units which contain items of more than one currency type or items to which currency is not applicable. 
            /// If the *status* field for this cash unit is *noValue* it is the responsibility of the application to assign 
            /// a value to this field. This value is persistent.
            /// </summary>
            public string CurrencyID { get; init; }

            /// <summary>
            /// Supplies the value of a single item in the cash unit. This value is expressed as floating point value.
            /// If the *currencyID* field for this cash unit is omitted, then this 
            /// field will contain zero. If the *status* field for this cash unit is *noValue* it is the responsibility of the 
            /// application to assign a value to this field. This value is persistent.
            /// </summary>
            public double? Value { get; init; }

            /// <summary>
            /// When *count* reaches this value the 
            /// threshold event CashManagement.CashUnitThresholdEvent (*high*) will be generated. This value can be different from
            /// the actual capacity of the cassette. 
            /// If this value is non-zero then hardware sensors in the device do not trigger threshold events. If this value is zero 
            /// then hardware sensors will trigger threshold events if *hardwareSensor* is TRUE. This value is persistent.
            /// </summary>
            public int? Maximum { get; init; }

            /// <summary>
            /// If this value is TRUE items cannot be dispensed from or deposited into the cash unit. 
            /// If this value is TRUE and the application attempts to use the cash unit a CashManagement.CashUnitErrorEvent 
            /// event will be generated and an error completion message will be returned. This value is persistent.
            /// </summary>
            public bool? AppLock { get; init; }

            /// <summary>
            /// A name which helps to identify the type of the cash unit. 
            /// This is especially useful in the case of cash units of type *document* where different 
            /// documents can have the same currency and value. For example, travelers checks and bank 
            /// checks may have the same currency and value but still need to be identifiable as different 
            /// types of document. Where this value is not relevant (e.g. in bill cash units) the property can be omitted. This value is persistent.
            /// </summary>
            public string CashUnitName { get; init; }

            /// <summary>
            /// This field is not applicable to retract and reject cash units. For all cash units which dispense items (all other), when *count*
            /// reaches this value the threshold event CashManagement.CashUnitThresholdEvent (*low*) will be generated. 
            /// If this value is non-zero then hardware sensors in the device do not trigger threshold events. 
            /// If this value is zero then hardware sensors will trigger threshold events if *hardwareSensor* is TRUE. This value is persistent.
            /// </summary>
            public int? Minimum { get; init; }

            /// <summary>
            /// A name identifying the physical location of the cash unit.
            /// </summary>
            public string PhysicalPositionName { get; init; }

            /// <summary>
            /// A 5 character string uniquely identifying the cash unit.
            /// </summary>
            public string UnitID { get; init; }

            /// <summary>
            /// The maximum number of items the cash unit can hold. This is only for informational purposes. 
            /// No threshold event CashManagement.CashUnitThresholdEvent will be generated. This value is persistent.
            /// </summary>
            public int? MaximumCapacity { get; init; }

            /// <summary>
            /// Specifies whether or not threshold events can be generated based on hardware sensors in the device. 
            /// If this value is TRUE then threshold 
            /// events may be generated based on hardware sensors as opposed to counts.
            /// </summary>
            public bool? HardwareSensor { get; init; }

            /// <summary>
            /// Specifies the type of items the cash unit takes as a combination of the following flags. 
            /// The table in the Comments section of this command defines how to interpret the combination of these flags 
            /// </summary>
            public CashUnit.ItemTypesEnum? ItemTypes { get; init; }

            /// <summary>
            /// List of banknote IDs can be stored in this unit
            /// </summary>
            public List<int> BanknoteIDs { get; init; }
        }

        public SetCashUnitInfoRequest(Dictionary<string, SetCashUnitConfiguration> CashUnitConfigurations,
                                      Dictionary<string, SetCashUnitAccounting> CashUnitAccountings)
        {
            this.CashUnitConfigurations = CashUnitConfigurations;
            this.CashUnitAccountings = CashUnitAccountings;
        }

        /// <summary>
        /// The object contains the cash unit configuration information to be updated.
        /// Key name must be used in output dictionary for the ConstructCashUnits
        /// </summary>
        public Dictionary<string, SetCashUnitConfiguration> CashUnitConfigurations { get; init; }

        /// <summary>
        /// The number of bills to be dispensed during the calibration process.
        /// Key name must be used in output dictionary for the ConstructCashUnits.
        /// </summary>
        public Dictionary<string, SetCashUnitAccounting> CashUnitAccountings { get; init; }
    }

    /// <summary>
    /// SetCashUnitInfoResult
    /// Return result of setting new cash unit configuration or counts
    /// </summary>
    public sealed class SetCashUnitInfoResult : DeviceResult
    {
        /// <summary>
        /// SetCashUnitInfoResult
        /// Return result of setting new cash unit configuration or counts
        /// </summary>
        public SetCashUnitInfoResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                     string ErrorDescription = null,
                                     SetCashUnitInfoCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary>
        /// Specifies the error code on setting new cash unit information or counts
        /// </summary>
        public SetCashUnitInfoCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }
}
