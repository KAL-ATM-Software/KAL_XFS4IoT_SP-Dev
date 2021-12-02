/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    public partial class GetQueryMediaHandler
    {
        private Task<GetQueryMediaCompletion.PayloadData> HandleGetQueryMedia(IGetQueryMediaEvents events, GetQueryMediaCommand getQueryMedia, CancellationToken cancel)
        {
            Dictionary<string, Media> medias = Printer.GetMedias();
            if (!medias.ContainsKey(getQueryMedia.Payload.MediaName))
            {
                return Task.FromResult(new GetQueryMediaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                               $"Specified form doesn't exist. {getQueryMedia.Payload.MediaName}",
                                                                               GetQueryMediaCompletion.PayloadData.ErrorCodeEnum.MediaNotFound));
            }
            return Task.FromResult((medias[getQueryMedia.Payload.MediaName]).QueryMedia());
        }
    }
}
