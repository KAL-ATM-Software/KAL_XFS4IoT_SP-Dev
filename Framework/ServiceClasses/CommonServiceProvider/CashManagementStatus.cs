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

namespace XFS4IoTFramework.Common
{
    public sealed class CashManagementStatusClass
    {
        public enum SafeDoorEnum
        {
            NotSupported,
            Open,
            Closed,
            Unknown,
        }

        public enum DispenserEnum
        {
            Ok,
            Attention,
            Stop,
            Unknown,
            NotSupported,
        }

        public enum AcceptorEnum
        {
            Ok,
            Attention,
            Stop,
            Unknown,
            NotSupported,
        }

        public enum ShutterEnum
        {
            Closed,
            Open,
            Jammed,
            Unknown,
            NotSupported
        }

        public CashManagementStatusClass()
        {
            SafeDoor = SafeDoorEnum.NotSupported;
            Dispenser = DispenserEnum.Unknown;
            Acceptor =  AcceptorEnum.Unknown;
            AllBanknoteItems = null;
        }
        public CashManagementStatusClass(SafeDoorEnum SafeDoor,
                                         DispenserEnum Dispenser,
                                         AcceptorEnum Acceptor,
                                         Dictionary<string, bool> AllBanknoteItems)
        {
            this.SafeDoor = SafeDoor;
            this.Dispenser = Dispenser;
            this.Acceptor = Acceptor;
            this.AllBanknoteItems = AllBanknoteItems;
        }



        /// <summary>
        /// Supplies the state of the safe door. Following values are possible:
        /// 
        /// * ```doorNotSupported``` - Physical device has no safe door or safe door state reporting is not supported.
        /// * ```doorOpen``` - Safe door is open.
        /// * ```doorClosed``` - Safe door is closed.
        /// * ```doorUnknown``` - Due to a hardware error or other condition, the state of the safe door cannot be determined.
        /// </summary>
        public SafeDoorEnum SafeDoor { get; set; }

        /// <summary>
        /// Supplies the state of the storage units for dispensing cash. Following values are possible:
        /// 
        /// * ```ok``` - All storage units present are in a good state.
        /// * ```attention``` - One or more of the storage units is in a low, empty, inoperative or manipulated condition. 
        /// Items can still be dispensed from at least one of the storage units.
        /// * ```stop``` - Due to a storage unit failure dispensing is impossible. No items can be dispensed because 
        /// all of the storage units are in an empty, inoperative or manipulated condition. This state may also occur 
        /// when a reject/retract storage unit is full or no reject/retract storage unit is present, or when an application 
        /// lock is set on every storage unit which can be locked.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the storage units cannot be determined.
        /// </summary>
        public DispenserEnum Dispenser { get; set; }



        /// <summary>
        /// Supplies the state of the storage units for accepting cash. Following values are possible:
        /// 
        /// * ```ok``` - All storage units present are in a good state.
        /// * ```attention``` - One or more of the storage units is in a high, full, inoperative or manipulated condition. 
        /// Items can still be accepted into at least one of the storage units.
        /// * ```stop``` - Due to a storage unit failure accepting is impossible. No items can be accepted because 
        /// all of the storage units are in a full, inoperative or manipulated condition. This state may also occur when
        /// a retract storage unit is full or no retract cash storage unit is present, or when an application lock is 
        /// set on every storage unit, or when counterfeit or suspect items are to be automatically retained within 
        /// storage units, but all of the designated storage units for storing them are full or inoperative.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the storage units cannot be 
        /// determined.
        /// </summary>
        public AcceptorEnum Acceptor { get; set; }

        /// <summary>
        /// Cash item can be recognised and enabled or enabled
        /// </summary>
        public Dictionary<string, bool> AllBanknoteItems { get; set; }
    }
}
