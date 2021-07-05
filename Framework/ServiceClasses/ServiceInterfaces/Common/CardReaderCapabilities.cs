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
    /// <summary>
    /// CardReaderCapabilitiesClass
    /// Store device capabilites for the card reader device
    /// </summary>
    public sealed class CardReaderCapabilitiesClass
    {
        public CardReaderCapabilitiesClass(DeviceTypeEnum Type,
                                           ReadableDataTypesEnum ReadTracks,
                                           WritableDataTypesEnum WriteTracks,
                                           ChipProtocolsEnum ChipProtocols,
                                           int MaxCardCount,
                                           SecurityTypeEnum SecurityType,
                                           PowerOptionEnum PowerOnOption,
                                           PowerOptionEnum PowerOffOption,
                                           bool FluxSensorProgrammable,
                                           bool ReadWriteAccessFollowingEject,
                                           WriteMethodsEnum WriteMode,
                                           ChipPowerOptionsEnum ChipPower,
                                           MemoryChipProtocolsEnum MemoryChipProtocols,
                                           EjectPositionsEnum EjectPosition,
                                           int NumberParkingStations)
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

        public enum DeviceTypeEnum
        {
            Motor,
            Swipe,
            Dip,
            LatchedDip,
            Contactless,
            IntelligentContactless,
            Permanent
        }

        [Flags]
        public enum ReadableDataTypesEnum
        {
            NotSupported = 0,
            Track1 = 0x0001,
            Track2 = 0x0002,
            Track3 = 0x0004,
            Track1Front = 0x0080,
            FrontImage = 0x0100,
            BackImage = 0x0200,
            Track1JIS = 0x0400,
            Track3JIS = 0x0800,
            Ddi = 0x4000,
            Watermark = 0x8000,
        }

        [Flags]
        public enum WritableDataTypesEnum
        {
            NotSupported = 0,
            Track1 = 0x0001,
            Track2 = 0x0002,
            Track3 = 0x0004,
            Track1Front = 0x0080,
            Track1JIS = 0x0400,
            Track3JIS = 0x0800,
        }

        [Flags]
        public enum ChipProtocolsEnum
        {
            NotSupported = 0,
            T0 = 0x0001,
            T1 = 0x0002,
            NotRequired = 0x0004,
            TypeAPart3 = 0x0008,
            TypeAPart4 = 0x0010,
            TypeB = 0x0020,
            TypeNFC = 0x0040,
        }

        public enum SecurityTypeEnum
        {
            NotSupported,
            Mm,
            Cim86
        }

        public enum PowerOptionEnum
        {
            NoAction,
            Eject,
            Retain,
            EjectThenRetain,
            ReadPosition
        }

        [Flags]
        public enum WriteMethodsEnum
        {
            NotSupported = 0,
            Loco = 0x0002,
            Hico = 0x0004,
            Auto = 0x0008,
        }

        [Flags]
        public enum ChipPowerOptionsEnum
        {
            NotSupported = 0,
            Cold = 0x0002,
            Warm = 0x0004,
            Off = 0x0008,
        }

        [Flags]
        public enum MemoryChipProtocolsEnum
        {
            NotSupported = 0,
            Siemens4442 = 0x0001,
            Gpm896 = 0x0002,
        }

        [Flags]
        public enum EjectPositionsEnum
        {
            NotSupported = 0,
            Exit = 0x0001,
            Transport = 0x0002,
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
        public DeviceTypeEnum Type { get; init; }

        /// <summary>
        /// Specifies the tracks that can be read by the card reader.
        /// </summary>
        public ReadableDataTypesEnum ReadTracks { get; init; }

        /// <summary>
        /// Specifies the tracks that can be read by the card reader.
        /// </summary>
        public WritableDataTypesEnum WriteTracks { get; init; }

        
        public ChipProtocolsEnum ChipProtocols { get; init; }

        /// <summary>
        /// Specifies the maximum numbers of cards that the retain bin and card stacker module bin can hold (zero if not
        /// available).
        /// </summary>
        public int MaxCardCount { get; init; }

        /// <summary>
        /// Specifies the type of security module as one of the following:
        /// 
        /// * ```notSupported``` - The device has no security module.
        /// * ```mm``` - The security module is a MMBox.
        /// * ```cim86```` - The security module is a CIM86.
        /// </summary>
        public SecurityTypeEnum SecurityType { get; init; }
 

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
        public PowerOptionEnum PowerOnOption { get; init; }

        /// <summary>
        /// Specifies the power-off capabilities of the device hardware.
        /// </summary>
        public PowerOptionEnum PowerOffOption { get; init; }

        /// <summary>
        /// Specifies whether the Flux Sensor on the card unit is programmable.
        /// </summary>
        public bool FluxSensorProgrammable { get; init; }

        /// <summary>
        /// Specifies whether a card may be read or written after having been pushed to the exit slot with a
        /// CardReader.EjectCard command. The card will be retracted back into the card reader.
        /// </summary>
        public bool ReadWriteAccessFollowingEject { get; init; }


        /// <summary>
        /// The write capabilities, with respect to whether the device can write low coercivity (loco) and/or high
        /// coercivity (hico) magnetic stripes as a combination of the following:
        /// </summary>
        public WriteMethodsEnum WriteMode { get; init; }


        /// <summary>
        /// The chip power management capabilities (in relation to the user or permanent chip controlled by the
        /// service, as a combination of the following:
        /// </summary>
        public ChipPowerOptionsEnum ChipPower { get; init; }

        /// <summary>
        /// The memory card protocols that are supported, as a combination of the following:
        /// </summary>
        public MemoryChipProtocolsEnum MemoryChipProtocols { get; init; }

        /// <summary>
        /// Specifies the target position that is supported for the eject operation, as a combination of the following:
        /// </summary>
        public EjectPositionsEnum EjectPosition { get; init; }

        /// <summary>
        /// Specifies the number of supported parking stations or card stackers. If a zero value is specified there is
        /// no parking station and no card stacker module supported.
        /// </summary>
        public int NumberParkingStations { get; init; }
    }
}