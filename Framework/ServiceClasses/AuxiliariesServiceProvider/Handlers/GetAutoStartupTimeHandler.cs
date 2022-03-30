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

        private async Task<GetAutoStartupTimeCompletion.PayloadData> HandleGetAutoStartupTime(IGetAutoStartupTimeEvents events, GetAutoStartupTimeCommand getAutoStartupTime, CancellationToken cancel)
        {
            if (Device.AuxiliariesCapabilities.AutoStartupMode == AuxiliariesCapabilities.AutoStartupModes.NotAvailable)
                return new GetAutoStartupTimeCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedCommand, "Device reported no supported AutoStartupModes.");
            
            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.GetAutoStartupTime()");

            var result = await Device.GetAutoStartupTime(cancel);

            Logger.Log(Constants.DeviceClass, $"AuxiliariesDev.GetAutoStartupTime() -> {result.CompletionCode}");

            if (result.CompletionCode == XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.Success)
                return new GetAutoStartupTimeCompletion.PayloadData(result.CompletionCode, result.ErrorDescription, result.Mode switch
                {
                    AutoStartupTimeModeEnum.Specific => GetAutoStartupTimeCompletion.PayloadData.ModeEnum.Specific,
                    AutoStartupTimeModeEnum.Daily => GetAutoStartupTimeCompletion.PayloadData.ModeEnum.Daily,
                    AutoStartupTimeModeEnum.Weekly => GetAutoStartupTimeCompletion.PayloadData.ModeEnum.Weekly,
                    _ => GetAutoStartupTimeCompletion.PayloadData.ModeEnum.Clear
                }, result.StartupTime == null ? null : new XFS4IoT.Auxiliaries.SystemTimeClass(result.StartupTime.Year, result.StartupTime.Month, result.StartupTime.DayOfWeek switch
                {
                    DayOfWeek.Saturday => XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Saturday,
                    DayOfWeek.Sunday => XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Sunday,
                    DayOfWeek.Monday => XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Monday,
                    DayOfWeek.Tuesday => XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Tuesday,
                    DayOfWeek.Wednesday => XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Wednesday,
                    DayOfWeek.Thursday => XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Thursday,
                    DayOfWeek.Friday => XFS4IoT.Auxiliaries.SystemTimeClass.DayOfWeekEnum.Friday,
                    _ => null
                }, result.StartupTime.Day, result.StartupTime.Hour, result.StartupTime.Minute));

            return new GetAutoStartupTimeCompletion.PayloadData(result.CompletionCode,
                                                                  result.ErrorDescription);
        }

    }
}
