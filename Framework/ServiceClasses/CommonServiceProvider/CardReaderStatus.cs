/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Common
{
    public sealed class CardReaderStatusClass(
        CardReaderStatusClass.MediaEnum Media,
        CardReaderStatusClass.SecurityEnum Security,
        CardReaderStatusClass.ChipPowerEnum ChipPower,
        CardReaderStatusClass.ChipModuleEnum ChipModule,
        CardReaderStatusClass.MagWriteModuleEnum MagWriteModule,
        CardReaderStatusClass.FrontImageModuleEnum FrontImageModule,
        CardReaderStatusClass.BackImageModuleEnum BackImageModule,
        CardReaderStatusClass.DispenserEnum Dispenser = CardReaderStatusClass.DispenserEnum.NotSupported,
        CardReaderStatusClass.DispenserTransportEnum DispenserTransport = CardReaderStatusClass.DispenserTransportEnum.NotSupported,
        CardReaderStatusClass.ShutterEnum Shutter = CardReaderStatusClass.ShutterEnum.NotSupported) : StatusBase
    {
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

        public enum SecurityEnum
        {
            NotSupported,
            NotReady,
            Open
        }

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

        public enum ChipModuleEnum
        {
            Ok,
            Inoperable,
            Unknown,
            NotSupported
        }

        public enum MagWriteModuleEnum
        {
            Ok,
            Inoperable,
            Unknown,
            NotSupported
        }

        public enum FrontImageModuleEnum
        {
            Ok,
            Inoperable,
            Unknown,
            NotSupported
        }

        public enum BackImageModuleEnum
        {
            Ok,
            Inoperable,
            Unknown,
            NotSupported
        }

        public enum DispenserEnum
        {
            Ok,
            State,
            Stop,
            Unknown,
            NotSupported,
        }

        public enum DispenserTransportEnum
        {
            Ok,
            Inoperative,
            Unknown,
            NotSupported,
        }

        public enum ShutterEnum
        {
            Closed,
            Open,
            Jammed,
            Unknown,
            NotSupported,
        }

        /// <summary>
        /// Specifies the transport/exit position media state as one of the following values:
        /// 
        /// * ```notSupported``` - Capability to report media position is not supported by the device (e.g. a typical
        ///   swipe reader or contactless chip card reader).
        /// * ```unknown``` - The media state cannot be determined with the device in its current state (e.g. the value
        ///   of device is *noDevice*, *powerOff*, *offline* or *hardwareError*.
        /// * ```present``` - Media is present in the device, not in the entering position and not jammed. On the
        ///   latched dip device, this indicates that the card is present in the device and the card is unlatched.
        /// * ```notPresent``` - Media is not present in the device and not at the entering position.
        /// * ```jammed``` - Media is jammed in the device; operator intervention is required.
        /// * ```entering``` - Media is at the entry/exit slot of a motorized device.
        /// * ```latched``` - Media is present and latched in a latched dip card unit. This means the card can be used
        ///   for chip card dialog.
        /// </summary>
        public MediaEnum Media 
        { 
            get { return media; }
            set
            {
                if (media != value)
                {
                    media = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private MediaEnum media = Media;

        /// <summary>
        /// Specifies the state of the security module as one of the following:
        /// 
        /// * ```notSupported``` - No security module is available.
        /// * ```notReady``` - The security module is not ready to process cards or is inoperable.
        /// * ```notPresent``` - The security module is open and ready to process cards.
        /// </summary>
        public SecurityEnum Security 
        {
            get { return security; }
            set
            {
                if (security != value)
                {
                    security = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private SecurityEnum security = Security;

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
        public ChipPowerEnum ChipPower 
        { 
            get { return chipPower; }
            set
            {
                if (chipPower != value)
                {
                    chipPower = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ChipPowerEnum chipPower = ChipPower;

        /// <summary>
        /// Specifies the state of the chip card module reader as one of the following:
        /// 
        /// * ```ok``` - The chip card module is in a good state.
        /// * ```inoperable``` - The chip card module is inoperable.
        /// * ```unknown``` - The state of the chip card module cannot be determined.
        /// * ```notSupported``` - Reporting the chip card module status is not supported.
        /// </summary>
        public ChipModuleEnum ChipModule 
        { 
            get { return chipModule; }
            set
            {
                if (chipModule != value)
                {  
                    chipModule = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ChipModuleEnum chipModule = ChipModule;

        /// <summary>
        /// Specifies the state of the magnetic card writer as one of the following:
        /// 
        /// * ```ok``` - The magnetic card writing module is in a good state.
        /// * ```inoperable``` - The magnetic card writing module is inoperable.
        /// * ```unknown``` - The state of the magnetic card writing module cannot be determined.
        /// * ```notSupported``` - Reporting the magnetic card writing module status is not supported.
        /// </summary>
        public MagWriteModuleEnum MagWriteModule 
        { 
            get { return magWriteModule; }
            set
            {
                if (magWriteModule != value)
                {
                    magWriteModule = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private MagWriteModuleEnum magWriteModule = MagWriteModule;

        /// <summary>
        /// Specifies the state of the front image reader as one of the following:
        /// 
        /// * ```ok``` - The front image reading module is in a good state.
        /// * ```inoperable``` - The front image reading module is inoperable.
        /// * ```unknown``` - The state of the front image reading module cannot be determined.
        /// * ```notSupported``` - Reporting the front image reading module status is not supported.
        /// </summary>
        public FrontImageModuleEnum FrontImageModule 
        {
            get { return frontImageModule; }
            set
            {
                if (frontImageModule != value)
                {
                    frontImageModule = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private FrontImageModuleEnum frontImageModule = FrontImageModule;

        /// <summary>
        /// Specifies the state of the back image reader as one of the following:
        /// 
        /// * ```ok``` - The back image reading module is in a good state.
        /// * ```inoperable``` - The back image reading module is inoperable.
        /// * ```unknown``` - The state of the back image reading module cannot be determined.
        /// * ```notSupported``` - Reporting the back image reading module status is not supported.
        /// </summary>
        public BackImageModuleEnum BackImageModule 
        {
            get { return backImageModule; }
            set
            {
                if (backImageModule != value)
                {
                    backImageModule = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private BackImageModuleEnum backImageModule = BackImageModule;

        /// <summary>
        /// Specifies the state of the dispensing card units as one of the following values. This property will be null
        /// in [Common.Status](#common.status) if no card dispensing functionality is supported and will also be null in
        /// [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
        /// 
        /// * ```ok``` - All dispense card units present are in a good state.
        /// * ```state``` - One or more of the dispense card units is in a low, empty or inoperative condition. Items
        ///   can still be dispensed from at least one of the card units.
        /// * ```stop``` - Due to a card unit failure dispensing is impossible. No items can be dispensed because all of
        ///   the card units are in an empty or inoperative condition.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the card units cannot be
        ///   determined.
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
        private DispenserEnum dispenser = Dispenser;

        /// <summary>
        /// Specifies the state of the dispenserTransport mechanism as one of the following values. This property will
        /// be null in [Common.Status](#common.status) if card dispensing functionality is not supported and will also
        /// be null in [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
        /// 
        /// * ```ok``` - The dispenser transport is in a good state.
        /// * ```inoperative``` - The dispenser transport is inoperative due to a hardware failure or media jam.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the dispenser transport cannot be
        ///   determined.
        /// </summary>
        public DispenserTransportEnum DispenserTransport 
        {
            get { return dispenserTransport; }
            set
            {
                if (dispenserTransport != value)
                {
                    dispenserTransport = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private DispenserTransportEnum dispenserTransport = DispenserTransport;

        /// <summary>
        /// Specifies the state of the shutter as one of the following values. This property will be null in
        /// [Common.Status](#common.status) if the device has no shutter or shutter state reporting is not supported. It
        /// will also be null in [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
        /// 
        /// * ```closed``` - The shutter is closed.
        /// * ```open``` - The shutter is opened.
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
    }
}
