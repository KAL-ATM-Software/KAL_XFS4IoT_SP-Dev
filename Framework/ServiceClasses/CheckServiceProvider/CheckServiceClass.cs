/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * CheckServiceClass.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Storage;
using XFS4IoTFramework.Check;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTServer
{
    public partial class CheckServiceClass
    {
        public CheckServiceClass(IServiceProvider ServiceProvider, 
                                 ILogger logger, 
                                 IPersistentData PersistentData)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CheckServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICheckDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(CheckServiceClass)}");
            StorageService = ServiceProvider.IsA<IStorageService>($"Invalid interface parameter specified for storage service. {nameof(CheckServiceClass)}");

            this.PersistentData = PersistentData.IsNotNull($"No persistent data interface is set in the " + nameof(CheckServiceClass));

            // Load persistent data
            LastTransactionStatus = PersistentData.Load<TransactionStatus>(ServiceProvider.Name + typeof(TransactionStatus).FullName);
            LastTransactionStatus ??= new();

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
            Logger.Log(Constants.DeviceClass, "CHeckScannerDev.CheckScannerCapabilities");
            CommonService.CheckScannerCapabilities = Device.CheckScannerCapabilities;
            Logger.Log(Constants.DeviceClass, "CHeckScannerDev.CheckScannerCapabilities=");

            CommonService.CheckScannerCapabilities.IsNotNull($"The device class set CheckScannerCapabilities property to null. The device class must report device capabilities.");
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "CHeckScannerDev.CheckScannerStatus");
            CommonService.CheckScannerStatus = Device.CheckScannerStatus;
            Logger.Log(Constants.DeviceClass, "CHeckScannerDev.CheckScannerStatus=");

            CommonService.CheckScannerStatus.IsNotNull($"The device class set CheckScannerStatus property to null. The device class must report device status.");
            CommonService.CheckScannerStatus.PropertyChanged += StatusChangedEventFowarder;

            foreach (var position in CommonService.CheckScannerStatus.Positions)
            {
                CheckScannerStatusClass.PositionStatusClass.PositionBitmapEnum thisPosition = position.Key switch
                {
                    CheckScannerCapabilitiesClass.PositionEnum.Input => CheckScannerStatusClass.PositionStatusClass.PositionBitmapEnum.Input,
                    CheckScannerCapabilitiesClass.PositionEnum.Output => CheckScannerStatusClass.PositionStatusClass.PositionBitmapEnum.Output,
                    CheckScannerCapabilitiesClass.PositionEnum.Refused => CheckScannerStatusClass.PositionStatusClass.PositionBitmapEnum.Refused,
                    _ => throw new InternalErrorException($"Unsupported position key specified. {position.Key}")
                };

                if (position.Value.Position is null)
                {
                    position.Value.Position = thisPosition;
                    position.Value.PropertyChanged += StatusChangedEventFowarder;
                }
                else
                {
                    // The device class maybe sharing the same status object with different locations.
                    // For example, output position, centor and default, then the PropertyChanged event is sent once property is changed.
                    // Need to handle multiple position status in one PropertyChanged event.
                    position.Value.Position |= thisPosition;
                }
            }
        }

        /// <summary>
        /// Persistent data storage access
        /// </summary>
        private readonly IPersistentData PersistentData;

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await CommonService.StatusChangedEvent(sender, propertyInfo);

        /// <summary>
        /// Store transaction status
        /// </summary>
        public TransactionStatus LastTransactionStatus { get; init; }

        /// <summary>
        /// Store transaction status persistently
        /// </summary>
        public void StoreTransactionStatus()
        {
            if (!PersistentData.Store<TransactionStatus>(ServiceProvider.Name + typeof(TransactionStatus).FullName, LastTransactionStatus))
            {
                Logger.Warning(Constants.Framework, $"Failed to save persistent data. {ServiceProvider.Name + typeof(TransactionStatus).FullName}");
            }
        }
    }
}
