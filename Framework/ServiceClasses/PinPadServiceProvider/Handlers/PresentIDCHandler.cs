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
using XFS4IoT.PinPad.Commands;
using XFS4IoT.PinPad.Completions;

namespace XFS4IoTFramework.PinPad
{
    public partial class PresentIDCHandler
    {
        private async Task<CommandResult<PresentIDCCompletion.PayloadData>> HandlePresentIDC(IPresentIDCEvents events, PresentIDCCommand presentIDC, CancellationToken cancel)
        {
            if (presentIDC.Payload.PresentAlgorithm is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No presentAlgorith specified.");
            }

            if (string.IsNullOrEmpty(presentIDC.Payload.ChipProtocol))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No chipProtocol specified to talk to the card chip.");
            }

            if (presentIDC.Payload.ChipData is null ||
                presentIDC.Payload.ChipData.Count == 0)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No chipData specified to send a card chip.");
            }

            if (presentIDC.Payload.PresentAlgorithm is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No PresentAlgorith specified.");
            }

            if (presentIDC.Payload.PresentAlgorithm == PresentIDCCommand.PayloadData.PresentAlgorithmEnum.PresentClear)
            {
                if (presentIDC.Payload.AlgorithmData is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"No algorithmData specified.");
                }
                if (presentIDC.Payload.AlgorithmData.PinOffset is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"No PinOffset specified.");
                }
                if (presentIDC.Payload.AlgorithmData.PinPointer is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"No PinPointer specified.");
                }
            }
            
            Logger.Log(Constants.DeviceClass, "PinPadDev.PresentIDC()");

            var result = await Device.PresentIDC(new PresentIDCRequest(presentIDC.Payload.ChipProtocol,
                                                                       presentIDC.Payload.ChipData,
                                                                       presentIDC.Payload.PresentAlgorithm == PresentIDCCommand.PayloadData.PresentAlgorithmEnum.PresentClear ? new PresentIDCRequest.PresentClearClass((int)presentIDC.Payload.AlgorithmData.PinOffset, (int)presentIDC.Payload.AlgorithmData.PinPointer) : null), 
                                                 cancel);

            Logger.Log(Constants.DeviceClass, $"PinPadDev.PresentIDC() -> {result.CompletionCode}");

            PresentIDCCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.ChipData?.Count > 0 ||
                !string.IsNullOrEmpty(result.ChipProtocol))
            {
                payload = new(
                    result.ErrorCode,
                    result.ChipProtocol,
                    result.ChipData);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
