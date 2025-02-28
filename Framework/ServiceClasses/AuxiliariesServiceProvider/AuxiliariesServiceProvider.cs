/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Auxiliaries;
using System.Threading;
using XFS4IoT.Common.Events;
using System.ComponentModel;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a auxiliaries service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical auxiliaries service, which only implements the Auxiliaries, Common and Lights interfaces. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class AuxiliariesServiceProvider : ServiceProvider, IAuxiliariesService, ICommonService, ILightsService
    {
        public AuxiliariesServiceProvider(
            EndpointDetails endpointDetails, 
            string ServiceName, 
            IDevice device, 
            ILogger logger)
            :
            base(endpointDetails,
                 ServiceName,
                 [XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.Auxiliaries, XFSConstants.ServiceClass.Lights],
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            LightsService = new LightsServiceClass(this, logger);
            Auxiliaries = new AuxiliariesServiceClass(this, logger);
        }

        private readonly AuxiliariesServiceClass Auxiliaries;
        private readonly CommonServiceClass CommonService;
        private readonly LightsServiceClass LightsService;

        #region Auxiliaries unsolicited events

        #endregion

        #region Common unsolicited events

        public Task StatusChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => CommonService.StatusChangedEvent(sender, propertyInfo);

        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the Auxiliaries Service.");

        public Task ErrorEvent(
            CommonStatusClass.ErrorEventIdEnum EventId,
            CommonStatusClass.ErrorActionEnum Action,
            string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);
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
        /// Stores Auxiliaries interface capabilites internally
        /// </summary>
        public AuxiliariesCapabilitiesClass AuxiliariesCapabilities { get => CommonService.AuxiliariesCapabilities; set => CommonService.AuxiliariesCapabilities = value; }

        /// <summary>
        /// Auxiliaries Status
        /// </summary>
        public AuxiliariesStatusClass AuxiliariesStatus { get => CommonService.AuxiliariesStatus; set => CommonService.AuxiliariesStatus = value; }

        /// <summary>
        /// Stores Lights interface capabilites internally
        /// </summary>
        public LightsCapabilitiesClass LightsCapabilities { get => CommonService.LightsCapabilities; set => CommonService.LightsCapabilities = value; }

        /// <summary>
        /// Lights Status
        /// </summary>
        public LightsStatusClass LightsStatus { get => CommonService.LightsStatus; set => CommonService.LightsStatus = value; }


        #endregion

        #region Auxiliaries Service

        #endregion
    }
}
