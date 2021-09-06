/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class ExportRSAEPPSignedItemHandler
    {
        private async Task<ExportRSAEPPSignedItemCompletion.PayloadData> HandleExportRSAEPPSignedItem(IExportRSAEPPSignedItemEvents events, ExportRSAEPPSignedItemCommand exportRSAEPPSignedItem, CancellationToken cancel)
        {
            if (exportRSAEPPSignedItem.Payload.ExportItemType is null)
            {
                return new ExportRSAEPPSignedItemCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"No item type specified to export.");
            }

            if (!KeyManagement.KeyManagementCapabilities.SignatureScheme.HasFlag(KeyManagementCapabilitiesClass.SignatureSchemeEnum.RSAKeyPair))
            {
                return new ExportRSAEPPSignedItemCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"The device doesn't support to RSA signature scheme.",
                                                                        ExportRSAEPPSignedItemCompletion.PayloadData.ErrorCodeEnum.NoRSAKeyPair);
            }

            if (!string.IsNullOrEmpty(exportRSAEPPSignedItem.Payload.SigKey))
            {
                KeyDetail keyDetail = KeyManagement.GetKeyDetail(exportRSAEPPSignedItem.Payload.SigKey);
                if (keyDetail is null)
                {
                    return new ExportRSAEPPSignedItemCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                            $"Specified signature key is not found. {exportRSAEPPSignedItem.Payload.SigKey}",
                                                                            ExportRSAEPPSignedItemCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
                }
                if (keyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
                {
                    return new ExportRSAEPPSignedItemCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                            $"Specified signature key is not loaded or unknown state. {exportRSAEPPSignedItem.Payload.SigKey}",
                                                                            ExportRSAEPPSignedItemCompletion.PayloadData.ErrorCodeEnum.AccessDenied);
                }
            }

            RSASignedItemResult result;
            if (exportRSAEPPSignedItem.Payload.ExportItemType == XFS4IoT.KeyManagement.TypeDataItemToExportEnum.EppId)
            {
                Logger.Log(Constants.DeviceClass, "KeyManagementDev.ExportEPPId()");

                result = await Device.ExportEPPId(new ExportEPPIdRequest(SignerEnum.EPP,
                                                                         exportRSAEPPSignedItem.Payload.SignatureAlgorithm switch
                                                                         {
                                                                             XFS4IoT.KeyManagement.RSASignatureAlgorithmEnum.RsassaPkcs1V15 => ExportEPPIdRequest.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5,
                                                                             XFS4IoT.KeyManagement.RSASignatureAlgorithmEnum.RsassaPss => ExportEPPIdRequest.RSASignatureAlgorithmEnum.RSASSA_PSS,
                                                                             _ => ExportEPPIdRequest.RSASignatureAlgorithmEnum.NoSignature
                                                                         },
                                                                         exportRSAEPPSignedItem.Payload.SigKey),
                                                  cancel);

                Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ExportEPPId() -> {result.CompletionCode}");
            }
            else
            {
                Logger.Log(Constants.DeviceClass, "KeyManagementDev.ExportRSAPublicKey()");

                result = await Device.ExportRSAPublicKey(new ExportRSAPublicKeyRequest(SignerEnum.EPP,
                                                                                       exportRSAEPPSignedItem.Payload.Name,
                                                                                       exportRSAEPPSignedItem.Payload.SignatureAlgorithm switch
                                                                                       {
                                                                                           XFS4IoT.KeyManagement.RSASignatureAlgorithmEnum.RsassaPkcs1V15 => ExportRSAPublicKeyRequest.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5,
                                                                                           XFS4IoT.KeyManagement.RSASignatureAlgorithmEnum.RsassaPss => ExportRSAPublicKeyRequest.RSASignatureAlgorithmEnum.RSASSA_PSS,
                                                                                           _ => ExportRSAPublicKeyRequest.RSASignatureAlgorithmEnum.NoSignature
                                                                                       },
                                                                                       exportRSAEPPSignedItem.Payload.SigKey), 
                                                         cancel);

                Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ExportRSAPublicKey() -> {result.CompletionCode}, {result.ErrorCode}");
            }

            return new ExportRSAEPPSignedItemCompletion.PayloadData(result.CompletionCode,
                                                                    result.ErrorDescription,
                                                                    result.ErrorCode is not null ? result.ErrorCode switch
                                                                    {
                                                                        RSASignedItemResult.ErrorCodeEnum.AccessDenied => ExportRSAEPPSignedItemCompletion.PayloadData.ErrorCodeEnum.AccessDenied,
                                                                        RSASignedItemResult.ErrorCodeEnum.KeyNotFound => ExportRSAEPPSignedItemCompletion.PayloadData.ErrorCodeEnum.KeyNotFound,
                                                                        _ => ExportRSAEPPSignedItemCompletion.PayloadData.ErrorCodeEnum.NoRSAKeyPair,
                                                                    } : null,
                                                                    result.Data is not null && result.Data.Count > 0 ? Convert.ToBase64String(result.Data.ToArray()) : null,
                                                                    result.SelfSignature is not null && result.SelfSignature.Count > 0 ? Convert.ToBase64String(result.SelfSignature.ToArray()) : null,
                                                                    result.Signature is not null && result.Signature.Count > 0 ? Convert.ToBase64String(result.Signature.ToArray()) : null);
        }
    }
}
