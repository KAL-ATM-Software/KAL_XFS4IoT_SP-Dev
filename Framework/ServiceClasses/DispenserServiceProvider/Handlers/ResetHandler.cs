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
using XFS4IoTServer;
using XFS4IoT.Dispenser.Commands;
using XFS4IoT.Dispenser.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.Dispenser
{
    public partial class ResetHandler
    {
        private async Task<ResetCompletion.PayloadData> HandleReset(IResetEvents events, ResetCommand reset, CancellationToken cancel)
        {
            ItemPosition itemPosition = null;

            if (string.IsNullOrEmpty(reset.Payload.Cashunit) &&
                reset.Payload.RetractArea is null &&
                reset.Payload.OutputPosition is null)
            {
                // Default position is device decide to move items
            }
            else
            {
                if (!string.IsNullOrEmpty(reset.Payload.Cashunit) &&
                    !Dispenser.CashUnits.ContainsKey(reset.Payload.Cashunit))
                {
                    return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"Specified CashUnit location is unknown.");
                }

                if (!string.IsNullOrEmpty(reset.Payload.Cashunit))
                {
                    itemPosition = new ItemPosition(reset.Payload.Cashunit);
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
                        ResetCommand.PayloadData.RetractAreaClass retract = reset.Payload.RetractArea.IsA<ResetCommand.PayloadData.RetractAreaClass>("RetractArea object must be the RetractAreaClass.");

                        CashDispenserCapabilitiesClass.RetractAreaEnum retractArea = CashDispenserCapabilitiesClass.RetractAreaEnum.Default;
                        if (retract.RetractArea is not null)
                        {
                            retractArea = retract.RetractArea switch
                            {
                                ResetCommand.PayloadData.RetractAreaClass.RetractAreaEnum.ItemCassette => CashDispenserCapabilitiesClass.RetractAreaEnum.ItemCassette,
                                ResetCommand.PayloadData.RetractAreaClass.RetractAreaEnum.Reject => CashDispenserCapabilitiesClass.RetractAreaEnum.Reject,
                                ResetCommand.PayloadData.RetractAreaClass.RetractAreaEnum.Retract => CashDispenserCapabilitiesClass.RetractAreaEnum.Retract,
                                ResetCommand.PayloadData.RetractAreaClass.RetractAreaEnum.Stacker => CashDispenserCapabilitiesClass.RetractAreaEnum.Stacker,
                                ResetCommand.PayloadData.RetractAreaClass.RetractAreaEnum.Transport => CashDispenserCapabilitiesClass.RetractAreaEnum.Transport,
                                _ => CashDispenserCapabilitiesClass.RetractAreaEnum.Default
                            };

                            if (!Dispenser.CashDispenserCapabilities.RetractAreas[retractArea])
                            {
                                return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                       $"Specified unsupported retract area. {retractArea}",
                                                                       ResetCompletion.PayloadData.ErrorCodeEnum.InvalidRetractPosition);
                            }
                        }

                        if (retractArea == CashDispenserCapabilitiesClass.RetractAreaEnum.Retract &&
                            retract.Index is null)
                        {
                            return new ResetCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Index property is set to null where the retract area is specified to retract position.");
                        }

                        itemPosition = new ItemPosition(new Retract(retractArea, index));
                    }
                    else if (reset.Payload.OutputPosition is not null)
                    {
                        CashDispenserCapabilitiesClass.OutputPositionEnum position = reset.Payload.OutputPosition switch
                        {
                            ResetCommand.PayloadData.OutputPositionEnum.Bottom => CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom,
                            ResetCommand.PayloadData.OutputPositionEnum.Center => CashDispenserCapabilitiesClass.OutputPositionEnum.Center,
                            ResetCommand.PayloadData.OutputPositionEnum.Front => CashDispenserCapabilitiesClass.OutputPositionEnum.Front,
                            ResetCommand.PayloadData.OutputPositionEnum.Left => CashDispenserCapabilitiesClass.OutputPositionEnum.Left,
                            ResetCommand.PayloadData.OutputPositionEnum.Rear => CashDispenserCapabilitiesClass.OutputPositionEnum.Rear,
                            ResetCommand.PayloadData.OutputPositionEnum.Right => CashDispenserCapabilitiesClass.OutputPositionEnum.Right,
                            ResetCommand.PayloadData.OutputPositionEnum.Top => CashDispenserCapabilitiesClass.OutputPositionEnum.Top,
                            _ => CashDispenserCapabilitiesClass.OutputPositionEnum.Default,

                        };

                        if (!Dispenser.CashDispenserCapabilities.OutputPositons[position])
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

            Dispenser.UpdateCashUnitAccounting(result.MovementResult);

            return new ResetCompletion.PayloadData(result.CompletionCode, 
                                                   result.ErrorDescription, 
                                                   result.ErrorCode);
        }
    }
}
