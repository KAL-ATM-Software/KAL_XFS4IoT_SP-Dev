/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.VendorApplication.Commands;
using XFS4IoT.VendorApplication.Completions;
using XFS4IoTFramework.Common;
using XFS4IoT;

namespace XFS4IoTFramework.VendorApplication
{
    public partial class StartLocalApplicationHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandleStartLocalApplication(IStartLocalApplicationEvents events, StartLocalApplicationCommand startLocalApplication, CancellationToken cancel)
        {
            if (startLocalApplication.Payload.AccessLevel is not null)
            {
                if (startLocalApplication.Payload.AccessLevel == StartLocalApplicationCommand.PayloadData.AccessLevelEnum.Basic &&
                    !Common.VendorApplicationCapabilities.SupportedAccessLevels.HasFlag(VendorApplicationCapabilitiesClass.SupportedAccessLevelEnum.Basic) ||
                    startLocalApplication.Payload.AccessLevel == StartLocalApplicationCommand.PayloadData.AccessLevelEnum.Intermediate &&
                    !Common.VendorApplicationCapabilities.SupportedAccessLevels.HasFlag(VendorApplicationCapabilitiesClass.SupportedAccessLevelEnum.Intermediate) ||
                    startLocalApplication.Payload.AccessLevel == StartLocalApplicationCommand.PayloadData.AccessLevelEnum.Full &&
                    !Common.VendorApplicationCapabilities.SupportedAccessLevels.HasFlag(VendorApplicationCapabilitiesClass.SupportedAccessLevelEnum.Full))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified access level is not supported by the device. {startLocalApplication.Payload.AccessLevel}");
                }
            }

            Logger.Log(Constants.DeviceClass, "VendorApplicationDev.StartLocalApplicationRequest()");
            var result = await Device.StartLocalApplication(new StartLocalApplicationRequest(startLocalApplication.Payload.AppName,
                                                                                             startLocalApplication.Payload.AccessLevel switch
                                                                                             {
                                                                                                 StartLocalApplicationCommand.PayloadData.AccessLevelEnum.Basic => StartLocalApplicationRequest.AccessLevelEnum.Basic,
                                                                                                 StartLocalApplicationCommand.PayloadData.AccessLevelEnum.Intermediate => StartLocalApplicationRequest.AccessLevelEnum.Intermediate,
                                                                                                 StartLocalApplicationCommand.PayloadData.AccessLevelEnum.Full => StartLocalApplicationRequest.AccessLevelEnum.Full,
                                                                                                 _ => null,
                                                                                             }),
                                                            cancel);
            Logger.Log(Constants.DeviceClass, $"VendorApplicationDev.StartLocalApplicationRequest() -> {result.CompletionCode}");

            if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success &&
                Common.VendorApplicationCapabilities.SupportedAccessLevels != VendorApplicationCapabilitiesClass.SupportedAccessLevelEnum.NotSupported)
            {
                if (startLocalApplication.Payload.AccessLevel is not null)
                {
                    Common.VendorApplicationStatus.AccessLevel = startLocalApplication.Payload.AccessLevel switch
                    {
                        StartLocalApplicationCommand.PayloadData.AccessLevelEnum.Basic => VendorApplicationStatusClass.AccessLevelEnum.Basic,
                        StartLocalApplicationCommand.PayloadData.AccessLevelEnum.Intermediate => VendorApplicationStatusClass.AccessLevelEnum.Intermediate,
                        _ => VendorApplicationStatusClass.AccessLevelEnum.Full,
                    };
                }
                else
                {
                    Common.VendorApplicationStatus.AccessLevel = VendorApplicationStatusClass.AccessLevelEnum.NotActive;
                }
            }

            return new(
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
