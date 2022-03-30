/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
    public partial class GetMediaListHandler
    {
        private Task<GetMediaListCompletion.PayloadData> HandleGetMediaList(IGetMediaListEvents events, GetMediaListCommand getMediaList, CancellationToken cancel)
        {
            Dictionary<string, Media> medias = Printer.GetMedias();
            return Task.FromResult(new GetMediaListCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                          null,
                                                                          medias.Count == 0 ? null :
                                                                          new List<string>(medias.Keys)));
        }
    }
}
