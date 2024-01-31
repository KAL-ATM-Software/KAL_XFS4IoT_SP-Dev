/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Camera;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{

    public partial class CameraServiceClass
    {

        public CameraServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CameraServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICameraDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(CameraServiceClass)}");

            GetCapabilities();
            GetStatus();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "CameraDev.CameraCapabilities");
            CommonService.CameraCapabilities = Device.CameraCapabilities;
            Logger.Log(Constants.DeviceClass, "CameraDev.CameraCapabilities=");

            CommonService.CameraCapabilities.IsNotNull($"The device class set CameraCapabilities property to null. The device class must report device capabilities.");
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "CameraDev.CameraStatus");
            CommonService.CameraStatus = Device.CameraStatus;
            Logger.Log(Constants.DeviceClass, "CameraDev.CameraStatus=");

            CommonService.CameraStatus.IsNotNull($"The device class set CameraStatus property to null. The device class must report device status.");
            foreach (var stdCam in CommonService.CameraStatus.CameraLocationStatus)
            {
                stdCam.Value.Location = stdCam.Key;
                stdCam.Value.PropertyChanged += StatusChangedEventFowarder;
            }
            foreach (var customCam in CommonService.CameraStatus.CustomCameraLocationStatus)
            {
                customCam.Value.CustomLocation = customCam.Key;
                customCam.Value.PropertyChanged += StatusChangedEventFowarder;
            }
        }

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await CommonService.StatusChangedEvent(sender, propertyInfo);
    }
}
