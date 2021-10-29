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
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.CashDispenser
{
    public partial class GetMixTableHandler
    {
        private Task<GetMixTableCompletion.PayloadData> HandleGetMixTable(IGetMixTableEvents events, GetMixTableCommand getMixTable, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(getMixTable.Payload.Mix))
            {
                return Task.FromResult(new GetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode, 
                                                                             $"No mix supplied.",
                                                                             GetMixTableCompletion.PayloadData.ErrorCodeEnum.InvalidMix));
            }

            Mix mix = CashDispenser.GetMix(getMixTable.Payload.Mix);
            if (mix is null ||
                mix.Type != Mix.TypeEnum.Table)
            {
                return Task.FromResult(new GetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                             $"Supplied mix number is not a MixTable. {getMixTable.Payload.Mix}",
                                                                             GetMixTableCompletion.PayloadData.ErrorCodeEnum.InvalidMix));
            }

            MixTable mixTable = mix.IsA<MixTable>($"Unexpected mix type. {mix.GetType()}");
            List<Dictionary<string, int>> mixRows = new();

            /* Payload for GetMixTable/SetMixTable under discussion in XFS committee
            foreach (var table in mixTable.Mixes)
            {
                Dictionary<string, int> cols = new();
                foreach (var col in table.Value)
                    foreach (var elem in col.Counts)
                        cols.Add(elem.Key.ToString(), elem.Value);
                }

                mixRows.Add(cols);
            }*/

            return Task.FromResult(new GetMixTableCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                         null,
                                                                         null,
                                                                         mixTable.MixNumber,
                                                                         mixTable.Name,
                                                                         mixRows));
        }
    }
}
