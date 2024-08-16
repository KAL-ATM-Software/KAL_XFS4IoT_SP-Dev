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
        private Task<CommandResult<DenominateCompletion.PayloadData>> HandleDenominate(IDenominateEvents events, DenominateCommand denominate, CancellationToken cancel)
        {
            // Validate command parameters
            if (denominate.Payload.Request is null ||
                denominate.Payload.Request.Denomination is null ||
                (denominate.Payload.Request.Denomination.Service is null &&
                 denominate.Payload.Request.Denomination.App is null))
            {
                return Task.FromResult(
                    new CommandResult<DenominateCompletion.PayloadData>(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"No denomination specified.")
                    );
            }

            if (denominate.Payload.Request.Denomination.Service is not null &&
                denominate.Payload.Request.Denomination.App is not null)
            {
                return Task.FromResult(
                    new CommandResult<DenominateCompletion.PayloadData>(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Invalid parameter specified for both application and service mix. it should be either application or service mix to be specified.")
                    );
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
                    return Task.FromResult(
                        new CommandResult<DenominateCompletion.PayloadData>(
                            new(DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidMixNumber),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode,
                            $"Invalid MixNumber specified. {denominate.Payload.Request.Denomination.Service.Mix}")
                        );
                }

                if (denominate.Payload.Request.Denomination.Service.Currencies is null)
                {
                    return Task.FromResult(
                        new CommandResult<DenominateCompletion.PayloadData>(
                            new(DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode,
                            $"No currency specified for service to get mix result.")
                        );
                }

                currencies = denominate.Payload.Request.Denomination.Service.Currencies;
                if (denominate.Payload.Request.Denomination.Service?.Partial is not null &&
                    denominate.Payload.Request.Denomination.Service?.Partial.Count > 0)
                {
                    counts = denominate.Payload.Request.Denomination.Service.Partial;
                }
            }
            else
            {
                // check application mix
                if (denominate.Payload.Request.Denomination.App.Currencies is null)
                {
                    return Task.FromResult(
                        new CommandResult<DenominateCompletion.PayloadData>(
                            new(DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode,
                            $"No currency specified for service to get mix result.")
                        );
                }

                if (denominate.Payload.Request.Denomination.App.Counts is null ||
                    denominate.Payload.Request.Denomination.App.Counts.Count == 0)
                {
                    return Task.FromResult(
                        new CommandResult<DenominateCompletion.PayloadData>(
                            new(DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode,
                            $"No currency specified for service to get mix result.")
                        );
                }

                currencies = denominate.Payload.Request.Denomination.App.Currencies;
                counts = denominate.Payload.Request.Denomination.App.Counts;
            }

            if (currencies.Select(c => string.IsNullOrEmpty(c.Key) || Regex.IsMatch(c.Key, "^[A-Z]{3}$")).ToList().Count == 0)
            {
                return Task.FromResult(
                    new CommandResult<DenominateCompletion.PayloadData>(
                        new(DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidCurrency),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Invalid currency specified.")
                    );
            }

            double totalAmount = currencies.Select(c => c.Value).Sum();
            if (totalAmount > int.MaxValue)
            {
                return Task.FromResult(
                    new CommandResult<DenominateCompletion.PayloadData>(
                        new(DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Large amount to dispense. {totalAmount}")
                    );
            }

            // Check total amount remaining in the dispenser
            if (totalAmount > 0)
            {
                double maxAmountLeftInDispenser = 0;
                foreach (var unit in Storage.CashUnits)
                {
                    if (!unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut))
                    {
                        continue;
                    }
                    // Check status
                    if (unit.Value.Status != UnitStorageBase.StatusEnum.Good &&
                        unit.Value.Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.Full &&
                        unit.Value.Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.High &&
                        unit.Value.Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.Low &&
                        unit.Value.Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.Healthy)
                    {
                        continue;
                    }
                    // Check counts first
                    if (unit.Value.Unit.Status.Count == 0)
                    {
                        continue;
                    }

                    maxAmountLeftInDispenser += unit.Value.Unit.Status.Count * unit.Value.Unit.Configuration.Value; 
                }

                // We can't dispense requested amount as dispenser has short remaining items.
                if (totalAmount > maxAmountLeftInDispenser)
                {
                    return Task.FromResult(
                        new CommandResult<DenominateCompletion.PayloadData>(
                            new(DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode,
                            $"The total of amount in the dispenser has {maxAmountLeftInDispenser} smaller than requested amount {totalAmount}.")
                        );
                }
            }
            if (counts?.Count > 0 &&
                counts?.Select(c => c.Value).Sum() != 0)
            {
                // Check minimum set of items for service mix or application mix
                foreach (var count in counts)
                {
                    if (!Storage.CashUnits.ContainsKey(count.Key)) 
                    {
                        return Task.FromResult(
                            new CommandResult<DenominateCompletion.PayloadData>(
                                new(DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable),
                                MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                $"Requested count from unrecognized cash unit {count.Key}.")
                            );
                    }

                    if (count.Value > 0)
                    {
                        if (!Storage.CashUnits[count.Key].Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOut))
                        {
                            return Task.FromResult(
                                new CommandResult<DenominateCompletion.PayloadData>(
                                    new(DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable),
                                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                    $"Requested count to be dispensed from invalid cash unit type. {count.Key}:{Storage.CashUnits[count.Key].Unit.Configuration.Types}")
                                );
                        }

                        if (Storage.CashUnits[count.Key].Status != UnitStorageBase.StatusEnum.Good &&
                            Storage.CashUnits[count.Key].Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.Full &&
                            Storage.CashUnits[count.Key].Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.High &&
                            Storage.CashUnits[count.Key].Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.Low &&
                            Storage.CashUnits[count.Key].Unit.Status.ReplenishmentStatus != CashStatusClass.ReplenishmentStatusEnum.Healthy)
                        {
                            return Task.FromResult(
                                new CommandResult<DenominateCompletion.PayloadData>(
                                    new(DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable),
                                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                    $"Requested count to be dispensed from invalid cash unit type. {count.Key}:{Storage.CashUnits[count.Key].Unit.Configuration.Types}")
                                );
                        }

                        if (Storage.CashUnits[count.Key].Unit.Status.Count < count.Value)
                        {
                            return Task.FromResult(
                                new CommandResult<DenominateCompletion.PayloadData>(
                                    new(DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable),
                                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                    $"Requested count to be dispensed from {count.Key}, whih has only {Storage.CashUnits[count.Key].Unit.Status.Count} against requested items{count.Value}.")
                                );
                        }
                    }
                }
            }

            Denominate denomToDispense = new(currencies, counts, Logger);

            if (denominate.Payload.Request.Denomination.App is not null)
            {
                if (denomToDispense.Values is null ||
                    denomToDispense.Values.Count == 0 ||
                    denomToDispense.Values.Select(c => c.Value).Sum() == 0)
                {
                    return Task.FromResult(
                        new CommandResult<DenominateCompletion.PayloadData>(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"No counts specified to dispense items from the cash units."));
                }

                // Check that a given denomination can currently be paid out or Test that a given amount matches a given denomination.
                Denominate.DispensableResultEnum Result = denomToDispense.IsDispensable(Storage.CashUnits, Common.CashDispenserCapabilities.MaxDispenseItems);
                switch (Result)
                {
                    case Denominate.DispensableResultEnum.Good:
                        break;
                    case Denominate.DispensableResultEnum.CashUnitError:
                        {
                            return Task.FromResult(
                                new CommandResult<DenominateCompletion.PayloadData>(
                                    new(DenominateCompletion.PayloadData.ErrorCodeEnum.CashUnitError),
                                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                    $"Invalid Cash Unit specified to dispense.")
                                );
                        }
                    case Denominate.DispensableResultEnum.CashUnitLocked:
                        {
                            return Task.FromResult(
                                new CommandResult<DenominateCompletion.PayloadData>(
                                    new(DenominateCompletion.PayloadData.ErrorCodeEnum.CashUnitError),
                                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                    $"Cash unit is locked.")
                                );
                        }
                    case Denominate.DispensableResultEnum.CashUnitNotEnough:
                        {
                            return Task.FromResult(
                                new CommandResult<DenominateCompletion.PayloadData>(
                                    new(DenominateCompletion.PayloadData.ErrorCodeEnum.TooManyItems),
                                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                    $"Cash unit doesn't have enough notes to dispense.")
                                );
                        }
                    case Denominate.DispensableResultEnum.InvalidCurrency:
                        {
                            return Task.FromResult(
                                new CommandResult<DenominateCompletion.PayloadData>(
                                    new(DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidCurrency),
                                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                    $"Invalid currency specified. ")
                                );
                        }
                    case Denominate.DispensableResultEnum.InvalidDenomination:
                        {
                            return Task.FromResult(
                                new CommandResult<DenominateCompletion.PayloadData>(
                                    new(DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination),
                                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                    $"Invalid denomination specified. ")
                                );
                        }
                    default:
                        Contracts.Assert(Result == Denominate.DispensableResultEnum.Good, $"Unexpected result received after an internal IsDispense call. {Result}");
                        break;
                }
            }
            else
            {
                if (totalAmount == 0)
                {
                    return Task.FromResult(
                        new CommandResult<DenominateCompletion.PayloadData>(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified amount is zero to dispense, but number of notes from each cash unit is not specified as well."));
                }

                if (denomToDispense.Values is null ||
                    denomToDispense.Values.Count == 0 ||
                    denomToDispense.Values.Select(c => c.Value).Sum() == 0)
                {
                    // Calculate the denomination, given an amount and mix number.
                    denomToDispense.Denomination = CashDispenser.GetMix(denominate.Payload.Request.Denomination.Service.Mix).Calculate(denomToDispense.CurrencyAmounts, Storage.CashUnits, Common.CashDispenserCapabilities.MaxDispenseItems);

                    if (denomToDispense.Values is null)
                    {
                        return Task.FromResult(
                            new CommandResult<DenominateCompletion.PayloadData>(
                                new(DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable),
                                MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                $"Mix failed to denominate on service mix. {denominate.Payload.Request.Denomination.Service.Mix}. " + string.Join(", ", denomToDispense.CurrencyAmounts.Select(d => string.Format("{0}{1}{2}", d.Key, ":", d.Value))))
                            );
                    }
                }
                else
                {
                    // Complete a partially specified denomination for a given amount.
                    Denomination mixDenom = CashDispenser.GetMix(denominate.Payload.Request.Denomination.Service.Mix).Calculate(denomToDispense.CurrencyAmounts, Storage.CashUnits, Common.CashDispenserCapabilities.MaxDispenseItems);
                    if (mixDenom.Values is null)
                    {
                        return Task.FromResult(
                            new CommandResult<DenominateCompletion.PayloadData>(
                                new(DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable),
                                MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                $"Specified amount is not dispensable. " + string.Join(", ", denomToDispense.CurrencyAmounts.Select(d => string.Format("{0}{1}{2}", d.Key, ":", d.Value))))
                            );
                    }

                    // denomToDispense contains minimum number of notes required.
                    foreach (var denom in denomToDispense.Values)
                    {
                        if (mixDenom.Values.ContainsKey(denom.Key))
                        {
                            if (denom.Value > mixDenom.Values[denom.Key])
                            {
                                return Task.FromResult(
                                    new CommandResult<DenominateCompletion.PayloadData>(
                                        new(DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable),
                                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                        $"Specified partial count is greater than mix result. Mix: " + string.Join(", ", mixDenom.Values.Select(d => d.Key + ":" + d.Value)) + $", minimum count: {denom.Key}: {denom.Value}")
                                    );
                            }
                        }
                        else
                        {
                            return Task.FromResult(
                                new CommandResult<DenominateCompletion.PayloadData>(
                                    new(DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable),
                                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                    $"Specified partial count is not included in the mix result. Mix: " + string.Join(", ", mixDenom.Values.Select(d => string.Format("{0}{1}{2}", d.Key, ":", d.Value))) + $", minimum count: {denom.Key}: {denom.Value}")
                                );
                        }
                    }

                    // minimum counts each cash units are covered by the mix result.
                    denomToDispense.Denomination = mixDenom;
                }
            }

            if (denomToDispense.Values is null)
            {
                return Task.FromResult(
                    new CommandResult<DenominateCompletion.PayloadData>(
                        new(DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable),
                    CompletionCode: MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    ErrorDescription: $"Requested amount is not dispensable. {totalAmount}")
                    );
            }

            return Task.FromResult(
                new CommandResult<DenominateCompletion.PayloadData>(
                    new(
                        Result: new(
                            Currencies: denomToDispense.CurrencyAmounts,
                            Values: denomToDispense.Values,
                            CashBox: denominate.Payload.Request.Denomination.Service is not null ?
                                     denominate.Payload.Request.Denomination.Service.CashBox :
                                     denominate.Payload.Request.Denomination.App.CashBox)
                        ),
                        CompletionCode: MessageHeader.CompletionCodeEnum.Success)
                );
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
