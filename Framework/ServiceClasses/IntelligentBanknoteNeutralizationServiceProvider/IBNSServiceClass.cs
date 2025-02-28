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
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.IntelligentBanknoteNeutralization;
using XFS4IoTFramework.Storage;

namespace XFS4IoTServer
{
    public partial class IntelligentBanknoteNeutralizationServiceClass
    {
        public IntelligentBanknoteNeutralizationServiceClass(
            IServiceProvider ServiceProvider,
            ILogger logger,
            IPersistentData PersistentData)
        {

            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(IntelligentBanknoteNeutralizationServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IIntelligentBanknoteNeutralizationDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(IntelligentBanknoteNeutralizationServiceClass)}");
            StorageService = ServiceProvider.IsA<IStorageService>($"Invalid interface parameter specified for storage service. {nameof(IntelligentBanknoteNeutralizationServiceClass)}");

            this.PersistentData = PersistentData;

            GetCapabilities();
            GetStatus();
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        /// <summary>
        /// Storage service interface
        /// </summary>
        public IStorageService StorageService { get; init; }

        /// <summary>
        /// Persistent data storage access
        /// </summary>
        private readonly IPersistentData PersistentData;

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "IBNSDev.IBNSCapabilities");
            CommonService.IBNSCapabilities = Device.IBNSCapabilities;
            Logger.Log(Constants.DeviceClass, "IBNSDev.IBNSCapabilities=");

            CommonService.IBNSCapabilities.IsNotNull($"The device class set IBNSCapabilities property to null. The device class must report device capabilities.");
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "IBNSDev.IBNSStatus");
            CommonService.IBNSStatus = Device.IBNSStatus;
            Logger.Log(Constants.DeviceClass, "IBNSDev.IBNSStatus=");

            CommonService.IBNSStatus.IsNotNull($"The device class set IBNSStatus property to null. The device class must report device status.");
            CommonService.IBNSStatus.PropertyChanged += StatusChangedEventFowarder;
            if (CommonService.IBNSStatus.PowerInfo is not null)
            {
                CommonService.IBNSStatus.PowerInfo.PropertyChanged += StatusChangedEventFowarder;
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
