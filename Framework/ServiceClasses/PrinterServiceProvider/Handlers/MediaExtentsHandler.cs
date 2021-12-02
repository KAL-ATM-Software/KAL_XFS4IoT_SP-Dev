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
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Printer
{
    public partial class MediaExtentsHandler
    {
        private Task<MediaExtentsCompletion.PayloadData> HandleMediaExtents(IMediaExtentsEvents events, MediaExtentsCommand mediaExtents, CancellationToken cancel)
        {
            return Task.FromResult(new MediaExtentsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                $"Command is not supported."));
        }
    }
}
