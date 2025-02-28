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
using XFS4IoT.PowerManagement.Commands;
using XFS4IoT.PowerManagement.Completions;

namespace XFS4IoTFramework.PowerManagement
{
    public partial class PowerSaveControlHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandlePowerSaveControl(IPowerSaveControlEvents events, PowerSaveControlCommand powerSaveControl, CancellationToken cancel)
        {
            if (powerSaveControl.Payload.MaxPowerSaveRecoveryTime is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No MaxPowerSaveRecoveryTime specified.");
            }

            Logger.Log(Constants.DeviceClass, "PowerManagementDev.PowerSaveControl()");
            var result = await Device.PowerSaveControl((int)powerSaveControl.Payload.MaxPowerSaveRecoveryTime, cancel);
            Logger.Log(Constants.DeviceClass, $"PowerManagementDev.PowerSaveControl() -> {result.CompletionCode}");

            return new(
                result.CompletionCode,
                result.ErrorDescription);
        }

    }
}
