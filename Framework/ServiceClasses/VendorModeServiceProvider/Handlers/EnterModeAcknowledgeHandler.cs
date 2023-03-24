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
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.VendorMode
{
    public partial class EnterModeAcknowledgeHandler
    {
        private async Task<EnterModeAcknowledgeCompletion.PayloadData> HandleEnterModeAcknowledge(IEnterModeAcknowledgeEvents events, EnterModeAcknowledgeCommand enterModeAcknowledge, CancellationToken cancel)
        {
            if (Common.VendorModeStatus.ServiceStatus != VendorModeStatusClass.ServiceStatusEnum.EnterPending)
            {
                return new EnterModeAcknowledgeCompletion.PayloadData(MessagePayload.CompletionCodeEnum.SequenceError, $"EnterModeAcknowledge command should be called after EnterModeRequest command. {Common.VendorModeStatus.ServiceStatus}");
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
                    Logger.Log(Constants.DeviceClass, "VendorModeDev.EnterVendorMode()");

                    var result = await Device.EnterVendorMode(cancel);

                    Logger.Log(Constants.DeviceClass, $"VendorModeDev.EnterVendorMode() -> {result.CompletionCode}");

                    if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                    {
                        Common.VendorModeStatus.ServiceStatus = VendorModeStatusClass.ServiceStatusEnum.Inactive;
                        return new EnterModeAcknowledgeCompletion.PayloadData(result.CompletionCode, result.ErrorDescription);
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

            return new EnterModeAcknowledgeCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, string.Empty);
        }

        public async Task CommandPostProcessing(object enterAckResult)
        {
            EnterModeAcknowledgeCompletion.PayloadData result = enterAckResult as EnterModeAcknowledgeCompletion.PayloadData;

            if (result.IsNotNull().CompletionCode == MessagePayload.CompletionCodeEnum.Success &&
                Common.VendorModeStatus.ServiceStatus == VendorModeStatusClass.ServiceStatusEnum.EnterPending && 
                VendorMode.PendingAcknowledge.Count == 0)
            {
                Common.VendorModeStatus.ServiceStatus = VendorModeStatusClass.ServiceStatusEnum.Active;
                await VendorMode.ModeEnteredEvent();
            }
        }
    }
}
