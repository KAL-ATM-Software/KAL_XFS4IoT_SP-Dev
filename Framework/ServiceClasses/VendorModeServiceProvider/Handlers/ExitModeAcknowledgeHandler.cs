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
using XFS4IoT.VendorMode.Commands;
using XFS4IoT.VendorMode.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.VendorMode
{
    public partial class ExitModeAcknowledgeHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandleExitModeAcknowledge(IExitModeAcknowledgeEvents events, ExitModeAcknowledgeCommand exitModeAcknowledge, CancellationToken cancel)
        {
            if (Common.VendorModeStatus.ServiceStatus != VendorModeStatusClass.ServiceStatusEnum.ExitPending)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.SequenceError, 
                    $"ExitModeAcknowledge command should be called after ExitModeRequest command. {Common.VendorModeStatus.ServiceStatus}");
            }

            // Check all registered clients are acknowledged. if all registered clients are acknkowledged, send ModeExited event.
            foreach (IConnection c in VendorMode.PendingAcknowledge)
            {
                if (c == Connection)
                {
                    VendorMode.PendingAcknowledge.Remove(c);
                    break;
                }
            }

            if (VendorMode.PendingAcknowledge.Count == 0)
            {
                try
                {
                    Logger.Log(Constants.DeviceClass, "VendorModeDev.ExitVendorMode()");

                    var result = await Device.ExitVendorMode(cancel);

                    Logger.Log(Constants.DeviceClass, $"VendorModeDev.ExitVendorMode() -> {result.CompletionCode}");

                    if (result.CompletionCode != MessageHeader.CompletionCodeEnum.Success)
                    {
                        Common.VendorModeStatus.ServiceStatus = VendorModeStatusClass.ServiceStatusEnum.Active;
                        return new(
                            result.CompletionCode, 
                            result.ErrorDescription);
                    }
                }
                catch (NotImplementedException)
                {
                    Logger.Log(Constants.DeviceClass, $"VendorModeDev.ExitVendorMode() -> Not implemented");
                }
                catch (Exception)
                {
                    Common.VendorModeStatus.ServiceStatus = VendorModeStatusClass.ServiceStatusEnum.Active;
                    throw;
                }
            }

            return new(MessageHeader.CompletionCodeEnum.Success);
        }

        public async Task CommandPostProcessing(object exitAckResult)
        {
            CommandResult<MessagePayloadBase> result = exitAckResult as CommandResult<MessagePayloadBase>;

            if (result.IsNotNull().CompletionCode == MessageHeader.CompletionCodeEnum.Success &&
                Common.VendorModeStatus.ServiceStatus == VendorModeStatusClass.ServiceStatusEnum.ExitPending &&
                VendorMode.PendingAcknowledge.Count == 0)
            {
                Common.VendorModeStatus.ServiceStatus = VendorModeStatusClass.ServiceStatusEnum.Inactive;
                await VendorMode.ModeExitedEvent();
            }
        }
    }
}
