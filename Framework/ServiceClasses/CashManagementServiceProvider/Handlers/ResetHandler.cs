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
using XFS4IoT.Completions;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoT.CashManagement;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashManagement
{
    public partial class ResetHandler
    {

        private async Task<ResetCompletion.PayloadData> HandleReset(IResetEvents events, ResetCommand reset, CancellationToken cancel)
        {
            ItemPosition itemPosition = null;

            if (string.IsNullOrEmpty(reset.Payload.Unit) &&
                reset.Payload.RetractArea is null &&
                reset.Payload.OutputPosition is null)
            {
                // Default position is device decide to move items
            }
            else
            {
                if (!string.IsNullOrEmpty(reset.Payload.Unit) &&
                    !CashManagement.CashUnits.ContainsKey(reset.Payload.Unit))
                {
                    return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"Specified CashUnit location is unknown. {reset.Payload.Unit}");
                }

                if (!string.IsNullOrEmpty(reset.Payload.Unit))
                {
                    itemPosition = new ItemPosition(reset.Payload.Unit);
                }
                else
                {
                    if (reset.Payload.RetractArea is not null &&
                        reset.Payload.OutputPosition is not null)
                    {
                        return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Specified both RetractArea and OutputPosition properties, the Service Provider doesn't know where the items to be moved.");
                    }

                    if (reset.Payload.RetractArea is not null)
                    {
                        int? index = null;
                        RetractClass retract = reset.Payload.RetractArea.IsA<RetractClass>("RetractArea object must be the RetractAreaClass.");

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
                                !CashManagement.CashManagementCapabilities.RetractAreas.HasFlag(retractArea))
                            {
                                return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                       $"Specified unsupported retract area. {retractArea}",
                                                                       ResetCompletion.PayloadData.ErrorCodeEnum.InvalidRetractPosition);
                            }
                        }

                        if (retractArea == CashManagementCapabilitiesClass.RetractAreaEnum.Retract &&
                            retract.Index is null)
                        {
                            return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Index property is set to null where the retract area is specified to retract position.");
                        }

                        itemPosition = new ItemPosition(new Retract(retractArea, index));
                    }
                    else if (reset.Payload.OutputPosition is not null)
                    {
                        CashManagementCapabilitiesClass.PositionEnum position = reset.Payload.OutputPosition switch
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
                            return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Specified unsupported output position. {position}");
                        }

                        itemPosition = new ItemPosition(position);
                    }
                }
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.ResetDeviceAsync()");

            var result = await Device.ResetDeviceAsync(events,
                                                       new ResetDeviceRequest(itemPosition),
                                                       cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.ResetDeviceAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            await CashManagement.UpdateCashAccounting(result.MovementResult);

            return new ResetCompletion.PayloadData(result.CompletionCode,
                                                   result.ErrorDescription,
                                                   result.ErrorCode);
        }
    }
}
