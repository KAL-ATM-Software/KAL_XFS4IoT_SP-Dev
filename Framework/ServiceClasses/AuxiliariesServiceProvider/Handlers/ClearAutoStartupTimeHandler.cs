/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * ClearAutoStartupTimeHandler.cs uses automatically generated parts.
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

        private async Task<ClearAutoStartupTimeCompletion.PayloadData> HandleClearAutoStartupTime(IClearAutoStartupTimeEvents events, ClearAutoStartupTimeCommand clearAutoStartupTime, CancellationToken cancel)
        {
            if (Device.AuxiliariesCapabilities.AutoStartupMode == AuxiliariesCapabilities.AutoStartupModes.NotAvailable)
                return new ClearAutoStartupTimeCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedCommand, "Device reported no supported AutoStartupModes.");

            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.ClearAutoStartupTime()");

            var result = await Device.ClearAutoStartupTime(cancel);

            Logger.Log(Constants.DeviceClass, $"AuxiliariesDev.ClearAutoStartupTime() -> {result.CompletionCode}");

            return new ClearAutoStartupTimeCompletion.PayloadData(result.CompletionCode,
                                                                  result.ErrorDescription);
        }

    }
}
