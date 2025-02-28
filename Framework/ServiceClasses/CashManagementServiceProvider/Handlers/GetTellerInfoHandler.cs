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
using XFS4IoTFramework.Common;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.CashManagement
{
    public partial class GetTellerInfoHandler
    {
        private async Task<CommandResult<GetTellerInfoCompletion.PayloadData>> HandleGetTellerInfo(IGetTellerInfoEvents events, GetTellerInfoCommand getTellerInfo, CancellationToken cancel)
        {

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.GetTellerInfoAsync()");

            var result = await Device.GetTellerInfoAsync(
                new GetTellerInfoRequest(
                    getTellerInfo.Payload.TellerID is null ? -1 : (int)getTellerInfo.Payload.TellerID,
                    getTellerInfo.Payload.Currency), 
                cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.GetTellerInfoAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            List<TellerDetailsClass> tellerDetails = null;
            if (result.Details is not null &&
                result.Details.Count > 0)
            {
                tellerDetails = [];
                foreach (var detail in result.Details)
                {
                    Dictionary<string, TellerTotalsClass> totals = [];
                    if (detail.Totals is not null &&
                        detail.Totals.Count > 0)
                    {
                        foreach (var total in detail.Totals)
                        {
                            totals.Add(
                                total.Key, 
                                new TellerTotalsClass(
                                    total.Value.ItemsReceived,
                                    total.Value.ItemsDispensed,
                                    total.Value.CoinsReceived,
                                    total.Value.CoinsDispensed,
                                    total.Value.CashBoxReceived,
                                    total.Value.CashBoxDispensed)
                                );
                        }
                    }

                    tellerDetails.Add(
                        new TellerDetailsClass(
                            detail.TellerId,
                            detail.InputPosition switch
                            {
                                CashManagementCapabilitiesClass.PositionEnum.InBottom => InputPositionEnum.InBottom,
                                CashManagementCapabilitiesClass.PositionEnum.InCenter => InputPositionEnum.InCenter,
                                CashManagementCapabilitiesClass.PositionEnum.InFront => InputPositionEnum.InFront,
                                CashManagementCapabilitiesClass.PositionEnum.InLeft => InputPositionEnum.InLeft,
                                CashManagementCapabilitiesClass.PositionEnum.InRear => InputPositionEnum.InRear,
                                CashManagementCapabilitiesClass.PositionEnum.InRight => InputPositionEnum.InRight,
                                CashManagementCapabilitiesClass.PositionEnum.InTop => InputPositionEnum.InTop,
                                _ => InputPositionEnum.InDefault,
                            },
                            detail.OutputPosition switch
                            {
                                CashManagementCapabilitiesClass.PositionEnum.OutBottom => OutputPositionEnum.OutBottom,
                                CashManagementCapabilitiesClass.PositionEnum.OutCenter => OutputPositionEnum.OutCenter,
                                CashManagementCapabilitiesClass.PositionEnum.OutFront => OutputPositionEnum.OutFront,
                                CashManagementCapabilitiesClass.PositionEnum.OutLeft => OutputPositionEnum.OutLeft,
                                CashManagementCapabilitiesClass.PositionEnum.OutRear => OutputPositionEnum.OutRear,
                                CashManagementCapabilitiesClass.PositionEnum.OutRight => OutputPositionEnum.OutRight,
                                CashManagementCapabilitiesClass.PositionEnum.OutTop => OutputPositionEnum.OutTop,
                                _ => OutputPositionEnum.OutDefault,
                            },
                            totals)
                        );
                }
            }

            GetTellerInfoCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                tellerDetails is not null)
            {
                payload = new(
                    ErrorCode: result.ErrorCode,
                    TellerDetails: tellerDetails);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
