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
using XFS4IoT.PinPad.Commands;

namespace XFS4IoTFramework.PinPad
{
    public partial class MaintainPinHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandleMaintainPin(IMaintainPinEvents events, MaintainPinCommand maintainPin, CancellationToken cancel)
        {
            if (maintainPin.Payload.MaintainPIN is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"MaintainPIN is not specified.");
            }

            Logger.Log(Constants.DeviceClass, "PinPadDev.VerifyPINLocalDES()");

            var result = await Device.MaintainPin((bool)maintainPin.Payload.MaintainPIN, cancel);

            Logger.Log(Constants.DeviceClass, $"PinPadDev.VerifyPINLocalDES() -> {result.CompletionCode}");

            return new(
                result.CompletionCode, 
                result.ErrorDescription);
        }
    }
}
