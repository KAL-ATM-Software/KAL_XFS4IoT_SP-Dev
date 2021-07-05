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
            if (!CashManagement.CashManagementCapabilities.SafeDoor)
            {
                return new OpenSafeDoorCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"OpenSafeDoor command received when the SP reported SafeDoor capability {CashManagement.CashManagementCapabilities.SafeDoor}");
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
