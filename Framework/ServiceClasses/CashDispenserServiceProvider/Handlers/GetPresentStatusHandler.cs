/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoT.CashDispenser;
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoT.CashManagement;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashDispenser;

public partial class GetPresentStatusHandler
{
    private async Task<GetPresentStatusCompletion.PayloadData> HandleGetPresentStatus(IGetPresentStatusEvents events, GetPresentStatusCommand getPresentStatus, CancellationToken cancel)
    {
        CashManagementCapabilitiesClass.OutputPositionEnum position = CashManagementCapabilitiesClass.OutputPositionEnum.Default;
        if (getPresentStatus.Payload.Position is not null)
        {
            position = getPresentStatus.Payload.Position switch
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

        var responceNonce = getPresentStatus.Payload.Nonce;
        string responseToken = null;
        if (responceNonce is not null)
            responseToken = await Device.GetPresentStatusTokenAsync(responceNonce);

        if (position == CashManagementCapabilitiesClass.OutputPositionEnum.NotSupported)
        {
            return new GetPresentStatusCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Specified invalid position {position}");
        }

        if (!Common.CashDispenserCapabilities.OutputPositions.HasFlag(position))
        {
            return new GetPresentStatusCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                              $"Specified unsupported position {position}",
                                                              GetPresentStatusCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition);
        }

        CashDispenser.LastCashDispenserPresentStatus.ContainsKey(position).IsTrue($"Unexpected position is specified. {position}");

        return new GetPresentStatusCompletion.PayloadData(
            CompletionCode:     MessagePayload.CompletionCodeEnum.Success,
            ErrorDescription:   null,
            ErrorCode:          null,
            Denomination:       new DenominationClass(
                CashDispenser.LastCashDispenserPresentStatus[position].LastDenomination?.CurrencyAmounts,
                CashDispenser.LastCashDispenserPresentStatus[position].LastDenomination?.Values),
            PresentState:       CashDispenser.LastCashDispenserPresentStatus[position].Status switch
                {
                    CashDispenserPresentStatus.PresentStatusEnum.NotPresented => GetPresentStatusCompletion.PayloadData.PresentStateEnum.NotPresented,
                    CashDispenserPresentStatus.PresentStatusEnum.Presented => GetPresentStatusCompletion.PayloadData.PresentStateEnum.Presented,
                    _ => GetPresentStatusCompletion.PayloadData.PresentStateEnum.Unknown
                },
            Token:              responseToken
            );
    }
}
