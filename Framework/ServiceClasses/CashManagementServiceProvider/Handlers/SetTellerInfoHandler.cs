/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoT.CashManagement;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashManagement
{
    public partial class SetTellerInfoHandler
    {
        private async Task<CommandResult<SetTellerInfoCompletion.PayloadData>> HandleSetTellerInfo(ISetTellerInfoEvents events, SetTellerInfoCommand setTellerInfo, CancellationToken cancel)
        {
            TellerDetail detail = null;
            if (setTellerInfo.Payload.TellerDetails is not null)
            {
                if (setTellerInfo.Payload.TellerDetails.TellerID is null ||
                    setTellerInfo.Payload.TellerDetails.TellerID < 0)
                {
                    return new(
                        new(SetTellerInfoCompletion.PayloadData.ErrorCodeEnum.InvalidTellerId),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Invalid teller ID specified. {setTellerInfo.Payload.TellerDetails.TellerID}");
                }

                if (setTellerInfo.Payload.TellerDetails.InputPosition is not null)
                {
                    if ((setTellerInfo.Payload.TellerDetails.InputPosition == InputPositionEnum.InBottom &&
                         !Common.CashManagementCapabilities.Positions.HasFlag(CashManagementCapabilitiesClass.PositionEnum.InBottom)) ||
                        (setTellerInfo.Payload.TellerDetails.InputPosition == InputPositionEnum.InCenter &&
                         !Common.CashManagementCapabilities.Positions.HasFlag(CashManagementCapabilitiesClass.PositionEnum.InCenter)) ||
                        (setTellerInfo.Payload.TellerDetails.InputPosition == InputPositionEnum.InFront &&
                         !Common.CashManagementCapabilities.Positions.HasFlag(CashManagementCapabilitiesClass.PositionEnum.InFront)) ||
                        (setTellerInfo.Payload.TellerDetails.InputPosition == InputPositionEnum.InLeft &&
                         !Common.CashManagementCapabilities.Positions.HasFlag(CashManagementCapabilitiesClass.PositionEnum.InLeft)) ||
                        (setTellerInfo.Payload.TellerDetails.InputPosition == InputPositionEnum.InRear &&
                         !Common.CashManagementCapabilities.Positions.HasFlag(CashManagementCapabilitiesClass.PositionEnum.InRear)) ||
                        (setTellerInfo.Payload.TellerDetails.InputPosition == InputPositionEnum.InRight &&
                         !Common.CashManagementCapabilities.Positions.HasFlag(CashManagementCapabilitiesClass.PositionEnum.InRight)) ||
                        (setTellerInfo.Payload.TellerDetails.InputPosition == InputPositionEnum.InTop &&
                         !Common.CashManagementCapabilities.Positions.HasFlag(CashManagementCapabilitiesClass.PositionEnum.InTop)))
                    {
                        return new(
                            new(SetTellerInfoCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode,
                            $"Specified input position is not supported in the capabilities. {setTellerInfo.Payload.TellerDetails.InputPosition}");
                    }
                }

                if (setTellerInfo.Payload.TellerDetails.OutputPosition is not null)
                {
                    if ((setTellerInfo.Payload.TellerDetails.OutputPosition == OutputPositionEnum.OutBottom &&
                         !Common.CashManagementCapabilities.Positions.HasFlag(CashManagementCapabilitiesClass.PositionEnum.OutBottom)) ||
                        (setTellerInfo.Payload.TellerDetails.OutputPosition == OutputPositionEnum.OutCenter &&
                         !Common.CashManagementCapabilities.Positions.HasFlag(CashManagementCapabilitiesClass.PositionEnum.OutCenter)) ||
                        (setTellerInfo.Payload.TellerDetails.OutputPosition == OutputPositionEnum.OutFront &&
                         !Common.CashManagementCapabilities.Positions.HasFlag(CashManagementCapabilitiesClass.PositionEnum.OutFront)) ||
                        (setTellerInfo.Payload.TellerDetails.OutputPosition == OutputPositionEnum.OutLeft &&
                         !Common.CashManagementCapabilities.Positions.HasFlag(CashManagementCapabilitiesClass.PositionEnum.OutLeft)) ||
                        (setTellerInfo.Payload.TellerDetails.OutputPosition == OutputPositionEnum.OutRear &&
                         !Common.CashManagementCapabilities.Positions.HasFlag(CashManagementCapabilitiesClass.PositionEnum.OutRear)) ||
                        (setTellerInfo.Payload.TellerDetails.OutputPosition == OutputPositionEnum.OutRight &&
                         !Common.CashManagementCapabilities.Positions.HasFlag(CashManagementCapabilitiesClass.PositionEnum.OutRight)) ||
                        (setTellerInfo.Payload.TellerDetails.OutputPosition == OutputPositionEnum.OutTop &&
                         !Common.CashManagementCapabilities.Positions.HasFlag(CashManagementCapabilitiesClass.PositionEnum.OutTop)))
                    {
                        return new(
                            new(SetTellerInfoCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode,
                            $"Specified output position is not supported in the capabilities. {setTellerInfo.Payload.TellerDetails.InputPosition}");
                    }
                }

                Dictionary<string, TellerDetail.TellerTotal> totals = new();
                if (setTellerInfo.Payload.TellerDetails.TellerTotals is not null &&
                    setTellerInfo.Payload.TellerDetails.TellerTotals.Count > 0)
                {
                    foreach (var total in setTellerInfo.Payload.TellerDetails.TellerTotals)
                    {
                        totals.Add(
                            total.Key, 
                            new TellerDetail.TellerTotal(
                                (double)total.Value.ItemsReceived,
                                (double)total.Value.ItemsDispensed,
                                (double)total.Value.CoinsReceived,
                                (double)total.Value.CoinsReceived,
                                (double)total.Value.CashBoxReceived,
                                (double)total.Value.CashBoxReceived)
                            );
                    }
                }

                detail = new TellerDetail(
                    (int)setTellerInfo.Payload.TellerDetails.TellerID,
                    setTellerInfo.Payload.TellerDetails.InputPosition is null ? CashManagementCapabilitiesClass.PositionEnum.InDefault :
                    setTellerInfo.Payload.TellerDetails.InputPosition switch
                    { 
                        InputPositionEnum.InBottom => CashManagementCapabilitiesClass.PositionEnum.InBottom,
                        InputPositionEnum.InCenter => CashManagementCapabilitiesClass.PositionEnum.InCenter,
                        InputPositionEnum.InFront => CashManagementCapabilitiesClass.PositionEnum.InFront,
                        InputPositionEnum.InLeft => CashManagementCapabilitiesClass.PositionEnum.InLeft,
                        InputPositionEnum.InRear => CashManagementCapabilitiesClass.PositionEnum.InRear,
                        InputPositionEnum.InRight => CashManagementCapabilitiesClass.PositionEnum.InRight,
                        InputPositionEnum.InTop => CashManagementCapabilitiesClass.PositionEnum.InTop,
                        _ => CashManagementCapabilitiesClass.PositionEnum.InDefault,
                    },
                    setTellerInfo.Payload.TellerDetails.OutputPosition is null ? CashManagementCapabilitiesClass.PositionEnum.OutDefault :
                    setTellerInfo.Payload.TellerDetails.OutputPosition switch
                    {
                        OutputPositionEnum.OutBottom => CashManagementCapabilitiesClass.PositionEnum.OutBottom,
                        OutputPositionEnum.OutCenter => CashManagementCapabilitiesClass.PositionEnum.OutCenter,
                        OutputPositionEnum.OutFront => CashManagementCapabilitiesClass.PositionEnum.OutFront,
                        OutputPositionEnum.OutLeft => CashManagementCapabilitiesClass.PositionEnum.OutLeft,
                        OutputPositionEnum.OutRear => CashManagementCapabilitiesClass.PositionEnum.OutRear,
                        OutputPositionEnum.OutRight => CashManagementCapabilitiesClass.PositionEnum.OutRight,
                        OutputPositionEnum.OutTop => CashManagementCapabilitiesClass.PositionEnum.OutTop,
                        _ => CashManagementCapabilitiesClass.PositionEnum.OutDefault,
                    },
                    totals);
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.SetTellerInfoAsync()");

            var result = await Device.SetTellerInfoAsync(
                new SetTellerInfoRequest(
                    setTellerInfo.Payload.Action switch
                    {
                        SetTellerInfoCommand.PayloadData.ActionEnum.Create => SetTellerInfoRequest.ActionEnum.Create,
                        SetTellerInfoCommand.PayloadData.ActionEnum.Delete => SetTellerInfoRequest.ActionEnum.Delete,
                        _ => SetTellerInfoRequest.ActionEnum.Modify,
                    },
                    detail),
                    cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.SetTellerInfoAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
