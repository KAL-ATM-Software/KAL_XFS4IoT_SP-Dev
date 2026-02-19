/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.German;

namespace XFS4IoTServer
{
    public partial class GermanServiceClass
    {

        public GermanServiceClass(
            IServiceProvider ServiceProvider,
            ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(GermanServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IGermanDevice>();

            RegisterFactory(ServiceProvider);

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(GermanServiceClass)}");

            GetCapabilities();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "GermanDev.GermanCapabilities");
            CommonService.GermanCapabilities = Device.GermanCapabilities;
            Logger.Log(Constants.DeviceClass, "GermanDev.GermanCapabilities=");

            CommonService.GermanCapabilities.IsNotNull($"The device class set GermanCapabilities property to null. The device class must report device capabilities.");
        }
    }
}
