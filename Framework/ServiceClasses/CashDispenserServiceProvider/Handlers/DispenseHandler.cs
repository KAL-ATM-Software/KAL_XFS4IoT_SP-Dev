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
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoT.CashDispenser;
using XFS4IoT.CashManagement;
using XFS4IoTFramework.Common;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.CashDispenser
{
    public partial class DispenseHandler
    {
        private async Task<DispenseCompletion.PayloadData> HandleDispense(IDispenseEvents events, DispenseCommand dispense, CancellationToken cancel)
        {
            bool present = false;
            CashDispenserCapabilitiesClass.OutputPositionEnum position = CashDispenserCapabilitiesClass.OutputPositionEnum.Default;
            if (dispense.Payload.Position is not null)
            {
                present = true;
                position = dispense.Payload.Position switch
                {
                    OutputPositionEnum.OutBottom => CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom,
                    OutputPositionEnum.OutCenter => CashDispenserCapabilitiesClass.OutputPositionEnum.Center,
                    OutputPositionEnum.OutDefault => CashDispenserCapabilitiesClass.OutputPositionEnum.Default,
                    OutputPositionEnum.OutFront => CashDispenserCapabilitiesClass.OutputPositionEnum.Front,
                    OutputPositionEnum.OutLeft => CashDispenserCapabilitiesClass.OutputPositionEnum.Left,
                    OutputPositionEnum.OutRear => CashDispenserCapabilitiesClass.OutputPositionEnum.Rear,
                    OutputPositionEnum.OutRight => CashDispenserCapabilitiesClass.OutputPositionEnum.Right,
                    OutputPositionEnum.OutTop => CashDispenserCapabilitiesClass.OutputPositionEnum.Top,
                    _ => CashDispenserCapabilitiesClass.OutputPositionEnum.NotSupported
                };
            }

            if (position == CashDispenserCapabilitiesClass.OutputPositionEnum.NotSupported ||
                !CashDispenser.CashDispenserCapabilities.OutputPositions.HasFlag(position))
            {
                return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                          $"Unsupported position. {position}",
                                                          DispenseCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition);
            }

            // Reset an internal present status
            CashDispenser.LastPresentStatus[position].Status = PresentStatus.PresentStatusEnum.NotPresented;
            CashDispenser.LastPresentStatus[position].LastDenomination = new (new Dictionary<string, double>(), new Dictionary<string, int>());

            if (dispense.Payload.Denomination.Denomination.Currencies is null &&
                dispense.Payload.Denomination.Denomination.Values is null)
            {
                return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode, 
                                                          $"Invalid amounts and values specified. either amount or values dispensing from each cash units required.",
                                                          DispenseCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination);
            }

            if (!string.IsNullOrEmpty(dispense.Payload.Denomination.Mix) &&
                CashDispenser.GetMix(dispense.Payload.Denomination.Mix) is null)
            {
                return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode, 
                                                          $"Invalid MixNumber specified. {dispense.Payload.Denomination.Mix}", 
                                                          DispenseCompletion.PayloadData.ErrorCodeEnum.InvalidMixNumber);
            }

            double totalAmount = 0;
            if (dispense.Payload.Denomination.Denomination.Currencies is not null)
                totalAmount = dispense.Payload.Denomination.Denomination.Currencies.Select(c => c.Value).Sum();

            Denominate denomToDispense = new(dispense.Payload.Denomination.Denomination.Currencies, 
                                             dispense.Payload.Denomination.Denomination.Values, 
                                             Logger);

            ////////////////////////////////////////////////////////////////////////////
            // 1) Check that a given denomination can currently be paid out or test that a given amount matches a given denomination.
            if (string.IsNullOrEmpty(dispense.Payload.Denomination.Mix))
            {
                if (totalAmount == 0 &&
                    (dispense.Payload.Denomination.Denomination.Values is null ||
                     dispense.Payload.Denomination.Denomination.Values.Count == 0))
                {
                    return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                              "No counts specified to dispense items from the cash units.");
                }

                Denominate.DispensableResultEnum Result = denomToDispense.IsDispensable(CashDispenser.CashUnits);
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
                                                                      "Invalid currency specified. ",
                                                                      DispenseCompletion.PayloadData.ErrorCodeEnum.InvalidCurrency);
                        }
                    case Denominate.DispensableResultEnum.InvalidDenomination:
                        {
                            return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                      "Invalid denimination specified. ",
                                                                      DispenseCompletion.PayloadData.ErrorCodeEnum.InvalidDenomination);
                        }
                    default:
                        Contracts.Assert(Result == Denominate.DispensableResultEnum.Good, $"Unexpected result received after an internal IsDispense call. {Result}");
                        break;
                }

                if (denomToDispense.Values is null)
                {
                    return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                              $"Mix failed to denominate. {dispense.Payload.Denomination.Mix}, {denomToDispense.CurrencyAmounts}",
                                                              DispenseCompletion.PayloadData.ErrorCodeEnum.NotDispensable);
                }
            }
            ////////////////////////////////////////////////////////////////////////////
            //  2) Calculate the denomination, given an amount and mix number.
            else if (!string.IsNullOrEmpty(dispense.Payload.Denomination.Mix) &&
                     (dispense.Payload.Denomination.Denomination.Values is null ||
                      dispense.Payload.Denomination.Denomination.Values.Count == 0))
            {
                if (totalAmount == 0)
                {
                    return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                              $"Specified amount is zero to dispense, but number of notes from each cash unit is not specified as well.");
                }

                denomToDispense.Denomination = CashDispenser.GetMix(dispense.Payload.Denomination.Mix).Calculate(denomToDispense.CurrencyAmounts, CashDispenser.CashUnits, CashDispenser.CashDispenserCapabilities.MaxDispenseItems);

                if (denomToDispense.Values is null)
                {
                    return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                              $"Mix failed to denominate. {dispense.Payload.Denomination.Mix}, {totalAmount}",
                                                              DispenseCompletion.PayloadData.ErrorCodeEnum.NotDispensable);
                }
            }
            ////////////////////////////////////////////////////////////////////////////
            //  3) Complete a partially specified denomination for a given amount.
            else if (!string.IsNullOrEmpty(dispense.Payload.Denomination.Mix) &&
                     dispense.Payload.Denomination.Denomination.Values.Count != 0)
            {
                if (totalAmount == 0)
                {
                    return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                              $"Specified amount is zero to dispense, but number of notes from each cash unit is not specified as well.");
                }

                Denomination mixDenom = CashDispenser.GetMix(dispense.Payload.Denomination.Mix).Calculate(denomToDispense.CurrencyAmounts, CashDispenser.CashUnits, CashDispenser.CashDispenserCapabilities.MaxDispenseItems);
                if (mixDenom.Values != denomToDispense.Values)
                {
                    return new DispenseCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode, 
                                                              $"Specified counts each cash unit to be dispensed is different from the result of mix algorithm.",
                                                              DispenseCompletion.PayloadData.ErrorCodeEnum.NotDispensable);
                }
            }
            else
            {
                Contracts.Assert(false, $"Unreachable code.");
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.DispenseAsync()");

            var result = await Device.DispenseAsync(events, 
                                                    new DispenseRequest(denomToDispense.Values,
                                                                        present,
                                                                        position,
                                                                        dispense.Payload.Token), 
                                                    cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.DispenseAsync() -> {result.CompletionCode}, {result.ErrorCode}");


            PresentStatus presentStatus = null;
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
                CashDispenser.LastPresentStatus[position].Status = PresentStatus.PresentStatusEnum.Unknown;
            else
                CashDispenser.LastPresentStatus[position].LastDenomination = denomToDispense.Denomination;

            if (presentStatus is not null)
            {
                CashDispenser.LastPresentStatus[position].Status = (PresentStatus.PresentStatusEnum)presentStatus.Status;

                if (presentStatus.LastDenomination is not null)
                    CashDispenser.LastPresentStatus[position].LastDenomination = presentStatus.LastDenomination;

                CashDispenser.LastPresentStatus[position].Token = presentStatus.Token;
            }

            await CashDispenser.UpdateCashAccounting(result.MovementResult);

            return new DispenseCompletion.PayloadData(result.CompletionCode,
                                                      result.ErrorDescription,
                                                      result.ErrorCode,
                                                      new DenominationClass(denomToDispense.CurrencyAmounts,
                                                                            denomToDispense.Values,
                                                                            dispense.Payload.Denomination.Denomination.CashBox));
        }
    }
}
