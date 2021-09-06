/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoTFramework.Common;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class LoadCertificateHandler
    {
        private async Task<LoadCertificateCompletion.PayloadData> HandleLoadCertificate(ILoadCertificateEvents events, LoadCertificateCommand loadCertificate, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(loadCertificate.Payload.CertificateData))
            {
                return new LoadCertificateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                 $"No certificate data to be loaded specified.");
            }

            if (loadCertificate.Payload.LoadOption is null)
            {
                return new LoadCertificateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                 $"No load option is specified.");
            }

            if (loadCertificate.Payload.Signer is null)
            {
                return new LoadCertificateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                 $"No signer is specified.");
            }

            bool certOptionOK = false;
            foreach (var loadOption in KeyManagement.KeyManagementCapabilities.LoadCertificationOptions)
            {
                if ((loadCertificate.Payload.LoadOption == LoadCertificateCommand.PayloadData.LoadOptionEnum.NewHost &&
                     loadOption.Option.HasFlag(KeyManagementCapabilitiesClass.LoadCertificateOptionEnum.NewHost)) ||
                    (loadCertificate.Payload.LoadOption == LoadCertificateCommand.PayloadData.LoadOptionEnum.ReplaceHost &&
                     loadOption.Option.HasFlag(KeyManagementCapabilitiesClass.LoadCertificateOptionEnum.ReplaceHost)) &&
                    (loadCertificate.Payload.Signer == LoadCertificateCommand.PayloadData.SignerEnum.CertHost &&
                     loadOption.Signer.HasFlag(KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.CertHost)) ||
                     (loadCertificate.Payload.Signer == LoadCertificateCommand.PayloadData.SignerEnum.Ca &&
                     loadOption.Signer.HasFlag(KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.CA)) ||
                     (loadCertificate.Payload.Signer == LoadCertificateCommand.PayloadData.SignerEnum.Hl &&
                     loadOption.Signer.HasFlag(KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.HL)))
                {
                    certOptionOK = true;
                    break;
                }
            }

            if (!certOptionOK)
            {
                return new LoadCertificateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                 $"No capabilties support for loading certificate. {loadCertificate.Payload.LoadOption} {loadCertificate.Payload.Signer}");
            }

            Logger.Log(Constants.DeviceClass, "KeyManagementDev.ImportCertificate()");

            var result = await Device.ImportCertificate(new ImportCertificateRequest(loadCertificate.Payload.LoadOption switch
                                                                                     {
                                                                                         LoadCertificateCommand.PayloadData.LoadOptionEnum.NewHost => ImportCertificateRequest.LoadOptionEnum.NewHost,
                                                                                         _ => ImportCertificateRequest.LoadOptionEnum.ReplaceHost
                                                                                     },
                                                                                     loadCertificate.Payload.Signer switch
                                                                                     {
                                                                                         LoadCertificateCommand.PayloadData.SignerEnum.CertHost => ImportCertificateRequest.SignerEnum.Host,
                                                                                         LoadCertificateCommand.PayloadData.SignerEnum.Ca => ImportCertificateRequest.SignerEnum.CA,
                                                                                         _ => ImportCertificateRequest.SignerEnum.HL,
                                                                                     },
                                                                                     Convert.FromBase64String(loadCertificate.Payload.CertificateData).ToList()),
                                                        cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ImportCertificate() -> {result.CompletionCode}, {result.ErrorCode}");

            return new LoadCertificateCompletion.PayloadData(result.CompletionCode,
                                                             result.ErrorDescription,
                                                             result.ErrorCode,
                                                             result.RSAKeyCheckMode switch
                                                             {
                                                                 ImportCertificateResult.RSAKeyCheckModeEnum.SHA1 => LoadCertificateCompletion.PayloadData.RsaKeyCheckModeEnum.Sha1,
                                                                 ImportCertificateResult.RSAKeyCheckModeEnum.SHA256 => LoadCertificateCompletion.PayloadData.RsaKeyCheckModeEnum.Sha256,
                                                                 _ => LoadCertificateCompletion.PayloadData.RsaKeyCheckModeEnum.None,
                                                             },
                                                             result.RSAData is not null && result.RSAData.Count > 0 ? Convert.ToBase64String(result.RSAData.ToArray()) : null);
        }
    }
}
