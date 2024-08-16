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
using XFS4IoT.Biometric.Commands;
using XFS4IoT.Biometric.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Biometric
{
    public partial class ResetHandler
    {

        private async Task<CommandResult<MessagePayloadBase>> HandleReset(IResetEvents events, ResetCommand reset, CancellationToken cancel)
        {
            if (Common.BiometricCapabilities.ClearData == BiometricCapabilitiesClass.ClearModesEnum.None)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.UnsupportedCommand, 
                    "Reset is not supported by this device.");
            }

            BiometricCapabilitiesClass.ClearModesEnum clearMode;

            if (reset?.Payload?.ClearData is null)
            {
                clearMode = Common.BiometricCapabilities.ClearData;
            }
            else
            {
                clearMode = reset.Payload.ClearData switch
                {
                    ResetCommand.PayloadData.ClearDataEnum.ScannedData => BiometricCapabilitiesClass.ClearModesEnum.ScannedData,
                    ResetCommand.PayloadData.ClearDataEnum.ImportedData => BiometricCapabilitiesClass.ClearModesEnum.ImportedData,
                    ResetCommand.PayloadData.ClearDataEnum.SetMatchedData => BiometricCapabilitiesClass.ClearModesEnum.SetMatchedData,
                    _ => throw new InvalidDataException("Invalid ClearData type supplied within Biometric.Reset handler.")
                };

                if (!Common.BiometricCapabilities.ClearData.HasFlag(clearMode))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "Unsupported ClearData specified in Reset payload.");
                }
            }

            Logger.Log(Constants.DeviceClass, "BiometricDev.ResetDeviceAsync()");
            var result = await Device.ResetDeviceAsync(new ClearDataRequest(clearMode), cancel);
            Logger.Log(Constants.DeviceClass, $"BiometricDev.ResetDeviceAsync() -> {result.CompletionCode}");

            // Send DataClearedEvent
            if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
            {
                if (clearMode.HasFlag(BiometricCapabilitiesClass.ClearModesEnum.ScannedData))
                    await Biometric.DataClearedEvent(BiometricCapabilitiesClass.ClearModesEnum.ScannedData);

                if (clearMode.HasFlag(BiometricCapabilitiesClass.ClearModesEnum.ImportedData))
                    await Biometric.DataClearedEvent(BiometricCapabilitiesClass.ClearModesEnum.ImportedData);

                if (clearMode.HasFlag(BiometricCapabilitiesClass.ClearModesEnum.SetMatchedData))
                    await Biometric.DataClearedEvent(BiometricCapabilitiesClass.ClearModesEnum.SetMatchedData);
            }

            return new(
                result.CompletionCode,
                result.ErrorDescription);
        }

    }
}
