/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
using static XFS4IoT.CashAcceptor.Completions.GetReplenishTargetCompletion.PayloadData;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class GetReplenishTargetHandler
    {
        private Task<CommandResult<GetReplenishTargetCompletion.PayloadData>> HandleGetReplenishTarget(IGetReplenishTargetEvents events, GetReplenishTargetCommand getReplenishTarget, CancellationToken cancel)
        {
            if (CashAcceptor.ReplenishTargets is null ||
                CashAcceptor.ReplenishTargets.Count == 0)
            {
                return Task.FromResult(
                    new CommandResult<GetReplenishTargetCompletion.PayloadData>(
                        MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                        $"The device class doesn't report replenish targets."));
            }

            List<TargetsClass> target = CashAcceptor.ReplenishTargets.Select(t => new GetReplenishTargetCompletion.PayloadData.TargetsClass(t)).ToList();

            return Task.FromResult(
                new CommandResult<GetReplenishTargetCompletion.PayloadData>(
                target.Count != 0 ? new(target) : null,
                MessageHeader.CompletionCodeEnum.Success)
                );
        }
    }
}
