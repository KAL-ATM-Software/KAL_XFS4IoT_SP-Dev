/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Printer
{
    public partial class ReadFormHandler
    {
        private Task<CommandResult<ReadFormCompletion.PayloadData>> HandleReadForm(IReadFormEvents events, ReadFormCommand readForm, CancellationToken cancel)
        {
            return Task.FromResult(
                new CommandResult<ReadFormCompletion.PayloadData>(
                    MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                    $"Command is not supported.")
                );
        }
    }
}
