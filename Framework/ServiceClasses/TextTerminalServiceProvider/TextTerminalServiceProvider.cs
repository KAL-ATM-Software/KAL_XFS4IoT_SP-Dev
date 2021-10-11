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
    public class TextTerminalServiceProvider : ServiceProvider, ITextTerminalServiceClass, ICommonServiceClass
    {
        public TextTerminalServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.TextTerminal },
                 device,
                 logger)
        {
            Common = new CommonServiceClass(this, logger);
            TextTerminal = new TextTerminalServiceClass(this, Common, logger);
        }

        private readonly TextTerminalServiceClass TextTerminal;
        private readonly CommonServiceClass Common;


        #region TextTerminal unsolicited events
        public Task FieldErrorEvent(FieldErrorEvent.PayloadData Payload) => TextTerminal.FieldErrorEvent(Payload);

        public Task FieldWarningEvent() => TextTerminal.FieldWarningEvent();

        public Task KeyEvent(KeyEvent.PayloadData Payload) => TextTerminal.KeyEvent(Payload);
        #endregion

        #region Common unsolicited events
        public Task PowerSaveChangeEvent(PowerSaveChangeEvent.PayloadData Payload) => Common.PowerSaveChangeEvent(Payload);

        public Task DevicePositionEvent(DevicePositionEvent.PayloadData Payload) => Common.DevicePositionEvent(Payload);

        public Task NonceClearedEvent(NonceClearedEvent.PayloadData Payload) => Common.NonceClearedEvent(Payload);

        public Task ExchangeStateChangedEvent(ExchangeStateChangedEvent.PayloadData Payload) => Common.ExchangeStateChangedEvent(Payload);
        #endregion

        public ITextTerminalService.KeyDetails SupportedKeys { get => TextTerminal.SupportedKeys; set => TextTerminal.SupportedKeys = value; }
        public bool FirstGetKeyDetailCommand { get => TextTerminal.FirstGetKeyDetailCommand; set => TextTerminal.FirstGetKeyDetailCommand = value; }

        public void UpdateKeyDetails() => TextTerminal.UpdateKeyDetails();

        public TextTerminalCapabilitiesClass TextTerminalCapabilities { get => Common.TextTerminalCapabilities; set => Common.TextTerminalCapabilities = value; }
    }
}