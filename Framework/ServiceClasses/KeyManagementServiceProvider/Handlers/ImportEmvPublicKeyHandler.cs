/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ImportEmvPublicKeyHandler.cs uses automatically generated parts.
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using System.Text.RegularExpressions;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class ImportEmvPublicKeyHandler
    {
        private async Task<CommandResult<ImportEmvPublicKeyCompletion.PayloadData>> HandleImportEmvPublicKey(IImportEmvPublicKeyEvents events, ImportEmvPublicKeyCommand importEmvPublicKey, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(importEmvPublicKey.Payload.Key))
            {
                return new(
                    CompletionCode: MessageHeader.CompletionCodeEnum.InvalidData,
                    ErrorDescription: $"No key name is specified.");
            }

            if (string.IsNullOrEmpty(importEmvPublicKey.Payload.KeyUsage))
            {
                return new(
                    CompletionCode: MessageHeader.CompletionCodeEnum.InvalidData,
                    ErrorDescription: $"No key usage is specified.");
            }
            if (!Regex.IsMatch(importEmvPublicKey.Payload.KeyUsage, KeyDetail.regxEMVKeyUsage))
            {
                return new(
                    CompletionCode: MessageHeader.CompletionCodeEnum.InvalidData,
                    ErrorDescription: $"Invalid key usage specified. {importEmvPublicKey.Payload.KeyUsage}");
            }
            // Check key attributes supported
            if (!Common.KeyManagementCapabilities.KeyAttributes.ContainsKey(importEmvPublicKey.Payload.KeyUsage))
            {
                return new(
                    new(ErrorCode: ImportEmvPublicKeyCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                    CompletionCode: MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    ErrorDescription: $"Specified key attribute is not supported. {importEmvPublicKey.Payload.KeyUsage}");
            }

            if (importEmvPublicKey.Payload.ImportScheme is null)
            {
                return new(
                    CompletionCode: MessageHeader.CompletionCodeEnum.InvalidData,
                    ErrorDescription: $"No import scheme is specified.");
            }

            if (importEmvPublicKey.Payload.Value is null ||
                importEmvPublicKey.Payload.Value.Count == 0)
            {
                return new(
                    CompletionCode: MessageHeader.CompletionCodeEnum.InvalidData,
                    ErrorDescription: $"No import data is specified.");
            }

            string verificationKey = importEmvPublicKey.Payload.VerifyKey;
            if (importEmvPublicKey.Payload.ImportScheme == ImportEmvPublicKeyCommand.PayloadData.ImportSchemeEnum.PlainCA ||
                importEmvPublicKey.Payload.ImportScheme == ImportEmvPublicKeyCommand.PayloadData.ImportSchemeEnum.ChecksumCA ||
                importEmvPublicKey.Payload.ImportScheme == ImportEmvPublicKeyCommand.PayloadData.ImportSchemeEnum.EpiCA)
            {
                // Import scheme doesn't require verification key.
                verificationKey = string.Empty;
            }
            else
            {
                if (string.IsNullOrEmpty(verificationKey))
                {
                    return new(
                        CompletionCode: MessageHeader.CompletionCodeEnum.InvalidData,
                        ErrorDescription: $"No verify key specified for this import scheme. {importEmvPublicKey.Payload.ImportScheme}");
                }
            }
            Logger.Log(Constants.DeviceClass, "KeyManagementDev.ImportEMVPublicKey()");

            var importEMVPublicKeyResult = await Device.ImportEMVPublicKey(new ImportEMVPublicKeyRequest(
                KeyName: importEmvPublicKey.Payload.Key is null ? string.Empty : importEmvPublicKey.Payload.Key,
                KeyUsage: importEmvPublicKey.Payload.KeyUsage,
                ImportScheme: importEmvPublicKey.Payload.ImportScheme switch
                {
                    ImportEmvPublicKeyCommand.PayloadData.ImportSchemeEnum.PlainCA => ImportEMVPublicKeyRequest.ImportSchemeEnum.PlainText_CA,
                    ImportEmvPublicKeyCommand.PayloadData.ImportSchemeEnum.ChecksumCA => ImportEMVPublicKeyRequest.ImportSchemeEnum.PlainText_Checksum_CA,
                    ImportEmvPublicKeyCommand.PayloadData.ImportSchemeEnum.EpiCA => ImportEMVPublicKeyRequest.ImportSchemeEnum.EPI_CA,
                    ImportEmvPublicKeyCommand.PayloadData.ImportSchemeEnum.Issuer => ImportEMVPublicKeyRequest.ImportSchemeEnum.Issuer,
                    ImportEmvPublicKeyCommand.PayloadData.ImportSchemeEnum.Icc => ImportEMVPublicKeyRequest.ImportSchemeEnum.ICC,
                    ImportEmvPublicKeyCommand.PayloadData.ImportSchemeEnum.IccPIN => ImportEMVPublicKeyRequest.ImportSchemeEnum.ICC_PIN,
                    ImportEmvPublicKeyCommand.PayloadData.ImportSchemeEnum.PkcsV1_5_CA => ImportEMVPublicKeyRequest.ImportSchemeEnum.PKCSV1_5_CA,
                    _ => throw new InvalidDataException($"Invalid ImportScheme specified {importEmvPublicKey.Payload.ImportScheme}.")
                },
                ImportData: importEmvPublicKey.Payload.Value,
                VerificationKeyName: verificationKey),
                cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ImportEMVPublicKey() -> {importEMVPublicKeyResult.CompletionCode}, {importEMVPublicKeyResult.ErrorCode}");

            ImportEmvPublicKeyCompletion.PayloadData payload = null;
            if (importEMVPublicKeyResult.ErrorCode is not null ||
                !string.IsNullOrEmpty(importEMVPublicKeyResult.ExpiryDate))
            {
                payload = new(
                    ErrorCode: importEMVPublicKeyResult.ErrorCode,
                    ExpiryDate: importEMVPublicKeyResult.ExpiryDate);
            }

            return new(
                payload,
                CompletionCode: importEMVPublicKeyResult.CompletionCode,
                ErrorDescription: importEMVPublicKeyResult.ErrorDescription);
        }
    }
}
