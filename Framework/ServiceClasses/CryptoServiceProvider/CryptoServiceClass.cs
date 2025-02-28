/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;

using XFS4IoTFramework.Common;
using XFS4IoTFramework.KeyManagement;
using XFS4IoTFramework.Crypto;

namespace XFS4IoTServer
{
    public partial class CryptoServiceClass
    {
        public CryptoServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CryptoServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICryptoDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(CryptoServiceClass)}");
            KeyManagementService = ServiceProvider.IsA<IKeyManagementService>($"Invalid interface parameter specified for key management service. {nameof(CryptoServiceClass)}");

            GetCapabilities();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }

        /// <summary>
        /// KeyManagement service interface
        /// </summary>
        private IKeyManagementService KeyManagementService { get; init; }


        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "CryptoDev.CryptoCapabilities");
            CommonService.CryptoCapabilities = Device.CryptoCapabilities;
            Logger.Log(Constants.DeviceClass, "CryptoDev.CryptoCapabilities=");

            CommonService.CryptoCapabilities.IsNotNull($"The device class set CryptoCapabilities property to null. The device class must report device capabilities.");
        }
    }
}
