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
using XFS4IoTFramework.GermanSpecific;

namespace XFS4IoTServer
{
    public partial class GermanSpecificServiceClass
    {

        public GermanSpecificServiceClass(
            IServiceProvider ServiceProvider,
            ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(GermanSpecificServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IGermanSpecificDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(GermanSpecificServiceClass)}");

            GetCapabilities();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "GermanSpecificDev.GermanSpecificCapabilities");
            CommonService.GermanSpecificCapabilities = Device.GermanSpecificCapabilities;
            Logger.Log(Constants.DeviceClass, "GermanSpecificDev.GermanSpecificCapabilities=");

            CommonService.IBNSCapabilities.IsNotNull($"The device class set GermanSpecificCapabilities property to null. The device class must report device capabilities.");
        }
    }
}
