/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;

namespace XFS4IoTFramework.CardReader
{
    public partial class ResetCountHandler
    {
        private async Task<ResetCountCompletion.PayloadData> HandleResetCount(IResetCountEvents events, ResetCountCommand resetCount, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CardReaderDev.ResetBinCountAsync()");
            var result = await Device.ResetBinCountAsync(cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ResetBinCountAsync() -> {result.CompletionCode}");

            return new ResetCountCompletion.PayloadData(result.CompletionCode, result.ErrorDescription);
        }
    }
}
