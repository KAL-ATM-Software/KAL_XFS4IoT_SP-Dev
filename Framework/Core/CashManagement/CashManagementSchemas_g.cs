/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * CashManagementSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.CashManagement
{

    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(SafeDoorEnum? SafeDoor = null, DispenserEnum? Dispenser = null, AcceptorEnum? Acceptor = null)
        {
            this.SafeDoor = SafeDoor;
            this.Dispenser = Dispenser;
            this.Acceptor = Acceptor;
        }

        public enum SafeDoorEnum
        {
            DoorNotSupported,
            DoorOpen,
            DoorClosed,
            DoorUnknown
        }

        /// <summary>
        /// Supplies the state of the safe door. Following values are possible:
        /// 
        /// * ```doorNotSupported``` - Physical device has no safe door or safe door state reporting is not supported.
        /// * ```doorOpen``` - Safe door is open.
        /// * ```doorClosed``` - Safe door is closed.
        /// * ```doorUnknown``` - Due to a hardware error or other condition, the state of the safe door cannot be determined.
        /// </summary>
        [DataMember(Name = "safeDoor")]
        public SafeDoorEnum? SafeDoor { get; private set; }

        public enum DispenserEnum
        {
            Ok,
            CashUnitState,
            CashUnitStop,
            CashUnitUnknown
        }

        /// <summary>
        /// Supplies the state of the logical cash units for dispensing. Following values are possible:
        /// 
        /// * ```ok``` - All cash units present are in a good state.
        /// * ```cashUnitState``` - One or more of the cash units is in a low, empty, inoperative or manipulated condition. 
        /// Items can still be dispensed from at least one of the cash units.
        /// * ```cashUnitStop``` - Due to a cash unit failure dispensing is impossible. No items can be dispensed because 
        /// all of the cash units are in an empty, inoperative or manipulated condition. This state may also occur 
        /// when a reject/retract cash unit is full or no reject/retract cash unit is present, or when an application 
        /// lock is set on every cash unit which can be locked.
        /// * ```cashUnitUnknown``` - Due to a hardware error or other condition, the state of the cash units cannot be determined.
        /// </summary>
        [DataMember(Name = "dispenser")]
        public DispenserEnum? Dispenser { get; private set; }

        public enum AcceptorEnum
        {
            Ok,
            CashUnitState,
            CashUnitStop,
            CashUnitUnknown
        }

        /// <summary>
        /// Supplies the state of the cash units for accepting cash. Following values are possible:
        /// 
        /// * ```ok``` - All cash units present are in a good state.
        /// * ```cashUnitState``` - One or more of the cash units is in a high, full, inoperative or manipulated condition. Items can still be accepted into at least one of the cash units.
        /// * ```cashUnitStop``` - Due to a cash unit failure accepting is impossible. No items can be accepted because all of the cash units are in a full, inoperative or manipulated condition.
        /// This state may also occur when a retract cash unit is full or no retract cash unit is present, 
        /// or when an application lock is set on every cash unit, or when Level 2/3 notes are to be automatically retained within cash units, 
        /// but all of the designated cash units for storing them are full or inoperative.
        /// * ```cashUnitUnknown``` - Due to a hardware error or other condition, the state of the cash units cannot be determined.
        /// </summary>
        [DataMember(Name = "acceptor")]
        public AcceptorEnum? Acceptor { get; private set; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(bool? SafeDoor = null, bool? CashBox = null, ExchangeTypeClass ExchangeType = null, ItemInfoTypesClass ItemInfoTypes = null, bool? ClassificationList = null, bool? PhysicalNoteList = null)
        {
            this.SafeDoor = SafeDoor;
            this.CashBox = CashBox;
            this.ExchangeType = ExchangeType;
            this.ItemInfoTypes = ItemInfoTypes;
            this.ClassificationList = ClassificationList;
            this.PhysicalNoteList = PhysicalNoteList;
        }

        /// <summary>
        /// Specifies whether or not the  CashManagement.OpenSafeDoor command is supported.
        /// </summary>
        [DataMember(Name = "safeDoor")]
        public bool? SafeDoor { get; private set; }

        /// <summary>
        /// This field is only applicable to teller type devices. 
        /// It specifies whether or not tellers have been assigned a cash box.
        /// </summary>
        [DataMember(Name = "cashBox")]
        public bool? CashBox { get; private set; }

        [DataContract]
        public sealed class ExchangeTypeClass
        {
            public ExchangeTypeClass(bool? ByHand = null, bool? ToCassettes = null, bool? ClearRecycler = null, bool? DepositInto = null)
            {
                this.ByHand = ByHand;
                this.ToCassettes = ToCassettes;
                this.ClearRecycler = ClearRecycler;
                this.DepositInto = DepositInto;
            }

            /// <summary>
            /// The device supports manual replenishment either by filling the cash unit by hand or by replacing the cash unit.
            /// </summary>
            [DataMember(Name = "byHand")]
            public bool? ByHand { get; private set; }

            /// <summary>
            /// The device supports moving items from the replenishment cash unit to another cash unit.
            /// </summary>
            [DataMember(Name = "toCassettes")]
            public bool? ToCassettes { get; private set; }

            /// <summary>
            /// The device supports the emptying of recycle cash units.
            /// </summary>
            [DataMember(Name = "clearRecycler")]
            public bool? ClearRecycler { get; private set; }

            /// <summary>
            /// The device supports moving items from the deposit entrance to the bill cash units.
            /// </summary>
            [DataMember(Name = "depositInto")]
            public bool? DepositInto { get; private set; }

        }

        /// <summary>
        /// Specifies the type of cash unit exchange operations supported by the device.
        /// </summary>
        [DataMember(Name = "exchangeType")]
        public ExchangeTypeClass ExchangeType { get; private set; }

        [DataContract]
        public sealed class ItemInfoTypesClass
        {
            public ItemInfoTypesClass(bool? SerialNumber = null, bool? Signature = null, bool? ImageFile = null)
            {
                this.SerialNumber = SerialNumber;
                this.Signature = Signature;
                this.ImageFile = ImageFile;
            }

            /// <summary>
            /// Serial Number of the item.
            /// </summary>
            [DataMember(Name = "serialNumber")]
            public bool? SerialNumber { get; private set; }

            /// <summary>
            /// Signature of the item.
            /// </summary>
            [DataMember(Name = "signature")]
            public bool? Signature { get; private set; }

            /// <summary>
            /// Image file of the item.
            /// </summary>
            [DataMember(Name = "imageFile")]
            public bool? ImageFile { get; private set; }

        }

        /// <summary>
        /// Specifies the types of information that can be retrieved through the CashManagement.GetItemInfo command.
        /// </summary>
        [DataMember(Name = "itemInfoTypes")]
        public ItemInfoTypesClass ItemInfoTypes { get; private set; }

        /// <summary>
        /// Specifies whether the device has the capability to maintain a classification list of serial numbers as well as
        /// supporting the associated operations. This can either be TRUE if the device has the capability or FALSE if it does not.
        /// </summary>
        [DataMember(Name = "classificationList")]
        public bool? ClassificationList { get; private set; }

        /// <summary>
        /// Specifies whether the Service supports note number lists on physical cash units.
        /// This can either be TRUE if the Service has the capability or FALSE if it does not.
        /// </summary>
        [DataMember(Name = "physicalNoteList")]
        public bool? PhysicalNoteList { get; private set; }

    }


}
