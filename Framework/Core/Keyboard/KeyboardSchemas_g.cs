/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * KeyboardSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.Keyboard
{

    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(AutoBeepModeEnum? AutoBeepMode = null)
        {
            this.AutoBeepMode = AutoBeepMode;
        }

        public enum AutoBeepModeEnum
        {
            Active,
            InActive
        }

        /// <summary>
        /// Specifies whether automatic beep tone on key press is active or not. Active and in-active key beeping is reported 
        /// independently. autoBeepMode can take a combination of the following values, if the flag is not set auto beeping 
        /// is not activated (or not supported) for that key type (i.e. active or in-active keys).
        /// The following values are possible:
        /// * ```active``` - An automatic tone will be generated for all active keys.
        /// * ```inActive``` - An automatic tone will be generated for all in-active keys.
        /// </summary>
        [DataMember(Name = "autoBeepMode")]
        public AutoBeepModeEnum? AutoBeepMode { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(AutoBeepClass AutoBeep = null, List<EtsCapsClass> EtsCaps = null)
        {
            this.AutoBeep = AutoBeep;
            this.EtsCaps = EtsCaps;
        }

        [DataContract]
        public sealed class AutoBeepClass
        {
            public AutoBeepClass(bool? ActiveAvailable = null, bool? ActiveSelectable = null, bool? InactiveAvailable = null, bool? InactiveSelectable = null)
            {
                this.ActiveAvailable = ActiveAvailable;
                this.ActiveSelectable = ActiveSelectable;
                this.InactiveAvailable = InactiveAvailable;
                this.InactiveSelectable = InactiveSelectable;
            }

            /// <summary>
            /// Automatic beep tone on active key key-press is supported.
            /// If this flag is not set then automatic beeping for active keys is not supported.
            /// </summary>
            [DataMember(Name = "activeAvailable")]
            public bool? ActiveAvailable { get; init; }

            /// <summary>
            /// Automatic beeping for active keys can be controlled turned on and off by the application.
            /// If this flag is not set then automatic beeping for active keys cannot be controlled by an application.
            /// </summary>
            [DataMember(Name = "activeSelectable")]
            public bool? ActiveSelectable { get; init; }

            /// <summary>
            /// Automatic beep tone on in-active key keypress is supported.
            /// If this flag is not set then automatic beeping for in-active keys is not supported.
            /// </summary>
            [DataMember(Name = "inactiveAvailable")]
            public bool? InactiveAvailable { get; init; }

            /// <summary>
            /// Automatic beeping for in-active keys can be controlled turned on and off by the application.
            /// If this flag is not set then automatic beeping for in-active keys cannot be controlled by an application.
            /// </summary>
            [DataMember(Name = "inactiveSelectable")]
            public bool? InactiveSelectable { get; init; }

        }

        /// <summary>
        /// Specifies whether the device will emit a key beep tone on key presses of active keys or inactive keys, and if so, which mode it supports
        /// </summary>
        [DataMember(Name = "autoBeep")]
        public AutoBeepClass AutoBeep { get; init; }

        [DataContract]
        public sealed class EtsCapsClass
        {
            public EtsCapsClass(int? XPos = null, int? YPos = null, int? XSize = null, int? YSize = null, int? MaximumTouchFrames = null, int? MaximumTouchKeys = null, FloatFlagsClass FloatFlags = null)
            {
                this.XPos = XPos;
                this.YPos = YPos;
                this.XSize = XSize;
                this.YSize = YSize;
                this.MaximumTouchFrames = MaximumTouchFrames;
                this.MaximumTouchKeys = MaximumTouchKeys;
                this.FloatFlags = FloatFlags;
            }

            /// <summary>
            /// Specifies the position of the left edge of the ets in Windows virtual screen coordinates.
            /// This value may be negative because the of the monitor position on the virtual desktop.
            /// </summary>
            [DataMember(Name = "xPos")]
            public int? XPos { get; init; }

            /// <summary>
            /// Specifies the position of the right edge of the ets in Windows virtual screen coordinates.
            /// This value may be negative because the of the monitor position on the virtual desktop.
            /// </summary>
            [DataMember(Name = "yPos")]
            public int? YPos { get; init; }

            /// <summary>
            /// Specifies the width of the ets in Windows virtual screen coordinates.
            /// </summary>
            [DataMember(Name = "xSize")]
            public int? XSize { get; init; }

            /// <summary>
            /// Specifies the height of the ets in Windows virtual screen coordinates.
            /// </summary>
            [DataMember(Name = "ySize")]
            public int? YSize { get; init; }

            /// <summary>
            /// Specifies the maximum number of Touch-Frames that the device can support in a touch keyboard definition.
            /// </summary>
            [DataMember(Name = "maximumTouchFrames")]
            public int? MaximumTouchFrames { get; init; }

            /// <summary>
            /// Specifies the maximum number of Touch-Keys that the device can support within any a touchframe.
            /// </summary>
            [DataMember(Name = "maximumTouchKeys")]
            public int? MaximumTouchKeys { get; init; }

            [DataContract]
            public sealed class FloatFlagsClass
            {
                public FloatFlagsClass(bool? X = null, bool? Y = null)
                {
                    this.X = X;
                    this.Y = Y;
                }

                /// <summary>
                /// Specifies that the device will randomly shift the layout in a horizontal direction
                /// </summary>
                [DataMember(Name = "x")]
                public bool? X { get; init; }

                /// <summary>
                /// Specifies that the device will randomly shift the layout in a vertical direction. 
                /// </summary>
                [DataMember(Name = "y")]
                public bool? Y { get; init; }

            }

            /// <summary>
            /// Specifies if the device can float the touch keyboards. FloatNone if the device cannot randomly shift the layout.
            /// </summary>
            [DataMember(Name = "floatFlags")]
            public FloatFlagsClass FloatFlags { get; init; }

        }

        /// <summary>
        /// Specifies the capabilities of the ets device.
        /// </summary>
        [DataMember(Name = "etsCaps")]
        public List<EtsCapsClass> EtsCaps { get; init; }

    }


    [DataContract]
    public sealed class LayoutClass
    {
        public LayoutClass(EntryModeEnum? EntryMode = null, List<FramesClass> Frames = null)
        {
            this.EntryMode = EntryMode;
            this.Frames = Frames;
        }

        public enum EntryModeEnum
        {
            Data,
            Pin,
            Secure
        }

        /// <summary>
        /// Specifies entry mode to which the layout applies. The following values are possible:
        /// * ```data``` - Specifies that the layout be applied to the DataEntry method.
        /// * ```pin``` - Specifies that the layout be applied to the PinEntry method.
        /// * ```secure``` - Specifies that the layout be applied to the SecureKeyEntry method.
        /// </summary>
        [DataMember(Name = "entryMode")]
        public EntryModeEnum? EntryMode { get; init; }

        [DataContract]
        public sealed class FramesClass
        {
            public FramesClass(int? XPos = null, int? YPos = null, int? XSize = null, int? YSize = null, FloatActionClass FloatAction = null, List<FksClass> Fks = null)
            {
                this.XPos = XPos;
                this.YPos = YPos;
                this.XSize = XSize;
                this.YSize = YSize;
                this.FloatAction = FloatAction;
                this.Fks = Fks;
            }

            /// <summary>
            /// For [ETS](#keyboard.generalinformation.ets), specifies the left coordinate of the frame as an offset from the left edge of the screen. 
            /// For all other device types, this value is ignored
            /// </summary>
            [DataMember(Name = "xPos")]
            public int? XPos { get; init; }

            /// <summary>
            /// For [ETS](#keyboard.generalinformation.ets), specifies the top coordinate of the frame as an offset from the top edge of the screen. 
            /// For all other device types, this value is ignored
            /// </summary>
            [DataMember(Name = "yPos")]
            public int? YPos { get; init; }

            /// <summary>
            /// For [ETS](#keyboard.generalinformation.ets), specifies the width of the frame. For all other device types, this value is ignored
            /// </summary>
            [DataMember(Name = "xSize")]
            public int? XSize { get; init; }

            /// <summary>
            /// For [ETS](#keyboard.generalinformation.ets), specifies the height of the frame. For all other device types, this value is ignored
            /// </summary>
            [DataMember(Name = "ySize")]
            public int? YSize { get; init; }

            [DataContract]
            public sealed class FloatActionClass
            {
                public FloatActionClass(bool? FloatX = null, bool? FloatY = null)
                {
                    this.FloatX = FloatX;
                    this.FloatY = FloatY;
                }

                /// <summary>
                /// Specifies that the device will randomly shift the layout in a horizontal direction
                /// </summary>
                [DataMember(Name = "floatX")]
                public bool? FloatX { get; init; }

                /// <summary>
                /// Specifies that the device will randomly shift the layout in a vertical direction
                /// </summary>
                [DataMember(Name = "floatY")]
                public bool? FloatY { get; init; }

            }

            /// <summary>
            /// Specifies if the device can float the touch keyboards
            /// </summary>
            [DataMember(Name = "floatAction")]
            public FloatActionClass FloatAction { get; init; }

            [DataContract]
            public sealed class FksClass
            {
                public FksClass(int? XPos = null, int? YPos = null, int? XSize = null, int? YSize = null, KeyTypeEnum? KeyType = null, string Fk = null, string ShiftFK = null)
                {
                    this.XPos = XPos;
                    this.YPos = YPos;
                    this.XSize = XSize;
                    this.YSize = YSize;
                    this.KeyType = KeyType;
                    this.Fk = Fk;
                    this.ShiftFK = ShiftFK;
                }

                /// <summary>
                /// Specifies the position of the top left corner of the FK relative to the left hand side of the layout.
                /// For [ETS](#keyboard.generalinformation.ets) devices, must be in the range defined in the frame. 
                /// For non-ETS devices, must be a value between 0 and 999, where 0 is the left edge and 999 is the right edge.
                /// </summary>
                [DataMember(Name = "xPos")]
                [DataTypes(Minimum = 0, Maximum = 999)]
                public int? XPos { get; init; }

                /// <summary>
                /// Specifies the position of the top left corner of the Function Key (FK) relative to the left hand side of the layout.
                /// For [ETS](#keyboard.generalinformation.ets) devices, must be in the range defined in the frame. 
                /// For non-ETS devices, must be a value between 0 and 999, where 0 is the top edge and 999 is the bottom edge.
                /// </summary>
                [DataMember(Name = "yPos")]
                [DataTypes(Minimum = 0, Maximum = 999)]
                public int? YPos { get; init; }

                /// <summary>
                /// Specifies the Function Key (FK) width. 
                /// For [ETS](#keyboard.generalinformation.ets), width is measured in pixels. For non-ETS devices, width is expressed as a value between 
                /// 1 and 1000, where 1 is the smallest possible size and 1000 is the full width of the layout.
                /// </summary>
                [DataMember(Name = "xSize")]
                [DataTypes(Minimum = 1, Maximum = 1000)]
                public int? XSize { get; init; }

                /// <summary>
                /// Specifies the Function Key (FK) height.
                /// For [ETS](#keyboard.generalinformation.ets), height is measured in pixels. 
                /// For non-ETS devices, height is expressed as a value between 1 and 1000, where 1 is the smallest 
                /// possible size and 1000 is the full height of the layout.
                /// </summary>
                [DataMember(Name = "ySize")]
                [DataTypes(Minimum = 1, Maximum = 1000)]
                public int? YSize { get; init; }

                public enum KeyTypeEnum
                {
                    Fk,
                    Fdk
                }

                /// <summary>
                /// Defines the type of XFS key definition value is represented by *fk* and *shiftFK*.
                /// If the key is physically present on the device but it is not used, this property can be omitted.
                /// The following values are possible:
                /// * ```fk``` - Function Keys are being used.
                /// * ```fdk``` - Function Descriptor Keys are being used.
                /// </summary>
                [DataMember(Name = "keyType")]
                public KeyTypeEnum? KeyType { get; init; }

                /// <summary>
                /// Specifies the Function Key associated with the physical area in non-shifted mode.
                /// This property is not required if the *keyType* is omitted.
                /// </summary>
                [DataMember(Name = "fk")]
                [DataTypes(Pattern = "^(one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|res0[1-8]|oem0[1-6]|doubleZero|tripleZero)$|^fdk(0[1-9]|[12][0-9]|3[0-2])$")]
                public string Fk { get; init; }

                /// <summary>
                /// Specifies the Function Key associated with the physical key in shifted mode.
                /// This property is not required if the *keyType* is omitted.
                /// </summary>
                [DataMember(Name = "shiftFK")]
                [DataTypes(Pattern = "^(one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|res0[1-8]|oem0[1-6]|doubleZero|tripleZero)$|^fdk(0[1-9]|[12][0-9]|3[0-2])$")]
                public string ShiftFK { get; init; }

            }

            /// <summary>
            /// Defining details of the keys in the keyboard.
            /// </summary>
            [DataMember(Name = "fks")]
            public List<FksClass> Fks { get; init; }

        }

        /// <summary>
        /// There can be one or more frames included. A Physical Frame can only contain Physical Keys. 
        /// It can contain Physical Keys positioned on the edge of the screen (for example, FDKs) or Physical Keys not positioned on the edge of the screen (for example EPP) but cannot contain both. 
        /// A [ETS](#keyboard.generalinformation.ets) can only contain Touch Keys. To determine the frame type, frameXSize and frameYSize should be checked. 
        /// Refer to the [layout table](#keyboard.generalinformation.layout) for the different types of frames, and see the diagram for an example.
        /// </summary>
        [DataMember(Name = "frames")]
        public List<FramesClass> Frames { get; init; }

    }


    public enum EntryCompletionEnum
    {
        Auto,
        Enter,
        Cancel,
        Continue,
        Clear,
        Backspace,
        Fdk,
        Help,
        Fk,
        ContFdk
    }


}
