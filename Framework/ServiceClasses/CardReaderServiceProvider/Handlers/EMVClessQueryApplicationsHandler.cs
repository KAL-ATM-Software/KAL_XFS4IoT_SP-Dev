/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.Completions;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CardReader
{
    public partial class EMVClessQueryApplicationsHandler
    {
        private Task<EMVClessQueryApplicationsCompletion.PayloadData> HandleEMVClessQueryApplications(IEMVClessQueryApplicationsEvents events, EMVClessQueryApplicationsCommand eMVClessQueryApplications, CancellationToken cancel)
        {
            if (Common.CardReaderCapabilities.Type != CardReaderCapabilitiesClass.DeviceTypeEnum.IntelligentContactless)
            {
                return Task.FromResult(new EMVClessQueryApplicationsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                                           $"This device is not an intelligent contactless CardReader. {Common.CardReaderCapabilities.Type}"));
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.EMVContactlessQueryApplications()");
            var result = Device.EMVContactlessQueryApplications();
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.EMVContactlessQueryApplications() -> {result.CompletionCode}");

            List<EMVClessQueryApplicationsCompletion.PayloadData.AppDataClass> appData = new();
            foreach (EMVApplication app in result.EMVApplications)
            {
                if (app.ApplicationIdentifier.Count == 0)
                    continue;
                appData.Add(new EMVClessQueryApplicationsCompletion.PayloadData.AppDataClass(app.ApplicationIdentifier,
                                                                                             app.KernelIdentifier));
            }

            return Task.FromResult(new EMVClessQueryApplicationsCompletion.PayloadData(result.CompletionCode,
                                                                                       result.ErrorDescription,
                                                                                       appData));
        }
    }
}
