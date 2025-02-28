/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
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
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class DeviceLockControlHandler
    {
        private async Task<CommandResult<DeviceLockControlCompletion.PayloadData>> HandleDeviceLockControl(IDeviceLockControlEvents events, DeviceLockControlCommand deviceLockControl, CancellationToken cancel)
        {
            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new(
                    new(DeviceLockControlCompletion.PayloadData.ErrorCodeEnum.ExchangeActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The exchange state is already in active.");
            }

            if (CashAcceptor.CashInStatus.Status == CashInStatusClass.StatusEnum.Active)
            {
                return new(
                    new(DeviceLockControlCompletion.PayloadData.ErrorCodeEnum.CashInActive),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The cash-in state is in active. {CashAcceptor.CashInStatus.Status}");
            }

            if ((deviceLockControl.Payload.DeviceAction is null &&
                 deviceLockControl.Payload.CashUnitAction is null) ||
                (deviceLockControl.Payload.DeviceAction == DeviceLockControlCommand.PayloadData.DeviceActionEnum.NoLockAction &&
                 deviceLockControl.Payload.CashUnitAction == DeviceLockControlCommand.PayloadData.CashUnitActionEnum.NoLockAction))
            {
                return new(MessageHeader.CompletionCodeEnum.Success);
            }

            if (deviceLockControl.Payload.CashUnitAction == DeviceLockControlCommand.PayloadData.CashUnitActionEnum.LockIndividual &&
                (deviceLockControl.Payload.UnitLockControl is null ||
                 deviceLockControl.Payload.UnitLockControl.Count == 0))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Specified cash unit action is {deviceLockControl.Payload.CashUnitAction}, but no cash unit specific action being specified.");
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.DeviceLockControl()");

            var result = await Device.DeviceLockControl(
                new DeviceLockRequest(
                    deviceLockControl.Payload.DeviceAction switch
                    {
                        DeviceLockControlCommand.PayloadData.DeviceActionEnum.Lock => DeviceLockRequest.DeviceActionEnum.Lock,
                        DeviceLockControlCommand.PayloadData.DeviceActionEnum.Unlock => DeviceLockRequest.DeviceActionEnum.Unlock,
                        _ => DeviceLockRequest.DeviceActionEnum.NoLockAction,
                    },
                    deviceLockControl.Payload.CashUnitAction switch
                    {
                        DeviceLockControlCommand.PayloadData.CashUnitActionEnum.LockAll => DeviceLockRequest.CashUnitActionEnum.LockAll,
                        DeviceLockControlCommand.PayloadData.CashUnitActionEnum.LockIndividual => DeviceLockRequest.CashUnitActionEnum.LockIndividual,
                        DeviceLockControlCommand.PayloadData.CashUnitActionEnum.UnlockAll => DeviceLockRequest.CashUnitActionEnum.UnlockAll,
                        _ => DeviceLockRequest.CashUnitActionEnum.NoLockAction,
                    },
                    deviceLockControl.Payload.CashUnitAction == DeviceLockControlCommand.PayloadData.CashUnitActionEnum.LockIndividual ? 
                    deviceLockControl.Payload.UnitLockControl.ToDictionary(c => c.StorageUnit, c => c.UnitAction switch
                    {
                        DeviceLockControlCommand.PayloadData.UnitLockControlClass.UnitActionEnum.Lock => DeviceLockRequest.UnitActionEnum.Lock,
                        _ => DeviceLockRequest.UnitActionEnum.Unlock
                    }) : null),
                cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.DeviceLockControl() -> {result.CompletionCode}, {result.ErrorCode}");

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
