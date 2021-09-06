/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class GenerateKCVHandler
    {
        private async Task<GenerateKCVCompletion.PayloadData> HandleGenerateKCV(IGenerateKCVEvents events, GenerateKCVCommand generateKCV, CancellationToken cancel)
        {
            if (!string.IsNullOrEmpty(generateKCV.Payload.Key))
            {
                KeyDetail keyDetail = KeyManagement.GetKeyDetail(generateKCV.Payload.Key);
                if (keyDetail is null)
                {
                    return new GenerateKCVCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                 $"Specified key doesn't exist. {generateKCV.Payload.Key}",
                                                                 GenerateKCVCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
                }

                if (keyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
                {
                    return new GenerateKCVCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                 $"Specified key doesn't exist. {generateKCV.Payload.Key}",
                                                                 GenerateKCVCompletion.PayloadData.ErrorCodeEnum.KeyNoValue);
                }
            }
            else
            {
                return new GenerateKCVCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                             $"No key name specified to generate KCV.");
            }

            if (generateKCV.Payload.KeyCheckMode is null)
            {
                return new GenerateKCVCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                             $"No key name specified to KCV mode specified.");
            }

            if (KeyManagement.KeyManagementCapabilities.KeyCheckModes == KeyManagementCapabilitiesClass.KeyCheckModeEnum.NotSupported)
            {
                return new GenerateKCVCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                             $"Key check mode is not supported.",
                                                             GenerateKCVCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported);
            }

            if (generateKCV.Payload.KeyCheckMode == GenerateKCVCommand.PayloadData.KeyCheckModeEnum.Zero &&
                !KeyManagement.KeyManagementCapabilities.KeyCheckModes.HasFlag(KeyManagementCapabilitiesClass.KeyCheckModeEnum.Zero) ||
                generateKCV.Payload.KeyCheckMode == GenerateKCVCommand.PayloadData.KeyCheckModeEnum.Self &&
                !KeyManagement.KeyManagementCapabilities.KeyCheckModes.HasFlag(KeyManagementCapabilitiesClass.KeyCheckModeEnum.Self))
            {
                return new GenerateKCVCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                             $"Specified Key check mode is not supported. {generateKCV.Payload.KeyCheckMode}",
                                                             GenerateKCVCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported);

            }
            Logger.Log(Constants.DeviceClass, "KeyManagementDev.GenerateKCV()");

            var result = await Device.GenerateKCV(new GenerateKCVRequest(generateKCV.Payload.Key,
                                                                         generateKCV.Payload.KeyCheckMode switch
                                                                         {
                                                                             GenerateKCVCommand.PayloadData.KeyCheckModeEnum.Self => GenerateKCVRequest.KeyCheckValueEnum.Self,
                                                                             _ => GenerateKCVRequest.KeyCheckValueEnum.Zero
                                                                         }), cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.GenerateKCV() -> {result.CompletionCode}, {result.ErrorCode}");

            return new GenerateKCVCompletion.PayloadData(result.CompletionCode,
                                                         result.ErrorDescription,
                                                         result.ErrorCode,
                                                         result.KCV is not null && result.KCV.Count > 0 ? Convert.ToBase64String(result.KCV.ToArray()) : null);
        }
    }
}
