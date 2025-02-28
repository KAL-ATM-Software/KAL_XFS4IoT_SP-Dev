/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class GetPresentStatusHandler
    {
        private Task<CommandResult<GetPresentStatusCompletion.PayloadData>> HandleGetPresentStatus(IGetPresentStatusEvents events, GetPresentStatusCommand getPresentStatus, CancellationToken cancel)
        {
            CashManagementCapabilitiesClass.PositionEnum position = CashManagementCapabilitiesClass.PositionEnum.NotSupported;
            foreach (var presentStatus in CashManagement.LastCashManagementPresentStatus)
            {
                if (!Common.CashAcceptorCapabilities.Positions.ContainsKey(presentStatus.Key) ||
                    presentStatus.Key == CashManagementCapabilitiesClass.PositionEnum.InBottom ||
                    presentStatus.Key == CashManagementCapabilitiesClass.PositionEnum.InCenter ||
                    presentStatus.Key == CashManagementCapabilitiesClass.PositionEnum.InDefault ||
                    presentStatus.Key == CashManagementCapabilitiesClass.PositionEnum.InFront ||
                    presentStatus.Key == CashManagementCapabilitiesClass.PositionEnum.InLeft ||
                    presentStatus.Key == CashManagementCapabilitiesClass.PositionEnum.InRear ||
                    presentStatus.Key == CashManagementCapabilitiesClass.PositionEnum.InRight ||
                    presentStatus.Key == CashManagementCapabilitiesClass.PositionEnum.InTop)
                {
                    continue;
                }
                // Only one position can be reported
                position = presentStatus.Key;
                break;
            }

            if (position == CashManagementCapabilitiesClass.PositionEnum.NotSupported)
            {
                return Task.FromResult(new CommandResult<GetPresentStatusCompletion.PayloadData>(
                    MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                    $"The device doesn't support output position."));
            }

            return Task.FromResult(
                new CommandResult<GetPresentStatusCompletion.PayloadData>(
                    new(
                        position switch
                        {
                            CashManagementCapabilitiesClass.PositionEnum.OutBottom => XFS4IoT.CashManagement.OutputPositionEnum.OutBottom,
                            CashManagementCapabilitiesClass.PositionEnum.OutCenter => XFS4IoT.CashManagement.OutputPositionEnum.OutCenter,
                            CashManagementCapabilitiesClass.PositionEnum.OutDefault => XFS4IoT.CashManagement.OutputPositionEnum.OutDefault,
                            CashManagementCapabilitiesClass.PositionEnum.OutFront => XFS4IoT.CashManagement.OutputPositionEnum.OutFront,
                            CashManagementCapabilitiesClass.PositionEnum.OutLeft => XFS4IoT.CashManagement.OutputPositionEnum.OutLeft,
                            CashManagementCapabilitiesClass.PositionEnum.OutRear => XFS4IoT.CashManagement.OutputPositionEnum.OutRear,
                            CashManagementCapabilitiesClass.PositionEnum.OutRight => XFS4IoT.CashManagement.OutputPositionEnum.OutRight,
                            CashManagementCapabilitiesClass.PositionEnum.OutTop => XFS4IoT.CashManagement.OutputPositionEnum.OutTop,
                            _ => null,
                        },
                        CashManagement.LastCashManagementPresentStatus[position].PresentState switch
                        {
                            CashManagementPresentStatus.PresentStateEnum.NotPresented => GetPresentStatusCompletion.PayloadData.PresentStateEnum.NotPresented,
                            CashManagementPresentStatus.PresentStateEnum.Presented => GetPresentStatusCompletion.PayloadData.PresentStateEnum.Presented,
                            _ => GetPresentStatusCompletion.PayloadData.PresentStateEnum.Unknown,
                        },
                        CashManagement.LastCashManagementPresentStatus[position].AdditionalBunches switch
                        {
                            CashManagementPresentStatus.AdditionalBunchesEnum.None => GetPresentStatusCompletion.PayloadData.AdditionalBunchesEnum.None,
                            CashManagementPresentStatus.AdditionalBunchesEnum.OneMore => GetPresentStatusCompletion.PayloadData.AdditionalBunchesEnum.OneMore,
                            _ => GetPresentStatusCompletion.PayloadData.AdditionalBunchesEnum.Unknown,
                        },
                        CashManagement.LastCashManagementPresentStatus[position].BunchesRemaining,
                        CashManagement.LastCashManagementPresentStatus[position].ReturnedItems.CopyTo(),
                        CashManagement.LastCashManagementPresentStatus[position].TotalReturnedItems.CopyTo(),
                        CashManagement.LastCashManagementPresentStatus[position].RemainingItems.CopyTo()),
                    MessageHeader.CompletionCodeEnum.Success)
                );
        }

        private ICashManagementService CashManagement { get => Provider.IsA<ICashManagementService>(); }
    }
}
