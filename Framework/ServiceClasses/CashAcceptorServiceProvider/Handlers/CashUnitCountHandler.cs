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
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Storage;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using System.Linq;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class CashUnitCountHandler
    {
        private async Task<CommandResult<CashUnitCountCompletion.PayloadData>> HandleCashUnitCount(ICashUnitCountEvents events, CashUnitCountCommand cashUnitCount, CancellationToken cancel)
        {
            if (Common.CashAcceptorCapabilities.CountActions == CashAcceptorCapabilitiesClass.CountActionEnum.NotSupported)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"The device does not support count operation.");
            }

            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new(
                    new(CashUnitCountCompletion.PayloadData.ErrorCodeEnum.ExchangeActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The exchange state is already in active.");
            }

            if (CashAcceptor.CashInStatus.Status == CashInStatusClass.StatusEnum.Active)
            {
                return new(
                    new(CashUnitCountCompletion.PayloadData.ErrorCodeEnum.CashInActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The cash-in state is in active. {CashAcceptor.CashInStatus.Status}");
            }

            if (cashUnitCount.Payload.Units is null ||
                cashUnitCount.Payload.Units?.Count > 0)
            {
                return new(
                    new(CashUnitCountCompletion.PayloadData.ErrorCodeEnum.InvalidCashUnit),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"No cash units specified.");
            }

            foreach (var storageId in from storageId in cashUnitCount.Payload.Units
                                      where !Storage.CashUnits.ContainsKey(storageId)
                                      select storageId)
            {
                return new(
                    new(CashUnitCountCompletion.PayloadData.ErrorCodeEnum.InvalidCashUnit),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The specified cash unit is not valid. {storageId}");
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashUnitCount()");

            var result = await Device.CashUnitCount(
                new CashUnitCountCommandEvents(Storage, events),
                new CashUnitCountRequest(cashUnitCount.Payload.Units),
                cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.CashUnitCount() -> {result.CompletionCode}, {result.ErrorCode}");

            await Storage.UpdateCashAccounting(result.MovementResult);

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
