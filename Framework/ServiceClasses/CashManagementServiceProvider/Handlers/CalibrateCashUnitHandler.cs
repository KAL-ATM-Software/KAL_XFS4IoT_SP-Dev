/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.CashManagement
{
    public partial class CalibrateCashUnitHandler
    {
        private async Task<CalibrateCashUnitCompletion.PayloadData> HandleCalibrateCashUnit(ICalibrateCashUnitEvents events, CalibrateCashUnitCommand calibrateCashUnit, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(calibrateCashUnit.Payload.Cashunit) ||
                !CashManagement.CashUnits.ContainsKey(calibrateCashUnit.Payload.Cashunit))
            {
                return new CalibrateCashUnitCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"The CashUnit property is specified null or empty. the application should specify items to be dispensed from which unit.");
            }

            ItemPosition itemPosition = null;

            if (calibrateCashUnit.Payload.Position is null ||
                (string.IsNullOrEmpty(calibrateCashUnit.Payload.Position.Cashunit) &&
                 calibrateCashUnit.Payload.Position.RetractArea is null &&
                 calibrateCashUnit.Payload.Position.OutputPosition is null))
            {
                // Default position to move items into is decided by the device 
            }
            else
            {
                if (!string.IsNullOrEmpty(calibrateCashUnit.Payload.Position.Cashunit) &&
                    !CashManagement.CashUnits.ContainsKey(calibrateCashUnit.Payload.Position.Cashunit))
                {
                    return new CalibrateCashUnitCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                       $"Specified CashUnit location is unknown.");
                }

                if (!string.IsNullOrEmpty(calibrateCashUnit.Payload.Position.Cashunit))
                {
                    itemPosition = new ItemPosition(calibrateCashUnit.Payload.Position.Cashunit);
                }
                else
                {
                    if (calibrateCashUnit.Payload.Position.RetractArea is not null &&
                        calibrateCashUnit.Payload.Position.OutputPosition is not null)
                    {
                        return new CalibrateCashUnitCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                           $"Both RetractArea and OutputPosition properties were specified. The Service Provider doesn't know where the items should be moved to.");
                    }

                    if (calibrateCashUnit.Payload.Position.RetractArea is not null)
                    {
                        int? index = null;

                        var retract = calibrateCashUnit.Payload.Position.RetractArea.IsA<CalibrateCashUnitCommand.PayloadData.PositionClass.RetractAreaClass>("RetractArea object must be the RetractAreaClass.");

                        CashDispenserCapabilitiesClass.RetractAreaEnum retractArea = CashDispenserCapabilitiesClass.RetractAreaEnum.Default;
                        if (retract.RetractArea is not null)
                        {
                            retractArea = retract.RetractArea switch
                            {
                                CalibrateCashUnitCommand.PayloadData.PositionClass.RetractAreaClass.RetractAreaEnum.ItemCassette => CashDispenserCapabilitiesClass.RetractAreaEnum.ItemCassette,
                                CalibrateCashUnitCommand.PayloadData.PositionClass.RetractAreaClass.RetractAreaEnum.Reject => CashDispenserCapabilitiesClass.RetractAreaEnum.Reject,
                                CalibrateCashUnitCommand.PayloadData.PositionClass.RetractAreaClass.RetractAreaEnum.Retract => CashDispenserCapabilitiesClass.RetractAreaEnum.Retract,
                                CalibrateCashUnitCommand.PayloadData.PositionClass.RetractAreaClass.RetractAreaEnum.Stacker => CashDispenserCapabilitiesClass.RetractAreaEnum.Stacker,
                                CalibrateCashUnitCommand.PayloadData.PositionClass.RetractAreaClass.RetractAreaEnum.Transport => CashDispenserCapabilitiesClass.RetractAreaEnum.Transport,
                                _ => CashDispenserCapabilitiesClass.RetractAreaEnum.Default
                            };

                            if (!CashManagement.CashDispenserCapabilities.RetractAreas[retractArea])
                            {
                                return new CalibrateCashUnitCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                                   $"Specified unsupported retract area. {retractArea}",
                                                                                   CalibrateCashUnitCompletion.PayloadData.ErrorCodeEnum.InvalidCashUnit);
                            }
                        }

                        if (retractArea == CashDispenserCapabilitiesClass.RetractAreaEnum.Retract &&
                            retract.Index is null)
                        {
                            return new CalibrateCashUnitCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                               "Index property is set to null where the retract area is specified to retract position.");
                        }

                        itemPosition = new ItemPosition(new Retract(retractArea, index));
                    }
                    else
                    {
                        CashDispenserCapabilitiesClass.OutputPositionEnum position = calibrateCashUnit.Payload.Position.OutputPosition switch
                        {
                            CalibrateCashUnitCommand.PayloadData.PositionClass.OutputPositionEnum.Bottom => CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom,
                            CalibrateCashUnitCommand.PayloadData.PositionClass.OutputPositionEnum.Center => CashDispenserCapabilitiesClass.OutputPositionEnum.Center,
                            CalibrateCashUnitCommand.PayloadData.PositionClass.OutputPositionEnum.Front => CashDispenserCapabilitiesClass.OutputPositionEnum.Front,
                            CalibrateCashUnitCommand.PayloadData.PositionClass.OutputPositionEnum.Left => CashDispenserCapabilitiesClass.OutputPositionEnum.Left,
                            CalibrateCashUnitCommand.PayloadData.PositionClass.OutputPositionEnum.Rear => CashDispenserCapabilitiesClass.OutputPositionEnum.Rear,
                            CalibrateCashUnitCommand.PayloadData.PositionClass.OutputPositionEnum.Right => CashDispenserCapabilitiesClass.OutputPositionEnum.Right,
                            CalibrateCashUnitCommand.PayloadData.PositionClass.OutputPositionEnum.Top => CashDispenserCapabilitiesClass.OutputPositionEnum.Top,
                            _ => CashDispenserCapabilitiesClass.OutputPositionEnum.Default,

                        };

                        if (!CashManagement.CashDispenserCapabilities.OutputPositons[position])
                        {
                            return new CalibrateCashUnitCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                               $"Specified unsupported output position. {position}");
                        }

                        itemPosition = new ItemPosition(position);
                    }
                }
            }

            Logger.Log(Constants.DeviceClass, "CashManagementDev.CalibrateCashUnitAsync()");

            var result = await Device.CalibrateCashUnitAsync(events, new CalibrateCashUnitRequest(calibrateCashUnit.Payload.Cashunit,
                                                                                                  calibrateCashUnit.Payload.NumOfBills is null ? 0 : (int)calibrateCashUnit.Payload.NumOfBills,
                                                                                                  itemPosition), cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.CalibrateCashUnitAsync() -> {result.CompletionCode}, {result.ErrorCode}");


            CashManagement.UpdateCashUnitAccounting(result.MovementResult);

            if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
            {
                return new CalibrateCashUnitCompletion.PayloadData(result.CompletionCode,
                                                                   result.ErrorDescription,
                                                                   result.ErrorCode);
            }

            CalibrateCashUnitCompletion.PayloadData.PositionClass resItemPosition = null;
            if (result.Position is not null)
            {
                CalibrateCashUnitCompletion.PayloadData.PositionClass.RetractAreaClass retractArea = null;
                if (result.Position.RetractArea is not null)
                {
                    retractArea = new CalibrateCashUnitCompletion.PayloadData.PositionClass.RetractAreaClass(
                                        null,
                                        result.Position.RetractArea.RetractArea switch
                                        {
                                            CashDispenserCapabilitiesClass.RetractAreaEnum.ItemCassette => CalibrateCashUnitCompletion.PayloadData.PositionClass.RetractAreaClass.RetractAreaEnum.ItemCassette,
                                            CashDispenserCapabilitiesClass.RetractAreaEnum.Reject => CalibrateCashUnitCompletion.PayloadData.PositionClass.RetractAreaClass.RetractAreaEnum.Reject,
                                            CashDispenserCapabilitiesClass.RetractAreaEnum.Stacker => CalibrateCashUnitCompletion.PayloadData.PositionClass.RetractAreaClass.RetractAreaEnum.Stacker,
                                            CashDispenserCapabilitiesClass.RetractAreaEnum.Transport => CalibrateCashUnitCompletion.PayloadData.PositionClass.RetractAreaClass.RetractAreaEnum.Transport,
                                            _ => CalibrateCashUnitCompletion.PayloadData.PositionClass.RetractAreaClass.RetractAreaEnum.Retract,
                                        },
                                        result.Position.RetractArea.Index);
                }
                resItemPosition = new CalibrateCashUnitCompletion.PayloadData.PositionClass(
                                        result.Position.CashUnit,
                                        retractArea,
                                        result.Position.OutputPosition is not null ? result.Position.OutputPosition switch
                                        {
                                            CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom => CalibrateCashUnitCompletion.PayloadData.PositionClass.OutputPositionEnum.Bottom,
                                            CashDispenserCapabilitiesClass.OutputPositionEnum.Center => CalibrateCashUnitCompletion.PayloadData.PositionClass.OutputPositionEnum.Center,
                                            CashDispenserCapabilitiesClass.OutputPositionEnum.Front => CalibrateCashUnitCompletion.PayloadData.PositionClass.OutputPositionEnum.Front,
                                            CashDispenserCapabilitiesClass.OutputPositionEnum.Left => CalibrateCashUnitCompletion.PayloadData.PositionClass.OutputPositionEnum.Left,
                                            CashDispenserCapabilitiesClass.OutputPositionEnum.Rear => CalibrateCashUnitCompletion.PayloadData.PositionClass.OutputPositionEnum.Rear,
                                            CashDispenserCapabilitiesClass.OutputPositionEnum.Right => CalibrateCashUnitCompletion.PayloadData.PositionClass.OutputPositionEnum.Right,
                                            CashDispenserCapabilitiesClass.OutputPositionEnum.Top => CalibrateCashUnitCompletion.PayloadData.PositionClass.OutputPositionEnum.Top,
                                            _ => CalibrateCashUnitCompletion.PayloadData.PositionClass.OutputPositionEnum.Default
                                        } : null);
            }

            int total = calibrateCashUnit.Payload.NumOfBills ?? 0;
            if (result.MovementResult is not null)
            {
                total = 0;
                foreach (var movement in result.MovementResult)
                    total += movement.Value.DispensedCount ?? 0;
            }
            return new CalibrateCashUnitCompletion.PayloadData(result.CompletionCode,
                                                               result.ErrorDescription,
                                                               result.ErrorCode,
                                                               calibrateCashUnit.Payload.Cashunit,
                                                               total,
                                                               resItemPosition);
        }
    }
}
