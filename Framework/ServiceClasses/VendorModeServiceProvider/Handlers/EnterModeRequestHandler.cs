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
    public partial class EnterModeRequestHandler
    { 
        private async Task<CommandResult<MessagePayloadBase>> HandleEnterModeRequest(IEnterModeRequestEvents events, EnterModeRequestCommand enterModeRequest, CancellationToken cancel)
        {
            if (Common.VendorModeStatus.ServiceStatus != VendorModeStatusClass.ServiceStatusEnum.Inactive)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.SequenceError, 
                    $"EnterModeRequest command should be called when vendor mode is inactive. {Common.VendorModeStatus.ServiceStatus}");
            }

            if (VendorMode.RegisteredClients.Count > 0)
            {
                // Broadcast ExitModeRequestEvent to registered clients
                await VendorMode.BroadcastEnterModeRequestEvent();
            }
            else
            {
                // No clients registered, just go directly to the vendor mode
                try
                {
                    Logger.Log(Constants.DeviceClass, "VendorModeDev.EnterVendorMode()");

                    var result = await Device.EnterVendorMode(cancel);

                    Logger.Log(Constants.DeviceClass, $"VendorModeDev.EnterVendorMode() -> {result.CompletionCode}");

                    if (result.CompletionCode != MessageHeader.CompletionCodeEnum.Success)
                    {
                        Common.VendorModeStatus.ServiceStatus = VendorModeStatusClass.ServiceStatusEnum.Inactive;
                        return new(
                            result.CompletionCode, 
                            result.ErrorDescription);
                    }
                }
                catch (NotImplementedException)
                {
                    Logger.Log(Constants.DeviceClass, $"VendorModeDev.EnterVendorMode() -> Not implemented");
                }
                catch (Exception)
                {
                    Common.VendorModeStatus.ServiceStatus = VendorModeStatusClass.ServiceStatusEnum.Inactive;
                    throw;
                }
            }
            
            // Broadcast EnterModeRequestEvent to registered clients
            return new(MessageHeader.CompletionCodeEnum.Success);
        }
        public async Task CommandPostProcessing(object enterModeResult)
        {
            CommandResult<MessagePayloadBase> result = enterModeResult as CommandResult<MessagePayloadBase>;

            if (result.IsNotNull().CompletionCode == MessageHeader.CompletionCodeEnum.Success &&
                Common.VendorModeStatus.ServiceStatus == VendorModeStatusClass.ServiceStatusEnum.EnterPending && 
                VendorMode.PendingAcknowledge.Count == 0)
            {
                Common.VendorModeStatus.ServiceStatus = VendorModeStatusClass.ServiceStatusEnum.Active;
                await VendorMode.ModeEnteredEvent();
            }
        }
    }
}
