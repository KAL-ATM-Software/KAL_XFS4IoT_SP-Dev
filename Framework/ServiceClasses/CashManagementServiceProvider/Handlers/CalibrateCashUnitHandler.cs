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
using XFS4IoT.CashManagement;
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
            if (string.IsNullOrEmpty(calibrateCashUnit.Payload.Unit) ||
                !CashManagement.CashUnits.ContainsKey(calibrateCashUnit.Payload.Unit))
            {
                return new CalibrateCashUnitCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"The CashUnit property is specified null or empty. the application should specify items to be dispensed from which unit.");
            }

            ItemPosition itemPosition = null;

            if (calibrateCashUnit.Payload.Position is null ||
                (string.IsNullOrEmpty(calibrateCashUnit.Payload.Position.Unit) &&
                 calibrateCashUnit.Payload.Position.RetractArea is null &&
                 calibrateCashUnit.Payload.Position.OutputPosition is null))
            {
                // Default position to move items into is decided by the device 
            }
            else
            {
                if (!string.IsNullOrEmpty(calibrateCashUnit.Payload.Position.Unit) &&
                    !CashManagement.CashUnits.ContainsKey(calibrateCashUnit.Payload.Position.Unit))
                {
                    return new CalibrateCashUnitCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                       $"Specified CashUnit location is unknown.");
                }

                if (!string.IsNullOrEmpty(calibrateCashUnit.Payload.Position.Unit))
                {
                    itemPosition = new ItemPosition(calibrateCashUnit.Payload.Position.Unit);
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

                        var retract = calibrateCashUnit.Payload.Position.RetractArea.IsA<RetractClass>("RetractArea object must be the RetractAreaClass.");

                        CashManagementCapabilitiesClass.RetractAreaEnum retractArea = CashManagementCapabilitiesClass.RetractAreaEnum.Default;
                        if (retract.RetractArea is not null)
                        {
                            retractArea = retract.RetractArea switch
                            {
                                RetractAreaEnum.ItemCassette => CashManagementCapabilitiesClass.RetractAreaEnum.ItemCassette,
                                RetractAreaEnum.Reject => CashManagementCapabilitiesClass.RetractAreaEnum.Reject,
                                RetractAreaEnum.Retract => CashManagementCapabilitiesClass.RetractAreaEnum.Retract,
                                RetractAreaEnum.Stacker => CashManagementCapabilitiesClass.RetractAreaEnum.Stacker,
                                RetractAreaEnum.Transport => CashManagementCapabilitiesClass.RetractAreaEnum.Transport,
                                _ => CashManagementCapabilitiesClass.RetractAreaEnum.Default
                            };

                            if (retractArea != CashManagementCapabilitiesClass.RetractAreaEnum.Default &&
                                !CashManagement.CashDispenserCapabilities.RetractAreas.HasFlag(retractArea))
                            {
                                return new CalibrateCashUnitCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                                   $"Specified unsupported retract area. {retractArea}",
                                                                                   CalibrateCashUnitCompletion.PayloadData.ErrorCodeEnum.InvalidCashUnit);
                            }
                        }

                        if (retractArea == CashManagementCapabilitiesClass.RetractAreaEnum.Retract &&
                            retract.Index is null)
                        {
                            return new CalibrateCashUnitCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                               "Index property is set to null where the retract area is specified to retract position.");
                        }

                        itemPosition = new ItemPosition(new Retract(retractArea, index));
                    }
                    else
                    {
                        CashManagementCapabilitiesClass.PositionEnum position = calibrateCashUnit.Payload.Position.OutputPosition switch
                        {
                            OutputPositionEnum.OutBottom => CashManagementCapabilitiesClass.PositionEnum.OutBottom,
                            OutputPositionEnum.OutCenter => CashManagementCapabilitiesClass.PositionEnum.OutCenter,
                            OutputPositionEnum.OutDefault => CashManagementCapabilitiesClass.PositionEnum.OutDefault,
                            OutputPositionEnum.OutFront => CashManagementCapabilitiesClass.PositionEnum.OutFront,
                            OutputPositionEnum.OutLeft => CashManagementCapabilitiesClass.PositionEnum.OutLeft,
                            OutputPositionEnum.OutRear => CashManagementCapabilitiesClass.PositionEnum.OutRear,
                            OutputPositionEnum.OutRight => CashManagementCapabilitiesClass.PositionEnum.OutRight,
                            OutputPositionEnum.OutTop => CashManagementCapabilitiesClass.PositionEnum.OutTop,
                            _ => CashManagementCapabilitiesClass.PositionEnum.NotSupported,

                        };

                        if (position == CashManagementCapabilitiesClass.PositionEnum.NotSupported ||
                            !CashManagement.CashManagementCapabilities.Positions.HasFlag(position))
                        {
                            return new CalibrateCashUnitCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                               $"Specified unsupported output position. {position}");
                        }

                        itemPosition = new ItemPosition(position);
                    }
                }
            }

            Logger.Log(Constants.DeviceClass, "CashManagementDev.CalibrateCashUnitAsync()");

            var result = await Device.CalibrateCashUnitAsync(events, new CalibrateCashUnitRequest(calibrateCashUnit.Payload.Unit,
                                                                                                  calibrateCashUnit.Payload.NumOfBills is null ? 0 : (int)calibrateCashUnit.Payload.NumOfBills,
                                                                                                  itemPosition), cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.CalibrateCashUnitAsync() -> {result.CompletionCode}, {result.ErrorCode}");


            await CashManagement.UpdateCashAccounting(result.MovementResult);

            if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
            {
                return new CalibrateCashUnitCompletion.PayloadData(result.CompletionCode,
                                                                   result.ErrorDescription,
                                                                   result.ErrorCode);
            }

            ItemPositionClass resItemPosition = null;
            if (result.Position is not null)
            {
                RetractClass retractArea = null;
                if (result.Position.RetractArea is not null)
                {
                    retractArea = new RetractClass(
                                        null,
                                        result.Position.RetractArea.RetractArea switch
                                        {
                                            CashManagementCapabilitiesClass.RetractAreaEnum.ItemCassette => RetractAreaEnum.ItemCassette,
                                            CashManagementCapabilitiesClass.RetractAreaEnum.Reject => RetractAreaEnum.Reject,
                                            CashManagementCapabilitiesClass.RetractAreaEnum.Stacker => RetractAreaEnum.Stacker,
                                            CashManagementCapabilitiesClass.RetractAreaEnum.Transport => RetractAreaEnum.Transport,
                                            _ => RetractAreaEnum.Retract,
                                        },
                                        result.Position.RetractArea.Index);
                }
                resItemPosition = new ItemPositionClass(
                                        result.Position.CashUnit,
                                        retractArea,
                                        result.Position.OutputPosition is not null ? result.Position.OutputPosition switch
                                        {
                                            CashManagementCapabilitiesClass.PositionEnum.OutBottom => OutputPositionEnum.OutBottom,
                                            CashManagementCapabilitiesClass.PositionEnum.OutCenter => OutputPositionEnum.OutCenter,
                                            CashManagementCapabilitiesClass.PositionEnum.OutFront => OutputPositionEnum.OutFront,
                                            CashManagementCapabilitiesClass.PositionEnum.OutLeft => OutputPositionEnum.OutLeft,
                                            CashManagementCapabilitiesClass.PositionEnum.OutRear => OutputPositionEnum.OutRear,
                                            CashManagementCapabilitiesClass.PositionEnum.OutRight => OutputPositionEnum.OutRight,
                                            CashManagementCapabilitiesClass.PositionEnum.OutTop => OutputPositionEnum.OutTop,
                                            _ => OutputPositionEnum.OutDefault
                                        } : null);
            }

            int total = calibrateCashUnit.Payload.NumOfBills ?? 0;
            if (result.MovementResult is not null)
            {
                total = 0;
                foreach (var movement in result.MovementResult)
                    total += movement.Value.Count;
            }
            return new CalibrateCashUnitCompletion.PayloadData(result.CompletionCode,
                                                               result.ErrorDescription,
                                                               result.ErrorCode,
                                                               calibrateCashUnit.Payload.Unit,
                                                               total,
                                                               resItemPosition);
        }
    }
}
