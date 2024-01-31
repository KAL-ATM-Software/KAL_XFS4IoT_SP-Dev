/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashDispenser;
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashDispenser
{
    public partial class DenominateHandler
    {
        private Task<DenominateCompletion.PayloadData> HandleDenominate(IDenominateEvents events, DenominateCommand denominate, CancellationToken cancel)
        {
            // Validate command parameters
            if (denominate.Payload.Request is null ||
                denominate.Payload.Request.Denomination is null ||
                (denominate.Payload.Request.Denomination.Service is null &&
                 denominate.Payload.Request.Denomination.App is null))
            {
                return Task.FromResult(new DenominateCompletion.PayloadData(
                    MessagePayload.CompletionCodeEnum.InvalidData,
                    $"No denomination specified."));
            }

            if (denominate.Payload.Request.Denomination.Service is not null &&
                denominate.Payload.Request.Denomination.App is not null)
            {
                return Task.FromResult(new DenominateCompletion.PayloadData(
                    MessagePayload.CompletionCodeEnum.InvalidData,
                    $"Invalid parameter specified for both application and service mix. it should be either application or service mix to be specified."));
            }

            // Check parameter either application or service mix.
            Dictionary<string, double> currencies;
            Dictionary<string, int> counts = null;
            if (denominate.Payload.Request.Denomination.Service is not null)
            {
                // Check mix valid parameters for service.
                if (!string.IsNullOrEmpty(denominate.Payload.Request.Denomination.Service.Mix) &&
                    CashDispenser.GetMix(denominate.Payload.Request.Denomination.Service.Mix) is null)
                {
                    return Task.FromResult(new DenominateCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.CommandErrorCode,
                        $"Invalid MixNumber specified. {denominate.Payload.Request.Denomination.Service.Mix}",
                        DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidMixNumber));
                }

                if (denominate.Payload.Request.Denomination.Service.Currencies is null)
                {
                    return Task.FromResult(new DenominateCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.CommandErrorCode,
                        $"No currency specified for service to get mix result.",
                        DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination));
                }

                currencies = denominate.Payload.Request.Denomination.Service.Currencies;
            }
            else
            {
                // check application mix
                if (denominate.Payload.Request.Denomination.App.Currencies is null)
                {
                    return Task.FromResult(new DenominateCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.CommandErrorCode,
                        $"No currency specified for service to get mix result.",
                        DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination));
                }

                if (denominate.Payload.Request.Denomination.App.Counts is null ||
                    denominate.Payload.Request.Denomination.App.Counts.Count == 0)
                {
                    return Task.FromResult(new DenominateCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.CommandErrorCode,
                        $"No currency specified for service to get mix result.",
                        DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination));
                }

                currencies = denominate.Payload.Request.Denomination.App.Currencies;
                counts = denominate.Payload.Request.Denomination.App.Counts;
            }

            if (currencies.Select(c => string.IsNullOrEmpty(c.Key) || Regex.IsMatch(c.Key, "^[A-Z]{3}$")).ToList().Count == 0)
            {
                return Task.FromResult(new DenominateCompletion.PayloadData(
                    MessagePayload.CompletionCodeEnum.CommandErrorCode,
                    $"Invalid currency specified.",
                    DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidCurrency));
            }

            double totalAmount = currencies.Select(c => c.Value).Sum();
            Denominate denomToDispense = new(currencies, counts, Logger);

            if (denominate.Payload.Request.Denomination.App is not null)
            {
                if (totalAmount == 0 &&
                    (counts is null ||
                     counts.Count == 0 ||
                     counts.Select(c => c.Value).Sum() == 0))
                {
                    return Task.FromResult(new DenominateCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.InvalidData,
                        $"No counts specified to dispense items from the cash units."));
                }

                // Check that a given denomination can currently be paid out or Test that a given amount matches a given denomination.
                Denominate.DispensableResultEnum Result = denomToDispense.IsDispensable(Storage.CashUnits);
                switch (Result)
                {
                    case Denominate.DispensableResultEnum.Good:
                        break;
                    case Denominate.DispensableResultEnum.CashUnitError:
                        {
                            return Task.FromResult(new DenominateCompletion.PayloadData(
                                MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                $"Invalid Cash Unit specified to dispense.",
                                DenominateCompletion.PayloadData.ErrorCodeEnum.CashUnitError));
                        }
                    case Denominate.DispensableResultEnum.CashUnitLocked:
                        {
                            return Task.FromResult(new DenominateCompletion.PayloadData(
                                MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                $"Cash unit is locked.",
                                DenominateCompletion.PayloadData.ErrorCodeEnum.CashUnitError));
                        }
                    case Denominate.DispensableResultEnum.CashUnitNotEnough:
                        {
                            return Task.FromResult(new DenominateCompletion.PayloadData(
                                MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                $"Cash unit doesn't have enough notes to dispense.",
                                DenominateCompletion.PayloadData.ErrorCodeEnum.TooManyItems));
                        }
                    case Denominate.DispensableResultEnum.InvalidCurrency:
                        {
                            return Task.FromResult(new DenominateCompletion.PayloadData(
                                MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                $"Invalid currency specified. ",
                                DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidCurrency));
                        }
                    case Denominate.DispensableResultEnum.InvalidDenomination:
                        {
                            return Task.FromResult(new DenominateCompletion.PayloadData(
                                MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                $"Invalid denomination specified. ",
                                DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination));
                        }
                    default:
                        Contracts.Assert(Result == Denominate.DispensableResultEnum.Good, $"Unexpected result received after an internal IsDispense call. {Result}");
                        break;
                }

                if (denomToDispense.Values is null)
                {
                    return Task.FromResult(new DenominateCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.CommandErrorCode,
                        $"Mix failed to denominate on application mix. {denomToDispense.CurrencyAmounts}",
                        DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable));
                }
            }
            else
            {
                if (totalAmount == 0)
                {
                    return Task.FromResult(new DenominateCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.InvalidData,
                        $"Specified amount is zero to dispense, but number of notes from each cash unit is not specified as well."));
                }

                if (counts is null ||
                    counts.Count == 0 ||
                    counts.Select(c => c.Value).Sum() == 0)
                {
                    // Calculate the denomination, given an amount and mix number.
                    denomToDispense.Denomination = CashDispenser.GetMix(denominate.Payload.Request.Denomination.Service.Mix).Calculate(denomToDispense.CurrencyAmounts, Storage.CashUnits, Common.CashDispenserCapabilities.MaxDispenseItems);

                    if (denomToDispense.Values is null)
                    {
                        return Task.FromResult(new DenominateCompletion.PayloadData(
                            MessagePayload.CompletionCodeEnum.CommandErrorCode,
                            $"Mix failed to denominate on service mix. {denominate.Payload.Request.Denomination.Service.Mix}, {denomToDispense.CurrencyAmounts}",
                            DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable));
                    }
                }
                else
                {
                    // Complete a partially specified denomination for a given amount.
                    Denomination mixDenom = CashDispenser.GetMix(denominate.Payload.Request.Denomination.Service.Mix).Calculate(denomToDispense.CurrencyAmounts, Storage.CashUnits, Common.CashDispenserCapabilities.MaxDispenseItems);
                    if (!mixDenom.Values.OrderBy((denom) => denom.Key).SequenceEqual(denomToDispense.Values.OrderBy((denom) => denom.Key)))
                    {
                        return Task.FromResult(new DenominateCompletion.PayloadData(
                            MessagePayload.CompletionCodeEnum.CommandErrorCode,
                            $"Specified counts each cash unit to be dispensed is different from the result of mix algorithm. internal mix result " + string.Join(", ", mixDenom.Values.Select(d => d.Key + ":" + d.Value)),
                            DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable));
                    }
                }
            }

            if (denomToDispense.Values is null)
            {
                return Task.FromResult(new DenominateCompletion.PayloadData(CompletionCode: MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                            ErrorDescription: $"Requested amount is not dispensable. {totalAmount}",
                                                                            ErrorCode: DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable));
            }

            return Task.FromResult(new DenominateCompletion.PayloadData(
                CompletionCode: MessagePayload.CompletionCodeEnum.Success,
                ErrorDescription: null,
                ErrorCode: null,
                Result: new(
                    Currencies: denomToDispense.CurrencyAmounts,
                    Values: denomToDispense.Values,
                    CashBox: denominate.Payload.Request.Denomination.Service is not null ?
                             denominate.Payload.Request.Denomination.Service.CashBox :
                             denominate.Payload.Request.Denomination.App.CashBox)
                ));
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
