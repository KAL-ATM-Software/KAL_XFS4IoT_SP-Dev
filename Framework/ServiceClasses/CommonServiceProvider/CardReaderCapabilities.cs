/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    public sealed class CardReaderCapabilitiesClass(
        CardReaderCapabilitiesClass.DeviceTypeEnum Type,
        CardReaderCapabilitiesClass.ReadableDataTypesEnum ReadTracks,
        CardReaderCapabilitiesClass.WritableDataTypesEnum WriteTracks,
        CardReaderCapabilitiesClass.ChipProtocolsEnum ChipProtocols,
        CardReaderCapabilitiesClass.SecurityTypeEnum SecurityType,
        CardReaderCapabilitiesClass.PowerOptionEnum PowerOnOption,
        CardReaderCapabilitiesClass.PowerOptionEnum PowerOffOption,
        bool FluxSensorProgrammable,
        bool ReadWriteAccessFollowingExit,
        CardReaderCapabilitiesClass.WriteMethodsEnum WriteMode,
        CardReaderCapabilitiesClass.ChipPowerOptionsEnum ChipPower,
        CardReaderCapabilitiesClass.MemoryChipProtocolsEnum MemoryChipProtocols,
        CardReaderCapabilitiesClass.PositionsEnum Positions,
        bool CardTakenSensor)
    {
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
            Track1 = 1 << 0,
            Track2 = 1 << 1,
            Track3 = 1 << 2,
            Track1Front = 1 << 3,
            FrontImage = 1 << 4,
            BackImage = 1 << 5,
            Track1JIS = 1 << 6,
            Track3JIS = 1 << 7,
            Ddi = 1 << 8,
            Watermark = 1 << 9,
        }

        [Flags]
        public enum WritableDataTypesEnum
        {
            NotSupported = 0,
            Track1 = 1 << 0,
            Track2 = 1 << 1,
            Track3 = 1 << 2,
            Track1Front = 1 << 3,
            Track1JIS = 1 << 4,
            Track3JIS = 1 << 5,
        }

        [Flags]
        public enum ChipProtocolsEnum
        {
            NotSupported = 0,
            T0 = 1 << 0,
            T1 = 1 << 1,
            NotRequired = 1 << 2,
            TypeAPart3 = 1 << 3,
            TypeAPart4 = 1 << 4,
            TypeB = 1 << 5,
            TypeNFC = 1 << 6,
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
            Exit,
            Retain,
            ExitThenRetain,
            Transport
        }

        [Flags]
        public enum WriteMethodsEnum
        {
            NotSupported = 0,
            Loco = 1 << 0,
            Hico = 1 << 1,
            Auto = 1 << 2,
        }

        [Flags]
        public enum ChipPowerOptionsEnum
        {
            NotSupported = 0,
            Cold = 1 << 0,
            Warm = 1 << 1,
            Off = 1 << 2,
        }

        [Flags]
        public enum MemoryChipProtocolsEnum
        {
            NotSupported = 0,
            Siemens4442 = 1 << 0,
            Gpm896 = 1 << 1,
        }

        [Flags]
        public enum PositionsEnum
        {
            NotSupported = 0,
            Exit = 1 << 0,
            Transport = 1 << 1,
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
        public DeviceTypeEnum Type { get; init; } = Type;

        /// <summary>
        /// Specifies the tracks that can be read by the card reader.
        /// </summary>
        public ReadableDataTypesEnum ReadTracks { get; init; } = ReadTracks;

        /// <summary>
        /// Specifies the tracks that can be read by the card reader.
        /// </summary>
        public WritableDataTypesEnum WriteTracks { get; init; } = WriteTracks;


        public ChipProtocolsEnum ChipProtocols { get; init; } = ChipProtocols;

        /// <summary>
        /// Specifies the type of security module as one of the following:
        /// 
        /// * ```notSupported``` - The device has no security module.
        /// * ```mm``` - The security module is a MMBox.
        /// * ```cim86```` - The security module is a CIM86.
        /// </summary>
        public SecurityTypeEnum SecurityType { get; init; } = SecurityType;


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
        public PowerOptionEnum PowerOnOption { get; init; } = PowerOnOption;

        /// <summary>
        /// Specifies the power-off capabilities of the device hardware.
        /// </summary>
        public PowerOptionEnum PowerOffOption { get; init; } = PowerOffOption;

        /// <summary>
        /// Specifies whether the Flux Sensor on the card unit is programmable.
        /// </summary>
        public bool FluxSensorProgrammable { get; init; } = FluxSensorProgrammable;

        /// <summary>
        /// Specifies whether a card may be read or written after having been pushed to the exit slot with a
        /// CardReader.EjectCard command. The card will be retracted back into the card reader.
        /// </summary>
        public bool ReadWriteAccessFollowingExit { get; init; } = ReadWriteAccessFollowingExit;


        /// <summary>
        /// The write capabilities, with respect to whether the device can write low coercivity (loco) and/or high
        /// coercivity (hico) magnetic stripes as a combination of the following:
        /// </summary>
        public WriteMethodsEnum WriteMode { get; init; } = WriteMode;


        /// <summary>
        /// The chip power management capabilities (in relation to the user or permanent chip controlled by the
        /// service, as a combination of the following:
        /// </summary>
        public ChipPowerOptionsEnum ChipPower { get; init; } = ChipPower;

        /// <summary>
        /// The memory card protocols that are supported, as a combination of the following:
        /// </summary>
        public MemoryChipProtocolsEnum MemoryChipProtocols { get; init; } = MemoryChipProtocols;

        /// <summary>
        /// Specifies the target position that is supported for the eject operation, as a combination of the following:
        /// </summary>
        public PositionsEnum Positions { get; init; } = Positions;

        /// <summary>
        /// Specifies whether or not the card reader has the ability to detect when a card is taken from the exit slot
        /// by a user.If true, a CardReader.MediaTakenEvent will be sent when the card is removed.
        /// </summary>
        public bool CardTakenSensor { get; init; } = CardTakenSensor;
    }
}