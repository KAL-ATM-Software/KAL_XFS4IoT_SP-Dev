/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.PowerManagement;
using System.ComponentModel;

namespace XFS4IoTServer
{
    public partial class PowerManagementServiceClass
    {
        public PowerManagementServiceClass(
            IServiceProvider ServiceProvider,
            ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(PowerManagementServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IPowerManagementDevice>();

            RegisterFactory(ServiceProvider);

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(PowerManagementServiceClass)}");

            GetStatus();
            GetCapabilities();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        private ICommonService CommonService { get; init; }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "PowerManagementDev.PowerManagementStatus");
            CommonService.PowerManagementStatus = Device.PowerManagementStatus;
            Logger.Log(Constants.DeviceClass, "PowerManagementDev.PowerManagementStatus=");

            CommonService.PowerManagementStatus.IsNotNull($"The device class set PowerManagementStatus property to null. The device class must report device status.");
            CommonService.PowerManagementStatus.PropertyChanged += StatusChangedEventFowarder;
            if (CommonService.PowerManagementStatus.PowerInfo is not null)
            {
                CommonService.PowerManagementStatus.PowerInfo.PropertyChanged += StatusChangedEventFowarder;
            }
        }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "PowerManagementDev.PowerManagementCapabilities");
            CommonService.PowerManagementCapabilities = Device.PowerManagementCapabilities;
            Logger.Log(Constants.DeviceClass, "PowerManagementDev.PowerManagementCapabilities=");

            CommonService.PowerManagementCapabilities.IsNotNull($"The device class set PowerManagementCapabilities property to null. The device class must report device capabilities.");
        }

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await CommonService.StatusChangedEvent(sender, propertyInfo);
    }
}
