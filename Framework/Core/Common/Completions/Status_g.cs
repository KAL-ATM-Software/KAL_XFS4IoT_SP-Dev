/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * Status_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Common.Completions
{
    [DataContract]
    [Completion(Name = "Common.Status")]
    public sealed class StatusCompletion : Completion<StatusCompletion.PayloadData>
    {
        public StatusCompletion(string RequestId, StatusCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            /// <summary>
            ///Status information common to all XFS4IoT services.
            /// </summary>
            public class CommonClass
            {
                public enum DeviceEnum
                {
                    Online,
                    Offline,
                    PowerOff,
                    NoDevice,
                    HardwareError,
                    UserError,
                    DeviceBusy,
                    FraudAttempt,
                    PotentialFraud,
                }
                [DataMember(Name = "device")] 
                public DeviceEnum? Device { get; private set; }
                [DataMember(Name = "extra")] 
                public List<string> Extra { get; private set; }
                
                /// <summary>
                ///Specifies the state of the guidance light indicators. A number of guidance light types are defined below. Vendor specific guidance lights are defined starting from the end of the array.
                /// </summary>
                public class GuideLightsClass 
                {
                    public enum FlashRateEnum
                    {
                        NotSupported,
                        Off,
                        Slow,
                        Medium,
                        Quick,
                        Continuous,
                    }
                    [DataMember(Name = "flashRate")] 
                    public FlashRateEnum? FlashRate { get; private set; }
                    public enum ColorEnum
                    {
                        NotSupported,
                        Off,
                        Red,
                        Green,
                        Yellow,
                        Blue,
                        Cyan,
                        Magenta,
                        White,
                    }
                    [DataMember(Name = "color")] 
                    public ColorEnum? Color { get; private set; }
                    public enum DirectionEnum
                    {
                        NotSupported,
                        Off,
                        Entry,
                        Exit,
                    }
                    [DataMember(Name = "direction")] 
                    public DirectionEnum? Direction { get; private set; }

                    public GuideLightsClass (FlashRateEnum? FlashRate, ColorEnum? Color, DirectionEnum? Direction)
                    {
                        this.FlashRate = FlashRate;
                        this.Color = Color;
                        this.Direction = Direction;
                    }
                }
                [DataMember(Name = "guideLights")] 
                public GuideLightsClass GuideLights { get; private set; }
                public enum DevicePositionEnum
                {
                    Inposition,
                    Notinposition,
                    Posunknown,
                }
                [DataMember(Name = "devicePosition")] 
                public DevicePositionEnum? DevicePosition { get; private set; }
                [DataMember(Name = "powerSaveRecoveryTime")] 
                public int? PowerSaveRecoveryTime { get; private set; }
                public enum AntiFraudModuleEnum
                {
                    NotSupp,
                    Ok,
                    Inop,
                    DeviceDetected,
                    Unknown,
                }
                [DataMember(Name = "antiFraudModule")] 
                public AntiFraudModuleEnum? AntiFraudModule { get; private set; }

                public CommonClass (DeviceEnum? Device, List<string> Extra, GuideLightsClass GuideLights, DevicePositionEnum? DevicePosition, int? PowerSaveRecoveryTime, AntiFraudModuleEnum? AntiFraudModule)
                {
                    this.Device = Device;
                    this.Extra = Extra;
                    this.GuideLights = GuideLights;
                    this.DevicePosition = DevicePosition;
                    this.PowerSaveRecoveryTime = PowerSaveRecoveryTime;
                    this.AntiFraudModule = AntiFraudModule;
                }


            }

            /// <summary>
            ///Status information for XFS4IoT services implementing the CardReader interface. This will be omitted if the CardReader interface is not supported.
            /// </summary>
            public class CardReaderClass
            {
                public enum MediaEnum
                {
                    NotSupported,
                    Unknown,
                    Present,
                    NotPresent,
                    Jammed,
                    Entering,
                    Latched,
                }
                [DataMember(Name = "media")] 
                public MediaEnum? Media { get; private set; }
                public enum RetainBinEnum
                {
                    NotSupported,
                    Ok,
                    Full,
                    High,
                    Missing,
                }
                [DataMember(Name = "retainBin")] 
                public RetainBinEnum? RetainBin { get; private set; }
                public enum SecurityEnum
                {
                    NotSupported,
                    Ready,
                    Open,
                }
                [DataMember(Name = "security")] 
                public SecurityEnum? Security { get; private set; }
                [DataMember(Name = "numberCards")] 
                public int? NumberCards { get; private set; }
                public enum ChipPowerEnum
                {
                    NotSupported,
                    Unknown,
                    Online,
                    Busy,
                    PoweredOff,
                    NoDevice,
                    HardwareError,
                    NoCard,
                }
                [DataMember(Name = "chipPower")] 
                public ChipPowerEnum? ChipPower { get; private set; }
                public enum ChipModuleEnum
                {
                    Ok,
                    Inoperable,
                    Unknown,
                    NotSupported,
                }
                [DataMember(Name = "chipModule")] 
                public ChipModuleEnum? ChipModule { get; private set; }
                public enum MagWriteModuleEnum
                {
                    Ok,
                    Inoperable,
                    Unknown,
                    NotSupported,
                }
                [DataMember(Name = "magWriteModule")] 
                public MagWriteModuleEnum? MagWriteModule { get; private set; }
                public enum FrontImageModuleEnum
                {
                    Ok,
                    Inoperable,
                    Unknown,
                    NotSupported,
                }
                [DataMember(Name = "frontImageModule")] 
                public FrontImageModuleEnum? FrontImageModule { get; private set; }
                public enum BackImageModuleEnum
                {
                    Ok,
                    Inoperable,
                    Unknown,
                    NotSupported,
                }
                [DataMember(Name = "backImageModule")] 
                public BackImageModuleEnum? BackImageModule { get; private set; }
                [DataMember(Name = "parkingStationMedia")] 
                public List<string> ParkingStationMedia { get; private set; }

                public CardReaderClass (MediaEnum? Media, RetainBinEnum? RetainBin, SecurityEnum? Security, int? NumberCards, ChipPowerEnum? ChipPower, ChipModuleEnum? ChipModule, MagWriteModuleEnum? MagWriteModule, FrontImageModuleEnum? FrontImageModule, BackImageModuleEnum? BackImageModule, List<string> ParkingStationMedia)
                {
                    this.Media = Media;
                    this.RetainBin = RetainBin;
                    this.Security = Security;
                    this.NumberCards = NumberCards;
                    this.ChipPower = ChipPower;
                    this.ChipModule = ChipModule;
                    this.MagWriteModule = MagWriteModule;
                    this.FrontImageModule = FrontImageModule;
                    this.BackImageModule = BackImageModule;
                    this.ParkingStationMedia = ParkingStationMedia;
                }


            }

            /// <summary>
            ///Status information for XFS4IoT services implementing the CashAcceptor interface. This will be omitted if the CashAcceptor interface is not supported.
            /// </summary>
            public class CashAcceptorClass
            {
                public enum IntermediateStackerEnum
                {
                    Empty,
                    NotEmpty,
                    Full,
                    Unknown,
                    NotSupported,
                }
                [DataMember(Name = "intermediateStacker")] 
                public IntermediateStackerEnum? IntermediateStacker { get; private set; }
                public enum StackerItemsEnum
                {
                    CustomerAccess,
                    NoCustomerAccess,
                    AccessUnknown,
                    NoItems,
                }
                [DataMember(Name = "stackerItems")] 
                public StackerItemsEnum? StackerItems { get; private set; }
                public enum BanknoteReaderEnum
                {
                    CustomokerAccess,
                    Inoperable,
                    Unknown,
                    NotSupported,
                }
                [DataMember(Name = "banknoteReader")] 
                public BanknoteReaderEnum? BanknoteReader { get; private set; }
                [DataMember(Name = "dropBox")] 
                public bool? DropBox { get; private set; }
                
                /// <summary>
                ///Array of structures for each position from which items can be accepted.
                /// </summary>
                public class PositionsClass 
                {
                    public enum PositionEnum
                    {
                        InLeft,
                        InRight,
                        InCenter,
                        InTop,
                        InBottom,
                        InFront,
                        InRear,
                        OutLeft,
                        OutRight,
                        OutCenter,
                        OutTop,
                        OutBottom,
                        OutFront,
                        OutRear,
                    }
                    [DataMember(Name = "position")] 
                    public PositionEnum? Position { get; private set; }
                    public enum ShutterEnum
                    {
                        Closed,
                        Open,
                        Jammed,
                        Unknown,
                        NotSupported,
                    }
                    [DataMember(Name = "shutter")] 
                    public ShutterEnum? Shutter { get; private set; }
                    public enum PositionStatusEnum
                    {
                        Empty,
                        NotEmpty,
                        Unknown,
                        NotSupported,
                    }
                    [DataMember(Name = "positionStatus")] 
                    public PositionStatusEnum? PositionStatus { get; private set; }
                    public enum TransportEnum
                    {
                        Ok,
                        Inoperative,
                        Unknown,
                        NotSupported,
                    }
                    [DataMember(Name = "transport")] 
                    public TransportEnum? Transport { get; private set; }
                    public enum TransportStatusEnum
                    {
                        Empty,
                        NotEmpty,
                        NotEmptyCustomer,
                        NotEmptyUnkown,
                        NotSupported,
                    }
                    [DataMember(Name = "transportStatus")] 
                    public TransportStatusEnum? TransportStatus { get; private set; }
                    public enum JammedShutterPositionEnum
                    {
                        NotSupported,
                        NotJammed,
                        Open,
                        PartiallyOpen,
                        Closed,
                        Unknown,
                    }
                    [DataMember(Name = "jammedShutterPosition")] 
                    public JammedShutterPositionEnum? JammedShutterPosition { get; private set; }

                    public PositionsClass (PositionEnum? Position, ShutterEnum? Shutter, PositionStatusEnum? PositionStatus, TransportEnum? Transport, TransportStatusEnum? TransportStatus, JammedShutterPositionEnum? JammedShutterPosition)
                    {
                        this.Position = Position;
                        this.Shutter = Shutter;
                        this.PositionStatus = PositionStatus;
                        this.Transport = Transport;
                        this.TransportStatus = TransportStatus;
                        this.JammedShutterPosition = JammedShutterPosition;
                    }
                }
                [DataMember(Name = "positions")] 
                public PositionsClass Positions { get; private set; }
                public enum MixedModeEnum
                {
                    NotActive,
                    Active,
                }
                [DataMember(Name = "mixedMode")] 
                public MixedModeEnum? MixedMode { get; private set; }

                public CashAcceptorClass (IntermediateStackerEnum? IntermediateStacker, StackerItemsEnum? StackerItems, BanknoteReaderEnum? BanknoteReader, bool? DropBox, PositionsClass Positions, MixedModeEnum? MixedMode)
                {
                    this.IntermediateStacker = IntermediateStacker;
                    this.StackerItems = StackerItems;
                    this.BanknoteReader = BanknoteReader;
                    this.DropBox = DropBox;
                    this.Positions = Positions;
                    this.MixedMode = MixedMode;
                }


            }

            /// <summary>
            ///Status information for XFS4IoT services implementing the CashDispenser interface. This will be omitted if the CashDispenser interface is not supported.
            /// </summary>
            public class CashDispenserClass
            {
                public enum IntermediateStackerEnum
                {
                    Empty,
                    NotEmpty,
                    NotEmptyCustomer,
                    NotEmptyUnknown,
                    Unknown,
                    NotSupported,
                }
                [DataMember(Name = "intermediateStacker")] 
                public IntermediateStackerEnum? IntermediateStacker { get; private set; }
                
                /// <summary>
                ///Array of structures for each position to which items can be dispensed or presented.
                /// </summary>
                public class PositionsClass 
                {
                    public enum PositionEnum
                    {
                        Left,
                        Right,
                        Center,
                        Top,
                        Bottom,
                        Front,
                        Rear,
                    }
                    [DataMember(Name = "position")] 
                    public PositionEnum? Position { get; private set; }
                    public enum ShutterEnum
                    {
                        Closed,
                        Open,
                        Jammed,
                        Unknown,
                        NotSupported,
                    }
                    [DataMember(Name = "shutter")] 
                    public ShutterEnum? Shutter { get; private set; }
                    public enum PositionStatusEnum
                    {
                        Empty,
                        NotEmpty,
                        Unknown,
                        NotSupported,
                    }
                    [DataMember(Name = "positionStatus")] 
                    public PositionStatusEnum? PositionStatus { get; private set; }
                    public enum TransportEnum
                    {
                        Ok,
                        Inoperative,
                        Unknown,
                        NotSupported,
                    }
                    [DataMember(Name = "transport")] 
                    public TransportEnum? Transport { get; private set; }
                    public enum TransportStatusEnum
                    {
                        Empty,
                        NotEmpty,
                        NotEmptyCustomer,
                        NotEmptyUnkown,
                        NotSupported,
                    }
                    [DataMember(Name = "transportStatus")] 
                    public TransportStatusEnum? TransportStatus { get; private set; }
                    public enum JammedShutterPositionEnum
                    {
                        NotSupported,
                        NotJammed,
                        Open,
                        PartiallyOpen,
                        Closed,
                        Unknown,
                    }
                    [DataMember(Name = "jammedShutterPosition")] 
                    public JammedShutterPositionEnum? JammedShutterPosition { get; private set; }

                    public PositionsClass (PositionEnum? Position, ShutterEnum? Shutter, PositionStatusEnum? PositionStatus, TransportEnum? Transport, TransportStatusEnum? TransportStatus, JammedShutterPositionEnum? JammedShutterPosition)
                    {
                        this.Position = Position;
                        this.Shutter = Shutter;
                        this.PositionStatus = PositionStatus;
                        this.Transport = Transport;
                        this.TransportStatus = TransportStatus;
                        this.JammedShutterPosition = JammedShutterPosition;
                    }
                }
                [DataMember(Name = "positions")] 
                public PositionsClass Positions { get; private set; }

                public CashDispenserClass (IntermediateStackerEnum? IntermediateStacker, PositionsClass Positions)
                {
                    this.IntermediateStacker = IntermediateStacker;
                    this.Positions = Positions;
                }


            }

            /// <summary>
            ///Status information for XFS4IoT services implementing the CashManagement interface. This will be omitted if the CashManagement interface is not supported.
            /// </summary>
            public class CashManagementClass
            {
                public enum SafeDoorEnum
                {
                    DoorNotSupported,
                    DoorOpen,
                    DoorClosed,
                    DoorUnknown,
                }
                [DataMember(Name = "safeDoor")] 
                public SafeDoorEnum? SafeDoor { get; private set; }
                public enum DispenserEnum
                {
                    Ok,
                    CashUnitState,
                    CashUnitStop,
                    CashUnitUnknown,
                }
                [DataMember(Name = "dispenser")] 
                public DispenserEnum? Dispenser { get; private set; }
                public enum AcceptorEnum
                {
                    Ok,
                    CashUnitState,
                    CashUnitStop,
                    CashUnitUnknown,
                }
                [DataMember(Name = "acceptor")] 
                public AcceptorEnum? Acceptor { get; private set; }

                public CashManagementClass (SafeDoorEnum? SafeDoor, DispenserEnum? Dispenser, AcceptorEnum? Acceptor)
                {
                    this.SafeDoor = SafeDoor;
                    this.Dispenser = Dispenser;
                    this.Acceptor = Acceptor;
                }


            }

            /// <summary>
            ///Status information for XFS4IoT services implementing the KeyManagement interface. This will be omitted if the KeyManagement interface is not supported.
            /// </summary>
            public class KeyManagementClass
            {
                public enum EncryptionStateEnum
                {
                    Ready,
                    NotReady,
                    NotInitialized,
                    Busy,
                    Undefined,
                    Initialized,
                }
                [DataMember(Name = "encryptionState")] 
                public EncryptionStateEnum? EncryptionState { get; private set; }
                public enum CertificateStateEnum
                {
                    Unknown,
                    Primary,
                    Secondary,
                    NotReady,
                }
                [DataMember(Name = "certificateState")] 
                public CertificateStateEnum? CertificateState { get; private set; }

                public KeyManagementClass (EncryptionStateEnum? EncryptionState, CertificateStateEnum? CertificateState)
                {
                    this.EncryptionState = EncryptionState;
                    this.CertificateState = CertificateState;
                }


            }

            /// <summary>
            ///Status information for XFS4IoT services implementing the Keyboard interface. This will be omitted if the Keyboard interface is not supported.
            /// </summary>
            public class KeyboardClass
            {
                public enum AutoBeepModeEnum
                {
                    Active,
                    InActive,
                }
                [DataMember(Name = "autoBeepMode")] 
                public AutoBeepModeEnum? AutoBeepMode { get; private set; }

                public KeyboardClass (AutoBeepModeEnum? AutoBeepMode)
                {
                    this.AutoBeepMode = AutoBeepMode;
                }


            }

            /// <summary>
            ///Status information for XFS4IoT services implementing the TextTerminal interface. This will be omitted if the TextTerminal interface is not supported.
            /// </summary>
            public class TextTerminalClass
            {
                public enum KeyboardEnum
                {
                    On,
                    Off,
                    Na,
                }
                [DataMember(Name = "keyboard")] 
                public KeyboardEnum? Keyboard { get; private set; }
                public enum KeyLockEnum
                {
                    On,
                    Off,
                    Na,
                }
                [DataMember(Name = "keyLock")] 
                public KeyLockEnum? KeyLock { get; private set; }
                [DataMember(Name = "displaySizeX")] 
                public int? DisplaySizeX { get; private set; }
                [DataMember(Name = "displaySizeY")] 
                public int? DisplaySizeY { get; private set; }
                
                /// <summary>
                ///Specifies array that specifies the state of each LED. Specifies the state of the na, off or a combination of the following flags consisting of one type B, and optionally one type C
                /// </summary>
                public class LedsClass 
                {
                    [DataMember(Name = "na")] 
                    public bool? Na { get; private set; }
                    [DataMember(Name = "off")] 
                    public bool? Off { get; private set; }
                    [DataMember(Name = "slowFlash")] 
                    public bool? SlowFlash { get; private set; }
                    [DataMember(Name = "mediumFlash")] 
                    public bool? MediumFlash { get; private set; }
                    [DataMember(Name = "quickFlash")] 
                    public bool? QuickFlash { get; private set; }
                    [DataMember(Name = "continuous")] 
                    public bool? Continuous { get; private set; }
                    [DataMember(Name = "red")] 
                    public bool? Red { get; private set; }
                    [DataMember(Name = "green")] 
                    public bool? Green { get; private set; }
                    [DataMember(Name = "yellow")] 
                    public bool? Yellow { get; private set; }
                    [DataMember(Name = "blue")] 
                    public bool? Blue { get; private set; }
                    [DataMember(Name = "cyan")] 
                    public bool? Cyan { get; private set; }
                    [DataMember(Name = "magenta")] 
                    public bool? Magenta { get; private set; }
                    [DataMember(Name = "white")] 
                    public bool? White { get; private set; }

                    public LedsClass (bool? Na, bool? Off, bool? SlowFlash, bool? MediumFlash, bool? QuickFlash, bool? Continuous, bool? Red, bool? Green, bool? Yellow, bool? Blue, bool? Cyan, bool? Magenta, bool? White)
                    {
                        this.Na = Na;
                        this.Off = Off;
                        this.SlowFlash = SlowFlash;
                        this.MediumFlash = MediumFlash;
                        this.QuickFlash = QuickFlash;
                        this.Continuous = Continuous;
                        this.Red = Red;
                        this.Green = Green;
                        this.Yellow = Yellow;
                        this.Blue = Blue;
                        this.Cyan = Cyan;
                        this.Magenta = Magenta;
                        this.White = White;
                    }
                }
                [DataMember(Name = "leds")] 
                public LedsClass Leds { get; private set; }

                public TextTerminalClass (KeyboardEnum? Keyboard, KeyLockEnum? KeyLock, int? DisplaySizeX, int? DisplaySizeY, LedsClass Leds)
                {
                    this.Keyboard = Keyboard;
                    this.KeyLock = KeyLock;
                    this.DisplaySizeX = DisplaySizeX;
                    this.DisplaySizeY = DisplaySizeY;
                    this.Leds = Leds;
                }


            }

            /// <summary>
            ///Status information for XFS4IoT services implementing the Printer interface. This will be omitted if the Printer interface is not supported.
            /// </summary>
            public class PrinterClass
            {
                public enum MediaEnum
                {
                    NotSupported,
                    Unknown,
                    Present,
                    NotPresent,
                    Jammed,
                    Entering,
                    Retracted,
                }
                [DataMember(Name = "media")] 
                public MediaEnum? Media { get; private set; }
                
                /// <summary>
                ///Specifies the state of paper supplies as one of the following values:**notSupported**
                ////  Capability not supported by the device.**unknown**
                ////  Status cannot be determined with device in its current state.**full**
                ////  The paper supply is full.**low**
                ////  The paper supply is low.**out**
                ////  The paper supply is empty.**jammed**
                ////  The paper supply is jammed.
                /// </summary>
                public class PaperClass 
                {
                    public enum UpperEnum
                    {
                        NotSupported,
                        Unknown,
                        Full,
                        Low,
                        Out,
                        Jammed,
                    }
                    [DataMember(Name = "upper")] 
                    public UpperEnum? Upper { get; private set; }
                    public enum LowerEnum
                    {
                        NotSupported,
                        Unknown,
                        Full,
                        Low,
                        Out,
                        Jammed,
                    }
                    [DataMember(Name = "lower")] 
                    public LowerEnum? Lower { get; private set; }
                    public enum ExternalEnum
                    {
                        NotSupported,
                        Unknown,
                        Full,
                        Low,
                        Out,
                        Jammed,
                    }
                    [DataMember(Name = "external")] 
                    public ExternalEnum? External { get; private set; }
                    public enum AuxEnum
                    {
                        NotSupported,
                        Unknown,
                        Full,
                        Low,
                        Out,
                        Jammed,
                    }
                    [DataMember(Name = "aux")] 
                    public AuxEnum? Aux { get; private set; }
                    public enum Aux2Enum
                    {
                        NotSupported,
                        Unknown,
                        Full,
                        Low,
                        Out,
                        Jammed,
                    }
                    [DataMember(Name = "aux2")] 
                    public Aux2Enum? Aux2 { get; private set; }
                    public enum ParkEnum
                    {
                        NotSupported,
                        Unknown,
                        Full,
                        Low,
                        Out,
                        Jammed,
                    }
                    [DataMember(Name = "park")] 
                    public ParkEnum? Park { get; private set; }

                    public PaperClass (UpperEnum? Upper, LowerEnum? Lower, ExternalEnum? External, AuxEnum? Aux, Aux2Enum? Aux2, ParkEnum? Park)
                    {
                        this.Upper = Upper;
                        this.Lower = Lower;
                        this.External = External;
                        this.Aux = Aux;
                        this.Aux2 = Aux2;
                        this.Park = Park;
                    }
                }
                [DataMember(Name = "paper")] 
                public PaperClass Paper { get; private set; }
                public enum TonerEnum
                {
                    NotSupported,
                    Unknown,
                    Full,
                    Low,
                    Out,
                }
                [DataMember(Name = "toner")] 
                public TonerEnum? Toner { get; private set; }
                public enum InkEnum
                {
                    NotSupported,
                    Unknown,
                    Full,
                    Low,
                    Out,
                }
                [DataMember(Name = "ink")] 
                public InkEnum? Ink { get; private set; }
                public enum LampEnum
                {
                    NotSupported,
                    Unknown,
                    Ok,
                    Fading,
                    Inop,
                }
                [DataMember(Name = "lamp")] 
                public LampEnum? Lamp { get; private set; }
                
                /// <summary>
                ///An array of bin state objects. If no retain bins are supported, the array will be empty.
                /// </summary>
                public class RetractBinsClass 
                {
                    public enum StateEnum
                    {
                        Unknown,
                        Ok,
                        Full,
                        High,
                        Missing,
                    }
                    [DataMember(Name = "state")] 
                    public StateEnum? State { get; private set; }
                    [DataMember(Name = "count")] 
                    public int? Count { get; private set; }

                    public RetractBinsClass (StateEnum? State, int? Count)
                    {
                        this.State = State;
                        this.Count = Count;
                    }
                }
                [DataMember(Name = "retractBins")] 
                public RetractBinsClass RetractBins { get; private set; }
                [DataMember(Name = "mediaOnStacker")] 
                public int? MediaOnStacker { get; private set; }
                
                /// <summary>
                ///Specifies the type of paper loaded as one of the following:**unknown**
                ////  No paper is loaded, reporting of this paper type is not supported or the paper type cannot be determined.**single**
                ////  The paper can be printed on only one side.**dual**
                ////  The paper can be printed on both sides.
                /// </summary>
                public class PaperTypeClass 
                {
                    public enum UpperEnum
                    {
                        Unknown,
                        Single,
                        Dual,
                    }
                    [DataMember(Name = "upper")] 
                    public UpperEnum? Upper { get; private set; }
                    public enum LowerEnum
                    {
                        Unknown,
                        Single,
                        Dual,
                    }
                    [DataMember(Name = "lower")] 
                    public LowerEnum? Lower { get; private set; }
                    public enum ExternalEnum
                    {
                        Unknown,
                        Single,
                        Dual,
                    }
                    [DataMember(Name = "external")] 
                    public ExternalEnum? External { get; private set; }
                    public enum AuxEnum
                    {
                        Unknown,
                        Single,
                        Dual,
                    }
                    [DataMember(Name = "aux")] 
                    public AuxEnum? Aux { get; private set; }
                    public enum Aux2Enum
                    {
                        Unknown,
                        Single,
                        Dual,
                    }
                    [DataMember(Name = "aux2")] 
                    public Aux2Enum? Aux2 { get; private set; }
                    public enum ParkEnum
                    {
                        Unknown,
                        Single,
                        Dual,
                    }
                    [DataMember(Name = "park")] 
                    public ParkEnum? Park { get; private set; }

                    public PaperTypeClass (UpperEnum? Upper, LowerEnum? Lower, ExternalEnum? External, AuxEnum? Aux, Aux2Enum? Aux2, ParkEnum? Park)
                    {
                        this.Upper = Upper;
                        this.Lower = Lower;
                        this.External = External;
                        this.Aux = Aux;
                        this.Aux2 = Aux2;
                        this.Park = Park;
                    }
                }
                [DataMember(Name = "paperType")] 
                public PaperTypeClass PaperType { get; private set; }
                public enum BlackMarkModeEnum
                {
                    NotSupported,
                    Unknown,
                    On,
                    Off,
                }
                [DataMember(Name = "blackMarkMode")] 
                public BlackMarkModeEnum? BlackMarkMode { get; private set; }

                public PrinterClass (MediaEnum? Media, PaperClass Paper, TonerEnum? Toner, InkEnum? Ink, LampEnum? Lamp, RetractBinsClass RetractBins, int? MediaOnStacker, PaperTypeClass PaperType, BlackMarkModeEnum? BlackMarkMode)
                {
                    this.Media = Media;
                    this.Paper = Paper;
                    this.Toner = Toner;
                    this.Ink = Ink;
                    this.Lamp = Lamp;
                    this.RetractBins = RetractBins;
                    this.MediaOnStacker = MediaOnStacker;
                    this.PaperType = PaperType;
                    this.BlackMarkMode = BlackMarkMode;
                }


            }

            /// <summary>
            ///Status information for XFS4IoT services implementing the CardEmbosser interface. This will be omitted if the CardEmbosser interface is not supported.
            /// </summary>
            public class CardEmbosserClass
            {
                public enum MediaEnum
                {
                    Present,
                    NotPresent,
                    Jammed,
                    NotSupported,
                    Unknown,
                    Entering,
                    Topper,
                    InHopper,
                    OutHopper,
                    Msre,
                    Retained,
                }
                [DataMember(Name = "media")] 
                public MediaEnum? Media { get; private set; }
                public enum RetainBinEnum
                {
                    Ok,
                    Full,
                    High,
                    NotSupported,
                }
                [DataMember(Name = "retainBin")] 
                public RetainBinEnum? RetainBin { get; private set; }
                public enum OutputBinEnum
                {
                    Ok,
                    Full,
                    High,
                    NotSupported,
                }
                [DataMember(Name = "outputBin")] 
                public OutputBinEnum? OutputBin { get; private set; }
                public enum InputBinEnum
                {
                    Ok,
                    Empty,
                    Low,
                    NotSupported,
                }
                [DataMember(Name = "inputBin")] 
                public InputBinEnum? InputBin { get; private set; }
                [DataMember(Name = "totalCards")] 
                public int? TotalCards { get; private set; }
                [DataMember(Name = "outputCards")] 
                public int? OutputCards { get; private set; }
                [DataMember(Name = "retainCards")] 
                public int? RetainCards { get; private set; }
                public enum TonerEnum
                {
                    Full,
                    Low,
                    Out,
                    NotSupported,
                    Unknown,
                }
                [DataMember(Name = "toner")] 
                public TonerEnum? Toner { get; private set; }

                public CardEmbosserClass (MediaEnum? Media, RetainBinEnum? RetainBin, OutputBinEnum? OutputBin, InputBinEnum? InputBin, int? TotalCards, int? OutputCards, int? RetainCards, TonerEnum? Toner)
                {
                    this.Media = Media;
                    this.RetainBin = RetainBin;
                    this.OutputBin = OutputBin;
                    this.InputBin = InputBin;
                    this.TotalCards = TotalCards;
                    this.OutputCards = OutputCards;
                    this.RetainCards = RetainCards;
                    this.Toner = Toner;
                }


            }

            /// <summary>
            ///Status information for XFS4IoT services implementing the BarcodeReader interface. This will be omitted if the BarcodeReader interface is not supported.
            /// </summary>
            public class BarcodeReaderClass
            {
                public enum ScannerEnum
                {
                    On,
                    Off,
                    Inoperative,
                    Unknown,
                }
                [DataMember(Name = "scanner")] 
                public ScannerEnum? Scanner { get; private set; }

                public BarcodeReaderClass (ScannerEnum? Scanner)
                {
                    this.Scanner = Scanner;
                }


            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, CommonClass Common = null, CardReaderClass CardReader = null, CashAcceptorClass CashAcceptor = null, CashDispenserClass CashDispenser = null, CashManagementClass CashManagement = null, KeyManagementClass KeyManagement = null, KeyboardClass Keyboard = null, TextTerminalClass TextTerminal = null, PrinterClass Printer = null, CardEmbosserClass CardEmbosser = null, BarcodeReaderClass BarcodeReader = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(StatusCompletion.PayloadData)}");

                this.Common = Common;
                this.CardReader = CardReader;
                this.CashAcceptor = CashAcceptor;
                this.CashDispenser = CashDispenser;
                this.CashManagement = CashManagement;
                this.KeyManagement = KeyManagement;
                this.Keyboard = Keyboard;
                this.TextTerminal = TextTerminal;
                this.Printer = Printer;
                this.CardEmbosser = CardEmbosser;
                this.BarcodeReader = BarcodeReader;
            }

            /// <summary>
            ///Status information common to all XFS4IoT services.
            /// </summary>
            [DataMember(Name = "common")] 
            public CommonClass Common { get; private set; }
            /// <summary>
            ///Status information for XFS4IoT services implementing the CardReader interface. This will be omitted if the CardReader interface is not supported.
            /// </summary>
            [DataMember(Name = "cardReader")] 
            public CardReaderClass CardReader { get; private set; }
            /// <summary>
            ///Status information for XFS4IoT services implementing the CashAcceptor interface. This will be omitted if the CashAcceptor interface is not supported.
            /// </summary>
            [DataMember(Name = "cashAcceptor")] 
            public CashAcceptorClass CashAcceptor { get; private set; }
            /// <summary>
            ///Status information for XFS4IoT services implementing the CashDispenser interface. This will be omitted if the CashDispenser interface is not supported.
            /// </summary>
            [DataMember(Name = "cashDispenser")] 
            public CashDispenserClass CashDispenser { get; private set; }
            /// <summary>
            ///Status information for XFS4IoT services implementing the CashManagement interface. This will be omitted if the CashManagement interface is not supported.
            /// </summary>
            [DataMember(Name = "cashManagement")] 
            public CashManagementClass CashManagement { get; private set; }
            /// <summary>
            ///Status information for XFS4IoT services implementing the KeyManagement interface. This will be omitted if the KeyManagement interface is not supported.
            /// </summary>
            [DataMember(Name = "keyManagement")] 
            public KeyManagementClass KeyManagement { get; private set; }
            /// <summary>
            ///Status information for XFS4IoT services implementing the Keyboard interface. This will be omitted if the Keyboard interface is not supported.
            /// </summary>
            [DataMember(Name = "keyboard")] 
            public KeyboardClass Keyboard { get; private set; }
            /// <summary>
            ///Status information for XFS4IoT services implementing the TextTerminal interface. This will be omitted if the TextTerminal interface is not supported.
            /// </summary>
            [DataMember(Name = "textTerminal")] 
            public TextTerminalClass TextTerminal { get; private set; }
            /// <summary>
            ///Status information for XFS4IoT services implementing the Printer interface. This will be omitted if the Printer interface is not supported.
            /// </summary>
            [DataMember(Name = "printer")] 
            public PrinterClass Printer { get; private set; }
            /// <summary>
            ///Status information for XFS4IoT services implementing the CardEmbosser interface. This will be omitted if the CardEmbosser interface is not supported.
            /// </summary>
            [DataMember(Name = "cardEmbosser")] 
            public CardEmbosserClass CardEmbosser { get; private set; }
            /// <summary>
            ///Status information for XFS4IoT services implementing the BarcodeReader interface. This will be omitted if the BarcodeReader interface is not supported.
            /// </summary>
            [DataMember(Name = "barcodeReader")] 
            public BarcodeReaderClass BarcodeReader { get; private set; }

        }
    }
}
