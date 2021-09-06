/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class ResetHandler
    {
        private async Task<ResetCompletion.PayloadData> HandleReset(IResetEvents events, ResetCommand reset, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "KeyManagementDev.ResetDevice()");

            var result = await Device.ResetDevice(cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ResetDevice() -> {result.CompletionCode}");

            return new ResetCompletion.PayloadData(result.CompletionCode,
                                                   result.ErrorDescription);
        }
    }
}
