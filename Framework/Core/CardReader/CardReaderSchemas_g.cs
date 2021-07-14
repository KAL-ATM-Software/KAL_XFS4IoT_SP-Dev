/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
        public StatusClass(MediaEnum? Media = null, RetainBinEnum? RetainBin = null, SecurityEnum? Security = null, int? NumberCards = null, ChipPowerEnum? ChipPower = null, ChipModuleEnum? ChipModule = null, MagWriteModuleEnum? MagWriteModule = null, FrontImageModuleEnum? FrontImageModule = null, BackImageModuleEnum? BackImageModule = null, List<ParkingStationMediaEnum> ParkingStationMedia = null)
        {
            this.Media = Media;
            this.RetainBin = RetainBin;
            this.Security = Security;
            this.NumberCards = NumberCards;
            this.ChipPower = ChipPower;
            this.ChipModule = ChipModule;
            this.MagWriteModule = MagWriteModule;
            this.FrontImageModule = FrontImageModule;
            this.BackImageModule = BackImageModule;
            this.ParkingStationMedia = ParkingStationMedia;
        }

        public enum MediaEnum
        {
            NotSupported,
            Unknown,
            Present,
            NotPresent,
            Jammed,
            Entering,
            Latched
        }

        /// <summary>
        /// Specifies the media state of the device as one of the following values. This status is independent of any
        /// media in the parking stations.
        /// 
        /// * ```notSupported``` - Capability to report media position is not supported by the device (e.g. a typical
        ///   swipe reader or contactless chip card reader).
        /// * ```unknown``` - The media state cannot be determined with the device in its current state (e.g. the value
        ///   of device is *noDevice*, *powerOff*, *offline* or *hardwareError*.
        /// * ```present``` - Media is present in the device, not in the entering position and not jammed. A card in a
        ///   parking station is not considered to be present. On the latched dip device, this indicates that the card
        ///   is present in the device and the card is unlatched.
        /// * ```notPresent``` - Media is not present in the device and not at the entering position.
        /// * ```jammed``` - Media is jammed in the device; operator intervention is required.
        /// * ```entering``` - Media is at the entry/exit slot of a motorized device.
        /// * ```latched``` - Media is present and latched in a latched dip card unit. This means the card can be used
        ///   for chip card dialog.
        /// </summary>
        [DataMember(Name = "media")]
        public MediaEnum? Media { get; init; }

        public enum RetainBinEnum
        {
            NotSupported,
            Ok,
            Full,
            High,
            Missing
        }

        /// <summary>
        /// Specifies the state of the retain bin.
        /// </summary>
        [DataMember(Name = "retainBin")]
        public RetainBinEnum? RetainBin { get; init; }

        public enum SecurityEnum
        {
            NotSupported,
            Ready,
            Open
        }

        /// <summary>
        /// Specifies the state of the security unit as one of the following:
        /// 
        /// * ```notSupported``` - No security module is available.
        /// * ```notReady``` - The security module is not ready to process cards or is inoperable.
        /// * ```notPresent``` - The security module is open and ready to process cards.
        /// </summary>
        [DataMember(Name = "security")]
        public SecurityEnum? Security { get; init; }

        /// <summary>
        /// The number of cards retained; applicable only to motor driven card units, for non-motorized card units this
        /// value is zero. This value is persistent it is reset to zero by the
        /// [CardReader.ResetCount](#cardreader.resetcount) command.
        /// </summary>
        [DataMember(Name = "numberCards")]
        public int? NumberCards { get; init; }

        public enum ChipPowerEnum
        {
            NotSupported,
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
        /// card. The state of the chip is one of the following:
        /// 
        /// * ```notSupported``` - Capability to report the state of the chip is not supported by the ID card unit
        ///   device. This value is returned for contactless chip card readers.
        /// * ```unknown``` - The state of the chip cannot be determined with the device in its current state.
        /// * ```online``` - The chip is present, powered on and online (i.e. operational, not busy processing a request
        ///   and not in an error state).
        /// * ```busy``` - The chip is present, powered on, and busy (unable to process an Execute command at this time).
        /// * ```poweredOff``` - The chip is present, but powered off (i.e. not contacted).
        /// * ```noDevice``` - A card is currently present in the device, but has no chip.
        /// * ```hardwareError``` - The chip is present, but inoperable due to a hardware error that prevents it from
        ///   being used (e.g. MUTE, if there is an unresponsive card in the reader).
        /// * ```noCard``` - There is no card in the device.
        /// </summary>
        [DataMember(Name = "chipPower")]
        public ChipPowerEnum? ChipPower { get; init; }

        public enum ChipModuleEnum
        {
            Ok,
            Inoperable,
            Unknown,
            NotSupported
        }

        /// <summary>
        /// Specifies the state of the chip card module reader as one of the following:
        /// 
        /// * ```ok``` - The chip card module is in a good state.
        /// * ```inoperable``` - The chip card module is inoperable.
        /// * ```unknown``` - The state of the chip card module cannot be determined.
        /// * ```notSupported``` - Reporting the chip card module status is not supported.
        /// </summary>
        [DataMember(Name = "chipModule")]
        public ChipModuleEnum? ChipModule { get; init; }

        public enum MagWriteModuleEnum
        {
            Ok,
            Inoperable,
            Unknown,
            NotSupported
        }

        /// <summary>
        /// Specifies the state of the magnetic card writer as one of the following:
        /// 
        /// * ```ok``` - The magnetic card writing module is in a good state.
        /// * ```inoperable``` - The magnetic card writing module is inoperable.
        /// * ```unknown``` - The state of the magnetic card writing module cannot be determined.
        /// * ```notSupported``` - Reporting the magnetic card writing module status is not supported.
        /// </summary>
        [DataMember(Name = "magWriteModule")]
        public MagWriteModuleEnum? MagWriteModule { get; init; }

        public enum FrontImageModuleEnum
        {
            Ok,
            Inoperable,
            Unknown,
            NotSupported
        }

        /// <summary>
        /// Specifies the state of the front image reader as one of the following:
        /// 
        /// * ```ok``` - The front image reading module is in a good state.
        /// * ```inoperable``` - The front image reading module is inoperable.
        /// * ```unknown``` - The state of the front image reading module cannot be determined.
        /// * ```notSupported``` - Reporting the front image reading module status is not supported.
        /// </summary>
        [DataMember(Name = "frontImageModule")]
        public FrontImageModuleEnum? FrontImageModule { get; init; }

        public enum BackImageModuleEnum
        {
            Ok,
            Inoperable,
            Unknown,
            NotSupported
        }

        /// <summary>
        /// Specifies the state of the back image reader as one of the following:
        /// 
        /// * ```ok``` - The back image reading module is in a good state.
        /// * ```inoperable``` - The back image reading module is inoperable.
        /// * ```unknown``` - The state of the back image reading module cannot be determined.
        /// * ```notSupported``` - Reporting the back image reading module status is not supported.
        /// </summary>
        [DataMember(Name = "backImageModule")]
        public BackImageModuleEnum? BackImageModule { get; init; }

        public enum ParkingStationMediaEnum
        {
            Present,
            NotPresent,
            Jammed,
            NotSupported,
            Unknown
        }

        /// <summary>
        /// An array which contains the states of the parking stations or card stacker module. This is omitted if no
        /// parking station and no card stacker module is supported. Each status can be one of the following:
        /// 
        /// * ```present``` - Media is present in the parking station, and not jammed.
        /// * ```notPresent``` - Media is not present in the parking station.
        /// * ```jammed``` - The parking station is jammed; operator intervention is required.
        /// * ```notSupported``` - Reporting the media status in a parking station is not supported by the device.
        /// * ```unknown``` - The media state cannot be determined.
        /// </summary>
        [DataMember(Name = "parkingStationMedia")]
        public List<ParkingStationMediaEnum> ParkingStationMedia { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(TypeEnum? Type = null, ReadTracksClass ReadTracks = null, WriteTracksClass WriteTracks = null, ChipProtocolsClass ChipProtocols = null, int? MaxCardCount = null, SecurityTypeEnum? SecurityType = null, PowerOnOptionEnum? PowerOnOption = null, PowerOffOptionEnum? PowerOffOption = null, bool? FluxSensorProgrammable = null, bool? ReadWriteAccessFollowingEject = null, WriteModeClass WriteMode = null, ChipPowerClass ChipPower = null, MemoryChipProtocolsClass MemoryChipProtocols = null, EjectPositionClass EjectPosition = null, int? NumberParkingStations = null)
        {
            this.Type = Type;
            this.ReadTracks = ReadTracks;
            this.WriteTracks = WriteTracks;
            this.ChipProtocols = ChipProtocols;
            this.MaxCardCount = MaxCardCount;
            this.SecurityType = SecurityType;
            this.PowerOnOption = PowerOnOption;
            this.PowerOffOption = PowerOffOption;
            this.FluxSensorProgrammable = FluxSensorProgrammable;
            this.ReadWriteAccessFollowingEject = ReadWriteAccessFollowingEject;
            this.WriteMode = WriteMode;
            this.ChipPower = ChipPower;
            this.MemoryChipProtocols = MemoryChipProtocols;
            this.EjectPosition = EjectPosition;
            this.NumberParkingStations = NumberParkingStations;
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
        ///   [CardReader.EjectCard](#cardreader.ejectcard) command is used to unlatch the card.
        /// * ```contactless``` - The ID card unit is a contactless card unit, i.e. no insertion of the card is required.
        /// * ```intelligentContactless``` - The ID card unit is an intelligent contactless card unit, i.e. no insertion
        ///   of the card is required and the card unit has built-in EMV or smart card application functionality that
        ///   adheres to the EMVCo Contactless Specifications or individual payment system's specifications. The ID card
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
        /// Specifies the tracks that can be read by the card reader.
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
            /// The card reader can access front track 1.
            /// </summary>
            [DataMember(Name = "frontTrack1")]
            public bool? FrontTrack1 { get; init; }

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

        }

        /// <summary>
        /// Specifies the tracks that can be read by the card reader.
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
            /// The card reader can handle the T=0 protocol.
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
        /// Specifies the chip card protocols that are supported by the card reader.
        /// </summary>
        [DataMember(Name = "chipProtocols")]
        public ChipProtocolsClass ChipProtocols { get; init; }

        /// <summary>
        /// Specifies the maximum numbers of cards that the retain bin and card stacker module bin can hold (zero if not
        /// available).
        /// </summary>
        [DataMember(Name = "maxCardCount")]
        public int? MaxCardCount { get; init; }

        public enum SecurityTypeEnum
        {
            NotSupported,
            Mm,
            Cim86
        }

        /// <summary>
        /// Specifies the type of security module as one of the following:
        /// 
        /// * ```notSupported``` - The device has no security module.
        /// * ```mm``` - The security module is a MMBox.
        /// * ```cim86```` - The security module is a CIM86.
        /// </summary>
        [DataMember(Name = "securityType")]
        public SecurityTypeEnum? SecurityType { get; init; }

        public enum PowerOnOptionEnum
        {
            NoAction,
            Eject,
            Retain,
            EjectThenRetain,
            ReadPosition
        }

        /// <summary>
        /// Specifies the power-on (or off) capabilities of the device hardware as one of the following options
        /// (applicable only to motor driven ID card units):
        /// 
        /// * ```noAction``` - No actions are supported by the device.
        /// * ```eject``` - The card will be ejected.
        /// * ```retain``` - The card will be retained.
        /// * ```ejectThenRetain``` - The card will be ejected for a finite time, then if not taken, the card will be
        ///   retained. The time for which the card remains ejected is vendor dependent.
        /// * ```readPosition``` - The card will be moved to the read position.
        /// </summary>
        [DataMember(Name = "powerOnOption")]
        public PowerOnOptionEnum? PowerOnOption { get; init; }

        public enum PowerOffOptionEnum
        {
            NoAction,
            Eject,
            Retain,
            EjectThenRetain,
            ReadPosition
        }

        /// <summary>
        /// Specifies the power-off capabilities of the device hardware. See
        /// [powerOnOption](#common.capabilities.completion.properties.cardreader.poweronoption).
        /// </summary>
        [DataMember(Name = "powerOffOption")]
        public PowerOffOptionEnum? PowerOffOption { get; init; }

        /// <summary>
        /// Specifies whether the Flux Sensor on the card unit is programmable.
        /// </summary>
        [DataMember(Name = "fluxSensorProgrammable")]
        public bool? FluxSensorProgrammable { get; init; }

        /// <summary>
        /// Specifies whether a card may be read or written after having been pushed to the exit slot with a
        /// [CardReader.EjectCard](#cardreader.ejectcard)  command. The card will be retracted back into the card reader.
        /// </summary>
        [DataMember(Name = "readWriteAccessFollowingEject")]
        public bool? ReadWriteAccessFollowingEject { get; init; }

        [DataContract]
        public sealed class WriteModeClass
        {
            public WriteModeClass(bool? NotSupported = null, bool? Loco = null, bool? Hico = null, bool? Auto = null)
            {
                this.NotSupported = NotSupported;
                this.Loco = Loco;
                this.Hico = Hico;
                this.Auto = Auto;
            }

            /// <summary>
            /// Does not support writing of magnetic stripes.
            /// </summary>
            [DataMember(Name = "notSupported")]
            public bool? NotSupported { get; init; }

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
            /// Service Provider is capable of automatically determining whether loco or hico magnetic stripes should be
            /// written.
            /// </summary>
            [DataMember(Name = "auto")]
            public bool? Auto { get; init; }

        }

        /// <summary>
        /// The write capabilities, with respect to whether the device can write low coercivity (loco) and/or high
        /// coercivity (hico) magnetic stripes as a combination of the following:
        /// </summary>
        [DataMember(Name = "writeMode")]
        public WriteModeClass WriteMode { get; init; }

        [DataContract]
        public sealed class ChipPowerClass
        {
            public ChipPowerClass(bool? NotSupported = null, bool? Cold = null, bool? Warm = null, bool? Off = null)
            {
                this.NotSupported = NotSupported;
                this.Cold = Cold;
                this.Warm = Warm;
                this.Off = Off;
            }

            /// <summary>
            /// The card reader cannot handle chip power management.
            /// </summary>
            [DataMember(Name = "notSupported")]
            public bool? NotSupported { get; init; }

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
        /// service, as a combination of the following:
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
        /// The memory card protocols that are supported, as a combination of the following:
        /// </summary>
        [DataMember(Name = "memoryChipProtocols")]
        public MemoryChipProtocolsClass MemoryChipProtocols { get; init; }

        [DataContract]
        public sealed class EjectPositionClass
        {
            public EjectPositionClass(bool? Exit = null, bool? Transport = null)
            {
                this.Exit = Exit;
                this.Transport = Transport;
            }

            /// <summary>
            /// The device can eject a card to the exit position, from which the user can remove it.
            /// </summary>
            [DataMember(Name = "exit")]
            public bool? Exit { get; init; }

            /// <summary>
            /// The device can eject a card to the transport just behind the exit position, from which the user cannot
            /// remove it. The device which supports this must also support the
            /// [exit](#common.capabilities.completion.properties.cardreader.ejectposition.exit) position.
            /// </summary>
            [DataMember(Name = "transport")]
            public bool? Transport { get; init; }

        }

        /// <summary>
        /// Specifies the target position that is supported for the eject operation, as a combination of the following:
        /// </summary>
        [DataMember(Name = "ejectPosition")]
        public EjectPositionClass EjectPosition { get; init; }

        /// <summary>
        /// Specifies the number of supported parking stations or card stackers. If a zero value is specified there is
        /// no parking station and no card stacker module supported.
        /// </summary>
        [DataMember(Name = "numberParkingStations")]
        public int? NumberParkingStations { get; init; }

    }


    public enum CardDataStatusEnum
    {
        Ok,
        DataMissing,
        DataInvalid,
        DataTooLong,
        DataTooShort,
        DataSourceNotSupported,
        DataSourceMissing
    }


    [DataContract]
    public sealed class CardDataClass
    {
        public CardDataClass(CardDataStatusEnum? Status = null, string Data = null)
        {
            this.Status = Status;
            this.Data = Data;
        }


        [DataMember(Name = "status")]
        public CardDataStatusEnum? Status { get; init; }

        /// <summary>
        /// Base64 encoded representation of the data
        /// </summary>
        [DataMember(Name = "data")]
        public string Data { get; init; }

    }


    [DataContract]
    public sealed class EMVClessUIClass
    {
        public EMVClessUIClass(int? MessageId = null, StatusEnum? Status = null, int? HoldTime = null, ValueQualifierEnum? ValueQualifier = null, string Value = null, string CurrencyCode = null, string LanguagePreferenceData = null)
        {
            this.MessageId = MessageId;
            this.Status = Status;
            this.HoldTime = HoldTime;
            this.ValueQualifier = ValueQualifier;
            this.Value = Value;
            this.CurrencyCode = CurrencyCode;
            this.LanguagePreferenceData = LanguagePreferenceData;
        }

        /// <summary>
        /// Represents the EMVCo defined message identifier that indicates the text string to be displayed, e.g., 0x1B
        /// is the “Authorising Please Wait” message (see EMVCo Contactless Specifications for Payment Systems Book A,
        /// Section 9.4).
        /// </summary>
        [DataMember(Name = "messageId")]
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
        public int? HoldTime { get; init; }

        public enum ValueQualifierEnum
        {
            Amount,
            Balance,
            NotApplicable
        }

        /// <summary>
        /// Qualifies
        /// [value](#cardreader.emvclessperformtransaction.completion.properties.track1.clessoutcome.uioutcome.value).
        /// This data is defined by EMVCo as either “Amount” or “Balance” as one of the following:
        /// 
        /// * ```amount``` - *value* is an Amount.
        /// * ```balance``` - *value* is a Balance.
        /// * ```notApplicable``` - *value* is neither of the above.
        /// </summary>
        [DataMember(Name = "valueQualifier")]
        public ValueQualifierEnum? ValueQualifier { get; init; }

        /// <summary>
        /// Represents the value of the amount or balance (as specified by
        /// [valueQualifier](#cardreader.emvclessperformtransaction.completion.properties.track1.clessoutcome.uioutcome.valuequalifier))
        /// to be displayed where appropriate.
        /// </summary>
        [DataMember(Name = "value")]
        public string Value { get; init; }

        /// <summary>
        /// Represents the numeric value of currency code as per ISO 4217.
        /// </summary>
        [DataMember(Name = "currencyCode")]
        public string CurrencyCode { get; init; }

        /// <summary>
        /// Represents the language preference (EMV Tag ‘5F2D’) if returned by the card. The application should use this
        /// data to display all messages in the specified language until the transaction concludes.
        /// </summary>
        [DataMember(Name = "languagePreferenceData")]
        public string LanguagePreferenceData { get; init; }

    }


    [DataContract]
    public sealed class EMVClessTxOutputDataClass
    {
        public EMVClessTxOutputDataClass(TxOutcomeEnum? TxOutcome = null, CardholderActionEnum? CardholderAction = null, string DataRead = null, ClessOutcomeClass ClessOutcome = null)
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
        /// If multiple data sources are returned, this parameter should be the same for each one. Specifies the
        /// contactless transaction outcome as one of the following flags:
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
        /// * ```confirmationRequired``` - Transaction was not completed as a result of a requirement to allow entry of
        ///   confirmation code on a mobile
        /// device. Transaction should be completed by issuing the
        /// [CardReader.EMVClessPerformTransaction](#cardreader.emvclessperformtransaction) after a card removal and a
        /// re-tap of the card.
        /// 
        /// NOTE: The values for outcome have been mapped against the EMV Entry Point Outcome structure values defined
        /// in the EMVCo Specifications for Contactless Payment Systems (Book A and B).
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
        /// Specifies the cardholder action as one of the following:
        /// 
        /// * ```none``` - Transaction was completed. No further action is required.
        /// * ```retap``` - The contactless card should be re-tapped to complete the transaction. This value can be
        ///   returned when [txOutcome](#cardreader.emvclessperformtransaction.completion.properties.track1.txoutcome)
        ///   is *onlineRequestCompletionRequired* or *confirmationRequired*.
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
        /// from the chip, i.e the value returned by the card reader device and no cryptogram tag (9F26). This value is
        /// terminated with a single null character and cannot contain UNICODE characters.
        /// </summary>
        [DataMember(Name = "dataRead")]
        public string DataRead { get; init; }

        [DataContract]
        public sealed class ClessOutcomeClass
        {
            public ClessOutcomeClass(CvmEnum? Cvm = null, AlternateInterfaceEnum? AlternateInterface = null, bool? Receipt = null, EMVClessUIClass UiOutcome = null, EMVClessUIClass UiRestart = null, int? FieldOffHoldTime = null, int? CardRemovalTimeout = null, string DiscretionaryData = null)
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
            /// Specifies the cardholder verification method (CVM) to be performed as one of the following:
            /// 
            /// * ```onlinePIN``` - Online PIN should be entered by the cardholder.
            /// * ```confirmationCodeVerified``` - A confirmation code entry has been successfully done on a mobile
            ///   device.
            /// * ```sign``` - Application should obtain cardholder signature.
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
            /// If [txOutcome](#cardreader.emvclessperformtransaction.completion.properties.track1.txoutcome) is not
            /// *tryAnotherInterface*, this should be ignored. If *txOutcome* is *tryAnotherInterface*, this specifies
            /// the alternative interface to be used to complete a transaction as one of the following:
            /// 
            /// * ```contact``` - Contact chip interface should be used to complete a transaction.
            /// * ```magneticStripe``` - Magnetic stripe interface should be used to complete a transaction.
            /// </summary>
            [DataMember(Name = "alternateInterface")]
            public AlternateInterfaceEnum? AlternateInterface { get; init; }

            /// <summary>
            /// Specifies whether a receipt should be printed. True indicates that a receipt is required.
            /// </summary>
            [DataMember(Name = "receipt")]
            public bool? Receipt { get; init; }

            /// <summary>
            /// The user interface details required to be displayed to the cardholder after processing the outcome of a
            /// contactless transaction. If no user interface details are required, this will be omitted. Please refer
            /// to EMVCo Contactless Specifications for Payment Systems Book A, Section 6.2 for details of the data
            /// within this object.
            /// </summary>
            [DataMember(Name = "uiOutcome")]
            public EMVClessUIClass UiOutcome { get; init; }

            /// <summary>
            /// The user interface details required to be displayed to the cardholder when a transaction needs to be
            /// completed with a re-tap. If no user interface details are required, this will be omitted.
            /// </summary>
            [DataMember(Name = "uiRestart")]
            public EMVClessUIClass UiRestart { get; init; }

            /// <summary>
            /// The application should wait for this specific hold time in units of 100 milliseconds, before re-enabling
            /// the contactless card reader by issuing either the
            /// [CardReader.EMVClessPerformTransaction](#cardreader.emvclessperformtransaction) command or the
            /// [CardReader.EMVClessIssuerUpdate](#cardreader.emvclessissuerupdate) command depending on the value of
            /// [txOutcome](#cardreader.emvclessperformtransaction.completion.properties.track1.txoutcome). For
            /// intelligent contactless card readers, the completion of this command ensures that the contactless chip
            /// card reader field is automatically turned off, so there is no need for the application to disable the
            /// field.
            /// </summary>
            [DataMember(Name = "fieldOffHoldTime")]
            public int? FieldOffHoldTime { get; init; }

            /// <summary>
            /// Specifies a timeout value in units of 100 milliseconds for prompting the user to remove the card.
            /// </summary>
            [DataMember(Name = "cardRemovalTimeout")]
            public int? CardRemovalTimeout { get; init; }

            /// <summary>
            /// Base64 encoded representation of the payment system's specific discretionary data read from the chip, in
            /// a BER-TLV format, after a contactless transaction has been completed. If discretionary data is not
            /// present, this will be omitted.
            /// </summary>
            [DataMember(Name = "discretionaryData")]
            public string DiscretionaryData { get; init; }

        }

        /// <summary>
        /// The Entry Point Outcome specified in EMVCo Specifications for Contactless Payment Systems (Book A and B).
        /// This can be omitted for contactless chip card readers that do not follow EMVCo Entry Point Specifications.
        /// </summary>
        [DataMember(Name = "clessOutcome")]
        public ClessOutcomeClass ClessOutcome { get; init; }

    }


}
