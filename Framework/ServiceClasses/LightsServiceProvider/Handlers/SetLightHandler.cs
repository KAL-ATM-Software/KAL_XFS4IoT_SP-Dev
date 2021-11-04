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
using XFS4IoT.Lights.Commands;
using XFS4IoT.Lights.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Lights
{
    public partial class SetLightHandler
    {

        private async Task<SetLightCompletion.PayloadData> HandleSetLight(ISetLightEvents events, SetLightCommand setLight, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "LightsDev.ChipIOAsync()");
            var result = await Device.SetLightAsync(new SetLightRequest("",
                                                                        new LightsStatusClass.LightOperation(LightsStatusClass.LightOperation.PositionEnum.Default,
                                                                                                             LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
                                                                                                             LightsStatusClass.LightOperation.ColourEnum.Default,
                                                                                                             LightsStatusClass.LightOperation.DirectionEnum.None)),
                                                    cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ChipIOAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new SetLightCompletion.PayloadData(result.CompletionCode,
                                                      result.ErrorDescription,
                                                      result.ErrorCode);
        }
    }
}
