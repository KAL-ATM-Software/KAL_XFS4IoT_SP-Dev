/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using XFS4IoTFramework.Storage;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class ReplenishHandler
    {
        private async Task<CommandResult<ReplenishCompletion.PayloadData>> HandleReplenish(IReplenishEvents events, ReplenishCommand replenish, CancellationToken cancel)
        {
            if (CashAcceptor.ReplenishTargets is null ||
                CashAcceptor.ReplenishTargets.Count == 0)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                    $"The device doesn't support replenish command.");
            }

            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new(
                    new(ReplenishCompletion.PayloadData.ErrorCodeEnum.ExchangeActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The exchange state is already in active.");
            }

            if (CashAcceptor.CashInStatus.Status == CashInStatusClass.StatusEnum.Active)
            {
                return new(
                    new(ReplenishCompletion.PayloadData.ErrorCodeEnum.CashInActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The cash-in state is in active. {CashAcceptor.CashInStatus.Status}");
            }

            if (string.IsNullOrEmpty(replenish.Payload.Source) ||
                !Storage.CashUnits.ContainsKey(replenish.Payload.Source))
            {
                return new(
                    new(ReplenishCompletion.PayloadData.ErrorCodeEnum.InvalidCashUnit),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The specified source of storage unit is not valid. {replenish.Payload.Source}");
            }

            foreach (var target in replenish.Payload.ReplenishTargets)
            {
                if (!CashAcceptor.ReplenishTargets.Contains(target.Target))
                {
                    return new(
                        new(ReplenishCompletion.PayloadData.ErrorCodeEnum.InvalidCashUnit),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified target is not supported by the device. {target.Target}");

                }
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.Replenish()");

            var result = await Device.Replenish(new ReplenishCommandEvents(Storage, events),
                                                new ReplenishRequest(replenish.Payload.Source,
                                                                     replenish.Payload.ReplenishTargets.ToDictionary(t => t.Target, t => t.NumberOfItemsToMove is null ? 0 : (int)t.NumberOfItemsToMove)),
                                                cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.Replenish() -> {result.CompletionCode}, {result.ErrorCode}");

            await Storage.UpdateCashAccounting(result.MovementResult);

            ReplenishCompletion.PayloadData payload = result.ErrorCode is not null ? new(result.ErrorCode) : null;

            if (result.OperationResult is not null)
            {
                List<ReplenishCompletion.PayloadData.ReplenishTargetResultsClass> targetResults = null;
                if (result.OperationResult.TargetResults?.Count > 0)
                {
                    targetResults = [];
                    foreach (var targetResult in result.OperationResult.TargetResults)
                    {
                        targetResults.Add(new ReplenishCompletion.PayloadData.ReplenishTargetResultsClass(targetResult.Key,
                                                                                                          targetResult.Value.CashItem,
                                                                                                          targetResult.Value.NumberOfItemsReceived));
                    }
                }

                if (targetResults is not null ||
                    result.OperationResult is not null)
                {
                    payload = new(
                        result.ErrorCode,
                        result.OperationResult.NumberOfItemsRemoved,
                        result.OperationResult.TotalNumberOfItemsRejected,
                        targetResults);
                }
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
