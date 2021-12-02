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
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashDispenser
{
    public partial class SetMixTableHandler
    {
        private Task<SetMixTableCompletion.PayloadData> HandleSetMixTable(ISetMixTableEvents events, SetMixTableCommand setMixTable, CancellationToken cancel)
        {
            if (setMixTable.Payload.MixNumber is null ||
                setMixTable.Payload.Name is null)
            {
                return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                             $"Supplied MixNumber or Name is null."));
            }

            if (setMixTable.Payload.MixRows is null ||
                setMixTable.Payload.MixRows.Count == 0)
            {
                return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                             $"Supplied MixRows is empty."));
            }

            foreach (var mix in CashDispenser.GetMixAlgorithms())
            {
                if (typeof(MixTable) != mix.GetType())
                    continue;

                if (((MixTable)mix.Value).MixNumber == setMixTable.Payload.MixNumber)
                {
                    return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                                 $"Specified mix number is already being used.",
                                                                                 SetMixTableCompletion.PayloadData.ErrorCodeEnum.InvalidMixNumber));
                }
            }

            Dictionary<double, List<MixTable.Table>> mixTables = new();
            foreach (var row in setMixTable.Payload.MixRows)
            {
                if (row.Amount is null)
                {
                    return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                 $"Supplied amount in the MixRows is null."));
                }

                foreach (var v in row.Mix)
                {
                    if (v is null)
                    {
                        return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                     $"Supplied value in the Mix is null."));
                    }
                    if (v.Value is null)
                    {
                        return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                     $"Supplied value in the Mix value is null."));
                    }
                    if (v.Count is null)
                    {
                        return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                     $"Supplied value in the Mix count is null."));
                    }
                }

                MixTable.Table mixTable = new((double)row.Amount, 
                                               new List<double>(from v in row.Mix
                                                                select (double)v.Value),
                                               new List<int>(from v in row.Mix
                                                             select (int)v.Count));

                if (mixTables.ContainsKey((double)row.Amount))
                    mixTables[(double)row.Amount].Add(mixTable);
                else
                    mixTables.Add((double)row.Amount, new List<MixTable.Table>() { mixTable });
            }

            CashDispenser.AddMix("mixTable" + setMixTable.Payload.MixNumber.ToString(), 
                                 new MixTable((int)setMixTable.Payload.MixNumber,
                                              setMixTable.Payload.Name,
                                              mixTables, 
                                              Logger));

            return Task.FromResult(new SetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, null));
        }
    }
}
