/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoT.CashDispenser;
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.CashDispenser
{
    public partial class GetMixTypesHandler
    {
        private Task<CommandResult<GetMixTypesCompletion.PayloadData>> HandleGetMixTypes(IGetMixTypesEvents events, GetMixTypesCommand getMixTypes, CancellationToken cancel)
        {
            Dictionary<string, MixClass> mixes = null;

            Dictionary<string, Mix> mixAlgorithms = CashDispenser.GetMixAlgorithms();
            foreach (var mixAlgorithm in mixAlgorithms)
            {
                MixClass.TypeEnum type = mixAlgorithm.Value.Type switch
                {
                    Mix.TypeEnum.Algorithm => MixClass.TypeEnum.Algorithm,
                    Mix.TypeEnum.Table => MixClass.TypeEnum.Table,
                    _ => MixClass.TypeEnum.Individual
                };

                string algorithm = string.Empty;
                if (mixAlgorithm.Value.Type == Mix.TypeEnum.Algorithm)
                {
                    if (mixAlgorithm.Value.Algorithm == Mix.AlgorithmEnum.equalEmptying ||
                        mixAlgorithm.Value.Algorithm == Mix.AlgorithmEnum.maxCashUnits ||
                        mixAlgorithm.Value.Algorithm == Mix.AlgorithmEnum.minimumBills)
                    {
                        algorithm = mixAlgorithm.Value.Algorithm.ToString();
                    }
                    else if (mixAlgorithm.Value.Algorithm == Mix.AlgorithmEnum.VendorSpecific)
                    {
                        algorithm = mixAlgorithm.Value.Name;
                    }
                }

                (mixes ??= []).Add(
                    mixAlgorithm.Key, 
                    new MixClass(
                        type,
                        algorithm,
                        mixAlgorithm.Value.Name)
                    );
            }

            return Task.FromResult(
                new CommandResult<GetMixTypesCompletion.PayloadData>(
                    mixes is not null ? new(Mixes: mixes) : null,
                    MessageHeader.CompletionCodeEnum.Success)
                );
        }
    }
}
