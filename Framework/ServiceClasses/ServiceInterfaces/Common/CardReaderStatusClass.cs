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
    public sealed class CardReaderStatusClass
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
            Ready,
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

        /// <summary>
        /// CardReaderStatusClass
        /// Store device status for the card reader device
        /// </summary>
        public CardReaderStatusClass(MediaEnum Media, 
                                     SecurityEnum Security, 
                                     ChipPowerEnum ChipPower, 
                                     ChipModuleEnum ChipModule, 
                                     MagWriteModuleEnum MagWriteModule, 
                                     FrontImageModuleEnum FrontImageModule, 
                                     BackImageModuleEnum BackImageModule)
        {
            this.Media = Media;
            this.Security = Security;
            this.ChipPower = ChipPower;
            this.ChipModule = ChipModule;
            this.MagWriteModule = MagWriteModule;
            this.FrontImageModule = FrontImageModule;
            this.BackImageModule = BackImageModule;
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
        public MediaEnum Media { get; set; }

        /// <summary>
        /// Specifies the state of the security module as one of the following:
        /// 
        /// * ```notSupported``` - No security module is available.
        /// * ```notReady``` - The security module is not ready to process cards or is inoperable.
        /// * ```notPresent``` - The security module is open and ready to process cards.
        /// </summary>
        public SecurityEnum Security { get; set; }

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
        public ChipPowerEnum ChipPower { get; set; }

        /// <summary>
        /// Specifies the state of the chip card module reader as one of the following:
        /// 
        /// * ```ok``` - The chip card module is in a good state.
        /// * ```inoperable``` - The chip card module is inoperable.
        /// * ```unknown``` - The state of the chip card module cannot be determined.
        /// * ```notSupported``` - Reporting the chip card module status is not supported.
        /// </summary>
        public ChipModuleEnum ChipModule { get; set; }

        /// <summary>
        /// Specifies the state of the magnetic card writer as one of the following:
        /// 
        /// * ```ok``` - The magnetic card writing module is in a good state.
        /// * ```inoperable``` - The magnetic card writing module is inoperable.
        /// * ```unknown``` - The state of the magnetic card writing module cannot be determined.
        /// * ```notSupported``` - Reporting the magnetic card writing module status is not supported.
        /// </summary>
        public MagWriteModuleEnum MagWriteModule { get; set; }

        
        /// <summary>
        /// Specifies the state of the front image reader as one of the following:
        /// 
        /// * ```ok``` - The front image reading module is in a good state.
        /// * ```inoperable``` - The front image reading module is inoperable.
        /// * ```unknown``` - The state of the front image reading module cannot be determined.
        /// * ```notSupported``` - Reporting the front image reading module status is not supported.
        /// </summary>
        public FrontImageModuleEnum FrontImageModule { get; set; }

        

        /// <summary>
        /// Specifies the state of the back image reader as one of the following:
        /// 
        /// * ```ok``` - The back image reading module is in a good state.
        /// * ```inoperable``` - The back image reading module is inoperable.
        /// * ```unknown``` - The state of the back image reading module cannot be determined.
        /// * ```notSupported``` - Reporting the back image reading module status is not supported.
        /// </summary>
        public BackImageModuleEnum BackImageModule { get; set; }

    }
}
