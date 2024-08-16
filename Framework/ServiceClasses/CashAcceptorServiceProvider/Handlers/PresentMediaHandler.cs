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
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class PresentMediaHandler
    {
        private async Task<CommandResult<PresentMediaCompletion.PayloadData>> HandlePresentMedia(IPresentMediaEvents events, PresentMediaCommand presentMedia, CancellationToken cancel)
        {
            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new(
                    new(PresentMediaCompletion.PayloadData.ErrorCodeEnum.ExchangeActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The exchange state is already in active.");
            }

            // First check the position capabilities
            CashManagementCapabilitiesClass.PositionEnum outputPosition = CashManagementCapabilitiesClass.PositionEnum.OutDefault;
            if (presentMedia.Payload.Position is not null)
            {
                outputPosition = presentMedia.Payload.Position switch
                {
                    XFS4IoT.CashManagement.PositionEnum.OutBottom => CashManagementCapabilitiesClass.PositionEnum.OutBottom,
                    XFS4IoT.CashManagement.PositionEnum.OutCenter => CashManagementCapabilitiesClass.PositionEnum.OutCenter,
                    XFS4IoT.CashManagement.PositionEnum.OutDefault => CashManagementCapabilitiesClass.PositionEnum.OutDefault,
                    XFS4IoT.CashManagement.PositionEnum.OutFront => CashManagementCapabilitiesClass.PositionEnum.OutFront,
                    XFS4IoT.CashManagement.PositionEnum.OutLeft => CashManagementCapabilitiesClass.PositionEnum.OutLeft,
                    XFS4IoT.CashManagement.PositionEnum.OutRear => CashManagementCapabilitiesClass.PositionEnum.OutRear,
                    XFS4IoT.CashManagement.PositionEnum.OutRight => CashManagementCapabilitiesClass.PositionEnum.OutRight,
                    XFS4IoT.CashManagement.PositionEnum.OutTop => CashManagementCapabilitiesClass.PositionEnum.OutTop,
                    _ => CashManagementCapabilitiesClass.PositionEnum.NotSupported
                };
            }

            if (!Common.CashAcceptorCapabilities.Positions.ContainsKey(outputPosition))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Unsupported output position. {outputPosition}");
            }

            if (!Common.CashAcceptorStatus.Positions.ContainsKey(outputPosition))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InternalError,
                    $"The device class does not report supported position status. {outputPosition}");
            }

            if (Common.CashAcceptorStatus.Positions[outputPosition].PositionStatus == CashManagementStatusClass.PositionStatusEnum.NotSupported)
            {
                return new(
                    new(PresentMediaCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The specified position is not supported. {outputPosition}");
            }

            if (Common.CashAcceptorStatus.Positions[outputPosition].PositionStatus == CashManagementStatusClass.PositionStatusEnum.Empty)
            {
                return new(
                    new(PresentMediaCompletion.PayloadData.ErrorCodeEnum.NoItems),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The specified position is empty. {outputPosition}");
            }

            if (Common.CashAcceptorCapabilities.Positions.ContainsKey(outputPosition))
            {
                if (!Common.CashAcceptorCapabilities.Positions[outputPosition].PresentControl)
                {
                    return new(MessageHeader.CompletionCodeEnum.InvalidData,
                                                                  $"Specified position reported by the PresentControl property in PositionCapabilities is {Common.CashAcceptorCapabilities.Positions[outputPosition].PresentControl}");
                }
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.PresentMedia()");

            var result = await Device.PresentMedia(new PresentMediaRequest(outputPosition, new(CashManagement.LastCashManagementPresentStatus[outputPosition])),
                                                   cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.PresentMedia() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.LastPresentStatus is not null)
            {
                // Update internal present status
                CashManagement.LastCashManagementPresentStatus[outputPosition] = new(result.LastPresentStatus);
                CashManagement.StoreCashManagementPresentStatus();
            }

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);

        }

        private ICashManagementService CashManagement { get => Provider.IsA<ICashManagementService>(); }
    }
}
