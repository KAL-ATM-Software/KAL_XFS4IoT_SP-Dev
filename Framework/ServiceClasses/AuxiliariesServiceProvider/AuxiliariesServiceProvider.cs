/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Auxiliaries;
using System.Threading;

namespace XFS4IoTServer
{
    /// <summary>
    /// Default implimentation of a auxiliaries service provider. 
    /// </summary>
    /// <remarks> 
    /// This represents a typical auxiliaries service, which only implements the Auxiliaries, Common and Lights interfaces. 
    /// It's possible to create other service provider types by combining multiple service classes in the 
    /// same way. 
    /// </remarks>
    public class AuxiliariesServiceProvider : ServiceProvider, IAuxiliariesService, ICommonService, ILightsService
    {
        public AuxiliariesServiceProvider(EndpointDetails endpointDetails, string ServiceName, IDevice device, ILogger logger)
            :
            base(endpointDetails,
                 ServiceName,
                 new[] { XFSConstants.ServiceClass.Common, XFSConstants.ServiceClass.Auxiliaries, XFSConstants.ServiceClass.Lights },
                 device,
                 logger)
        {
            CommonService = new CommonServiceClass(this, logger, ServiceName);
            LightsService = new LightsServiceClass(this, logger);
            Auxiliaries = new AuxiliariesServiceClass(this, logger);
        }

        private readonly AuxiliariesServiceClass Auxiliaries;
        private readonly CommonServiceClass CommonService;
        private readonly LightsServiceClass LightsService;

        #region Auxiliaries unsolicited events
        public Task OperatorSwitchStateChanged(AuxiliariesStatus.OperatorSwitchEnum newState)
            => Auxiliaries.OperatorSwitchStateChanged(newState);

        public Task TamperSensorStateChanged(AuxiliariesStatus.SensorEnum newState)
            => Auxiliaries.TamperSensorStateChanged(newState);

        public Task InternalTamperSensorStateChanged(AuxiliariesStatus.SensorEnum newState)
            => Auxiliaries.InternalTamperSensorStateChanged(newState);

        public Task SeismicSensorStateChanged(AuxiliariesStatus.SensorEnum newState)
            => Auxiliaries.SeismicSensorStateChanged(newState);

        public Task HeatSensorStateChanged(AuxiliariesStatus.SensorEnum newState)
            => Auxiliaries.HeatSensorStateChanged(newState);

        public Task ProximitySensorStateChanged(AuxiliariesStatus.PresenceSensorEnum newState)
            => Auxiliaries.ProximitySensorStateChanged(newState);

        public Task AmbientLightSensorStateChanged(AuxiliariesStatus.AmbientLightSensorEnum newState)
            => Auxiliaries.AmbientLightSensorStateChanged(newState);

        public Task EnchancedAudioSensorStateChanged(AuxiliariesStatus.PresenceSensorEnum newState)
            => Auxiliaries.EnchancedAudioSensorStateChanged(newState);

        public Task BootSwitchSensorStateChanged(AuxiliariesStatus.SensorEnum newState)
            => Auxiliaries.BootSwitchSensorStateChanged(newState);

        public Task DisplaySensorStateChanged(AuxiliariesStatus.DisplaySensorEnum newState)
            => Auxiliaries.DisplaySensorStateChanged(newState);

        public Task OperatorCallButtonSensorStateChanged(AuxiliariesStatus.SensorEnum newState)
            => Auxiliaries.OperatorCallButtonSensorStateChanged(newState);

        public Task HandsetSensorStateChanged(AuxiliariesStatus.HandsetSensorStatusEnum newState)
            => Auxiliaries.HandsetSensorStateChanged(newState);

        public Task HeadsetMicrophoneSensorStateChanged(AuxiliariesStatus.PresenceSensorEnum newState)
            => Auxiliaries.HeadsetMicrophoneSensorStateChanged(newState);

        public Task FasciaMicrophoneSensorStateChanged(AuxiliariesStatus.SensorEnum newState)
            => Auxiliaries.FasciaMicrophoneSensorStateChanged(newState);

