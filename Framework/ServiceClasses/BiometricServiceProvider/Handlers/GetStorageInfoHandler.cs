/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/


using System;
using System.Linq;
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
    public partial class GetStorageInfoHandler
    {

        private Task<GetStorageInfoCompletion.PayloadData> HandleGetStorageInfo(IGetStorageInfoEvents events, GetStorageInfoCommand getStorageInfo, CancellationToken cancel)
        {
            if (Common.BiometricCapabilities.Storage == XFS4IoTFramework.Common.BiometricCapabilitiesClass.StorageEnum.None)
                return Task.FromResult(new GetStorageInfoCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedCommand, "This device has no storage."));

            var deviceStorageInfo = Device.StorageInfo;
            if (deviceStorageInfo == null || deviceStorageInfo.Count == 0)
                return Task.FromResult(new GetStorageInfoCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode, "No imported data stored", GetStorageInfoCompletion.PayloadData.ErrorCodeEnum.NoImportedData));

            Dictionary<string, XFS4IoT.Biometric.DataTypeClass> storageInfo = new();
            foreach(var kv in deviceStorageInfo)
            {
                storageInfo.Add(kv.Key, new XFS4IoT.Biometric.DataTypeClass(
                    kv.Value.Format switch
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
                        _ => throw Contracts.Fail<NotImplementedException>("Unexpected Format specified.")
                    },
                    kv.Value.Algorithm switch
                    {
                        BiometricCapabilitiesClass.AlgorithmEnum.Ecb => XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Ecb,
                        BiometricCapabilitiesClass.AlgorithmEnum.Cbc => XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Cbc,
                        BiometricCapabilitiesClass.AlgorithmEnum.Cfb => XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Cfb,
                        BiometricCapabilitiesClass.AlgorithmEnum.Rsa => XFS4IoT.Biometric.DataTypeClass.AlgorithmEnum.Rsa,
                        null => null,
                        _ => throw Contracts.Fail<NotImplementedException>("Unexpected Algorithm specified.")
                    },
                    kv.Value.KeyName));
            }

            return Task.FromResult(new GetStorageInfoCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.Success, null, null, storageInfo)); 
        }

    }
}
