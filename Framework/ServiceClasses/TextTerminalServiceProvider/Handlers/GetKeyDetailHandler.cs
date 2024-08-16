/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoT.TextTerminal;

namespace XFS4IoTFramework.TextTerminal
{
    [CommandHandlerAsync]
    public partial class GetKeyDetailHandler
    {
        private Task<CommandResult<GetKeyDetailCompletion.PayloadData>> HandleGetKeyDetail(IGetKeyDetailEvents events, GetKeyDetailCommand getKeyDetail, CancellationToken cancel)
        {
            // Get KeyDetails if they are not cached yet.
            if (TextTerminal.FirstGetKeyDetailCommand)
            {
                TextTerminal.UpdateKeyDetails();
                TextTerminal.FirstGetKeyDetailCommand = false;
            }

            Dictionary<string, KeyClass> commandKeys = null;
            foreach (var commandKey in TextTerminal.SupportedKeys.CommandKeys)
            {
                (commandKeys ??= []).Add(commandKey.Key, new(Terminate: commandKey.Value));
            }
            return Task.FromResult(
                new CommandResult<GetKeyDetailCompletion.PayloadData>(
                    new(
                        Keys: TextTerminal.SupportedKeys.Keys,
                        CommandKeys: commandKeys),
                    CompletionCode: MessageHeader.CompletionCodeEnum.Success)
                );
        }

    }
}
