/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
        public OutPosClass(CashManagement.OutputPositionEnum? Position = null, ShutterEnum? Shutter = null, PositionStatusEnum? PositionStatus = null, TransportEnum? Transport = null, TransportStatusEnum? TransportStatus = null, JammedShutterPositionEnum? JammedShutterPosition = null)
        {
            this.Position = Position;
            this.Shutter = Shutter;
            this.PositionStatus = PositionStatus;
            this.Transport = Transport;
            this.TransportStatus = TransportStatus;
            this.JammedShutterPosition = JammedShutterPosition;
        }

        [DataMember(Name = "position")]
        public CashManagement.OutputPositionEnum? Position { get; init; }

        public enum ShutterEnum
        {
            Closed,
            Open,
            Jammed,
            Unknown,
            NotSupported
        }

        /// <summary>
        /// Supplies the state of the shutter. Following values are possible:
        /// 
        /// * ```closed``` - The shutter is operational and is closed.
        /// * ```open``` - The shutter is operational and is open.
        /// * ```jammed``` - The shutter is jammed and is not operational. The field jammedShutterPosition provides the positional state of the shutter.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the shutter cannot be determined.
        /// * ```notSupported``` - The physical device has no shutter or shutter state reporting is not supported.
        /// </summary>
        [DataMember(Name = "shutter")]
        public ShutterEnum? Shutter { get; init; }

        public enum PositionStatusEnum
        {
            Empty,
            NotEmpty,
            Unknown,
            NotSupported
        }

        /// <summary>
        /// Returns information regarding items which may be at the output position. 
        /// If the device is a recycler it is possible that the output position will not be empty due to a previous cash-in operation.
        /// Following values are possible:
        /// 
        /// * ```empty``` - The output position is empty.
        /// * ```notEmpty``` - The output position is not empty.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the output position cannot be determined.
        /// * ```notSupported``` - The device is not capable of reporting whether or not items are at the output position.
        /// </summary>
        [DataMember(Name = "positionStatus")]
        public PositionStatusEnum? PositionStatus { get; init; }

        public enum TransportEnum
        {
            Ok,
            Inoperative,
            Unknown,
            NotSupported
        }

        /// <summary>
        /// Supplies the state of the transport mechanism. The transport is defined as any area leading to or from the position.
        /// Following values are possible:
        /// 
        /// * ```ok``` - The transport is in a good state.
        /// * ```inoperative``` - The transport is inoperative due to a hardware failure or media jam.
        /// * ```unknown``` -Due to a hardware error or other condition the state of the transport cannot be determined.
        /// * ```notSupported``` - The physical device has no transport or transport state reporting is not supported.
        /// </summary>
        [DataMember(Name = "transport")]
        public TransportEnum? Transport { get; init; }

        public enum TransportStatusEnum
        {
            Empty,
            NotEmpty,
            NotEmptyCustomer,
            NotEmptyUnknown,
            NotSupported
        }

        /// <summary>
        /// Returns information regarding items which may be on the transport. If the device is a recycler 
        /// device it is possible that the transport will not be empty due to a previous cash-in operation. 
        /// Following values are possible:
        /// 
        /// * ```empty``` - The transport is empty.
        /// * ```notEmpty``` - The transport is not empty.
        /// * ```notEmptyCustomer``` - Items which a customer has had access to are on the transport.
        /// * ```notEmptyUnknown``` - Due to a hardware error or other condition it is not known whether there are items on the transport.
        /// * ```notSupported``` - The device is not capable of reporting whether items are on the transport.
        /// </summary>
        [DataMember(Name = "transportStatus")]
        public TransportStatusEnum? TransportStatus { get; init; }

        public enum JammedShutterPositionEnum
        {
            NotSupported,
            NotJammed,
            Open,
            PartiallyOpen,
            Closed,
            Unknown
        }

        /// <summary>
        /// Returns information regarding the position of the jammed shutter.
        /// Following values are possible:
        /// 
        /// * ```notSupported``` - The physical device has no shutter or the reporting of the position of a jammed shutter is not supported.
        /// * ```notJammed``` - The shutter is not jammed.
        /// * ```open``` - The shutter is jammed, but fully open.
        /// * ```partiallyOpen``` - The shutter is jammed, but partially open.
        /// * ```closed``` - The shutter is jammed, but fully closed.
        /// * ```unknown``` - The position of the shutter is unknown.
        /// </summary>
        [DataMember(Name = "jammedShutterPosition")]
        public JammedShutterPositionEnum? JammedShutterPosition { get; init; }

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
            Unknown,
            NotSupported
        }

        /// <summary>
        /// Supplies the state of the intermediate stacker. These bills are typically present on the intermediate 
        /// stacker as a result of a retract operation or because a dispense has been performed without a subsequent present.
        /// Following values are possible:
        /// 
        /// * ```empty``` - The intermediate stacker is empty.
        /// * ```notEmpty``` - The intermediate stacker is not empty. The items have not been in customer access.
        /// * ```notEmptyCustomer``` - The intermediate stacker is not empty. The items have been in customer access. If the device is 
        /// a recycler then the items on the intermediate stacker may be there as a result of a previous cash-in operation.
        /// * ```notEmptyUnknown``` - The intermediate stacker is not empty. It is not known if the items have been in customer access.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the intermediate stacker cannot be determined.
        /// * ```notSupported``` - The physical device has no intermediate stacker.
        /// </summary>
        [DataMember(Name = "intermediateStacker")]
        public IntermediateStackerEnum? IntermediateStacker { get; init; }

        /// <summary>
        /// Array of structures for each position to which items can be dispensed or presented.
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
        /// If no limit applies this value will be zero - in this case, if an attempt is made to dispense more items 
        /// than the hardware limitations will allow, the Service will implement the dispense as a series 
        /// of sub-dispense operations (see section Sub-Dispensing Command Flow).
        /// </summary>
        [DataMember(Name = "maxDispenseItems")]
        [DataTypes(Minimum = 1)]
        public int? MaxDispenseItems { get; init; }

        /// <summary>
        /// If true the shutter is controlled implicitly by the service. 
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
            public RetractAreasClass(bool? Retract = null, bool? Transport = null, bool? Stacker = null, bool? Reject = null, bool? ItemCassette = null)
            {
                this.Retract = Retract;
                this.Transport = Transport;
                this.Stacker = Stacker;
                this.Reject = Reject;
                this.ItemCassette = ItemCassette;
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
            /// The items may be retracted to storage units that can be dispensed from.
            /// </summary>
            [DataMember(Name = "itemCassette")]
            public bool? ItemCassette { get; init; }

        }

        /// <summary>
        /// Specifies the area to which items may be retracted. 
        /// If the device does not have a retract capability all flags will be set to false.
        /// </summary>
        [DataMember(Name = "retractAreas")]
        public RetractAreasClass RetractAreas { get; init; }

        [DataContract]
        public sealed class RetractTransportActionsClass
        {
            public RetractTransportActionsClass(bool? Present = null, bool? Retract = null, bool? Reject = null, bool? ItemCassette = null)
            {
                this.Present = Present;
                this.Retract = Retract;
                this.Reject = Reject;
                this.ItemCassette = ItemCassette;
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
            /// The items may be moved to storage units that can be dispensed from.
            /// </summary>
            [DataMember(Name = "itemCassette")]
            public bool? ItemCassette { get; init; }

        }

        /// <summary>
        /// Specifies the actions which may be performed on items which have been retracted to the transport. 
        /// If the device does not have the capability to retract items to the transport or move items from the 
        /// transport all flags will be set to false.
        /// </summary>
        [DataMember(Name = "retractTransportActions")]
        public RetractTransportActionsClass RetractTransportActions { get; init; }

        [DataContract]
        public sealed class RetractStackerActionsClass
        {
            public RetractStackerActionsClass(bool? Present = null, bool? Retract = null, bool? Reject = null, bool? ItemCassette = null)
            {
                this.Present = Present;
                this.Retract = Retract;
                this.Reject = Reject;
                this.ItemCassette = ItemCassette;
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
            /// The items may be moved to storage units that can be dispensed from.
            /// </summary>
            [DataMember(Name = "itemCassette")]
            public bool? ItemCassette { get; init; }

        }

        /// <summary>
        /// Specifies the actions which may be performed on items which have been retracted to the stacker. 
        /// If the device does not have the capability to retract items to the stacker or move items from the stacker 
        /// all flags will be set to false.
        /// </summary>
        [DataMember(Name = "retractStackerActions")]
        public RetractStackerActionsClass RetractStackerActions { get; init; }

        /// <summary>
        /// Specifies whether or not the Dispenser supports stacking items to an intermediate position before 
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
        /// Specifies the Dispenser move item options which are available.
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
        /// * ```individual``` - the mix is not calculated by the service, completely specified by the application.
        /// * ```algorithm``` - the mix is calculated using one of the algorithms specified by _algorithm_.
        /// * ```table``` - the mix is calculated using a mix table - see
        /// [CashDispenser.GetMixTable](#cashdispenser.getmixtable)
        /// <example>algorithm</example>
        /// </summary>
        [DataMember(Name = "type")]
        public TypeEnum? Type { get; init; }

        /// <summary>
        /// If _type_ is _algorithm_, specifies the algorithm type as one of the following. There are three pre-defined
        /// algorithms, additional vendor-defined algorithms can also be defined. Omitted if the mix is not an algorithm.
        /// 
        /// * ```minimumBills``` - Select a mix requiring the minimum possible number of items.
        /// * ```equalEmptying``` - The denomination is selected based upon criteria which ensure that over the course 
        /// of its operation the storage units will empty as far as possible at the same rate and will therefore go 
        /// low and then empty at approximately the same time.
        /// * ```maxCashUnits``` - The denomination will be selected based upon criteria which ensures the maximum 
        /// number of storage units are used.
        /// * ```&lt;vendor-defined mix&gt;``` - A vendor defined mix algorithm
        /// <example>minimumBills</example>
        /// </summary>
        [DataMember(Name = "algorithm")]
        [DataTypes(Pattern = @"^minimumBills$|^equalEmptying$|^maxCashUnits$|^[A-Za-z0-9]*$")]
        public string Algorithm { get; init; }

        /// <summary>
        /// Name of the table or algorithm used. May be omitted.
        /// <example>Minimum Bills</example>
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; init; }

    }


    [DataContract]
    public sealed class DenominationClass
    {
        public DenominationClass(Dictionary<string, double> Currencies = null, Dictionary<string, int> Values = null, Dictionary<string, double> CashBox = null)
        {
            this.Currencies = Currencies;
            this.Values = Values;
            this.CashBox = CashBox;
        }

        /// <summary>
        /// List of currency and amount combinations for denomination requests or output. There will be one entry for 
        /// each currency in the denomination. The property name is the ISO 4217 currency identifier. This list can be 
        /// omitted on a request if _values_ specifies the entire request.
        /// </summary>
        [DataMember(Name = "currencies")]
        public Dictionary<string, double> Currencies { get; init; }

        /// <summary>
        /// This list specifies the number of items to take from the cash units. If specified in a request, the output 
        /// denomination must include these items.
        /// 
        /// The property name is storage unit object name as stated by the [Storage.GetStorage](#storage.getstorage)
        /// command. The value of the entry is the number of items to take from that unit.
        /// </summary>
        [DataMember(Name = "values")]
        public Dictionary<string, int> Values { get; init; }

        /// <summary>
        /// Only applies to Teller Dispensers. Amount to be paid from the teller’s cash box.
        /// </summary>
        [DataMember(Name = "cashBox")]
        public Dictionary<string, double> CashBox { get; init; }

    }


    [DataContract]
    public sealed class DenominateRequestClass
    {
        public DenominateRequestClass(DenominationClass Denomination = null, string Mix = null, int? TellerID = null)
        {
            this.Denomination = Denomination;
            this.Mix = Mix;
            this.TellerID = TellerID;
        }

        [DataMember(Name = "denomination")]
        public DenominationClass Denomination { get; init; }

        /// <summary>
        /// Mix algorithm or house mix table to be used as defined by mixes reported by
        /// [CashDispenser.GetMixTypes](#cashdispenser.getmixtypes). May be omitted if the request is entirely specified
        /// by _counts_.
        /// <example>mix1</example>
        /// </summary>
        [DataMember(Name = "mix")]
        public string Mix { get; init; }

        /// <summary>
        /// Only applies to Teller Dispensers. Identification of teller.
        /// </summary>
        [DataMember(Name = "tellerID")]
        public int? TellerID { get; init; }

    }


}
