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
using XFS4IoTServer;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.VendorMode;
using XFS4IoT.Commands;
using System.ComponentModel;

namespace XFS4IoTServer
{
    public partial class VendorModeServiceClass
    {
        public VendorModeServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(VendorModeServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IVendorModeDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(VendorModeServiceClass)}");

            CommonService.VendorModeStatus = new VendorModeStatusClass(VendorModeStatusClass.DeviceStatusEnum.Online,
                                                                       VendorModeStatusClass.ServiceStatusEnum.Inactive);
            CommonService.VendorModeStatus.PropertyChanged += StatusChangedEventFowarder;
        }

        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        #region VendorMode unsolicited events

        /// <summary>
        /// This event is used to indicate that the system has exited Vendor Mode
        /// </summary>
        public Task ModeExitedEvent() => ModeExitedEvent(new XFS4IoT.VendorMode.Events.ModeExitedEvent.PayloadData(RegisteredClients.Select(n => n.Value).ToList()));

        /// <summary>
        /// This service event is used to indicate the request to exit Vendor Mode to all registered clients
        /// </summary>
        public Task BroadcastExitModeRequestEvent()
        {
            CommonService.VendorModeStatus.ServiceStatus = VendorModeStatusClass.ServiceStatusEnum.ExitPending;
            PendingAcknowledge = RegisteredClients.Select(c => c.Key).ToList(); 
            return ServiceProvider.BroadcastEvent(PendingAcknowledge, new XFS4IoT.VendorMode.Events.ExitModeRequestEvent());
        }

        /// <summary>
        /// This service event is used to indicate the request to enter Vendor Mode
        /// </summary>
        public Task BroadcastEnterModeRequestEvent()
        {
            CommonService.VendorModeStatus.ServiceStatus = VendorModeStatusClass.ServiceStatusEnum.EnterPending;
            PendingAcknowledge = RegisteredClients.Select(c => c.Key).ToList();
            return ServiceProvider.BroadcastEvent(PendingAcknowledge, new XFS4IoT.VendorMode.Events.EnterModeRequestEvent());
        }

        #endregion

        /// <summary>
        /// Pending on receiving acknowledge from the clients
        /// </summary>
        public List<IConnection> PendingAcknowledge { get; set; } = new();

        /// <summary>
        /// List of registered client via Register command
        /// </summary>
        public Dictionary<IConnection, string> RegisteredClients { get; set; } = new();

        /// <summary>
        /// Status changed event handler defined in each of device status class
        /// </summary>
        /// <param name="sender">object where the property is changed</param>
        /// <param name="propertyInfo">including name of property is being changed</param>
        private async void StatusChangedEventFowarder(object sender, PropertyChangedEventArgs propertyInfo) => await CommonService.StatusChangedEvent(sender, propertyInfo);
    }
}
