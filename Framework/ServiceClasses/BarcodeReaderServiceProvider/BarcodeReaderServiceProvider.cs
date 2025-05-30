﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a barcode reader service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical barcode reader, which only implements the BarcodeReader and Common interfaces. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class BarcodeReaderServiceProvider : ServiceProvider, IBarcodeReaderService, ICommonService
    {
        public BarcodeReaderServiceProvider(
            EndpointDetails endpointDetails, 
            string ServiceName, 
            IDevice device, 
            ILogger logger)
            :
            base(endpointDetails,
                 ServiceName,
                 [ XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.BarcodeReader ],
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            BarcodeReader = new BarcodeReaderServiceClass(this, logger);
        }

        private readonly BarcodeReaderServiceClass BarcodeReader;
        private readonly CommonServiceClass CommonService;

        #region Common unsolicited events

        public Task StatusChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => CommonService.StatusChangedEvent(sender, propertyInfo);

        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the BarcodeReader Service.");

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
        /// Stores BarcodeReader interface capabilites internally
        /// </summary>
        public BarcodeReaderCapabilitiesClass BarcodeReaderCapabilities { get => CommonService.BarcodeReaderCapabilities; set => CommonService.BarcodeReaderCapabilities = value; }

        /// <summary>
        /// BarcodeReader Status
        /// </summary>
        public BarcodeReaderStatusClass BarcodeReaderStatus { get => CommonService.BarcodeReaderStatus; set => CommonService.BarcodeReaderStatus = value; }


        #endregion
    }
}
