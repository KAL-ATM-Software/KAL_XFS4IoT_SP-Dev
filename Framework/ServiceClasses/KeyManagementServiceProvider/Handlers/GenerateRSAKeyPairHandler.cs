/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using XFS4IoT;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class GenerateRSAKeyPairHandler
    {
        private async Task<CommandResult<GenerateRSAKeyPairCompletion.PayloadData>> HandleGenerateRSAKeyPair(IGenerateRSAKeyPairEvents events, GenerateRSAKeyPairCommand generateRSAKeyPair, CancellationToken cancel)
        {
            if (generateRSAKeyPair.Payload.Key is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No key name specified.");
            }

            // Check key attributes supported
            List<string> keyUsages = ["S0", "S1", "S2"];
            for (int i = 0; i < 100; i++)
                keyUsages.Add(i.ToString("00"));
            bool asymmetricKeySupported = false;
            foreach (string keyUsage in keyUsages)
            {
                if (Common.KeyManagementCapabilities.KeyAttributes.ContainsKey(keyUsage))
                {
                    List<string> algorithms = [ "R" ];
                    for (int i = 0; i < 10; i++)
                        algorithms.Add(i.ToString("0"));
                    foreach (string algorithm in algorithms)
                    {
                        if (Common.KeyManagementCapabilities.KeyAttributes[keyUsage].ContainsKey(algorithm))
                        {
                            List<string> modes = ["S", "T"];
                            for (int i = 0; i < 10; i++)
                                modes.Add(i.ToString("0"));
                            foreach (string mode in modes)
                            {
                                asymmetricKeySupported = Common.KeyManagementCapabilities.KeyAttributes[keyUsage][algorithm].ContainsKey(mode);
                                if (asymmetricKeySupported)
                                    break;
                            }
                        }
                        if (asymmetricKeySupported)
                            break;
                    }
                }
                if (asymmetricKeySupported)
                    break;
            }

            if (!asymmetricKeySupported)
            {
                return new(
                    new(GenerateRSAKeyPairCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No asymmetric key, algorithm or mode supported.");
            }

            int keySlot = KeyManagement.FindKeySlot(generateRSAKeyPair.Payload.Key);

            Logger.Log(Constants.DeviceClass, "KeyManagementDev.GenerateRSAKeyPair()");

            var result = await Device.GenerateRSAKeyPair(
                new GenerateRSAKeyPairRequest(
                    generateRSAKeyPair.Payload.Key,
                    keySlot,
                    generateRSAKeyPair.Payload.Use switch
                    {
                        GenerateRSAKeyPairCommand.PayloadData.UseEnum.RsaPrivate => GenerateRSAKeyPairRequest.ModeOfUseEnum.T,
                        _ => GenerateRSAKeyPairRequest.ModeOfUseEnum.S
                    },
                    generateRSAKeyPair.Payload.ModulusLength is null ? 0 : (int)generateRSAKeyPair.Payload.ModulusLength,
                    generateRSAKeyPair.Payload.ExponentValue is null ? GenerateRSAKeyPairRequest.ExponentEnum.Default : generateRSAKeyPair.Payload.ExponentValue switch
                    {
                        GenerateRSAKeyPairCommand.PayloadData.ExponentValueEnum.Exponent1 => GenerateRSAKeyPairRequest.ExponentEnum.Exponent1,
                        GenerateRSAKeyPairCommand.PayloadData.ExponentValueEnum.Exponent4 => GenerateRSAKeyPairRequest.ExponentEnum.Exponent4,
                        GenerateRSAKeyPairCommand.PayloadData.ExponentValueEnum.Exponent16 => GenerateRSAKeyPairRequest.ExponentEnum.Exponent16,
                        _ => GenerateRSAKeyPairRequest.ExponentEnum.Default,
                    }),
                cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.GenerateRSAKeyPair() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
            {
                if (result.LoadedKeyDetail == null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InternalError,
                        $"No key detailed information provided by the device class.");
                }

                if (string.IsNullOrEmpty(result.LoadedKeyDetail.KeyUsage) ||
                    !Regex.IsMatch(result.LoadedKeyDetail.KeyUsage, "^S[0-2]$|^[0-9][0-9]$"))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InternalError,
                        $"Invalid key usage specified by the device class. {result.LoadedKeyDetail?.KeyUsage}");
                }
                if (string.IsNullOrEmpty(result.LoadedKeyDetail.Algorithm) ||
                    !Regex.IsMatch(result.LoadedKeyDetail.Algorithm, "^[R0-9]$"))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InternalError,
                        $"Invalid algorithm specified by the device class. {result.LoadedKeyDetail?.Algorithm}");
                }
                if (string.IsNullOrEmpty(result.LoadedKeyDetail.ModeOfUse) ||
                    !Regex.IsMatch(result.LoadedKeyDetail.ModeOfUse, "^[ST0-9]]$"))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InternalError,
                        $"Invalid mode specified by the device class. {result.LoadedKeyDetail?.ModeOfUse}");
                }

                // Successfully loaded and add key information managing internally
                KeyManagement.AddKey(
                    generateRSAKeyPair.Payload.Key,
                    result.UpdatedKeySlot is null ? keySlot : (int)result.UpdatedKeySlot,
                    result.LoadedKeyDetail.KeyUsage,
                    result.LoadedKeyDetail.Algorithm,
                    result.LoadedKeyDetail.ModeOfUse,
                    result.LoadedKeyDetail.KeyLength,
                    KeyDetail.KeyStatusEnum.Loaded,
                    false,
                    null,
                    result.LoadedKeyDetail.KeyVersionNumber,
                    result.LoadedKeyDetail.Exportability,
                    result.LoadedKeyDetail.OptionalKeyBlockHeader,
                    result.LoadedKeyDetail.Generation,
                    result.LoadedKeyDetail.ActivatingDate,
                    result.LoadedKeyDetail.ExpiryDate,
                    result.LoadedKeyDetail.Version);
            }

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
