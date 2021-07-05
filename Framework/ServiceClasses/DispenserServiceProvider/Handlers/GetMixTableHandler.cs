/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/


using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Dispenser.Commands;
using XFS4IoT.Dispenser.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Dispenser
{
    public partial class GetMixTableHandler
    {
        private Task<GetMixTableCompletion.PayloadData> HandleGetMixTable(IGetMixTableEvents events, GetMixTableCommand getMixTable, CancellationToken cancel)
        {
            if (getMixTable.Payload.MixNumber is null)
            {
                return Task.FromResult(new GetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                                             $"Invalid mix number supplied. {getMixTable.Payload.MixNumber}",
                                                                             GetMixTableCompletion.PayloadData.ErrorCodeEnum.InvalidMixNumber));
            }

            Mix mix = Dispenser.GetMix((int)getMixTable.Payload.MixNumber);
            if (mix is null ||
                mix.Type != Mix.TypeEnum.Table)
            {
                return Task.FromResult(new GetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                             $"Supplied mix number is not a MixTable. {getMixTable.Payload.MixNumber}",
                                                                             GetMixTableCompletion.PayloadData.ErrorCodeEnum.InvalidMixNumber));
            }

            MixTable mixTable = mix.IsA<MixTable>($"Unexpected mix type. {mix.GetType()}");
            List<GetMixTableCompletion.PayloadData.MixRowsClass> mixRows = new();

            foreach (var table in mixTable.Mixes)
            {
                List<int> cols = new();
                foreach (var col in table.Value)
                    cols.AddRange(col.Counts.Select(v=>v.Value).ToList());

                mixRows.Add(new GetMixTableCompletion.PayloadData.MixRowsClass(table.Key, cols));
            }

            return Task.FromResult(new GetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                         null,
                                                                         null,
                                                                         mixTable.MixNumber,
                                                                         mixTable.Name,
                                                                         mixTable.Values.Select(c => c).ToList(),
                                                                         mixRows));
        }
    }
}
