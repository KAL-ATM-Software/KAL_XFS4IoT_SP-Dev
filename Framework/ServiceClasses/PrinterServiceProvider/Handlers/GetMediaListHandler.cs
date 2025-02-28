/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Printer
{
    [CommandHandlerAsync]
    public partial class GetMediaListHandler
    {
        private Task<CommandResult<GetMediaListCompletion.PayloadData>> HandleGetMediaList(IGetMediaListEvents events, GetMediaListCommand getMediaList, CancellationToken cancel)
        {
            Dictionary<string, Media> medias = Printer.GetMedias();
            return Task.FromResult(
                new CommandResult<GetMediaListCompletion.PayloadData>(
                    medias.Count == 0 ? null : new(new List<string>(medias.Keys)),
                    MessageHeader.CompletionCodeEnum.Success)
                );
        }
    }
}
