/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;

namespace XFS4IoTFramework.Common
{
    [CommandHandlerAsync]
    public partial class StatusHandler
    {
        private Task<StatusCompletion.PayloadData> HandleStatus(IStatusEvents events, StatusCommand status, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CommonDev.Status()");
            var result = Device.Status();
            Logger.Log(Constants.DeviceClass, $"CommonDev.Status() -> {result.CompletionCode}");

            return Task.FromResult(result);
        }
    }
}
