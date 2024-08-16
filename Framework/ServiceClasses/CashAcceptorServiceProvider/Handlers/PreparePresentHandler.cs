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
using XFS4IoT.CashManagement;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class PreparePresentHandler
    {
        private async Task<CommandResult<PreparePresentCompletion.PayloadData>> HandlePreparePresent(IPreparePresentEvents events, PreparePresentCommand preparePresent, CancellationToken cancel)
        {
            CashManagementCapabilitiesClass.PositionEnum outputPosition = CashManagementCapabilitiesClass.PositionEnum.OutDefault;
            if (preparePresent.Payload.Position is not null)
            {
                outputPosition = preparePresent.Payload.Position switch
                {
                    OutputPositionEnum.OutBottom => CashManagementCapabilitiesClass.PositionEnum.OutBottom,
                    OutputPositionEnum.OutCenter => CashManagementCapabilitiesClass.PositionEnum.OutCenter,
                    OutputPositionEnum.OutDefault => CashManagementCapabilitiesClass.PositionEnum.OutDefault,
                    OutputPositionEnum.OutFront => CashManagementCapabilitiesClass.PositionEnum.OutFront,
                    OutputPositionEnum.OutLeft => CashManagementCapabilitiesClass.PositionEnum.OutLeft,
                    OutputPositionEnum.OutRear => CashManagementCapabilitiesClass.PositionEnum.OutRear,
                    OutputPositionEnum.OutRight => CashManagementCapabilitiesClass.PositionEnum.OutRight,
                    OutputPositionEnum.OutTop => CashManagementCapabilitiesClass.PositionEnum.OutTop,
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
                    new(PreparePresentCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The specified position is not supported. {outputPosition}");
            }

            if (Common.CashAcceptorStatus.Positions[outputPosition].PositionStatus != CashManagementStatusClass.PositionStatusEnum.Empty)
            {
                return new(
                    new(PreparePresentCompletion.PayloadData.ErrorCodeEnum.PositionNotEmpty),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The specified position is not empty. {outputPosition}");
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.PreparePresent()");

            var result = await Device.PreparePresent(new PreparePresentCommandEvents(Storage, events),
                                                     new PreparePresentRequest(outputPosition),
                                                     cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.PreparePresent() -> {result.CompletionCode}, {result.ErrorCode}");

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);

        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
