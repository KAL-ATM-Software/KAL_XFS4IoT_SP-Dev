/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * CashUnitInfoChangedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashManagement.Events
{

    [DataContract]
    [Event(Name = "CashManagement.CashUnitInfoChangedEvent")]
    public sealed class CashUnitInfoChangedEvent : UnsolicitedEvent<CashUnitInfoChangedEvent.PayloadData>
    {

        public CashUnitInfoChangedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(StatusEnum? Status = null, TypeEnum? Type = null, string CurrencyID = null, double? Value = null, int? LogicalCount = null, int? Maximum = null, bool? AppLock = null, string CashUnitName = null, int? InitialCount = null, int? DispensedCount = null, int? PresentedCount = null, int? RetractedCount = null, int? RejectCount = null, int? Minimum = null, string PhysicalPositionName = null, string UnitID = null, int? Count = null, int? MaximumCapacity = null, bool? HardwareSensor = null, ItemTypeClass ItemType = null, int? CashInCount = null, NoteNumberListClass NoteNumberList = null, List<int> NoteIDs = null)
                : base()
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
                this.ItemType = ItemType;
                this.CashInCount = CashInCount;
                this.NoteNumberList = NoteNumberList;
                this.NoteIDs = NoteIDs;
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
            [DataMember(Name = "status")]
            public StatusEnum? Status { get; private set; }

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
            [DataMember(Name = "type")]
            public TypeEnum? Type { get; private set; }

            /// <summary>
            /// A three character string storing the ISO format [Ref. 2] Currency ID. This value will be omitted for 
            /// cash units which contain items of more than one currency type or items to which currency is not applicable. 
            /// If the *status* field for this cash unit is *noValue* it is the responsibility of the application to assign 
            /// a value to this field. This value is persistent.
            /// </summary>
            [DataMember(Name = "currencyID")]
            public string CurrencyID { get; private set; }

            /// <summary>
            /// Supplies the value of a single item in the cash unit. This value is expressed as floating point value.
            /// If the *currencyID* field for this cash unit is omitted, then this 
            /// field will contain zero. If the *status* field for this cash unit is *noValue* it is the responsibility of the 
            /// application to assign a value to this field. This value is persistent.
            /// </summary>
            [DataMember(Name = "value")]
            public double? Value { get; private set; }

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
            [DataMember(Name = "logicalCount")]
            public int? LogicalCount { get; private set; }

            /// <summary>
            /// When *count* reaches this value the 
            /// threshold event CashManagement.CashUnitThresholdEvent (*high*) will be generated. This value can be different from
            /// the actual capacity of the cassette. 
            /// If this value is non-zero then hardware sensors in the device do not trigger threshold events. If this value is zero 
            /// then hardware sensors will trigger threshold events if *hardwareSensor* is TRUE. This value is persistent.
            /// </summary>
            [DataMember(Name = "maximum")]
            public int? Maximum { get; private set; }

            /// <summary>
            /// If this value is TRUE items cannot be dispensed from or deposited into the cash unit. 
            /// If this value is TRUE and the application attempts to use the cash unit a CashManagement.CashUnitErrorEvent 
            /// event will be generated and an error completion message will be returned. This value is persistent.
            /// </summary>
            [DataMember(Name = "appLock")]
            public bool? AppLock { get; private set; }

            /// <summary>
            /// A name which helps to identify the type of the cash unit. 
            /// This is especially useful in the case of cash units of type *document* where different 
            /// documents can have the same currency and value. For example, travelers checks and bank 
            /// checks may have the same currency and value but still need to be identifiable as different 
            /// types of document. Where this value is not relevant (e.g. in bill cash units) the property can be omitted. This value is persistent.
            /// </summary>
            [DataMember(Name = "cashUnitName")]
            public string CashUnitName { get; private set; }

            /// <summary>
            /// Initial number of items contained in the cash unit. This value is persistent.
            /// </summary>
            [DataMember(Name = "initialCount")]
            public int? InitialCount { get; private set; }

            /// <summary>
            /// The number of items dispensed from this cash unit. 
            /// This count is incremented when the items are removed from the cash units. 
            /// This count includes any items that were rejected during the dispense operation and are no longer in this cash unit. 
            /// This field is always zero for cash units with a *type* of *rejectCassette* or *retractCassette*. This value is persistent.
            /// </summary>
            [DataMember(Name = "dispensedCount")]
            public int? DispensedCount { get; private set; }

            /// <summary>
            /// The number of items from this cash unit that have been presented to the customer. 
            /// This count is incremented when the items are presented to the customer.
            /// If it is unknown if a customer has been presented with the items, then this count is not updated. 
            /// This field is always zero for cash units with a *type* of *rejectCassette* or *retractCassette*. This value is persistent.
            /// </summary>
            [DataMember(Name = "presentedCount")]
            public int? PresentedCount { get; private set; }

            /// <summary>
            /// The number of items that have been accessible to a customer and retracted into the 
            /// cash unit. This value is persistent.
            /// </summary>
            [DataMember(Name = "retractedCount")]
            public int? RetractedCount { get; private set; }

            /// <summary>
            /// The number of items dispensed from this cash unit which have been rejected, are in a cash unit 
            /// other than this cash unit, and which have not been accessible to a customer. This value may be unreliable, 
            /// since a typical reason for rejecting items is a suspected pick failure. Other reasons for rejecting items 
            /// may include incorrect note denominations, classifications not valid for dispensing, or where the transaction 
            /// has been cancelled and a Reject command has been called. For reject and retract cash units 
            /// (*type* is *rejectCassette* or *retractCassette*) this field does not apply and will be reported as zero. This value is persistent.
            /// </summary>
            [DataMember(Name = "rejectCount")]
            public int? RejectCount { get; private set; }

            /// <summary>
            /// This field is not applicable to retract and reject cash units. For all cash units which dispense items (all other), when *count*
            /// reaches this value the threshold event CashManagement.CashUnitThresholdEvent (*low*) will be generated. 
            /// If this value is non-zero then hardware sensors in the device do not trigger threshold events. 
            /// If this value is zero then hardware sensors will trigger threshold events if *hardwareSensor* is TRUE. This value is persistent.
            /// </summary>
            [DataMember(Name = "minimum")]
            public int? Minimum { get; private set; }

            /// <summary>
            /// A name identifying the physical location of the cash unit.
            /// </summary>
            [DataMember(Name = "physicalPositionName")]
            public string PhysicalPositionName { get; private set; }

            /// <summary>
            /// A 5 character string uniquely identifying the cash unit.
            /// </summary>
            [DataMember(Name = "unitID")]
            public string UnitID { get; private set; }

            /// <summary>
            /// As defined by the *logicalCount* description, but with the following exceptions:
            /// This count does not include items dispensed but not yet presented.
            /// On cash units with *type* set to \"retractCassette\" the count represents 
            /// the number of items, unless the device cannot count items during a retract, in which case this count will be zero.
            /// This value is persistent.
            /// </summary>
            [DataMember(Name = "count")]
            public int? Count { get; private set; }

            /// <summary>
            /// The maximum number of items the cash unit can hold. This is only for informational purposes. 
            /// No threshold event CashManagement.CashUnitThresholdEvent will be generated. This value is persistent.
            /// </summary>
            [DataMember(Name = "maximumCapacity")]
            public int? MaximumCapacity { get; private set; }

            /// <summary>
            /// Specifies whether or not threshold events can be generated based on hardware sensors in the device. 
            /// If this value is TRUE then threshold 
            /// events may be generated based on hardware sensors as opposed to counts.
            /// </summary>
            [DataMember(Name = "hardwareSensor")]
            public bool? HardwareSensor { get; private set; }

            [DataContract]
            public sealed class ItemTypeClass
            {
                public ItemTypeClass(bool? All = null, bool? Unfit = null, bool? Individual = null, bool? Level1 = null, bool? Level2 = null, bool? Level3 = null, bool? ItemProcessor = null, bool? UnfitIndividual = null)
                {
                    this.All = All;
                    this.Unfit = Unfit;
                    this.Individual = Individual;
                    this.Level1 = Level1;
                    this.Level2 = Level2;
                    this.Level3 = Level3;
                    this.ItemProcessor = ItemProcessor;
                    this.UnfitIndividual = UnfitIndividual;
                }

                /// <summary>
                /// The cash unit takes all fit banknote types. These are level 4 notes which are fit for recycling.
                /// </summary>
                [DataMember(Name = "all")]
                public bool? All { get; private set; }

                /// <summary>
                /// The cash unit takes all unfit banknotes. These are level 4 notes which are unfit for recycling.
                /// </summary>
                [DataMember(Name = "unfit")]
                public bool? Unfit { get; private set; }

                /// <summary>
                /// The cash unit takes all types of fit banknotes specified in an individual list. These are level 4 notes
                /// which are fit for recycling.
                /// </summary>
                [DataMember(Name = "individual")]
                public bool? Individual { get; private set; }

                /// <summary>
                /// Level 1 note types are stored in this cash unit.
                /// </summary>
                [DataMember(Name = "level1")]
                public bool? Level1 { get; private set; }

                /// <summary>
                /// If notes can be classified as level 2, then level 2 note types are stored in this cash unit.
                /// </summary>
                [DataMember(Name = "level2")]
                public bool? Level2 { get; private set; }

                /// <summary>
                /// If notes can be classified as level 3, then level 3 note types are stored in this cash unit.
                /// </summary>
                [DataMember(Name = "level3")]
                public bool? Level3 { get; private set; }

                /// <summary>
                /// The cash unit can accept items on the ItemProcessor interface.
                /// </summary>
                [DataMember(Name = "itemProcessor")]
                public bool? ItemProcessor { get; private set; }

                /// <summary>
                /// The cash unit takes all types of unfit banknotes specified in an individual list. These are level 4
                /// notes which are unfit for recycling.
                /// </summary>
                [DataMember(Name = "unfitIndividual")]
                public bool? UnfitIndividual { get; private set; }

            }

            /// <summary>
            /// Specifies the type of items the cash unit takes as a combination of the following flags. 
            /// The table in the Comments section of this command defines how to interpret the combination of these flags (TODO: include Table)
            /// </summary>
            [DataMember(Name = "itemType")]
            public ItemTypeClass ItemType { get; private set; }

            /// <summary>
            /// Count of items that have entered the cash unit. This counter is incremented whenever an item 
            /// enters a cash unit for any reason, unless it originated 
            /// from this cash unit but was returned without being accessible to a customer. For a retract cash unit this 
            /// value represents the total number of items of all types in the cash unit, or if the device cannot count 
            /// items during a retract operation this value will be zero. This value is persistent.
            /// </summary>
            [DataMember(Name = "cashInCount")]
            public int? CashInCount { get; private set; }

            [DataContract]
            public sealed class NoteNumberListClass
            {
                public NoteNumberListClass(List<NoteNumberClass> NoteNumber = null)
                {
                    this.NoteNumber = NoteNumber;
                }

                [DataContract]
                public sealed class NoteNumberClass
                {
                    public NoteNumberClass(int? NoteID = null, int? Count = null)
                    {
                        this.NoteID = NoteID;
                        this.Count = Count;
                    }

                    /// <summary>
                    /// Identification of note type. The Note ID represents the note identifiers reported by the *CashAcceptor.BanknoteTypes* command. 
                    /// If this value is zero then the note type is unknown.
                    /// </summary>
                    [DataMember(Name = "noteID")]
                    public int? NoteID { get; private set; }

                    /// <summary>
                    /// Actual count of cash items. The value is incremented each time cash items are moved to a cash unit. 
                    /// In the case of recycle cash units this count is decremented as defined in the description of the *logicalCount* field.
                    /// </summary>
                    [DataMember(Name = "count")]
                    public int? Count { get; private set; }

                }

                /// <summary>
                /// Array of banknote numbers the cash unit contains.
                /// </summary>
                [DataMember(Name = "noteNumber")]
                public List<NoteNumberClass> NoteNumber { get; private set; }

            }

            /// <summary>
            /// Array of cash items inside the cash unit. The content of this structure is persistent. 
            /// If the cash unit is Dispenser specific cash unit with *type* *billCassette* or the contents of the cash unit are not known
            /// this structure will be omitted.
            /// If the cash unit is of *type* *retractCassette* this pointer will be omitted except for the following cases:
            /// 
            /// •\tIf the retract cash unit is configured to accept level 2 notes then the number and type of level 2 notes is returned in 
            /// the *noteNumberList* and *count* contains the number of retract operations. *cashInCount* contains the actual number of level 2 notes.
            /// 
            /// •\tIf items are recognized during retract operations then the number and type of notes retracted is returned in *noteNumberList*
            /// and *count* contains the number of retract operations. *cashInCount* contains the actual number of retracted items.
            /// </summary>
            [DataMember(Name = "noteNumberList")]
            public NoteNumberListClass NoteNumberList { get; private set; }

            /// <summary>
            /// Array of integers which contains the note IDs of the banknotes the cash-in cash unit or 
            /// recycle cash unit can take. This field only applies to *individual* cassette types. If there are no note IDs 
            /// defined for the cassette or the cassette is not defined as *individual* then *noteIDs* will be omitted.
            /// </summary>
            [DataMember(Name = "noteIDs")]
            public List<int> NoteIDs { get; private set; }

        }

    }
}
