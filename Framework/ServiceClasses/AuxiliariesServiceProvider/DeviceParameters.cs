/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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

    public class StartupTime
    {
        public StartupTime(int? Year, int? Month, DayOfWeek? DayOfWeek, int? Day, int? Hour, int? Minute)
        {
            this.Year = Year;
            this.Month = Month;
            this.DayOfWeek = DayOfWeek;
            this.Day = Day;
            this.Hour = Hour;
            this.Minute = Minute;
        }

        public int? Year { get; init; }
        public int? Month { get; init; }
        public DayOfWeek? DayOfWeek { get; init; }
        public int? Day { get; init; }
        public int? Hour { get; init; }
        public int? Minute { get; init; }
    }

    /// <summary>
    /// SetAutostartupTimeRequest
    /// Provide startup time information
    /// </summary>
    public sealed class SetAutostartupTimeRequest
    {
        /// <summary>
        /// SetAutostartupTimeRequest
        /// Sets the auto startup configuration
        /// </summary>
        /// <param name="StartupTime">Auto startup time</param>
        /// <param name="Mode">Auto startup mode</param>
        public SetAutostartupTimeRequest(StartupTime StartupTime, AutoStartupTimeModeEnum Mode)
        {
            this.StartupTime = StartupTime;
            this.Mode = Mode;
        }

        public StartupTime StartupTime { get; init; }
        public AutoStartupTimeModeEnum Mode { get; init; }
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
        public GetAutostartupTimeResult(MessagePayload.CompletionCodeEnum CompletionCode, StartupTime StartupTime, AutoStartupTimeModeEnum Mode)
            : base(CompletionCode, null)
        {
            this.StartupTime = StartupTime;
            this.Mode = Mode;
        }
        public GetAutostartupTimeResult(MessagePayload.CompletionCodeEnum CompletionCode, string ErrorDescription)
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
        public AuxiliariesStatus.AudioRateEnum? AudioRate { get; set; }
        public AuxiliariesStatus.AudioSignalEnum? AudioSignal { get; set; }
        public SetAuxiliaryOnOff? Heating { get; set; }
        public SetAuxiliaryOnOff? DisplayBackLight { get; set; }
        public SetAuxiliaryOnOff? SignageDisplay { get; set; }
        public int? Volume { get; set; }
        public SetUpsEnum? Ups { get; set; }
        public SetAuxiliaryOnOff? AudibleAlarm { get; set; }
        public AuxiliariesStatus.EnhancedAudioControlEnum? EnhancedAudioControl { get; set; }
        public AuxiliariesStatus.EnhancedAudioControlEnum? EnhancedMicrophoneControl { get; set; }
        public int? MicrophoneVolume { get; set; }
    }
}
