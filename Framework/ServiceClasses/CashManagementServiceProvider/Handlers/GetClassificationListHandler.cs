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
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.CashManagement
{
    public partial class GetClassificationListHandler
    {
        private Task<GetClassificationListCompletion.PayloadData> HandleGetClassificationList(IGetClassificationListEvents events, GetClassificationListCommand getClassificationList, CancellationToken cancel)
        {
            // NOT SUPPORTED
            return Task.FromResult(new GetClassificationListCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand, null));
        }
    }
}
