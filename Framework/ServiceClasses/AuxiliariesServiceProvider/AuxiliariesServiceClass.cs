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
using XFS4IoTFramework.Auxiliaries;
using XFS4IoT.Auxiliaries;
using System.Threading;
using XFS4IoT.Common.Events;
using System.ComponentModel;

namespace XFS4IoTServer
{

    public partial class AuxiliariesServiceClass
    {
        public AuxiliariesServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(AuxiliariesServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IAuxiliariesDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(AuxiliariesServiceClass)}");

            GetCapabilities();
            GetStatus();
        }

        private readonly ICommonService CommonService;

        #region Auxiliaries Service 

        #endregion

        #region Auxiliaries unsolicited events

        #endregion

        #region Common unsolicited events

        #endregion

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.AuxiliariesStatus");
            CommonService.AuxiliariesStatus = Device.AuxiliariesStatus;
            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.AuxiliariesStatus=");

            CommonService.AuxiliariesStatus.IsNotNull($"The device class set AuxiliariesStatus property to null. The device class must report device status.");
            CommonService.AuxiliariesStatus.PropertyChanged += StatusChangedEventFowarder;
            if (CommonService.AuxiliariesStatus.Doors is not null)
            {
                foreach (var aux in CommonService.AuxiliariesStatus.Doors)
                {
                    aux.Value.Type = aux.Key;
                    aux.Value.PropertyChanged += StatusChangedEventFowarder;
                }
            }
        }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.AuxiliariesCapabilities");
            CommonService.AuxiliariesCapabilities = Device.AuxiliariesCapabilities;
            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.AuxiliariesCapabilities=");

            CommonService.AuxiliariesCapabilities.IsNotNull($"The device class set AuxiliariesCapabilities property to null. The device class must report device capabilities.");
        }

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await CommonService.StatusChangedEvent(sender, propertyInfo);
    }
}
