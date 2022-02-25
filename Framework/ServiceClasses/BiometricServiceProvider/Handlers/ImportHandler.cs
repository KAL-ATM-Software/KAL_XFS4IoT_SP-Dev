/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * ImportHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Biometric.Commands;
using XFS4IoT.Biometric.Completions;
using System.Collections.Generic;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Biometric
{
    public partial class ImportHandler
    {

        private async Task<ImportCompletion.PayloadData> HandleImport(IImportEvents events, ImportCommand import, CancellationToken cancel)
        {

            if (import?.Payload?.Templates is null || import.Payload.Templates.Count == 0)
                return new ImportCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                        "No Templates specified for Import command.",
                                                        ImportCompletion.PayloadData.ErrorCodeEnum.InvalidData);

            var biometricData = new List<BiometricData>();

            foreach(var item in import.Payload.Templates)
            {
                if (item.Data is null || item.Data.Count == 0)
                    return new ImportCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                            "No Data specified for Template",
                                                            ImportCompletion.PayloadData.ErrorCodeEnum.InvalidData);

                if (item.Type is null || item.Type.Format is null)
                    return new ImportCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                            "No DataType specified for Template",
                                                            ImportCompletion.PayloadData.ErrorCodeEnum.InvalidData);

                biometricData.Add(new BiometricData(
                    new BiometricDataType(
                        item.Type.Format switch
                        {
                            XFS4IoT.Biometric.DataTypeClass.FormatEnum.IsoFid => BiometricCapabilitiesClass.FormatEnum.IsoFid,
                            XFS4IoT.Biometric.DataTypeClass.FormatEnum.IsoFmd => BiometricCapabilitiesClass.FormatEnum.IsoFmd,
                            XFS4IoT.Biometric.DataTypeClass.FormatEnum.AnsiFid => BiometricCapabilitiesClass.FormatEnum.AnsiFid,
                            XFS4IoT.Biometric.DataTypeClass.FormatEnum.AnsiFmd => BiometricCapabilitiesClass.FormatEnum.AnsiFmd,
                            XFS4IoT.Biometric.DataTypeClass.FormatEnum.Qso => BiometricCapabilitiesClass.FormatEnum.Qso,
                            XFS4IoT.Biometric.DataTypeClass.FormatEnum.Wso => BiometricCapabilitiesClass.FormatEnum.Wso,
                            XFS4IoT.Biometric.DataTypeClass.FormatEnum.ReservedRaw1 => BiometricCapabilitiesClass.FormatEnum.ReservedRaw1,
                            XFS4IoT.Biometric.DataTypeClass.FormatEnum.ReservedTemplate1 => BiometricCapabilitiesClass.FormatEnum.ReservedTemplate1,
                            XFS4IoT.Biometric.DataTypeClass.FormatEnum.ReservedRaw2 => BiometricCapabilitiesClass.FormatEnum.ReservedRaw2,
                            XFS4IoT.Biometric.DataTypeClass.FormatEnum.ReservedTemplate2 => BiometricCapabilitiesClass.FormatEnum.ReservedTemplate2,
                            XFS4IoT.Biometric.DataTypeClass.FormatEnum.ReservedRaw3 => BiometricCapabilitiesClass.FormatEnum.ReservedRaw3,
                            XFS4IoT.Biometric.DataTypeClass.FormatEnum.ReservedTemplate3 => BiometricCapabilitiesClass.FormatEnum.ReservedTemplate3,
                            _ => throw Contracts.Fail<NotImplementedException>("Unexpected Format value specified.")
                        },
                        item.Type.Algorithm switch
                        {
                            XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Cbc => BiometricCapabilitiesClass.AlgorithmEnum.Cbc,
                            XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Cfb => BiometricCapabilitiesClass.AlgorithmEnum.Cfb,
                            XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Ecb => BiometricCapabilitiesClass.AlgorithmEnum.Ecb,
                            XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Rsa => BiometricCapabilitiesClass.AlgorithmEnum.Rsa,
                            null => null,
                            _ => throw Contracts.Fail<NotImplementedException>("Unexpected Algorithm value specified.")
                        },
                        item.Type.KeyName),
                    item.Data));
            }



            Logger.Log(Constants.DeviceClass, "BiometricDev.ImportAsync()");

            var result = await Device.ImportAsync(new ImportRequest(biometricData), cancel);

            Logger.Log(Constants.DeviceClass, $"BiometricDev.ImportAsync() -> {result.CompletionCode}");

            Dictionary<string, XFS4IoT.Biometric.DataTypeClass> importedTemplates = null;
            if (result.Templates != null && result.Templates.Count > 0)
            {
                importedTemplates = new();
                foreach (var item in result.Templates)
                {
                    item.Key.IsNotNullOrWhitespace("BiometricDataType id passed by the Device should not be null or whitespace.");
                    item.Value.IsNotNull("BiometricDataType passed by the Device should not be null.");

                    importedTemplates.Add(item.Key,
                        new XFS4IoT.Biometric.DataTypeClass(
                            item.Value.Format switch
                            {
                                BiometricCapabilitiesClass.FormatEnum.IsoFid => XFS4IoT.Biometric.DataTypeClass.FormatEnum.IsoFid,
                                BiometricCapabilitiesClass.FormatEnum.IsoFmd => XFS4IoT.Biometric.DataTypeClass.FormatEnum.IsoFmd,
                                BiometricCapabilitiesClass.FormatEnum.AnsiFid => XFS4IoT.Biometric.DataTypeClass.FormatEnum.AnsiFid,
                                BiometricCapabilitiesClass.FormatEnum.AnsiFmd => XFS4IoT.Biometric.DataTypeClass.FormatEnum.AnsiFmd,
                                BiometricCapabilitiesClass.FormatEnum.Qso => XFS4IoT.Biometric.DataTypeClass.FormatEnum.Qso,
                                BiometricCapabilitiesClass.FormatEnum.Wso => XFS4IoT.Biometric.DataTypeClass.FormatEnum.Wso,
                                BiometricCapabilitiesClass.FormatEnum.ReservedRaw1 => XFS4IoT.Biometric.DataTypeClass.FormatEnum.ReservedRaw1,
                                BiometricCapabilitiesClass.FormatEnum.ReservedTemplate1 => XFS4IoT.Biometric.DataTypeClass.FormatEnum.ReservedTemplate1,
                                BiometricCapabilitiesClass.FormatEnum.ReservedRaw2 => XFS4IoT.Biometric.DataTypeClass.FormatEnum.ReservedRaw2,
                                BiometricCapabilitiesClass.FormatEnum.ReservedTemplate2 => XFS4IoT.Biometric.DataTypeClass.FormatEnum.ReservedTemplate2,
                                BiometricCapabilitiesClass.FormatEnum.ReservedRaw3 => XFS4IoT.Biometric.DataTypeClass.FormatEnum.ReservedRaw3,
                                BiometricCapabilitiesClass.FormatEnum.ReservedTemplate3 => XFS4IoT.Biometric.DataTypeClass.FormatEnum.ReservedTemplate3,
                                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected Format supplied by the device. {item.Value.Format}")
                            },
                            item.Value.Algorithm switch
                            {
                                BiometricCapabilitiesClass.AlgorithmEnum.Ecb => XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Ecb,
                                BiometricCapabilitiesClass.AlgorithmEnum.Cbc => XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Cbc,
                                BiometricCapabilitiesClass.AlgorithmEnum.Cfb => XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Cfb,
                                BiometricCapabilitiesClass.AlgorithmEnum.Rsa => XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Rsa,
                                null => null,
                                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected Algorithm supplied by the device. {item.Value.Format}")
                            },
                            item.Value.KeyName));
                }
            }

            return new ImportCompletion.PayloadData(result.CompletionCode,
                                                    result.ErrorDescription,
                                                    result.ErrorCode,
                                                    importedTemplates);

        }

    }
}
