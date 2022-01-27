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
using XFS4IoT.VendorMode.Commands;
using XFS4IoT.VendorMode.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.VendorMode
{
    public partial class ExitModeAcknowledgeHandler
    {
        private async Task<ExitModeAcknowledgeCompletion.PayloadData> HandleExitModeAcknowledge(IExitModeAcknowledgeEvents events, ExitModeAcknowledgeCommand exitModeAcknowledge, CancellationToken cancel)
        {
            if (Common.VendorModeStatus.ServiceStatus != VendorModeStatusClass.ServiceStatusEnum.ExitPending)
            {
                return new ExitModeAcknowledgeCompletion.PayloadData(MessagePayload.CompletionCodeEnum.SequenceError, $"ExitModeAcknowledge command should be called after ExitModeRequest command. {Common.VendorModeStatus.ServiceStatus}");
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

                    if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                    {
                        Common.VendorModeStatus.ServiceStatus = VendorModeStatusClass.ServiceStatusEnum.Active;
                        return new ExitModeAcknowledgeCompletion.PayloadData(result.CompletionCode, result.ErrorDescription);
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

                await VendorMode.ModeExitedEvent();
                Common.VendorModeStatus.ServiceStatus = VendorModeStatusClass.ServiceStatusEnum.Inactive;
            }

            return new ExitModeAcknowledgeCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, string.Empty);
        }
    }
}
