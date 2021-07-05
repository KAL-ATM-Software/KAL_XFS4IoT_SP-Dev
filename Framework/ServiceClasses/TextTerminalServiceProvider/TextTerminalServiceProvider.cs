using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Common.Events;
using XFS4IoT.TextTerminal.Events;
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
    class TextTerminalServiceProvider : ServiceProvider, ITextTerminalServiceClass, ICommonServiceClass
    {
        public TextTerminalServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.TextTerminal },
                 device,
                 logger)
        {
            TextTerminal = new TextTerminalServiceClass(this, logger);
            Common = new CommonServiceClass(this, logger);
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
        #endregion

    }
}