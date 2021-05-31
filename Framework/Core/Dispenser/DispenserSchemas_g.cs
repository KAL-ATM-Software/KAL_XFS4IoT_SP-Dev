/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * DispenserSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.Dispenser
{

    [DataContract]
    public sealed class OutposClass
    {
        public OutposClass(PositionEnum? Position = null, ShutterEnum? Shutter = null, PositionStatusEnum? PositionStatus = null, TransportEnum? Transport = null, TransportStatusEnum? TransportStatus = null, JammedShutterPositionEnum? JammedShutterPosition = null)
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
            Left,
            Right,
            Center,
            Top,
            Bottom,
            Front,
            Rear
        }

        /// <summary>
        /// Supplies the output position as one of the following values:
        /// 
        /// * ```left``` - Left output position.
        /// * ```right``` - Right output position.
        /// * ```center``` - Center output position.
        /// * ```top``` - Top output position.
        /// * ```bottom``` - Bottom output position.
        /// * ```front``` - Front output position.
        /// * ```rear``` - Rear output position.
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
        /// * ```closed``` - The shutter is operational and is closed.
        /// * ```open``` - The shutter is operational and is open.
        /// * ```jammed``` - The shutter is jammed and is not operational. The field jammedShutterPosition provides the positional state of the shutter.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the shutter cannot be determined.
        /// * ```notSupported``` - The physical device has no shutter or shutter state reporting is not supported.
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
        /// * ```ok``` - The transport is in a good state.
        /// * ```inoperative``` - The transport is inoperative due to a hardware failure or media jam.
        /// * ```unknown``` -Due to a hardware error or other condition the state of the transport cannot be determined.
        /// * ```notSupported``` - The physical device has no transport or transport state reporting is not supported.
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
        /// device it is possible that the transport will not be empty due to a previous cash-in operation. 
        /// Following values are possible:
        /// 
        /// * ```empty``` - The transport is empty.
        /// * ```notEmpty``` - The transport is not empty.
        /// * ```notEmptyCustomer``` - Items which a customer has had access to are on the transport.
        /// * ```notEmptyUnkown``` - Due to a hardware error or other condition it is not known whether there are items on the transport.
        /// * ```notSupported``` - The device is not capable of reporting whether items are on the transport.
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
        /// * ```notSupported``` - The physical device has no shutter or the reporting of the position of a jammed shutter is not supported.
        /// * ```notJammed``` - The shutter is not jammed.
        /// * ```open``` - The shutter is jammed, but fully open.
        /// * ```partiallyOpen``` - The shutter is jammed, but partially open.
        /// * ```closed``` - The shutter is jammed, but fully closed.
        /// * ```unknown``` - The position of the shutter is unknown.
        /// </summary>
        [DataMember(Name = "jammedShutterPosition")]
        public JammedShutterPositionEnum? JammedShutterPosition { get; private set; }

    }


    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(IntermediateStackerEnum? IntermediateStacker = null, List<OutposClass> Positions = null)
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
        public IntermediateStackerEnum? IntermediateStacker { get; private set; }

        /// <summary>
        /// Array of structures for each position to which items can be dispensed or presented.
        /// </summary>
        [DataMember(Name = "positions")]
        public List<OutposClass> Positions { get; private set; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(TypeEnum? Type = null, int? MaxDispenseItems = null, bool? Shutter = null, bool? ShutterControl = null, RetractAreasClass RetractAreas = null, RetractTransportActionsClass RetractTransportActions = null, RetractStackerActionsClass RetractStackerActions = null, bool? IntermediateStacker = null, bool? ItemsTakenSensor = null, PositionsClass Positions = null, MoveItemsClass MoveItems = null, bool? PrepareDispense = null)
        {
            this.Type = Type;
            this.MaxDispenseItems = MaxDispenseItems;
            this.Shutter = Shutter;
            this.ShutterControl = ShutterControl;
            this.RetractAreas = RetractAreas;
            this.RetractTransportActions = RetractTransportActions;
            this.RetractStackerActions = RetractStackerActions;
            this.IntermediateStacker = IntermediateStacker;
            this.ItemsTakenSensor = ItemsTakenSensor;
            this.Positions = Positions;
            this.MoveItems = MoveItems;
            this.PrepareDispense = PrepareDispense;
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
        public TypeEnum? Type { get; private set; }

        /// <summary>
        /// Supplies the maximum number of items that can be dispensed in a single dispense operation. 
        /// If no limit applies this value will be zero - in this case, if an attempt is made to dispense more items 
        /// than the hardware limitations will allow, the Service will implement the dispense as a series 
        /// of sub-dispense operations (see section Sub-Dispensing Command Flow).
        /// </summary>
        [DataMember(Name = "maxDispenseItems")]
        public int? MaxDispenseItems { get; private set; }

        /// <summary>
        /// Specifies whether or not the commands Dispenser.OpenShutter and Dispenser.CloseShutter are supported.
        /// </summary>
        [DataMember(Name = "shutter")]
        public bool? Shutter { get; private set; }

        /// <summary>
        /// If set to TRUE the shutter is controlled implicitly by the Service. 
        /// If set to FALSE the shutter must be controlled explicitly by the application 
        /// using the Dispenser.OpenShutter and the Dispenser.CloseShutter commands. 
        /// This field is always set to TRUE if the device has no shutter. 
        /// This field applies to all shutters and all output positions.
        /// </summary>
        [DataMember(Name = "shutterControl")]
        public bool? ShutterControl { get; private set; }

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
            /// The items may be retracted to the item cassettes, i.e. cassettes that can be dispensed from.
            /// </summary>
            [DataMember(Name = "itemCassette")]
            public bool? ItemCassette { get; private set; }

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
            /// The items may be moved to the item cassettes, i.e. cassettes that can be dispensed from.
            /// </summary>
            [DataMember(Name = "itemCassette")]
            public bool? ItemCassette { get; private set; }

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
            /// The items may be moved to the item cassettes, i.e. cassettes that can be dispensed from.
            /// </summary>
            [DataMember(Name = "itemCassette")]
            public bool? ItemCassette { get; private set; }

        }

        /// <summary>
        /// Specifies the actions which may be performed on items which have been retracted to the stacker. 
        /// If the device does not have the capability to retract items to the stacker or move items from the stacker 
        /// all flags will be set to false.
        /// </summary>
        [DataMember(Name = "retractStackerActions")]
        public RetractStackerActionsClass RetractStackerActions { get; private set; }

        /// <summary>
        /// Specifies whether or not the Dispenser supports stacking items to an intermediate position before 
        /// the items are moved to the exit position. If this value is TRUE, the field \"present\" 
        /// of the Dispenser.Dispense command can be set to FALSE.
        /// </summary>
        [DataMember(Name = "intermediateStacker")]
        public bool? IntermediateStacker { get; private set; }

        /// <summary>
        /// Specifies whether the Dispenser can detect when items at the exit position are taken by the user. 
        /// If set to TRUE the Service generates an accompanying Dispenser.ItemsTakenEvent. 
        /// If set to FALSE this event is not generated. This field applies to all output positions.
        /// </summary>
        [DataMember(Name = "itemsTakenSensor")]
        public bool? ItemsTakenSensor { get; private set; }

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
            public bool? Left { get; private set; }

            /// <summary>
            /// The Dispenser has a right output position.
            /// </summary>
            [DataMember(Name = "right")]
            public bool? Right { get; private set; }

            /// <summary>
            /// The Dispenser has a center output position.
            /// </summary>
            [DataMember(Name = "center")]
            public bool? Center { get; private set; }

            /// <summary>
            /// The Dispenser has a top output position.
            /// </summary>
            [DataMember(Name = "top")]
            public bool? Top { get; private set; }

            /// <summary>
            /// The Dispenser has a bottom output position.
            /// </summary>
            [DataMember(Name = "bottom")]
            public bool? Bottom { get; private set; }

            /// <summary>
            /// The Dispenser has a front output position.
            /// </summary>
            [DataMember(Name = "front")]
            public bool? Front { get; private set; }

            /// <summary>
            /// The Dispenser has a rear output position.
            /// </summary>
            [DataMember(Name = "rear")]
            public bool? Rear { get; private set; }

        }

        /// <summary>
        /// Specifies the Dispenser output positions which are available.
        /// </summary>
        [DataMember(Name = "positions")]
        public PositionsClass Positions { get; private set; }

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
            /// The Dispenser can dispense items from the cash units to the intermediate stacker while there are items
            /// on the transport.
            /// </summary>
            [DataMember(Name = "fromCashUnit")]
            public bool? FromCashUnit { get; private set; }

            /// <summary>
            /// The Dispenser can retract items to the cash units while there are items on the intermediate stacker.
            /// </summary>
            [DataMember(Name = "toCashUnit")]
            public bool? ToCashUnit { get; private set; }

            /// <summary>
            /// The Dispenser can retract items to the transport while there are items on the intermediate stacker.
            /// </summary>
            [DataMember(Name = "toTransport")]
            public bool? ToTransport { get; private set; }

            /// <summary>
            /// The Dispenser can dispense items from the cash units to the intermediate stacker while there are already items on the 
            /// intermediate stacker that have not been in customer access. Items remaining on the stacker from a previous dispense 
            /// may first need to be rejected explicitly by the application if they are not to be presented.
            /// </summary>
            [DataMember(Name = "toStacker")]
            public bool? ToStacker { get; private set; }

        }

        /// <summary>
        /// Specifies the Dispenser move item options which are available.
        /// </summary>
        [DataMember(Name = "moveItems")]
        public MoveItemsClass MoveItems { get; private set; }

        /// <summary>
        /// On some hardware it can take a significant amount of time for the dispenser to get ready to dispense media. 
        /// On this type of hardware the Dispenser.PrepareDispense command can be used to improve transaction performance. 
        /// This flag indicates if the hardware requires the application to use the Dispenser.PrepareDispense command 
        /// to maximize transaction performance. If this flag is TRUE then the Dispenser.PrepareDispense 
        /// command is supported and can be used to improve transaction performance. 
        /// If this flag is FALSE then the Dispenser.PrepareDispense command is not supported.
        /// </summary>
        [DataMember(Name = "prepareDispense")]
        public bool? PrepareDispense { get; private set; }

    }


}
