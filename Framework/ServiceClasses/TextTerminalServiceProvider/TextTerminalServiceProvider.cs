/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Common.Events;
using XFS4IoT.TextTerminal.Events;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.TextTerminal;
using XFS4IoTServer;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a text terminal service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical text terminal, which only implements the TextTerminal and Common interfaces. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class TextTerminalServiceProvider : ServiceProvider, ITextTerminalService, ICommonService, ILightsService
    {
        public TextTerminalServiceProvider(
            EndpointDetails endpointDetails, 
            string ServiceName, 
            IDevice device, 
            ILogger logger)
            :
            base(endpointDetails,
                 ServiceName,
                 [XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.TextTerminal, XFSConstants.ServiceClass.Lights],
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            TextTerminal = new TextTerminalServiceClass(this, logger);
            LightsService = new LightsServiceClass(this, logger);
        }

        private readonly TextTerminalServiceClass TextTerminal;
        private readonly CommonServiceClass CommonService;
        private readonly LightsServiceClass LightsService;

        #region Common unsolicited events
        public Task StatusChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => CommonService.StatusChangedEvent(sender, propertyInfo);

        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the TextTerminal Service.");

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
        /// Stores TexTerminal interface capabilites internally
        /// </summary>
        public TextTerminalCapabilitiesClass TextTerminalCapabilities { get => CommonService.TextTerminalCapabilities; set => CommonService.TextTerminalCapabilities = value; }

        /// <summary>
        /// Stores TexTerminal interface status internally
        /// </summary>
        public TextTerminalStatusClass TextTerminalStatus { get => CommonService.TextTerminalStatus; set => CommonService.TextTerminalStatus = value; }

        /// <summary>
        /// Stores Lights interface capabilites internally
        /// </summary>
        public LightsCapabilitiesClass LightsCapabilities { get => CommonService.LightsCapabilities; set => CommonService.LightsCapabilities = value; }

        /// <summary>
        /// Lights Status
        /// </summary>
        public LightsStatusClass LightsStatus { get => CommonService.LightsStatus; set => CommonService.LightsStatus = value; }


        #endregion

        public ITextTerminalService.KeyDetails SupportedKeys { get => TextTerminal.SupportedKeys; set => TextTerminal.SupportedKeys = value; }
        public bool FirstGetKeyDetailCommand { get => TextTerminal.FirstGetKeyDetailCommand; set => TextTerminal.FirstGetKeyDetailCommand = value; }

        public void UpdateKeyDetails() => TextTerminal.UpdateKeyDetails();

    }
}