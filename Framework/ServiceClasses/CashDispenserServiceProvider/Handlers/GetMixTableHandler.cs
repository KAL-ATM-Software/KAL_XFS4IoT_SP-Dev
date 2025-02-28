/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
        private Task<CommandResult<GetMixTableCompletion.PayloadData>> HandleGetMixTable(IGetMixTableEvents events, GetMixTableCommand getMixTable, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(getMixTable.Payload.Mix))
            {
                return Task.FromResult(
                    new CommandResult<GetMixTableCompletion.PayloadData>(
                        new(GetMixTableCompletion.PayloadData.ErrorCodeEnum.InvalidMix),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"No mix supplied.")
                    );
            }

            Mix mix = CashDispenser.GetMix(getMixTable.Payload.Mix);
            if (mix is null)
            {
                return Task.FromResult
                    (new CommandResult<GetMixTableCompletion.PayloadData>(
                        new(GetMixTableCompletion.PayloadData.ErrorCodeEnum.InvalidMix),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Supplied mix number is not a MixTable. {getMixTable.Payload.Mix}")
                    );
            }

            MixTable mixTable = mix.IsA<MixTable>($"Unexpected mix type. {mix.GetType()}");
            List<XFS4IoT.CashDispenser.MixRowClass> mixRows = null;

            foreach (var tables in mixTable.Mixes)
            {
                foreach (var table in tables.Value)
                {
                    List<XFS4IoT.CashDispenser.MixRowClass.MixClass> breakdown = new();
                    foreach (var list in table.Counts)
                    {
                        breakdown.Add(new (list.Key, list.Value));
                    }
                    (mixRows ??= []).Add(new XFS4IoT.CashDispenser.MixRowClass(table.Amount, breakdown));
                }
            }

            return Task.FromResult(
                new CommandResult<GetMixTableCompletion.PayloadData>(
                    new(
                        MixNumber: mixTable.MixNumber,
                        Name: mixTable.Name,
                        MixRows: mixRows),
                    CompletionCode: MessageHeader.CompletionCodeEnum.Success)
                );
        }
    }
}
