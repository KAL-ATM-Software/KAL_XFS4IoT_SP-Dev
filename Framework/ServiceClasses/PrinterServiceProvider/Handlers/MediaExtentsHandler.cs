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

namespace XFS4IoTFramework.Printer
{
    public partial class MediaExtentsHandler
    {
        private Task<CommandResult<MediaExtentsCompletion.PayloadData>> HandleMediaExtents(IMediaExtentsEvents events, MediaExtentsCommand mediaExtents, CancellationToken cancel)
        {
            return Task.FromResult(
                new CommandResult<MediaExtentsCompletion.PayloadData>(
                    MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                    $"Command is not supported.")
                );
        }
    }
}
