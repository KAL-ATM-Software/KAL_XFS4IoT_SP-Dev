/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Common
{
    public sealed class CashManagementStatusClass : StatusBase
    {
        public sealed class PositionStatusClass : StatusBase
        {
            public PositionStatusClass(ShutterEnum Shutter,
                                       PositionStatusEnum PositionStatus,
                                       TransportEnum Transport,
                                       TransportStatusEnum TransportStatus)
            {
                shutter = Shutter;
                positionStatus = PositionStatus;
                transport = Transport;
                transportStatus = TransportStatus;
            }

            public PositionStatusClass()
            {
            }

            /// <summary>
            /// This property is set by the framework to generate status changed event
            /// </summary>
            public CashManagementCapabilitiesClass.OutputPositionEnum? CashDispenserPosition { get; set; } = null;
            public CashManagementCapabilitiesClass.PositionEnum? CashAcceptorPosition { get; set; } = null;

            /// <summary>
            /// Supplies the state of the shutter. Following values are possible:
            /// 
            /// * ```closed``` - The shutter is operational and is closed.
            /// * ```open``` - The shutter is operational and is open.
            /// * ```jammed``` - The shutter is jammed and is not operational. The field jammedShutterPosition provides the positional state of the shutter.
            /// * ```unknown``` - Due to a hardware error or other condition, the state of the shutter cannot be determined.
            /// * ```notSupported``` - The physical device has no shutter or shutter state reporting is not supported.
            /// </summary>
            public ShutterEnum Shutter 
            {
                get { return shutter; } 
                set
                {
                    if (shutter != value)
                    {
                        shutter = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private ShutterEnum shutter = ShutterEnum.NotSupported;

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
            public PositionStatusEnum PositionStatus 
            { 
                get { return positionStatus; }
                set
                {
                    if (positionStatus != value)
                    {
                        positionStatus = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private PositionStatusEnum positionStatus = PositionStatusEnum.NotSupported;

            /// <summary>
            /// Supplies the state of the transport mechanism. The transport is defined as any area leading to or from the position.
            /// Following values are possible:
            /// 
            /// * ```ok``` - The transport is in a good state.
            /// * ```inoperative``` - The transport is inoperative due to a hardware failure or media jam.
            /// * ```unknown``` -Due to a hardware error or other condition the state of the transport cannot be determined.
            /// * ```notSupported``` - The physical device has no transport or transport state reporting is not supported.
            /// </summary>
            public TransportEnum Transport 
            { 
                get { return transport; } 
                set
                {
                    if (transport != value)
                    {
                        transport = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private TransportEnum transport = TransportEnum.NotSupported;

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
            public TransportStatusEnum TransportStatus 
            { 
                get { return transportStatus; } 
                set
                {
                    if (transportStatus != value)
                    {
                        transportStatus = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private TransportStatusEnum transportStatus = TransportStatusEnum.NotSupported;
        }

        public enum PositionStatusEnum
        {
            Empty,
            NotEmpty,
            Unknown,
            NotSupported,
        }

        public enum TransportEnum
        {
            Ok,
            Inoperative,
            Unknown,
            NotSupported,
        }

        public enum TransportStatusEnum
        {
            Empty,
            NotEmpty,
            NotEmptyCustomer,
            Unknown,
            NotSupported,
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
            JammedOpen,
            JammedPartiallyOpen,
            JammedClosed,
            JammedUnknown,
            Unknown,
            NotSupported,
        }

        public CashManagementStatusClass()
        { }
        public CashManagementStatusClass(DispenserEnum Dispenser,
                                         AcceptorEnum Acceptor)
        {
            dispenser = Dispenser;
            acceptor = Acceptor;
        }

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
        public DispenserEnum Dispenser 
        { 
            get { return dispenser; }
            set
            {
                if (dispenser != value)
                {
                    dispenser = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private DispenserEnum dispenser = DispenserEnum.NotSupported;


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
        public AcceptorEnum Acceptor 
        {
            get { return acceptor; }
            set
            {
                if (acceptor != value)
                {
                    acceptor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private AcceptorEnum acceptor = AcceptorEnum.NotSupported;
    }
}