        public Task DoorSensorStateChanged(AuxiliariesCapabilities.DoorType door, AuxiliariesStatus.DoorStatusEnum newState)
            => Auxiliaries.DoorSensorStateChanged(door, newState);

        public Task VandalShieldSensorStateChanged(AuxiliariesStatus.VandalShieldStatusEnum newState)
            => Auxiliaries.VandalShieldSensorStateChanged(newState);

        public Task OpenClosedIndicatorStateChanged(AuxiliariesStatus.OpenClosedIndicatorEnum newState)
            => Auxiliaries.OpenClosedIndicatorStateChanged(newState);

        public Task AudioStateChanged(AuxiliariesStatus.AudioRateEnum newRate, AuxiliariesStatus.AudioSignalEnum newSignal)
            => Auxiliaries.AudioStateChanged(newRate, newSignal);

        public Task HeatingStateChanged(AuxiliariesStatus.SensorEnum newState)
            => Auxiliaries.HeatingStateChanged(newState);

        public Task ConsumerDisplayBacklightStateChanged(AuxiliariesStatus.SensorEnum newState)
            => Auxiliaries.ConsumerDisplayBacklightStateChanged(newState);

        public Task SignageDisplayStateChanged(AuxiliariesStatus.SensorEnum newState)
            => Auxiliaries.SignageDisplayStateChanged(newState);

        public Task VolumeStateChanged(int newState)
            => Auxiliaries.VolumeStateChanged(newState);

        public Task UpsStateChanged(AuxiliariesStatus.UpsStatusEnum newState)
            => Auxiliaries.UpsStateChanged(newState);

        public Task AudibleAlarmStateChanged(AuxiliariesStatus.SensorEnum newState)
            => Auxiliaries.AudibleAlarmStateChanged(newState);

        public Task EnhancedAudioControlStateChanged(AuxiliariesStatus.EnhancedAudioControlEnum newState)
            => Auxiliaries.EnhancedAudioControlStateChanged(newState);

        public Task EnhancedMicrophoneControlStateChanged(AuxiliariesStatus.EnhancedAudioControlEnum newState)
            => Auxiliaries.EnhancedMicrophoneControlStateChanged(newState);

        public Task MicrophoneVolumeStateChanged(AuxiliariesStatus.MicrophoneVolumeStatus newState)
            => Auxiliaries.MicrophoneVolumeStateChanged(newState);
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

        public Task NonceClearedEvent(string ReasonDescription) => throw new NotImplementedException("NonceClearedEvent is not supported in the Auxiliaries Service.");

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
        /// Stores Auxiliaries interface capabilites internally
        /// </summary>
        public AuxiliariesCapabilities AuxiliariesCapabilities { get => CommonService.AuxiliariesCapabilities; set => CommonService.AuxiliariesCapabilities = value; }

        /// <summary>
        /// Auxiliaries Status
        /// </summary>
        public AuxiliariesStatus AuxiliariesStatus { get => CommonService.AuxiliariesStatus; set => CommonService.AuxiliariesStatus = value; }

        /// <summary>
        /// Stores Lights interface capabilites internally
        /// </summary>
        public LightsCapabilitiesClass LightsCapabilities { get => CommonService.LightsCapabilities; set => CommonService.LightsCapabilities = value; }

        /// <summary>
        /// Lights Status
        /// </summary>
        public LightsStatusClass LightsStatus { get => CommonService.LightsStatus; set => CommonService.LightsStatus = value; }


        #endregion

        #region Auxiliaries Service
        public Task RegisterStatusChangedEvents(IConnection connection, EventTypesEnum eventIdentifiers, CancellationToken token)
            => Auxiliaries.RegisterStatusChangedEvents(connection, eventIdentifiers, token);

        public Task DeregisterStatusChangedEvents(IConnection connection, EventTypesEnum eventIdentifiers, CancellationToken token)
            => Auxiliaries.DeregisterStatusChangedEvents(connection, eventIdentifiers, token);
        #endregion
    }
}
