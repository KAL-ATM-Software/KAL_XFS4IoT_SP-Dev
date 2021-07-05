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
    public partial class OpenShutterHandler
    {
        private async Task<OpenShutterCompletion.PayloadData> HandleOpenShutter(IOpenShutterEvents events, OpenShutterCommand openShutter, CancellationToken cancel)
        {
            CashDispenserCapabilitiesClass.OutputPositionEnum position = CashDispenserCapabilitiesClass.OutputPositionEnum.Default;
            if (openShutter.Payload.Position is not null)
            {
                position = openShutter.Payload.Position switch
                {
                    OpenShutterCommand.PayloadData.PositionEnum.Bottom => CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom,
                    OpenShutterCommand.PayloadData.PositionEnum.Center => CashDispenserCapabilitiesClass.OutputPositionEnum.Center,
                    OpenShutterCommand.PayloadData.PositionEnum.Default => CashDispenserCapabilitiesClass.OutputPositionEnum.Default,
                    OpenShutterCommand.PayloadData.PositionEnum.Front => CashDispenserCapabilitiesClass.OutputPositionEnum.Front,
                    OpenShutterCommand.PayloadData.PositionEnum.Left => CashDispenserCapabilitiesClass.OutputPositionEnum.Left,
                    OpenShutterCommand.PayloadData.PositionEnum.Rear => CashDispenserCapabilitiesClass.OutputPositionEnum.Rear,
                    OpenShutterCommand.PayloadData.PositionEnum.Right => CashDispenserCapabilitiesClass.OutputPositionEnum.Right,
                    OpenShutterCommand.PayloadData.PositionEnum.Top => CashDispenserCapabilitiesClass.OutputPositionEnum.Top,
                    _ => CashDispenserCapabilitiesClass.OutputPositionEnum.Default
                };
            }

            // Check the shutter capabilites
            if (!Dispenser.CashDispenserCapabilities.Shutter)
            {
                return new OpenShutterCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                             $"The shutter command is not supported by the device.");
            }

            if (Dispenser.CashDispenserCapabilities.Shutter &&
                Dispenser.CashDispenserCapabilities.ShutterControl)
            {
                Logger.Log(Constants.Framework, "The application requested shutter command even if the device support implicit shutter control.");
            }

            // Check supported position for this device
            Dispenser.CashDispenserCapabilities.OutputPositons.ContainsKey(position).IsTrue($"Unsupported position specified. {position}");

            if (!Dispenser.CashDispenserCapabilities.OutputPositons[position])
            {
                return new OpenShutterCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                             $"Specified unsupported position {position}",
                                                             OpenShutterCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition);
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.OpenCloseShutterAsync()");

            var result = await Device.OpenCloseShutterAsync(new OpenCloseShutterRequest(OpenCloseShutterRequest.ActionEnum.Open, position), cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.OpenCloseShutterAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            OpenShutterCompletion.PayloadData.ErrorCodeEnum? errorCode = null;
            if (result.ErrorCode is not null)
            {
                errorCode = result.ErrorCode switch
                {
                    OpenCloseShutterResult.ErrorCodeEnum.ExchangeActive => OpenShutterCompletion.PayloadData.ErrorCodeEnum.ExchangeActive,
                    OpenCloseShutterResult.ErrorCodeEnum.ShutterOpen => OpenShutterCompletion.PayloadData.ErrorCodeEnum.ShutterOpen,
                    OpenCloseShutterResult.ErrorCodeEnum.ShutterNotOpen => OpenShutterCompletion.PayloadData.ErrorCodeEnum.ShutterNotOpen,
                    OpenCloseShutterResult.ErrorCodeEnum.UnsupportedPosition => OpenShutterCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition,
                    _ => OpenShutterCompletion.PayloadData.ErrorCodeEnum.ShutterNotOpen,
                };
            }
            return new OpenShutterCompletion.PayloadData(result.CompletionCode, 
                                                         result.ErrorDescription,
                                                         errorCode);
        }
    }
}
