/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTServer;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XFS4IoTFramework.Common
{
    public sealed class DepositStatusClass(
        DepositStatusClass.DepositTransportEnum DepositTransport,
        DepositStatusClass.EnvelopDispenserEnum EnvelopDispenser,
        DepositStatusClass.PrinterEnum Printer,
        DepositStatusClass.TonerEnum Toner,
        DepositStatusClass.ShutterEnum Shutter,
        DepositStatusClass.DepositLocationEnum DepositLocation) : StatusBase
    {
        public enum DepositTransportEnum
        {
            NotSupported,
            Healthy,     //The deposit transport is in a good state.
            Inoperative, //The deposit transport is inoperative due to a hardware failure or media jam.
            Unknown,     //Due to a hardware error or other condition the state of the deposit transport cannot be
        }

        public enum EnvelopDispenserEnum
        {
            NotSupported,
            Healthy,     //The printer is present and in a good state.
            Inoperative, //The printer is in an inoperable state.
            Unknown,     //Due to a hardware error or other condition, the state of the printer cannot be determined.
        }

        public enum PrinterEnum
        {
            Healthy,     //The printer is present and in a good state.
            Inoperative, //The printer is in an inoperable state.
            Unknown,     //Due to a hardware error or other condition, the state of the printer cannot be determined.
        }

        public enum TonerEnum
        {
            NotSupported,
            Full,    //The toner or ink supply is full or the ribbon is OK.
            Low,     //The toner or ink supply is low or the print contrast with a ribbon is weak.
            Out,     //The toner or ink supply is empty or the print contrast with a ribbon is not sufficient any more.
            Unknown, //Status of toner or ink supply or the ribbon cannot be determined with the device in its current state.
        }

        public enum ShutterEnum
        {
            NotSupported,
            Closed,  //The shutter is closed.
            Open,    //The shutter is open.
            Jammed,  //The shutter is jammed.
            Unknown, //Due to a hardware error or other condition, the state of the shutter cannot be determined.
        }

        public enum DepositLocationEnum
        {
            NotSupported,
            Unknown,   //Cannot determine the location of the last deposited item.
            Container, //The item is in the container.
            Transport, //The item is in the transport.
            Printer,   //The item is in the printer.
            Shutter,   //The item is at the shutter (available for removal).
            None,      //No item was entered on the last[Deposit.Entry] (#deposit.entry).
            Removed,   //The item was removed.
        }

        /// <summary>
        /// Specifies the state of the deposit transport mechanism that transports the envelope
        /// into the deposit container.
        /// 
        /// Possible values are:
        /// * ```healthy``` - The deposit transport is in a good state.
        /// * ```inoperative``` - The deposit transport is inoperative due to a hardware failure or media jam.
        /// * ```unknown``` - Due to a hardware error or other condition the state of the deposit transport 
        /// cannot be determined.
        /// </summary>
        public DepositTransportEnum DepositTransport
        {
            get { return depositTransport; }
            set
            {
                if (depositTransport != value)
                {
                    depositTransport = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private DepositTransportEnum depositTransport = DepositTransport;

        /// <summary>
        /// Specifies the state of the envelope dispenser.
        ///
        /// Possible values are:
        /// * ```healthy```- The envelope dispenser is present and in a good state.
        /// * ```inoperative``` - The envelope dispenser is present but in an inoperable state.No envelopes can be dispensed.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the envelope dispenser cannot be determined.
        /// </summary>
        public EnvelopDispenserEnum EnvelopDispenser 
        { 
            get { return envelopDispenser; }
            set
            {
                if (envelopDispenser != value)
                {
                    envelopDispenser = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private EnvelopDispenserEnum envelopDispenser = EnvelopDispenser;

        /// <summary>
        /// Specifies the state of the printer.
        /// 
        /// Possible values are:
        /// * ```healthy```- The printer is present and in a good state.
        /// * ```inoperative``` - The printer is in an inoperable state.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the printer cannot be determined.
        /// </summary>
        public PrinterEnum Printer
        {
            get { return printer; }
            set
            {
                if (printer != value)
                {
                    printer = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private PrinterEnum printer = Printer;

        /// <summary>
        /// Specifies the state of the toner (or ink) for the printer.
        /// 
        /// Possible values are:
        /// * ```full``` - The toner or ink supply is full or the ribbon is OK.
        /// * ```low``` - The toner or ink supply is low or the print contrast with a ribbon is weak.
        /// * ```out``` - The toner or ink supply is empty or the print contrast with a ribbon is not sufficient any more.
        /// * ```unknown``` - Status of toner or ink supply or the ribbon cannot be determined with the device in its current state.
        /// </summary>
        public TonerEnum Toner
        {
            get { return toner; }
            set
            {
                if (toner != value)
                {
                    toner = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private TonerEnum toner = Toner;

        /// <summary>
        /// Specifies the state of the shutter or door.
        /// 
        /// Possible values are:
        /// * ```closed``` - The shutter is closed.
        /// * ```open``` - The shutter is open.
        /// * ```jammed``` - The shutter is jammed.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the shutter cannot be determined.
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
        private ShutterEnum shutter = Shutter;

        /// <summary>
        ///  Specifies the location of the item deposited at the end of the last Entry operation.
        ///  
        /// Possible values are:
        /// * ```unknown``` - Cannot determine the location of the last deposited item.
        /// * ```container``` - The item is in the container.
        /// * ```transport``` - The item is in the transport.
        /// * ```printer``` - The item is in the printer.
        /// * ```shutter``` - The item is at the shutter (available for removal).
        /// * ```none``` - No item was entered on the last[Deposit.Entry] (#deposit.entry).
        /// * ```removed``` - The item was removed.
        /// </summary>
        public DepositLocationEnum DepositLocation
        {
            get { return depositLocation; }
            set
            {
                if (depositLocation != value)
                {
                    depositLocation = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private DepositLocationEnum depositLocation = DepositLocation;
    }
}
