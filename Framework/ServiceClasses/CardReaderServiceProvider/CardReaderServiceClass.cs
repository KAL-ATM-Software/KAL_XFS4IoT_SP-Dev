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
using XFS4IoT.Commands;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.CardReader;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTServer
{
    public partial class CardReaderServiceClass
    {
        public CardReaderServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CardReaderServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICardReaderDevice>();
            
            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(CardReaderServiceClass)}");
            StorageService = ServiceProvider.IsA<IStorageService>($"Invalid interface parameter specified for storage service. {nameof(CardReaderServiceClass)}");

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

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "CardReaderDev.CardReaderCapabilities");
            CommonService.CardReaderCapabilities = Device.CardReaderCapabilities;
            Logger.Log(Constants.DeviceClass, "CardReaderDev.CardReaderCapabilities=");

            CommonService.CardReaderCapabilities.IsNotNull($"The device class set CardReaderCapabilities property to null. The device class must report device capabilities.");
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "CardReaderDev.CardReaderStatus");
            CommonService.CardReaderStatus = Device.CardReaderStatus;
            Logger.Log(Constants.DeviceClass, "CardReaderDev.CardReaderStatus=");

            CommonService.CardReaderStatus.IsNotNull($"The device class set CardReaderStatus property to null. The device class must report device status.");
            CommonService.CardReaderStatus.PropertyChanged += StatusChangedEventFowarder;
        }

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await CommonService.StatusChangedEvent(sender, propertyInfo);
    }
}
