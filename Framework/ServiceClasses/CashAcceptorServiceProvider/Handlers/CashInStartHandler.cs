/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.CashManagement;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class CashInStartHandler
    {
        private async Task<CashInStartCompletion.PayloadData> HandleCashInStart(ICashInStartEvents events, CashInStartCommand cashInStart, CancellationToken cancel)
        {
            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new CashInStartCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                             $"The exchange state is already in active.",
                                                             CashInStartCompletion.PayloadData.ErrorCodeEnum.ExchangeActive);
            }

            if (CashAcceptor.CashInStatus.Status == CashInStatusClass.StatusEnum.Active)
            {
                return new CashInStartCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                             $"The cash-in state is already in active.",
                                                             CashInStartCompletion.PayloadData.ErrorCodeEnum.CashInActive);
            }

            CashManagementCapabilitiesClass.PositionEnum outputPosition = CashManagementCapabilitiesClass.PositionEnum.OutDefault;
            if (cashInStart.Payload.OutputPosition is not null)
            {
                outputPosition = cashInStart.Payload.OutputPosition switch
                {
                    OutputPositionEnum.OutBottom => CashManagementCapabilitiesClass.PositionEnum.OutBottom,
                    OutputPositionEnum.OutCenter => CashManagementCapabilitiesClass.PositionEnum.OutCenter,
                    OutputPositionEnum.OutDefault => CashManagementCapabilitiesClass.PositionEnum.OutDefault,
                    OutputPositionEnum.OutFront => CashManagementCapabilitiesClass.PositionEnum.OutFront,
                    OutputPositionEnum.OutLeft => CashManagementCapabilitiesClass.PositionEnum.OutLeft,
                    OutputPositionEnum.OutRear => CashManagementCapabilitiesClass.PositionEnum.OutRear,
                    OutputPositionEnum.OutRight => CashManagementCapabilitiesClass.PositionEnum.OutRight,
                    OutputPositionEnum.OutTop => CashManagementCapabilitiesClass.PositionEnum.OutTop,
                    _ => throw new InternalErrorException($"Unexpected output position specified. {cashInStart.Payload.OutputPosition}"),
                };
            }

            if (!Common.CashAcceptorCapabilities.Positions.ContainsKey(outputPosition))
            {
                return new CashInStartCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                             $"Unsupported output position. {outputPosition}");
            }

            if (Common.CashAcceptorStatus.Positions[outputPosition].PositionStatus == CashManagementStatusClass.PositionStatusEnum.NotSupported)
            {
                return new CashInStartCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                             $"The specified output position is not supported. {outputPosition}",
                                                             CashInStartCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition);
            }

            CashManagementCapabilitiesClass.PositionEnum inputPosition = CashManagementCapabilitiesClass.PositionEnum.InDefault;
            if (cashInStart.Payload.InputPosition is not null)
            {
                inputPosition = cashInStart.Payload.InputPosition switch
                {
                    InputPositionEnum.InBottom => CashManagementCapabilitiesClass.PositionEnum.InBottom,
                    InputPositionEnum.InCenter => CashManagementCapabilitiesClass.PositionEnum.InCenter,
                    InputPositionEnum.InDefault => CashManagementCapabilitiesClass.PositionEnum.InDefault,
                    InputPositionEnum.InFront => CashManagementCapabilitiesClass.PositionEnum.InFront,
                    InputPositionEnum.InLeft => CashManagementCapabilitiesClass.PositionEnum.InLeft,
                    InputPositionEnum.InRear => CashManagementCapabilitiesClass.PositionEnum.InRear,
                    InputPositionEnum.InRight => CashManagementCapabilitiesClass.PositionEnum.InRight,
                    InputPositionEnum.InTop => CashManagementCapabilitiesClass.PositionEnum.InTop,
                    _ => throw new InternalErrorException($"Unexpected input position specified. {cashInStart.Payload.InputPosition}"),
                };
            }

            if (!Common.CashAcceptorCapabilities.Positions.ContainsKey(inputPosition))
            {
                return new CashInStartCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                             $"Unsupported input position. {inputPosition}");
            }

            if (Common.CashAcceptorStatus.Positions[inputPosition].PositionStatus == CashManagementStatusClass.PositionStatusEnum.NotSupported)
            {
                return new CashInStartCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                             $"The specified input position is not supported. {inputPosition}",
                                                             CashInStartCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition);
            }

            // Clear TotalReturnedItems for the present status
            foreach (var presentStatus in CashManagement.LastCashManagementPresentStatus)
            {
                presentStatus.Value.TotalReturnedItems = new();
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashInStart()");

            var result = await Device.CashInStart(new CashInStartRequest(
                                                        cashInStart.Payload.TellerID,
                                                        cashInStart.Payload.UseRecycleUnits ?? false,
                                                        outputPosition,
                                                        inputPosition,
                                                        cashInStart.Payload.TotalItemsLimit ?? 0,
                                                        cashInStart.Payload.AmountLimit?.Where(a => (a.Value is not null && a.Value != 0) && (!string.IsNullOrEmpty(a.Currency) && Regex.IsMatch(a.Currency, "^[A-Z]{3}$"))).ToDictionary(a => a.Currency, a => (double)a.Value)
                                                       ), cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.CashInStart() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                CashManagement.CashInStatusManaged.NumOfRefusedItems = 0;
                CashManagement.CashInStatusManaged.CashCounts = new();
                CashManagement.CashInStatusManaged.Status = CashInStatusClass.StatusEnum.Active;
                CashManagement.StoreCashInStatus();
            }

            return new CashInStartCompletion.PayloadData(result.CompletionCode,
                                                         result.ErrorDescription,
                                                         result.ErrorCode);
        }

        private ICashManagementService CashManagement { get => Provider.IsA<ICashManagementService>(); }
    }
}
