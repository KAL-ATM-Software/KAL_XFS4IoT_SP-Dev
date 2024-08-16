/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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

namespace XFS4IoTFramework.KeyManagement
{
    public partial class ReplaceCertificateHandler
    {
        private async Task<CommandResult<ReplaceCertificateCompletion.PayloadData>> HandleReplaceCertificate(IReplaceCertificateEvents events, ReplaceCertificateCommand replaceCertificate, CancellationToken cancel)
        {
            if (replaceCertificate.Payload.ReplaceCertificate is null ||
                replaceCertificate.Payload.ReplaceCertificate.Count == 0)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No certificate data specified.");
            }

            Logger.Log(Constants.DeviceClass, "KeyManagementDev.ReplaceCertificate()");

            var result = await Device.ReplaceCertificate(new ReplaceCertificateRequest(replaceCertificate.Payload.ReplaceCertificate), 
                                                         cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ReplaceCertificate() -> {result.CompletionCode}, {result.ErrorCode}");

            ReplaceCertificateCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.Digest?.Count > 0)
            {
                payload = new(
                    result.ErrorCode,
                    result.Digest);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
