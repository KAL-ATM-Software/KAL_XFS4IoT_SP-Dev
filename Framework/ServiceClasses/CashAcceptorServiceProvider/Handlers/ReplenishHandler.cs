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
        private async Task<ReplenishCompletion.PayloadData> HandleReplenish(IReplenishEvents events, ReplenishCommand replenish, CancellationToken cancel)
        {
            if (CashAcceptor.ReplenishTargets is null ||
                CashAcceptor.ReplenishTargets.Count == 0)
            {
                return new ReplenishCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                           $"The device doesn't support replenish command.");
            }

            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new ReplenishCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"The exchange state is already in active.",
                                                           ReplenishCompletion.PayloadData.ErrorCodeEnum.ExchangeActive);
            }

            if (CashAcceptor.CashInStatus.Status == CashInStatusClass.StatusEnum.Active)
            {
                return new ReplenishCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"The cash-in state is in active. {CashAcceptor.CashInStatus.Status}",
                                                           ReplenishCompletion.PayloadData.ErrorCodeEnum.CashInActive);
            }

            if (string.IsNullOrEmpty(replenish.Payload.Source) ||
                !Storage.CashUnits.ContainsKey(replenish.Payload.Source))
            {
                return new ReplenishCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"The specified source of storage unit is not valid. {replenish.Payload.Source}",
                                                           ReplenishCompletion.PayloadData.ErrorCodeEnum.InvalidCashUnit);
            }

            foreach (var target in replenish.Payload.ReplenishTargets)
            {
                if (!CashAcceptor.ReplenishTargets.Contains(target.Target))
                {
                    return new ReplenishCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Specified target is not supported by the device. {target.Target}",
                                                               ReplenishCompletion.PayloadData.ErrorCodeEnum.InvalidCashUnit);

                }
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.Replenish()");

            var result = await Device.Replenish(new ReplenishCommandEvents(Storage, events),
                                                new ReplenishRequest(replenish.Payload.Source,
                                                                     replenish.Payload.ReplenishTargets.ToDictionary(t => t.Target, t => t.NumberOfItemsToMove is null ? 0 : (int)t.NumberOfItemsToMove)),
                                                cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.Replenish() -> {result.CompletionCode}, {result.ErrorCode}");

            await Storage.UpdateCashAccounting(result.MovementResult);

            ReplenishCompletion.PayloadData payload = new(result.CompletionCode,
                                                          result.ErrorDescription,
                                                          result.ErrorCode);
            if (result.OperationResult is not null)
            {
                List<ReplenishCompletion.PayloadData.ReplenishTargetResultsClass> targetResults = null;
                if (result.OperationResult.TargetResults?.Count > 0)
                {
                    targetResults = new();
                    foreach (var targetResult in result.OperationResult.TargetResults)
                    {
                        targetResults.Add(new ReplenishCompletion.PayloadData.ReplenishTargetResultsClass(targetResult.Key,
                                                                                                          targetResult.Value.CashItem,
                                                                                                          targetResult.Value.NumberOfItemsReceived));
                    }
                }

                payload = new(result.CompletionCode,
                              result.ErrorDescription,
                              result.ErrorCode,
                              result.OperationResult.NumberOfItemsRemoved,
                              result.OperationResult.TotalNumberOfItemsRejected,
                              targetResults);
            }

            return payload;
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
