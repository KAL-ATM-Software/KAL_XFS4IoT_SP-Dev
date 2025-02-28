/***********************************************************************************************\
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
using XFS4IoT.Biometric.Events;
using XFS4IoT.Common.Events;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a camera service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical camera device, which only implements the Camera and Common interfaces. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class CameraServiceProvider : ServiceProvider, ICameraService, ICommonService
    {
        public CameraServiceProvider(
            EndpointDetails endpointDetails, 
            string ServiceName, 
            IDevice device, 
            ILogger logger, 
            IPersistentData persistentData)
            :
            base(endpointDetails,
                 ServiceName,
                 [XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.Camera],
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            Camera = new CameraServiceClass(this, logger);
        }

        private readonly CameraServiceClass Camera;
        private readonly CommonServiceClass CommonService;

        #region Common unsolicited events

        public Task StatusChangedEvent(object sender, PropertyChangedEventArgs propertyInfo) => CommonService.StatusChangedEvent(sender, propertyInfo);

        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the Camera Service.");

        public Task ErrorEvent(
            CommonStatusClass.ErrorEventIdEnum EventId,
            CommonStatusClass.ErrorActionEnum Action,
            string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);

        #endregion

        #region Camera events

        public enum MediaThresholdEnum
        {
            Healthy, //The recording media is a good state.
            High,    // The recording media is almost full.
            Full     //The recording media is full.
        }
        /// <summary>
        /// This event is used to specify that the state of the recording media reached a threshold.
        /// </summary>
        public Task MediaThresholdEvent(MediaThresholdEnum Threshold)
            => Camera.MediaThresholdEvent(new(Threshold switch
            {
                MediaThresholdEnum.Full => XFS4IoT.Camera.Events.MediaThresholdEvent.PayloadData.MediaThresholdEnum.Full,
                MediaThresholdEnum.High => XFS4IoT.Camera.Events.MediaThresholdEvent.PayloadData.MediaThresholdEnum.High,
                MediaThresholdEnum.Healthy => XFS4IoT.Camera.Events.MediaThresholdEvent.PayloadData.MediaThresholdEnum.Ok,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected ClearMode within {nameof(DataClearedEvent)}")
            }));

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
        /// Stores Camera interface capabilites internally
        /// </summary>
        public CameraCapabilitiesClass CameraCapabilities { get => CommonService.CameraCapabilities; set => CommonService.CameraCapabilities = value; }

        /// <summary>
        /// Camera Status
        /// </summary>
        public CameraStatusClass CameraStatus { get => CommonService.CameraStatus; set => CommonService.CameraStatus = value; }

        #endregion
    }
}
