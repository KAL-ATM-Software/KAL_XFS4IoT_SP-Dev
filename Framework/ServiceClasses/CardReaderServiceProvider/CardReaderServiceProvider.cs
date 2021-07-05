/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * CardReaderServiceProvider.cs.cs uses automatically generated parts. 
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoT.CardReader.Events;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CardReader;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a card reader service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical card reader, which only implements the CardReader and Common interfaces. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class CardReaderServiceProvider : ServiceProvider, ICardReaderServiceClass, ICommonServiceClass
    {
        public CardReaderServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.CardReader },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger);
            CardReader = new CardReaderServiceClass(this, CommonService, logger);
        }

        private readonly CardReaderServiceClass CardReader;
        private readonly CommonServiceClass CommonService;

        #region CardReader unsolicited events
        public Task MediaRemovedEvent() => CardReader.MediaRemovedEvent();

        public Task RetainBinThresholdEvent(RetainBinThresholdEvent.PayloadData Payload) => CardReader.RetainBinThresholdEvent(Payload);

        public Task CardActionEvent(CardActionEvent.PayloadData Payload) => CardReader.CardActionEvent(Payload);
        #endregion

        #region Common unsolicited events
        public Task PowerSaveChangeEvent(PowerSaveChangeEvent.PayloadData Payload) => CommonService.PowerSaveChangeEvent(Payload);

        public Task DevicePositionEvent(DevicePositionEvent.PayloadData Payload) => CommonService.DevicePositionEvent(Payload);
        #endregion

        /// <summary>
        /// Stores CardReader interface capabilites internally
        /// </summary>
        public CardReaderCapabilitiesClass CardReaderCapabilities { get => CommonService.CardReaderCapabilities; set => CommonService.CardReaderCapabilities = value; }

    }
}
