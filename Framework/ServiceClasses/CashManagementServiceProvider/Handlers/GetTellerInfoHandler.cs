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
    public partial class GetTellerInfoHandler
    {

        private Task<GetTellerInfoCompletion.PayloadData> HandleGetTellerInfo(IGetTellerInfoEvents events, GetTellerInfoCommand getTellerInfo, CancellationToken cancel)
        {
            // NOT SUPPORTED
            return Task.FromResult(new GetTellerInfoCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand, null));
        }
    }
}
