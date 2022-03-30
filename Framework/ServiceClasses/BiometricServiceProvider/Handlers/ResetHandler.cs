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

        private async Task<ResetCompletion.PayloadData> HandleReset(IResetEvents events, ResetCommand reset, CancellationToken cancel)
        {
            if (Common.BiometricCapabilities.ClearData == BiometricCapabilitiesClass.ClearModesEnum.None)
                return new ResetCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedCommand, "Reset is not supported by this device.");

            BiometricCapabilitiesClass.ClearModesEnum clearMode;

            if (reset?.Payload?.ClearData is null)
                clearMode = Common.BiometricCapabilities.ClearData;

            else
            {
                clearMode = reset.Payload.ClearData switch
                {
                    XFS4IoT.Biometric.ClearDataEnum.ScannedData => BiometricCapabilitiesClass.ClearModesEnum.ScannedData,
                    XFS4IoT.Biometric.ClearDataEnum.ImportedData => BiometricCapabilitiesClass.ClearModesEnum.ImportedData,
                    XFS4IoT.Biometric.ClearDataEnum.SetMatchedData => BiometricCapabilitiesClass.ClearModesEnum.SetMatchedData,
                    _ => throw new InvalidDataException("Invalid ClearData type supplied within Biometric.Reset handler.")
                };

                if (!Common.BiometricCapabilities.ClearData.HasFlag(clearMode))
                    return new ResetCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "Unsupported ClearData specified in Reset payload.");
            }


            Logger.Log(Constants.DeviceClass, "BiometricDev.ResetDeviceAsync()");
            var result = await Device.ResetDeviceAsync(new ClearDataRequest(clearMode), cancel);
            Logger.Log(Constants.DeviceClass, $"BiometricDev.ResetDeviceAsync() -> {result.CompletionCode}");

            // Send DataClearedEvent
            if (result.CompletionCode == XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.Success)
            {
                if (clearMode.HasFlag(BiometricCapabilitiesClass.ClearModesEnum.ScannedData))
                    await Biometric.DataClearedEvent(BiometricCapabilitiesClass.ClearModesEnum.ScannedData);

                if (clearMode.HasFlag(BiometricCapabilitiesClass.ClearModesEnum.ImportedData))
                    await Biometric.DataClearedEvent(BiometricCapabilitiesClass.ClearModesEnum.ImportedData);

                if (clearMode.HasFlag(BiometricCapabilitiesClass.ClearModesEnum.SetMatchedData))
                    await Biometric.DataClearedEvent(BiometricCapabilitiesClass.ClearModesEnum.SetMatchedData);
            }

            return new ResetCompletion.PayloadData(result.CompletionCode,
                                                   result.ErrorDescription);
        }

    }
}
