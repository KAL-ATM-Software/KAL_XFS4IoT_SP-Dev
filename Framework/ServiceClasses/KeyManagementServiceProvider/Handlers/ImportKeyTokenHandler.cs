/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ImportKeyTokenHandler.cs uses automatically generated parts.
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class ImportKeyTokenHandler
    {
        private async Task<CommandResult<ImportKeyTokenCompletion.PayloadData>> HandleImportKeyToken(IImportKeyTokenEvents events, ImportKeyTokenCommand importKeyToken, CancellationToken cancel)
        {
            if (importKeyToken.Payload.KeyToken is null ||
                importKeyToken.Payload.KeyToken.Count == 0)
            {
                return new(
                    CompletionCode: MessageHeader.CompletionCodeEnum.InvalidData,
                    ErrorDescription: $"No key token specified.");
            }

            // if key name is in the token if the load option with CRL
            if (string.IsNullOrEmpty(importKeyToken.Payload.Key) &&
                (importKeyToken.Payload.LoadOption == ImportKeyTokenCommand.PayloadData.LoadOptionEnum.NoRandom ||
                 importKeyToken.Payload.LoadOption == ImportKeyTokenCommand.PayloadData.LoadOptionEnum.Random))
            {
                return new(
                    CompletionCode: MessageHeader.CompletionCodeEnum.InvalidData,
                    ErrorDescription: $"No key name specified when the load option is specified {importKeyToken.Payload.LoadOption}.");
            }

            if (string.IsNullOrEmpty(importKeyToken.Payload.KeyUsage))
            {
                return new(
                    CompletionCode: MessageHeader.CompletionCodeEnum.InvalidData,
                    ErrorDescription: $"No key usage specified.");
            }
            if (!Regex.IsMatch(importKeyToken.Payload.KeyUsage, KeyDetail.regxKeyUsage))
            {
                return new(
                    CompletionCode: MessageHeader.CompletionCodeEnum.InvalidData,
                    ErrorDescription: $"Invalid key usage specified. {importKeyToken.Payload.KeyUsage}");
            }
            // Check key attributes supported
            if (!Common.KeyManagementCapabilities.KeyAttributes.ContainsKey(importKeyToken.Payload.KeyUsage))
            {
                return new(
                    new(ErrorCode: ImportKeyTokenCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                    CompletionCode: MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    ErrorDescription: $"Specified key attribute is not supported. {importKeyToken.Payload.KeyUsage}");
            }

            Logger.Log(Constants.DeviceClass, "KeyManagementDev.ImportKeyToken()");

            var importKeyTokenResult = await Device.ImportKeyToken(new ImportKeyTokenRequest(
                KeyName: importKeyToken.Payload.Key is null ? string.Empty : importKeyToken.Payload.Key,
                KeyUsage: importKeyToken.Payload.KeyUsage,
                KeyToken: importKeyToken.Payload.KeyToken,
                LoadOption: importKeyToken.Payload.LoadOption switch
                {
                    ImportKeyTokenCommand.PayloadData.LoadOptionEnum.NoRandom => ImportKeyTokenRequest.LoadOptionEnum.NoRandom,
                    ImportKeyTokenCommand.PayloadData.LoadOptionEnum.Random => ImportKeyTokenRequest.LoadOptionEnum.Random,
                    ImportKeyTokenCommand.PayloadData.LoadOptionEnum.NoRandomCrl => ImportKeyTokenRequest.LoadOptionEnum.NoRandom_CRL,
                    ImportKeyTokenCommand.PayloadData.LoadOptionEnum.RandomCrl => ImportKeyTokenRequest.LoadOptionEnum.Random_CRL,
                    _ => throw new InvalidDataException($"Invalid LoadOption specified {importKeyToken.Payload.LoadOption}.")
                }),
                cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ImportKeyToken() -> {importKeyTokenResult.CompletionCode}, {importKeyTokenResult.ErrorCode}");

            ImportKeyTokenCompletion.PayloadData payload = null;
            if (importKeyTokenResult.ErrorCode is not null ||
                importKeyTokenResult.KCV?.Count > 0 ||
                importKeyTokenResult.AuthenticationData?.Count > 0)
            {
                payload = new(
                    ErrorCode: importKeyTokenResult.ErrorCode,
                    KeyLength: importKeyTokenResult.KeyLength,
                    KeyAcceptAlgorithm: importKeyTokenResult.AuthenticationAlgorithm == ImportKeyTokenResult.AuthenticationAlgorithmEnum.None ?
                    null :
                    importKeyTokenResult.AuthenticationAlgorithm switch
                    {
                        ImportKeyTokenResult.AuthenticationAlgorithmEnum.SHA1 => ImportKeyTokenCompletion.PayloadData.KeyAcceptAlgorithmEnum.Sha1,
                        ImportKeyTokenResult.AuthenticationAlgorithmEnum.SHA256 => ImportKeyTokenCompletion.PayloadData.KeyAcceptAlgorithmEnum.Sha256,
                        _ => throw new InternalErrorException($"Unexpected authentication algorithm is specified. {importKeyTokenResult.AuthenticationAlgorithm}"),
                    },
                    KeyAcceptData: importKeyTokenResult.AuthenticationAlgorithm == ImportKeyTokenResult.AuthenticationAlgorithmEnum.None ?
                    null : importKeyTokenResult.AuthenticationData,
                    KeyCheckMode: importKeyTokenResult.KCVMode == ImportKeyTokenResult.KeyCheckValueEnum.None ?
                    null :
                    importKeyTokenResult.KCVMode switch
                    {
                        ImportKeyTokenResult.KeyCheckValueEnum.Self => ImportKeyTokenCompletion.PayloadData.KeyCheckModeEnum.KcvSelf,
                        ImportKeyTokenResult.KeyCheckValueEnum.Zero => ImportKeyTokenCompletion.PayloadData.KeyCheckModeEnum.KcvZero,
                        _ => throw new InternalErrorException($"Unexpected KCV mode is specified. {importKeyTokenResult.KCVMode}"),
                    },
                    KeyCheckValue: importKeyTokenResult.KCVMode == ImportKeyTokenResult.KeyCheckValueEnum.None ?
                    null : importKeyTokenResult.KCV);
            }

            return new(
                payload,
                CompletionCode: importKeyTokenResult.CompletionCode,
                ErrorDescription: importKeyTokenResult.ErrorDescription);
        }
    }
}
