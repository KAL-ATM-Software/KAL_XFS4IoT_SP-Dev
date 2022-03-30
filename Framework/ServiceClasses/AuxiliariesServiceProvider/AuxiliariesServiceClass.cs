/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoTFramework.Auxiliaries;
using XFS4IoT.Auxiliaries;
using XFS4IoT.Auxiliaries.Events;
using System.Threading;

namespace XFS4IoTServer
{

    public partial class AuxiliariesServiceClass
    {
        public AuxiliariesServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(AuxiliariesServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IAuxiliariesDevice>();

            CommonService = ServiceProvider.IsA<ICommonService>($"Invalid interface parameter specified for common service. {nameof(AuxiliariesServiceClass)}");

            StatusChangedRegistry = new(this.Logger, this.ServiceProvider);

            GetCapabilities();
            GetStatus();
        }

        private readonly AuxiliariesStatusChangedRegistry StatusChangedRegistry;
        private readonly ICommonService CommonService;

        #region Auxiliaries Service 
        public Task RegisterStatusChangedEvents(IConnection connection, EventTypesEnum eventIdentifiers, CancellationToken token)
            => StatusChangedRegistry.RegisterStatusChangedEvents(connection, eventIdentifiers, token);

        public Task DeregisterStatusChangedEvents(IConnection connection, EventTypesEnum eventIdentifiers, CancellationToken token)
            => StatusChangedRegistry.DeregisterStatusChangedEvents(connection, eventIdentifiers, token);
        #endregion

        #region Auxiliaries unsolicited events

