/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * CashDispenserSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.CashDispenser
{

    [DataContract]
    public sealed class OutPosClass
    {
        public OutPosClass(CashManagement.OutputPositionEnum? Position = null, ShutterEnum? Shutter = null, PositionStatusEnum? PositionStatus = null, TransportEnum? Transport = null, TransportStatusEnum? TransportStatus = null)
        {
            this.Position = Position;
            this.Shutter = Shutter;
            this.PositionStatus = PositionStatus;
            this.Transport = Transport;
            this.TransportStatus = TransportStatus;
        }

        [DataMember(Name = "position")]
        public CashManagement.OutputPositionEnum? Position { get; init; }

        public enum ShutterEnum
        {
            Closed,
            Open,
            JammedOpen,
            JammedPartiallyOpen,
            JammedClosed,
            JammedUnknown,
            Unknown
        }

        /// <summary>
        /// Supplies the state of the shutter. This property is null in [Common.Status](#common.status) if
        /// the physical position has no shutter, otherwise the following values are possible:
        /// 
        /// * ```closed``` - The shutter is operational and is closed.
        /// * ```open``` - The shutter is operational and is open.
        /// * ```jammedOpen``` - The shutter is jammed, but fully open. It is not operational.
        /// * ```jammedPartiallyOpen``` - The shutter is jammed, but partially open. It is not operational.
        /// * ```jammedClosed``` - The shutter is jammed, but fully closed. It is not operational.
        /// * ```jammedUnknown``` - The shutter is jammed, but its position is unknown. It is not operational.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the shutter cannot be determined.
        /// </summary>
        [DataMember(Name = "shutter")]
        public ShutterEnum? Shutter { get; init; }

        public enum PositionStatusEnum
        {
            Empty,
            NotEmpty,
            Unknown
        }

        /// <summary>
        /// Returns information regarding items which may be at the output position.
        /// If the device is a recycler it is possible that the output position will not be empty due to a previous cash-in operation.
        /// This property is null in [Common.Status](#common.status) if the device is not capable of reporting whether
        /// items are at the position, otherwise the following values are possible:
        /// 
        /// * ```empty``` - The position is empty.
        /// * ```notEmpty``` - The position is not empty.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the position cannot be determined.
        /// </summary>
        [DataMember(Name = "positionStatus")]
        public PositionStatusEnum? PositionStatus { get; init; }

        public enum TransportEnum
        {
            Ok,
            Inoperative,
            Unknown
        }

        /// <summary>
        /// Supplies the state of the transport mechanism. The transport is defined as any area leading to or from the position.
        /// This property is null in [Common.Status](#common.status) if the device has no transport or
        /// transport state reporting is not supported, otherwise the following values are possible:
        /// 
        /// * ```ok``` - The transport is in a good state.
        /// * ```inoperative``` - The transport is inoperative due to a hardware failure or media jam.
        /// * ```unknown``` - Due to a hardware error or other condition the state of the transport cannot be determined.
        /// </summary>
        [DataMember(Name = "transport")]
        public TransportEnum? Transport { get; init; }

        public enum TransportStatusEnum
        {
            Empty,
            NotEmpty,
            NotEmptyCustomer,
            Unknown
        }

        /// <summary>
        /// Returns information regarding items which may be on the transport. If the device is a recycler
        /// device it is possible that the transport will not be empty due to a previous cash-in operation.
        /// This property is null in [Common.Status](#common.status) if the device has no transport or
        /// is not capable of reporting whether items are on the transport, otherwise the following values are possible:
        /// 
        /// * ```empty``` - The transport is empty.
        /// * ```notEmpty``` - The transport is not empty.
        /// * ```notEmptyCustomer``` - Items which a customer has had access to are on the transport.
        /// * ```unknown``` - Due to a hardware error or other condition it is not known whether there are items on the transport.
        /// </summary>
        [DataMember(Name = "transportStatus")]
        public TransportStatusEnum? TransportStatus { get; init; }

    }


    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(IntermediateStackerEnum? IntermediateStacker = null, List<OutPosClass> Positions = null)
        {
            this.IntermediateStacker = IntermediateStacker;
            this.Positions = Positions;
        }

        public enum IntermediateStackerEnum
        {
            Empty,
            NotEmpty,
            NotEmptyCustomer,
            NotEmptyUnknown,
            Unknown
        }

        /// <summary>
        /// Supplies the state of the intermediate stacker. These bills are typically present on the intermediate
        /// stacker as a result of a retract operation or because a dispense has been performed without a subsequent present.
        /// This property is null in [Common.Status](#common.status) if the physical device has no intermediate stacker,
        /// otherwise the following values are possible:
        /// 
        /// * ```empty``` - The intermediate stacker is empty.
        /// * ```notEmpty``` - The intermediate stacker is not empty. The items have not been in customer access.
        /// * ```notEmptyCustomer``` - The intermediate stacker is not empty. The items have been in customer access. If the device is
        /// a recycler then the items on the intermediate stacker may be there as a result of a previous cash-in operation.
        /// * ```notEmptyUnknown``` - The intermediate stacker is not empty. It is not known if the items have been in customer access.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the intermediate stacker cannot be determined.
        /// </summary>
        [DataMember(Name = "intermediateStacker")]
        public IntermediateStackerEnum? IntermediateStacker { get; init; }

        /// <summary>
        /// Array of structures for each position to which items can be dispensed or presented. This may be null
        /// in [Common.StatusChangedEvent](#common.statuschangedevent) if no position states have changed.
        /// </summary>
        [DataMember(Name = "positions")]
        public List<OutPosClass> Positions { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(TypeEnum? Type = null, int? MaxDispenseItems = null, bool? ShutterControl = null, RetractAreasClass RetractAreas = null, RetractTransportActionsClass RetractTransportActions = null, RetractStackerActionsClass RetractStackerActions = null, bool? IntermediateStacker = null, bool? ItemsTakenSensor = null, PositionsClass Positions = null, MoveItemsClass MoveItems = null)
        {
            this.Type = Type;
            this.MaxDispenseItems = MaxDispenseItems;
            this.ShutterControl = ShutterControl;
            this.RetractAreas = RetractAreas;
            this.RetractTransportActions = RetractTransportActions;
            this.RetractStackerActions = RetractStackerActions;
            this.IntermediateStacker = IntermediateStacker;
            this.ItemsTakenSensor = ItemsTakenSensor;
            this.Positions = Positions;
            this.MoveItems = MoveItems;
        }

        public enum TypeEnum
        {
            TellerBill,
            SelfServiceBill,
            TellerCoin,
            SelfServiceCoin
        }

        /// <summary>
        /// Supplies the type of Dispenser. Following values are possible:
        /// 
        /// * ```tellerBill``` - The Dispenser is a Teller Bill Dispenser.
        /// * ```selfServiceBill``` - The Dispenser is a Self-Service Bill Dispenser.
        /// * ```tellerCoin``` - The Dispenser is a Teller Coin Dispenser.
        /// * ```selfServiceCoin``` - The Dispenser is a Self-Service Coin Dispenser.
        /// </summary>
        [DataMember(Name = "type")]
        public TypeEnum? Type { get; init; }

        /// <summary>
        /// Supplies the maximum number of items that can be dispensed in a single dispense operation.
        /// </summary>
        [DataMember(Name = "maxDispenseItems")]
        [DataTypes(Minimum = 1)]
        public int? MaxDispenseItems { get; init; }

        /// <summary>
        /// If true the shutter is controlled implicitly by the Service.
        /// If false the shutter must be controlled explicitly by the application using the
        /// [CashManagement.OpenShutter](#cashmanagement.openshutter) and
        /// [CashManagement.CloseShutter](#cashmanagement.closeshutter) commands.
        /// 
        /// This property is always true if the device has no shutter.
        /// This property applies to all shutters and all output positions.
        /// </summary>
        [DataMember(Name = "shutterControl")]
        public bool? ShutterControl { get; init; }

        [DataContract]
        public sealed class RetractAreasClass
        {
            public RetractAreasClass(bool? Retract = null, bool? Transport = null, bool? Stacker = null, bool? Reject = null, bool? ItemCassette = null, bool? CashIn = null)
            {
                this.Retract = Retract;
                this.Transport = Transport;
                this.Stacker = Stacker;
                this.Reject = Reject;
                this.ItemCassette = ItemCassette;
                this.CashIn = CashIn;
            }

            /// <summary>
            /// The items may be retracted to a retract storage unit.
            /// </summary>
            [DataMember(Name = "retract")]
            public bool? Retract { get; init; }

            /// <summary>
            /// The items may be retracted to the transport.
            /// </summary>
            [DataMember(Name = "transport")]
            public bool? Transport { get; init; }

            /// <summary>
            /// The items may be retracted to the intermediate stacker.
            /// </summary>
            [DataMember(Name = "stacker")]
            public bool? Stacker { get; init; }

            /// <summary>
            /// The items may be retracted to a reject storage unit.
            /// </summary>
            [DataMember(Name = "reject")]
            public bool? Reject { get; init; }

            /// <summary>
            /// The items may be retracted to storage units which would be used during a Cash In transaction including recycling storage units.
            /// </summary>
            [DataMember(Name = "itemCassette")]
            public bool? ItemCassette { get; init; }

            /// <summary>
            /// The items may be retracted to storage units which would be used during a Cash In transaction not including recycling storage units.
            /// </summary>
            [DataMember(Name = "cashIn")]
            public bool? CashIn { get; init; }

        }

        /// <summary>
        /// Specifies the area to which items may be retracted.
        /// If the device does not have a retract capability this will be null.
        /// </summary>
        [DataMember(Name = "retractAreas")]
        public RetractAreasClass RetractAreas { get; init; }

        [DataContract]
        public sealed class RetractTransportActionsClass
        {
            public RetractTransportActionsClass(bool? Present = null, bool? Retract = null, bool? Reject = null, bool? ItemCassette = null, bool? CashIn = null)
            {
                this.Present = Present;
                this.Retract = Retract;
                this.Reject = Reject;
                this.ItemCassette = ItemCassette;
                this.CashIn = CashIn;
            }

            /// <summary>
            /// The items may be presented.
            /// </summary>
            [DataMember(Name = "present")]
            public bool? Present { get; init; }

            /// <summary>
            /// The items may be moved to a retract storage unit.
            /// </summary>
            [DataMember(Name = "retract")]
            public bool? Retract { get; init; }

            /// <summary>
            /// The items may be moved to a reject storage unit.
            /// </summary>
            [DataMember(Name = "reject")]
            public bool? Reject { get; init; }

            /// <summary>
            /// The items may be moved to storage units which would be used during a Cash In transaction including recycling storage units.
            /// </summary>
            [DataMember(Name = "itemCassette")]
            public bool? ItemCassette { get; init; }

            /// <summary>
            /// The items may be moved to storage units which would be used during a Cash In transaction not including recycling storage units.
            /// </summary>
            [DataMember(Name = "cashIn")]
            public bool? CashIn { get; init; }

        }

        /// <summary>
        /// Specifies the actions which may be performed on items which have been retracted to the transport.
        /// If the device does not have the capability to retract items to the transport or move items from the
        /// transport this will be null.
        /// </summary>
        [DataMember(Name = "retractTransportActions")]
        public RetractTransportActionsClass RetractTransportActions { get; init; }

        [DataContract]
        public sealed class RetractStackerActionsClass
        {
            public RetractStackerActionsClass(bool? Present = null, bool? Retract = null, bool? Reject = null, bool? ItemCassette = null, bool? CashIn = null)
            {
                this.Present = Present;
                this.Retract = Retract;
                this.Reject = Reject;
                this.ItemCassette = ItemCassette;
                this.CashIn = CashIn;
            }

            /// <summary>
            /// The items may be presented.
            /// </summary>
            [DataMember(Name = "present")]
            public bool? Present { get; init; }

            /// <summary>
            /// The items may be moved to a retract storage unit.
            /// </summary>
            [DataMember(Name = "retract")]
            public bool? Retract { get; init; }

            /// <summary>
            /// The items may be moved to a reject storage unit.
            /// </summary>
            [DataMember(Name = "reject")]
            public bool? Reject { get; init; }

            /// <summary>
            /// The items may be moved to storage units which would be used during a Cash In transaction including recycling storage units.
            /// </summary>
            [DataMember(Name = "itemCassette")]
            public bool? ItemCassette { get; init; }

            /// <summary>
            /// The items may be moved to storage units which would be used during a Cash In transaction not including recycling storage units.
            /// </summary>
            [DataMember(Name = "cashIn")]
            public bool? CashIn { get; init; }

        }

        /// <summary>
        /// Specifies the actions which may be performed on items which have been retracted to the stacker.
        /// If the device does not have the capability to retract items to the stacker or move items from the stacker
        /// this will be null.
        /// </summary>
        [DataMember(Name = "retractStackerActions")]
        public RetractStackerActionsClass RetractStackerActions { get; init; }

        /// <summary>
        /// Specifies whether the Dispenser supports stacking items to an intermediate position before
        /// the items are moved to the exit position.
        /// </summary>
        [DataMember(Name = "intermediateStacker")]
        public bool? IntermediateStacker { get; init; }

        /// <summary>
        /// Specifies whether the Dispenser can detect when items at the exit position are taken by the user. This
        /// applies to all output positions.
        /// 
        /// If true the Service generates an accompanying
        /// [CashManagement.ItemsTakenEvent](#cashmanagement.itemstakenevent).
        /// 
        /// If false this event is not generated.
        /// </summary>
        [DataMember(Name = "itemsTakenSensor")]
        public bool? ItemsTakenSensor { get; init; }

        [DataContract]
        public sealed class PositionsClass
        {
            public PositionsClass(bool? Left = null, bool? Right = null, bool? Center = null, bool? Top = null, bool? Bottom = null, bool? Front = null, bool? Rear = null)
            {
                this.Left = Left;
                this.Right = Right;
                this.Center = Center;
                this.Top = Top;
                this.Bottom = Bottom;
                this.Front = Front;
                this.Rear = Rear;
            }

            /// <summary>
            /// The Dispenser has a left output position.
            /// </summary>
            [DataMember(Name = "left")]
            public bool? Left { get; init; }

            /// <summary>
            /// The Dispenser has a right output position.
            /// </summary>
            [DataMember(Name = "right")]
            public bool? Right { get; init; }

            /// <summary>
            /// The Dispenser has a center output position.
            /// </summary>
            [DataMember(Name = "center")]
            public bool? Center { get; init; }

            /// <summary>
            /// The Dispenser has a top output position.
            /// </summary>
            [DataMember(Name = "top")]
            public bool? Top { get; init; }

            /// <summary>
            /// The Dispenser has a bottom output position.
            /// </summary>
            [DataMember(Name = "bottom")]
            public bool? Bottom { get; init; }

            /// <summary>
            /// The Dispenser has a front output position.
            /// </summary>
            [DataMember(Name = "front")]
            public bool? Front { get; init; }

            /// <summary>
            /// The Dispenser has a rear output position.
            /// </summary>
            [DataMember(Name = "rear")]
            public bool? Rear { get; init; }

        }

        /// <summary>
        /// Specifies the Dispenser output positions which are available.
        /// </summary>
        [DataMember(Name = "positions")]
        public PositionsClass Positions { get; init; }

        [DataContract]
        public sealed class MoveItemsClass
        {
            public MoveItemsClass(bool? FromCashUnit = null, bool? ToCashUnit = null, bool? ToTransport = null, bool? ToStacker = null)
            {
                this.FromCashUnit = FromCashUnit;
                this.ToCashUnit = ToCashUnit;
                this.ToTransport = ToTransport;
                this.ToStacker = ToStacker;
            }

            /// <summary>
            /// The Dispenser can dispense items from the storage units to the intermediate stacker while there are items
            /// on the transport.
            /// </summary>
            [DataMember(Name = "fromCashUnit")]
            public bool? FromCashUnit { get; init; }

            /// <summary>
            /// The Dispenser can retract items to the storage units while there are items on the intermediate stacker.
            /// </summary>
            [DataMember(Name = "toCashUnit")]
            public bool? ToCashUnit { get; init; }

            /// <summary>
            /// The Dispenser can retract items to the transport while there are items on the intermediate stacker.
            /// </summary>
            [DataMember(Name = "toTransport")]
            public bool? ToTransport { get; init; }

            /// <summary>
            /// The Dispenser can dispense items from the storage units to the intermediate stacker while there are
            /// already items on the intermediate stacker that have not been in customer access. Items remaining on the
            /// stacker from a previous dispense may first need to be rejected explicitly by the application if they
            /// are not to be presented.
            /// </summary>
            [DataMember(Name = "toStacker")]
            public bool? ToStacker { get; init; }

        }

        /// <summary>
        /// Specifies the Dispenser move item options which are available. If not applicable, this property is null.
        /// </summary>
        [DataMember(Name = "moveItems")]
        public MoveItemsClass MoveItems { get; init; }

    }


    [DataContract]
    public sealed class MixClass
    {
        public MixClass(TypeEnum? Type = null, string Algorithm = null, string Name = null)
        {
            this.Type = Type;
            this.Algorithm = Algorithm;
            this.Name = Name;
        }

        public enum TypeEnum
        {
            Individual,
            Algorithm,
            Table
        }

        /// <summary>
        /// Specifies the mix type as one of the following:
        /// 
        /// * ```individual``` - the mix is not calculated by the Service, completely specified by the application.
        /// * ```algorithm``` - the mix is calculated using one of the algorithms specified by *algorithm*.
        /// * ```table``` - the mix is calculated using a mix table - see
        /// [CashDispenser.GetMixTable](#cashdispenser.getmixtable).
        /// <example>algorithm</example>
        /// </summary>
        [DataMember(Name = "type")]
        public TypeEnum? Type { get; init; }

        /// <summary>
        /// If *type* is *algorithm*, specifies the algorithm type as one of the following. There are three pre-defined
        /// algorithms, additional vendor-defined algorithms can also be defined. null if the mix is not an algorithm.
        /// 
        /// * ```minimumBills``` - Select a mix requiring the minimum possible number of items.
        /// * ```equalEmptying``` - The denomination is selected based upon criteria which ensure that over the course
        /// of its operation the storage units will empty as far as possible at the same rate and will therefore go
        /// low and then empty at approximately the same time.
        /// * ```maxCashUnits``` - The denomination is selected based upon criteria which ensures the maximum
        /// number of storage units are used.
        /// * ```&lt;vendor-defined mix&gt;``` - A vendor defined mix algorithm.
        /// <example>minimumBills</example>
        /// </summary>
        [DataMember(Name = "algorithm")]
        [DataTypes(Pattern = @"^minimumBills$|^equalEmptying$|^maxCashUnits$|^[A-Za-z0-9]*$")]
        public string Algorithm { get; init; }

        /// <summary>
        /// Name of the table or algorithm used. May be null if not defined.
        /// <example>Minimum Bills</example>
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; init; }

    }


    [DataContract]
    public sealed class MixRowClass
    {
        public MixRowClass(double? Amount = null, List<MixClass> Mix = null)
        {
            this.Amount = Amount;
            this.Mix = Mix;
        }

        /// <summary>
        /// Absolute value of the amount denominated by this mix row.
        /// <example>0.30</example>
        /// </summary>
        [DataMember(Name = "amount")]
        public double? Amount { get; init; }

        [DataContract]
        public sealed class MixClass
        {
            public MixClass(double? Value = null, int? Count = null)
            {
                this.Value = Value;
                this.Count = Count;
            }

            /// <summary>
            /// The absolute value of a single cash item.
            /// <example>0.05</example>
            /// </summary>
            [DataMember(Name = "value")]
            public double? Value { get; init; }

            /// <summary>
            /// The number of items of *value* contained in the mix.
            /// <example>6</example>
            /// </summary>
            [DataMember(Name = "count")]
            [DataTypes(Minimum = 1)]
            public int? Count { get; init; }

        }

        /// <summary>
        /// The items used to create *amount*. Each element in this array defines the quantity of a given item used to
        /// create the mix. An example showing how 0.30 can be broken down would be:
        /// 
        /// ```
        /// [
        ///   {
        ///     "value": 0.05,
        ///     "count": 2
        ///   },
        ///   {
        ///     "value": 0.10,
        ///     "count": 2
        ///   }
        /// ]
        /// ```
        /// </summary>
        [DataMember(Name = "mix")]
        public List<MixClass> Mix { get; init; }

    }


    [DataContract]
    public sealed class DenominationCashBoxClass
    {
        public DenominationCashBoxClass(Dictionary<string, double> Currencies = null)
        {
            this.Currencies = Currencies;
        }

        /// <summary>
        /// List of currency and amount combinations for denomination requests or output. There will be one entry for
        /// each currency in the denomination.
        /// </summary>
        [DataMember(Name = "currencies")]
        public Dictionary<string, double> Currencies { get; init; }

    }


    [DataContract]
    public sealed class DenominationClass
    {
        public DenominationClass(Dictionary<string, double> Currencies = null, Dictionary<string, int> Values = null, DenominationCashBoxClass CashBox = null)
        {
            this.Currencies = Currencies;
            this.Values = Values;
            this.CashBox = CashBox;
        }

        /// <summary>
        /// List of currency and amount combinations for denomination requests or output. There will be one entry for
        /// each currency in the denomination.
        /// </summary>
        [DataMember(Name = "currencies")]
        public Dictionary<string, double> Currencies { get; init; }

        /// <summary>
        /// This list specifies the number of items to take or which have been taken from the storage units. If specified in
        /// a request, the output denomination must include these items.
        /// 
        /// The property name is storage unit object name as stated by the [Storage.GetStorage](#storage.getstorage)
        /// command. The value of the entry is the number of items to take from that unit.
        /// </summary>
        [DataMember(Name = "values")]
        public Dictionary<string, int> Values { get; init; }

        [DataMember(Name = "cashBox")]
        public DenominationCashBoxClass CashBox { get; init; }

    }


    [DataContract]
    public sealed class ApplicationMixClass
    {
        public ApplicationMixClass(Dictionary<string, double> Currencies = null, Dictionary<string, int> Counts = null, DenominationCashBoxClass CashBox = null)
        {
            this.Currencies = Currencies;
            this.Counts = Counts;
            this.CashBox = CashBox;
        }

        /// <summary>
        /// List of currency and amount combinations for denomination requests or output. There will be one entry for
        /// each currency in the denomination.
        /// </summary>
        [DataMember(Name = "currencies")]
        public Dictionary<string, double> Currencies { get; init; }

        /// <summary>
        /// This list specifies the number of items to take or which have been taken from the storage units. If specified in
        /// a request, the output denomination must include these items.
        /// 
        /// The property name is storage unit object name as stated by the [Storage.GetStorage](#storage.getstorage)
        /// command. The value of the entry is the number of items to take from that unit.
        /// </summary>
        [DataMember(Name = "counts")]
        public Dictionary<string, int> Counts { get; init; }

        [DataMember(Name = "cashBox")]
        public DenominationCashBoxClass CashBox { get; init; }

    }


    [DataContract]
    public sealed class ServiceMixClass
    {
        public ServiceMixClass(Dictionary<string, double> Currencies = null, Dictionary<string, int> Partial = null, string Mix = null, DenominationCashBoxClass CashBox = null)
        {
            this.Currencies = Currencies;
            this.Partial = Partial;
            this.Mix = Mix;
            this.CashBox = CashBox;
        }

        /// <summary>
        /// List of currency and amount combinations for denomination requests or output. There will be one entry for
        /// each currency in the denomination.
        /// </summary>
        [DataMember(Name = "currencies")]
        public Dictionary<string, double> Currencies { get; init; }

        /// <summary>
        /// This list specifies items which must be included in a denominate or dispense request. Mixes may only be valid if
        /// they contain at least these specified items. This may be null if there are no minimum requirements.
        /// 
        /// The property name is storage unit object name as stated by the [Storage.GetStorage](#storage.getstorage)
        /// command. The value of the entry is the number of items to take from that unit.
        /// </summary>
        [DataMember(Name = "partial")]
        public Dictionary<string, int> Partial { get; init; }

        /// <summary>
        /// Mix algorithm or house mix table to be used as defined by mixes reported by
        /// [CashDispenser.GetMixTypes](#cashdispenser.getmixtypes).
        /// <example>mix1</example>
        /// </summary>
        [DataMember(Name = "mix")]
        [DataTypes(Pattern = @"^mix[0-9A-Za-z]+$")]
        public string Mix { get; init; }

        [DataMember(Name = "cashBox")]
        public DenominationCashBoxClass CashBox { get; init; }

    }


    [DataContract]
    public sealed class DenominateRequestClass
    {
        public DenominateRequestClass(DenominationClass Denomination = null, int? TellerID = null)
        {
            this.Denomination = Denomination;
            this.TellerID = TellerID;
        }

        [DataContract]
        public sealed class DenominationClass
        {
            public DenominationClass(ApplicationMixClass App = null, ServiceMixClass Service = null)
            {
                this.App = App;
                this.Service = Service;
            }

            [DataMember(Name = "app")]
            public ApplicationMixClass App { get; init; }

            [DataMember(Name = "service")]
            public ServiceMixClass Service { get; init; }

        }

        /// <summary>
        /// The items to be denominated or dispensed as appropriate. The mix of items is either determined by the
        /// Service or the Application.
        /// </summary>
        [DataMember(Name = "denomination")]
        public DenominationClass Denomination { get; init; }

        /// <summary>
        /// Only applies to Teller Dispensers, null if not applicable. Identification of teller.
        /// </summary>
        [DataMember(Name = "tellerID")]
        [DataTypes(Minimum = 0)]
        public int? TellerID { get; init; }

    }


}
