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
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoT.CashDispenser;
using XFS4IoT.CashManagement;
using XFS4IoTFramework.Common;
using XFS4IoT.Completions;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashDispenser
{
    public partial class DispenseHandler
    {
        private async Task<DispenseCompletion.PayloadData> HandleDispense(IDispenseEvents events, DispenseCommand dispense, CancellationToken cancel)
        {
            //Present should always be false unless the device does not have a stacker.
            //XFS4 currently does not have a bPresent property on dispense, so no way to determine if the application wishes to Dispense and Present currently.
            bool present = false;

            CashManagementCapabilitiesClass.OutputPositionEnum position = CashManagementCapabilitiesClass.OutputPositionEnum.Default;
            if (dispense.Payload.Position is not null)
            {
                position = dispense.Payload.Position switch
                {
                    OutputPositionEnum.OutBottom => CashManagementCapabilitiesClass.OutputPositionEnum.Bottom,
                    OutputPositionEnum.OutCenter => CashManagementCapabilitiesClass.OutputPositionEnum.Center,
                    OutputPositionEnum.OutDefault => CashManagementCapabilitiesClass.OutputPositionEnum.Default,
                    OutputPositionEnum.OutFront => CashManagementCapabilitiesClass.OutputPositionEnum.Front,
                    OutputPositionEnum.OutLeft => CashManagementCapabilitiesClass.OutputPositionEnum.Left,
                    OutputPositionEnum.OutRear => CashManagementCapabilitiesClass.OutputPositionEnum.Rear,
                    OutputPositionEnum.OutRight => CashManagementCapabilitiesClass.OutputPositionEnum.Right,
                    OutputPositionEnum.OutTop => CashManagementCapabilitiesClass.OutputPositionEnum.Top,
                    _ => CashManagementCapabilitiesClass.OutputPositionEnum.NotSupported
                };
            }

            if (position == CashManagementCapabilitiesClass.OutputPositionEnum.NotSupported ||
                !Common.CashDispenserCapabilities.OutputPositions.HasFlag(position))
            {
                return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                          $"Unsupported position. {position}",
                                                          DispenseCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition);
            }

            // Reset an internal present status
            CashDispenser.LastCashDispenserPresentStatus[position].Status = CashDispenserPresentStatus.PresentStatusEnum.NotPresented;
            CashDispenser.LastCashDispenserPresentStatus[position].LastDenomination = new (new Dictionary<string, double>(), new Dictionary<string, int>());

            // Validate command parameters
            if (dispense.Payload is null ||
                dispense.Payload.Denomination is null ||
                (dispense.Payload.Denomination.Denomination.Service is null &&
                 dispense.Payload.Denomination.Denomination.App is null))
            {
                return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"No denomination specified.");
            }

            if (dispense.Payload.Denomination.Denomination.Service is not null &&
                dispense.Payload.Denomination.Denomination.App is not null)
            {
                return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"Invalid parameter specified for both application and service mix. it should be either application or service mix to be specified.");
            }

            // Check parameter either application or service mix.
            Dictionary<string, double> currencies;
            Dictionary<string, int> counts = null;
            if (dispense.Payload.Denomination.Denomination.Service is not null)
            {
                // Check mix valid parameters for service.
                if (!string.IsNullOrEmpty(dispense.Payload.Denomination.Denomination.Service.Mix) &&
                    CashDispenser.GetMix(dispense.Payload.Denomination.Denomination.Service.Mix) is null)
                {
                    return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                              $"Invalid MixNumber specified. {dispense.Payload.Denomination.Denomination.Service.Mix}",
                                                              DispenseCompletion.PayloadData.ErrorCodeEnum.InvalidMixNumber);
                }

                if (dispense.Payload.Denomination.Denomination.Service.Currencies is null)
                {
                    return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                              $"No currency specified for service to get mix result.",
                                                              DispenseCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination);
                }

                currencies = dispense.Payload.Denomination.Denomination.Service.Currencies;

                if (dispense.Payload.Denomination.Denomination.Service?.Partial is not null &&
                    dispense.Payload.Denomination.Denomination.Service?.Partial.Count > 0)
                {
                    counts = dispense.Payload.Denomination.Denomination.Service.Partial;
                }
            }
            else
            {
                // check application mix
                if (dispense.Payload.Denomination.Denomination.App.Currencies is null)
                {
                    return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                              $"No currency specified for service to get mix result.",
                                                              DispenseCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination);
                }

                if (dispense.Payload.Denomination.Denomination.App.Counts is null ||
                    dispense.Payload.Denomination.Denomination.App.Counts.Count == 0)
                {
                    return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                              $"No currency specified for service to get mix result.",
                                                              DispenseCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination);
                }

                currencies = dispense.Payload.Denomination.Denomination.App.Currencies;
                counts = dispense.Payload.Denomination.Denomination.App.Counts;
            }

            if (currencies.Select(c => string.IsNullOrEmpty(c.Key) || Regex.IsMatch(c.Key, "^[A-Z]{3}$")).ToList().Count == 0)
            {
                return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                          $"Invalid currency specified.",
                                                          DispenseCompletion.PayloadData.ErrorCodeEnum.InvalidCurrency);
            }

            // Negative count check
            if (counts is not null)
            {
                foreach (var count in counts)
                {
                    if (count.Value >= 0)
                    {
                        continue;
                    }

                    return new DispenseCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.CommandErrorCode,
                        $"Negative count is specified. Unit:{count.Key}:{count.Value}",
                        DispenseCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination);
                }
            }

            double totalAmount = currencies.Select(c => c.Value).Sum();
            Denominate denomToDispense = new(currencies, counts, Logger);

            if (dispense.Payload.Denomination.Denomination.App is not null)
            {
                if (totalAmount == 0 &&
                    (counts is null ||
                     counts.Count == 0 ||
                     counts.Select(c => c.Value).Sum() == 0))
                {
                    return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"No counts specified to dispense items from the cash units.");
                }

                // Check that a given denomination can currently be paid out or Test that a given amount matches a given denomination.
                Denominate.DispensableResultEnum Result = denomToDispense.IsDispensable(Storage.CashUnits);
                switch (Result)
                {
                    case Denominate.DispensableResultEnum.Good:
                        break;
                    case Denominate.DispensableResultEnum.CashUnitError:
                        {
                            return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                      $"Invalid Cash Unit specified to dispense.",
                                                                      DispenseCompletion.PayloadData.ErrorCodeEnum.CashUnitError);
                        }
                    case Denominate.DispensableResultEnum.CashUnitLocked:
                        {
                            return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                      $"Cash unit is locked.",
                                                                      DispenseCompletion.PayloadData.ErrorCodeEnum.CashUnitError);
                        }
                    case Denominate.DispensableResultEnum.CashUnitNotEnough:
                        {
                            return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                      $"Cash unit doesn't have enough notes to dispense.",
                                                                      DispenseCompletion.PayloadData.ErrorCodeEnum.TooManyItems);
                        }
                    case Denominate.DispensableResultEnum.InvalidCurrency:
                        {
                            return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                      $"Invalid currency specified. ",
                                                                      DispenseCompletion.PayloadData.ErrorCodeEnum.InvalidCurrency);
                        }
                    case Denominate.DispensableResultEnum.InvalidDenomination:
                        {
                            return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                      $"Invalid denomination specified. ",
                                                                      DispenseCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination);
                        }
                    default:
                        Contracts.Assert(Result == Denominate.DispensableResultEnum.Good, $"Unexpected result received after an internal IsDispense call. {Result}");
                        break;
                }

                if (denomToDispense.Values is null)
                {
                    return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                              $"Mix failed to denominate on application mix. {denomToDispense.CurrencyAmounts}",
                                                              DispenseCompletion.PayloadData.ErrorCodeEnum.NotDispensable);
                }
            }
            else
            {
                if (totalAmount == 0)
                {
                    return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Specified amount is zero to dispense, but number of notes from each cash unit is not specified as well.");
                }

                if (counts is null ||
                    counts.Count == 0 ||
                    counts.Select(c => c.Value).Sum() == 0)
                {
                    // Calculate the denomination, given an amount and mix number.
                    denomToDispense.Denomination = CashDispenser.GetMix(dispense.Payload.Denomination.Denomination.Service.Mix).Calculate(denomToDispense.CurrencyAmounts, Storage.CashUnits, Common.CashDispenserCapabilities.MaxDispenseItems);

                    if (denomToDispense.Values is null)
                    {
                        return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                  $"Mix failed to denominate on service mix. {dispense.Payload.Denomination.Denomination.Service.Mix}, {denomToDispense.CurrencyAmounts}",
                                                                  DispenseCompletion.PayloadData.ErrorCodeEnum.NotDispensable);
                    }
                }
                else
                {
                    // Complete a partially specified denomination for a given amount.
                    Denomination mixDenom = CashDispenser.GetMix(dispense.Payload.Denomination.Denomination.Service.Mix).Calculate(denomToDispense.CurrencyAmounts, Storage.CashUnits, Common.CashDispenserCapabilities.MaxDispenseItems);
                    // denomToDispense contains minimum number of notes required.
                    foreach (var denom in denomToDispense.Values)
                    {
                        if (mixDenom.Values.ContainsKey(denom.Key))
                        {
                            if (denom.Value > mixDenom.Values[denom.Key])
                            {
                                return new DispenseCompletion.PayloadData(
                                    MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                    $"Specified partial count is greater than mix result. Mix: " + string.Join(", ", mixDenom.Values.Select(d => d.Key + ":" + d.Value)) + $", minimum count: {denom.Key}: {denom.Value}",
                                    DispenseCompletion.PayloadData.ErrorCodeEnum.NotDispensable);
                            }
                        }
                        else
                        {
                            return new DispenseCompletion.PayloadData(
                                    MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                    $"Specified partial count is not included in the mix result. Mix: " + string.Join(", ", mixDenom.Values.Select(d => string.Format("{0}{1}{2}", d.Key, ":", d.Value))) + $", minimum count: {denom.Key}: {denom.Value}",
                                    DispenseCompletion.PayloadData.ErrorCodeEnum.NotDispensable);
                        }
                    }

                    // minimum counts each cash units are covered by the mix result.
                    denomToDispense.Denomination = mixDenom;
                }
            }

            if (denomToDispense.Values is null)
            {
                return new DispenseCompletion.PayloadData(CompletionCode: MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                          ErrorDescription: $"Requested amount is not dispensable. {totalAmount}",
                                                          ErrorCode: DispenseCompletion.PayloadData.ErrorCodeEnum.NotDispensable);
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.DispenseAsync()");

            var result = await Device.DispenseAsync(new DispenseCommandEvents(Storage, events), 
                                                    new DispenseRequest(denomToDispense.Values,
                                                                        present,
                                                                        position,
                                                                        dispense.Payload.Token), 
                                                    cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.DispenseAsync() -> {result.CompletionCode}, {result.ErrorCode}");


            CashDispenserPresentStatus presentStatus = null;
            try
            {
                Logger.Log(Constants.DeviceClass, "CashDispenserDev.GetPresentStatus()");

                presentStatus = Device.GetPresentStatus(position);

                Logger.Log(Constants.DeviceClass, $"CashDispenserDev.GetPresentStatus() -> {presentStatus}");
            }
            catch (NotImplementedException)
            {
                Logger.Log(Constants.DeviceClass, $"CashDispenserDev.GetPresentStatus() -> Not implemented");
            }
            catch (Exception)
            {
                throw;
            }

            // Update an internal present status
            if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                CashDispenser.LastCashDispenserPresentStatus[position].Status = CashDispenserPresentStatus.PresentStatusEnum.Unknown;
            else
                CashDispenser.LastCashDispenserPresentStatus[position].LastDenomination = denomToDispense.Denomination;

            if (presentStatus is not null)
            {
                CashDispenser.LastCashDispenserPresentStatus[position].Status = (CashDispenserPresentStatus.PresentStatusEnum)presentStatus.Status;

                if (presentStatus.LastDenomination is not null)
                    CashDispenser.LastCashDispenserPresentStatus[position].LastDenomination = new(presentStatus.LastDenomination.CurrencyAmounts, presentStatus.LastDenomination.Values);

                CashDispenser.LastCashDispenserPresentStatus[position].DispenseToken = presentStatus.DispenseToken;
            }

            CashDispenser.StoreCashDispenserPresentStatus();

            await Storage.UpdateCashAccounting(result.MovementResult);

            if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
            {
                return new DispenseCompletion.PayloadData(
                    CompletionCode: result.CompletionCode,
                    ErrorDescription: result.ErrorDescription,
                    ErrorCode: result.ErrorCode);
            }

            return new DispenseCompletion.PayloadData(
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription,
                ErrorCode: result.ErrorCode,
                Denomination: denomToDispense.Values is null ?
                              null :
                              new(Currencies: denomToDispense.CurrencyAmounts,
                                  Values: denomToDispense.Values,
                                  CashBox: dispense.Payload.Denomination.Denomination.Service is not null ?
                                    dispense.Payload.Denomination.Denomination.Service.CashBox :
                                    dispense.Payload.Denomination.Denomination.App.CashBox),
                                  Bunches: result.Bunches >= 1 ?
                                    result.Bunches.ToString() :
                                    "unknown");
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
