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
    public sealed class AuxiliariesStatusClass(
        AuxiliariesStatusClass.OperatorSwitchEnum OperatorSwitch = AuxiliariesStatusClass.OperatorSwitchEnum.NotAvailable,
        AuxiliariesStatusClass.SensorEnum TamperSensor = AuxiliariesStatusClass.SensorEnum.NotAvailable,
        AuxiliariesStatusClass.SensorEnum InternalTamperSensor = AuxiliariesStatusClass.SensorEnum.NotAvailable,
        AuxiliariesStatusClass.SensorEnum SeismicSensor = AuxiliariesStatusClass.SensorEnum.NotAvailable,
        AuxiliariesStatusClass.SensorEnum HeatSensor = AuxiliariesStatusClass.SensorEnum.NotAvailable,
        AuxiliariesStatusClass.PresenceSensorEnum ProximitySensor = AuxiliariesStatusClass.PresenceSensorEnum.NotAvailable,
        AuxiliariesStatusClass.AmbientLightSensorEnum AmbientLightSensor = AuxiliariesStatusClass.AmbientLightSensorEnum.NotAvailable,
        AuxiliariesStatusClass.PresenceSensorEnum EnhancedAudioSensor = AuxiliariesStatusClass.PresenceSensorEnum.NotAvailable,
        AuxiliariesStatusClass.SensorEnum BootSwitchSensor = AuxiliariesStatusClass.SensorEnum.NotAvailable,
        AuxiliariesStatusClass.DisplaySensorEnum DisplaySensor = AuxiliariesStatusClass.DisplaySensorEnum.NotAvailable,
        AuxiliariesStatusClass.SensorEnum OperatorCallButtonSensor = AuxiliariesStatusClass.SensorEnum.NotAvailable,
        AuxiliariesStatusClass.HandsetSensorStatusEnum HandsetSensor = AuxiliariesStatusClass.HandsetSensorStatusEnum.NotAvailable,
        AuxiliariesStatusClass.PresenceSensorEnum HeadsetMicrophoneSensor = AuxiliariesStatusClass.PresenceSensorEnum.NotAvailable,
        AuxiliariesStatusClass.SensorEnum FasciaMicrophoneSensor = AuxiliariesStatusClass.SensorEnum.NotAvailable,
        AuxiliariesStatusClass.VandalShieldStatusEnum VandalShield = AuxiliariesStatusClass.VandalShieldStatusEnum.NotAvailable,
        Dictionary<AuxiliariesCapabilitiesClass.DoorType, AuxiliariesStatusClass.DoorStatusClass> Doors = null,
        AuxiliariesStatusClass.OpenClosedIndicatorEnum OpenClosedIndicator = AuxiliariesStatusClass.OpenClosedIndicatorEnum.NotAvailable,
        AuxiliariesStatusClass.AudioRateEnum AudioRate = AuxiliariesStatusClass.AudioRateEnum.Off,
        AuxiliariesStatusClass.AudioSignalEnum AudioSignal = AuxiliariesStatusClass.AudioSignalEnum.Keypress,
        AuxiliariesStatusClass.SensorEnum Heating = AuxiliariesStatusClass.SensorEnum.NotAvailable,
        AuxiliariesStatusClass.SensorEnum ConsumerDisplayBacklight = AuxiliariesStatusClass.SensorEnum.NotAvailable,
        AuxiliariesStatusClass.SensorEnum SignageDisplay = AuxiliariesStatusClass.SensorEnum.NotAvailable,
        int Volume = 0,
        AuxiliariesStatusClass.UpsStatusEnum UPS = AuxiliariesStatusClass.UpsStatusEnum.NotAvailable,
        AuxiliariesStatusClass.SensorEnum AudibleAlarm = AuxiliariesStatusClass.SensorEnum.NotAvailable,
        AuxiliariesStatusClass.EnhancedAudioControlEnum EnhancedAudioControl = AuxiliariesStatusClass.EnhancedAudioControlEnum.NotAvailable,
        AuxiliariesStatusClass.EnhancedAudioControlEnum EnhancedMicrophoneControl = AuxiliariesStatusClass.EnhancedAudioControlEnum.NotAvailable,
        int MicrophoneVolume = 0) : StatusBase
    {
        public sealed class DoorStatusClass(DoorStatusEnum DoorStatus) : StatusBase
        {
            /// <summary>
            /// This property is set by the framework for StatusChangedEvent generation
            /// </summary>
            public AuxiliariesCapabilitiesClass.DoorType? Type { get; set; } = null;

            public DoorStatusEnum DoorStatus
            {
                get { return doorStatus; }
                set
                {
                    if (doorStatus != value)
                    {
                        doorStatus = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            private DoorStatusEnum doorStatus = DoorStatus;
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
            Good = 1 << 4, // Good battery state, either Low or Good.
        }

        public OperatorSwitchEnum OperatorSwitch 
        { 
            get { return operatorSwitch; }
            set
            {
                if (operatorSwitch != value)
                {
                    operatorSwitch = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private OperatorSwitchEnum operatorSwitch = OperatorSwitch;

        public SensorEnum TamperSensor 
        { 
            get { return tamperSensor; }
            set
            {
                if (tamperSensor != value)
                {
                    tamperSensor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private SensorEnum tamperSensor = TamperSensor;

        public SensorEnum InternalTamperSensor 
        { 
            get { return internalTamperSensor; }
            set
            {
                if (internalTamperSensor != value)
                {
                    internalTamperSensor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private SensorEnum internalTamperSensor = InternalTamperSensor;

        public SensorEnum SeismicSensor 
        { 
            get { return seismicSensor; }
            set
            {
                if (seismicSensor != value)
                {
                    seismicSensor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private SensorEnum seismicSensor = SeismicSensor;

        public SensorEnum HeatSensor 
        { 
            get { return heatSensor; }
            set
            {
                if (heatSensor != value)
                {
                    heatSensor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private SensorEnum heatSensor = HeatSensor;

        public PresenceSensorEnum ProximitySensor 
        { 
            get { return proximitySensor; }
            set
            {
                if (proximitySensor != value)
                {  
                    proximitySensor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private PresenceSensorEnum proximitySensor = ProximitySensor;

        public AmbientLightSensorEnum AmbientLightSensor 
        { 
            get { return ambientLightSensor; }
            set
            {
                if (ambientLightSensor != value)
                {
                    ambientLightSensor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private AmbientLightSensorEnum ambientLightSensor = AmbientLightSensor;

        public PresenceSensorEnum EnhancedAudioSensor 
        { 
            get { return enhancedAudioSensor; }
            set
            {
                if (enhancedAudioSensor != value)
                {
                    enhancedAudioSensor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private PresenceSensorEnum enhancedAudioSensor = EnhancedAudioSensor;

        public SensorEnum BootSwitchSensor 
        { 
            get { return bootSwitchSensor; }
            set
            {
                if (bootSwitchSensor != value)
                {
                    bootSwitchSensor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private SensorEnum bootSwitchSensor = BootSwitchSensor;

        public DisplaySensorEnum DisplaySensor
        { 
            get { return displaySensor; }
            set
            {
                if (displaySensor != value)
                {
                    displaySensor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private DisplaySensorEnum displaySensor = DisplaySensor;

        public SensorEnum OperatorCallButtonSensor 
        {
            get { return operatorCallButtonSensor; } 
            set
            {
                if (operatorCallButtonSensor != value)
                {
                    operatorCallButtonSensor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private SensorEnum operatorCallButtonSensor = OperatorCallButtonSensor;

        public HandsetSensorStatusEnum HandsetSensor 
        { 
            get { return handsetSensor; }
            set
            {
                if (handsetSensor != value)
                {
                    handsetSensor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private HandsetSensorStatusEnum handsetSensor = HandsetSensor;

        public PresenceSensorEnum HeadsetMicrophoneSensor 
        { 
            get { return headsetMicrophoneSensor; }
            set
            {
                if (headsetMicrophoneSensor != value)
                {
                    headsetMicrophoneSensor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private PresenceSensorEnum headsetMicrophoneSensor = HeadsetMicrophoneSensor;

        public SensorEnum FasciaMicrophoneSensor 
        { 
            get { return fasciaMicrophoneSensor; }
            set
            {
                fasciaMicrophoneSensor = value;
                if (fasciaMicrophoneSensor != value)
                {
                    fasciaMicrophoneSensor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private SensorEnum fasciaMicrophoneSensor = FasciaMicrophoneSensor;

        public VandalShieldStatusEnum VandalShield 
        { 
            get { return vandalShield; }
            set
            {
                if (vandalShield != value)
                {
                    vandalShield = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private VandalShieldStatusEnum vandalShield = VandalShield;

        public Dictionary<AuxiliariesCapabilitiesClass.DoorType, DoorStatusClass> Doors { get; init; } = Doors;

        public OpenClosedIndicatorEnum OpenClosedIndicator 
        { 
            get { return openClosedIndicator; }
            set
            {
                if (openClosedIndicator != value)
                {
                    openClosedIndicator = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private OpenClosedIndicatorEnum openClosedIndicator = OpenClosedIndicator;

        public enum AudioRateEnum 
        {
            On,
            Off,
            NotAvailable,
        }
        public enum AudioSignalEnum 
        { 
            Keypress, 
            Exclamation, 
            Warning, 
            Error, 
            Critical,
            NotAvailable,
        }

        public AudioRateEnum AudioRate 
        { 
            get { return audioRate; }
            set
            {
                if (audioRate != value)
                {  
                    audioRate = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private AudioRateEnum audioRate = AudioRate;

        public AudioSignalEnum AudioSignal 
        { 
            get { return audioSignal; }
            set
            {
                if (audioSignal != value)
                {
                    audioSignal = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private AudioSignalEnum audioSignal = AudioSignal;

        public SensorEnum Heating 
        { 
            get { return heating; }
            set
            {
                if (heating != value)
                {
                    heating = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private SensorEnum heating = Heating;

        public SensorEnum ConsumerDisplayBacklight 
        { 
            get { return consumerDisplayBacklight; }
            set
            {
                if (consumerDisplayBacklight != value)
                {
                    consumerDisplayBacklight = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private SensorEnum consumerDisplayBacklight = ConsumerDisplayBacklight;

        public SensorEnum SignageDisplay 
        { 
            get { return signageDisplay; }
            set
            {
                if (signageDisplay != value)
                {
                    signageDisplay = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private SensorEnum signageDisplay = SignageDisplay;

        /// <summary>
        /// This property is zero if the device doesn't support volume.
        /// </summary>
        public int Volume 
        { 
            get { return volume; }
            set
            {
                if (volume != value)
                {
                    volume = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int volume = Volume;

        public UpsStatusEnum UPS 
        { 
            get {  return ups; }
            set
            {
                if (ups != value)
                {
                    ups = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private UpsStatusEnum ups = UPS;

        public SensorEnum AudibleAlarm 
        { 
            get { return audibleAlarm; }
            set
            {
                if (audibleAlarm != value)
                {
                    audibleAlarm = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private SensorEnum audibleAlarm = AudibleAlarm;

        public EnhancedAudioControlEnum EnhancedAudioControl 
        {
            get { return enhancedAudioControl; }
            set
            {
                if (enhancedAudioControl  != value)
                {
                    enhancedAudioControl = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private EnhancedAudioControlEnum enhancedAudioControl = EnhancedAudioControl;

        public EnhancedAudioControlEnum EnhancedMicrophoneControl 
        {
            get { return enhancedMicrophoneControl; }
            set
            {
                if (enhancedMicrophoneControl != value)
                {
                    enhancedMicrophoneControl = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private EnhancedAudioControlEnum enhancedMicrophoneControl = EnhancedMicrophoneControl;
        
        /// <summary>
        /// This property is zero if the device doesn't support volume.
        /// </summary>
        public int MicrophoneVolume 
        {
            get { return microphoneVolume; }
            set
            {
                if (microphoneVolume != value)
                {
                    microphoneVolume = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int microphoneVolume = MicrophoneVolume;
    }
}
