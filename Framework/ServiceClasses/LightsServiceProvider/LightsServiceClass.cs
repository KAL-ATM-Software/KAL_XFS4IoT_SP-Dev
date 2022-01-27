/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
using XFS4IoTFramework.Lights;

namespace XFS4IoTServer
{
    public partial class LightsServiceClass
    {
        public LightsServiceClass(IServiceProvider ServiceProvider,
                                  ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(LightsServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ILightsDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(LightsServiceClass)}");

            GetCapabilities();
            GetStatus();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "LightsDev.LightsStatus");
            CommonService.LightsStatus = Device.LightsStatus;
            Logger.Log(Constants.DeviceClass, "LightsDev.LightsStatus=");

            CommonService.LightsStatus.IsNotNull($"The device class set LightsStatus property to null. The device class must report device status.");
        }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "LightsDev.LightsCapabilities");
            CommonService.LightsCapabilities = Device.LightsCapabilities;
            Logger.Log(Constants.DeviceClass, "LightsDev.LightsCapabilities=");

            CommonService.LightsCapabilities.IsNotNull($"The device class set LightsCapabilities property to null. The device class must report device capabilities.");
        }
    }
}
