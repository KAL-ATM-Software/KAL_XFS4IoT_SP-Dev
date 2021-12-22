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
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.CashManagement
{
    public partial class OpenSafeDoorHandler
    {
        private async Task<OpenSafeDoorCompletion.PayloadData> HandleOpenSafeDoor(IOpenSafeDoorEvents events, OpenSafeDoorCommand openSafeDoor, CancellationToken cancel)
        {
            if (CashManagement.CashManagementStatus.SafeDoor == Common.CashManagementStatusClass.SafeDoorEnum.NotSupported)
            {
                return new OpenSafeDoorCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"OpenSafeDoor command received when the SP reported SafeDoor status {CashManagement.CashManagementStatus.SafeDoor}");
            }

            if (CashManagement.CashManagementStatus.SafeDoor == Common.CashManagementStatusClass.SafeDoorEnum.Open)
            {
                return new OpenSafeDoorCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                              $"The safe door is already opened.");
            }

            Logger.Log(Constants.DeviceClass, "CashManagementDev.UnlockSafeAsync()");

            var result = await Device.UnlockSafeAsync(cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.UnlockSafeAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new OpenSafeDoorCompletion.PayloadData(result.CompletionCode,
                                                          result.ErrorDescription,
                                                          result.ErrorCode);
        }
    }
}
