/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
        public PositionClass(CashManagement.PositionEnum? Position = null, ShutterEnum? Shutter = null, PositionStatusEnum? PositionStatus = null, TransportEnum? Transport = null, TransportStatusEnum? TransportStatus = null)
        {
            this.Position = Position;
            this.Shutter = Shutter;
            this.PositionStatus = PositionStatus;
            this.Transport = Transport;
            this.TransportStatus = TransportStatus;
        }

        [DataMember(Name = "position")]
        public CashManagement.PositionEnum? Position { get; init; }

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
        /// * ```closed``` - The shutter is operational and is fully closed.
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
            Unknown,
            ForeignItems
        }

        /// <summary>
        /// The status of the input or output position. This property is null in [Common.Status](#common.status) if
        /// the device is not capable of reporting whether items are at the position, otherwise the following values are
        /// possible:
        /// 
        /// * ```empty``` - The position is empty.
        /// * ```notEmpty``` - The position is not empty.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the position cannot be determined.
        /// * ```foreignItems``` - Foreign items have been detected in the position.
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
        /// Supplies the state of the transport mechanism. The transport is defined as any area leading to or from the
        /// position. This property is null in [Common.Status](#common.status) if the device has no transport or
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
        /// device it is possible that the transport will not be empty due to a previous dispense operation. 
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
        public StatusClass(IntermediateStackerEnum? IntermediateStacker = null, StackerItemsEnum? StackerItems = null, BanknoteReaderEnum? BanknoteReader = null, bool? DropBox = null, List<PositionClass> Positions = null)
        {
            this.IntermediateStacker = IntermediateStacker;
            this.StackerItems = StackerItems;
            this.BanknoteReader = BanknoteReader;
            this.DropBox = DropBox;
            this.Positions = Positions;
        }

        public enum IntermediateStackerEnum
        {
            Empty,
            NotEmpty,
            Full,
            Unknown
        }

        /// <summary>
        /// Supplies the state of the intermediate stacker. This property is null in [Common.Status](#common.status) if
        /// the physical device has no intermediate stacker, otherwise the following values are possible:
        /// 
        /// * ```empty``` - The intermediate stacker is empty.
        /// * ```notEmpty``` - The intermediate stacker is not empty.
        /// * ```full``` - The intermediate stacker is full. This may also be reported during a cash-in transaction
        /// where a limit specified by [CashAcceptor.CashInStart](#cashacceptor.cashinstart) has been reached.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the intermediate stacker
        /// cannot be determined.
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
        /// This property informs the application whether items on the intermediate stacker have been in customer access. 
        /// This property is null in [Common.Status](#common.status) if the physical device has no intermediate stacker,
        /// otherwise the following values are possible:
        /// 
        /// * ```customerAccess``` - Items on the intermediate stacker have been in customer access. If the device is a 
        /// cash recycler then the items on the intermediate stacker may be there as a result of a previous 
        /// cash-out operation.
        /// * ```noCustomerAccess``` - Items on the intermediate stacker have not been in customer access.
        /// * ```accessUnknown``` - It is not known if the items on the intermediate stacker have been in customer access.
        /// * ```noItems``` - There are no items on the intermediate stacker.
        /// </summary>
        [DataMember(Name = "stackerItems")]
        public StackerItemsEnum? StackerItems { get; init; }

        public enum BanknoteReaderEnum
        {
            Ok,
            Inoperable,
            Unknown
        }

        /// <summary>
        /// Supplies the state of the banknote reader. This property is null in [Common.Status](#common.status) if the
        /// physical device has no banknote reader, otherwise the following values are possible:
        /// 
        /// * ```ok``` - The banknote reader is in a good state.
        /// * ```inoperable``` - The banknote reader is inoperable.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the banknote reader cannot be
        /// determined.
        /// </summary>
        [DataMember(Name = "banknoteReader")]
        public BanknoteReaderEnum? BanknoteReader { get; init; }

        /// <summary>
        /// The drop box is an area within the Cash Acceptor where items which have caused a problem during an operation 
        /// are stored. This property specifies the status of the drop box. 
        /// If true, some items are stored in the drop box due to a cash-in transaction which caused a problem.
        /// If false, the drop box is empty or there is no drop box.
        /// This property may be null if there is no drop box or its state has not changed in
        /// [Common.StatusChangedEvent](#common.statuschangedevent).
        /// <example>true</example>
        /// </summary>
        [DataMember(Name = "dropBox")]
        public bool? DropBox { get; init; }

        /// <summary>
        /// Array of structures reporting status for each position from which items can be accepted. This may be null
        /// in [Common.StatusChangedEvent](#common.statuschangedevent) if no position states have changed.
        /// </summary>
        [DataMember(Name = "positions")]
        public List<PositionClass> Positions { get; init; }

    }


    [DataContract]
    public sealed class PosCapsClass
    {
        public PosCapsClass(CashManagement.PositionEnum? Position = null, UsageClass Usage = null, bool? ShutterControl = null, bool? ItemsTakenSensor = null, bool? ItemsInsertedSensor = null, RetractAreasClass RetractAreas = null, bool? PresentControl = null, bool? PreparePresent = null)
        {
            this.Position = Position;
            this.Usage = Usage;
            this.ShutterControl = ShutterControl;
            this.ItemsTakenSensor = ItemsTakenSensor;
            this.ItemsInsertedSensor = ItemsInsertedSensor;
            this.RetractAreas = RetractAreas;
            this.PresentControl = PresentControl;
            this.PreparePresent = PreparePresent;
        }

        [DataMember(Name = "position")]
        public CashManagement.PositionEnum? Position { get; init; }

        [DataContract]
        public sealed class UsageClass
        {
            public UsageClass(bool? In = null, bool? Refuse = null, bool? Rollback = null)
            {
                this.In = In;
                this.Refuse = Refuse;
                this.Rollback = Rollback;
            }

            /// <summary>
            /// It is an input position.
            /// </summary>
            [DataMember(Name = "in")]
            public bool? In { get; init; }

            /// <summary>
            /// It is an output position used to refuse items.
            /// </summary>
            [DataMember(Name = "refuse")]
            public bool? Refuse { get; init; }

            /// <summary>
            /// It is an output position used to rollback items.
            /// </summary>
            [DataMember(Name = "rollback")]
            public bool? Rollback { get; init; }

        }

        /// <summary>
        /// Indicates if a position is used to input, reject or rollback.
        /// </summary>
        [DataMember(Name = "usage")]
        public UsageClass Usage { get; init; }

        /// <summary>
        /// If true the shutter is controlled implicitly by the Service.
        /// 
        /// If false the shutter must be controlled explicitly by the application using the 
        /// [CashManagement.OpenShutter](#cashmanagement.openshutter) and 
        /// [CashManagement.CloseShutter](#cashmanagement.closeshutter) commands.
        /// 
        /// In either case the [CashAcceptor.PresentMedia](#cashacceptor.presentmedia) command may be used if
        /// *presentControl* is false. The *shutterControl* property is always true if the described position has no 
        /// shutter.
        /// </summary>
        [DataMember(Name = "shutterControl")]
        public bool? ShutterControl { get; init; }

        /// <summary>
        /// Specifies whether or not the described position can detect when items at the exit position are taken by the user.
        /// 
        /// If true the service generates an accompanying
        /// [CashManagement.ItemsTakenEvent](#cashmanagement.itemstakenevent). If false this event is not generated. 
        /// 
        /// This property relates to output and refused positions.
        /// </summary>
        [DataMember(Name = "itemsTakenSensor")]
        public bool? ItemsTakenSensor { get; init; }

        /// <summary>
        /// Specifies whether the described position has the ability to detect when items have been inserted by the user. 
        /// 
        /// If true the service generates an accompanying [CashManagement.ItemsInsertedEvent](#cashmanagement.itemsinsertedevent). 
        /// If false this event is not generated. 
        /// 
        /// This property relates to all input positions.
        /// </summary>
        [DataMember(Name = "itemsInsertedSensor")]
        public bool? ItemsInsertedSensor { get; init; }

        [DataContract]
        public sealed class RetractAreasClass
        {
            public RetractAreasClass(bool? Retract = null, bool? Reject = null, bool? Transport = null, bool? Stacker = null, bool? BillCassettes = null, bool? CashIn = null)
            {
                this.Retract = Retract;
                this.Reject = Reject;
                this.Transport = Transport;
                this.Stacker = Stacker;
                this.BillCassettes = BillCassettes;
                this.CashIn = CashIn;
            }

            /// <summary>
            /// Items may be retracted to a retract storage unit.
            /// </summary>
            [DataMember(Name = "retract")]
            public bool? Retract { get; init; }

            /// <summary>
            /// Items may be retracted to a reject storage unit.
            /// </summary>
            [DataMember(Name = "reject")]
            public bool? Reject { get; init; }

            /// <summary>
            /// Items may be retracted to the transport.
            /// </summary>
            [DataMember(Name = "transport")]
            public bool? Transport { get; init; }

            /// <summary>
            /// Items may be retracted to the intermediate stacker.
            /// </summary>
            [DataMember(Name = "stacker")]
            public bool? Stacker { get; init; }

            /// <summary>
            /// Items may be retracted to item cassettes, i.e. cash-in and recycle storage units.
            /// </summary>
            [DataMember(Name = "billCassettes")]
            public bool? BillCassettes { get; init; }

            /// <summary>
            /// Items may be retracted to a cash-in storage unit.
            /// </summary>
            [DataMember(Name = "cashIn")]
            public bool? CashIn { get; init; }

        }

        /// <summary>
        /// Specifies the areas to which items may be retracted from this position. This is not reported if the device 
        /// cannot retract.
        /// </summary>
        [DataMember(Name = "retractAreas")]
        public RetractAreasClass RetractAreas { get; init; }

        /// <summary>
        /// Specifies how the presenting of media items is controlled. 
        /// 
        /// If true then the [CashAcceptor.PresentMedia](#cashacceptor.presentmedia) command is not supported and items 
        /// are moved to the output position for removal as part of the relevant command, e.g. *CashAcceptor.CashIn* or 
        /// *CashAcceptor.CashInRollback* where there is implicit shutter control. 
        /// 
        /// If false then items returned or rejected can be moved to the output position using the 
        /// *CashAcceptor.PresentMedia* command, this includes items returned or rejected as part of a 
        /// *CashAcceptor.CashIn* or *CashAcceptor.CashInRollback* operation. The *CashAcceptor.PresentMedia*
        /// command will open and close the shutter implicitly.
        /// </summary>
        [DataMember(Name = "presentControl")]
        public bool? PresentControl { get; init; }

        /// <summary>
        /// Specifies how the presenting of items is controlled. 
        /// 
        /// If false then items to be removed are moved to the output position as part of the relevant command. e.g.
        /// *CashManagement.OpenShutter*, *CashAcceptor.PresentMedia* or *CashAcceptor.CashInRollback*.
        /// 
        /// If true then items are moved to the output position using the [CashAcceptor.PreparePresent](#cashacceptor.preparepresent)
        /// command.
        /// </summary>
        [DataMember(Name = "preparePresent")]
        public bool? PreparePresent { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(TypeEnum? Type = null, int? MaxCashInItems = null, bool? Shutter = null, bool? ShutterControl = null, int? IntermediateStacker = null, bool? ItemsTakenSensor = null, bool? ItemsInsertedSensor = null, List<PosCapsClass> Positions = null, RetractAreasClass RetractAreas = null, RetractTransportActionsClass RetractTransportActions = null, RetractStackerActionsClass RetractStackerActions = null, CashInLimitClass CashInLimit = null, CountActionsClass CountActions = null, RetainActionClass RetainAction = null)
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
            this.RetainAction = RetainAction;
        }

        public enum TypeEnum
        {
            TellerBill,
            SelfServiceBill,
            TellerCoin,
            SelfServiceCoin
        }

        /// <summary>
        /// Supplies the type of CashAcceptor. The following values are possible:
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
        /// [CashManagement.OpenShutter](#cashmanagement.openshutter) and [CashManagement.CloseShutter](#cashmanagement.closeshutter) is supported. The definition of a shutter will depend on the h/w 
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
        /// *presentControl* property is false.
        /// 
        /// This property is always true if the device has no shutter.
        /// This property applies to all shutters and all positions.
        /// </summary>
        [DataMember(Name = "shutterControl")]
        public bool? ShutterControl { get; init; }

        /// <summary>
        /// Specifies the number of items the intermediate stacker for cash-in can hold. Will be null if there is
        /// no intermediate stacker for cash-in available.
        /// </summary>
        [DataMember(Name = "intermediateStacker")]
        [DataTypes(Minimum = 0)]
        public int? IntermediateStacker { get; init; }

        /// <summary>
        /// Specifies whether the CashAcceptor can detect when items at the exit position are taken by the user. 
        /// If true the Service generates an accompanying [CashManagement.ItemsTakenEvent](#cashmanagement.itemstakenevent). 
        /// If false this event is not generated. 
        /// This property relates to all output positions.
        /// </summary>
        [DataMember(Name = "itemsTakenSensor")]
        public bool? ItemsTakenSensor { get; init; }

        /// <summary>
        /// Specifies whether the CashAcceptor has the ability to detect when items have been inserted by 
        /// the user. 
        /// If true the service generates an accompanying [CashManagement.ItemsInsertedEvent](#cashmanagement.itemsinsertedevent). 
        /// If false this event is not generated. 
        /// This relates to all input positions and should not be reported as true unless item insertion can be detected.
        /// </summary>
        [DataMember(Name = "itemsInsertedSensor")]
        public bool? ItemsInsertedSensor { get; init; }

        /// <summary>
        /// Array of position capabilities for all positions configured in this service.
        /// </summary>
        [DataMember(Name = "positions")]
        public List<PosCapsClass> Positions { get; init; }

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
            /// Items may be retracted to cash-in storage units.
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
            /// Items may be retracted to cash-in storage units.
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
            /// Items may be retracted to cash-in storage units.
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
        /// limit the amount or the number of items during cash-in operations this property is null.
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
            /// The counting of individual storage units is supported.
            /// </summary>
            [DataMember(Name = "individual")]
            public bool? Individual { get; init; }

            /// <summary>
            /// The counting of all storage units is supported.
            /// </summary>
            [DataMember(Name = "all")]
            public bool? All { get; init; }

        }

        /// <summary>
        /// Specifies the count action supported by the [CashAcceptor.CashUnitCount](#cashacceptor.cashunitcount)
        /// command. If the device does not support counting then this property is null.
        /// </summary>
        [DataMember(Name = "countActions")]
        public CountActionsClass CountActions { get; init; }

        [DataContract]
        public sealed class RetainActionClass
        {
            public RetainActionClass(bool? Counterfeit = null, bool? Suspect = null, bool? Inked = null)
            {
                this.Counterfeit = Counterfeit;
                this.Suspect = Suspect;
                this.Inked = Inked;
            }

            /// <summary>
            /// Items classified as counterfeit are retained during a cash-in transaction.
            /// </summary>
            [DataMember(Name = "counterfeit")]
            public bool? Counterfeit { get; init; }

            /// <summary>
            /// Items classified as suspect are retained during a cash-in transaction.
            /// </summary>
            [DataMember(Name = "suspect")]
            public bool? Suspect { get; init; }

            /// <summary>
            /// Items classified as inked are retained during a cash-in transaction.
            /// </summary>
            [DataMember(Name = "inked")]
            public bool? Inked { get; init; }

        }

        /// <summary>
        /// If [counterfeit, inked or suspect](#cashmanagement.generalinformation.noteclassification) items
        /// are supported by the Service (see 
        /// [classifications](#common.capabilities.completion.description.cashmanagement.classifications)), this
        /// specifies whether such items are retained by the device if detected during a cash-in
        /// transaction. See [acceptor](#common.status.completion.description.cashmanagement.acceptor) for details of
        /// the impact on offering cash-in transactions if unable to retain items due to storage unit status.
        /// 
        /// This applies regardless of whether their specific note type is configured to not 
        /// be accepted by [CashAcceptor.ConfigureNoteTypes](#cashacceptor.configurenotetypes).
        /// 
        /// This property may be null if none of these note classifications are supported.
        /// </summary>
        [DataMember(Name = "retainAction")]
        public RetainActionClass RetainAction { get; init; }

    }


}
