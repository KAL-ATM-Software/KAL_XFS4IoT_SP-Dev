/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
using XFS4IoT.Dispenser.Commands;
using XFS4IoT.Dispenser.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Dispenser
{
    public partial class SetMixTableHandler
    {
        private Task<SetMixTableCompletion.PayloadData> HandleSetMixTable(ISetMixTableEvents events, SetMixTableCommand setMixTable, CancellationToken cancel)
        {
            if (setMixTable.Payload.MixNumber is null ||
                setMixTable.Payload.Name is null)
            {
                return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                             "Supplied MixNumber or Name is null."));
            }

            if (setMixTable.Payload.MixHeader is null ||
                setMixTable.Payload.MixHeader.Count == 0)
            {
                return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                             "Supplied MixHeader is empty."));
            }

            if (setMixTable.Payload.MixRows is null ||
                setMixTable.Payload.MixRows.Count == 0)
            {
                return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                             "Supplied MixRows is empty."));
            }

            Dictionary<double, List<MixTable.Table>> mixTables = new();
            foreach (SetMixTableCommand.PayloadData.MixRowsClass row in setMixTable.Payload.MixRows)
            {
                if (row.Amount is null)
                {
                    return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                    "Supplied amount in the MixRows is null."));
                }

                if (setMixTable.Payload.MixHeader.Count != row.Mixture.Count)
                {
                    return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                    "Supplied mixHeader and the mixRow.Mixture size is different."));
                }

                foreach (int? v in row.Mixture)
                {
                    if (v is null)
                    {
                        return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                        "Supplied value in the Mixture is null."));
                    }
                }

                MixTable.Table mixTable = new((double)row.Amount, setMixTable.Payload.MixHeader, row.Mixture);
                if (row.Amount != mixTable.Amount)
                {
                    return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                 "Amount supplied in Cols is different with the "));
                }

                if (mixTables.ContainsKey((double)row.Amount))
                    mixTables[(double)row.Amount].Add(mixTable);
                else
                    mixTables.Add((double)row.Amount, new List<MixTable.Table>() { mixTable });
            }

            Dispenser.AddMix((int)setMixTable.Payload.MixNumber, 
                             new MixTable((int)setMixTable.Payload.MixNumber,
                                          setMixTable.Payload.Name,
                                          setMixTable.Payload.MixHeader,
                                          mixTables));

            return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, null));
        }
    }
}
