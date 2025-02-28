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
using XFS4IoT.Camera.Commands;
using XFS4IoT.Camera.Completions;

namespace XFS4IoTFramework.Camera
{
    public partial class ResetHandler
    {

        private async Task<CommandResult<MessagePayloadBase>> HandleReset(IResetEvents events, ResetCommand reset, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CameraDev.ResetDeviceAsync()");
            var result = await Device.ResetDeviceAsync(cancel);
            Logger.Log(Constants.DeviceClass, $"CameraDev.ResetDeviceAsync() -> {result.CompletionCode}");

            return new(
                result.CompletionCode, 
                result.ErrorDescription);
        }

    }
}
