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
using XFS4IoT.BanknoteNeutralization.Commands;
using XFS4IoT.BanknoteNeutralization.Completions;
using System.Text.RegularExpressions;

namespace XFS4IoTFramework.BanknoteNeutralization
{
    public partial class TriggerNeutralizationHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandleTriggerNeutralization(ITriggerNeutralizationEvents events, TriggerNeutralizationCommand triggerNeutralization, CancellationToken cancel)
        {
            if (Common.IBNSCapabilities.CustomInputStatus is null ||
                Common.IBNSCapabilities.CustomInputStatus.Count == 0)
            {
                Logger.Log(Constants.Framework, $"The device doesn't have the presence of a set of custom inputs. this command will not work.");
            }

            if (triggerNeutralization.Payload.NeutralizationAction is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No NewState property specified.");
            }

            if (!string.IsNullOrEmpty(triggerNeutralization.Payload.Token))
            {
                if (!Regex.IsMatch(triggerNeutralization.Payload.Token, @"^(?=[!-~]{0,1024}$)NONCE=[0-9A-F]+,TOKENFORMAT=1,TOKENLENGTH=[0-9]{4},(?:[A-Z0-9]+=[^,=]+?,)+HMACSHA256=[0-9A-F]{64}$"))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Token contains invalid characters.");
                }
            }

            Logger.Log(Constants.DeviceClass, "IBNSDev.TriggerNeutralization()");
            var result = await Device.TriggerNeutralizationAsync(new(
                Trigger: triggerNeutralization.Payload.NeutralizationAction switch
                {
                    TriggerNeutralizationCommand.PayloadData.NeutralizationActionEnum.Trigger => TriggerNeutralizationRequest.TriggerEnum.Trigger,
                    _ => throw new InvalidDataException($"Unexpected property value specified. {nameof(triggerNeutralization.Payload.NeutralizationAction)}={triggerNeutralization.Payload.NeutralizationAction}"),
                },
                E2EToken: triggerNeutralization.Payload.Token),
                cancel);
            Logger.Log(Constants.DeviceClass, $"IBNSDev.TriggerNeutralization() -> {result.CompletionCode}");

            return new(
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }
    }
}
