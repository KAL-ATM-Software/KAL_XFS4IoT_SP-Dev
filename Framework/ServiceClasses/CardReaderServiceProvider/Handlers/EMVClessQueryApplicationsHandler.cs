/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;

namespace XFS4IoTFramework.CardReader
{
    public partial class EMVClessQueryApplicationsHandler
    {
        private Task<EMVClessQueryApplicationsCompletion.PayloadData> HandleEMVClessQueryApplications(IEMVClessQueryApplicationsEvents events, EMVClessQueryApplicationsCommand eMVClessQueryApplications, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CardReaderDev.EMVContactlessQueryApplications()");
            var result = Device.EMVContactlessQueryApplications();
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.EMVContactlessQueryApplications() -> {result.CompletionCode}");

            List<EMVClessQueryApplicationsCompletion.PayloadData.AppDataClass> appData = new();
            foreach (EMVApplication app in result.EMVApplications)
            {
                if (app.ApplicationIdentifier.Count == 0)
                    continue;
                appData.Add(new EMVClessQueryApplicationsCompletion.PayloadData.AppDataClass(Convert.ToBase64String(app.ApplicationIdentifier.ToArray()),
                                                                                             app.KernelIdentifier.Count == 0 ? null : Convert.ToBase64String(app.KernelIdentifier.ToArray())));
            }

            return Task.FromResult(new EMVClessQueryApplicationsCompletion.PayloadData(result.CompletionCode,
                                                                                       result.ErrorDescription,
                                                                                       appData));
        }
    }
}
