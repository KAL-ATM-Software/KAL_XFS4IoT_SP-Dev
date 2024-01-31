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
    public partial class ClearHandler
    {

        private async Task<ClearCompletion.PayloadData> HandleClear(IClearEvents events, ClearCommand clear, CancellationToken cancel)
        {
            if (Common.BiometricCapabilities.ClearData == BiometricCapabilitiesClass.ClearModesEnum.None)
                return new ClearCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedCommand, "Clear is not supported by this device.");

            BiometricCapabilitiesClass.ClearModesEnum clearMode;

            if (clear?.Payload?.ClearData is null)
                clearMode = Common.BiometricCapabilities.ClearData;

            else
            {
                clearMode = clear.Payload.ClearData switch
                {
                    ClearCommand.PayloadData.ClearDataEnum.ScannedData => BiometricCapabilitiesClass.ClearModesEnum.ScannedData,
                    ClearCommand.PayloadData.ClearDataEnum.ImportedData => BiometricCapabilitiesClass.ClearModesEnum.ImportedData,
                    ClearCommand.PayloadData.ClearDataEnum.SetMatchedData => BiometricCapabilitiesClass.ClearModesEnum.SetMatchedData,
                    _ => throw new InvalidDataException("Invalid ClearData type supplied within Biometric.Clear handler.")
                };

                if (!Common.BiometricCapabilities.ClearData.HasFlag(clearMode))
                    return new ClearCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "Unsupported ClearData specified in Clear payload.");
            }

            Logger.Log(Constants.DeviceClass, "BiometricDev.ClearAsync()");
            var result = await Device.ClearAsync(new ClearDataRequest(clearMode), cancel);
            Logger.Log(Constants.DeviceClass, $"BiometricDev.ClearAsync() -> {result.CompletionCode}");

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

            return new ClearCompletion.PayloadData(result.CompletionCode,
                                                   result.ErrorDescription);
        }

    }
}
