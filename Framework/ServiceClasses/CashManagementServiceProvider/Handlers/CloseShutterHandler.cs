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
using XFS4IoT.Completions;
using XFS4IoT.Common;
using XFS4IoT.CashManagement;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoTFramework.Common;
using XFS4IoT.CashManagement.Events;

namespace XFS4IoTFramework.CashManagement
{
    public partial class CloseShutterHandler
    {
        private async Task<CloseShutterCompletion.PayloadData> HandleCloseShutter(ICloseShutterEvents events, CloseShutterCommand closeShutter, CancellationToken cancel)
        {
            CashManagementCapabilitiesClass.PositionEnum position = CashManagementCapabilitiesClass.PositionEnum.NotSupported;
            if (closeShutter.Payload.Position is not null)
            {
                position = closeShutter.Payload.Position switch
                {
                    PositionEnum.OutBottom => CashManagementCapabilitiesClass.PositionEnum.OutBottom,
                    PositionEnum.OutCenter => CashManagementCapabilitiesClass.PositionEnum.OutCenter,
                    PositionEnum.OutDefault => CashManagementCapabilitiesClass.PositionEnum.OutDefault,
                    PositionEnum.OutFront => CashManagementCapabilitiesClass.PositionEnum.OutFront,
                    PositionEnum.OutLeft => CashManagementCapabilitiesClass.PositionEnum.OutLeft,
                    PositionEnum.OutRear => CashManagementCapabilitiesClass.PositionEnum.OutRear,
                    PositionEnum.OutRight => CashManagementCapabilitiesClass.PositionEnum.OutRight,
                    PositionEnum.OutTop => CashManagementCapabilitiesClass.PositionEnum.OutTop,
                    PositionEnum.InBottom => CashManagementCapabilitiesClass.PositionEnum.InBottom,
                    PositionEnum.InCenter => CashManagementCapabilitiesClass.PositionEnum.InCenter,
                    PositionEnum.InDefault => CashManagementCapabilitiesClass.PositionEnum.InDefault,
                    PositionEnum.InFront => CashManagementCapabilitiesClass.PositionEnum.InFront,
                    PositionEnum.InLeft => CashManagementCapabilitiesClass.PositionEnum.InLeft,
                    PositionEnum.InRear => CashManagementCapabilitiesClass.PositionEnum.InRear,
                    PositionEnum.InRight => CashManagementCapabilitiesClass.PositionEnum.InRight,
                    PositionEnum.InTop => CashManagementCapabilitiesClass.PositionEnum.InTop,
                    _ => CashManagementCapabilitiesClass.PositionEnum.NotSupported,
                };
            }

            // Check the shutter capabilites
            if (CashManagement.CashManagementCapabilities.ShutterControl)
            {
                Logger.Log(Constants.Framework, "The application requested shutter command even if the device support implicit shutter control.");
            }

            if (position == CashManagementCapabilitiesClass.PositionEnum.NotSupported ||
                !CashManagement.CashManagementCapabilities.Positions.HasFlag(position))
            {
                return new CloseShutterCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                              $"Specified unsupported position {position}",
                                                               CloseShutterCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition);
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.OpenCloseShutterAsync()");

            var result = await Device.OpenCloseShutterAsync(new OpenCloseShutterRequest(OpenCloseShutterRequest.ActionEnum.Close, position), cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.OpenCloseShutterAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                await events.ShutterStatusChangedEvent(new ShutterStatusChangedEvent.PayloadData(closeShutter.Payload.Position,
                                                                                                 ShutterEnum.Closed));
            }
            else if (result.CompletionCode == MessagePayload.CompletionCodeEnum.HardwareError)
            {
                await events.ShutterStatusChangedEvent(new ShutterStatusChangedEvent.PayloadData(closeShutter.Payload.Position,
                                                                                                 result.Jammed ? ShutterEnum.Jammed : ShutterEnum.Unknown));
            }

            CloseShutterCompletion.PayloadData.ErrorCodeEnum? errorCode = null;
            if (result.ErrorCode is not null)
            {
                errorCode = result.ErrorCode switch
                {
                    OpenCloseShutterResult.ErrorCodeEnum.ExchangeActive => CloseShutterCompletion.PayloadData.ErrorCodeEnum.ExchangeActive,
                    OpenCloseShutterResult.ErrorCodeEnum.ShutterClosed => CloseShutterCompletion.PayloadData.ErrorCodeEnum.ShutterClosed,
                    OpenCloseShutterResult.ErrorCodeEnum.ShutterNotClosed => CloseShutterCompletion.PayloadData.ErrorCodeEnum.ShutterNotClosed,
                    OpenCloseShutterResult.ErrorCodeEnum.UnsupportedPosition => CloseShutterCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition,
                    _ => CloseShutterCompletion.PayloadData.ErrorCodeEnum.ShutterNotClosed,
                };
            }
            return new CloseShutterCompletion.PayloadData(result.CompletionCode,
                                                          result.ErrorDescription,
                                                          errorCode);
        }
    }
}
