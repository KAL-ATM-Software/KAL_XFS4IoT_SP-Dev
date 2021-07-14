/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.TextTerminal.Commands;
using XFS4IoT.TextTerminal.Completions;
using System.Collections.Generic;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.TextTerminal
{
    [CommandHandlerAsync]
    public partial class GetKeyDetailHandler
    {
        private Task<GetKeyDetailCompletion.PayloadData> HandleGetKeyDetail(IGetKeyDetailEvents events, GetKeyDetailCommand getKeyDetail, CancellationToken cancel)
        {
            // Get KeyDetails if they are not cached yet.
            if (TextTerminal.FirstGetKeyDetailCommand)
            {
                TextTerminal.UpdateKeyDetails();
                TextTerminal.FirstGetKeyDetailCommand = false;
            }


            return Task.FromResult(new GetKeyDetailCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                null,
                TextTerminal.SupportedKeys.Keys,
                new() { TextTerminal.SupportedKeys.CommandKeysClass }));
        }

    }
}
