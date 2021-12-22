/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Common
{
    public partial class SetTransactionStateHandler
    {

        private async Task<SetTransactionStateCompletion.PayloadData> HandleSetTransactionState(ISetTransactionStateEvents events, SetTransactionStateCommand setTransactionState, CancellationToken cancel)
        {
            if (setTransactionState.Payload.State is null)
            {
                return new SetTransactionStateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                     $"No transaction state specified.");
            }
            if (string.IsNullOrEmpty(setTransactionState.Payload.TransactionID))
            {
                return new SetTransactionStateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                     $"No transaction ID specified.");
            }

            Logger.Log(Constants.DeviceClass, "CommonDev.SetTransactionState()");
            var result = await Device.SetTransactionState(new SetTransactionStateRequest(setTransactionState.Payload.State switch
                                                                                         {
                                                                                             SetTransactionStateCommand.PayloadData.StateEnum.Active => SetTransactionStateRequest.StateEnum.Active,
                                                                                             _ => SetTransactionStateRequest.StateEnum.Inactive,
                                                                                         },
                                                                                         setTransactionState.Payload.TransactionID));
            Logger.Log(Constants.DeviceClass, $"CommonDev.SetTransactionState() -> {result.CompletionCode}");

            return new SetTransactionStateCompletion.PayloadData(result.CompletionCode,
                                                                 result.ErrorDescription);
        }
    }
}
