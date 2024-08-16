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
using System.Collections.Generic;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Biometric
{
    public partial class ReadHandler
    {
        private async Task<CommandResult<ReadCompletion.PayloadData>> HandleRead(IReadEvents events, ReadCommand read, CancellationToken cancel)
        {
            if (read?.Payload is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    "No payload supplied for Read command.");
            }

            BiometricCapabilitiesClass.ScanModesEnum scanMode = read.Payload.Mode switch
            {
                ReadCommand.PayloadData.ModeEnum.Scan => BiometricCapabilitiesClass.ScanModesEnum.Scan,
                ReadCommand.PayloadData.ModeEnum.Match => BiometricCapabilitiesClass.ScanModesEnum.Match,
                null => BiometricCapabilitiesClass.ScanModesEnum.None,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected Mode supplied for Read command. {read.Payload.Mode}")
            };

            List<BiometricDataType> dataTypes = null;
            if (read.Payload.DataTypes is not null && read.Payload.DataTypes.Count > 0)
            {
                dataTypes = new();
                foreach (var item in read.Payload.DataTypes)
                {
                    if (item is null || item.Format is null)
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            "Invalid DataType supplied.");
                    }

                    dataTypes.Add(new BiometricDataType(
                        item.Format switch
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
                        item.Algorithm switch
                        {
                            XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Cbc => BiometricCapabilitiesClass.AlgorithmEnum.Cbc,
                            XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Cfb => BiometricCapabilitiesClass.AlgorithmEnum.Cfb,
                            XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Ecb => BiometricCapabilitiesClass.AlgorithmEnum.Cfb,
                            XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Rsa => BiometricCapabilitiesClass.AlgorithmEnum.Rsa,
                            null => null,
                            _ => throw Contracts.Fail<NotImplementedException>("Unexpected Algorithm value specified.")
                        },
                        item.KeyName));
                }
            }

            Logger.Log(Constants.DeviceClass, "BiometricDev.ReadAsync()");

            var result = await Device.ReadAsync(new(events),
                                                new ReadRequest(read.Header.Timeout ?? 0, dataTypes, read.Payload.NumCaptures ?? 0, scanMode), 
                                                cancel);

            Logger.Log(Constants.DeviceClass, $"BiometricDev.ReadAsync() -> {result.CompletionCode}");


            List<XFS4IoT.Biometric.BioDataClass> biometricData = null;
            if (result.Data?.Count > 0)
            {
                biometricData = [];
                foreach (var item in result.Data)
                {
                    item.Data.IsNotNull("Device returned null Data from Read operation.");
                    Contracts.IsTrue(item.Data.Count > 0, "Device returned 0 length Data from Read operation.");

                    biometricData.Add(new XFS4IoT.Biometric.BioDataClass(
                        new XFS4IoT.Biometric.DataTypeClass(
                            item.DataType.Format switch
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
                                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected Format supplied by the device. {item.DataType.Format}")
                            },
                            item.DataType.Algorithm switch
                            {
                                BiometricCapabilitiesClass.AlgorithmEnum.Ecb => XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Ecb,
                                BiometricCapabilitiesClass.AlgorithmEnum.Cbc => XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Cbc,
                                BiometricCapabilitiesClass.AlgorithmEnum.Cfb => XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Cfb,
                                BiometricCapabilitiesClass.AlgorithmEnum.Rsa => XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Rsa,
                                BiometricCapabilitiesClass.AlgorithmEnum.None or null => null,
                                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected Algorithm supplied by the device. {item.DataType.Format}")
                            },
                            item.DataType.KeyName),
                        item.Data));
                }
            }

            ReadCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                biometricData is not null)
            {
                payload = new(
                    result.ErrorCode,
                    biometricData);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
