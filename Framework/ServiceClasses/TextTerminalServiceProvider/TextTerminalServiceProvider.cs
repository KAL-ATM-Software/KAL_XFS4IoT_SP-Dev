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


        #region TextTerminal unsolicited events
        public Task FieldWarningEvent() => TextTerminal.FieldWarningEvent();

        public Task KeyEvent(string Key, string CommandKey) => TextTerminal.KeyEvent(new KeyEvent.PayloadData(Key, CommandKey));
        #endregion

        #region Common unsolicited events
        public Task PowerSaveChangeEvent(int PowerSaveRecoveryTime) => CommonService.PowerSaveChangeEvent(new PowerSaveChangeEvent.PayloadData(PowerSaveRecoveryTime));

        public Task DevicePositionEvent(CommonStatusClass.PositionStatusEnum Position) => CommonService.DevicePositionEvent(
                                                                                                        new DevicePositionEvent.PayloadData(Position switch
                                                                                                        {
                                                                                                            CommonStatusClass.PositionStatusEnum.InPosition => XFS4IoT.Common.PositionStatusEnum.InPosition,
                                                                                                            CommonStatusClass.PositionStatusEnum.NotInPosition => XFS4IoT.Common.PositionStatusEnum.NotInPosition,
                                                                                                            _ => XFS4IoT.Common.PositionStatusEnum.Unknown,
                                                                                                        }
                                                                                                    ));

        public Task NonceClearedEvent(string ReasonDescription) => CommonService.NonceClearedEvent(new NonceClearedEvent.PayloadData(ReasonDescription));

        public Task ExchangeStateChangedEvent(CommonStatusClass.ExchangeEnum Exchange) => CommonService.ExchangeStateChangedEvent(
                                                                                                        new ExchangeStateChangedEvent.PayloadData(Exchange switch
                                                                                                        {
                                                                                                            CommonStatusClass.ExchangeEnum.Active => XFS4IoT.Common.ExchangeEnum.Active,
                                                                                                            CommonStatusClass.ExchangeEnum.Inactive => XFS4IoT.Common.ExchangeEnum.Inactive,
                                                                                                            _ => XFS4IoT.Common.ExchangeEnum.NotSupported,
                                                                                                        }
                                                                                                    ));
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