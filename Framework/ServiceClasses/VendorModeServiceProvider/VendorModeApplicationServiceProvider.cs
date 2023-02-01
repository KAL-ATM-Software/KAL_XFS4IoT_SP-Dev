/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.CashManagement.Events;
using XFS4IoT.Common.Events;
using XFS4IoT.Storage.Events;
using XFS4IoTFramework.VendorApplication;
using XFS4IoTFramework.VendorMode;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    /// <summary>
    /// The vendor mode service provider
    /// </summary>
    public class VendorModeApplicationServiceProvider : ServiceProvider, ICommonService, IVendorModeService, IVendorApplicationService
    {
        public VendorModeApplicationServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.VendorMode, XFSConstants.ServiceClass.VendorApplication },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            VendorModeService = new VendorModeServiceClass(this, logger);
            VendorApplicationService = new VendorApplicationServiceClass(this, logger);
        }

        private readonly CommonServiceClass CommonService;
        private readonly VendorModeServiceClass VendorModeService;
        private readonly VendorApplicationServiceClass VendorApplicationService;

        #region Common unsolicited events
        public Task StatusChangedEvent(CommonStatusClass.DeviceEnum? Device,
                                       CommonStatusClass.PositionStatusEnum? Position,
                                       int? PowerSaveRecoveryTime,
                                       CommonStatusClass.AntiFraudModuleEnum? AntiFraudModule,
                                       CommonStatusClass.ExchangeEnum? Exchange,
                                       CommonStatusClass.EndToEndSecurityEnum? EndToEndSecurity) => CommonService.StatusChangedEvent(Device,
                                                                                                                                     Position,
                                                                                                                                     PowerSaveRecoveryTime,
                                                                                                                                     AntiFraudModule,
                                                                                                                                     Exchange,
                                                                                                                                     EndToEndSecurity);


        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the VendorMode Service.");

        public Task ErrorEvent(CommonStatusClass.ErrorEventIdEnum EventId,
                               CommonStatusClass.ErrorActionEnum Action,
                               string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);

        #endregion

        #region VendorMode unsolicited events

        /// <summary>
        /// This event is used to indicate that the system has exited Vendor Mode
        /// </summary>
        public Task ModeExitedEvent() => VendorModeService.ModeExitedEvent();

        /// <summary>
        /// This event is used to indicate that the system has entered Vendor Mode
        /// </summary>
        public Task ModeEnteredEvent() => VendorModeService.ModeEnteredEvent();

        /// <summary>
        /// This service event is used to indicate the request to exit Vendor Mode to all registered clients
        /// </summary>
        public Task BroadcastExitModeRequestEvent() => VendorModeService.BroadcastExitModeRequestEvent();

        /// <summary>
        /// This service event is used to indicate the request to enter Vendor Mode
        /// </summary>
        public Task BroadcastEnterModeRequestEvent() => VendorModeService.BroadcastEnterModeRequestEvent();

        #endregion

        #region Common Service

        /// <summary>
        /// Stores Common interface capabilites internally
        /// </summary>
        public CommonCapabilitiesClass CommonCapabilities { get => CommonService.CommonCapabilities; set => CommonService.CommonCapabilities = value; }

        /// <summary>
        /// Common Status
        /// </summary>
        public CommonStatusClass CommonStatus { get => CommonService.CommonStatus; set => CommonService.CommonStatus = value; }

        /// <summary>
        /// Stores vendor mode status
        /// </summary>
        public VendorModeStatusClass VendorModeStatus { get => CommonService.VendorModeStatus; set => CommonService.VendorModeStatus = value; }

        /// <summary>
        /// Stores vendor application capabilites
        /// </summary>
        public VendorApplicationCapabilitiesClass VendorApplicationCapabilities { get => CommonService.VendorApplicationCapabilities; set => CommonService.VendorApplicationCapabilities = value; }

        /// <summary>
        /// Stores vendor application status
        /// </summary>
        public VendorApplicationStatusClass VendorApplicationStatus { get => CommonService.VendorApplicationStatus; set => CommonService.VendorApplicationStatus = value; }


        #endregion

        #region VendorApplication unsolicited events

        /// <summary>
        /// This event is used to indicate the vendor dependent application has exited, 
        /// allowing an application the opportunity to exit Vendor Mode.
        /// </summary>
        public Task VendorAppExitedEvent() => VendorApplicationService.VendorAppExitedEvent();

        /// <summary>
        /// This event is used to indicate that the required interface has changed. 
        /// </summary>
        public Task InterfaceChangedEvent(ActiveInterfaceEnum ActiveInterface)
        {
            return VendorApplicationService.InterfaceChangedEvent(new XFS4IoT.VendorApplication.Events.InterfaceChangedEvent.PayloadData(
                                                                        ActiveInterface switch
                                                                        {
                                                                            ActiveInterfaceEnum.Consumer => XFS4IoT.VendorApplication.Events.InterfaceChangedEvent.PayloadData.ActiveInterfaceEnum.Consumer,
                                                                            _ => XFS4IoT.VendorApplication.Events.InterfaceChangedEvent.PayloadData.ActiveInterfaceEnum.Operator,
                                                                        })
                                                                  );
        }

        #endregion

        /// <summary>
        /// Pending on receiving acknowledge from the clients
        /// </summary>
        public List<IConnection> PendingAcknowledge { get => VendorModeService.PendingAcknowledge; set => VendorModeService.PendingAcknowledge = value; }

        /// <summary>
        /// List of registered client via Register command
        /// </summary>
        public Dictionary<IConnection, string> RegisteredClients { get => VendorModeService.RegisteredClients; set => VendorModeService.RegisteredClients = value; }
    }
}
