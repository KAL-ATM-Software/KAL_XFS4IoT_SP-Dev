/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Auxiliaries.Commands;
using XFS4IoT.Auxiliaries.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Auxiliaries
{
    public partial class SetAutoStartUpTimeHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandleSetAutoStartUpTime(ISetAutoStartUpTimeEvents events, SetAutoStartUpTimeCommand setAutostartupTime, CancellationToken cancel)
        {
            if (Device.AuxiliariesCapabilities.AutoStartupMode == AuxiliariesCapabilitiesClass.AutoStartupModes.NotAvailable)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.UnsupportedCommand, 
                    "Device reported no supported AutoStartupModes.");
            }

            if (setAutostartupTime?.Payload?.Mode is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    "Auto startup Mode not specified.");
            }

            if (setAutostartupTime?.Payload?.StartTime is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    "Auto startup StartTime not specified.");
            }

            if (!Device.AuxiliariesCapabilities.AutoStartupMode.HasFlag(setAutostartupTime.Payload.Mode switch
            {
                SetAutoStartUpTimeCommand.PayloadData.ModeEnum.Specific => AuxiliariesCapabilitiesClass.AutoStartupModes.Specific,
                SetAutoStartUpTimeCommand.PayloadData.ModeEnum.Daily => AuxiliariesCapabilitiesClass.AutoStartupModes.Daily,
                SetAutoStartUpTimeCommand.PayloadData.ModeEnum.Weekly => AuxiliariesCapabilitiesClass.AutoStartupModes.Weekly,
                _ => throw new NotImplementedException("Invalid mode supplied in HandleSetAutostartupTime")
            }))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.UnsupportedData, 
                    "Supplied auto startup mode is not supported by the device.");
            }

            StartupTime startupTime;

            if(setAutostartupTime.Payload.Mode is SetAutoStartUpTimeCommand.PayloadData.ModeEnum.Specific)
            {
                if (setAutostartupTime.Payload.StartTime.Year is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData, 
                        "StartTime.Year must be provided when Mode = Specific.");
                }
                if (setAutostartupTime.Payload.StartTime.Month is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData, 
                        "StartTime.Month must be provided when Mode = Specific.");
                }
                if (setAutostartupTime.Payload.StartTime.Day is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData, 
                        "StartTime.Day must be provided when Mode = Specific.");
                }
                if (setAutostartupTime.Payload.StartTime.Hour is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData, 
                        "StartTime.Hour must be provided when Mode = Specific.");
                }
                if (setAutostartupTime.Payload.StartTime.Minute is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData, 
                        "StartTime.Minute must be provided when Mode = Specific.");
                }
                startupTime = new StartupTime(setAutostartupTime.Payload.StartTime.Year, setAutostartupTime.Payload.StartTime.Month, null, setAutostartupTime.Payload.StartTime.Day, setAutostartupTime.Payload.StartTime.Hour, setAutostartupTime.Payload.StartTime.Minute);
            }
            else if(setAutostartupTime.Payload.Mode is SetAutoStartUpTimeCommand.PayloadData.ModeEnum.Daily)
            {
                if (setAutostartupTime.Payload.StartTime.Hour is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData, 
                        "StartTime.Hour must be provided when Mode = Daily.");
                }
                if (setAutostartupTime.Payload.StartTime.Minute is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData, 
                        "StartTime.Minute must be provided when Mode = Daily.");
                }
                startupTime = new StartupTime(null, null, null, null, setAutostartupTime.Payload.StartTime.Hour, setAutostartupTime.Payload.StartTime.Minute);
            }
            else //ModeEnum.Weekly
            {
                if (setAutostartupTime.Payload.StartTime.DayOfWeek is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData, 
                        "StartTime.DayOfWeek must be provided when Mode = Weekly.");
                }
                if (setAutostartupTime.Payload.StartTime.Hour is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData, 
                        "StartTime.Hour must be provided when Mode = Weekly.");
                }
                if (setAutostartupTime.Payload.StartTime.Minute is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData, 
                        "StartTime.Minute must be provided when Mode = Weekly.");
                }
                startupTime = new StartupTime(null, null, setAutostartupTime.Payload.StartTime.DayOfWeek switch
                {
                    XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Saturday => DayOfWeek.Saturday,
                    XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Sunday => DayOfWeek.Sunday,
                    XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Monday => DayOfWeek.Monday,
                    XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Tuesday => DayOfWeek.Tuesday,
                    XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Wednesday => DayOfWeek.Wednesday,
                    XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Thursday => DayOfWeek.Thursday,
                    XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Friday => DayOfWeek.Friday,
                    _ => null,
                }, null, setAutostartupTime.Payload.StartTime.Hour, setAutostartupTime.Payload.StartTime.Minute);
            }


            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.SetAutostartupTime()");

            var result = await Device.SetAutostartupTime(new(startupTime, setAutostartupTime.Payload.Mode switch
            {
                SetAutoStartUpTimeCommand.PayloadData.ModeEnum.Specific => AutoStartupTimeModeEnum.Specific,
                SetAutoStartUpTimeCommand.PayloadData.ModeEnum.Daily => AutoStartupTimeModeEnum.Daily,
                SetAutoStartUpTimeCommand.PayloadData.ModeEnum.Weekly => AutoStartupTimeModeEnum.Weekly,
                _ => throw new NotImplementedException("Unexpected ModeEnum in SetAutoStartupTime. " + setAutostartupTime.Payload.Mode)
            }), cancel);

            Logger.Log(Constants.DeviceClass, $"AuxiliariesDev.SetAutostartupTime() -> {result.CompletionCode}");

            return new(
                result.CompletionCode, 
                result.ErrorDescription);
        }
    }
}
