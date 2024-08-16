/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoT;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class ResetHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandleReset(IResetEvents events, ResetCommand reset, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "KeyManagementDev.ResetDevice()");

            var result = await Device.ResetDevice(cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ResetDevice() -> {result.CompletionCode}");

            return new(
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
