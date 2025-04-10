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
using XFS4IoTServer;
using XFS4IoT.VendorApplication.Commands;
using XFS4IoT.VendorApplication.Completions;

namespace XFS4IoTFramework.VendorApplication
{
    public partial class GetActiveInterfaceHandler
    {
        private Task<CommandResult<GetActiveInterfaceCompletion.PayloadData>> HandleGetActiveInterface(IGetActiveInterfaceEvents events, GetActiveInterfaceCommand getActiveInterface, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "VendorApplicationDev.GetActiveInterface()");
            var result = Device.GetActiveInterface();

            Logger.Log(Constants.DeviceClass, $"VendorApplicationDev.GetActiveInterface() -> {result.CompletionCode}");

            return Task.FromResult(
                new CommandResult<GetActiveInterfaceCompletion.PayloadData>(
                    result.ActiveInterface is null ? 
                    null : 
                    new(
                        result.ActiveInterface switch
                        {
                            ActiveInterfaceEnum.Consumer => GetActiveInterfaceCompletion.PayloadData.ActiveInterfaceEnum.Consumer,
                            ActiveInterfaceEnum.Operator => GetActiveInterfaceCompletion.PayloadData.ActiveInterfaceEnum.Operator,
                            _ => throw new InternalErrorException($"Unsupported active interface is set. {result.ActiveInterface}"),
                        }),
                    result.CompletionCode,
                    result.ErrorDescription)
                );
        }
    }
}
