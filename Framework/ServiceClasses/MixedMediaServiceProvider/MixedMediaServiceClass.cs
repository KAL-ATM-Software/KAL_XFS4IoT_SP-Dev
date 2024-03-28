/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.MixedMedia;

namespace XFS4IoTServer
{
    public partial class MixedMediaServiceClass
    {
        public MixedMediaServiceClass(
            IServiceProvider ServiceProvider,
            ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(MixedMediaServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IMixedMediaDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(MixedMediaServiceClass)}");

            GetCapabilities();
            GetStatus();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "MixedMediaDev.MixedMediaCapabilities");
            CommonService.MixedMediaCapabilities = Device.MixedMediaCapabilities;
            Logger.Log(Constants.DeviceClass, "MixedMediaDev.MixedMediaCapabilities=");

            CommonService.MixedMediaCapabilities.IsNotNull($"The device class set MixedMediaCapabilities property to null. The device class must report device capabilities.");
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "MixedMediaDev.MixedMediaStatus");
            CommonService.MixedMediaStatus = Device.MixedMediaStatus;
            Logger.Log(Constants.DeviceClass, "MixedMediaDev.MixedMediaStatus=");

            CommonService.MixedMediaStatus.IsNotNull($"The device class set MixedMediaStatus property to null. The device class must report device status.");
            CommonService.MixedMediaStatus.PropertyChanged += StatusChangedEventFowarder;
        }

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await CommonService.StatusChangedEvent(sender, propertyInfo);
    }
}
