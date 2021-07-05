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
using XFS4IoTServer;
using XFS4IoT.Dispenser.Commands;
using XFS4IoT.Dispenser.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Dispenser
{
    public partial class CloseShutterHandler
    {
        private async Task<CloseShutterCompletion.PayloadData> HandleCloseShutter(ICloseShutterEvents events, CloseShutterCommand closeShutter, CancellationToken cancel)
        {
            CashDispenserCapabilitiesClass.OutputPositionEnum position = CashDispenserCapabilitiesClass.OutputPositionEnum.Default;
            if (closeShutter.Payload.Position is not null)
            {
                position = closeShutter.Payload.Position switch
                {
                    CloseShutterCommand.PayloadData.PositionEnum.Bottom => CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom,
                    CloseShutterCommand.PayloadData.PositionEnum.Center => CashDispenserCapabilitiesClass.OutputPositionEnum.Center,
                    CloseShutterCommand.PayloadData.PositionEnum.Default => CashDispenserCapabilitiesClass.OutputPositionEnum.Default,
                    CloseShutterCommand.PayloadData.PositionEnum.Front => CashDispenserCapabilitiesClass.OutputPositionEnum.Front,
                    CloseShutterCommand.PayloadData.PositionEnum.Left => CashDispenserCapabilitiesClass.OutputPositionEnum.Left,
                    CloseShutterCommand.PayloadData.PositionEnum.Rear => CashDispenserCapabilitiesClass.OutputPositionEnum.Rear,
                    CloseShutterCommand.PayloadData.PositionEnum.Right => CashDispenserCapabilitiesClass.OutputPositionEnum.Right,
                    CloseShutterCommand.PayloadData.PositionEnum.Top => CashDispenserCapabilitiesClass.OutputPositionEnum.Top,
                    _ => CashDispenserCapabilitiesClass.OutputPositionEnum.Default
                };
            }

            // Check the shutter capabilites
            if (!Dispenser.CashDispenserCapabilities.Shutter)
            {
                return new CloseShutterCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"The shutter command is not supported by the device.");
            }

            if (Dispenser.CashDispenserCapabilities.Shutter &&
                Dispenser.CashDispenserCapabilities.ShutterControl)
            {
                Logger.Log(Constants.Framework, "The application requested shutter command even if the device support implicit shutter control.");
            }

            Dispenser.CashDispenserCapabilities.OutputPositons.ContainsKey(position).IsTrue($"Unsupported position specified. {position}");

            if (!Dispenser.CashDispenserCapabilities.OutputPositons[position])
            {
                return new CloseShutterCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                              $"Specified unsupported position {position}",
                                                               CloseShutterCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition);
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
            return new CloseShutterCompletion.PayloadData(result.CompletionCode, 
                                                          result.ErrorDescription, 
                                                          errorCode);
        }
    }
}
