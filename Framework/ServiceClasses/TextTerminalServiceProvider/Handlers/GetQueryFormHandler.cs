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
using XFS4IoT.Completions;

namespace XFS4IoTFramework.TextTerminal
{
    public partial class GetQueryFormHandler
    {
        private Task<GetQueryFormCompletion.PayloadData> HandleGetQueryForm(IGetQueryFormEvents events, GetQueryFormCommand getQueryForm, CancellationToken cancel)
        {
            return Task.FromResult(new GetQueryFormCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                          $"The XFS form is not supported."));
        }
    }
}
