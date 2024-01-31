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
using XFS4IoTFramework.Common;
using XFS4IoT.Auxiliaries.Commands;
using XFS4IoT.Auxiliaries.Completions;

namespace XFS4IoTFramework.Auxiliaries
{
    public partial class GetAutoStartupTimeHandler
    {

        private async Task<GetAutoStartUpTimeCompletion.PayloadData> HandleGetAutoStartupTime(IGetAutoStartupTimeEvents events, GetAutoStartUpTimeCommand getAutoStartupTime, CancellationToken cancel)
        {
            if (Device.AuxiliariesCapabilities.AutoStartupMode == AuxiliariesCapabilitiesClass.AutoStartupModes.NotAvailable)
                return new GetAutoStartUpTimeCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedCommand, "Device reported no supported AutoStartupModes.");
            
            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.GetAutoStartupTime()");

            var result = await Device.GetAutoStartupTime(cancel);

            Logger.Log(Constants.DeviceClass, $"AuxiliariesDev.GetAutoStartupTime() -> {result.CompletionCode}");

            if (result.CompletionCode == XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.Success)
            {
                return new GetAutoStartUpTimeCompletion.PayloadData(
                    CompletionCode: result.CompletionCode,
                    ErrorDescription: result.ErrorDescription,
                    Mode: result.Mode switch
                    {
                        AutoStartupTimeModeEnum.Specific => GetAutoStartUpTimeCompletion.PayloadData.ModeEnum.Specific,
                        AutoStartupTimeModeEnum.Daily => GetAutoStartUpTimeCompletion.PayloadData.ModeEnum.Daily,
                        AutoStartupTimeModeEnum.Weekly => GetAutoStartUpTimeCompletion.PayloadData.ModeEnum.Weekly,
                        _ => null,
                    },
                    StartTime: result.StartupTime == null ?
                               null :
                               new(Year: result.StartupTime.Year,
                                   Month: result.StartupTime.Month,
                                   DayOfWeek: result.StartupTime.DayOfWeek switch
                                   {
                                       DayOfWeek.Saturday => XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Saturday,
                                       DayOfWeek.Sunday => XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Sunday,
                                       DayOfWeek.Monday => XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Monday,
                                       DayOfWeek.Tuesday => XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Tuesday,
                                       DayOfWeek.Wednesday => XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Wednesday,
                                       DayOfWeek.Thursday => XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Thursday,
                                       DayOfWeek.Friday => XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Friday,
                                       _ => null,
                                   },
                                   Day: result.StartupTime.Day,
                                   Hour: result.StartupTime.Hour,
                                   Minute: result.StartupTime.Minute));
            }

            return new GetAutoStartUpTimeCompletion.PayloadData(CompletionCode:result.CompletionCode,
                                                                ErrorDescription: result.ErrorDescription);
        }

    }
}
