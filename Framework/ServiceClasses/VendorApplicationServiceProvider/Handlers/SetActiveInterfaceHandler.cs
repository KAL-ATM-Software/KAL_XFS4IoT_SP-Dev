/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.Completions;
using XFS4IoT.VendorApplication.Commands;
using XFS4IoT.VendorApplication.Completions;

namespace XFS4IoTFramework.VendorApplication
{
    public partial class SetActiveInterfaceHandler
    {

        private async Task<SetActiveInterfaceCompletion.PayloadData> HandleSetActiveInterface(ISetActiveInterfaceEvents events, SetActiveInterfaceCommand setActiveInterface, CancellationToken cancel)
        {
            // Supported active interfaces in capabilites are missing in the specification

            ActiveInterfaceEnum requestedInterface = setActiveInterface.Payload.ActiveInterface switch
            {
                SetActiveInterfaceCommand.PayloadData.ActiveInterfaceEnum.Consumer => ActiveInterfaceEnum.Consumer,
                _ => ActiveInterfaceEnum.Operator,
            };

            Logger.Log(Constants.DeviceClass, "VendorApplicationDev.GetActiveInterface()");
            var current = Device.GetActiveInterface();

            Logger.Log(Constants.DeviceClass, $"VendorApplicationDev.GetActiveInterface() -> {current.CompletionCode}");

            if (current.ActiveInterface == requestedInterface)
            {
                // No need to change active interface
                return new SetActiveInterfaceCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, string.Empty);
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.SetActiveInterface()");
            var result = await Device.SetActiveInterface(new SetActiveInterfaceRequest(requestedInterface),
                                                         cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.SetActiveInterface() -> {result.CompletionCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                await VendorApplication.InterfaceChangedEvent(requestedInterface);
            }

            return new SetActiveInterfaceCompletion.PayloadData(result.CompletionCode,
                                                                result.ErrorDescription);
        }
    }
}
