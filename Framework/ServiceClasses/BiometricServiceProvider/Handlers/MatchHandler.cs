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
using XFS4IoT.Biometric.Commands;
using XFS4IoT.Biometric.Completions;
using XFS4IoTFramework.Common;
using System.Collections.Generic;

namespace XFS4IoTFramework.Biometric
{
    public partial class MatchHandler
    {

        private async Task<CommandResult<MatchCompletion.PayloadData>> HandleMatch(IMatchEvents events, MatchCommand match, CancellationToken cancel)
        {
            if (match is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    "No payload specified.");
            }

            if (match.Payload?.CompareMode is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    "CompareMode was not specified.");
            }

            if (match.Payload?.Threshold is null)
            {
                return new(
                    new(MatchCompletion.PayloadData.ErrorCodeEnum.InvalidThreshold),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    "Threshold was not specified.");
            }

            if (match.Payload.Threshold.Value < 0 || match.Payload.Threshold.Value > 100)
            {
                return new(
                    new(MatchCompletion.PayloadData.ErrorCodeEnum.InvalidThreshold),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    "Threshold must be within range 0-100.");
            }

            BiometricCapabilitiesClass.CompareModesEnum compareMode;

            if (match.Payload.CompareMode is MatchCommand.PayloadData.CompareModeEnum.Identity)
            {
                if (!Common.BiometricCapabilities.CompareModes.HasFlag(BiometricCapabilitiesClass.CompareModesEnum.Identity))
                {
                    return new(
                        new(MatchCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        "CompareMode.Identity is not supported by this device.");
                }
                else if (match.Payload.Maximum is null)
                {
                    return new(
                        new(MatchCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        "Maximum must be set when CompareMode is Identity.");
                }

                if (!string.IsNullOrWhiteSpace(match.Payload.Identifier))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        "Identifier should be ommitted when CompareMode is Identity.");
                }

                compareMode = BiometricCapabilitiesClass.CompareModesEnum.Identity;
            }
            else
            {
                if (!Common.BiometricCapabilities.CompareModes.HasFlag(BiometricCapabilitiesClass.CompareModesEnum.Verify))
                    return new(
                        new(MatchCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        "CompareMode.Verify is not supported by this device.");

                var deviceStorageInfo = Device.StorageInfo;

                if (deviceStorageInfo == null || deviceStorageInfo.Count == 0)
                {
                    return new(
                        new(MatchCompletion.PayloadData.ErrorCodeEnum.NoImportedData),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"No data previously imported using Import command.");
                }

                if (string.IsNullOrWhiteSpace(match.Payload.Identifier))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        "Identifier must be specified when CompareMode is Verify.");
                }

                if (!deviceStorageInfo.ContainsKey(match.Payload.Identifier))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Identifier supplied is not stored by the device. {match.Payload.Identifier}");
                }

                if (match.Payload.Maximum is not null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        "Maximum should be ommitted when CompareMode is Verify.");
                }

                compareMode = BiometricCapabilitiesClass.CompareModesEnum.Verify;
            }


            Logger.Log(Constants.DeviceClass, "BiometricDev.MatchAsync()");

            var result = await Device.MatchAsync(new MatchRequest(compareMode, match.Payload.Threshold.Value, match.Payload.Identifier, match.Payload.Maximum), cancel);
            
            Logger.Log(Constants.DeviceClass, $"BiometricDev.MatchAsync() -> {result.CompletionCode}");


            Dictionary<string, XFS4IoT.Biometric.Completions.MatchCompletion.PayloadData.CandidatesClass> candidates = null;
            if (result.Candidates?.Count > 0)
            {
                candidates = [];
                foreach(var candidate in result.Candidates)
                {
                    candidate.Key.IsNotNullOrWhitespace("Match candidate returned by device used invalid id.");
                    Contracts.IsTrue(candidate.Value.ConfidenceLevel >= 0 && candidate.Value.ConfidenceLevel <= 100, $"MatchCandidate ConfidenceLevel returned by device is not within 0-100 range. {candidate.Value.ConfidenceLevel}");
                    Contracts.IsTrue(candidate.Value.ConfidenceLevel >= match.Payload.Threshold, $"MatchCandidate ConfidenceLevel returned by device is not >= to the Threshold requested. {candidate.Value.ConfidenceLevel}-{match.Payload.Threshold}");

                    candidates.Add(candidate.Key, new(candidate.Value.ConfidenceLevel, candidate.Value.Data));
                }
            }

            MatchCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                candidates is not null)
            {
                payload = new(
                    result.ErrorCode,
                    candidates);
            }
            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }

    }
}
