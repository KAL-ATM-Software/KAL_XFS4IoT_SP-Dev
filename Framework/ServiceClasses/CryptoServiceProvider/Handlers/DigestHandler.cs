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
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Crypto.Commands;
using XFS4IoT.Crypto.Completions;
using XFS4IoTFramework.Common;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Crypto
{
    public partial class DigestHandler
    {
        private async Task<CommandResult<DigestCompletion.PayloadData>> HandleDigest(IDigestEvents events, DigestCommand digest, CancellationToken cancel)
        {
            if (digest.Payload.HashAlgorithm is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    $"No hash algorithm specified.");
            }
            if (digest.Payload.Data is null ||
                digest.Payload.Data.Count == 0)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    $"No data specified to generate hash.");
            }

            if ((digest.Payload.HashAlgorithm == DigestCommand.PayloadData.HashAlgorithmEnum.Sha1 &&
                !Common.CryptoCapabilities.EMVHashAlgorithms.HasFlag(CryptoCapabilitiesClass.EMVHashAlgorithmEnum.SHA1_Digest)) ||
                (digest.Payload.HashAlgorithm == DigestCommand.PayloadData.HashAlgorithmEnum.Sha256 &&
                !Common.CryptoCapabilities.EMVHashAlgorithms.HasFlag(CryptoCapabilitiesClass.EMVHashAlgorithmEnum.SHA256_Digest)))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    $"Specified EMV has algorith is not supported. {digest.Payload.HashAlgorithm}. See capabilities");
            }

            Logger.Log(Constants.DeviceClass, "CryptoDev.GenerateDigest()");

            var result = await Device.GenerateDigest(
                new GenerateDigestRequest(
                    digest.Payload.HashAlgorithm switch
                    {
                        DigestCommand.PayloadData.HashAlgorithmEnum.Sha1=> HashAlgorithmEnum.SHA1,
                        _ => HashAlgorithmEnum.SHA256 
                    }, 
                    digest.Payload.Data),
                cancel);

            Logger.Log(Constants.DeviceClass, $"CryptoDev.GenerateDigest() -> {result.CompletionCode}, {result.ErrorCode}");

            DigestCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.Digest?.Count > 0)
            {
                payload = new(
                    ErrorCode: result.ErrorCode,
                    Digest: result.Digest);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
