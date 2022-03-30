/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Common
{
    public sealed class AuxiliariesStatus
    {
        public AuxiliariesStatus(OperatorSwitchEnum OperatorSwitch = OperatorSwitchEnum.NotAvailable,
                                 SensorEnum TamperSensor = SensorEnum.NotAvailable,
                                 SensorEnum InternalTamperSensor = SensorEnum.NotAvailable,
                                 SensorEnum SeismicSensor = SensorEnum.NotAvailable,
                                 SensorEnum HeatSensor = SensorEnum.NotAvailable,
                                 PresenceSensorEnum ProximitySensor = PresenceSensorEnum.NotAvailable,
                                 AmbientLightSensorEnum AmbientLightSensor = AmbientLightSensorEnum.NotAvailable,
                                 PresenceSensorEnum EnhancedAudioSensor = PresenceSensorEnum.NotAvailable,
                                 SensorEnum BootSwitchSensor = SensorEnum.NotAvailable,
                                 DisplaySensorEnum DisplaySensor = DisplaySensorEnum.NotAvailable,
                                 SensorEnum OperatorCallButtonSensor = SensorEnum.NotAvailable,
                                 HandsetSensorStatusEnum HandsetSensor = HandsetSensorStatusEnum.NotAvailable,
                                 PresenceSensorEnum HeadsetMicrophoneSensor = PresenceSensorEnum.NotAvailable,
                                 SensorEnum FasciaMicrophoneSensor = SensorEnum.NotAvailable,
                                 VandalShieldStatusEnum VandalShield = VandalShieldStatusEnum.NotAvailable,
                                 Dictionary<AuxiliariesCapabilities.DoorType, DoorStatusEnum> Doors = null,
                                 OpenClosedIndicatorEnum OpenClosedIndicator = OpenClosedIndicatorEnum.NotAvailable,
                                 AudioRateEnum AudioRate = AudioRateEnum.Off,
                                 AudioSignalEnum AudioSignal = AudioSignalEnum.Keypress,
                                 SensorEnum Heating = SensorEnum.NotAvailable,
                                 SensorEnum ConsumerDisplayBacklight = SensorEnum.NotAvailable,
                                 SensorEnum SignageDisplay = SensorEnum.NotAvailable,
                                 int Volume = 1,
                                 UpsStatusEnum UPS = UpsStatusEnum.NotAvailable,
                                 SensorEnum AudibleAlarm = SensorEnum.NotAvailable,
                                 EnhancedAudioControlEnum EnhancedAudioControl = EnhancedAudioControlEnum.NotAvailable,
                                 EnhancedAudioControlEnum EnhancedMicrophoneControl = EnhancedAudioControlEnum.NotAvailable,
                                 MicrophoneVolumeStatus MicrophoneVolume = null)
        {
            this.OperatorSwitch = OperatorSwitch;
            this.TamperSensor = TamperSensor;
            this.InternalTamperSensor = InternalTamperSensor;
            this.SeismicSensor = SeismicSensor;
            this.HeatSensor = HeatSensor;
            this.ProximitySensor = ProximitySensor;
            this.AmbientLightSensor = AmbientLightSensor;
            this.EnhancedAudioSensor = EnhancedAudioSensor;
            this.BootSwitchSensor = BootSwitchSensor;
            this.DisplaySensor = DisplaySensor;
            this.OperatorCallButtonSensor = OperatorCallButtonSensor;
            this.HandsetSensor = HandsetSensor;
            this.HeadsetMicrophoneSensor = HeadsetMicrophoneSensor;
            this.FasciaMicrophoneSensor = FasciaMicrophoneSensor;
            this.VandalShield = VandalShield;
            this.Doors = Doors;
            this.OpenClosedIndicator = OpenClosedIndicator;
            this.AudioRate = AudioRate;
            this.AudioSignal = AudioSignal;
            this.Heating = Heating;
            this.ConsumerDisplayBacklight = ConsumerDisplayBacklight;
            this.SignageDisplay = SignageDisplay;
            this.Volume = Volume;
            this.UPS = UPS;
            this.AudibleAlarm = AudibleAlarm;
            this.EnhancedAudioControl = EnhancedAudioControl;
            this.EnhancedMicrophoneControl = EnhancedMicrophoneControl;
            this.MicrophoneVolume = MicrophoneVolume;
        }

        public enum SensorEnum
        {
            NotAvailable,
            On,
            Off
        }

        public enum OperatorSwitchEnum
        {
            NotAvailable,
            Run,
            Maintenance,
            Supervisor
        }

        public enum PresenceSensorEnum
        {
            NotAvailable,
            Present,
            NotPresent
        }

        public enum AmbientLightSensorEnum
        {
            NotAvailable,
            VeryDark,
            Dark,
            MediumLight,
            Light,
            VeryLight
        }
        public enum DisplaySensorEnum
        {
            NotAvailable,
            Off,
            On,
            DisplayError
        }

        public enum HandsetSensorStatusEnum
        {
            NotAvailable,
            OnTheHook,
            OffTheHook
        }

        public enum DoorStatusEnum
        {
            NotAvailable,
            Closed,
            Open,
            Locked,
            Bolted,
            Tampered
        }

        public enum VandalShieldStatusEnum
        {
            NotAvailable,
            Closed,
            Open,
            Locked,
            Service,
            Keyboard,
            PartiallyOpen,
            Jammed,
            Tampered
        }

        public enum OpenClosedIndicatorEnum
        {
            NotAvailable,
            Closed,
            Open
        }

        public enum EnhancedAudioControlEnum
        {
            NotAvailable,
            PublicAudioManual,
            PublicAudioAuto,
            PublicAudioSemiAuto,
            PrivateAudioManual,
            PrivateAudioAuto,
            PrivateAudioSemiAuto
        }

        [Flags]
        public enum UpsStatusEnum
        {
            NotAvailable = 0, 
            Low = 1 << 0,
            Engaged = 1 << 1,
            Powering = 1 << 2,
            Recovered = 1 << 3,
        }

        public OperatorSwitchEnum OperatorSwitch { get; set; }

        public SensorEnum TamperSensor { get; set; }

        public SensorEnum InternalTamperSensor { get; set; }

        public SensorEnum SeismicSensor { get; set; }

        public SensorEnum HeatSensor { get; set; }

        public PresenceSensorEnum ProximitySensor { get; set; }

        public AmbientLightSensorEnum AmbientLightSensor { get; set; }

        public PresenceSensorEnum EnhancedAudioSensor { get; set; }

        public SensorEnum BootSwitchSensor { get; set; }

        public DisplaySensorEnum DisplaySensor { get; set; }

        public SensorEnum OperatorCallButtonSensor { get; set; }

        public HandsetSensorStatusEnum HandsetSensor { get; set; }

        public PresenceSensorEnum HeadsetMicrophoneSensor { get; set; }

        public SensorEnum FasciaMicrophoneSensor { get; set; }

        public VandalShieldStatusEnum VandalShield { get; set; }
        
        public Dictionary<AuxiliariesCapabilities.DoorType, DoorStatusEnum> Doors { get; set; }

        public OpenClosedIndicatorEnum OpenClosedIndicator { get; set; }

        public enum AudioRateEnum { On, Off }
        public enum AudioSignalEnum { Keypress, Exclamation, Warning, Error, Critical }

        public AudioRateEnum AudioRate { get; set; }

        public AudioSignalEnum AudioSignal { get; set; }

        public SensorEnum Heating { get; set; }

        public SensorEnum ConsumerDisplayBacklight { get; set; }

        public SensorEnum SignageDisplay { get; set; }

        public int Volume { get; set; }

        public UpsStatusEnum UPS { get; set; }

        public SensorEnum AudibleAlarm { get; set; }

        public EnhancedAudioControlEnum EnhancedAudioControl { get; set; }

        public EnhancedAudioControlEnum EnhancedMicrophoneControl { get; set; }

        public record MicrophoneVolumeStatus(bool Available, int VolumeLevel);
        public MicrophoneVolumeStatus MicrophoneVolume { get; set; }
    }
}
