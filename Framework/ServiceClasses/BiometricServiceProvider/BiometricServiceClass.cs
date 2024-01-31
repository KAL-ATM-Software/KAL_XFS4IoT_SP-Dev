/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Biometric;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{

    public partial class BiometricServiceClass
    {

        public BiometricServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(BiometricServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IBiometricDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(BiometricServiceClass)}");

            GetCapabilities();
            GetStatus();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "BiometricDev.BiometricCapabilities");
            CommonService.BiometricCapabilities = Device.BiometricCapabilities;
            Logger.Log(Constants.DeviceClass, "BiometricDev.BiometricCapabilities=");

            CommonService.BiometricCapabilities.IsNotNull($"The device class set BiometricCapabilities property to null. The device class must report device capabilities.");
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "BiometricDev.BiometricStatus");
            CommonService.BiometricStatus = Device.BiometricStatus;
            Logger.Log(Constants.DeviceClass, "BiometricDev.BiometricStatus=");

            CommonService.BiometricStatus.IsNotNull($"The device class set BiometricStatus property to null. The device class must report device status.");
            CommonService.BiometricStatus.PropertyChanged += StatusChangedEventFowarder;
        }

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await CommonService.StatusChangedEvent(sender, propertyInfo);
    }
}
