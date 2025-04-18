/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.Completions;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using XFS4IoT;

namespace XFS4IoTFramework.Common
{
    public partial class GetTransactionStateHandler
    {
        private async Task<CommandResult<GetTransactionStateCompletion.PayloadData>> HandleGetTransactionState(IGetTransactionStateEvents events, GetTransactionStateCommand getTransactionState, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CommonDev.GetTransactionState()");
            var result = await Device.GetTransactionState();
            Logger.Log(Constants.DeviceClass, $"CommonDev.GetTransactionState() -> {result.CompletionCode}");

            return new(
                result.State is not null ? new(
                    result.State switch
                    {
                        TransactionStateEnum.Active => GetTransactionStateCompletion.PayloadData.StateEnum.Active,
                        TransactionStateEnum.Inactive => GetTransactionStateCompletion.PayloadData.StateEnum.Inactive,
                        _ => null,
                    },
                    result.TransactionID) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
