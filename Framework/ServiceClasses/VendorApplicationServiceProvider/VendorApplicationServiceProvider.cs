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
    /// The vendor application service provider
    /// </summary>
    public class VendorApplicationServiceProvider : ServiceProvider, IVendorApplicationService, ICommonService
    {
        public VendorApplicationServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.VendorApplication },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            VendorApplicationService = new VendorApplicationServiceClass(this, logger);
        }

        private readonly VendorApplicationServiceClass VendorApplicationService;
        private readonly CommonServiceClass CommonService;


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


        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the VendorApplication Service.");

        public Task ErrorEvent(CommonStatusClass.ErrorEventIdEnum EventId,
                               CommonStatusClass.ErrorActionEnum Action,
                               string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);

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
        /// Stores vendor application capabilites
        /// </summary>
        public VendorApplicationCapabilitiesClass VendorApplicationCapabilities { get => CommonService.VendorApplicationCapabilities; set => CommonService.VendorApplicationCapabilities = value; }

        /// <summary>
        /// Stores vendor application status
        /// </summary>
        public VendorApplicationStatusClass VendorApplicationStatus { get => CommonService.VendorApplicationStatus; set => CommonService.VendorApplicationStatus = value; }

        #endregion
    }
}
