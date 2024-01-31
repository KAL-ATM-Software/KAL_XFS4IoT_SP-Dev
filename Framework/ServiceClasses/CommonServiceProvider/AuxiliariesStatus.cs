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
    public sealed class AuxiliariesStatusClass : StatusBase
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

        public AuxiliariesStatusClass(OperatorSwitchEnum OperatorSwitch = OperatorSwitchEnum.NotAvailable,
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
                                 Dictionary<AuxiliariesCapabilitiesClass.DoorType, DoorStatusClass> Doors = null,
                                 OpenClosedIndicatorEnum OpenClosedIndicator = OpenClosedIndicatorEnum.NotAvailable,
                                 AudioRateEnum AudioRate = AudioRateEnum.Off,
                                 AudioSignalEnum AudioSignal = AudioSignalEnum.Keypress,
                                 SensorEnum Heating = SensorEnum.NotAvailable,
                                 SensorEnum ConsumerDisplayBacklight = SensorEnum.NotAvailable,
                                 SensorEnum SignageDisplay = SensorEnum.NotAvailable,
                                 int Volume = 0,
                                 UpsStatusEnum UPS = UpsStatusEnum.NotAvailable,
                                 SensorEnum AudibleAlarm = SensorEnum.NotAvailable,
                                 EnhancedAudioControlEnum EnhancedAudioControl = EnhancedAudioControlEnum.NotAvailable,
                                 EnhancedAudioControlEnum EnhancedMicrophoneControl = EnhancedAudioControlEnum.NotAvailable,
                                 int MicrophoneVolume = 0)
        {
            operatorSwitch = OperatorSwitch;
            tamperSensor = TamperSensor;
            internalTamperSensor = InternalTamperSensor;
            seismicSensor = SeismicSensor;
            heatSensor = HeatSensor;
            proximitySensor = ProximitySensor;
            ambientLightSensor = AmbientLightSensor;
            enhancedAudioSensor = EnhancedAudioSensor;
            bootSwitchSensor = BootSwitchSensor;
            displaySensor = DisplaySensor;
            operatorCallButtonSensor = OperatorCallButtonSensor;
            handsetSensor = HandsetSensor;
            headsetMicrophoneSensor = HeadsetMicrophoneSensor;
            fasciaMicrophoneSensor = FasciaMicrophoneSensor;
            vandalShield = VandalShield;
            this.Doors = Doors;
            openClosedIndicator = OpenClosedIndicator;
            audioRate = AudioRate;
            audioSignal = AudioSignal;
            heating = Heating;
            consumerDisplayBacklight = ConsumerDisplayBacklight;
            signageDisplay = SignageDisplay;
            volume = Volume;
            ups = UPS;
            audibleAlarm = AudibleAlarm;
            enhancedAudioControl = EnhancedAudioControl;
            enhancedMicrophoneControl = EnhancedMicrophoneControl;
            microphoneVolume = MicrophoneVolume;
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
        private OperatorSwitchEnum operatorSwitch = OperatorSwitchEnum.NotAvailable;

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
        private SensorEnum tamperSensor = SensorEnum.NotAvailable;

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
        private SensorEnum internalTamperSensor = SensorEnum.NotAvailable;

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
        private SensorEnum seismicSensor = SensorEnum.NotAvailable;

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
        private SensorEnum heatSensor = SensorEnum.NotAvailable;

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
        private PresenceSensorEnum proximitySensor = PresenceSensorEnum.NotAvailable;

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
        private AmbientLightSensorEnum ambientLightSensor = AmbientLightSensorEnum.NotAvailable;

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
        private PresenceSensorEnum enhancedAudioSensor = PresenceSensorEnum.NotAvailable;

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
        private SensorEnum bootSwitchSensor = SensorEnum.NotAvailable;

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
        private DisplaySensorEnum displaySensor = DisplaySensorEnum.NotAvailable;

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
        private SensorEnum operatorCallButtonSensor = SensorEnum.NotAvailable;

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
        private HandsetSensorStatusEnum handsetSensor = HandsetSensorStatusEnum.NotAvailable;

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
        private PresenceSensorEnum headsetMicrophoneSensor = PresenceSensorEnum.NotAvailable;

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
        private SensorEnum fasciaMicrophoneSensor = SensorEnum.NotAvailable;

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
        private VandalShieldStatusEnum vandalShield = VandalShieldStatusEnum.NotAvailable;

        public Dictionary<AuxiliariesCapabilitiesClass.DoorType, DoorStatusClass> Doors { get; init; }

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
        private OpenClosedIndicatorEnum openClosedIndicator = OpenClosedIndicatorEnum.NotAvailable;

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
        private AudioRateEnum audioRate = AudioRateEnum.NotAvailable;

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
        private AudioSignalEnum audioSignal = AudioSignalEnum.NotAvailable;

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
        private SensorEnum heating = SensorEnum.NotAvailable;

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
        private SensorEnum consumerDisplayBacklight = SensorEnum.NotAvailable;

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
        private SensorEnum signageDisplay = SensorEnum.NotAvailable;

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
        private int volume = 0;

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
        private UpsStatusEnum ups = UpsStatusEnum.NotAvailable;

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
        private SensorEnum audibleAlarm = SensorEnum.NotAvailable;

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
        private EnhancedAudioControlEnum enhancedAudioControl = EnhancedAudioControlEnum.NotAvailable;

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
        private EnhancedAudioControlEnum enhancedMicrophoneControl = EnhancedAudioControlEnum.NotAvailable;
        
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
        private int microphoneVolume = 0;
    }
}
