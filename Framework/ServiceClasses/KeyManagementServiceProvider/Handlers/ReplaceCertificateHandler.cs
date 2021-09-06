/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Linq;
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
    public partial class ReplaceCertificateHandler
    {
        private async Task<ReplaceCertificateCompletion.PayloadData> HandleReplaceCertificate(IReplaceCertificateEvents events, ReplaceCertificateCommand replaceCertificate, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(replaceCertificate.Payload.ReplaceCertificate) ||
                replaceCertificate.Payload.ReplaceCertificate.Length == 0)
            {
                return new ReplaceCertificateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                    $"No certificate data specified.");
            }

            Logger.Log(Constants.DeviceClass, "KeyManagementDev.ReplaceCertificate()");

            var result = await Device.ReplaceCertificate(new ReplaceCertificateRequest(Convert.FromBase64String(replaceCertificate.Payload.ReplaceCertificate).ToList()), 
                                                         cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ReplaceCertificate() -> {result.CompletionCode}, {result.ErrorCode}");

            return new ReplaceCertificateCompletion.PayloadData(result.CompletionCode,
                                                                result.ErrorDescription,
                                                                result.ErrorCode,
                                                                result.Digest is not null && result.Digest.Count > 0 ? Convert.ToBase64String(result.Digest.ToArray()) : null);
        }
    }
}
