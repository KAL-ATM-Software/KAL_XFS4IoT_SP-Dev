/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Common.Events;
using XFS4IoT.TextTerminal.Events;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.TextTerminal;
using XFS4IoTServer;

namespace TextTerminalProvider
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
        public TextTerminalServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.TextTerminal },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            TextTerminal = new TextTerminalServiceClass(this, CommonService, logger);
        }

        private readonly TextTerminalServiceClass TextTerminal;
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


        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the TextTerminal Service.");

        public Task ErrorEvent(CommonStatusClass.ErrorEventIdEnum EventId,
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

        #endregion

        public ITextTerminalService.KeyDetails SupportedKeys { get => TextTerminal.SupportedKeys; set => TextTerminal.SupportedKeys = value; }
        public bool FirstGetKeyDetailCommand { get => TextTerminal.FirstGetKeyDetailCommand; set => TextTerminal.FirstGetKeyDetailCommand = value; }

        public void UpdateKeyDetails() => TextTerminal.UpdateKeyDetails();

        public TextTerminalCapabilitiesClass TextTerminalCapabilities { get => CommonService.TextTerminalCapabilities; set => CommonService.TextTerminalCapabilities = value; }
    }
}