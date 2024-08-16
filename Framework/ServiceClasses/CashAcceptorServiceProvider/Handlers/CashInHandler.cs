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
using XFS4IoT.Completions;
using XFS4IoTFramework.Storage;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using XFS4IoT.CashManagement;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class CashInHandler
    {
        private async Task<CommandResult<CashInCompletion.PayloadData>> HandleCashIn(ICashInEvents events, CashInCommand cashIn, CancellationToken cancel)
        {
            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new(
                    new(CashInCompletion.PayloadData.ErrorCodeEnum.ExchangeActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The exchange state is already in active.");
            }

            if (CashAcceptor.CashInStatus.Status != CashInStatusClass.StatusEnum.Active)
            {
                return new(
                    new(CashInCompletion.PayloadData.ErrorCodeEnum.NoCashInActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The cash-in state is not in active. {CashAcceptor.CashInStatus.Status}");
            }

            // Clear TotalReturnedItems for the present status
            foreach (var presentStatus in CashManagement.LastCashManagementPresentStatus)
            {
                presentStatus.Value.TotalReturnedItems = new();
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashIn()");

            var result = await Device.CashIn(new CashInCommandEvents(Storage, events), 
                                             new CashInRequest(cashIn.Header.Timeout ?? 0), 
                                             cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.CashIn() -> {result.CompletionCode}, {result.ErrorCode}");

            await Storage.UpdateCashAccounting(result.MovementResult);

            StorageCashCountsClass movement = null;

            if (result.ItemCounts?.Count > 0)
            {
                movement = new(result.Unrecognized);

                Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> itemAcceptedResult = new();
                foreach (var itemCount in result.ItemCounts)
                {
                    itemAcceptedResult.Add(itemCount.Key, new XFS4IoT.CashManagement.StorageCashCountClass(itemCount.Value.Fit, itemCount.Value.Unfit, itemCount.Value.Suspect, itemCount.Value.Counterfeit, itemCount.Value.Inked));

                    if (CashManagement.CashInStatusManaged.CashCounts.ItemCounts.TryGetValue(itemCount.Key, out CashItemCountClass value))
                    {
                        value.Counterfeit += itemCount.Value.Counterfeit;
                        value.Fit += itemCount.Value.Fit;
                        value.Inked += itemCount.Value.Inked;
                        value.Suspect += itemCount.Value.Suspect;
                        value.Unfit += itemCount.Value.Unfit;
                    }
                    else
                    {
                        CashManagement.CashInStatusManaged.CashCounts.ItemCounts.Add(itemCount.Key, new(itemCount.Value));
                    }
                }
                movement.ExtendedProperties = itemAcceptedResult;
            }

            CashInCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                movement is not null)
            {
                payload = new(
                    ErrorCode: result.ErrorCode,
                    Items: movement);
            }

            CashManagement.CashInStatusManaged.CashCounts.Unrecognized = result.Unrecognized;
            CashManagement.StoreCashInStatus();

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
        private ICashManagementService CashManagement { get => Provider.IsA<ICashManagementService>(); }
    }
}
