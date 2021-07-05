/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Dispenser.Commands;
using XFS4IoT.Dispenser.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Dispenser
{
    public partial class DenominateHandler
    {
        private Task<DenominateCompletion.PayloadData> HandleDenominate(IDenominateEvents events, DenominateCommand denominate, CancellationToken cancel)
        {
            int mixNumber = 0;
            if (denominate.Payload.MixNumber is not null)
                mixNumber = (int)denominate.Payload.MixNumber;

            if (mixNumber != 0 &&
                Dispenser.GetMix((int)denominate.Payload.MixNumber) is null)
            {
                return Task.FromResult(new DenominateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                                            $"Invalid MixNumber specified. {mixNumber}",
                                                                            DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidMixNumber));
            }

            if (denominate.Payload.Denomination.Currencies is null &&
                denominate.Payload.Denomination.Values is null)
            {
                return Task.FromResult(new DenominateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                            $"Invalid amounts and values specified. either amount or values dispensing from each cash units required.",
                                                                            DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination));
            }

            double totalAmount = 0;
            if (denominate.Payload.Denomination.Currencies is not null)
                totalAmount= denominate.Payload.Denomination.Currencies.Select(c => c.Value).Sum();


            Denominate denomToDispense = new(denominate.Payload.Denomination.Currencies, denominate.Payload.Denomination.Values);

            ////////////////////////////////////////////////////////////////////////////
            // 1) Check that a given denomination can currently be paid out or Test that a given amount matches a given denomination.
            if (mixNumber == 0)
            {
                if (totalAmount == 0 &&
                    (denominate.Payload.Denomination.Values is null ||
                     denominate.Payload.Denomination.Values.Count == 0))
                {
                    return Task.FromResult(new DenominateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                "No counts specified to dispense items from the cash units."));
                }

                Denominate.DispensableResultEnum Result = denomToDispense.IsDispensable(Dispenser.CashUnits, Logger);
                switch (Result)
                {
                    case Denominate.DispensableResultEnum.Good:
                        break;
                    case Denominate.DispensableResultEnum.CashUnitError:
                        {
                            return Task.FromResult(new DenominateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.HardwareError,
                                                                                        $"Invalid Cash Unit specified to dispense.",
                                                                                        DenominateCompletion.PayloadData.ErrorCodeEnum.CashUnitError));
                        }
                    case Denominate.DispensableResultEnum.CashUnitLocked:
                        {
                            return Task.FromResult(new DenominateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.HardwareError,
                                                                                        $"Cash unit is locked.",
                                                                                        DenominateCompletion.PayloadData.ErrorCodeEnum.CashUnitError));
                        }
                    case Denominate.DispensableResultEnum.CashUnitNotEnough:
                        {
                            return Task.FromResult(new DenominateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.HardwareError,
                                                                                        $"Cash unit doesn't have enough notes to dispense.",
                                                                                        DenominateCompletion.PayloadData.ErrorCodeEnum.TooManyItems));
                        }
                    case Denominate.DispensableResultEnum.InvalidCurrency:
                        {
                            return Task.FromResult(new DenominateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                        $"Invalid currency specified. ",
                                                                                        DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidCurrency));
                        }
                    case Denominate.DispensableResultEnum.InvalidDenomination:
                        {
                            return Task.FromResult(new DenominateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                        $"Invalid denomination specified. ",
                                                                                        DenominateCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination));
                        }
                    default:
                        Contracts.Assert(Result == Denominate.DispensableResultEnum.Good, $"Unexpected result received after an internal IsDispense call. {Result}");
                        break;
                }

                if (denomToDispense.Values is null)
                {
                    return Task.FromResult(new DenominateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                                $"Mix failed to denominate. {mixNumber}, {denomToDispense.CurrencyAmounts}",
                                                                                DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable));
                }
            }
            ////////////////////////////////////////////////////////////////////////////
            //  2) Calculate the denomination, given an amount and mix number.
            else if (mixNumber != 0 &&
                     (denominate.Payload.Denomination.Values is null ||
                      denominate.Payload.Denomination.Values.Count == 0))
            {
                if (totalAmount == 0)
                {
                    return Task.FromResult(new DenominateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                $"Specified amount is zero to dispense, but number of notes from each cash unit is not specified as well."));
                }

                denomToDispense.Denomination = Dispenser.GetMix(mixNumber).Calculate(denomToDispense.CurrencyAmounts, Dispenser.CashUnits, Dispenser.CashDispenserCapabilities.MaxDispenseItems, Logger);

                if (denomToDispense.Values is null)
                {
                    return Task.FromResult(new DenominateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                                $"Mix failed to denominate. {mixNumber}, {denomToDispense.CurrencyAmounts}",
                                                                                DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable));
                }
            }
            ////////////////////////////////////////////////////////////////////////////
            //  3) Complete a partially specified denomination for a given amount.
            else if (mixNumber != 0 &&
                     denominate.Payload.Denomination.Values.Count != 0)
            {
                if (totalAmount == 0)
                {
                    return Task.FromResult(new DenominateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                $"Specified amount is zero to dispense, but number of notes from each cash unit is not specified as well."));
                }

                Denomination mixDenom = Dispenser.GetMix(mixNumber).Calculate(denomToDispense.CurrencyAmounts, Dispenser.CashUnits, Dispenser.CashDispenserCapabilities.MaxDispenseItems, Logger);
                if (mixDenom.Values != denomToDispense.Values)
                {
                    return Task.FromResult(new DenominateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                                $"Specified counts each cash unit to be dispensed is different from the result of mix algorithm.",
                                                                                DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable));
                }
            }
            else
            {
                Contracts.Assert(false, $"Unreachable code.");
            }

            return Task.FromResult(new DenominateCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                        null,
                                                                        denomToDispense.Values is null ? DenominateCompletion.PayloadData.ErrorCodeEnum.NotDispensable : null,
                                                                        denomToDispense.CurrencyAmounts,
                                                                        denomToDispense.Values,
                                                                        denominate.Payload.Denomination.CashBox));
        }
    }
}
