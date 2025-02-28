/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Auxiliaries
{
    // The classes used by the device interface for an Input/Output parameters

    public enum AutoStartupTimeModeEnum
    {
        Clear,
        Specific,
        Daily,
        Weekly
    }

    public class StartupTime(int? Year, int? Month, DayOfWeek? DayOfWeek, int? Day, int? Hour, int? Minute)
    {
        public int? Year { get; init; } = Year;
        public int? Month { get; init; } = Month;
        public DayOfWeek? DayOfWeek { get; init; } = DayOfWeek;
        public int? Day { get; init; } = Day;
        public int? Hour { get; init; } = Hour;
        public int? Minute { get; init; } = Minute;
    }

    /// <summary>
    /// SetAutostartupTimeRequest
    /// Provide startup time information
    /// </summary>
    /// <remarks>
    /// SetAutostartupTimeRequest
    /// Sets the auto startup configuration
    /// </remarks>
    /// <param name="StartupTime">Auto startup time</param>
    /// <param name="Mode">Auto startup mode</param>
    public sealed class SetAutostartupTimeRequest(StartupTime StartupTime, AutoStartupTimeModeEnum Mode)
    {
        public StartupTime StartupTime { get; init; } = StartupTime;
        public AutoStartupTimeModeEnum Mode { get; init; } = Mode;
    }

    /// <summary>
    /// GetAutostartupTimeResult
    /// Provide startup time information
    /// </summary>
    public sealed class GetAutostartupTimeResult : DeviceResult
    {

        /// <summary>
        /// GetAutostartupTimeResult
        /// Retrieve the auto startup configuration
        /// </summary>
        /// <param name="CompletionCode">Completion code</param>
        /// <param name="StartupTime">Auto startup time</param>
        /// <param name="Mode">Auto startup mode</param>
        public GetAutostartupTimeResult(
            MessageHeader.CompletionCodeEnum CompletionCode, 
            StartupTime StartupTime, 
            AutoStartupTimeModeEnum Mode)
            : base(CompletionCode, null)
        {
            this.StartupTime = StartupTime;
            this.Mode = Mode;
        }
        public GetAutostartupTimeResult(
            MessageHeader.CompletionCodeEnum CompletionCode, 
            string ErrorDescription)
            : base(CompletionCode, ErrorDescription)
        {
            this.StartupTime = null;
            this.Mode = null;
        }

        public StartupTime StartupTime { get; init; }
        public AutoStartupTimeModeEnum? Mode { get; init; }
    }

    public sealed class SetAuxiliariesRequest
    {
        public enum SetDoorEnum
        {
            Bolt, Unbolt
        }

        public enum SetVandalShieldEnum
        {
            Closed, Open, Service, Keyboard
        }

        public enum SetOpenClosedIndicatorEnum
        {
            Open, Closed
        }

        public enum SetAuxiliaryOnOff
        {
            On, Off
        }

        public enum SetUpsEnum
        {
            Engage, Disengage
        }

        public SetDoorEnum? SafeDoor { get; set; }
        public SetVandalShieldEnum? VandalShield { get; set; }
        public SetDoorEnum? FrontCabinetDoor { get; set; }
        public SetDoorEnum? RearCabinetDoor { get; set; }
        public SetDoorEnum? LeftCabinetDoor { get; set; }
        public SetDoorEnum? RightCabinetDoor { get; set; }
        public SetOpenClosedIndicatorEnum? OpenClosedIndicator { get; set; }
        public SetAuxiliaryOnOff? FasciaLight { get; set; }
        public AuxiliariesStatusClass.AudioRateEnum? AudioRate { get; set; }
        public AuxiliariesStatusClass.AudioSignalEnum? AudioSignal { get; set; }
        public SetAuxiliaryOnOff? Heating { get; set; }
        public SetAuxiliaryOnOff? DisplayBackLight { get; set; }
        public SetAuxiliaryOnOff? SignageDisplay { get; set; }
        public int? Volume { get; set; }
        public SetUpsEnum? Ups { get; set; }
        public SetAuxiliaryOnOff? AudibleAlarm { get; set; }
        public AuxiliariesStatusClass.EnhancedAudioControlEnum? EnhancedAudioControl { get; set; }
        public AuxiliariesStatusClass.EnhancedAudioControlEnum? EnhancedMicrophoneControl { get; set; }
        public int? MicrophoneVolume { get; set; }
    }
}
