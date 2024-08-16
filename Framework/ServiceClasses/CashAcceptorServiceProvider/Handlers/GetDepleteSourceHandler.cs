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
    public partial class GetDepleteSourceHandler
    {

        private Task<CommandResult<GetDepleteSourceCompletion.PayloadData>> HandleGetDepleteSource(IGetDepleteSourceEvents events, GetDepleteSourceCommand getDepleteSource, CancellationToken cancel)
        {
            if (CashAcceptor.DepleteCashUnitSources is null ||
                CashAcceptor.DepleteCashUnitSources.Count == 0)
            {
                Task.FromResult(new CommandResult<GetDepleteSourceCompletion.PayloadData>(MessageHeader.CompletionCodeEnum.UnsupportedCommand));
            }

            if (!Storage.CashUnits.ContainsKey(getDepleteSource.Payload.CashUnitTarget))
            {
                Task.FromResult(new CommandResult<GetDepleteSourceCompletion.PayloadData>
                    (MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Invalid target of storage id specified. {getDepleteSource.Payload.CashUnitTarget}"));
            }

            List<GetDepleteSourceCompletion.PayloadData.DepleteSourcesClass> depleteSources = null;
            foreach (var source in CashAcceptor.DepleteCashUnitSources[getDepleteSource.Payload.CashUnitTarget])
            {
                (depleteSources ??= []).Add(new GetDepleteSourceCompletion.PayloadData.DepleteSourcesClass(source));
            }

            return Task.FromResult(
                new CommandResult<GetDepleteSourceCompletion.PayloadData>(
                    depleteSources is not null ? new(depleteSources) : null,
                    MessageHeader.CompletionCodeEnum.Success)
                );
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
