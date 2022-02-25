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

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a biometric service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical biometric device, which only implements the Biometric and Common interfaces. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class BiometricServiceProvider : ServiceProvider, IBiometricService, ICommonService
    {
        public BiometricServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.Biometric },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            Biometric = new BiometricServiceClass(this, logger);
        }

        private readonly BiometricServiceClass Biometric;
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


        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the Biometric Service.");

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

        /// <summary>
        /// Stores Biometric interface capabilites internally
        /// </summary>
        public BiometricCapabilitiesClass BiometricCapabilities { get => CommonService.BiometricCapabilities; set => CommonService.BiometricCapabilities = value; }

        /// <summary>
        /// Biometric Status
        /// </summary>
        public BiometricStatusClass BiometricStatus { get => CommonService.BiometricStatus; set => CommonService.BiometricStatus = value; }

        #endregion

        #region Biometric unsolicited events
        public Task PresentSubjectEvent()
            => Biometric.PresentSubjectEvent();

        public Task SubjectDetectedEvent()
            => Biometric.SubjectDetectedEvent();

        public Task RemoveSubjectEvent()
            => Biometric.RemoveSubjectEvent();

        public Task SubjectRemovedEvent()
            => Biometric.SubjectRemovedEvent();

        public Task DataClearedEvent(BiometricCapabilitiesClass.ClearModesEnum ClearMode)
            => Biometric.DataClearedEvent(new XFS4IoT.Biometric.Events.DataClearedEvent.PayloadData(ClearMode switch
            {
                BiometricCapabilitiesClass.ClearModesEnum.None => null,
                BiometricCapabilitiesClass.ClearModesEnum.ScannedData => XFS4IoT.Biometric.ClearDataEnum.ScannedData,
                BiometricCapabilitiesClass.ClearModesEnum.ImportedData => XFS4IoT.Biometric.ClearDataEnum.ImportedData,
                BiometricCapabilitiesClass.ClearModesEnum.SetMatchedData => XFS4IoT.Biometric.ClearDataEnum.SetMatchedData,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected ClearMode within {nameof(DataClearedEvent)}")
            }));

        public Task OrientationEvent()
            => Biometric.OrientationEvent();
        #endregion
    }
}
