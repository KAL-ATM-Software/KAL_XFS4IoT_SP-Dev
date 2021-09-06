/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class GetCertificateHandler
    {
        private async Task<GetCertificateCompletion.PayloadData> HandleGetCertificate(IGetCertificateEvents events, GetCertificateCommand getCertificate, CancellationToken cancel)
        {
            if (getCertificate.Payload.GetCertificate is null)
            {
                return new GetCertificateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                $"No certification type specified.");
            }

            Logger.Log(Constants.DeviceClass, "KeyManagementDev.ExportCertificate()");

            var result = await Device.ExportCertificate(new ExportCertificateRequest(getCertificate.Payload.GetCertificate switch
                                                                                     {
                                                                                         GetCertificateCommand.PayloadData.GetCertificateEnum.Enckey => ExportCertificateRequest.CertificateTypeEnum.EncryptionKey,
                                                                                         GetCertificateCommand.PayloadData.GetCertificateEnum.Verificationkey => ExportCertificateRequest.CertificateTypeEnum.VerificationKey,
                                                                                         _ => ExportCertificateRequest.CertificateTypeEnum.HostKey,
                                                                                     }), cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ExportCertificate() -> {result.CompletionCode}, {result.ErrorCode}");

            return new GetCertificateCompletion.PayloadData(result.CompletionCode,
                                                            result.ErrorDescription,
                                                            result.ErrorCode,
                                                            result.Certificate is not null && result.Certificate.Count > 0 ? Convert.ToBase64String(result.Certificate.ToArray()) : null);
        }
    }
}
