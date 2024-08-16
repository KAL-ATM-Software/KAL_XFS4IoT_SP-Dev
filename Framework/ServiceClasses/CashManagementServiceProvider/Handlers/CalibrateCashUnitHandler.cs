/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoTFramework.Storage;
using System.Linq;

namespace XFS4IoTFramework.CashManagement
{
    public partial class CalibrateCashUnitHandler
    {
        private async Task<CommandResult<CalibrateCashUnitCompletion.PayloadData>> HandleCalibrateCashUnit(ICalibrateCashUnitEvents events, CalibrateCashUnitCommand calibrateCashUnit, CancellationToken cancel)
        {
            ItemDestination destination = new();

            if (calibrateCashUnit.Payload.Position is null ||
                calibrateCashUnit.Payload.Position.Target is null)
            {
                // Default position is device decide to move items
            }
            else
            {
                if (calibrateCashUnit.Payload.Position.Target == ItemTargetEnumEnum.SingleUnit &&
                    string.IsNullOrEmpty(calibrateCashUnit.Payload.Position.Unit))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified location to {calibrateCashUnit.Payload.Position.Target}, but target unit {calibrateCashUnit.Payload.Position.Unit} property is an empty string.");
                }

                if (calibrateCashUnit.Payload.Position.Target == ItemTargetEnumEnum.SingleUnit &&
                    !string.IsNullOrEmpty(calibrateCashUnit.Payload.Position.Unit) &&
                    !Storage.CashUnits.ContainsKey(calibrateCashUnit.Payload.Position.Unit))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified CashUnit location is unknown. {calibrateCashUnit.Payload.Position.Unit}");
                }

