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
using XFS4IoT.Completions;
using XFS4IoT.BanknoteNeutralization.Commands;
using XFS4IoT.BanknoteNeutralization.Completions;
using System.Text.RegularExpressions;

namespace XFS4IoTFramework.BanknoteNeutralization
{
    public partial class SetProtectionHandler
    {
        private async Task<CommandResult<SetProtectionCompletion.PayloadData>> HandleSetProtection(ISetProtectionEvents events, SetProtectionCommand setProtection, CancellationToken cancel)
        {
            if (Common.IBNSCapabilities.Mode == XFS4IoTFramework.Common.IBNSCapabilitiesClass.ModeEnum.Autonomous)
            {
                Logger.Log(Constants.Framework, $"The device supports autonomous mode. this command will not work.");
            }

            if (setProtection.Payload.NewState is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No NewState property specified.");
            }

            if (!string.IsNullOrEmpty(setProtection.Payload.Token))
            {
                if (!Regex.IsMatch(setProtection.Payload.Token, @"^(?=[!-~]{0,1024}$)NONCE=[0-9A-F]+,TOKENFORMAT=1,TOKENLENGTH=[0-9]{4},(?:[A-Z0-9]+=[^,=]+?,)+HMACSHA256=[0-9A-F]{64}$"))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Token contains invalid characters.");
                }
            }

            Logger.Log(Constants.DeviceClass, "IBNSDev.SetProtection()");
            var result = await Device.SetProtectionAsync(new(
                Protection: setProtection.Payload.NewState switch
                { 
                    SetProtectionCommand.PayloadData.NewStateEnum.Disarm => SetProtectionRequest.ProtectionEnum.Disarm,
                    SetProtectionCommand.PayloadData.NewStateEnum.Arm => SetProtectionRequest.ProtectionEnum.Arm,
                    SetProtectionCommand.PayloadData.NewStateEnum.IgnoreAllSafeSensors => SetProtectionRequest.ProtectionEnum.IgnoreAllSafeSensors,
                    _ => throw new InvalidDataException($"Unexpected property value specified. {nameof(setProtection.Payload.NewState)}={setProtection.Payload.NewState}"),
                },
                E2EToken: setProtection.Payload.Token), 
                cancel);
            Logger.Log(Constants.DeviceClass, $"IBNSDev.SetProtection() -> {result.CompletionCode}");

            return new(
                Payload: result.ErrorCode is null ? 
                null : 
                new(result.ErrorCode),
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }
    }
}
