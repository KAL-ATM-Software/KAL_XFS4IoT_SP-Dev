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
using XFS4IoTServer;
using XFS4IoT.Crypto.Commands;
using XFS4IoT.Crypto.Completions;

namespace XFS4IoTFramework.Crypto
{
    public partial class GenerateRandomHandler
    {
        private async Task<GenerateRandomCompletion.PayloadData> HandleGenerateRandom(IGenerateRandomEvents events, GenerateRandomCommand generateRandom, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CryptoDev.GenerateRandomNumber()");

            var result = await Device.GenerateRandomNumber(cancel);

            Logger.Log(Constants.DeviceClass, $"CryptoDev.GenerateRandomNumber() -> {result.CompletionCode}, {result.ErrorCode}");

            byte[] random = result.RandomNumber?.ToArray();
            return new GenerateRandomCompletion.PayloadData(result.CompletionCode,
                                                            result.ErrorDescription,
                                                            result.ErrorCode,
                                                            random is null ? string.Empty : Convert.ToBase64String(random));
        }
    }
}
