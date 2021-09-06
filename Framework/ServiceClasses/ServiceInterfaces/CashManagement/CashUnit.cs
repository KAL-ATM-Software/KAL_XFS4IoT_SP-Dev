/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.CashManagement
{
    /// <summary>
    /// Cash Unit strcuture the device class supports
    /// </summary>
    [Serializable()]
    public sealed record CashUnit
    {
        [Flags]
        public enum ItemTypesEnum
        {
            All = 0x0001,
            Unfit = 0x0002,
            Individual = 0x0004,
            Level1 = 0x0008,
            Level2 = 0x0010,
            Level3 = 0x0040,
            ItemProcessor = 0x0080,
            UnfitIndividual = 0x0100,
        }

        public enum StatusEnum
        {
            Ok,
            Full,
            High,
            Low,
            Empty,
            Inoperative,
            Missing,
            NoValue,
            NoReference,
            Manipulated
        }

        public enum TypeEnum
        {
            BillCassette,
            NotApplicable,
            RejectCassette,
            CoinCylinder,
            CoinDispenser,
            RetractCassette,
            Coupon,
            Document,
            ReplenishmentContainer,
            Recycling,
            CashIn
        }

        public CashUnit(StatusEnum Status,
                        TypeEnum Type,
                        string CurrencyID,
                        double Value,
                        int LogicalCount,
                        int Maximum,
                        bool AppLock,
                        string CashUnitName,
                        int InitialCount,
                        int DispensedCount,
                        int PresentedCount,
                        int RetractedCount,
                        int RejectCount,
                        int Minimum,
                        string PhysicalPositionName,
                        string UnitID,
                        int Count,
                        int MaximumCapacity,
                        bool HardwareSensor,
                        ItemTypesEnum ItemTypes,
                        int CashInCount,
                        List<BankNoteNumber> BankNoteNumberList,
                        List<int> BanknoteIDs)
        {
            this.Status = Status;
            this.Type = Type;
            this.CurrencyID = CurrencyID;
            this.Value = Value;
            this.LogicalCount = LogicalCount;
            this.Maximum = Maximum;
            this.AppLock = AppLock;
            this.CashUnitName = CashUnitName;
            this.InitialCount = InitialCount;
            this.DispensedCount = DispensedCount;
            this.PresentedCount = PresentedCount;
            this.RetractedCount = RetractedCount;
            this.RejectCount = RejectCount;
            this.Minimum = Minimum;
            this.PhysicalPositionName = PhysicalPositionName;
            this.UnitID = UnitID;
            this.Count = Count;
            this.MaximumCapacity = MaximumCapacity;
            this.HardwareSensor = HardwareSensor;
            this.ItemTypes = ItemTypes;
            this.CashInCount = CashInCount;
            this.BankNoteNumberList = BankNoteNumberList;
            this.BanknoteIDs = BanknoteIDs;
        }

        public CashUnit()
        {
            this.Status = StatusEnum.NoValue;
            this.Type = TypeEnum.NotApplicable;
            this.CurrencyID = string.Empty;
            this.Value = double.MinValue;
            this.LogicalCount = int.MinValue;
            this.Maximum = int.MinValue;
            this.AppLock = false;
            this.CashUnitName = string.Empty;
            this.InitialCount = int.MinValue;
            this.DispensedCount = int.MinValue;
            this.PresentedCount = int.MinValue;
            this.RetractedCount = int.MinValue;
            this.RejectCount = int.MinValue;
            this.Minimum = int.MinValue;
            this.PhysicalPositionName = string.Empty;
            this.UnitID = string.Empty;
            this.Count = int.MinValue;
            this.MaximumCapacity = int.MinValue;
            this.HardwareSensor = true;
            this.ItemTypes = ItemTypesEnum.All;
            this.CashInCount = int.MinValue;
            this.BankNoteNumberList = new List<BankNoteNumber>();
            this.BanknoteIDs = new List<int>();
        }

        public CashUnit(CashUnitConfiguration Unit)
        {
            this.Status = StatusEnum.NoValue;
            this.Type = Unit.Type;
            this.CurrencyID = Unit.CurrencyID;
            this.Value = Unit.Value;
            this.LogicalCount = 0;
            this.Maximum = Unit.Maximum;
            this.AppLock = Unit.AppLock;
            this.CashUnitName = Unit.CashUnitName;
            this.InitialCount = 0;
            this.DispensedCount = 0;
            this.PresentedCount = 0;
            this.RetractedCount = 0;
            this.RejectCount = 0;
            this.Minimum = Unit.Minimum;
            this.PhysicalPositionName = Unit.PhysicalPositionName;
            this.UnitID = Unit.UnitID;
            this.Count = 0;
            this.MaximumCapacity = Unit.MaximumCapacity;
            this.HardwareSensor = Unit.HardwareSensor;
            this.ItemTypes = Unit.ItemTypes;
            this.CashInCount = 0;
            this.BankNoteNumberList = new List<BankNoteNumber>();
            this.BanknoteIDs = Unit.BanknoteIDs;
        }

        /// <summary>
        /// Supplies the status of the cash unit.
        /// Following values are possible:
        /// 
        /// * ```ok``` - The cash unit is in a good state.
        /// * ```full``` - The cash unit is full.
        /// * ```high``` - The cash unit is almost full (i.e. reached or exceeded the threshold defined by *maximum*). 
        /// * ```low``` - The cash unit is almost empty (i.e. reached or below the threshold defined by *minimum*). 
        /// * ```empty``` - The cash unit is empty, or insufficient items in the cash unit are preventing further dispense operations.
        /// * ```inoperative``` - The cash unit is inoperative.
        /// * ```missing``` - The cash unit is missing.
        /// * ```noValue``` - The values of the specified cash unit are not available.
        /// * ```noReference``` - There is no reference value available for the notes in this cash unit. The cash unit has not been calibrated.
        /// * ```manipulated``` - The cash unit has been inserted (including removal followed by a reinsertion) when the device 
        /// was not in the exchange state. This cash unit cannot be dispensed from.
        /// </summary>
        public StatusEnum Status { get; set; }

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
        public TypeEnum Type { get; private set; }

        /// <summary>
        /// A three character string storing the ISO format [Ref. 2] Currency ID. This value will be omitted for 
        /// cash units which contain items of more than one currency type or items to which currency is not applicable. 
        /// If the *status* field for this cash unit is *noValue* it is the responsibility of the application to assign 
        /// a value to this field. This value is persistent.
        /// </summary>
        public string CurrencyID { get; private set; }

        /// <summary>
        /// Supplies the value of a single item in the cash unit. This value is expressed as floating point value.
        /// If the *currencyID* field for this cash unit is omitted, then this 
        /// field will contain zero. If the *status* field for this cash unit is *noValue* it is the responsibility of the 
        /// application to assign a value to this field. This value is persistent.
        /// </summary>
        public double Value { get; private set; }

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
        public int LogicalCount { get; set; }

        /// <summary>
        /// When *count* reaches this value the 
        /// threshold event CashManagement.CashUnitThresholdEvent (*high*) will be generated. This value can be different from
        /// the actual capacity of the cassette. 
        /// If this value is non-zero then hardware sensors in the device do not trigger threshold events. If this value is zero 
        /// then hardware sensors will trigger threshold events if *hardwareSensor* is TRUE. This value is persistent.
        /// </summary>
        public int Maximum { get; private set; }

        /// <summary>
        /// If this value is TRUE items cannot be dispensed from or deposited into the cash unit. 
        /// If this value is TRUE and the application attempts to use the cash unit a CashManagement.CashUnitErrorEvent 
        /// event will be generated and an error completion message will be returned. This value is persistent.
        /// </summary>
        public bool AppLock { get; private set; }

        /// <summary>
        /// A name which helps to identify the type of the cash unit. 
        /// This is especially useful in the case of cash units of type *document* where different 
        /// documents can have the same currency and value. For example, travelers checks and bank 
        /// checks may have the same currency and value but still need to be identifiable as different 
        /// types of document. Where this value is not relevant (e.g. in bill cash units) the property can be omitted. This value is persistent.
        /// </summary>
        public string CashUnitName { get; private set; }

        /// <summary>
        /// Initial number of items contained in the cash unit. This value is persistent.
        /// </summary>
        public int InitialCount { get; set; }

        /// <summary>
        /// The number of items dispensed from this cash unit. 
        /// This count is incremented when the items are removed from the cash units. 
        /// This count includes any items that were rejected during the dispense operation and are no longer in this cash unit. 
        /// This field is always zero for cash units with a *type* of *rejectCassette* or *retractCassette*. This value is persistent.
        /// </summary>
        public int DispensedCount { get; set; }

        /// <summary>
        /// The number of items from this cash unit that have been presented to the customer. 
        /// This count is incremented when the items are presented to the customer.
        /// If it is unknown if a customer has been presented with the items, then this count is not updated. 
        /// This field is always zero for cash units with a *type* of *rejectCassette* or *retractCassette*. This value is persistent.
        /// </summary>
        public int PresentedCount { get; set; }

        /// <summary>
        /// The number of items that have been accessible to a customer and retracted into the 
        /// cash unit. This value is persistent.
        /// </summary>
        public int RetractedCount { get; set; }

        /// <summary>
        /// The number of items dispensed from this cash unit which have been rejected, are in a cash unit 
        /// other than this cash unit, and which have not been accessible to a customer. This value may be unreliable, 
        /// since a typical reason for rejecting items is a suspected pick failure. Other reasons for rejecting items 
        /// may include incorrect note denominations, classifications not valid for dispensing, or where the transaction 
        /// has been cancelled and a Reject command has been called. For reject and retract cash units 
        /// (*type* is *rejectCassette* or *retractCassette*) this field does not apply and will be reported as zero. This value is persistent.
        /// </summary>
        public int RejectCount { get; set; }

        /// <summary>
        /// This field is not applicable to retract and reject cash units. For all cash units which dispense items (all other), when *count*
        /// reaches this value the threshold event CashManagement.CashUnitThresholdEvent (*low*) will be generated. 
        /// If this value is non-zero then hardware sensors in the device do not trigger threshold events. 
        /// If this value is zero then hardware sensors will trigger threshold events if *hardwareSensor* is TRUE. This value is persistent.
        /// </summary>
        public int Minimum { get; private set; }

        /// <summary>
        /// A name identifying the physical location of the cash unit.
        /// </summary>
        public string PhysicalPositionName { get; private set; }

        /// <summary>
        /// A 5 character string uniquely identifying the cash unit.
        /// </summary>
        public string UnitID { get; private set; }

        /// <summary>
        /// As defined by the *logicalCount* description, but with the following exceptions:
        /// This count does not include items dispensed but not yet presented.
        /// On cash units with *type* set to \"retractCassette\" the count represents 
        /// the number of items, unless the device cannot count items during a retract, in which case this count will be zero.
        /// This value is persistent.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// The maximum number of items the cash unit can hold. This is only for informational purposes. 
        /// No threshold event CashManagement.CashUnitThresholdEvent will be generated. This value is persistent.
        /// </summary>
        public int MaximumCapacity { get; private set; }

        /// <summary>
        /// Specifies whether or not threshold events can be generated based on hardware sensors in the device. 
        /// If this value is TRUE then threshold 
        /// events may be generated based on hardware sensors as opposed to counts.
        /// </summary>
        public bool HardwareSensor { get; private set; }

        /// <summary>
        /// Specifies the type of items the cash unit takes as a combination of the following flags. 
        /// The table in the Comments section of this command defines how to interpret the combination of these flags 
        /// </summary>
        public ItemTypesEnum ItemTypes { get; private set; }

        /// <summary>
        /// Count of items that have entered the cash unit. This counter is incremented whenever an item 
        /// enters a cash unit for any reason, unless it originated 
        /// from this cash unit but was returned without being accessible to a customer. For a retract cash unit this 
        /// value represents the total number of items of all types in the cash unit, or if the device cannot count 
        /// items during a retract operation this value will be zero. This value is persistent.
        /// </summary>
        public int CashInCount { get; set; }

        /// <summary>
        /// Array of banknote numbers the cash unit contains.
        /// Include all acceptable banknote types and counts here
        /// </summary>
        public List<BankNoteNumber> BankNoteNumberList { get; set; }

        /// <summary>
        /// List of banknote IDs can be stored in this unit
        /// </summary>
        public List<int> BanknoteIDs { get; private set; }

        /// <summary>
        /// Return or set cash unit structure data
        /// </summary>
        public CashUnitConfiguration Configuration
        {
            get
            {
                return new CashUnitConfiguration(this.Type,
                                                 this.CurrencyID,
                                                 this.Value,
                                                 this.Maximum,
                                                 this.AppLock,
                                                 this.CashUnitName,
                                                 this.Minimum,
                                                 this.PhysicalPositionName,
                                                 this.UnitID,
                                                 this.MaximumCapacity,
                                                 this.HardwareSensor,
                                                 this.ItemTypes,
                                                 this.BanknoteIDs);
            }

            set
            {
                this.Type = value.Type;
                this.CurrencyID = value.CurrencyID;
                this.Value = value.Value;
                this.Maximum = value.Maximum;
                this.AppLock = value.AppLock;
                this.CashUnitName = value.CashUnitName;
                this.Minimum = value.Minimum;
                this.PhysicalPositionName = value.PhysicalPositionName;
                this.UnitID = value.UnitID;
                this.MaximumCapacity = value.MaximumCapacity;
                this.HardwareSensor = value.HardwareSensor;
                this.ItemTypes = value.ItemTypes;
                this.BanknoteIDs = value.BanknoteIDs;
            }
        }

        /// <summary>
        /// Return or set various counters
        /// </summary>
        public CashUnitAccounting Accounting
        {
            get
            {
                return new CashUnitAccounting(this.LogicalCount,
                                              this.InitialCount,
                                              this.DispensedCount,
                                              this.PresentedCount,
                                              this.RetractedCount,
                                              this.RejectCount,
                                              this.Count,
                                              this.CashInCount,
                                              this.BankNoteNumberList);
            }

            set
            {
                this.LogicalCount = value.LogicalCount;
                this.InitialCount = value.InitialCount;
                this.DispensedCount = value.DispensedCount;
                this.PresentedCount = value.PresentedCount;
                this.RetractedCount = value.RetractedCount;
                this.RejectCount = value.RejectCount;
                this.Count = value.Count;
                this.CashInCount = value.CashInCount;
                this.BankNoteNumberList = value.BankNoteNumberList;
            }
        }
    }

    /// <summary>
    /// CashUnitAccounting
    /// The device specific class update counts after device operation is completed
    /// </summary>
    public sealed record CashUnitAccounting
    {
        public CashUnitAccounting()
        {
            this.LogicalCount = 0;
            this.InitialCount = 0;
            this.DispensedCount = 0;
            this.PresentedCount = 0;
            this.RetractedCount = 0;
            this.RejectCount = 0;
            this.Count = 0;
            this.CashInCount = 0;
            this.BankNoteNumberList = new List<BankNoteNumber>();
        }

        public CashUnitAccounting(int LogicalCount,
                                  int InitialCount,
                                  int DispensedCount,
                                  int PresentedCount,
                                  int RetractedCount,
                                  int RejectCount,
                                  int Count,
                                  int CashInCount,
                                  List<BankNoteNumber> BankNoteNumberList) 
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
        public int LogicalCount { get; set; }

        /// <summary>
        /// Initial number of items contained in the cash unit. This value is persistent.
        /// </summary>
        public int InitialCount { get; set; }

        /// <summary>
        /// The number of items dispensed from this cash unit. 
        /// This count is incremented when the items are removed from the cash units. 
        /// This count includes any items that were rejected during the dispense operation and are no longer in this cash unit. 
        /// This field is always zero for cash units with a *type* of *rejectCassette* or *retractCassette*. This value is persistent.
        /// </summary>
        public int DispensedCount { get; set; }

        /// <summary>
        /// The number of items from this cash unit that have been presented to the customer. 
        /// This count is incremented when the items are presented to the customer.
        /// If it is unknown if a customer has been presented with the items, then this count is not updated. 
        /// This field is always zero for cash units with a *type* of *rejectCassette* or *retractCassette*. This value is persistent.
        /// </summary>
        public int PresentedCount { get; set; }

        /// <summary>
        /// The number of items that have been accessible to a customer and retracted into the 
        /// cash unit. This value is persistent.
        /// </summary>
        public int RetractedCount { get; set; }

        /// <summary>
        /// The number of items dispensed from this cash unit which have been rejected, are in a cash unit 
        /// other than this cash unit, and which have not been accessible to a customer. This value may be unreliable, 
        /// since a typical reason for rejecting items is a suspected pick failure. Other reasons for rejecting items 
        /// may include incorrect note denominations, classifications not valid for dispensing, or where the transaction 
        /// has been cancelled and a Reject command has been called. For reject and retract cash units 
        /// (*type* is *rejectCassette* or *retractCassette*) this field does not apply and will be reported as zero. This value is persistent.
        /// </summary>
        public int RejectCount { get; set; }

        /// <summary>
        /// As defined by the *logicalCount* description, but with the following exceptions:
        /// This count does not include items dispensed but not yet presented.
        /// On cash units with *type* set to \"retractCassette\" the count represents 
        /// the number of items, unless the device cannot count items during a retract, in which case this count will be zero.
        /// This value is persistent.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Count of items that have entered the cash unit. This counter is incremented whenever an item 
        /// enters a cash unit for any reason, unless it originated 
        /// from this cash unit but was returned without being accessible to a customer. For a retract cash unit this 
        /// value represents the total number of items of all types in the cash unit, or if the device cannot count 
        /// items during a retract operation this value will be zero. This value is persistent.
        /// </summary>
        public int CashInCount { get; set; }

        /// <summary>
        /// Array of banknote numbers the cash unit contains.
        /// Include all acceptable banknote types and counts here
        /// </summary>
        public List<BankNoteNumber> BankNoteNumberList { get; set; }
    }

    /// <summary>
    /// Cash Unit configuration the device supports
    /// </summary>
    public sealed record CashUnitConfiguration
    {
        public CashUnitConfiguration(CashUnit.TypeEnum Type,
                                     string CurrencyID,
                                     double Value,
                                     int Maximum,
                                     bool AppLock,
                                     string CashUnitName,
                                     int Minimum,
                                     string PhysicalPositionName,
                                     string UnitID,
                                     int MaximumCapacity,
                                     bool HardwareSensor,
                                     CashUnit.ItemTypesEnum ItemTypes,
                                     List<int> BanknoteIDs)
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
        public CashUnit.TypeEnum Type { get; init; }

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
        public double Value { get; init; }

        /// <summary>
        /// When *count* reaches this value the 
        /// threshold event CashManagement.CashUnitThresholdEvent (*high*) will be generated. This value can be different from
        /// the actual capacity of the cassette. 
        /// If this value is non-zero then hardware sensors in the device do not trigger threshold events. If this value is zero 
        /// then hardware sensors will trigger threshold events if *hardwareSensor* is TRUE. This value is persistent.
        /// </summary>
        public int Maximum { get; init; }

        /// <summary>
        /// If this value is TRUE items cannot be dispensed from or deposited into the cash unit. 
        /// If this value is TRUE and the application attempts to use the cash unit a CashManagement.CashUnitErrorEvent 
        /// event will be generated and an error completion message will be returned. This value is persistent.
        /// </summary>
        public bool AppLock { get; init; }

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
        public int Minimum { get; init; }

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
        public int MaximumCapacity { get; init; }

        /// <summary>
        /// Specifies whether or not threshold events can be generated based on hardware sensors in the device. 
        /// If this value is TRUE then threshold 
        /// events may be generated based on hardware sensors as opposed to counts.
        /// </summary>
        public bool HardwareSensor { get; init; }

        /// <summary>
        /// Specifies the type of items the cash unit takes as a combination of the following flags. 
        /// The table in the Comments section of this command defines how to interpret the combination of these flags 
        /// </summary>
        public CashUnit.ItemTypesEnum ItemTypes { get; init; }

        /// <summary>
        /// List of banknote IDs can be stored in this unit
        /// </summary>
        public List<int> BanknoteIDs { get; init; }

    }
}
