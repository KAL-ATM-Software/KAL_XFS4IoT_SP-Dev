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

namespace XFS4IoTFramework.Lights
{
    public partial class SetLightHandler
    {
        private Task<SetLightCompletion.PayloadData> HandleSetLight(ISetLightEvents events, SetLightCommand setLight, CancellationToken cancel)
        {
            return Task.FromResult(new SetLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                      $"SetLight commnad is not supported yet."));
        }
    }
}
