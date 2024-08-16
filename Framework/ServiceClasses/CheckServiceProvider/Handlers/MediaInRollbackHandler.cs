/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
using XFS4IoT.Check;
using XFS4IoTFramework.Storage;
using XFS4IoT.Storage;
using System.Linq;

namespace XFS4IoTFramework.Check
{
    public partial class MediaInRollbackHandler
    {
        private async Task<CommandResult<MediaInRollbackCompletion.PayloadData>> HandleMediaInRollback(IMediaInRollbackEvents events, MediaInRollbackCommand mediaInRollback, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CheckDev.MediaInRollbackAsync()");

            var result = await Device.MediaInRollbackAsync(
                events: new MediaInRollbackCommandEvents(events),
                request: new(),
                cancellation: cancel);

            Logger.Log(Constants.DeviceClass, $"CheckDev.MediaInRollbackAsync() -> {result.CompletionCode}");

            if (Check.LastTransactionStatus.MediaInTransactionState == TransactionStatus.MediaInTransactionStateEnum.Active)
            {
                if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
                {
                    Check.LastTransactionStatus.MediaInTransactionState = TransactionStatus.MediaInTransactionStateEnum.Rollback;
                }
                else
                {
                    Check.LastTransactionStatus.MediaInTransactionState = TransactionStatus.MediaInTransactionStateEnum.Failure;
                }
            }

            if (result.MediaInEndCount is not null)
            {
                Check.LastTransactionStatus.TotalBunchesRefused = result.MediaInEndCount.BunchesRefused;
                Check.LastTransactionStatus.TotalItemsRefused = result.MediaInEndCount.ItemsRefused;
            }
            Check.StoreTransactionStatus();

            Dictionary<string, StorageCheckCountClass> countDelta = [];
            if (result.MediaInEndCount?.MovementResult is not null)
            {
                foreach (var storage in result.MediaInEndCount.MovementResult)
                {
                    countDelta.Add(storage.Key, new(0, storage.Value.Count, storage.Value.MediaRetracted ? 1 : 0));
                }
            }

            // Update internal check counts and send associated events.
            if (countDelta.Count > 0)
            {
                await Storage.UpdateCheckStorageCount(countDelta);
            }

            MediaInEndDataClass count = null;
            if (result.MediaInEndCount is not null)
            {
                Dictionary<string, StorageUnitClass> units = null;
                if (result.MediaInEndCount.MovementResult?.Count > 0)
                {
                    List<string> updatedUnits = [];
                    updatedUnits.AddRange(from itemMovement in result.MediaInEndCount.MovementResult
                                          select itemMovement.Key);
                    if (updatedUnits.Count > 0)
                    {
                        units = Storage.GetStorages(updatedUnits);
                    }
                }

                count = new(
                    ItemsReturned: result.MediaInEndCount.ItemsReturned < 0 ?
                        null :
                        result.MediaInEndCount.ItemsReturned,
                    ItemsRefused: result.MediaInEndCount.ItemsRefused < 0 ?
                        null :
                        result.MediaInEndCount.ItemsRefused,
                    BunchesRefused: result.MediaInEndCount.BunchesRefused < 0 ?
                        null :
                        result.MediaInEndCount.BunchesRefused,
                        Storage: units is null ?
                    null :
                    new(units));
            }

            MediaInRollbackCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                count is not null)
            {
                payload = new(
                    ErrorCode: result.ErrorCode,
                    MediaInRollback: count);
            }

            return new(
                payload,
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
