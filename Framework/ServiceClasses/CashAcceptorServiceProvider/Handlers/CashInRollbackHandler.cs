/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using XFS4IoT.CashManagement;
using XFS4IoT.Completions;
using XFS4IoTFramework.Storage;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class CashInRollbackHandler
    {
        private async Task<CommandResult<CashInRollbackCompletion.PayloadData>> HandleCashInRollback(ICashInRollbackEvents events, CashInRollbackCommand cashInRollback, CancellationToken cancel)
        {
            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new(
                    new(CashInRollbackCompletion.PayloadData.ErrorCodeEnum.ExchangeActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The exchange state is already in active.");
            }

            if (CashAcceptor.CashInStatus.Status != CashInStatusClass.StatusEnum.Active)
            {
                return new(
                    new(CashInRollbackCompletion.PayloadData.ErrorCodeEnum.NoCashInActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The cash-in state is not in active. {CashAcceptor.CashInStatus.Status}");
            }

            // Clear TotalReturnedItems for the present status
            foreach (var presentStatus in CashManagement.LastCashManagementPresentStatus)
            {
                presentStatus.Value.TotalReturnedItems = new();
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashInRollback()");

            var result = await Device.CashInRollback(new CashInRollbackCommandEvents(Storage, events), cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.CashInRollback() -> {result.CompletionCode}, {result.ErrorCode}");

            // Ending cash-in operation
            CashManagement.CashInStatusManaged.Status = CashInStatusClass.StatusEnum.Rollback;
            CashManagement.StoreCashInStatus();

            Dictionary<string, StorageCashInClass> itemMovementResult = null;
            if (result.MovementResult?.Count > 0)
            {
                itemMovementResult = [];
                foreach (var movement in result.MovementResult)
                {
                    if (movement.Value.StorageCashInCount is null)
                    {
                        // Ignore if the device class reports no cash count information.
                    }
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> deposited = new();
                    foreach (var item in movement.Value.StorageCashInCount.Deposited.ItemCounts)
                    {
                        deposited.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    }
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> retracted = new();
                    foreach (var item in movement.Value.StorageCashInCount.Retracted.ItemCounts)
                    {
                        retracted.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    }
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> rejected = new();
                    foreach (var item in movement.Value.StorageCashInCount.Rejected.ItemCounts)
                    {
                        rejected.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    }
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> distributed = new();
                    foreach (var item in movement.Value.StorageCashInCount.Distributed.ItemCounts)
                    {
                        distributed.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    }
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> transport = new();
                    foreach (var item in movement.Value.StorageCashInCount.Transport.ItemCounts)
                    {
                        transport.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    }

                    StorageCashCountsClass depositedCount = new(movement.Value.StorageCashInCount.Deposited.Unrecognized)
                    {
                        ExtendedProperties = deposited
                    };
                    StorageCashCountsClass retractedCount = new(movement.Value.StorageCashInCount.Retracted.Unrecognized)
                    {
                        ExtendedProperties = retracted
                    };
                    StorageCashCountsClass rejectedCount = new(movement.Value.StorageCashInCount.Rejected.Unrecognized)
                    {
                        ExtendedProperties = rejected
                    };
                    StorageCashCountsClass distributedCount = new(movement.Value.StorageCashInCount.Distributed.Unrecognized)
                    {
                        ExtendedProperties = distributed
                    };
                    StorageCashCountsClass transportCount = new(movement.Value.StorageCashInCount.Transport.Unrecognized)
                    {
                        ExtendedProperties = transport
                    };

                    itemMovementResult.Add(
                        movement.Key, 
                        new StorageCashInClass(
                            movement.Value.StorageCashInCount.RetractOperations,
                            depositedCount,
                            retractedCount,
                            rejectedCount,
                            distributedCount,
                            transportCount
                            ));
                }
            }

            await Storage.UpdateCashAccounting(result.MovementResult);

            CashInRollbackCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                itemMovementResult is not null)
            {
                payload = new(
                    ErrorCode: result.ErrorCode,
                    Storage: itemMovementResult);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
        private ICashManagementService CashManagement { get => Provider.IsA<ICashManagementService>(); }
    }
}
