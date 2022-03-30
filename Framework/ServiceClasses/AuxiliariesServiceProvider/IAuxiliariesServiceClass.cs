/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * AuxiliariesServiceClass.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Threading.Tasks;

using XFS4IoTFramework.Auxiliaries;
using XFS4IoT.Auxiliaries.Events;
using XFS4IoTFramework.Common;
using XFS4IoT.Auxiliaries;
using System;
using System.Threading;

namespace XFS4IoTServer
{
    public interface IAuxiliariesService
    {

        public Task OperatorSwitchStateChanged(AuxiliariesStatus.OperatorSwitchEnum newState);
        public Task TamperSensorStateChanged(AuxiliariesStatus.SensorEnum newState);
        public Task InternalTamperSensorStateChanged(AuxiliariesStatus.SensorEnum newState);
        public Task SeismicSensorStateChanged(AuxiliariesStatus.SensorEnum newState);
        public Task HeatSensorStateChanged(AuxiliariesStatus.SensorEnum newState);
        public Task ProximitySensorStateChanged(AuxiliariesStatus.PresenceSensorEnum newState);
        public Task AmbientLightSensorStateChanged(AuxiliariesStatus.AmbientLightSensorEnum newState);
        public Task EnchancedAudioSensorStateChanged(AuxiliariesStatus.PresenceSensorEnum newState);
        public Task BootSwitchSensorStateChanged(AuxiliariesStatus.SensorEnum newState);
        public Task DisplaySensorStateChanged(AuxiliariesStatus.DisplaySensorEnum newState);
        public Task OperatorCallButtonSensorStateChanged(AuxiliariesStatus.SensorEnum newState);
        public Task HandsetSensorStateChanged(AuxiliariesStatus.HandsetSensorStatusEnum newState);
        public Task HeadsetMicrophoneSensorStateChanged(AuxiliariesStatus.PresenceSensorEnum newState);
        public Task FasciaMicrophoneSensorStateChanged(AuxiliariesStatus.SensorEnum newState);
        public Task DoorSensorStateChanged(AuxiliariesCapabilities.DoorType door, AuxiliariesStatus.DoorStatusEnum newState);
        public Task VandalShieldSensorStateChanged(AuxiliariesStatus.VandalShieldStatusEnum newState);
        public Task OpenClosedIndicatorStateChanged(AuxiliariesStatus.OpenClosedIndicatorEnum newState);
        public Task AudioStateChanged(AuxiliariesStatus.AudioRateEnum newRate, AuxiliariesStatus.AudioSignalEnum newSignal);
        public Task HeatingStateChanged(AuxiliariesStatus.SensorEnum newState);
        public Task ConsumerDisplayBacklightStateChanged(AuxiliariesStatus.SensorEnum newState);
        public Task SignageDisplayStateChanged(AuxiliariesStatus.SensorEnum newState);
        public Task VolumeStateChanged(int newState);
        public Task UpsStateChanged(AuxiliariesStatus.UpsStatusEnum newState);
        public Task AudibleAlarmStateChanged(AuxiliariesStatus.SensorEnum newState);
        public Task EnhancedAudioControlStateChanged(AuxiliariesStatus.EnhancedAudioControlEnum newState);
        public Task EnhancedMicrophoneControlStateChanged(AuxiliariesStatus.EnhancedAudioControlEnum newState);
        public Task MicrophoneVolumeStateChanged(AuxiliariesStatus.MicrophoneVolumeStatus newState);

        public Task RegisterStatusChangedEvents(IConnection connection, EventTypesEnum eventIdentifiers, CancellationToken token);
        public Task DeregisterStatusChangedEvents(IConnection connection, EventTypesEnum eventIdentifiers, CancellationToken token);
    }

    public interface IAuxiliariesServiceClass : IAuxiliariesService, IAuxiliariesUnsolicitedEvents
    {
    }
}
