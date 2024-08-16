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

namespace XFS4IoTFramework.TextTerminal
{
    public partial class DefineKeysHandler
    {
        private Task<CommandResult<DefineKeysCompletion.PayloadData>> HandleDefineKeys(IDefineKeysEvents events, DefineKeysCommand defineKeys, CancellationToken cancel)
        {
            return Task.FromResult(
                new CommandResult<DefineKeysCompletion.PayloadData>(
                    MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                    $"The XFS form is not supported.")
                );
        }
    }
}
