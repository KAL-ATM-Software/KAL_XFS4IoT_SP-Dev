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
    public partial class CashInEndHandler
    {
        private async Task<CashInEndCompletion.PayloadData> HandleCashInEnd(ICashInEndEvents events, CashInEndCommand cashInEnd, CancellationToken cancel)
        {
            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new CashInEndCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"The exchange state is already in active.",
                                                           CashInEndCompletion.PayloadData.ErrorCodeEnum.ExchangeActive);
            }

            if (CashAcceptor.CashInStatus.Status != CashInStatusClass.StatusEnum.Active)
            {
                return new CashInEndCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"The cash-in state is not in active. {CashAcceptor.CashInStatus.Status}",
                                                           CashInEndCompletion.PayloadData.ErrorCodeEnum.NoCashInActive);
            }

            // Clear TotalReturnedItems for the present status
            foreach (var presentStatus in CashManagement.LastCashManagementPresentStatus)
            {
                presentStatus.Value.TotalReturnedItems = new();
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashInEnd()");

            var result = await Device.CashInEnd(new CashInEndCommandEvents(Storage, events), cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.CashInEnd() -> {result.CompletionCode}, {result.ErrorCode}");

            // Ending cash-in operation
            CashManagement.CashInStatusManaged.Status = CashInStatusClass.StatusEnum.Ok;
            CashManagement.StoreCashInStatus();

            Dictionary<string, StorageCashInClass> itemMovementResult = null;
            if (result.MovementResult?.Count > 0)
            {
                itemMovementResult = new();
                foreach (var movement in result.MovementResult)
                {
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> deposited = new();
                    foreach (var item in movement.Value.StorageCashInCount.Deposited.ItemCounts)
                        deposited.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> retracted = new();
                    foreach (var item in movement.Value.StorageCashInCount.Retracted.ItemCounts)
                        retracted.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> rejected = new();
                    foreach (var item in movement.Value.StorageCashInCount.Rejected.ItemCounts)
                        rejected.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> distributed = new();
                    foreach (var item in movement.Value.StorageCashInCount.Distributed.ItemCounts)
                        distributed.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> transport = new();
                    foreach (var item in movement.Value.StorageCashInCount.Transport.ItemCounts)
                        transport.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));

                    StorageCashCountsClass depositedCount = new(movement.Value.StorageCashInCount.Deposited.Unrecognized);
                    depositedCount.ExtendedProperties = deposited;
                    StorageCashCountsClass retractedCount = new(movement.Value.StorageCashInCount.Retracted.Unrecognized);
                    retractedCount.ExtendedProperties = retracted;
                    StorageCashCountsClass rejectedCount = new(movement.Value.StorageCashInCount.Rejected.Unrecognized);
                    rejectedCount.ExtendedProperties = rejected;
                    StorageCashCountsClass distributedCount = new(movement.Value.StorageCashInCount.Distributed.Unrecognized);
                    distributedCount.ExtendedProperties = distributed;
                    StorageCashCountsClass transportCount = new(movement.Value.StorageCashInCount.Transport.Unrecognized);
                    transportCount.ExtendedProperties = transport;

                    itemMovementResult.Add(movement.Key, new StorageCashInClass
                                                             (
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

            return new CashInEndCompletion.PayloadData(result.CompletionCode,
                                                       result.ErrorDescription,
                                                       result.ErrorCode,
                                                       itemMovementResult);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
        private ICashManagementService CashManagement { get => Provider.IsA<ICashManagementService>(); }
    }
}
