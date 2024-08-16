/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoT.VendorMode.Commands;
using XFS4IoT.VendorMode.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.VendorMode
{
    public partial class ExitModeRequestHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandleExitModeRequest(IExitModeRequestEvents events, ExitModeRequestCommand exitModeRequest, CancellationToken cancel)
        {
            if (Common.VendorModeStatus.ServiceStatus != VendorModeStatusClass.ServiceStatusEnum.Active)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.SequenceError, 
                    $"ExitModeRequest command should be called when vendor mode is active. {Common.VendorModeStatus.ServiceStatus}");
            }

            if (VendorMode.RegisteredClients.Count > 0)
            {
                // Broadcast ExitModeRequestEvent to registered clients
                await VendorMode.BroadcastExitModeRequestEvent();
            }
            else
            {
                // No clients registered and exit directly
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

        public async Task CommandPostProcessing(object exitModeResult)
        {
            CommandResult<MessagePayloadBase> result = exitModeResult as CommandResult<MessagePayloadBase>;

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
