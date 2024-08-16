/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Check.Commands;
using XFS4IoT.Check.Completions;

namespace XFS4IoTFramework.Check
{
    public partial class ExpelMediaHandler
    {
        private async Task<CommandResult<ExpelMediaCompletion.PayloadData>> HandleExpelMedia(IExpelMediaEvents events, ExpelMediaCommand expelMedia, CancellationToken cancel)
        {
            // No status check or capabilities

            Logger.Log(Constants.DeviceClass, "CheckDev.ExpelMediaAsync()");
            var result = await Device.ExpelMediaAsync(cancel);
            Logger.Log(Constants.DeviceClass, $"CheckDev.ExpelMediaAsync() -> {result.CompletionCode}");

            return new(
                result.ErrorCode is not null ? new(ErrorCode: result.ErrorCode) : null,
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }
    }
}
