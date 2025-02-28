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
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class ExportRSADeviceSignedItemHandler
    {
        private async Task<CommandResult<ExportRSADeviceSignedItemCompletion.PayloadData>> HandleExportRSADeviceSignedItem(IExportRSADeviceSignedItemEvents events, ExportRSADeviceSignedItemCommand exportRSADeviceSignedItem, CancellationToken cancel)
        {
            if (exportRSADeviceSignedItem.Payload.ExportItemType is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No item type specified to export.");
            }

            if (!Common.KeyManagementCapabilities.SignatureScheme.HasFlag(KeyManagementCapabilitiesClass.SignatureSchemeEnum.ExportEPPID))
            {
                return new(
                    new(ExportRSADeviceSignedItemCompletion.PayloadData.ErrorCodeEnum.NoRSAKeyPair),
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"The device doesn't support to RSA signature scheme.");
            }

            if (!string.IsNullOrEmpty(exportRSADeviceSignedItem.Payload.SigKey))
            {
                KeyDetail keyDetail = KeyManagement.GetKeyDetail(exportRSADeviceSignedItem.Payload.SigKey);
                if (keyDetail is null)
                {
                    return new(
                        new(ExportRSADeviceSignedItemCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified signature key is not found. {exportRSADeviceSignedItem.Payload.SigKey}");
                }
                if (keyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
                {
                    return new(
                        new(ExportRSADeviceSignedItemCompletion.PayloadData.ErrorCodeEnum.AccessDenied),
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified signature key is not loaded or unknown state. {exportRSADeviceSignedItem.Payload.SigKey}");
                }
            }

            RSASignedItemResult result;
            if (exportRSADeviceSignedItem.Payload.ExportItemType == XFS4IoT.KeyManagement.TypeDataItemToExportEnum.DeviceId)
            {
                Logger.Log(Constants.DeviceClass, "KeyManagementDev.ExportEPPId()");

                result = await Device.ExportEPPId(
                    new ExportEPPIdRequest(
                        SignerEnum.EPP,
                        exportRSADeviceSignedItem.Payload.SignatureAlgorithm switch
                        {
                            XFS4IoT.KeyManagement.RSASignatureAlgorithmEnum.RsassaPkcs1V15 => ExportEPPIdRequest.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5,
                            XFS4IoT.KeyManagement.RSASignatureAlgorithmEnum.RsassaPss => ExportEPPIdRequest.RSASignatureAlgorithmEnum.RSASSA_PSS,
                            _ => ExportEPPIdRequest.RSASignatureAlgorithmEnum.NoSignature
                        },
                        exportRSADeviceSignedItem.Payload.SigKey),
                    cancel);

                Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ExportEPPId() -> {result.CompletionCode}");
            }
            else
            {
                Logger.Log(Constants.DeviceClass, "KeyManagementDev.ExportRSAPublicKey()");

                result = await Device.ExportRSAPublicKey(
                    new ExportRSAPublicKeyRequest(
                        SignerEnum.EPP,
                        exportRSADeviceSignedItem.Payload.Name,
                        exportRSADeviceSignedItem.Payload.SignatureAlgorithm switch
                        {
                            XFS4IoT.KeyManagement.RSASignatureAlgorithmEnum.RsassaPkcs1V15 => ExportRSAPublicKeyRequest.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5,
                            XFS4IoT.KeyManagement.RSASignatureAlgorithmEnum.RsassaPss => ExportRSAPublicKeyRequest.RSASignatureAlgorithmEnum.RSASSA_PSS,
                            _ => ExportRSAPublicKeyRequest.RSASignatureAlgorithmEnum.NoSignature
                        },
                        exportRSADeviceSignedItem.Payload.SigKey),
                    cancel);

                Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ExportRSAPublicKey() -> {result.CompletionCode}, {result.ErrorCode}");
            }

            ExportRSADeviceSignedItemCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.Data?.Count > 0 ||
                result.SelfSignature?.Count > 0 ||
                result.Signature?.Count > 0)
            {
                payload = new(
                    result.ErrorCode switch
                    {
                        RSASignedItemResult.ErrorCodeEnum.AccessDenied => ExportRSADeviceSignedItemCompletion.PayloadData.ErrorCodeEnum.AccessDenied,
                        RSASignedItemResult.ErrorCodeEnum.KeyNotFound => ExportRSADeviceSignedItemCompletion.PayloadData.ErrorCodeEnum.KeyNotFound,
                        RSASignedItemResult.ErrorCodeEnum.NoRSAKeyPair => ExportRSADeviceSignedItemCompletion.PayloadData.ErrorCodeEnum.NoRSAKeyPair,
                        _ => null,
                    },
                    result.Data,
                    result.SelfSignature,
                    result.Signature);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
