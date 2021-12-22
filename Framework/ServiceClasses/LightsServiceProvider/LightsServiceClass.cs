/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Lights;

namespace XFS4IoTServer
{
    public partial class LightsServiceClass
    {
        public LightsServiceClass(IServiceProvider ServiceProvider,
                                  ICommonService CommonService,
                                  ILogger logger)
            : this(ServiceProvider, logger)
        {
            CommonService.IsNotNull($"Unexpected parameter set for common service in the " + nameof(LightsServiceClass));
            this.CommonService = CommonService.IsA<ICommonService>($"Invalid interface parameter specified for common service. " + nameof(LightsServiceClass));

            GetCapabilities();
            GetStatus();
        }

        #region Common Service
        /// <summary>
        /// Common service interface
        /// </summary>
        public ICommonService CommonService { get; init; }

        /// <summary>
        /// Stores Common interface capabilites internally
        /// </summary>
        public CommonCapabilitiesClass CommonCapabilities { get => CommonService.CommonCapabilities; set => CommonService.CommonCapabilities = value; }

        /// <summary>
        /// Common Status
        /// </summary>
        public CommonStatusClass CommonStatus { get => CommonService.CommonStatus; set => CommonService.CommonStatus = value; }

        /// <summary>
        /// Stores Light capabilities
        /// </summary>
        public LightsCapabilitiesClass LightsCapabilities { get => CommonService.LightsCapabilities; set => CommonService.LightsCapabilities = value; }

        /// <summary>
        /// Stores Light Status
        /// </summary>
        public LightsStatusClass LightsStatus { get => CommonService.LightsStatus; set => CommonService.LightsStatus = value; }

        #endregion

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
        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the Crypto Service.");

        public Task ErrorEvent(CommonStatusClass.ErrorEventIdEnum EventId,
                               CommonStatusClass.ErrorActionEnum Action,
                               string VendorDescription) => CommonService.ErrorEvent(EventId, Action, VendorDescription);

        #endregion

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "LightsDev.LightsStatus");
            LightsStatus = Device.LightsStatus;
            Logger.Log(Constants.DeviceClass, "LightsDev.LightsStatus=");

            LightsStatus.IsNotNull($"The device class set LightsStatus property to null. The device class must report device status.");
        }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "LightsDev.LightsCapabilities");
            LightsCapabilities = Device.LightsCapabilities;
            Logger.Log(Constants.DeviceClass, "LightsDev.LightsCapabilities=");

            LightsCapabilities.IsNotNull($"The device class set LightsCapabilities property to null. The device class must report device capabilities.");
        }
    }
}
