/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class DepleteHandler
    {
        private async Task<CommandResult<DepleteCompletion.PayloadData>> HandleDeplete(IDepleteEvents events, DepleteCommand deplete, CancellationToken cancel)
        {
            if (CashAcceptor.DepleteCashUnitSources is null ||
                CashAcceptor.DepleteCashUnitSources.Count == 0)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                    $"The device doesn't support deplete operation.");
            }

            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new(
                    new(DepleteCompletion.PayloadData.ErrorCodeEnum.ExchangeActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The exchange state is already in active.");
            }

            if (CashAcceptor.CashInStatus.Status == CashInStatusClass.StatusEnum.Active)
            {
                return new(
                    new(DepleteCompletion.PayloadData.ErrorCodeEnum.CashInActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The cash-in state is in active. {CashAcceptor.CashInStatus.Status}");
            }

            if (string.IsNullOrEmpty(deplete.Payload.CashUnitTarget) ||
                !Storage.CashUnits.ContainsKey(deplete.Payload.CashUnitTarget))
            {
                return new(
                    new(DepleteCompletion.PayloadData.ErrorCodeEnum.InvalidCashUnit),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The specified destination of cash unit is not valid. {deplete.Payload.CashUnitTarget}");
            }

            // Check specified source is supported or not
            if (!CashAcceptor.DepleteCashUnitSources.Keys.Contains(deplete.Payload.CashUnitTarget))
            {
                return new(
                    new(DepleteCompletion.PayloadData.ErrorCodeEnum.InvalidCashUnit),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified target is not supported by the device. {deplete.Payload.CashUnitTarget}");
            }

            if (deplete.Payload.DepleteSources is null ||
                deplete.Payload.DepleteSources.Count == 0)
            {
                return new(
                    new(DepleteCompletion.PayloadData.ErrorCodeEnum.InvalidCashUnit),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"No source of cash units specified.");
            }

            foreach (var unit in from unit in deplete.Payload.DepleteSources
                                 where !Storage.CashUnits.ContainsKey(unit.Source)
                                 select unit)
            {
                return new(
                    new(DepleteCompletion.PayloadData.ErrorCodeEnum.InvalidCashUnit),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The specified source of the cash unit is not valid. {unit.Source}");
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.Deplete()");

            var result = await Device.Deplete(
                new DepleteCommandEvents(Storage, events), 
                new DepleteRequest(
                    deplete.Payload.DepleteSources.ToDictionary(s => s.Source, s => (int)s.NumberOfItemsToMove),
                    deplete.Payload.CashUnitTarget
                    ), 
                cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.Deplete() -> {result.CompletionCode}, {result.ErrorCode}");

            await Storage.UpdateCashAccounting(result.MovementResult);

            DepleteCompletion.PayloadData payload = result.ErrorCode is not null ? new(result.ErrorCode) : null;

            if (result.OperationResult is not null)
            {
                List<DepleteCompletion.PayloadData.DepleteSourceResultsClass> depleteSrouceResults = null;
                if (result.OperationResult.SourceResults?.Count > 0)
                {
                    depleteSrouceResults = new();
                    foreach (var sourceResult in result.OperationResult.SourceResults)
                    {
                        depleteSrouceResults.Add(
                            new DepleteCompletion.PayloadData.DepleteSourceResultsClass(
                                sourceResult.Key,
                                sourceResult.Value.CashItem,
                                sourceResult.Value.NumberOfItemsRemoved)
                            );
                    }
                }

                payload = new(
                    result.ErrorCode,
                    result.OperationResult.TotalNumberOfItemsReceived,
                    result.OperationResult.TotalNumberOfItemsRejected,
                    depleteSrouceResults);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
