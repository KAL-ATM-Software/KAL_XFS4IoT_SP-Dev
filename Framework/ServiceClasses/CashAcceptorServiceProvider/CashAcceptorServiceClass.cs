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
using XFS4IoTFramework.CashAcceptor;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTServer
{

    public partial class CashAcceptorServiceClass
    {

        public CashAcceptorServiceClass(IServiceProvider ServiceProvider,
                                        ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CashAcceptorServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICashAcceptorDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(CashAcceptorServiceClass)}");
            CashManagement = ServiceProvider.IsA<XFS4IoTFramework.CashManagement.ICashManagementService>($"Invalid interface parameter specified for cash management service. {nameof(CashAcceptorServiceClass)}");
            Storage = ServiceProvider.IsA<IStorageService>($"Invalid interface parameter specified for storage service. {nameof(CashAcceptorServiceClass)}");

            GetCapabilities();
            GetStatus();

            // Get CashAcceptor specific status and capabilites
            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashInStatus");
            CashInStatus = Device.CashInStatus;
            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashInStatus=");

            if (CashInStatus is null)
            {
                Logger.Log(Constants.DeviceClass, "The device class does not support CashInStatus.");
                // The device class doesn't support cash-in status and use it managed by the framework.
                CashInStatus = CashManagement.CashInStatusManaged;
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.DeviceLockStatus");
            DeviceLockStatus = Device.DeviceLockStatus;
            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.DeviceLockStatus=");

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.GetDepleteCashUnitSources()");
            DepleteCashUnitSources = Device.GetDepleteCashUnitSources();
            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.GetDepleteCashUnitSources->");

            if (DepleteCashUnitSources?.Count > 0)
            {
                // Check valid storage id for all sources
                foreach (var sources in DepleteCashUnitSources)
                {
                    Storage.CashUnits.ContainsKey(sources.Key).IsTrue($"Invalid storage Id provided by the service provider for the target storage for deplete operation. {sources.Key}");
                    foreach (var source in sources.Value)
                    {
                        Storage.CashUnits.ContainsKey(source).IsTrue($"Invalid storage Id provided by the service provider for the source storage for deplete operation. {source}");
                    }
                }
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.ReplenishTargets()");
            ReplenishTargets = Device.ReplenishTargets();
            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.ReplenishTargets()->");
            if (ReplenishTargets?.Count > 0)
            {
                // Check valid storage id for all targets
                foreach (var target in ReplenishTargets)
                {
                    Storage.CashUnits.ContainsKey(target).IsTrue($"Invalid storage Id provided by the service provider for the target storage for the replenish. {target}");
                }
            }
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        /// <summary>
        /// CashManagement service interface
        /// </summary>
        public XFS4IoTFramework.CashManagement.ICashManagementService CashManagement { get; init; }

        /// <summary>
        /// Storage service interface
        /// </summary>
        public IStorageService Storage { get; init; }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashAcceptorCapabilities");
            CommonService.CashAcceptorCapabilities = Device.CashAcceptorCapabilities;
            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashAcceptorCapabilities=");

            CommonService.CashAcceptorCapabilities.IsNotNull($"The device class set CashAcceptorCapabilities property to null. The device class must report device capabilities.");
        }

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashAcceptorStatus");
            CommonService.CashAcceptorStatus = Device.CashAcceptorStatus;
            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.CashAcceptorStatus=");

            CommonService.CashAcceptorStatus.IsNotNull($"The device class set CashAcceptorStatus property to null. The device class must report device status.");
            CommonService.CashAcceptorStatus.PropertyChanged += StatusChangedEventFowarder;
            if (CommonService.CashAcceptorStatus.Positions is not null)
            {
                foreach (var position in CommonService.CashAcceptorStatus.Positions)
                {
                    if (position.Value.CashAcceptorPosition is null)
                    {
                        position.Value.CashAcceptorPosition = position.Key;
                        position.Value.PropertyChanged += StatusChangedEventFowarder;
                    }
                    else
                    {
                        // The device class maybe sharing the same status object with different locations.
                        // For example, output position, centor and default, then the PropertyChanged event is sent once property is changed.
                        // Need to handle multiple position status in one PropertyChanged event.
                        position.Value.CashAcceptorPosition |= position.Key;
                    }
                }
            }
        }

        /// <summary>
        /// The information about the status of the currently active cash-in transaction or 
        /// in the case where no cash-in transaction is active the status of the most recently ended cash-in transaction. 
        /// </summary>
        public XFS4IoTFramework.CashManagement.CashInStatusClass CashInStatus { get; init; }

        /// <summary>
        /// The physical lock/unlock status of the CashAcceptor device and storages.
        /// </summary>
        public DeviceLockStatusClass DeviceLockStatus { get; init; }

        /// <summary>
        /// The deplete target and destination information
        /// Key - The storage id can be used for target of the depletion operation.
        /// Value - List of storage id can be used for source of the depletion operation
        /// </summary>
        public Dictionary<string, List<string>> DepleteCashUnitSources { get; init; }

        /// <summary>
        /// Which storage units can be specified as targets for a given source storage unit with the CashAcceptor.Replenish command
        /// </summary>
        public List<string> ReplenishTargets { get; init; }

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await CommonService.StatusChangedEvent(sender, propertyInfo);
    }
}