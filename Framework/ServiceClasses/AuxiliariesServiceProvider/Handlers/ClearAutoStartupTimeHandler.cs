/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    public partial class ClearAutoStartupTimeHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandleClearAutoStartupTime(IClearAutoStartupTimeEvents events, ClearAutoStartupTimeCommand clearAutoStartupTime, CancellationToken cancel)
        {
            if (Device.AuxiliariesCapabilities.AutoStartupMode == AuxiliariesCapabilitiesClass.AutoStartupModes.NotAvailable)
            {
                return new(MessageHeader.CompletionCodeEnum.UnsupportedCommand, "Device reported no supported AutoStartupModes.");
            }

            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.ClearAutoStartupTime()");

            var result = await Device.ClearAutoStartupTime(cancel);

            Logger.Log(Constants.DeviceClass, $"AuxiliariesDev.ClearAutoStartupTime() -> {result.CompletionCode}");

            return new(
                result.CompletionCode, 
                result.ErrorDescription);
        }
    }
}
