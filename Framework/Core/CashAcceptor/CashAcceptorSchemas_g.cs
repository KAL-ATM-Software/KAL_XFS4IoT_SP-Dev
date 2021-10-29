/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CashAcceptorSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.CashAcceptor
{

    [DataContract]
    public sealed class PositionClass
    {
        public PositionClass(CashManagement.PositionEnum? Position = null, ShutterEnum? Shutter = null, PositionStatusEnum? PositionStatus = null, TransportEnum? Transport = null, TransportStatusEnum? TransportStatus = null, JammedShutterPositionEnum? JammedShutterPosition = null)
        {
            this.Position = Position;
            this.Shutter = Shutter;
            this.PositionStatus = PositionStatus;
            this.Transport = Transport;
            this.TransportStatus = TransportStatus;
            this.JammedShutterPosition = JammedShutterPosition;
        }

        [DataMember(Name = "position")]
        public CashManagement.PositionEnum? Position { get; init; }

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
        /// * ```jammed``` - The shutter is jammed and is not operational. The _jammedShutterPosition_ provides
        /// the positional state of the shutter.
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
        /// The status of the input or output position.
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
        /// * ```unknown``` - Due to a hardware error or other condition the state of the transport cannot be determined.
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
        /// device it is possible that the transport will not be empty due to a previous dispense operation. 
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
        /// * ```notSupported``` - The physical device has no shutter or the reporting of the position of a jammed 
        /// shutter is not supported.
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
        public StatusClass(IntermediateStackerEnum? IntermediateStacker = null, StackerItemsEnum? StackerItems = null, BanknoteReaderEnum? BanknoteReader = null, bool? DropBox = null, List<PositionClass> Positions = null, MixedModeEnum? MixedMode = null)
        {
            this.IntermediateStacker = IntermediateStacker;
            this.StackerItems = StackerItems;
            this.BanknoteReader = BanknoteReader;
            this.DropBox = DropBox;
            this.Positions = Positions;
            this.MixedMode = MixedMode;
        }

        public enum IntermediateStackerEnum
        {
            Empty,
            NotEmpty,
            Full,
            Unknown,
            NotSupported
        }

        /// <summary>
        /// Supplies the state of the intermediate stacker. Following values are possible:
        /// 
        /// * ```empty``` - The intermediate stacker is empty.
        /// * ```notEmpty``` - The intermediate stacker is not empty.
        /// * ```full``` - The intermediate stacker is full. This may also be reported during a cash-in transaction
        /// where a limit specified by [CashAcceptor.CashInStart](#cashacceptor.cashinstart) has been reached.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the intermediate stacker
        /// cannot be determined.
        /// * ```notSupported``` - The physical device has no intermediate stacker.
        /// </summary>
        [DataMember(Name = "intermediateStacker")]
        public IntermediateStackerEnum? IntermediateStacker { get; init; }

        public enum StackerItemsEnum
        {
            CustomerAccess,
            NoCustomerAccess,
            AccessUnknown,
            NoItems
        }

        /// <summary>
        /// This field informs the application whether items on the intermediate stacker have been in customer access. 
        /// Following values are possible:
        /// 
        /// * ```customerAccess``` - Items on the intermediate stacker have been in customer access. If the device is a 
        /// cash recycler then the items on the intermediate stacker may be there as a result of a previous 
        /// cash-out operation.
        /// * ```noCustomerAccess``` - Items on the intermediate stacker have not been in customer access.
        /// * ```accessUnknown``` - It is not known if the items on the intermediate stacker have been in customer access.
        /// * ```noItems``` - There are no items on the intermediate stacker or the physical device has no intermediate 
        /// stacker.
        /// </summary>
        [DataMember(Name = "stackerItems")]
        public StackerItemsEnum? StackerItems { get; init; }

        public enum BanknoteReaderEnum
        {
            Ok,
            Inoperable,
            Unknown,
            NotSupported
        }

        /// <summary>
        /// Supplies the state of the banknote reader. Following values are possible:
        /// 
        /// * ```ok``` - The banknote reader is in a good state.
        /// * ```inoperable``` - The banknote reader is inoperable.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the banknote reader cannot be
        /// determined.
        /// * ```notSupported``` - The physical device has no banknote reader.
        /// </summary>
        [DataMember(Name = "banknoteReader")]
        public BanknoteReaderEnum? BanknoteReader { get; init; }

        /// <summary>
        /// The drop box is an area within the CashAcceptor where items which have caused a problem during an operation 
        /// are stored. This field specifies the status of the drop box. 
        /// If true, some items are stored in the drop box due to a cash-in transaction which caused a problem.
        /// If false, the drop box is empty or there is no drop box.
        /// </summary>
        [DataMember(Name = "dropBox")]
        public bool? DropBox { get; init; }

        /// <summary>
        /// Array of structures for each position from which items can be accepted.
        /// </summary>
        [DataMember(Name = "positions")]
        public List<PositionClass> Positions { get; init; }

        public enum MixedModeEnum
        {
            NotActive,
            Active
        }

        /// <summary>
        /// Reports if Mixed Media mode is active. Following values are possible:
        /// 
        /// * ```notActive``` - Mixed Media transactions are not supported by the device or Mixed Media mode is not activated.
        /// * ```active``` - Mixed Media mode using the CashAcceptor and ItemProcessor interfaces is activated.
        /// </summary>
        [DataMember(Name = "mixedMode")]
        public MixedModeEnum? MixedMode { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(TypeEnum? Type = null, int? MaxCashInItems = null, bool? Shutter = null, bool? ShutterControl = null, int? IntermediateStacker = null, bool? ItemsTakenSensor = null, bool? ItemsInsertedSensor = null, PositionsClass Positions = null, RetractAreasClass RetractAreas = null, RetractTransportActionsClass RetractTransportActions = null, RetractStackerActionsClass RetractStackerActions = null, CashInLimitClass CashInLimit = null, CountActionsClass CountActions = null, CounterfeitActionEnum? CounterfeitAction = null)
        {
            this.Type = Type;
            this.MaxCashInItems = MaxCashInItems;
            this.Shutter = Shutter;
            this.ShutterControl = ShutterControl;
            this.IntermediateStacker = IntermediateStacker;
            this.ItemsTakenSensor = ItemsTakenSensor;
            this.ItemsInsertedSensor = ItemsInsertedSensor;
            this.Positions = Positions;
            this.RetractAreas = RetractAreas;
            this.RetractTransportActions = RetractTransportActions;
            this.RetractStackerActions = RetractStackerActions;
            this.CashInLimit = CashInLimit;
            this.CountActions = CountActions;
            this.CounterfeitAction = CounterfeitAction;
        }

        public enum TypeEnum
        {
            TellerBill,
            SelfServiceBill,
            TellerCoin,
            SelfServiceCoin
        }

        /// <summary>
        /// Supplies the type of CashAcceptor. Following values are possible:
        /// 
        /// * ```tellerBill``` - The CashAcceptor is a Teller Bill Acceptor.
        /// * ```selfServiceBill``` - The CashAcceptor is a Self-Service Bill Acceptor.
        /// * ```tellerCoin``` - The CashAcceptor is a Teller Coin Acceptor.
        /// * ```selfServiceCoin``` - The CashAcceptor is a Self-Service Coin Acceptor.
        /// </summary>
        [DataMember(Name = "type")]
        public TypeEnum? Type { get; init; }

        /// <summary>
        /// Supplies the maximum number of items that can be accepted in a single 
        /// [CashAcceptor.CashIn](#cashacceptor.cashin) command. This value reflects the hardware limitations of the 
        /// device and therefore it does not change as part of the 
        /// [CashAcceptor.CashInStart](#cashacceptor.cashinstart) command.
        /// </summary>
        [DataMember(Name = "maxCashInItems")]
        [DataTypes(Minimum = 1)]
        public int? MaxCashInItems { get; init; }

        /// <summary>
        /// If true then the device has a shutter and explicit shutter control through the commands 
        /// OpenShutter and CloseShutter is supported. The definition of a shutter will depend on the h/w 
        /// implementation. On some devices where items are automatically detected and accepted then a shutter is simply
        /// a latch that is opened and closed, usually under implicit control by the Service. On other devices, the term
        /// shutter refers to a door, which is opened and closed to allow the customer to place the items onto a tray.
        /// If a Service cannot detect when items are inserted and there is a shutter on the device, then it must 
        /// provide explicit application control of the shutter.
        /// </summary>
        [DataMember(Name = "shutter")]
        public bool? Shutter { get; init; }

        /// <summary>
        /// If true the shutter is controlled implicitly by the service.
        /// 
        /// If false the shutter must be controlled explicitly by the application using the
        /// [CashManagement.OpenShutter](#cashmanagement.openshutter) and
        /// [CashManagement.CloseShutter](#cashmanagement.closeshutter) commands.
        /// 
        /// In either case the [CashAcceptor.PresentMedia](#cashacceptor.presentmedia) command may be used if the 
        /// _presentControl_ property is false.
        /// 
        /// This property is always true if the device has no shutter.
        /// This field applies to all shutters and all positions.
        /// </summary>
        [DataMember(Name = "shutterControl")]
        public bool? ShutterControl { get; init; }

        /// <summary>
        /// Specifies the number of items the intermediate stacker for cash-in can hold. Zero means that there is 
        /// no intermediate stacker for cash-in available.
        /// </summary>
        [DataMember(Name = "intermediateStacker")]
        [DataTypes(Minimum = 0)]
        public int? IntermediateStacker { get; init; }

        /// <summary>
        /// Specifies whether or not the CashAcceptor can detect when items at the exit position are taken by the user. 
        /// If true the Service generates an accompanying [CashManagement.ItemsTakenEvent](#cashmanagement.itemstakenevent). 
        /// If false this event is not generated. 
        /// This field relates to all output positions.
        /// </summary>
        [DataMember(Name = "itemsTakenSensor")]
        public bool? ItemsTakenSensor { get; init; }

        /// <summary>
        /// Specifies whether the CashAcceptor has the ability to detect when items have actually been inserted by 
        /// the user. 
        /// If true the service generates an accompanying [CashManagement.ItemsInsertedEvent](#cashmanagement.itemsinsertedevent). 
        /// If false this event is not generated. 
        /// This field relates to all input positions. 
        /// This flag should not be reported as true unless item insertion can be detected.
        /// </summary>
        [DataMember(Name = "itemsInsertedSensor")]
        public bool? ItemsInsertedSensor { get; init; }

        [DataContract]
        public sealed class PositionsClass
        {
            public PositionsClass(bool? InLeft = null, bool? InRight = null, bool? InCenter = null, bool? InTop = null, bool? InBottom = null, bool? InFront = null, bool? InRear = null, bool? OutLeft = null, bool? OutRight = null, bool? OutCenter = null, bool? OutTop = null, bool? OutBottom = null, bool? OutFront = null, bool? OutRear = null)
            {
                this.InLeft = InLeft;
                this.InRight = InRight;
                this.InCenter = InCenter;
                this.InTop = InTop;
                this.InBottom = InBottom;
                this.InFront = InFront;
                this.InRear = InRear;
                this.OutLeft = OutLeft;
                this.OutRight = OutRight;
                this.OutCenter = OutCenter;
                this.OutTop = OutTop;
                this.OutBottom = OutBottom;
                this.OutFront = OutFront;
                this.OutRear = OutRear;
            }

            /// <summary>
            /// The CashAcceptor has a left input position.
            /// </summary>
            [DataMember(Name = "inLeft")]
            public bool? InLeft { get; init; }

            /// <summary>
            /// The CashAcceptor has a right input position.
            /// </summary>
            [DataMember(Name = "inRight")]
            public bool? InRight { get; init; }

            /// <summary>
            /// The CashAcceptor has a center input position.
            /// </summary>
            [DataMember(Name = "inCenter")]
            public bool? InCenter { get; init; }

            /// <summary>
            /// The CashAcceptor has a top input position.
            /// </summary>
            [DataMember(Name = "inTop")]
            public bool? InTop { get; init; }

            /// <summary>
            /// The CashAcceptor has a bottom input position.
            /// </summary>
            [DataMember(Name = "inBottom")]
            public bool? InBottom { get; init; }

            /// <summary>
            /// The CashAcceptor has a front input position.
            /// </summary>
            [DataMember(Name = "inFront")]
            public bool? InFront { get; init; }

            /// <summary>
            /// The CashAcceptor has a rear input position.
            /// </summary>
            [DataMember(Name = "inRear")]
            public bool? InRear { get; init; }

            /// <summary>
            /// The CashAcceptor has a left output position.
            /// </summary>
            [DataMember(Name = "outLeft")]
            public bool? OutLeft { get; init; }

            /// <summary>
            /// The CashAcceptor has a right output position.
            /// </summary>
            [DataMember(Name = "outRight")]
            public bool? OutRight { get; init; }

            /// <summary>
            /// The CashAcceptor has a center output position.
            /// </summary>
            [DataMember(Name = "outCenter")]
            public bool? OutCenter { get; init; }

            /// <summary>
            /// The CashAcceptor has a top output position.
            /// </summary>
            [DataMember(Name = "outTop")]
            public bool? OutTop { get; init; }

            /// <summary>
            /// The CashAcceptor has a bottom output position.
            /// </summary>
            [DataMember(Name = "outBottom")]
            public bool? OutBottom { get; init; }

            /// <summary>
            /// The CashAcceptor has a front output position.
            /// </summary>
            [DataMember(Name = "outFront")]
            public bool? OutFront { get; init; }

            /// <summary>
            /// The CashAcceptor has a rear output position.
            /// </summary>
            [DataMember(Name = "outRear")]
            public bool? OutRear { get; init; }

        }

        /// <summary>
        /// Specifies the CashAcceptor input and output positions which are available.
        /// </summary>
        [DataMember(Name = "positions")]
        public PositionsClass Positions { get; init; }

        [DataContract]
        public sealed class RetractAreasClass
        {
            public RetractAreasClass(bool? Retract = null, bool? Transport = null, bool? Stacker = null, bool? Reject = null, bool? BillCassette = null, bool? CashIn = null)
            {
                this.Retract = Retract;
                this.Transport = Transport;
                this.Stacker = Stacker;
                this.Reject = Reject;
                this.BillCassette = BillCassette;
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
            /// The items may be retracted to cash-in and recycle storage units.
            /// </summary>
            [DataMember(Name = "billCassette")]
            public bool? BillCassette { get; init; }

            /// <summary>
            /// Items may be retracted to a cash-in storage unit.
            /// </summary>
            [DataMember(Name = "cashIn")]
            public bool? CashIn { get; init; }

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
            public RetractTransportActionsClass(bool? Present = null, bool? Retract = null, bool? Reject = null, bool? BillCassette = null, bool? CashIn = null)
            {
                this.Present = Present;
                this.Retract = Retract;
                this.Reject = Reject;
                this.BillCassette = BillCassette;
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
            /// The items may be moved to the cash-in and recycle storage units.
            /// </summary>
            [DataMember(Name = "billCassette")]
            public bool? BillCassette { get; init; }

            /// <summary>
            /// Items may be retracted to a cash-in storage unit.
            /// </summary>
            [DataMember(Name = "cashIn")]
            public bool? CashIn { get; init; }

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
            public RetractStackerActionsClass(bool? Present = null, bool? Retract = null, bool? Reject = null, bool? BillCassette = null, bool? CashIn = null)
            {
                this.Present = Present;
                this.Retract = Retract;
                this.Reject = Reject;
                this.BillCassette = BillCassette;
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
            /// The items may be moved to the cash-in and recycle storage units.
            /// </summary>
            [DataMember(Name = "billCassette")]
            public bool? BillCassette { get; init; }

            /// <summary>
            /// Items may be retracted to a cash-in storage unit.
            /// </summary>
            [DataMember(Name = "cashIn")]
            public bool? CashIn { get; init; }

        }

        /// <summary>
        /// Specifies the actions which may be performed on items which have been retracted to the stacker. 
        /// If the device does not have the capability to retract items to the stacker or move items from the stacker 
        /// all flags will be set to false.
        /// </summary>
        [DataMember(Name = "retractStackerActions")]
        public RetractStackerActionsClass RetractStackerActions { get; init; }

        [DataContract]
        public sealed class CashInLimitClass
        {
            public CashInLimitClass(bool? ByTotalItems = null, bool? ByAmount = null)
            {
                this.ByTotalItems = ByTotalItems;
                this.ByAmount = ByAmount;
            }

            /// <summary>
            /// The number of successfully processed cash-in items can be limited by specifying the total number of items.
            /// </summary>
            [DataMember(Name = "byTotalItems")]
            public bool? ByTotalItems { get; init; }

            /// <summary>
            /// The number of successfully processed cash-in items can be limited by specifying the maximum amount of a specific currency.
            /// </summary>
            [DataMember(Name = "byAmount")]
            public bool? ByAmount { get; init; }

        }

        /// <summary>
        /// Specifies which cash-in limitations are supported for the
        /// [CashAcceptor.CashInStart](#cashacceptor.cashinstart) command. If the device does not have the capability to
        /// limit the amount or the number of items during cash-in operations this property is omitted.
        /// </summary>
        [DataMember(Name = "cashInLimit")]
        public CashInLimitClass CashInLimit { get; init; }

        [DataContract]
        public sealed class CountActionsClass
        {
            public CountActionsClass(bool? Individual = null, bool? All = null)
            {
                this.Individual = Individual;
                this.All = All;
            }

            /// <summary>
            /// The counting of individual cash units is supported.
            /// </summary>
            [DataMember(Name = "individual")]
            public bool? Individual { get; init; }

            /// <summary>
            /// The counting of all cash units is supported.
            /// </summary>
            [DataMember(Name = "all")]
            public bool? All { get; init; }

        }

        /// <summary>
        /// Specifies the count action supported by the [CashAcceptor.CashUnitCount](#cashacceptor.cashunitcount)
        /// command. If the device does not support counting then this property is omitted.
        /// </summary>
        [DataMember(Name = "countActions")]
        public CountActionsClass CountActions { get; init; }

        public enum CounterfeitActionEnum
        {
            None,
            Level2,
            Level23
        }

        /// <summary>
        /// Specifies whether level 2/3 notes (see 
        /// [NoteClassification](#cashmanagement.generalinformation.noteclassification)) are allowed to be returned
        /// to the customer during a cash-in transaction. If notes are not to be returned to the customer by 
        /// these rules, they will not be returned regardless of whether their specific note type is configured to not 
        /// be accepted by [CashAcceptor.ConfigureNoteTypes](#cashacceptor.configurenotetypes). Following rules are 
        /// possible:
        /// 
        /// * ```none``` - The device is not able to classify notes as level 1, 2, 3 or 4.
        /// * ```level2``` - Notes are classified as level 1, 2, 3 or 4 and only level 2 notes will not be returned to 
        /// the customer in a cash-in transaction.
        /// * ```level23``` - Notes are classified as level 1, 2, 3 or 4 and level 2 and level 3 notes will not be 
        /// returned to the customer in a cash-in transaction.
        /// </summary>
        [DataMember(Name = "counterfeitAction")]
        public CounterfeitActionEnum? CounterfeitAction { get; init; }

    }


}
