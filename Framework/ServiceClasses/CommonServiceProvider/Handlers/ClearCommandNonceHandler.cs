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
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;

namespace XFS4IoTFramework.Common
{
    public partial class ClearCommandNonceHandler
    {

        private async Task<CommandResult<MessagePayloadBase>> HandleClearCommandNonce(IClearCommandNonceEvents events, ClearCommandNonceCommand clearCommandNonce, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CommonDev.ClearCommandNonce()");
            var result = await Device.ClearCommandNonce();
            Logger.Log(Constants.DeviceClass, $"CommonDev.ClearCommandNonce() -> {result.CompletionCode}");

            return new(
                result.CompletionCode, 
                result.ErrorDescription);
        }

    }
}