                if (calibrateCashUnit.Payload.Position.Target == ItemTargetEnumEnum.SingleUnit)
                {
                    destination = new ItemDestination(CashUnit: calibrateCashUnit.Payload.Position.Unit);
                }
                else
                {
                    if (calibrateCashUnit.Payload.Position.Target == ItemTargetEnumEnum.Retract ||
                        calibrateCashUnit.Payload.Position.Target == ItemTargetEnumEnum.Transport ||
                        calibrateCashUnit.Payload.Position.Target == ItemTargetEnumEnum.Stacker ||
                        calibrateCashUnit.Payload.Position.Target == ItemTargetEnumEnum.ItemCassette ||
                        calibrateCashUnit.Payload.Position.Target == ItemTargetEnumEnum.CashIn ||
                        calibrateCashUnit.Payload.Position.Target == ItemTargetEnumEnum.Reject)
                    {
                        CashManagementCapabilitiesClass.RetractAreaEnum retractArea = calibrateCashUnit.Payload.Position.Target switch
                        {
                            ItemTargetEnumEnum.ItemCassette => CashManagementCapabilitiesClass.RetractAreaEnum.ItemCassette,
                            ItemTargetEnumEnum.CashIn => CashManagementCapabilitiesClass.RetractAreaEnum.CashIn,
                            ItemTargetEnumEnum.Retract => CashManagementCapabilitiesClass.RetractAreaEnum.Retract,
                            ItemTargetEnumEnum.Stacker => CashManagementCapabilitiesClass.RetractAreaEnum.Stacker,
                            ItemTargetEnumEnum.Reject => CashManagementCapabilitiesClass.RetractAreaEnum.Reject,
                            _ => CashManagementCapabilitiesClass.RetractAreaEnum.Transport,
                        };

                        if (!Common.CashManagementCapabilities.RetractAreas.HasFlag(retractArea))
                        {
                            return new(
                                new(CalibrateCashUnitCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition),
                                MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                $"Specified unsupported retract area. {retractArea}");
                        }

                        if (retractArea == CashManagementCapabilitiesClass.RetractAreaEnum.Retract)
                        {
                            int index = 0;
                            if (calibrateCashUnit.Payload.Position.Index is null)
                            {
                                Logger.Warning(Constants.Framework, $"Index property is set to null where the retract area is specified to retract position. default to zero.");
                            }
                            else
                            {
                                index = (int)calibrateCashUnit.Payload.Position.Index;
                            }

                            if (index < 0)
                            {
                                Logger.Warning(Constants.Framework, $"Index property is set negative value {index}. default to zero.");
                                index = 0;
                            }

                            // Check the index is valid
                            int totalRetractUnits = (from unit in Storage.CashUnits
                                                     where unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract) ||
                                                         unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract)
                                                     select unit).Count();
                            if ((int)index > totalRetractUnits)
                            {
                                return new(
                                    MessageHeader.CompletionCodeEnum.InvalidData,
                                    $"Unexpected index property value is set where the retract area is specified to retract position. " +
                                    $"The value of index one is the first retract position and increments by one for each subsequent position. {index}");
                            }

                            destination = new ItemDestination(ItemTargetEnum.Retract, index);
                        }
                        else
                        {
                            destination = new ItemDestination(
                               calibrateCashUnit.Payload.Position.Target switch
                               {
                                   ItemTargetEnumEnum.ItemCassette => ItemTargetEnum.ItemCassette,
                                   ItemTargetEnumEnum.CashIn => ItemTargetEnum.CashIn,
                                   ItemTargetEnumEnum.Stacker => ItemTargetEnum.Stacker,
                                   ItemTargetEnumEnum.Reject => ItemTargetEnum.Reject,
                                   _ => ItemTargetEnum.Transport,
                               });
                        }
                    }
                    else
                    {
                        if (Common.CashManagementCapabilities.Positions == CashManagementCapabilitiesClass.PositionEnum.NotSupported)
                        {
                            return new(
                                MessageHeader.CompletionCodeEnum.InvalidData,
                                $"Specified unsupported output position. {calibrateCashUnit.Payload.Position.Target}");
                        }

                        CashManagementCapabilitiesClass.PositionEnum position = calibrateCashUnit.Payload.Position.Target switch
                        {
                            ItemTargetEnumEnum.OutBottom => CashManagementCapabilitiesClass.PositionEnum.OutBottom,
                            ItemTargetEnumEnum.OutCenter => CashManagementCapabilitiesClass.PositionEnum.OutCenter,
                            ItemTargetEnumEnum.OutDefault => CashManagementCapabilitiesClass.PositionEnum.OutDefault,
                            ItemTargetEnumEnum.OutFront => CashManagementCapabilitiesClass.PositionEnum.OutFront,
                            ItemTargetEnumEnum.OutLeft => CashManagementCapabilitiesClass.PositionEnum.OutLeft,
                            ItemTargetEnumEnum.OutRear => CashManagementCapabilitiesClass.PositionEnum.OutRear,
                            ItemTargetEnumEnum.OutRight => CashManagementCapabilitiesClass.PositionEnum.OutRight,
                            ItemTargetEnumEnum.OutTop => CashManagementCapabilitiesClass.PositionEnum.OutTop,
                            _ => throw new InvalidDataException($"Unsupported output position is specified. {calibrateCashUnit.Payload.Position.Target}"),
                        };

                        if (!Common.CashManagementCapabilities.Positions.HasFlag(position))
                        {
                            return new(
                                MessageHeader.CompletionCodeEnum.InvalidData,
                                $"Specified unsupported output position. {calibrateCashUnit.Payload.Position.Target}");
                        }

                        destination = new ItemDestination(
                               calibrateCashUnit.Payload.Position.Target switch
                               {
                                   ItemTargetEnumEnum.OutBottom => ItemTargetEnum.OutBottom,
                                   ItemTargetEnumEnum.OutCenter => ItemTargetEnum.OutCenter,
                                   ItemTargetEnumEnum.OutDefault => ItemTargetEnum.OutDefault,
                                   ItemTargetEnumEnum.OutFront => ItemTargetEnum.OutFront,
                                   ItemTargetEnumEnum.OutLeft => ItemTargetEnum.OutLeft,
                                   ItemTargetEnumEnum.OutRear => ItemTargetEnum.OutRear,
                                   ItemTargetEnumEnum.OutRight => ItemTargetEnum.OutRight,
                                   _ => ItemTargetEnum.OutTop,
                               });
                    }
                }
            }

            Logger.Log(Constants.DeviceClass, "CashManagementDev.CalibrateCashUnitAsync()");

            var result = await Device.CalibrateCashUnitAsync(
                new CalibrateCashUnitCommandEvents(Storage, events),
                new CalibrateCashUnitRequest(calibrateCashUnit.Payload.Unit,
                    calibrateCashUnit.Payload.NumOfBills is null ? 
                    0 : 
                    (int)calibrateCashUnit.Payload.NumOfBills,
                    destination), 
                cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.CalibrateCashUnitAsync() -> {result.CompletionCode}, {result.ErrorCode}");


            await Storage.UpdateCashAccounting(result.MovementResult);

            if (result.CompletionCode != MessageHeader.CompletionCodeEnum.Success)
            {
                return new(
                    new(result.ErrorCode),
                    result.CompletionCode,
                    result.ErrorDescription);
            }

            CalibrateCashUnitCompletion.PayloadData.ResultClass movementResult = null;
            if (result.Position is not null)
            {
                int total = calibrateCashUnit.Payload.NumOfBills ?? 0;
                if (result.MovementResult is not null)
                {
                    total = 0;
                    foreach (var movement in result.MovementResult)
                        total += movement.Value.Count;
                }
                movementResult = new(
                    Unit: calibrateCashUnit.Payload.Unit,
                    NumOfBills: total,
                    Position: result.Position is not null ?
                        new(
                            Target: result.Position.Destination == ItemTargetEnum.Default ?
                            null :
                            result.Position.Destination switch
                            {
                                ItemTargetEnum.SingleUnit => ItemTargetEnumEnum.SingleUnit,
                                ItemTargetEnum.Retract => ItemTargetEnumEnum.Retract,
                                ItemTargetEnum.Transport => ItemTargetEnumEnum.Transport,
                                ItemTargetEnum.Stacker => ItemTargetEnumEnum.Stacker,
                                ItemTargetEnum.Reject => ItemTargetEnumEnum.Reject,
                                ItemTargetEnum.ItemCassette => ItemTargetEnumEnum.ItemCassette,
                                ItemTargetEnum.CashIn => ItemTargetEnumEnum.CashIn,
                                ItemTargetEnum.OutDefault => ItemTargetEnumEnum.OutDefault,
                                ItemTargetEnum.OutLeft => ItemTargetEnumEnum.OutLeft,
                                ItemTargetEnum.OutRight => ItemTargetEnumEnum.OutRight,
                                ItemTargetEnum.OutCenter => ItemTargetEnumEnum.OutCenter,
                                ItemTargetEnum.OutTop => ItemTargetEnumEnum.OutTop,
                                ItemTargetEnum.OutBottom => ItemTargetEnumEnum.OutBottom,
                                ItemTargetEnum.OutFront => ItemTargetEnumEnum.OutFront,
                                _ => ItemTargetEnumEnum.OutRear,
                            },
                            Unit: result.Position.CashUnit,
                            Index: result.Position.IndexOfRetractUnit > 0 ?
                                   result.Position.IndexOfRetractUnit : null) :
                        null
                    );
            }

            CalibrateCashUnitCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                movementResult is not null)
            {
                payload = new(
                    ErrorCode: result.ErrorCode,
                    Result: movementResult);
            }

            return new(
                payload,
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
