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
    public sealed class InposClass
    {
        public InposClass(PositionEnum? Position = null, ShutterEnum? Shutter = null, PositionStatusEnum? PositionStatus = null, TransportEnum? Transport = null, TransportStatusEnum? TransportStatus = null, JammedShutterPositionEnum? JammedShutterPosition = null)
        {
            this.Position = Position;
            this.Shutter = Shutter;
            this.PositionStatus = PositionStatus;
            this.Transport = Transport;
            this.TransportStatus = TransportStatus;
            this.JammedShutterPosition = JammedShutterPosition;
        }

        public enum PositionEnum
        {
            InLeft,
            InRight,
            InCenter,
            InTop,
            InBottom,
            InFront,
            InRear,
            OutLeft,
            OutRight,
            OutCenter,
            OutTop,
            OutBottom,
            OutFront,
            OutRear
        }

        /// <summary>
        /// Supplies the input or output position as one of the following values:
        /// 
        /// \"inLeft\": Left input position.
        /// 
        /// \"inRight\": Right input position.
        /// 
        /// \"inCenter\": Center input position.
        /// 
        /// \"inTop\": Top input position.
        /// 
        /// \"inBottom\": Bottom input position.
        /// 
        /// \"inFront\": Front input position.
        /// 
        /// \"inRear\": Rear input position.
        /// 
        /// \"outLeft\": Left output position.
        /// 
        /// \"outRight\": Right output position.
        /// 
        /// \"outCenter\": Center output position.
        /// 
        /// \"outTop\": Top output position.
        /// 
        /// \"outBottom\": Bottom output position.
        /// 
        /// \"outFront\": Front output position.
        /// 
        /// \"outRear\": Rear output position.
        /// </summary>
        [DataMember(Name = "position")]
        public PositionEnum? Position { get; private set; }

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
        /// \"closed\": The shutter is operational and is closed.
        /// 
        /// \"open\": The shutter is operational and is open.
        /// 
        /// \"jammed\": The shutter is jammed and is not operational. The field jammedShutterPosition provides the positional state of the shutter.
        /// 
        /// \"unknown\": Due to a hardware error or other condition, the state of the shutter cannot be determined.
        /// 
        /// \"notSupported\": The physical device has no shutter or shutter state reporting is not supported.
        /// </summary>
        [DataMember(Name = "shutter")]
        public ShutterEnum? Shutter { get; private set; }

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
        /// \"empty\": The output position is empty.
        /// 
        /// \"notEmpty\": The output position is not empty.
        /// 
        /// \"unknown\": Due to a hardware error or other condition, the state of the output position cannot be determined.
        /// 
        /// \"notSupported\": The device is not capable of reporting whether or not items are at the output position.
        /// </summary>
        [DataMember(Name = "positionStatus")]
        public PositionStatusEnum? PositionStatus { get; private set; }

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
        /// \"ok\": The transport is in a good state.
        /// 
        /// \"inoperative\": The transport is inoperative due to a hardware failure or media jam.
        /// 
        /// \"unknown\":Due to a hardware error or other condition the state of the transport cannot be determined.
        /// 
        /// \"notSupported\": The physical device has no transport or transport state reporting is not supported.
        /// </summary>
        [DataMember(Name = "transport")]
        public TransportEnum? Transport { get; private set; }

        public enum TransportStatusEnum
        {
            Empty,
            NotEmpty,
            NotEmptyCustomer,
            NotEmptyUnkown,
            NotSupported
        }

        /// <summary>
        /// Returns information regarding items which may be on the transport. If the device is a recycler 
        /// device it is possible that the transport will not be empty due to a previous dispense operation. 
        /// Following values are possible:
        /// 
        /// \"empty\": The transport is empty.
        /// 
        /// \"notEmpty\": The transport is not empty.
        /// 
        /// \"notEmptyCustomer\": Items which a customer has had access to are on the transport.
        /// 
        /// \"notEmptyUnkown\": Due to a hardware error or other condition it is not known whether there are items on the transport.
        /// 
        /// \"notSupported\": The device is not capable of reporting whether items are on the transport.
        /// </summary>
        [DataMember(Name = "transportStatus")]
        public TransportStatusEnum? TransportStatus { get; private set; }

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
        /// \"notSupported\": The physical device has no shutter or the reporting of the position of a jammed shutter is not supported.
        /// 
        /// \"notJammed\": The shutter is not jammed.
        /// 
        /// \"open\": The shutter is jammed, but fully open.
        /// 
        /// \"partiallyOpen\": The shutter is jammed, but partially open.
        /// 
        /// \"closed\": The shutter is jammed, but fully closed.
        /// 
        /// \"unknown\": The position of the shutter is unknown.
        /// </summary>
        [DataMember(Name = "jammedShutterPosition")]
        public JammedShutterPositionEnum? JammedShutterPosition { get; private set; }

    }


    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(IntermediateStackerEnum? IntermediateStacker = null, StackerItemsEnum? StackerItems = null, BanknoteReaderEnum? BanknoteReader = null, bool? DropBox = null, List<InposClass> Positions = null, MixedModeEnum? MixedMode = null)
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
        /// \"empty\": The intermediate stacker is empty.
        /// 
        /// \"notEmpty\": The intermediate stacker is not empty.
        /// 
        /// \"full\": The intermediate stacker is full. This may also be reported during a cash-in transaction where a limit specified by CashAcceptor.SetCashInLimit has been reached.
        /// 
        /// \"unknown\": Due to a hardware error or other condition, the state of the intermediate stacker cannot be determined.
        /// 
        /// \"notSupported\": The physical device has no intermediate stacker.
        /// </summary>
        [DataMember(Name = "intermediateStacker")]
        public IntermediateStackerEnum? IntermediateStacker { get; private set; }

        public enum StackerItemsEnum
        {
            CustomerAccess,
            NoCustomerAccess,
            AccessUnknown,
            NoItems
        }

        /// <summary>
        /// This field informs the application whether items on the intermediate stacker have been in customer access. Following values are possible:
        /// 
        /// \"customerAccess\": Items on the intermediate stacker have been in customer access. If the device is a cash recycler then the items on the intermediate stacker may be there as a result of a previous cash-out operation.
        /// 
        /// \"noCustomerAccess\": Items on the intermediate stacker have not been in customer access.
        /// 
        /// \"accessUnknown\": It is not known if the items on the intermediate stacker have been in customer access.
        /// 
        /// \"noItems\": There are no items on the intermediate stacker or the physical device has no intermediate stacker.
        /// </summary>
        [DataMember(Name = "stackerItems")]
        public StackerItemsEnum? StackerItems { get; private set; }

        public enum BanknoteReaderEnum
        {
            CustomokerAccess,
            Inoperable,
            Unknown,
            NotSupported
        }

        /// <summary>
        /// Supplies the state of the banknote reader. Following values are possible:
        /// 
        /// \"ok\": The banknote reader is in a good state.
        /// 
        /// \"inoperable\": The banknote reader is inoperable.
        /// 
        /// \"unknown\": Due to a hardware error or other condition, the state of the banknote reader cannot be determined.
        /// 
        /// \"notSupported\": The physical device has no banknote reader.
        /// </summary>
        [DataMember(Name = "banknoteReader")]
        public BanknoteReaderEnum? BanknoteReader { get; private set; }

        /// <summary>
        /// The drop box is an area within the CashAcceptor where items which have caused a problem during an operation are stored. 
        /// This field specifies the status of the drop box. TRUE means that some items are stored in the drop box due to a cash-in 
        /// transaction which caused a problem. FALSE indicates that the drop box is empty.
        /// </summary>
        [DataMember(Name = "dropBox")]
        public bool? DropBox { get; private set; }

        /// <summary>
        /// Array of structures for each position from which items can be accepted.
        /// </summary>
        [DataMember(Name = "positions")]
        public List<InposClass> Positions { get; private set; }

        public enum MixedModeEnum
        {
            NotActive,
            Active
        }

        /// <summary>
        /// Reports if Mixed Media mode is active. Following values are possible:
        /// 
        /// \"notActive\": Mixed Media transactions are not supported by the device or Mixed Media mode is not activated.
        /// 
        /// \"active\": Mixed Media mode using the CashAcceptor and ItemProcessor interfaces is activated.
        /// </summary>
        [DataMember(Name = "mixedMode")]
        public MixedModeEnum? MixedMode { get; private set; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(TypeEnum? Type = null, int? MaxCashInItems = null, bool? Shutter = null, bool? ShutterControl = null, int? IntermediateStacker = null, bool? ItemsTakenSensor = null, bool? ItemsInsertedSensor = null, PositionsClass Positions = null, RetractAreasClass RetractAreas = null, RetractTransportActionsClass RetractTransportActions = null, RetractStackerActionsClass RetractStackerActions = null, bool? CompareSignatures = null, bool? Replenish = null, CashInLimitClass CashInLimit = null, CountActionsClass CountActions = null, bool? DeviceLockControl = null, bool? MixedMode = null, bool? MixedDepositAndRollback = null, bool? Deplete = null, CounterfeitActionEnum? CounterfeitAction = null)
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
            this.CompareSignatures = CompareSignatures;
            this.Replenish = Replenish;
            this.CashInLimit = CashInLimit;
            this.CountActions = CountActions;
            this.DeviceLockControl = DeviceLockControl;
            this.MixedMode = MixedMode;
            this.MixedDepositAndRollback = MixedDepositAndRollback;
            this.Deplete = Deplete;
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
        /// \"tellerBill\": The CashAcceptor is a Teller Bill Acceptor.
        /// 
        /// \"selfServiceBill\": The CashAcceptor is a Self-Service Bill Acceptor.
        /// 
        /// \"tellerCoin\": The CashAcceptor is a Teller Coin Acceptor.
        /// 
        /// \"selfServiceCoin\": The CashAcceptor is a Self-Service Coin Acceptor.
        /// </summary>
        [DataMember(Name = "type")]
        public TypeEnum? Type { get; private set; }

        /// <summary>
        /// Supplies the maximum number of items that can be accepted in a single CashAcceptor.CashIn command. This value reflects the hardware limitations of the device and therefore it does not change as part of the CashAcceptor.SetCashInLimit command.
        /// </summary>
        [DataMember(Name = "maxCashInItems")]
        public int? MaxCashInItems { get; private set; }

        /// <summary>
        /// If this flag is TRUE then the device has a shutter and explicit shutter control through the commands OpenShutter and CloseShutter is supported. The definition of a shutter will depend on the h/w implementation. On some devices where items are automatically detected and accepted then a shutter is simply a latch that is opened and closed, usually under implicit control by the Service. On other devices, the term shutter refers to a door, which is opened and closed to allow the customer to place the items onto a tray. If a Service cannot detect when items are inserted and there is a shutter on the device, then it must provide explicit application control of the shutter.
        /// </summary>
        [DataMember(Name = "shutter")]
        public bool? Shutter { get; private set; }

        /// <summary>
        /// If set to TRUE the shutter is controlled implicitly by the Service. If set to FALSE the shutter must be controlled explicitly by the application using the OpenShutter and the CloseShutter commands. In either case the PresentMedia command may be used if the presentControl field is reported as FALSE. The shutterControl field is always set to TRUE if the device has no shutter. This field applies to all shutters and all positions.
        /// </summary>
        [DataMember(Name = "shutterControl")]
        public bool? ShutterControl { get; private set; }

        /// <summary>
        /// Specifies the number of items the intermediate stacker for cash-in can hold. Zero means that there is no intermediate stacker for cash-in available.
        /// </summary>
        [DataMember(Name = "intermediateStacker")]
        public int? IntermediateStacker { get; private set; }

        /// <summary>
        /// Specifies whether or not the CashAcceptor can detect when items at the exit position are taken by the user. If set to TRUE the Service generates an accompanying CashAcceptor.ItemsTaken event. If set to FALSE this event is not generated. This field relates to all output positions.
        /// </summary>
        [DataMember(Name = "itemsTakenSensor")]
        public bool? ItemsTakenSensor { get; private set; }

        /// <summary>
        /// Specifies whether the CashAcceptor has the ability to detect when items have actually been inserted by the user. If set to TRUE the Service generates an accompanying CashAcceptor.ItemsInserted event. If set to FALSE this event is not generated. This field relates to all input positions. This flag should not be reported as TRUE unless item insertion can be detected.
        /// </summary>
        [DataMember(Name = "itemsInsertedSensor")]
        public bool? ItemsInsertedSensor { get; private set; }

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
            public bool? InLeft { get; private set; }

            /// <summary>
            /// The CashAcceptor has a right input position.
            /// </summary>
            [DataMember(Name = "inRight")]
            public bool? InRight { get; private set; }

            /// <summary>
            /// The CashAcceptor has a center input position.
            /// </summary>
            [DataMember(Name = "inCenter")]
            public bool? InCenter { get; private set; }

            /// <summary>
            /// The CashAcceptor has a top input position.
            /// </summary>
            [DataMember(Name = "inTop")]
            public bool? InTop { get; private set; }

            /// <summary>
            /// The CashAcceptor has a bottom input position.
            /// </summary>
            [DataMember(Name = "inBottom")]
            public bool? InBottom { get; private set; }

            /// <summary>
            /// The CashAcceptor has a front input position.
            /// </summary>
            [DataMember(Name = "inFront")]
            public bool? InFront { get; private set; }

            /// <summary>
            /// The CashAcceptor has a rear input position.
            /// </summary>
            [DataMember(Name = "inRear")]
            public bool? InRear { get; private set; }

            /// <summary>
            /// The CashAcceptor has a left output position.
            /// </summary>
            [DataMember(Name = "outLeft")]
            public bool? OutLeft { get; private set; }

            /// <summary>
            /// The CashAcceptor has a right output position.
            /// </summary>
            [DataMember(Name = "outRight")]
            public bool? OutRight { get; private set; }

            /// <summary>
            /// The CashAcceptor has a center output position.
            /// </summary>
            [DataMember(Name = "outCenter")]
            public bool? OutCenter { get; private set; }

            /// <summary>
            /// The CashAcceptor has a top output position.
            /// </summary>
            [DataMember(Name = "outTop")]
            public bool? OutTop { get; private set; }

            /// <summary>
            /// The CashAcceptor has a bottom output position.
            /// </summary>
            [DataMember(Name = "outBottom")]
            public bool? OutBottom { get; private set; }

            /// <summary>
            /// The CashAcceptor has a front output position.
            /// </summary>
            [DataMember(Name = "outFront")]
            public bool? OutFront { get; private set; }

            /// <summary>
            /// The CashAcceptor has a rear output position.
            /// </summary>
            [DataMember(Name = "outRear")]
            public bool? OutRear { get; private set; }

        }

        /// <summary>
        /// Specifies the CashAcceptor input and output positions which are available.
        /// </summary>
        [DataMember(Name = "positions")]
        public PositionsClass Positions { get; private set; }

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
            /// The items may be retracted to a retract cash unit.
            /// </summary>
            [DataMember(Name = "retract")]
            public bool? Retract { get; private set; }

            /// <summary>
            /// The items may be retracted to the transport.
            /// </summary>
            [DataMember(Name = "transport")]
            public bool? Transport { get; private set; }

            /// <summary>
            /// The items may be retracted to the intermediate stacker.
            /// </summary>
            [DataMember(Name = "stacker")]
            public bool? Stacker { get; private set; }

            /// <summary>
            /// The items may be retracted to a reject cash unit.
            /// </summary>
            [DataMember(Name = "reject")]
            public bool? Reject { get; private set; }

            /// <summary>
            /// The items may be retracted to the item cassettes, i.e. cash-in and recycle cash units.
            /// </summary>
            [DataMember(Name = "billCassette")]
            public bool? BillCassette { get; private set; }

            /// <summary>
            /// Items may be retracted to a cash-in cash unit.
            /// </summary>
            [DataMember(Name = "cashIn")]
            public bool? CashIn { get; private set; }

        }

        /// <summary>
        /// Specifies the area to which items may be retracted. 
        /// If the device does not have a retract capability all flags will be set to false.
        /// </summary>
        [DataMember(Name = "retractAreas")]
        public RetractAreasClass RetractAreas { get; private set; }

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
            public bool? Present { get; private set; }

            /// <summary>
            /// The items may be moved to a retract cash unit.
            /// </summary>
            [DataMember(Name = "retract")]
            public bool? Retract { get; private set; }

            /// <summary>
            /// The items may be moved to a reject bin.
            /// </summary>
            [DataMember(Name = "reject")]
            public bool? Reject { get; private set; }

            /// <summary>
            /// The items may be moved to the item cassettes, i.e. cash-in and recycle cash units.
            /// </summary>
            [DataMember(Name = "billCassette")]
            public bool? BillCassette { get; private set; }

            /// <summary>
            /// Items may be retracted to a cash-in cash unit.
            /// </summary>
            [DataMember(Name = "cashIn")]
            public bool? CashIn { get; private set; }

        }

        /// <summary>
        /// Specifies the actions which may be performed on items which have been retracted to the transport. 
        /// If the device does not have the capability to retract items to the transport or move items from the 
        /// transport all flags will be set to false.
        /// </summary>
        [DataMember(Name = "retractTransportActions")]
        public RetractTransportActionsClass RetractTransportActions { get; private set; }

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
            public bool? Present { get; private set; }

            /// <summary>
            /// The items may be moved to a retract cash unit.
            /// </summary>
            [DataMember(Name = "retract")]
            public bool? Retract { get; private set; }

            /// <summary>
            /// The items may be moved to a reject bin.
            /// </summary>
            [DataMember(Name = "reject")]
            public bool? Reject { get; private set; }

            /// <summary>
            /// The items may be moved to the item cassettes, i.e. cash-in and recycle cash units.
            /// </summary>
            [DataMember(Name = "billCassette")]
            public bool? BillCassette { get; private set; }

            /// <summary>
            /// Items may be retracted to a cash-in cash unit.
            /// </summary>
            [DataMember(Name = "cashIn")]
            public bool? CashIn { get; private set; }

        }

        /// <summary>
        /// Specifies the actions which may be performed on items which have been retracted to the stacker. 
        /// If the device does not have the capability to retract items to the stacker or move items from the stacker 
        /// all flags will be set to false.
        /// </summary>
        [DataMember(Name = "retractStackerActions")]
        public RetractStackerActionsClass RetractStackerActions { get; private set; }

        /// <summary>
        /// Specifies if the Service has the ability to compare signatures through command CashAcceptor.CompareP6Signature.
        /// </summary>
        [DataMember(Name = "compareSignatures")]
        public bool? CompareSignatures { get; private set; }

        /// <summary>
        /// If set to TRUE the CashAcceptor.ReplenishTarget and CashAcceptor.Replenish commands are supported.
        /// </summary>
        [DataMember(Name = "replenish")]
        public bool? Replenish { get; private set; }

        [DataContract]
        public sealed class CashInLimitClass
        {
            public CashInLimitClass(bool? ByTotalItems = null, bool? ByAmount = null, bool? Multiple = null, bool? RefuseOther = null)
            {
                this.ByTotalItems = ByTotalItems;
                this.ByAmount = ByAmount;
                this.Multiple = Multiple;
                this.RefuseOther = RefuseOther;
            }

            /// <summary>
            /// The number of successfully processed cash-in items can be limited by specifying the total number of items.
            /// </summary>
            [DataMember(Name = "byTotalItems")]
            public bool? ByTotalItems { get; private set; }

            /// <summary>
            /// The number of successfully processed cash-in items can be limited by specifying the maximum amount of a specific currency.
            /// </summary>
            [DataMember(Name = "byAmount")]
            public bool? ByAmount { get; private set; }

            /// <summary>
            /// CashAcceptor.SetCashInLimit may be called multiple times in a cash-in transaction to update previously specified amount limits. Only valid if combined with "byAmount".
            /// </summary>
            [DataMember(Name = "multiple")]
            public bool? Multiple { get; private set; }

            /// <summary>
            /// If multiple currencies can be accepted and an amount limit is specified for one or more currencies, any other unspecified currencies are refused. 
            /// If not specified, there is no amount limit for unspecified currencies. Only valid if specified with \"byAmount\".
            /// </summary>
            [DataMember(Name = "refuseOther")]
            public bool? RefuseOther { get; private set; }

        }

        /// <summary>
        /// Specifies whether the cash-in limitation is supported or not for the CashAcceptor.SetCashInLimit command. 
        /// If the device does not have the capability to limit the amount or the number of items during cash-in operations
        /// all flags will be set to false.
        /// </summary>
        [DataMember(Name = "cashInLimit")]
        public CashInLimitClass CashInLimit { get; private set; }

        [DataContract]
        public sealed class CountActionsClass
        {
            public CountActionsClass(bool? Individual = null, bool? All = null)
            {
                this.Individual = Individual;
                this.All = All;
            }

            /// <summary>
            /// The counting of individual cash units via the input structure of the CashAcceptor.CashUnitCount command is supported.
            /// </summary>
            [DataMember(Name = "individual")]
            public bool? Individual { get; private set; }

            /// <summary>
            /// The counting of all cash units via the empty payload structure of the CashAcceptor.CashUnitCount command is supported.
            /// </summary>
            [DataMember(Name = "all")]
            public bool? All { get; private set; }

        }

        /// <summary>
        /// Specifies the count action supported by the CashAcceptor.CashUnitCount command. If the device does not support counting 
        /// then all flags will be set to false.
        /// </summary>
        [DataMember(Name = "countActions")]
        public CountActionsClass CountActions { get; private set; }

        /// <summary>
        /// Specifies whether the CashAcceptor supports physical lock/unlock control of the CashAcceptor device and/or the cash units. If this value is set to TRUE, 
        /// the device and/or the cash units can be locked and unlocked by the CashAcceptor.DeviceLockControl command, and the lock status can be 
        /// retrieved by the CashAcceptor.DeviceLockStatus command. If this value is set to FALSE, the CashAcceptor will not support the physical lock/unlock control of the CashAcceptor device or the cash units; 
        /// </summary>
        [DataMember(Name = "deviceLockControl")]
        public bool? DeviceLockControl { get; private set; }

        /// <summary>
        /// Specifies whether the device supports accepting and processing items other than the types defined in the CashAcceptor specification. For a description of Mixed Media transactions see section ATM Mixed Media Transaction Flow – Application Guidelines. 
        /// </summary>
        [DataMember(Name = "mixedMode")]
        public bool? MixedMode { get; private set; }

        /// <summary>
        /// Specifies whether the device can deposit one type of media and rollback the other in the same Mixed Media transaction. Where mixedDepositAndRollback is TRUE the Service can accept CashAcceptor.CashInEnd and ItemProcessor.MediaInRollback or CashAcceptor.CashInRollback and ItemProcessor.MediaInEnd to complete the current transaction. This value can only be TRUE where mixedMode == TRUE. When mixedDepositAndRollback is FALSE applications must either deposit or return ALL items to complete a transaction. Where Mixed Media transactions are not supported mixedDepositAndRollback is FALSE.
        /// </summary>
        [DataMember(Name = "mixedDepositAndRollback")]
        public bool? MixedDepositAndRollback { get; private set; }

        /// <summary>
        /// If set to TRUE the CashAcceptor.Deplete command is supported.
        /// </summary>
        [DataMember(Name = "deplete")]
        public bool? Deplete { get; private set; }

        public enum CounterfeitActionEnum
        {
            None,
            Level2,
            Level23
        }

        /// <summary>
        /// If level 2/3 notes are not to be returned to the customer by these rules, they will not be returned regardless of whether their 
        /// specific note type is configured to not be accepted by CashAcceptor.ConfigureNotetypes. Following rules are possible:
        /// 
        /// \"none\": The device is not able to classify notes as level 1, 2, 3 or 4.
        /// 
        /// \"level2\": Notes are classified as level 1, 2, 3 or 4 and only level 2 notes will not be returned to the customer in a cash-in transaction.
        /// 
        /// \"level23\": Notes are classified as level 1, 2, 3 or 4 and level 2 and level 3 notes will not be returned to the customer in a cash-in transaction.
        /// </summary>
        [DataMember(Name = "counterfeitAction")]
        public CounterfeitActionEnum? CounterfeitAction { get; private set; }

    }


}
