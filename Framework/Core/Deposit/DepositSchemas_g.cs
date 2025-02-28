/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Deposit interface.
 * DepositSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.Deposit
{

    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(DepTransportEnum? DepTransport = null, EnvDispenserEnum? EnvDispenser = null, PrinterEnum? Printer = null, TonerEnum? Toner = null, ShutterEnum? Shutter = null, DepositLocationEnum? DepositLocation = null)
        {
            this.DepTransport = DepTransport;
            this.EnvDispenser = EnvDispenser;
            this.Printer = Printer;
            this.Toner = Toner;
            this.Shutter = Shutter;
            this.DepositLocation = DepositLocation;
        }

        public enum DepTransportEnum
        {
            Ok,
            Inoperative,
            Unknown
        }

        /// <summary>
        /// Specifies the state of the deposit transport mechanism that transports the envelope into the deposit
        /// container. This property is null in [Common.Status](#common.status) if the device has no deposit transport,
        /// otherwise the following values are possible:
        /// 
        /// * ```ok``` - The deposit transport is in a good state.
        /// * ```inoperative``` - The deposit transport is inoperative due to a hardware failure or media jam.
        /// * ```unknown``` - Due to a hardware error or other condition the state of the deposit transport cannot be
        ///   determined.
        /// </summary>
        [DataMember(Name = "depTransport")]
        public DepTransportEnum? DepTransport { get; init; }

        public enum EnvDispenserEnum
        {
            Ok,
            Inoperative,
            Unknown
        }

        /// <summary>
        /// Specifies the state of the envelope dispenser.
        /// 
        /// This property is null in [Common.Status](#common.status) if the device has no envelope dispenser,
        /// otherwise the following values are possible:
        /// 
        /// * ```ok```- The envelope dispenser is present and in a good state.
        /// * ```inoperative``` - The envelope dispenser is present but in an inoperable state. No envelopes can be dispensed.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the envelope dispenser cannot be determined.
        /// </summary>
        [DataMember(Name = "envDispenser")]
        public EnvDispenserEnum? EnvDispenser { get; init; }

        public enum PrinterEnum
        {
            Ok,
            Inoperative,
            Unknown
        }

        /// <summary>
        /// Specifies the state of the printer.
        /// 
        /// This property is null in [Common.Status](#common.status) if the device has no printer,
        /// otherwise the following values are possible:
        /// 
        /// * ```ok```- The printer is present and in a good state.
        /// * ```inoperative``` - The printer is in an inoperable state.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the printer cannot be determined.
        /// </summary>
        [DataMember(Name = "printer")]
        public PrinterEnum? Printer { get; init; }

        public enum TonerEnum
        {
            Full,
            Low,
            Out,
            Unknown
        }

        /// <summary>
        /// Specifies the state of the toner (or ink) for the printer. This may be null in
        /// [Common.Status](#common.status) if the physical device does not support printing or the capability to report
        /// the status of the toner/ink is not supported by the device, otherwise the following values are possible:
        /// 
        /// * ```full``` - The toner or ink supply is full or the ribbon is OK.
        /// * ```low``` - The toner or ink supply is low or the print contrast with a ribbon is weak.
        /// * ```out``` - The toner or ink supply is empty or the print contrast with a ribbon is not sufficient any more.
        /// * ```unknown``` - Status of toner or ink supply or the ribbon cannot be determined with the device in its current state.
        /// </summary>
        [DataMember(Name = "toner")]
        public TonerEnum? Toner { get; init; }

        public enum ShutterEnum
        {
            Closed,
            Open,
            Jammed,
            Unknown
        }

        /// <summary>
        /// Specifies the state of the shutter or door.
        /// 
        /// This may be null in [Common.Status](#common.status) if the physical device has no shutter, otherwise
        /// the following values are possible:
        /// 
        /// * ```closed``` - The shutter is closed.
        /// * ```open``` - The shutter is open.
        /// * ```jammed``` - The shutter is jammed.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the shutter cannot be determined.
        /// </summary>
        [DataMember(Name = "shutter")]
        public ShutterEnum? Shutter { get; init; }

        public enum DepositLocationEnum
        {
            Unknown,
            Container,
            Transport,
            Printer,
            Shutter,
            None,
            Removed
        }

        /// <summary>
        /// Specifies the location of the item deposited at the end of the last [Deposit.Entry](#deposit.entry) command.
        /// 
        /// This may be null in [Common.Status](#common.status) if not supported or in
        /// [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged, otherwise
        /// the following values are possible:
        /// 
        /// * ```unknown``` - Cannot determine the location of the last deposited item.
        /// * ```container``` - The item is in the container.
        /// * ```transport``` - The item is in the transport.
        /// * ```printer``` - The item is in the printer.
        /// * ```shutter``` - The item is at the shutter (available for removal).
        /// * ```none``` - No item was entered on the last [Deposit.Entry](#deposit.entry).
        /// * ```removed``` - The item was removed.
        /// </summary>
        [DataMember(Name = "depositLocation")]
        public DepositLocationEnum? DepositLocation { get; init; }

    }


    [DataContract]
    public sealed class PrintCapabilitiesClass
    {
        public PrintCapabilitiesClass(bool? Toner = null, bool? PrintOnRetract = null, int? MaxNumChars = null, bool? UnicodeSupport = null)
        {
            this.Toner = Toner;
            this.PrintOnRetract = PrintOnRetract;
            this.MaxNumChars = MaxNumChars;
            this.UnicodeSupport = UnicodeSupport;
        }

        /// <summary>
        /// Specifies whether the printer has a toner (or ink) cassette.
        /// <example>true</example>
        /// </summary>
        [DataMember(Name = "toner")]
        public bool? Toner { get; init; }

        /// <summary>
        /// Specifies whether the device can print the *printData* in a [Deposit.Retract](#deposit.retract) on
        /// retracted envelopes.
        /// <example>true</example>
        /// </summary>
        [DataMember(Name = "printOnRetract")]
        public bool? PrintOnRetract { get; init; }

        /// <summary>
        /// Specifies the maximum number of non-proportional ASCII characters that can be printed on the envelope. Any
        /// attempt to print more characters than this will result in *invalidData* being returned.
        /// 
        /// This property is related to the printable area supported by the device, therefore the actual number of
        /// characters which can be printed will be affected by the characters to be printed and the size of the media
        /// being printed on, therefore it is possible that the print data may be truncated.
        /// <example>32</example>
        /// </summary>
        [DataMember(Name = "maxNumChars")]
        [DataTypes(Minimum = 1)]
        public int? MaxNumChars { get; init; }

        /// <summary>
        /// Specifies whether the range of characters that can be printed on the envelope may include Unicode characters.
        /// Note that print data is always supplied in Unicode, but some devices may not be able to print a full range
        /// of characters and are restricted to the ASCII character range.
        /// 
        /// If *true*, characters in the Unicode range can be printed. If *false*, only characters in the ASCII range
        /// can be printed.
        /// 
        /// **Note** - regardless of this capability, the device may not be able to print all of the characters in either
        /// specified range. If a character is not supported by the device it will be replaced by a vendor dependent
        /// substitution character. It is the responsibility of the vendor to supply information about which characters
        /// are supported on a given device.
        /// <example>true</example>
        /// </summary>
        [DataMember(Name = "unicodeSupport")]
        public bool? UnicodeSupport { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(TypeClass Type = null, bool? DepTransport = null, PrintCapabilitiesClass Printer = null, bool? Shutter = null, RetractEnvelopeEnum? RetractEnvelope = null)
        {
            this.Type = Type;
            this.DepTransport = DepTransport;
            this.Printer = Printer;
            this.Shutter = Shutter;
            this.RetractEnvelope = RetractEnvelope;
        }

        [DataContract]
        public sealed class TypeClass
        {
            public TypeClass(bool? Envelope = null, bool? Bag = null)
            {
                this.Envelope = Envelope;
                this.Bag = Bag;
            }

            /// <summary>
            /// The depository accepts envelopes.
            /// <example>true</example>
            /// </summary>
            [DataMember(Name = "envelope")]
            public bool? Envelope { get; init; }

            /// <summary>
            /// The depository accepts bags.
            /// <example>true</example>
            /// </summary>
            [DataMember(Name = "bag")]
            public bool? Bag { get; init; }

        }

        /// <summary>
        /// Specifies the type of the depository device. At least one of the properties must be true.
        /// </summary>
        [DataMember(Name = "type")]
        public TypeClass Type { get; init; }

        /// <summary>
        /// Specifies whether a deposit transport mechanism is available.
        /// <example>true</example>
        /// </summary>
        [DataMember(Name = "depTransport")]
        public bool? DepTransport { get; init; }

        [DataMember(Name = "printer")]
        public PrintCapabilitiesClass Printer { get; init; }

        /// <summary>
        /// Specifies whether a deposit transport shutter is available.
        /// <example>true</example>
        /// </summary>
        [DataMember(Name = "shutter")]
        public bool? Shutter { get; init; }

        public enum RetractEnvelopeEnum
        {
            Container,
            Dispenser
        }

        /// <summary>
        /// Specifies the ability of the envelope dispenser to retract envelopes. May be null if there is no envelope
        /// dispenser or it does not have the capability to retract envelopes, otherwise one of the following:
        /// 
        /// * ```container``` - Retracted envelopes are put in the deposit container.
        /// * ```dispenser``` - Retracted envelopes are retracted back to the envelope dispenser.
        /// </summary>
        [DataMember(Name = "retractEnvelope")]
        public RetractEnvelopeEnum? RetractEnvelope { get; init; }

    }


    [DataContract]
    public sealed class StorageCapabilitiesClass
    {
        public StorageCapabilitiesClass(EnvSupplyEnum? EnvSupply = null)
        {
            this.EnvSupply = EnvSupply;
        }

        public enum EnvSupplyEnum
        {
            Motorized,
            Manual
        }

        /// <summary>
        /// Defines what type of envelope supply unit the device has. This property may be null if the device
        /// has no envelope supply or the envelope supply is manual and envelopes can be taken at any time,
        /// otherwise one of the following:
        /// 
        /// * ```motorized``` - Envelope supply can dispense envelopes.
        /// * ```manual``` - Envelope supply is manual and must be unlocked to allow envelopes to be taken.
        /// The [Deposit.EnvTakenEvent](#deposit.envtakenevent) cannot be sent and [Deposit.Retract](#deposit.retract)
        /// cannot be supported.
        /// </summary>
        [DataMember(Name = "envSupply")]
        public EnvSupplyEnum? EnvSupply { get; init; }

    }


    [DataContract]
    public sealed class StorageStatusClass
    {
        public StorageStatusClass(DepContainerEnum? DepContainer = null, EnvSupplyEnum? EnvSupply = null, int? NumOfDeposits = null)
        {
            this.DepContainer = DepContainer;
            this.EnvSupply = EnvSupply;
            this.NumOfDeposits = NumOfDeposits;
        }

        public enum DepContainerEnum
        {
            Ok,
            High,
            Full,
            Inoperative,
            Missing,
            Unknown
        }

        /// <summary>
        /// Specifies the state of the deposit container that contains the deposited envelopes or bags. This may be null
        /// if the physical device is not able to determine the status of the deposit container, otherwise the following
        /// values are possible:
        /// 
        /// * ```ok```- The deposit container is in a good state.
        /// * ```high```- The deposit container is almost full (threshold).
        /// * ```full```- The deposit container is full.
        /// * ```inoperative```- The deposit container is inoperative.
        /// * ```missing```- The deposit container is missing.
        /// * ```unknown```- Due to a hardware error or other condition, the state of the deposit container cannot be
        ///   determined.
        /// <example>full</example>
        /// </summary>
        [DataMember(Name = "depContainer")]
        public DepContainerEnum? DepContainer { get; init; }

        public enum EnvSupplyEnum
        {
            Ok,
            Low,
            Empty,
            Inoperative,
            Missing,
            Unlocked,
            Unknown
        }

        /// <summary>
        /// Specifies the state of the envelope supply unit.
        /// 
        /// This property may be null if the device has no envelope supply, otherwise the following values are possible:
        /// 
        /// * ```ok``` - The envelope supply unit is in a good state (and locked).
        /// * ```low``` - The envelope supply unit is present but low.
        /// * ```empty``` - The envelope supply unit is present but empty. No envelopes can be dispensed.
        /// * ```inoperative``` - The envelope supply unit is in an inoperable state. No envelopes can be dispensed.
        /// * ```missing``` - The envelope supply unit is missing.
        /// * ```unlocked``` - The envelope supply unit is unlocked.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the envelope supply cannot be
        ///   determined.
        /// <example>unlocked</example>
        /// </summary>
        [DataMember(Name = "envSupply")]
        public EnvSupplyEnum? EnvSupply { get; init; }

        /// <summary>
        /// Reports the number of envelopes or bags in the deposit container. This value is persistent, i.e.,
        /// maintained through power failures, opens, closes and system resets. It is incremented starting from the
        /// count set by [numOfDeposits](#storage.setstorage.command.description.storage.unit1.deposit.numofdeposits).
        /// This may be null in events if unchanged.
        /// <example>15</example>
        /// </summary>
        [DataMember(Name = "numOfDeposits")]
        [DataTypes(Minimum = 0)]
        public int? NumOfDeposits { get; init; }

    }


    [DataContract]
    public sealed class StorageClass
    {
        public StorageClass(StorageCapabilitiesClass Capabilities = null, StorageStatusClass Status = null)
        {
            this.Capabilities = Capabilities;
            this.Status = Status;
        }

        [DataMember(Name = "capabilities")]
        public StorageCapabilitiesClass Capabilities { get; init; }

        [DataMember(Name = "status")]
        public StorageStatusClass Status { get; init; }

    }


    [DataContract]
    public sealed class StorageSetClass
    {
        public StorageSetClass(int? NumOfDeposits = null)
        {
            this.NumOfDeposits = NumOfDeposits;
        }

        /// <summary>
        /// Specifies the number of envelopes or bags in the deposit container. This value is persistent, i.e.,
        /// maintained through power failures, opens, closes and system resets. This initializes
        /// [numOfDeposits](#storage.getstorage.completion.description.storage.unit1.deposit.status.numofdeposits).
        /// <example>15</example>
        /// </summary>
        [DataMember(Name = "numOfDeposits")]
        [DataTypes(Minimum = 0)]
        public int? NumOfDeposits { get; init; }

    }


}
