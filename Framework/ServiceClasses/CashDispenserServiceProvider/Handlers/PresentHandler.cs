/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoT.CashManagement;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using XFS4IoT.Completions;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashDispenser
{
    public partial class PresentHandler
    {
        private async Task<CommandResult<PresentCompletion.PayloadData>> HandlePresent(IPresentEvents events, PresentCommand present, CancellationToken cancel)
        {
            CashManagementCapabilitiesClass.OutputPositionEnum position = CashManagementCapabilitiesClass.OutputPositionEnum.Default;
            if (present.Payload?.Position is not null)
            {
                position = present.Payload.Position switch
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

            if (position == CashManagementCapabilitiesClass.OutputPositionEnum.NotSupported)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Invalid position specified. {position}");
            }

            if (!Common.CashDispenserCapabilities.OutputPositions.HasFlag(position))
            {
                return new(
                    new(PresentCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Unsupported position specified. {position}");
            }
                
            Logger.Log(Constants.DeviceClass, "CashDispenserDev.PresentCashAsync()");

            var result = await Device.PresentCashAsync(new PresentCashCommandEvents(events), new PresentCashRequest(position), cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.PresentCashAsync() -> {result.CompletionCode}, {result.ErrorCode}");


            CashDispenserPresentStatus presentStatus = null;
            try
            {
                Logger.Log(Constants.DeviceClass, "CashDispenserDev.GetPresentStatus()");

                presentStatus = Device.GetPresentStatus(position);

                Logger.Log(Constants.DeviceClass, $"CashDispenserDev.GetPresentStatus() -> {presentStatus}");
            }
            catch (NotImplementedException)
            {
                Logger.Log(Constants.DeviceClass, $"CashDispenserDev.GetPresentStatus() -> Not implemented");
            }
            catch (Exception)
            {
                throw;
            }

            // Update an internal present status
            CashDispenser.LastCashDispenserPresentStatus[position].Status = result.CompletionCode switch
            {
                MessageHeader.CompletionCodeEnum.Success => CashDispenserPresentStatus.PresentStatusEnum.Presented,
                _ => CashDispenserPresentStatus.PresentStatusEnum.Unknown
            };

            if (presentStatus is not null)
            {
                CashDispenser.LastCashDispenserPresentStatus[position].Status = (CashDispenserPresentStatus.PresentStatusEnum)presentStatus.Status;

                if (presentStatus.LastDenomination is not null)
                    CashDispenser.LastCashDispenserPresentStatus[position].LastDenomination = new(presentStatus.LastDenomination.CurrencyAmounts, presentStatus.LastDenomination.Values);

                CashDispenser.LastCashDispenserPresentStatus[position].DispenseToken = presentStatus.DispenseToken;
            }

            CashDispenser.StoreCashDispenserPresentStatus();

            await Storage.UpdateCashAccounting(result.MovementResult);

            PositionEnum resPostion = position switch
            {
                CashManagementCapabilitiesClass.OutputPositionEnum.Bottom => PositionEnum.OutBottom,
                CashManagementCapabilitiesClass.OutputPositionEnum.Center => PositionEnum.OutCenter,
                CashManagementCapabilitiesClass.OutputPositionEnum.Front => PositionEnum.OutFront,
                CashManagementCapabilitiesClass.OutputPositionEnum.Left => PositionEnum.OutLeft,
                CashManagementCapabilitiesClass.OutputPositionEnum.Rear => PositionEnum.OutRear,
                CashManagementCapabilitiesClass.OutputPositionEnum.Right => PositionEnum.OutRight,
                CashManagementCapabilitiesClass.OutputPositionEnum.Top => PositionEnum.OutTop,
                _ => PositionEnum.OutDefault
            };

            PositionInfoNullableClass positionInfo = new(resPostion, result.NumBunchesRemaining < 0 ? "unknown" : result.NumBunchesRemaining.ToString());

            return new(
                new(
                    result.ErrorCode,
                    positionInfo),
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
