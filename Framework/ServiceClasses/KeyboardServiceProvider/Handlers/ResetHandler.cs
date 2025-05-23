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
using XFS4IoT.Keyboard.Commands;
using XFS4IoT.Keyboard.Completions;
using XFS4IoTFramework.KeyManagement;

namespace XFS4IoTFramework.Keyboard
{
    public partial class ResetHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandleReset(IResetEvents events, ResetCommand reset, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "KeyboardDev.ResetDevice()");

            var result = await Device.ResetDevice(cancel);

            Logger.Log(Constants.DeviceClass, $"KeyboardDev.ResetDevice() -> {result.CompletionCode}");

            if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
            {
                SecureKeyEntryStatusClass status = KeyManagement.GetSecureKeyEntryStatus();
                status.ResetSecureKeyBuffered();
            }

            return new(
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IKeyManagementService KeyManagement { get => Provider.IsA<IKeyManagementService>(); }
    }
}
