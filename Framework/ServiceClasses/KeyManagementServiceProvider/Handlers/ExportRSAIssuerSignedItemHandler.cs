/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoT;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class ExportRSAIssuerSignedItemHandler
    {
        private async Task<CommandResult<ExportRSAIssuerSignedItemCompletion.PayloadData>> HandleExportRSAIssuerSignedItem(IExportRSAIssuerSignedItemEvents events, ExportRSAIssuerSignedItemCommand exportRSAIssuerSignedItem, CancellationToken cancel)
        {
            if (exportRSAIssuerSignedItem.Payload.ExportItemType is null)
            {
                return new(MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No item type specified to export.");
            }

            RSASignedItemResult result;
            if (exportRSAIssuerSignedItem.Payload.ExportItemType == XFS4IoT.KeyManagement.TypeDataItemToExportEnum.DeviceId)
            {
                Logger.Log(Constants.DeviceClass, "KeyManagementDev.ExportEPPId()");

                result = await Device.ExportEPPId(new ExportEPPIdRequest(SignerEnum.Issuer), cancel);

                Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ExportEPPId() -> {result.CompletionCode}");
            }
            else
            {
                Logger.Log(Constants.DeviceClass, "KeyManagementDev.ExportRSAPublicKey()");

                result = await Device.ExportRSAPublicKey(new ExportRSAPublicKeyRequest(SignerEnum.Issuer,
                                                                                       exportRSAIssuerSignedItem.Payload.Name), 
                                                         cancel);

                Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ExportRSAPublicKey() -> {result.CompletionCode}, {result.ErrorCode}");
            }

            ExportRSAIssuerSignedItemCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.Data?.Count > 0 ||
                result.Signature?.Count > 0)
            {
                payload = new(
                    result.ErrorCode switch
                    {
                        RSASignedItemResult.ErrorCodeEnum.AccessDenied => ExportRSAIssuerSignedItemCompletion.PayloadData.ErrorCodeEnum.AccessDenied,
                        RSASignedItemResult.ErrorCodeEnum.KeyNotFound => ExportRSAIssuerSignedItemCompletion.PayloadData.ErrorCodeEnum.KeyNotFound,
                        RSASignedItemResult.ErrorCodeEnum.NoRSAKeyPair => ExportRSAIssuerSignedItemCompletion.PayloadData.ErrorCodeEnum.NoRSAKeyPair,
                        _ => null
                    },
                    result.Data,
                    result.SignatureAlgorithm switch
                    {
                        RSASignedItemResult.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5 => XFS4IoT.KeyManagement.RSASignatureAlgorithmEnum.RsassaPkcs1V15,
                        RSASignedItemResult.RSASignatureAlgorithmEnum.RSASSA_PSS => XFS4IoT.KeyManagement.RSASignatureAlgorithmEnum.RsassaPss,
                        _ => XFS4IoT.KeyManagement.RSASignatureAlgorithmEnum.Na,
                    },
                    result.Signature);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
