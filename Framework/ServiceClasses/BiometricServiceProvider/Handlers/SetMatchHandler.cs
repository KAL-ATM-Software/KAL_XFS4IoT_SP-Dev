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
using XFS4IoTFramework.Common;
using XFS4IoT.Biometric.Commands;
using XFS4IoT.Biometric.Completions;

namespace XFS4IoTFramework.Biometric
{
    public partial class SetMatchHandler
    {

        private async Task<SetMatchCompletion.PayloadData> HandleSetMatch(ISetMatchEvents events, SetMatchCommand setMatch, CancellationToken cancel)
        {
            if (setMatch?.Payload?.CompareMode is null)
                return new SetMatchCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData,
                                                          "CompareMode was not specified.");

            if (setMatch?.Payload?.Threshold is null)
                return new SetMatchCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                          "Threshold was not specified.",
                                                          SetMatchCompletion.PayloadData.ErrorCodeEnum.InvalidThreshold);

            if (setMatch.Payload.Threshold.Value < 0 || setMatch.Payload.Threshold.Value > 100)
                return new SetMatchCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                          "Threshold must be within range 0-100.",
                                                          SetMatchCompletion.PayloadData.ErrorCodeEnum.InvalidThreshold);

            BiometricCapabilitiesClass.CompareModesEnum compareMode;

            if (setMatch.Payload.CompareMode is SetMatchCommand.PayloadData.CompareModeEnum.Identity)
            {
                if (!Common.BiometricCapabilities.CompareModes.HasFlag(BiometricCapabilitiesClass.CompareModesEnum.Identity))
                    return new SetMatchCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                              "CompareMode.Identity is not supported by this device.",
                                                              SetMatchCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported);

                else if(setMatch.Payload.Maximum is null)
                    return new SetMatchCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                              "Maximum must be set when CompareMode is Identity.",
                                                              SetMatchCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported);

                if (!string.IsNullOrWhiteSpace(setMatch.Payload.Identifier))
                    return new SetMatchCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData,
                                                              "Identifier should be ommitted when CompareMode is Identity.");

                compareMode = BiometricCapabilitiesClass.CompareModesEnum.Identity;
            }
            else
            {
                if (!Common.BiometricCapabilities.CompareModes.HasFlag(BiometricCapabilitiesClass.CompareModesEnum.Verify))
                    return new SetMatchCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                          "CompareMode.Verify is not supported by this device.",
                                                          SetMatchCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported);

                if(string.IsNullOrWhiteSpace(setMatch.Payload.Identifier))
                    return new SetMatchCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                          "Identifier must be specified when CompareMode is Verify.",
                                                          SetMatchCompletion.PayloadData.ErrorCodeEnum.InvalidIdentifier);
                
                var deviceStorageInfo = Device.StorageInfo;

                if(deviceStorageInfo is null || !deviceStorageInfo.ContainsKey(setMatch.Payload.Identifier))
                    return new SetMatchCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                          $"Identifier supplied is not stored by the device. {setMatch.Payload.Identifier}",
                                                          SetMatchCompletion.PayloadData.ErrorCodeEnum.InvalidIdentifier);

                if (setMatch.Payload.Maximum is not null)
                    return new SetMatchCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData,
                                                              "Maximum should be ommitted when CompareMode is Verify.");

                compareMode = BiometricCapabilitiesClass.CompareModesEnum.Verify;
            }


            Logger.Log(Constants.DeviceClass, "BiometricDev.SetMatchAsync()");
            var result = await Device.SetMatchAsync(new MatchRequest(compareMode, setMatch.Payload.Threshold.Value, setMatch.Payload.Identifier, setMatch.Payload.Maximum), cancel);
            Logger.Log(Constants.DeviceClass, $"BiometricDev.SetMatchAsync() -> {result.CompletionCode}");

            return new SetMatchCompletion.PayloadData(result.CompletionCode,
                                                      result.ErrorDescription,
                                                      result.ErrorCode);
        }

    }
}