        public Task OperatorSwitchStateChanged(AuxiliariesStatus.OperatorSwitchEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(newState switch
            {
                AuxiliariesStatus.OperatorSwitchEnum.Run => OperatorSwitchStateEnum.Run,
                AuxiliariesStatus.OperatorSwitchEnum.Maintenance => OperatorSwitchStateEnum.Maintenance,
                AuxiliariesStatus.OperatorSwitchEnum.Supervisor => OperatorSwitchStateEnum.Supervisor,
                AuxiliariesStatus.OperatorSwitchEnum.NotAvailable => OperatorSwitchStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(OperatorSwitchStateChanged)}.")
            })), EventTypesEnum.OperatorSwitch);

        public Task TamperSensorStateChanged(AuxiliariesStatus.SensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(TamperSensor: newState switch
            {
                AuxiliariesStatus.SensorEnum.On => TamperSensorStateEnum.On,
                AuxiliariesStatus.SensorEnum.Off => TamperSensorStateEnum.Off,
                AuxiliariesStatus.SensorEnum.NotAvailable => TamperSensorStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(TamperSensorStateChanged)}.")
            })), EventTypesEnum.TamperSensor);

        public Task InternalTamperSensorStateChanged(AuxiliariesStatus.SensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(InternalTamperSensor: newState switch
            {
                AuxiliariesStatus.SensorEnum.On => InternalTamperSensorStateEnum.On,
                AuxiliariesStatus.SensorEnum.Off => InternalTamperSensorStateEnum.Off,
                AuxiliariesStatus.SensorEnum.NotAvailable => InternalTamperSensorStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(InternalTamperSensorStateChanged)}.")
            })), EventTypesEnum.InternalTamperSensor);

        public Task SeismicSensorStateChanged(AuxiliariesStatus.SensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(SeismicSensorState: newState switch
            {
                AuxiliariesStatus.SensorEnum.On => SeismicSensorStateEnum.On,
                AuxiliariesStatus.SensorEnum.Off => SeismicSensorStateEnum.Off,
                AuxiliariesStatus.SensorEnum.NotAvailable => SeismicSensorStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(SeismicSensorStateChanged)}.")
            })), EventTypesEnum.SeismicSensor);

        public Task HeatSensorStateChanged(AuxiliariesStatus.SensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(HeatSensorState: newState switch
            {
                AuxiliariesStatus.SensorEnum.On => HeatSensorStateEnum.On,
                AuxiliariesStatus.SensorEnum.Off => HeatSensorStateEnum.Off,
                AuxiliariesStatus.SensorEnum.NotAvailable => HeatSensorStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(HeatSensorStateChanged)}.")
            })), EventTypesEnum.HeatSensor);

        public Task ProximitySensorStateChanged(AuxiliariesStatus.PresenceSensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(ProximitySensorState: newState switch
            {
                AuxiliariesStatus.PresenceSensorEnum.Present => ProximitySensorStateEnum.Present,
                AuxiliariesStatus.PresenceSensorEnum.NotPresent => ProximitySensorStateEnum.NotPresent,
                AuxiliariesStatus.PresenceSensorEnum.NotAvailable => ProximitySensorStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(ProximitySensorStateChanged)}.")
            })), EventTypesEnum.ProximitySensor);

        public Task AmbientLightSensorStateChanged(AuxiliariesStatus.AmbientLightSensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(AmbientLightSensorState: newState switch
            {
                AuxiliariesStatus.AmbientLightSensorEnum.VeryDark => AmbientLightSensorStateEnum.VeryDark,
                AuxiliariesStatus.AmbientLightSensorEnum.Dark => AmbientLightSensorStateEnum.Dark,
                AuxiliariesStatus.AmbientLightSensorEnum.MediumLight => AmbientLightSensorStateEnum.MediumLight,
                AuxiliariesStatus.AmbientLightSensorEnum.Light => AmbientLightSensorStateEnum.Light,
                AuxiliariesStatus.AmbientLightSensorEnum.VeryLight => AmbientLightSensorStateEnum.VeryLight,
                AuxiliariesStatus.AmbientLightSensorEnum.NotAvailable => AmbientLightSensorStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(AmbientLightSensorStateChanged)}.")
            })), EventTypesEnum.AmbientLightSensor);

        public Task EnchancedAudioSensorStateChanged(AuxiliariesStatus.PresenceSensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(EnhancedAudioSensorState: newState switch
            {
                AuxiliariesStatus.PresenceSensorEnum.Present => EnhancedAudioSensorStateEnum.Present,
                AuxiliariesStatus.PresenceSensorEnum.NotPresent => EnhancedAudioSensorStateEnum.NotPresent,
                AuxiliariesStatus.PresenceSensorEnum.NotAvailable => EnhancedAudioSensorStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(EnchancedAudioSensorStateChanged)}.")
            })), EventTypesEnum.EnhancedAudio);

        public Task BootSwitchSensorStateChanged(AuxiliariesStatus.SensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(BootSwitchSensorState: newState switch
            {
                AuxiliariesStatus.SensorEnum.On => BootSwitchSensorStateEnum.On,
                AuxiliariesStatus.SensorEnum.Off => BootSwitchSensorStateEnum.Off,
                AuxiliariesStatus.SensorEnum.NotAvailable => BootSwitchSensorStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(BootSwitchSensorStateChanged)}.")
            })), EventTypesEnum.BootSwitch);

        public Task DisplaySensorStateChanged(AuxiliariesStatus.DisplaySensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(DisplaySensorState: newState switch
            {
                AuxiliariesStatus.DisplaySensorEnum.Off => DisplaySensorStateEnum.Off,
                AuxiliariesStatus.DisplaySensorEnum.On => DisplaySensorStateEnum.On,
                AuxiliariesStatus.DisplaySensorEnum.DisplayError => DisplaySensorStateEnum.DisplayError,
                _ => DisplaySensorStateEnum.NotAvailable
            })), EventTypesEnum.ConsumerDisplay);

        public Task OperatorCallButtonSensorStateChanged(AuxiliariesStatus.SensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(OperatorCallButtonSensorState: newState switch
            {
                AuxiliariesStatus.SensorEnum.On => OperatorCallButtonSensorStateEnum.On,
                AuxiliariesStatus.SensorEnum.Off => OperatorCallButtonSensorStateEnum.Off,
                AuxiliariesStatus.SensorEnum.NotAvailable => OperatorCallButtonSensorStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(OperatorCallButtonSensorStateChanged)}.")
            })), EventTypesEnum.OperatorCallButton);

        public Task HandsetSensorStateChanged(AuxiliariesStatus.HandsetSensorStatusEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(HandsetSensorState: newState switch
            {
                AuxiliariesStatus.HandsetSensorStatusEnum.OnTheHook => HandsetSensorStateEnum.OnTheHook,
                AuxiliariesStatus.HandsetSensorStatusEnum.OffTheHook => HandsetSensorStateEnum.OffTheHook,
                AuxiliariesStatus.HandsetSensorStatusEnum.NotAvailable => HandsetSensorStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(HandsetSensorStateChanged)}.")
            })), EventTypesEnum.HandsetSensor);

        public Task HeadsetMicrophoneSensorStateChanged(AuxiliariesStatus.PresenceSensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(HeadsetMicrophoneSensorState: newState switch
            {
                AuxiliariesStatus.PresenceSensorEnum.Present => HeadsetMicrophoneSensorStateEnum.Present,
                AuxiliariesStatus.PresenceSensorEnum.NotPresent => HeadsetMicrophoneSensorStateEnum.NotPresent,
                AuxiliariesStatus.PresenceSensorEnum.NotAvailable => HeadsetMicrophoneSensorStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(HeadsetMicrophoneSensorStateChanged)}.")
            })), EventTypesEnum.HeadsetMicrophone);

        public Task FasciaMicrophoneSensorStateChanged(AuxiliariesStatus.SensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(FasciaMicrophoneSensorState: newState switch
            {
                AuxiliariesStatus.SensorEnum.On => FasciaMicrophoneSensorStateEnum.On,
                AuxiliariesStatus.SensorEnum.Off => FasciaMicrophoneSensorStateEnum.Off,
                AuxiliariesStatus.SensorEnum.NotAvailable => FasciaMicrophoneSensorStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(FasciaMicrophoneSensorStateChanged)}.")
            })), EventTypesEnum.FasciaMicrophone);

        public Task DoorSensorStateChanged(AuxiliariesCapabilities.DoorType door, AuxiliariesStatus.DoorStatusEnum newState)
        {
            switch (door)
            {
                case AuxiliariesCapabilities.DoorType.Safe:
                    return StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(SafeDoorState: newState switch
                    {
                        AuxiliariesStatus.DoorStatusEnum.Closed => SafeDoorStateEnum.Closed,
                        AuxiliariesStatus.DoorStatusEnum.Open => SafeDoorStateEnum.Open,
                        AuxiliariesStatus.DoorStatusEnum.Locked => SafeDoorStateEnum.Locked,
                        AuxiliariesStatus.DoorStatusEnum.Bolted => SafeDoorStateEnum.Bolted,
                        AuxiliariesStatus.DoorStatusEnum.Tampered => SafeDoorStateEnum.Tampered,
                        AuxiliariesStatus.DoorStatusEnum.NotAvailable => SafeDoorStateEnum.NotAvailable,
                        _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(DoorSensorStateChanged)}.")
                    })), EventTypesEnum.SafeDoor);

                case AuxiliariesCapabilities.DoorType.FrontCabinet:
                    return StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(CabinetFrontDoorState: newState switch
                    {
                        AuxiliariesStatus.DoorStatusEnum.Closed => CabinetFrontDoorStateEnum.Closed,
                        AuxiliariesStatus.DoorStatusEnum.Open => CabinetFrontDoorStateEnum.Open,
                        AuxiliariesStatus.DoorStatusEnum.Locked => CabinetFrontDoorStateEnum.Locked,
                        AuxiliariesStatus.DoorStatusEnum.Bolted => CabinetFrontDoorStateEnum.Bolted,
                        AuxiliariesStatus.DoorStatusEnum.Tampered => CabinetFrontDoorStateEnum.Tampered,
                        AuxiliariesStatus.DoorStatusEnum.NotAvailable => CabinetFrontDoorStateEnum.NotAvailable,
                        _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(DoorSensorStateChanged)}.")
                    })), EventTypesEnum.CabinetFront);

                case AuxiliariesCapabilities.DoorType.RearCabinet:
                    return StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(CabinetRearDoorState: newState switch
                    {
                        AuxiliariesStatus.DoorStatusEnum.Closed => CabinetRearDoorStateEnum.Closed,
                        AuxiliariesStatus.DoorStatusEnum.Open => CabinetRearDoorStateEnum.Open,
                        AuxiliariesStatus.DoorStatusEnum.Locked => CabinetRearDoorStateEnum.Locked,
                        AuxiliariesStatus.DoorStatusEnum.Bolted => CabinetRearDoorStateEnum.Bolted,
                        AuxiliariesStatus.DoorStatusEnum.Tampered => CabinetRearDoorStateEnum.Tampered,
                        AuxiliariesStatus.DoorStatusEnum.NotAvailable => CabinetRearDoorStateEnum.NotAvailable,
                        _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(DoorSensorStateChanged)}.")
                    })), EventTypesEnum.CabinetRear);

                case AuxiliariesCapabilities.DoorType.LeftCabinet:
                    return StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(CabinetLeftDoorState: newState switch
                    {
                        AuxiliariesStatus.DoorStatusEnum.Closed => CabinetLeftDoorStateEnum.Closed,
                        AuxiliariesStatus.DoorStatusEnum.Open => CabinetLeftDoorStateEnum.Open,
                        AuxiliariesStatus.DoorStatusEnum.Locked => CabinetLeftDoorStateEnum.Locked,
                        AuxiliariesStatus.DoorStatusEnum.Bolted => CabinetLeftDoorStateEnum.Bolted,
                        AuxiliariesStatus.DoorStatusEnum.Tampered => CabinetLeftDoorStateEnum.Tampered,
                        AuxiliariesStatus.DoorStatusEnum.NotAvailable => CabinetLeftDoorStateEnum.NotAvailable,
                        _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(DoorSensorStateChanged)}.")
                    })), EventTypesEnum.CabinetLeft);

                case AuxiliariesCapabilities.DoorType.RightCabinet:
                    return StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(CabinetRightDoorState: newState switch
                    {
                        AuxiliariesStatus.DoorStatusEnum.Closed => CabinetRightDoorStateEnum.Closed,
                        AuxiliariesStatus.DoorStatusEnum.Open => CabinetRightDoorStateEnum.Open,
                        AuxiliariesStatus.DoorStatusEnum.Locked => CabinetRightDoorStateEnum.Locked,
                        AuxiliariesStatus.DoorStatusEnum.Bolted => CabinetRightDoorStateEnum.Bolted,
                        AuxiliariesStatus.DoorStatusEnum.Tampered => CabinetRightDoorStateEnum.Tampered,
                        AuxiliariesStatus.DoorStatusEnum.NotAvailable => CabinetRightDoorStateEnum.NotAvailable,
                        _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(DoorSensorStateChanged)}.")
                    })), EventTypesEnum.CabinetRight);

                default:
                    throw Contracts.Fail<NotImplementedException>("Unexpected door type passed to DoorSensorStateChanged.");
            }
        }

        public Task VandalShieldSensorStateChanged(AuxiliariesStatus.VandalShieldStatusEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(VandalShieldState: newState switch
            {
                AuxiliariesStatus.VandalShieldStatusEnum.Closed => VandalShieldStateEnum.Closed,
                AuxiliariesStatus.VandalShieldStatusEnum.Open => VandalShieldStateEnum.Open,
                AuxiliariesStatus.VandalShieldStatusEnum.Locked => VandalShieldStateEnum.Locked,
                AuxiliariesStatus.VandalShieldStatusEnum.Service => VandalShieldStateEnum.Service,
                AuxiliariesStatus.VandalShieldStatusEnum.Keyboard => VandalShieldStateEnum.Keyboard,
                AuxiliariesStatus.VandalShieldStatusEnum.PartiallyOpen => VandalShieldStateEnum.PartiallyOpen,
                AuxiliariesStatus.VandalShieldStatusEnum.Jammed => VandalShieldStateEnum.Jammed,
                AuxiliariesStatus.VandalShieldStatusEnum.Tampered => VandalShieldStateEnum.Tampered,
                AuxiliariesStatus.VandalShieldStatusEnum.NotAvailable => VandalShieldStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(VandalShieldSensorStateChanged)}.")
            })), EventTypesEnum.VandalShield);

        public Task OpenClosedIndicatorStateChanged(AuxiliariesStatus.OpenClosedIndicatorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(OpenClosedIndicatorState: newState switch
            {
                AuxiliariesStatus.OpenClosedIndicatorEnum.Closed => OpenClosedIndicatorStateEnum.Closed,
                AuxiliariesStatus.OpenClosedIndicatorEnum.Open => OpenClosedIndicatorStateEnum.Open,
                AuxiliariesStatus.OpenClosedIndicatorEnum.NotAvailable => OpenClosedIndicatorStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(OpenClosedIndicatorStateChanged)}.")
            })), EventTypesEnum.OpenCloseIndicator);

        public Task AudioStateChanged(AuxiliariesStatus.AudioRateEnum newRate, AuxiliariesStatus.AudioSignalEnum newSignal)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(AudioState:
                new AudioStateClass(
                    newRate switch
                    {
                        AuxiliariesStatus.AudioRateEnum.On => AudioStateClass.RateEnum.On,
                        AuxiliariesStatus.AudioRateEnum.Off => AudioStateClass.RateEnum.Off,
                        _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newRate in {nameof(AudioStateChanged)}.")
                    },
                    newSignal switch
                    {
                        AuxiliariesStatus.AudioSignalEnum.Exclamation => AudioStateClass.SignalEnum.Exclamation,
                        AuxiliariesStatus.AudioSignalEnum.Warning => AudioStateClass.SignalEnum.Warning,
                        AuxiliariesStatus.AudioSignalEnum.Error => AudioStateClass.SignalEnum.Error,
                        AuxiliariesStatus.AudioSignalEnum.Critical => AudioStateClass.SignalEnum.Critical,
                        AuxiliariesStatus.AudioSignalEnum.Keypress => AudioStateClass.SignalEnum.Keypress,
                        _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newSignal in {nameof(AudioStateChanged)}.")
                    }))), EventTypesEnum.AudioIndicator);

        public Task HeatingStateChanged(AuxiliariesStatus.SensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(HeatingState: newState switch
            {
                AuxiliariesStatus.SensorEnum.On => HeatingStateEnum.On,
                AuxiliariesStatus.SensorEnum.Off => HeatingStateEnum.Off,
                AuxiliariesStatus.SensorEnum.NotAvailable => HeatingStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(HeatingStateChanged)}.")
            })), EventTypesEnum.HeatingIndicator);

        public Task ConsumerDisplayBacklightStateChanged(AuxiliariesStatus.SensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(ConsumerDisplayBacklightState: newState switch
            {
                AuxiliariesStatus.SensorEnum.On => ConsumerDisplayBacklightStateEnum.On,
                AuxiliariesStatus.SensorEnum.Off => ConsumerDisplayBacklightStateEnum.Off,
                AuxiliariesStatus.SensorEnum.NotAvailable => ConsumerDisplayBacklightStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(ConsumerDisplayBacklightStateChanged)}.")
            })), EventTypesEnum.ConsumerDisplayBacklight);

        public Task SignageDisplayStateChanged(AuxiliariesStatus.SensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(SignageDisplayState: newState switch
            {
                AuxiliariesStatus.SensorEnum.On => SignageDisplayStateEnum.On,
                AuxiliariesStatus.SensorEnum.Off => SignageDisplayStateEnum.Off,
                AuxiliariesStatus.SensorEnum.NotAvailable => SignageDisplayStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(SignageDisplayStateChanged)}.")
            })), EventTypesEnum.SignageDisplay);

        public Task VolumeStateChanged(int newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(
                VolumeState: new(newState))), EventTypesEnum.VolumeControl);

        public Task UpsStateChanged(AuxiliariesStatus.UpsStatusEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(UpsState: new(
                newState.HasFlag(AuxiliariesStatus.UpsStatusEnum.Low),
                newState.HasFlag(AuxiliariesStatus.UpsStatusEnum.Engaged),
                newState.HasFlag(AuxiliariesStatus.UpsStatusEnum.Powering),
                newState.HasFlag(AuxiliariesStatus.UpsStatusEnum.Recovered)
                ))), EventTypesEnum.Ups);

        public Task AudibleAlarmStateChanged(AuxiliariesStatus.SensorEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(AudibleAlarmState: newState switch
            {
                AuxiliariesStatus.SensorEnum.On => AudibleAlarmStateEnum.On,
                AuxiliariesStatus.SensorEnum.Off => AudibleAlarmStateEnum.Off,
                AuxiliariesStatus.SensorEnum.NotAvailable => AudibleAlarmStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(AudibleAlarmStateChanged)}.")
            })), EventTypesEnum.AudibleAlarm);

        public Task EnhancedAudioControlStateChanged(AuxiliariesStatus.EnhancedAudioControlEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(EnhancedAudioControlState: newState switch
            {
                AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioManual => EnhancedAudioControlStateEnum.PublicAudioManual,
                AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioAuto => EnhancedAudioControlStateEnum.PublicAudioAuto,
                AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioSemiAuto => EnhancedAudioControlStateEnum.PublicAudioSemiAuto,
                AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioManual => EnhancedAudioControlStateEnum.PrivateAudioManual,
                AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioAuto => EnhancedAudioControlStateEnum.PrivateAudioAuto,
                AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioSemiAuto => EnhancedAudioControlStateEnum.PrivateAudioSemiAuto,
                AuxiliariesStatus.EnhancedAudioControlEnum.NotAvailable => EnhancedAudioControlStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(EnhancedAudioControlStateChanged)}.")
            })), EventTypesEnum.EnhancedAudioControl);

        public Task EnhancedMicrophoneControlStateChanged(AuxiliariesStatus.EnhancedAudioControlEnum newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(EnhancedMicrophoneControlState: newState switch
            {
                AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioManual => EnhancedMicrophoneControlStateEnum.PublicAudioManual,
                AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioAuto => EnhancedMicrophoneControlStateEnum.PublicAudioAuto,
                AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioSemiAuto => EnhancedMicrophoneControlStateEnum.PublicAudioSemiAuto,
                AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioManual => EnhancedMicrophoneControlStateEnum.PrivateAudioManual,
                AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioAuto => EnhancedMicrophoneControlStateEnum.PrivateAudioAuto,
                AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioSemiAuto => EnhancedMicrophoneControlStateEnum.PrivateAudioSemiAuto,
                AuxiliariesStatus.EnhancedAudioControlEnum.NotAvailable => EnhancedMicrophoneControlStateEnum.NotAvailable,
                _ => throw Contracts.Fail<NotImplementedException>($"Unexpected newState in {nameof(EnhancedMicrophoneControlStateChanged)}.")
            })), EventTypesEnum.EnhancedMicrophoneControl);

        public Task MicrophoneVolumeStateChanged(AuxiliariesStatus.MicrophoneVolumeStatus newState)
            => StatusChangedRegistry.BroadcastRegisteredStatusChangedEvents(new AuxiliaryStatusEvent(new(
                MicrophoneVolumeState: new(newState.Available, newState.VolumeLevel))), EventTypesEnum.MicrophoneVolume);
        #endregion

        private void GetStatus()
        {
            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.AuxiliariesStatus");
            CommonService.AuxiliariesStatus = Device.AuxiliariesStatus;
            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.AuxiliariesStatus=");

            CommonService.AuxiliariesStatus.IsNotNull($"The device class set AuxiliariesStatus property to null. The device class must report device status.");
        }

        private void GetCapabilities()
        {
            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.AuxiliariesCapabilities");
            CommonService.AuxiliariesCapabilities = Device.AuxiliariesCapabilities;
            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.AuxiliariesCapabilities=");

            CommonService.AuxiliariesCapabilities.IsNotNull($"The device class set AuxiliariesCapabilities property to null. The device class must report device capabilities.");
        }

    }
}
