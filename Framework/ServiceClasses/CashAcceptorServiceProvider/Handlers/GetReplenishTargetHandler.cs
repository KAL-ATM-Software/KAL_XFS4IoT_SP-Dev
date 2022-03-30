/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class GetReplenishTargetHandler
    {
        private Task<GetReplenishTargetCompletion.PayloadData> HandleGetReplenishTarget(IGetReplenishTargetEvents events, GetReplenishTargetCommand getReplenishTarget, CancellationToken cancel)
        {
            if (CashAcceptor.ReplenishTargets is null ||
                CashAcceptor.ReplenishTargets.Count == 0)
            {
                return Task.FromResult(new GetReplenishTargetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                                    $"The device class doesn't report replenish targets."));
            }

            return Task.FromResult(new GetReplenishTargetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                                string.Empty,
                                                                                CashAcceptor.ReplenishTargets.Select(t => new GetReplenishTargetCompletion.PayloadData.TargetsClass(t)).ToList()));
        }
    }
}
