/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * CardReaderSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.CardReader
{

    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(MediaEnum? Media = null, SecurityEnum? Security = null, ChipPowerEnum? ChipPower = null, ChipModuleEnum? ChipModule = null, MagWriteModuleEnum? MagWriteModule = null, FrontImageModuleEnum? FrontImageModule = null, BackImageModuleEnum? BackImageModule = null)
        {
            this.Media = Media;
            this.Security = Security;
            this.ChipPower = ChipPower;
            this.ChipModule = ChipModule;
            this.MagWriteModule = MagWriteModule;
            this.FrontImageModule = FrontImageModule;
            this.BackImageModule = BackImageModule;
        }

        public enum MediaEnum
        {
            Unknown,
            Present,
            NotPresent,
            Jammed,
            Entering,
            Latched
        }

        /// <summary>
        /// Specifies the transport/exit position media state. This property will be null if the capability to report
        /// media position is not supported by the device (e.g., a typical swipe reader or contactless chip card
        /// reader), otherwise one of the following values:
        /// 
        /// * ```unknown``` - The media state cannot be determined with the device in its current state (e.g. the value
        ///   of [device](#common.status.completion.properties.common.device) is *noDevice*, *powerOff*, *offline* or *hardwareError*.
        /// * ```present``` - Media is present in the device, not in the entering position and not jammed. On the
        ///   latched dip device, this indicates that the card is present in the device and the card is unlatched.
        /// * ```notPresent``` - Media is not present in the device and not at the entering position.
        /// * ```jammed``` - Media is jammed in the device; operator intervention is required.
        /// * ```entering``` - Media is at the entry/exit slot of a motorized device.
        /// * ```latched``` - Media is present and latched in a latched dip card unit. This means the card can be used
        ///   for chip card dialog.
        /// </summary>
        [DataMember(Name = "media")]
        public MediaEnum? Media { get; init; }

        public enum SecurityEnum
        {
            NotReady,
            Open
        }

        /// <summary>
        /// Specifies the state of the security module. This property will be null if no security module is available,
        /// otherwise one of the following values:
        /// 
        /// * ```notReady``` - The security module is not ready to process cards or is inoperable.
        /// * ```open``` - The security module is open and ready to process cards.
        /// </summary>
        [DataMember(Name = "security")]
        public SecurityEnum? Security { get; init; }

        public enum ChipPowerEnum
        {
            Unknown,
            Online,
            Busy,
            PoweredOff,
            NoDevice,
            HardwareError,
            NoCard
        }

        /// <summary>
        /// Specifies the state of the chip controlled by this service. Depending on the value of capabilities response,
        /// this can either be the chip on the currently inserted user card or the chip on a permanently connected chip
        /// card. This property will be null if the capability to report the state of the chip is not supported by the
        /// ID card unit device and will apply to contactless chip card readers, otherwise one of the following values:
        /// 
        /// * ```unknown``` - The state of the chip cannot be determined with the device in its current state.
        /// * ```online``` - The chip is present, powered on and online (i.e. operational, not busy processing a request
        ///   and not in an error state).
        /// * ```busy``` - The chip is present, powered on, and busy (unable to process a command at this time).
        /// * ```poweredOff``` - The chip is present, but powered off (i.e. not contacted).
        /// * ```noDevice``` - A card is currently present in the device, but has no chip.
        /// * ```hardwareError``` - The chip is present, but inoperable due to a hardware error that prevents it from
        ///   being used (e.g., MUTE, if there is an unresponsive card in the reader).
        /// * ```noCard``` - There is no card in the device.
        /// </summary>
        [DataMember(Name = "chipPower")]
        public ChipPowerEnum? ChipPower { get; init; }

        public enum ChipModuleEnum
        {
            Ok,
            Inoperable,
            Unknown
        }

        /// <summary>
        /// Specifies the state of the chip card module reader. This property will be null if reporting the chip card
        /// module status is not supported, otherwise one of the following values:
        /// 
        /// * ```ok``` - The chip card module is in a good state.
        /// * ```inoperable``` - The chip card module is inoperable.
        /// * ```unknown``` - The state of the chip card module cannot be determined.
        /// </summary>
        [DataMember(Name = "chipModule")]
        public ChipModuleEnum? ChipModule { get; init; }

        public enum MagWriteModuleEnum
        {
            Ok,
            Inoperable,
            Unknown
        }

        /// <summary>
        /// Specifies the state of the magnetic card writer. This property will be null if reporting the magnetic card
        /// writing module status is not supported, otherwise one of the following values:
        /// 
        /// * ```ok``` - The magnetic card writing module is in a good state.
        /// * ```inoperable``` - The magnetic card writing module is inoperable.
        /// * ```unknown``` - The state of the magnetic card writing module cannot be determined.
        /// </summary>
        [DataMember(Name = "magWriteModule")]
        public MagWriteModuleEnum? MagWriteModule { get; init; }

        public enum FrontImageModuleEnum
        {
            Ok,
            Inoperable,
            Unknown
        }

        /// <summary>
        /// Specifies the state of the front image reader. This property will be null if reporting the front image
        /// reading module status is not supported, otherwise one of the following values:
        /// 
        /// * ```ok``` - The front image reading module is in a good state.
        /// * ```inoperable``` - The front image reading module is inoperable.
        /// * ```unknown``` - The state of the front image reading module cannot be determined.
        /// </summary>
        [DataMember(Name = "frontImageModule")]
        public FrontImageModuleEnum? FrontImageModule { get; init; }

        public enum BackImageModuleEnum
        {
            Ok,
            Inoperable,
            Unknown
        }

        /// <summary>
        /// Specifies the state of the back image reader. This property will be null if reporting the back image
        /// reading module status is not supported, otherwise one of the following values:
        /// 
        /// * ```ok``` - The back image reading module is in a good state.
        /// * ```inoperable``` - The back image reading module is inoperable.
        /// * ```unknown``` - The state of the back image reading module cannot be determined.
        /// </summary>
        [DataMember(Name = "backImageModule")]
        public BackImageModuleEnum? BackImageModule { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(TypeEnum? Type = null, ReadTracksClass ReadTracks = null, WriteTracksClass WriteTracks = null, ChipProtocolsClass ChipProtocols = null, SecurityTypeEnum? SecurityType = null, PowerOnOptionEnum? PowerOnOption = null, PowerOffOptionEnum? PowerOffOption = null, bool? FluxSensorProgrammable = null, bool? ReadWriteAccessFromExit = null, WriteModeClass WriteMode = null, ChipPowerClass ChipPower = null, MemoryChipProtocolsClass MemoryChipProtocols = null, PositionsClass Positions = null, bool? CardTakenSensor = null)
        {
            this.Type = Type;
            this.ReadTracks = ReadTracks;
            this.WriteTracks = WriteTracks;
            this.ChipProtocols = ChipProtocols;
            this.SecurityType = SecurityType;
            this.PowerOnOption = PowerOnOption;
            this.PowerOffOption = PowerOffOption;
            this.FluxSensorProgrammable = FluxSensorProgrammable;
            this.ReadWriteAccessFromExit = ReadWriteAccessFromExit;
            this.WriteMode = WriteMode;
            this.ChipPower = ChipPower;
            this.MemoryChipProtocols = MemoryChipProtocols;
            this.Positions = Positions;
            this.CardTakenSensor = CardTakenSensor;
        }

        public enum TypeEnum
        {
            Motor,
            Swipe,
            Dip,
            LatchedDip,
            Contactless,
            IntelligentContactless,
            Permanent
        }

        /// <summary>
        /// Specifies the type of the ID card unit as one of the following:
        /// 
        /// * ```motor``` - The ID card unit is a motor driven card unit.
        /// * ```swipe``` - The ID card unit is a swipe (pull-through) card unit.
        /// * ```dip``` - The ID card unit is a dip card unit. This dip type is not capable of latching cards entered.
        /// * ```latchedDip``` - The ID card unit is a latched dip card unit. This device type is used when a dip card
        ///   unit device supports chip communication. The latch ensures the consumer cannot remove the card during chip
        ///   communication. Any card entered will automatically latch when a request to initiate a chip dialog is made
        ///   (via the [CardReader.ReadRawData](#cardreader.readrawdata) command). The
        ///   [CardReader.Move](#cardreader.move) command is used to unlatch the card.
        /// * ```contactless``` - The ID card unit is a contactless card unit, i.e. no insertion of the card is required.
        /// * ```intelligentContactless``` - The ID card unit is an intelligent contactless card unit, i.e. no insertion
        ///   of the card is required and the card unit has built-in EMV or smart card application functionality that
        ///   adheres to the EMVCo Contactless Specifications [[Ref. cardreader-3](#ref-cardreader-3)] or individual payment system's specifications. The ID card
        ///   unit is capable of performing both magnetic stripe emulation and EMV-like transactions.
        /// * ```permanent``` - The ID card unit is dedicated to a permanently housed chip card (no user interaction is
        /// available with this type of card).
        /// </summary>
        [DataMember(Name = "type")]
        public TypeEnum? Type { get; init; }

        [DataContract]
        public sealed class ReadTracksClass
        {
            public ReadTracksClass(bool? Track1 = null, bool? Track2 = null, bool? Track3 = null, bool? Watermark = null, bool? FrontTrack1 = null, bool? FrontImage = null, bool? BackImage = null, bool? Track1JIS = null, bool? Track3JIS = null, bool? Ddi = null)
            {
                this.Track1 = Track1;
                this.Track2 = Track2;
                this.Track3 = Track3;
                this.Watermark = Watermark;
                this.FrontTrack1 = FrontTrack1;
                this.FrontImage = FrontImage;
                this.BackImage = BackImage;
                this.Track1JIS = Track1JIS;
                this.Track3JIS = Track3JIS;
                this.Ddi = Ddi;
            }

            /// <summary>
            /// The card reader can access track 1.
            /// </summary>
            [DataMember(Name = "track1")]
            public bool? Track1 { get; init; }

            /// <summary>
            /// The card reader can access track 2.
            /// </summary>
            [DataMember(Name = "track2")]
            public bool? Track2 { get; init; }

            /// <summary>
            /// The card reader can access track 3.
            /// </summary>
            [DataMember(Name = "track3")]
            public bool? Track3 { get; init; }

            /// <summary>
            /// The card reader can access the Swedish watermark track.
            /// </summary>
            [DataMember(Name = "watermark")]
            public bool? Watermark { get; init; }

            /// <summary>
            /// The card reader can access front track 1.
            /// </summary>
            [DataMember(Name = "frontTrack1")]
            public bool? FrontTrack1 { get; init; }

            /// <summary>
            /// The card reader can read the front image of the card.
            /// </summary>
            [DataMember(Name = "frontImage")]
            public bool? FrontImage { get; init; }

            /// <summary>
            /// The card reader can read the back image of the card.
            /// </summary>
            [DataMember(Name = "backImage")]
            public bool? BackImage { get; init; }

            /// <summary>
            /// The card reader can access JIS I track 1.
            /// </summary>
            [DataMember(Name = "track1JIS")]
            public bool? Track1JIS { get; init; }

            /// <summary>
            /// The card reader can access JIS I track 3.
            /// </summary>
            [DataMember(Name = "track3JIS")]
            public bool? Track3JIS { get; init; }

            /// <summary>
            /// The card reader can provide dynamic digital identification of the magnetic strip.
            /// </summary>
            [DataMember(Name = "ddi")]
            public bool? Ddi { get; init; }

        }

        /// <summary>
        /// Specifies the tracks that can be read by the card reader. May be null if not applicable.
        /// </summary>
        [DataMember(Name = "readTracks")]
        public ReadTracksClass ReadTracks { get; init; }

        [DataContract]
        public sealed class WriteTracksClass
        {
            public WriteTracksClass(bool? Track1 = null, bool? Track2 = null, bool? Track3 = null, bool? FrontTrack1 = null, bool? Track1JIS = null, bool? Track3JIS = null)
            {
                this.Track1 = Track1;
                this.Track2 = Track2;
                this.Track3 = Track3;
                this.FrontTrack1 = FrontTrack1;
                this.Track1JIS = Track1JIS;
                this.Track3JIS = Track3JIS;
            }

            /// <summary>
            /// The card reader can write on track 1.
            /// </summary>
            [DataMember(Name = "track1")]
            public bool? Track1 { get; init; }

            /// <summary>
            /// The card reader can write on track 2.
            /// </summary>
            [DataMember(Name = "track2")]
            public bool? Track2 { get; init; }

            /// <summary>
            /// The card reader can write on track 3.
            /// </summary>
            [DataMember(Name = "track3")]
            public bool? Track3 { get; init; }

            /// <summary>
            /// The card reader can write on front track 1.
            /// </summary>
            [DataMember(Name = "frontTrack1")]
            public bool? FrontTrack1 { get; init; }

            /// <summary>
            /// The card reader can write on JIS I track 1.
            /// </summary>
            [DataMember(Name = "track1JIS")]
            public bool? Track1JIS { get; init; }

            /// <summary>
            /// The card reader can write on JIS I track 3.
            /// </summary>
            [DataMember(Name = "track3JIS")]
            public bool? Track3JIS { get; init; }

        }

        /// <summary>
        /// Specifies the tracks that can be written by the card reader. May be null if no tracks can be written.
        /// </summary>
        [DataMember(Name = "writeTracks")]
        public WriteTracksClass WriteTracks { get; init; }

        [DataContract]
        public sealed class ChipProtocolsClass
        {
            public ChipProtocolsClass(bool? ChipT0 = null, bool? ChipT1 = null, bool? ChipProtocolNotRequired = null, bool? ChipTypeAPart3 = null, bool? ChipTypeAPart4 = null, bool? ChipTypeB = null, bool? ChipTypeNFC = null)
            {
                this.ChipT0 = ChipT0;
                this.ChipT1 = ChipT1;
                this.ChipProtocolNotRequired = ChipProtocolNotRequired;
                this.ChipTypeAPart3 = ChipTypeAPart3;
                this.ChipTypeAPart4 = ChipTypeAPart4;
                this.ChipTypeB = ChipTypeB;
                this.ChipTypeNFC = ChipTypeNFC;
            }

            /// <summary>
            /// The card reader can handle the T=0 protocol.
            /// </summary>
            [DataMember(Name = "chipT0")]
            public bool? ChipT0 { get; init; }

            /// <summary>
            /// The card reader can handle the T=1 protocol.
            /// </summary>
            [DataMember(Name = "chipT1")]
            public bool? ChipT1 { get; init; }

            /// <summary>
            /// The carder is capable of communicating with the chip without requiring the application to specify any
            /// protocol.
            /// </summary>
            [DataMember(Name = "chipProtocolNotRequired")]
            public bool? ChipProtocolNotRequired { get; init; }

            /// <summary>
            /// The card reader can handle the ISO 14443 (Part3) Type A contactless chip card protocol.
            /// </summary>
            [DataMember(Name = "chipTypeAPart3")]
            public bool? ChipTypeAPart3 { get; init; }

            /// <summary>
            /// The card reader can handle the ISO 14443 (Part4) Type A contactless chip card protocol.
            /// </summary>
            [DataMember(Name = "chipTypeAPart4")]
            public bool? ChipTypeAPart4 { get; init; }

            /// <summary>
            /// The card reader can handle the ISO 14443 Type B contactless chip card protocol.
            /// </summary>
            [DataMember(Name = "chipTypeB")]
            public bool? ChipTypeB { get; init; }

            /// <summary>
            /// The card reader can handle the ISO 18092 (106/212/424kbps) contactless chip card protocol.
            /// </summary>
            [DataMember(Name = "chipTypeNFC")]
            public bool? ChipTypeNFC { get; init; }

        }

        /// <summary>
        /// Specifies the chip card protocols that are supported by the card reader. May be null if none are supported.
        /// </summary>
        [DataMember(Name = "chipProtocols")]
        public ChipProtocolsClass ChipProtocols { get; init; }

        public enum SecurityTypeEnum
        {
            Mm,
            Cim86
        }

        /// <summary>
        /// Specifies the type of security module as one of the following or null if the device has no security module.
        /// 
        /// * ```mm``` - The security module is a MMBox.
        /// * ```cim86``` - The security module is a CIM86.
        /// </summary>
        [DataMember(Name = "securityType")]
        public SecurityTypeEnum? SecurityType { get; init; }

        public enum PowerOnOptionEnum
        {
            Exit,
            Retain,
            ExitThenRetain,
            Transport
        }

        /// <summary>
        /// Specifies the power-on (or off) capabilities of the device hardware as one of the following options
        /// (applicable only to motor driven ID card units). May be null if the device does not support power on
        /// (or off) options.
        /// 
        /// * ```exit``` - The card will be moved to the exit position.
        /// * ```retain``` - The card will be moved to a *retain* storage unit.
        /// * ```exitThenRetain``` - The card will be moved to the exit position for a finite time, then if not taken,
        ///   the card will be moved to a *retain* storage unit. The time for which the card remains at the exit
        ///   position is vendor dependent.
        /// * ```transport``` - The card will be moved to the transport position.
        /// 
        /// If multiple *retain* storage units are present, the storage unit to which the card is retained is vendor
        /// specific.
        /// </summary>
        [DataMember(Name = "powerOnOption")]
        public PowerOnOptionEnum? PowerOnOption { get; init; }

        public enum PowerOffOptionEnum
        {
            Exit,
            Retain,
            ExitThenRetain,
            Transport
        }

        /// <summary>
        /// Specifies the power-off capabilities of the device hardware. See
        /// [powerOnOption](#common.capabilities.completion.description.cardreader.poweronoption).
        /// </summary>
        [DataMember(Name = "powerOffOption")]
        public PowerOffOptionEnum? PowerOffOption { get; init; }

        /// <summary>
        /// Specifies whether the Flux Sensor on the card unit is programmable.
        /// </summary>
        [DataMember(Name = "fluxSensorProgrammable")]
        public bool? FluxSensorProgrammable { get; init; }

        /// <summary>
        /// Specifies whether a card may be read or written after having been moved to the exit position with a
        /// [CardReader.Move](#cardreader.move) command. The card will be moved back into the card reader.
        /// </summary>
        [DataMember(Name = "readWriteAccessFromExit")]
        public bool? ReadWriteAccessFromExit { get; init; }

        [DataContract]
        public sealed class WriteModeClass
        {
            public WriteModeClass(bool? Loco = null, bool? Hico = null, bool? Auto = null)
            {
                this.Loco = Loco;
                this.Hico = Hico;
                this.Auto = Auto;
            }

            /// <summary>
            /// Supports writing of loco magnetic stripes.
            /// </summary>
            [DataMember(Name = "loco")]
            public bool? Loco { get; init; }

            /// <summary>
            /// Supports writing of hico magnetic stripes.
            /// </summary>
            [DataMember(Name = "hico")]
            public bool? Hico { get; init; }

            /// <summary>
            /// The Service is capable of automatically determining whether loco or hico magnetic stripes should be
            /// written.
            /// </summary>
            [DataMember(Name = "auto")]
            public bool? Auto { get; init; }

        }

        /// <summary>
        /// The write capabilities, with respect to whether the device can write low coercivity (loco) and/or high
        /// coercivity (hico) magnetic stripes. May be null if not applicable.
        /// </summary>
        [DataMember(Name = "writeMode")]
        public WriteModeClass WriteMode { get; init; }

        [DataContract]
        public sealed class ChipPowerClass
        {
            public ChipPowerClass(bool? Cold = null, bool? Warm = null, bool? Off = null)
            {
                this.Cold = Cold;
                this.Warm = Warm;
                this.Off = Off;
            }

            /// <summary>
            /// The card reader can power on the chip and reset it (Cold Reset).
            /// </summary>
            [DataMember(Name = "cold")]
            public bool? Cold { get; init; }

            /// <summary>
            /// The card reader can reset the chip (Warm Reset).
            /// </summary>
            [DataMember(Name = "warm")]
            public bool? Warm { get; init; }

            /// <summary>
            /// The card reader can power off the chip.
            /// </summary>
            [DataMember(Name = "off")]
            public bool? Off { get; init; }

        }

        /// <summary>
        /// The chip power management capabilities (in relation to the user or permanent chip controlled by the
        /// Service. May be null if not applicable.
        /// </summary>
        [DataMember(Name = "chipPower")]
        public ChipPowerClass ChipPower { get; init; }

        [DataContract]
        public sealed class MemoryChipProtocolsClass
        {
            public MemoryChipProtocolsClass(bool? Siemens4442 = null, bool? Gpm896 = null)
            {
                this.Siemens4442 = Siemens4442;
                this.Gpm896 = Gpm896;
            }

            /// <summary>
            /// The device supports the Siemens 4442 Card Protocol (also supported by the Gemplus GPM2K card).
            /// </summary>
            [DataMember(Name = "siemens4442")]
            public bool? Siemens4442 { get; init; }

            /// <summary>
            /// The device supports the Gemplus GPM 896 Card Protocol.
            /// </summary>
            [DataMember(Name = "gpm896")]
            public bool? Gpm896 { get; init; }

        }

        /// <summary>
        /// The memory card protocols that are supported. May be null if not applicable.
        /// </summary>
        [DataMember(Name = "memoryChipProtocols")]
        public MemoryChipProtocolsClass MemoryChipProtocols { get; init; }

        [DataContract]
        public sealed class PositionsClass
        {
            public PositionsClass(bool? Exit = null, bool? Transport = null)
            {
                this.Exit = Exit;
                this.Transport = Transport;
            }

            /// <summary>
            /// The device can move a card to the exit position. In this position, the card is accessible to the user.
            /// </summary>
            [DataMember(Name = "exit")]
            public bool? Exit { get; init; }

            /// <summary>
            /// The device can move a card to the transport. In this position, the card is not accessible to the user. A
            /// service which supports this position must also support the *exit* position.
            /// </summary>
            [DataMember(Name = "transport")]
            public bool? Transport { get; init; }

        }

        /// <summary>
        /// Specifies the target positions that are supported for the [CardReader.Move](#cardreader.move)
        /// command. This is independent of the storage units. May be null if not applicable.
        /// </summary>
        [DataMember(Name = "positions")]
        public PositionsClass Positions { get; init; }

        /// <summary>
        /// Specifies whether or not the card reader has the ability to detect when a card is taken from the exit slot
        /// by a user. If true, a [CardReader.MediaRemovedEvent](#cardreader.mediaremovedevent) will be sent when the card
        /// is removed.
        /// </summary>
        [DataMember(Name = "cardTakenSensor")]
        public bool? CardTakenSensor { get; init; }

    }


    public enum CardDataStatusEnum
    {
        DataMissing,
        DataInvalid,
        DataTooLong,
        DataTooShort,
        DataSourceNotSupported,
        DataSourceMissing
    }


    [DataContract]
    public sealed class CardDataNullableClass
    {
        public CardDataNullableClass(CardDataStatusEnum? Status = null, List<byte> Data = null)
        {
            this.Status = Status;
            this.Data = Data;
        }

        [DataMember(Name = "status")]
        public CardDataStatusEnum? Status { get; init; }

        /// <summary>
        /// Base64 encoded representation of the data. This property is null if not read.
        /// <example>O2gAUACFyEARAJAC</example>
        /// </summary>
        [DataMember(Name = "data")]
        [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
        public List<byte> Data { get; init; }

    }


    [DataContract]
    public sealed class CardDataClass
    {
        public CardDataClass(CardDataStatusEnum? Status = null, List<byte> Data = null)
        {
            this.Status = Status;
            this.Data = Data;
        }

        [DataMember(Name = "status")]
        public CardDataStatusEnum? Status { get; init; }

        /// <summary>
        /// Base64 encoded representation of the data. This property is null if not read.
        /// <example>O2gAUACFyEARAJAC</example>
        /// </summary>
        [DataMember(Name = "data")]
        [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
        public List<byte> Data { get; init; }

    }


    [DataContract]
    public sealed class ValueDetailsClass
    {
        public ValueDetailsClass(QualifierEnum? Qualifier = null, string Value = null, int? CurrencyCode = null)
        {
            this.Qualifier = Qualifier;
            this.Value = Value;
            this.CurrencyCode = CurrencyCode;
        }

        public enum QualifierEnum
        {
            Amount,
            Balance
        }

        /// <summary>
        /// Qualifies *value*. This data is defined by EMVCo as one of the following:
        /// 
        /// * ```amount``` - *value* is an Amount.
        /// * ```balance``` - *value* is a Balance.
        /// <example>amount</example>
        /// </summary>
        [DataMember(Name = "qualifier")]
        public QualifierEnum? Qualifier { get; init; }

        /// <summary>
        /// Represents the numeric value of the amount or balance (as specified by *qualifier*) to be displayed where
        /// appropriate. The format of this property is defined by EMVCo.
        /// <example>000000012345</example>
        /// </summary>
        [DataMember(Name = "value")]
        [DataTypes(Pattern = @"^[0-9]{12}$")]
        public string Value { get; init; }

        /// <summary>
        /// Represents the numeric value of the currency code as defined by ISO 4217 [[Ref. cardreader-5](#ref-cardreader-5)].
        /// <example>826</example>
        /// </summary>
        [DataMember(Name = "currencyCode")]
        [DataTypes(Minimum = 0, Maximum = 999)]
        public int? CurrencyCode { get; init; }

    }


    [DataContract]
    public sealed class EMVClessUIClass
    {
        public EMVClessUIClass(int? MessageId = null, StatusEnum? Status = null, int? HoldTime = null, ValueDetailsClass ValueDetails = null, string LanguagePreferenceData = null)
        {
            this.MessageId = MessageId;
            this.Status = Status;
            this.HoldTime = HoldTime;
            this.ValueDetails = ValueDetails;
            this.LanguagePreferenceData = LanguagePreferenceData;
        }

        /// <summary>
        /// Represents the EMVCo defined message identifier that indicates the text string to be displayed, e.g., 0x1B
        /// is the “Authorising Please Wait” message (see EMVCo Contactless Specifications for Payment Systems Book A
        /// [[Ref. cardreader-3](#ref-cardreader-3)], Section 9.4).
        /// </summary>
        [DataMember(Name = "messageId")]
        [DataTypes(Minimum = 0)]
        public int? MessageId { get; init; }

        public enum StatusEnum
        {
            NotReady,
            Idle,
            ReadyToRead,
            Processing,
            CardReadOk,
            ProcessingError
        }

        /// <summary>
        /// Represents the EMVCo defined transaction status value to be indicated through the Beep/LEDs as one of the
        /// following:
        /// 
        /// * ```notReady``` - Contactless card reader is not able to communicate with a card. This status occurs
        ///   towards the end of a contactless transaction or if the reader is not powered on.
        /// * ```idle``` - Contactless card reader is powered on, but the reader field is not yet active for
        ///   communication with a card.
        /// * ```readyToRead``` - Contactless card reader is powered on and attempting to initiate communication with a
        ///   card.
        /// * ```processing``` - Contactless card reader is in the process of reading the card.
        /// * ```cardReadOk``` - Contactless card reader was able to read a card successfully.
        /// * ```processingError``` - Contactless card reader was not able to process the card successfully.
        /// </summary>
        [DataMember(Name = "status")]
        public StatusEnum? Status { get; init; }

        /// <summary>
        /// Represents the hold time in units of 100 milliseconds for which the application should display the message
        /// before processing the next user interface data.
        /// </summary>
        [DataMember(Name = "holdTime")]
        [DataTypes(Minimum = 0)]
        public int? HoldTime { get; init; }

        [DataMember(Name = "valueDetails")]
        public ValueDetailsClass ValueDetails { get; init; }

        /// <summary>
        /// Represents the language preference (EMV Tag '5F2D') if returned by the card. If not returned, this property
        /// reports null. The application should use this data to display all messages in the specified
        /// language until the transaction concludes.
        /// <example>en</example>
        /// </summary>
        [DataMember(Name = "languagePreferenceData")]
        [DataTypes(Pattern = @"^[a-z]{2}")]
        public string LanguagePreferenceData { get; init; }

    }


    [DataContract]
    public sealed class EMVClessOutcomeClass
    {
        public EMVClessOutcomeClass(CvmEnum? Cvm = null, AlternateInterfaceEnum? AlternateInterface = null, bool? Receipt = null, EMVClessUIClass UiOutcome = null, EMVClessUIClass UiRestart = null, int? FieldOffHoldTime = null, int? CardRemovalTimeout = null, List<byte> DiscretionaryData = null)
        {
            this.Cvm = Cvm;
            this.AlternateInterface = AlternateInterface;
            this.Receipt = Receipt;
            this.UiOutcome = UiOutcome;
            this.UiRestart = UiRestart;
            this.FieldOffHoldTime = FieldOffHoldTime;
            this.CardRemovalTimeout = CardRemovalTimeout;
            this.DiscretionaryData = DiscretionaryData;
        }

        public enum CvmEnum
        {
            OnlinePIN,
            ConfirmationCodeVerified,
            Sign,
            NoCVM,
            NoCVMPreference
        }

        /// <summary>
        /// Specifies the card holder verification method (CVM) to be performed as one of the following:
        /// 
        /// * ```onlinePIN``` - Online PIN should be entered by the card holder.
        /// * ```confirmationCodeVerified``` - A confirmation code entry has been successfully done on a mobile
        ///   device.
        /// * ```sign``` - Application should obtain card holder signature.
        /// * ```noCVM``` - No CVM is required for this transaction.
        /// * ```noCVMPreference``` - There is no CVM preference, but application can follow the payment system's
        ///   rules to process the transaction.
        /// </summary>
        [DataMember(Name = "cvm")]
        public CvmEnum? Cvm { get; init; }

        public enum AlternateInterfaceEnum
        {
            Contact,
            MagneticStripe
        }

        /// <summary>
        /// This specifies the alternative interface to be used to complete a transaction if applicable as one of the following:
        /// 
        /// * ```contact``` - *txOutcome* is *tryAnotherInterface* and the contact chip interface should be used to complete a transaction.
        /// * ```magneticStripe``` - *txOutcome* is *tryAnotherInterface* and the magnetic stripe interface should be used to complete a transaction.
        /// 
        /// This will be null if *txOutcome* is not *tryAnotherInterface*.
        /// <example>magneticStripe</example>
        /// </summary>
        [DataMember(Name = "alternateInterface")]
        public AlternateInterfaceEnum? AlternateInterface { get; init; }

        /// <summary>
        /// Specifies whether a receipt should be printed.
        /// </summary>
        [DataMember(Name = "receipt")]
        public bool? Receipt { get; init; }

        /// <summary>
        /// The user interface details required to be displayed to the card holder after processing the outcome of a
        /// contactless transaction. If no user interface details are required, this will be null. Please refer
        /// to EMVCo Contactless Specifications for Payment Systems Book A [[Ref. cardreader-3](#ref-cardreader-3)], Section 6.2 for details of the data
        /// within this object.
        /// </summary>
        [DataMember(Name = "uiOutcome")]
        public EMVClessUIClass UiOutcome { get; init; }

        /// <summary>
        /// The user interface details required to be displayed to the card holder when a transaction needs to be
        /// completed with a re-tap. If no user interface details are required, this will be null.
        /// </summary>
        [DataMember(Name = "uiRestart")]
        public EMVClessUIClass UiRestart { get; init; }

        /// <summary>
        /// The application should wait for this specific hold time in units of 100 milliseconds, before re-enabling
        /// the contactless card reader by issuing either the
        /// [CardReader.EMVClessPerformTransaction](#cardreader.emvclessperformtransaction) command or the
        /// [CardReader.EMVClessIssuerUpdate](#cardreader.emvclessissuerupdate) command depending on the value of
        /// [txOutcome](#cardreader.emvclessperformtransaction.completion.properties.chip.txoutcome). For
        /// intelligent contactless card readers, the completion of this command ensures that the contactless chip
        /// card reader field is automatically turned off, so there is no need for the application to disable the
        /// field.
        /// </summary>
        [DataMember(Name = "fieldOffHoldTime")]
        [DataTypes(Minimum = 0)]
        public int? FieldOffHoldTime { get; init; }

        /// <summary>
        /// Specifies a timeout value in units of 100 milliseconds for prompting the user to remove the card.
        /// </summary>
        [DataMember(Name = "cardRemovalTimeout")]
        [DataTypes(Minimum = 0)]
        public int? CardRemovalTimeout { get; init; }

        /// <summary>
        /// Base64 encoded representation of the payment system's specific discretionary data read from the chip, in
        /// a BER-TLV format, after a contactless transaction has been completed. If discretionary data is not
        /// present, this will be null.
        /// <example>O2gAUACFyEARAJAC</example>
        /// </summary>
        [DataMember(Name = "discretionaryData")]
        [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
        public List<byte> DiscretionaryData { get; init; }

    }


    [DataContract]
    public sealed class EMVClessPerformTransactionEMVClessTxOutputDataClass
    {
        public EMVClessPerformTransactionEMVClessTxOutputDataClass(TxOutcomeEnum? TxOutcome = null, CardholderActionEnum? CardholderAction = null, List<byte> DataRead = null, EMVClessOutcomeClass ClessOutcome = null)
        {
            this.TxOutcome = TxOutcome;
            this.CardholderAction = CardholderAction;
            this.DataRead = DataRead;
            this.ClessOutcome = ClessOutcome;
        }

        public enum TxOutcomeEnum
        {
            MultipleCards,
            Approve,
            Decline,
            OnlineRequest,
            OnlineRequestCompletionRequired,
            TryAgain,
            TryAnotherInterface,
            EndApplication,
            ConfirmationRequired
        }

        /// <summary>
        /// If multiple data sources are returned, this property is the same for each one. Specifies the
        /// contactless transaction outcome as one of the following:
        /// 
        /// * ```multipleCards``` - Transaction could not be completed as more than one contactless card was tapped.
        /// * ```approve``` - Transaction was approved offline.
        /// * ```decline``` - Transaction was declined offline.
        /// * ```onlineRequest``` - Transaction was requested for online authorization.
        /// * ```onlineRequestCompletionRequired``` - Transaction requested online authorization and will be completed
        ///   after a re-tap of the card. Transaction should be completed by issuing the
        ///   [CardReader.EMVClessIssuerUpdate](#cardreader.emvclessissuerupdate) command.
        /// * ```tryAgain``` - Transaction could not be completed due to a card read error. The contactless card could
        ///   be tapped again to re-attempt the transaction.
        /// * ```tryAnotherInterface``` - Transaction could not be completed over the contactless interface. Another
        ///   interface may be suitable for this transaction (for example contact).
        /// * ```endApplication``` - Transaction cannot be completed on the contactless card due to an irrecoverable
        ///   error.
        /// * ```confirmationRequired``` - Transaction was not completed because of a requirement to allow entry of
        ///   confirmation code on a mobile device. Transaction should be completed by issuing the
        ///   [CardReader.EMVClessPerformTransaction](#cardreader.emvclessperformtransaction) after a card removal and a
        ///   re-tap of the card.
        /// 
        /// **Note:** _The values for outcome have been mapped against the EMV Entry Point Outcome structure values defined
        /// in the EMVCo Contactless Specifications for Payment Systems (Book A and B) [[Ref. cardreader-3](#ref-cardreader-3)]._
        /// </summary>
        [DataMember(Name = "txOutcome")]
        public TxOutcomeEnum? TxOutcome { get; init; }

        public enum CardholderActionEnum
        {
            None,
            Retap,
            HoldCard
        }

        /// <summary>
        /// Specifies the card holder action as one of the following:
        /// 
        /// * ```none``` - Transaction was completed. No further action is required.
        /// * ```retap``` - The contactless card should be re-tapped to complete the transaction. This value can be
        ///   returned when [txOutcome](#cardreader.emvclessperformtransaction.completion.properties.chip.txoutcome)
        ///   is *onlineRequest*, *onlineRequestCompletionRequired* or *confirmationRequired*.
        /// * ```holdCard``` - The contactless card should not be removed from the field until the transaction is
        ///   completed.
        /// </summary>
        [DataMember(Name = "cardholderAction")]
        public CardholderActionEnum? CardholderAction { get; init; }

        /// <summary>
        /// The Base64 encoded representation of the data read from the chip after a contactless transaction has been
        /// completed successfully. If the member name is
        /// [chip](#cardreader.emvclessperformtransaction.completion.properties.chip), the BER-TLV formatted data
        /// contains cryptogram tag (9F26) after a contactless chip transaction has been completed successfully. If the
        /// member name is [track1](#cardreader.emvclessperformtransaction.completion.properties.track1),
        /// [track2](#cardreader.emvclessperformtransaction.completion.properties.track2) or
        /// [track3](#cardreader.emvclessperformtransaction.completion.properties.track3) this contains the data read
        /// from the chip, i.e the value returned by the card reader device and no cryptogram tag (9F26).
        /// <example>O2gAUACFyEARAJAC</example>
        /// </summary>
        [DataMember(Name = "dataRead")]
        [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
        public List<byte> DataRead { get; init; }

        [DataMember(Name = "clessOutcome")]
        public EMVClessOutcomeClass ClessOutcome { get; init; }

    }


    [DataContract]
    public sealed class EMVClessIssuerUpdateEMVClessTxOutputDataClass
    {
        public EMVClessIssuerUpdateEMVClessTxOutputDataClass(TxOutcomeEnum? TxOutcome = null, List<byte> DataRead = null, EMVClessOutcomeClass ClessOutcome = null)
        {
            this.TxOutcome = TxOutcome;
            this.DataRead = DataRead;
            this.ClessOutcome = ClessOutcome;
        }

        public enum TxOutcomeEnum
        {
            MultipleCards,
            Approve,
            Decline,
            TryAgain,
            TryAnotherInterface
        }

        /// <summary>
        /// If multiple data sources are returned, this property is the same for each one. Specifies the
        /// contactless transaction outcome as one of the following:
        /// 
        /// * ```multipleCards``` - Transaction could not be completed as more than one contactless card was tapped.
        /// * ```approve``` - Transaction was approved offline.
        /// * ```decline``` - Transaction was declined offline.
        /// * ```tryAgain``` - Transaction could not be completed due to a card read error. The contactless card could
        ///   be tapped again to re-attempt the transaction.
        /// * ```tryAnotherInterface``` - Transaction could not be completed over the contactless interface. Another
        ///   interface may be suitable for this transaction (for example contact).
        /// 
        /// **Note:** _The values for outcome have been mapped against the EMV Entry Point Outcome structure values defined
        /// in the EMVCo Contactless Specifications for Payment Systems (Book A and B) [[Ref. cardreader-3](#ref-cardreader-3)]._
        /// </summary>
        [DataMember(Name = "txOutcome")]
        public TxOutcomeEnum? TxOutcome { get; init; }

        /// <summary>
        /// The Base64 encoded representation of the data read from the chip after a contactless transaction has been
        /// completed successfully. The BER-TLV formatted data contains cryptogram tag (9F26) after a contactless chip
        /// transaction has been completed successfully.
        /// <example>O2gAUACFyEARAJAC</example>
        /// </summary>
        [DataMember(Name = "dataRead")]
        [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
        public List<byte> DataRead { get; init; }

        [DataMember(Name = "clessOutcome")]
        public EMVClessOutcomeClass ClessOutcome { get; init; }

    }


    [DataContract]
    public sealed class StorageCapabilitiesClass
    {
        public StorageCapabilitiesClass(TypeEnum? Type = null, bool? HardwareSensors = null)
        {
            this.Type = Type;
            this.HardwareSensors = HardwareSensors;
        }

        public enum TypeEnum
        {
            Retain,
            Dispense,
            Park
        }

        /// <summary>
        /// The type of card storage. This property may be
        /// null in events if the type did not change, otherwise will be one of the following
        /// values:
        /// 
        /// * ```retain``` - The storage unit can retain cards.
        /// * ```dispense``` - The storage unit can dispense cards.
        /// * ```park``` - The storage unit can be used to temporarily store a card allowing another card to enter the
        ///   transport.
        /// </summary>
        [DataMember(Name = "type")]
        public TypeEnum? Type { get; init; }

        /// <summary>
        /// Indicates whether the storage unit has hardware sensors that can detect threshold states.
        /// This property may be null in events if it did not change.
        /// <example>true</example>
        /// </summary>
        [DataMember(Name = "hardwareSensors")]
        public bool? HardwareSensors { get; init; }

    }


    [DataContract]
    public sealed class StorageConfigurationClass
    {
        public StorageConfigurationClass(string CardID = null, int? Threshold = null)
        {
            this.CardID = CardID;
            this.Threshold = Threshold;
        }

        /// <summary>
        /// The identifier that may be used to identify the type of cards in the storage unit. This is only applicable
        /// to [dispense](#storage.getstorage.completion.properties.storage.unit1.card.capabilities.type) storage units
        /// and may be null in events if it did not change.
        /// <example>LoyaltyCard</example>
        /// </summary>
        [DataMember(Name = "cardID")]
        public string CardID { get; init; }

        /// <summary>
        /// If the threshold value is non-zero, hardware sensors in the storage unit do not trigger
        /// [Storage.StorageThresholdEvent](#storage.storagethresholdevent) events.
        /// This property may be null in events if it did not change.
        /// 
        /// If non-zero, when *count* reaches the threshold value:
        /// 
        /// * For [retain](#storage.getstorage.completion.properties.storage.unit1.card.capabilities.type) type storage
        ///   units, a [high](#storage.getstorage.completion.properties.storage.unit1.card.status.replenishmentstatus)
        ///   threshold will be sent.
        /// * For [dispense](#storage.getstorage.completion.properties.storage.unit1.card.capabilities.type) type
        ///   storage units, a
        ///   [low](#storage.getstorage.completion.properties.storage.unit1.card.status.replenishmentstatus) threshold
        ///   will be sent.
        /// <example>10</example>
        /// </summary>
        [DataMember(Name = "threshold")]
        [DataTypes(Minimum = 0)]
        public int? Threshold { get; init; }

    }


    [DataContract]
    public sealed class StorageStatusClass
    {
        public StorageStatusClass(int? InitialCount = null, int? Count = null, int? RetainCount = null, ReplenishmentStatusEnum? ReplenishmentStatus = null)
        {
            this.InitialCount = InitialCount;
            this.Count = Count;
            this.RetainCount = RetainCount;
            this.ReplenishmentStatus = ReplenishmentStatus;
        }

        /// <summary>
        /// The initial number of cards in the storage unit. This is only applicable to
        /// [dispense](#storage.getstorage.completion.properties.storage.unit1.card.capabilities.type) type storage
        /// units. This property may be null in events if it did not change.
        /// 
        /// This value is persistent.
        /// </summary>
        [DataMember(Name = "initialCount")]
        [DataTypes(Minimum = 0)]
        public int? InitialCount { get; init; }

        /// <summary>
        /// The number of cards in the storage unit.
        /// 
        /// If the storage unit type is
        /// [dispense](#storage.getstorage.completion.properties.storage.unit1.card.capabilities.type):
        /// 
        /// * This count also includes a card dispensed from the storage unit which has not been moved to either the
        ///   exit position or a [dispense](#storage.getstorage.completion.properties.storage.unit1.card.capabilities.type)
        ///   type storage unit.
        /// * This count is decremented when a card from the card storage unit is moved to the exit position or
        ///   retained. If this value reaches zero it will not decrement further but will remain at zero.
        /// 
        /// If the storage unit type is [retain](#storage.getstorage.completion.properties.storage.unit1.card.capabilities.type):
        /// 
        /// * The count is incremented when a card is moved into the storage unit.
        /// 
        /// If the storage unit type is [park](#storage.getstorage.completion.properties.storage.unit1.card.capabilities.type):
        /// 
        /// * The count will increment when a card is moved into the storage module and decremented when a card is
        ///   moved out of the storage module.
        /// 
        /// This value is persistent and may be null in events if it did not change.
        /// </summary>
        [DataMember(Name = "count")]
        [DataTypes(Minimum = 0)]
        public int? Count { get; init; }

        /// <summary>
        /// The number of cards from this storage unit which are in a
        /// [retain](#storage.getstorage.completion.properties.storage.unit1.card.capabilities.type) storage unit.
        /// 
        /// This is only applicable to
        /// [dispense](#storage.getstorage.completion.properties.storage.unit1.card.capabilities.type) type storage
        /// units.
        /// 
        /// This value is persistent and may be null in events if it did not change.
        /// </summary>
        [DataMember(Name = "retainCount")]
        [DataTypes(Minimum = 0)]
        public int? RetainCount { get; init; }

        public enum ReplenishmentStatusEnum
        {
            Ok,
            Full,
            High,
            Low,
            Empty
        }

        /// <summary>
        /// The state of the cards in the storage unit if it can be determined. Note that overall
        /// [status](#storage.getstorage.completion.properties.storage.unit1.status) of the storage unit must be 
        /// considered when deciding whether the storage unit is usable and whether replenishment status is
        /// applicable. If the overall status is *missing* this will be null.
        /// The property may also be null in events if it did not change.
        /// 
        /// The following values are possible:
        /// 
        /// * ```ok``` - The storage unit is in a good state.
        /// * ```full``` - The storage unit is full.
        /// * ```high``` - The storage unit is almost full (either sensor based or above the
        ///   [threshold](#storage.getstorage.completion.properties.storage.unit1.card.configuration.threshold)).
        /// * ```low``` - The storage unit is almost empty (either sensor based or below the
        ///   [threshold](#storage.getstorage.completion.properties.storage.unit1.card.configuration.threshold)).
        /// * ```empty``` - The storage unit is empty.
        /// </summary>
        [DataMember(Name = "replenishmentStatus")]
        public ReplenishmentStatusEnum? ReplenishmentStatus { get; init; }

    }


    [DataContract]
    public sealed class StorageClass
    {
        public StorageClass(StorageCapabilitiesClass Capabilities = null, StorageConfigurationClass Configuration = null, StorageStatusClass Status = null)
        {
            this.Capabilities = Capabilities;
            this.Configuration = Configuration;
            this.Status = Status;
        }

        [DataMember(Name = "capabilities")]
        public StorageCapabilitiesClass Capabilities { get; init; }

        [DataMember(Name = "configuration")]
        public StorageConfigurationClass Configuration { get; init; }

        [DataMember(Name = "status")]
        public StorageStatusClass Status { get; init; }

    }


    [DataContract]
    public sealed class StorageStatusSetClass
    {
        public StorageStatusSetClass(int? InitialCount = null)
        {
            this.InitialCount = InitialCount;
        }

        /// <summary>
        /// The number of cards in the storage unit at the last replenishment. If specified,
        /// [count](#storage.getstorage.completion.properties.storage.unit1.card.status.count) is set to match
        /// this value and
        /// [retainCount](#storage.getstorage.completion.properties.storage.unit1.card.status.retaincount) is set to
        /// zero.
        /// </summary>
        [DataMember(Name = "initialCount")]
        [DataTypes(Minimum = 0)]
        public int? InitialCount { get; init; }

    }


    [DataContract]
    public sealed class StorageSetClass
    {
        public StorageSetClass(StorageConfigurationClass Configuration = null, StorageStatusSetClass Status = null)
        {
            this.Configuration = Configuration;
            this.Status = Status;
        }

        [DataMember(Name = "configuration")]
        public StorageConfigurationClass Configuration { get; init; }

        [DataMember(Name = "status")]
        public StorageStatusSetClass Status { get; init; }

    }


}
