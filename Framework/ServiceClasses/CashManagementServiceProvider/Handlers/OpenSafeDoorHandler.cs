/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashManagement
{
    public partial class OpenSafeDoorHandler
    {
        private async Task<OpenSafeDoorCompletion.PayloadData> HandleOpenSafeDoor(IOpenSafeDoorEvents events, OpenSafeDoorCommand openSafeDoor, CancellationToken cancel)
        {
            if (Common.CashManagementStatus.SafeDoor == CashManagementStatusClass.SafeDoorEnum.NotSupported)
            {
                return new OpenSafeDoorCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"OpenSafeDoor command received when the SP reported SafeDoor status {Common.CashManagementStatus.SafeDoor}");
            }

            if (Common.CashManagementStatus.SafeDoor == CashManagementStatusClass.SafeDoorEnum.Open)
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
