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
using XFS4IoT.Completions;
using XFS4IoT.Common;
using XFS4IoT.CashManagement;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashManagement
{
    public partial class CloseShutterHandler
    {
        private async Task<CommandResult<CloseShutterCompletion.PayloadData>> HandleCloseShutter(ICloseShutterEvents events, CloseShutterCommand closeShutter, CancellationToken cancel)
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
            if (Common.CashManagementCapabilities.ShutterControl)
            {
                Logger.Log(Constants.Framework, "The application requested shutter command even if the device support implicit shutter control.");
            }

            if (position == CashManagementCapabilitiesClass.PositionEnum.NotSupported ||
                !Common.CashManagementCapabilities.Positions.HasFlag(position))
            {
                return new(
                    new(CloseShutterCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified unsupported position {position}");
            }

            if (Common.CashDispenserCapabilities is not null &&
                position != CashManagementCapabilitiesClass.PositionEnum.InBottom &&
                position != CashManagementCapabilitiesClass.PositionEnum.InCenter &&
                position != CashManagementCapabilitiesClass.PositionEnum.InDefault &&
                position != CashManagementCapabilitiesClass.PositionEnum.InFront &&
                position != CashManagementCapabilitiesClass.PositionEnum.InLeft &&
                position != CashManagementCapabilitiesClass.PositionEnum.InRear &&
                position != CashManagementCapabilitiesClass.PositionEnum.InRight &&
                position != CashManagementCapabilitiesClass.PositionEnum.InTop)
            {
                CashManagementCapabilitiesClass.OutputPositionEnum outPos = position switch
                {
                    CashManagementCapabilitiesClass.PositionEnum.OutBottom => CashManagementCapabilitiesClass.OutputPositionEnum.Bottom,
                    CashManagementCapabilitiesClass.PositionEnum.OutCenter => CashManagementCapabilitiesClass.OutputPositionEnum.Center,
                    CashManagementCapabilitiesClass.PositionEnum.OutFront => CashManagementCapabilitiesClass.OutputPositionEnum.Front,
                    CashManagementCapabilitiesClass.PositionEnum.OutLeft => CashManagementCapabilitiesClass.OutputPositionEnum.Left,
                    CashManagementCapabilitiesClass.PositionEnum.OutRear => CashManagementCapabilitiesClass.OutputPositionEnum.Rear,
                    CashManagementCapabilitiesClass.PositionEnum.OutRight => CashManagementCapabilitiesClass.OutputPositionEnum.Right,
                    CashManagementCapabilitiesClass.PositionEnum.OutTop => CashManagementCapabilitiesClass.OutputPositionEnum.Top,
                    _ => CashManagementCapabilitiesClass.OutputPositionEnum.Default,
                };

                if (Common.CashDispenserStatus.Positions[outPos].Shutter == CashManagementStatusClass.ShutterEnum.Closed)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.Success,
                        $"The shutter is already closed.");
                }
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.OpenCloseShutterAsync()");

            var result = await Device.OpenCloseShutterAsync(new OpenCloseShutterRequest(OpenCloseShutterRequest.ActionEnum.Close, position), cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.OpenCloseShutterAsync() -> {result.CompletionCode}, {result.ErrorCode}");

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
            return new(
                errorCode is not null ? new(errorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
