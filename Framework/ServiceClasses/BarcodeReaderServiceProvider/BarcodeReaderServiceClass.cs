/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
using XFS4IoTFramework.BarcodeReader;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{

    public partial class BarcodeReaderServiceClass
    {

        public BarcodeReaderServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(BarcodeReaderServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IBarcodeReaderDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(BarcodeReaderServiceClass)}");
            
            GetCapabilities();
            GetStatus();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "BarcodeReaderDev.BarcodeReaderCapabilities");
            CommonService.BarcodeReaderCapabilities = Device.BarcodeReaderCapabilities;
            Logger.Log(Constants.DeviceClass, "BarcodeReaderDev.BarcodeReaderCapabilities=");

            CommonService.BarcodeReaderCapabilities.IsNotNull($"The device class set BarcodeReaderCapabilities property to null. The device class must report device capabilities.");
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "BarcodeReaderDev.CardReaderStatus");
            CommonService.BarcodeReaderStatus = Device.BarcodeReaderStatus;
            Logger.Log(Constants.DeviceClass, "BarcodeReaderDev.CardReaderStatus=");

            CommonService.BarcodeReaderStatus.IsNotNull($"The device class set BarcodeReaderStatus property to null. The device class must report device status.");
            CommonService.BarcodeReaderStatus.PropertyChanged += StatusChangedEventFowarder;
        }

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await CommonService.StatusChangedEvent(sender, propertyInfo);
    }
}
