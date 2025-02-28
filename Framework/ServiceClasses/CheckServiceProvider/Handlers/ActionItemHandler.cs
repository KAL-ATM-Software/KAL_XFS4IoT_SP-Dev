/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Check.Commands;
using XFS4IoT.Check.Completions;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.Check
{
    public partial class ActionItemHandler
    {
        private async Task<CommandResult<ActionItemCompletion.PayloadData>> HandleActionItem(IActionItemEvents events, ActionItemCommand actionItem, CancellationToken cancel)
        {
            if (Common.CheckScannerCapabilities.MaxMediaOnStacker != 0)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                    $"{nameof(ActionItemCommand)} is valid for the device does not have a stacker. {nameof(Common.CheckScannerCapabilities.MaxMediaOnStacker)} is {Common.CheckScannerCapabilities.MaxMediaOnStacker}");
            }

            Logger.Log(Constants.DeviceClass, "CheckDev.ActionItemAsync()");

            var result = await Device.ActionItemAsync(
                events: new ActionItemCommandEvents(Storage, Check, events),
                request: new(),
                cancellation: cancel);

            Logger.Log(Constants.DeviceClass, $"CheckDev.ActionItemAsync() -> {result.CompletionCode}");

            Dictionary<string, StorageCheckCountClass> countDelta = [];
            if (result.MovementResult is not null)
            {
                foreach (var storage in result.MovementResult)
                {
                    countDelta.Add(storage.Key, new(0, storage.Value.Count, storage.Value.MediaRetracted ? 1 : 0));
                }
            }

            // Update internal check counts and send associated events.
            if (countDelta.Count > 0)
            {
                await Storage.UpdateCheckStorageCount(countDelta);
            }

            return new(
                result.ErrorCode is not null ? new(ErrorCode: result.ErrorCode) : null,
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
