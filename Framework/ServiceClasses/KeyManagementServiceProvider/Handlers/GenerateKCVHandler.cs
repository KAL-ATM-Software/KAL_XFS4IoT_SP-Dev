/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoT;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class GenerateKCVHandler
    {
        private async Task<CommandResult<GenerateKCVCompletion.PayloadData>> HandleGenerateKCV(IGenerateKCVEvents events, GenerateKCVCommand generateKCV, CancellationToken cancel)
        {
            if (!string.IsNullOrEmpty(generateKCV.Payload.Key))
            {
                KeyDetail keyDetail = KeyManagement.GetKeyDetail(generateKCV.Payload.Key);
                if (keyDetail is null)
                {
                    return new(
                        new(GenerateKCVCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified key doesn't exist. {generateKCV.Payload.Key}");
                }

                if (keyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
                {
                    return new(
                        new(GenerateKCVCompletion.PayloadData.ErrorCodeEnum.KeyNoValue),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified key doesn't exist. {generateKCV.Payload.Key}");
                }
            }
            else
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No key name specified to generate KCV.");
            }

            if (generateKCV.Payload.KeyCheckMode is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No key name specified to KCV mode specified.");
            }

            if (Common.KeyManagementCapabilities.KeyCheckModes == KeyManagementCapabilitiesClass.KeyCheckModeEnum.NotSupported)
            {
                return new(
                    new(GenerateKCVCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Key check mode is not supported.");
            }

            if (generateKCV.Payload.KeyCheckMode == GenerateKCVCommand.PayloadData.KeyCheckModeEnum.Zero &&
                !Common.KeyManagementCapabilities.KeyCheckModes.HasFlag(KeyManagementCapabilitiesClass.KeyCheckModeEnum.Zero) ||
                generateKCV.Payload.KeyCheckMode == GenerateKCVCommand.PayloadData.KeyCheckModeEnum.Self &&
                !Common.KeyManagementCapabilities.KeyCheckModes.HasFlag(KeyManagementCapabilitiesClass.KeyCheckModeEnum.Self))
            {
                return new(
                    new(GenerateKCVCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified Key check mode is not supported. {generateKCV.Payload.KeyCheckMode}");

            }
            Logger.Log(Constants.DeviceClass, "KeyManagementDev.GenerateKCV()");

            var result = await Device.GenerateKCV(
                new GenerateKCVRequest(
                    generateKCV.Payload.Key,
                    generateKCV.Payload.KeyCheckMode switch
                    {
                        GenerateKCVCommand.PayloadData.KeyCheckModeEnum.Self => GenerateKCVRequest.KeyCheckValueEnum.Self,
                        _ => GenerateKCVRequest.KeyCheckValueEnum.Zero
                    }), 
                cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.GenerateKCV() -> {result.CompletionCode}, {result.ErrorCode}");

            GenerateKCVCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.KCV?.Count > 0)
            {
                payload = new(
                    result.ErrorCode,
                    result.KCV);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
