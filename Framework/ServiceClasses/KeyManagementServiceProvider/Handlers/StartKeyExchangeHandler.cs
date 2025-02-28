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
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class StartKeyExchangeHandler
    {
        private async Task<CommandResult<StartKeyExchangeCompletion.PayloadData>> HandleStartKeyExchange(IStartKeyExchangeEvents events, StartKeyExchangeCommand startKeyExchange, CancellationToken cancel)
        {
            if (!Common.KeyManagementCapabilities.SignatureScheme.HasFlag(KeyManagementCapabilitiesClass.SignatureSchemeEnum.RandomNumber))
            {
                bool certOptionOK = false;
                foreach (var loadOption in Common.KeyManagementCapabilities.LoadCertificationOptions)
                {
                    if (loadOption.Signer.HasFlag(KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.CertHost) ||
                        loadOption.Signer.HasFlag(KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.CA_TR34) ||
                        loadOption.Signer.HasFlag(KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.HL))
                    {
                        certOptionOK = true;
                        break;
                    }
                }

                if (!certOptionOK)
                {
                    return new(
                        new(StartKeyExchangeCompletion.PayloadData.ErrorCodeEnum.AccessDenied),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"No certificate or signature RKL scheme supported.");
                }
            }

            Logger.Log(Constants.DeviceClass, "KeyManagementDev.StartKeyExchange()");

            var result = await Device.StartKeyExchange(cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.StartKeyExchange() -> {result.CompletionCode}, {result.ErrorCode}");

            StartKeyExchangeCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.RandomItem?.Count > 0)
            {
                payload = new(
                    result.ErrorCode,
                    result.RandomItem);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
