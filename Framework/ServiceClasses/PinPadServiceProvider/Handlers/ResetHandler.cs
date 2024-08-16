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
using XFS4IoT.PinPad.Commands;
using XFS4IoT.PinPad.Completions;

namespace XFS4IoTFramework.PinPad
{
    public partial class ResetHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandleReset(IResetEvents events, ResetCommand reset, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "PinPadDev.ResetDevice()");

            var result = await Device.ResetDevice(cancel);

            Logger.Log(Constants.DeviceClass, $"PinPadDev.ResetDevice() -> {result.CompletionCode}");

            return new(
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
