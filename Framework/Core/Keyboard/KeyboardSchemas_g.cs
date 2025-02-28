/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
        public StatusClass(AutoBeepModeClass AutoBeepMode = null)
        {
            this.AutoBeepMode = AutoBeepMode;
        }

        [DataContract]
        public sealed class AutoBeepModeClass
        {
            public AutoBeepModeClass(bool? ActiveAvailable = null, bool? InactiveAvailable = null)
            {
                this.ActiveAvailable = ActiveAvailable;
                this.InactiveAvailable = InactiveAvailable;
            }

            /// <summary>
            /// Specifies whether an automatic tone will be generated for all active keys. This may be null in
            /// [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
            /// </summary>
            [DataMember(Name = "activeAvailable")]
            public bool? ActiveAvailable { get; init; }

            /// <summary>
            /// Specifies whether an automatic tone will be generated for all inactive keys. This may be null in
            /// [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
            /// </summary>
            [DataMember(Name = "inactiveAvailable")]
            public bool? InactiveAvailable { get; init; }

        }

        /// <summary>
        /// Specifies whether automatic beep tone on key press is active or not. Active and inactive key beeping is
        /// reported independently.
        /// </summary>
        [DataMember(Name = "autoBeepMode")]
        public AutoBeepModeClass AutoBeepMode { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(AutoBeepClass AutoBeep = null, EtsCapsClass EtsCaps = null)
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
            /// Automatic beeping for active keys can be controlled (i.e. turned on and off) by the application.
            /// If this flag is not set then automatic beeping for active keys cannot be controlled by an application.
            /// </summary>
            [DataMember(Name = "activeSelectable")]
            public bool? ActiveSelectable { get; init; }

            /// <summary>
            /// Automatic beep tone on inactive key keypress is supported.
            /// If this flag is not set then automatic beeping for inactive keys is not supported.
            /// </summary>
            [DataMember(Name = "inactiveAvailable")]
            public bool? InactiveAvailable { get; init; }

            /// <summary>
            /// Automatic beeping for inactive keys can be controlled (i.e. turned on and off) by the application.
            /// If this flag is not set then automatic beeping for inactive keys cannot be controlled by an application.
            /// </summary>
            [DataMember(Name = "inactiveSelectable")]
            public bool? InactiveSelectable { get; init; }

        }

        /// <summary>
        /// Specifies whether the device will emit a key beep tone on key presses of active keys or inactive keys,
        /// and if so, which mode it supports. This property is null if not supported.
        /// </summary>
        [DataMember(Name = "autoBeep")]
        public AutoBeepClass AutoBeep { get; init; }

        [DataContract]
        public sealed class EtsCapsClass
        {
            public EtsCapsClass(int? XPos = null, int? YPos = null, int? XSize = null, int? YSize = null, int? MaximumTouchFrames = null, int? MaximumTouchKeys = null, FloatClass Float = null)
            {
                this.XPos = XPos;
                this.YPos = YPos;
                this.XSize = XSize;
                this.YSize = YSize;
                this.MaximumTouchFrames = MaximumTouchFrames;
                this.MaximumTouchKeys = MaximumTouchKeys;
                this.Float = Float;
            }

            /// <summary>
            /// Specifies the position of the left edge of the Encrypting Touch Screen in virtual screen coordinates.
            /// This value may be negative because the of the monitor position on the virtual desktop.
            /// </summary>
            [DataMember(Name = "xPos")]
            [DataTypes(Minimum = 0)]
            public int? XPos { get; init; }

            /// <summary>
            /// Specifies the position of the right edge of the Encrypting Touch Screen in virtual screen coordinates.
            /// This value may be negative because the of the monitor position on the virtual desktop.
            /// </summary>
            [DataMember(Name = "yPos")]
            [DataTypes(Minimum = 0)]
            public int? YPos { get; init; }

            /// <summary>
            /// Specifies the width of the Encrypting Touch Screen in virtual screen coordinates.
            /// </summary>
            [DataMember(Name = "xSize")]
            [DataTypes(Minimum = 0)]
            public int? XSize { get; init; }

            /// <summary>
            /// Specifies the height of the Encrypting Touch Screen in virtual screen coordinates.
            /// </summary>
            [DataMember(Name = "ySize")]
            [DataTypes(Minimum = 0)]
            public int? YSize { get; init; }

            /// <summary>
            /// Specifies the maximum number of Touch-Frames that the device can support in a touch keyboard
            /// definition.
            /// </summary>
            [DataMember(Name = "maximumTouchFrames")]
            [DataTypes(Minimum = 0)]
            public int? MaximumTouchFrames { get; init; }

            /// <summary>
            /// Specifies the maximum number of Touch-Keys that the device can support within a touch frame.
            /// </summary>
            [DataMember(Name = "maximumTouchKeys")]
            [DataTypes(Minimum = 0)]
            public int? MaximumTouchKeys { get; init; }

            [DataContract]
            public sealed class FloatClass
            {
                public FloatClass(bool? X = null, bool? Y = null)
                {
                    this.X = X;
                    this.Y = Y;
                }

                /// <summary>
                /// Specifies that the device will randomly shift the layout in a horizontal direction.
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
            /// Specifies if the device can float the touch keyboards. Both properties *x* and *y* are false
            /// if the device cannot randomly shift the layout. This property is null if not supported.
            /// </summary>
            [DataMember(Name = "float")]
            public FloatClass Float { get; init; }

        }

        /// <summary>
        /// Specifies the capabilities of the Encrypting Touch Screen device.
        /// This property is null if not supported.
        /// </summary>
        [DataMember(Name = "etsCaps")]
        public EtsCapsClass EtsCaps { get; init; }

    }


    [DataContract]
    public sealed class KeyClass
    {
        public KeyClass(int? XPos = null, int? YPos = null, int? XSize = null, int? YSize = null, string Key = null, string ShiftKey = null)
        {
            this.XPos = XPos;
            this.YPos = YPos;
            this.XSize = XSize;
            this.YSize = YSize;
            this.Key = Key;
            this.ShiftKey = ShiftKey;
        }

        /// <summary>
        /// Specifies the position of the left edge of the key relative to the left side of the frame.
        /// </summary>
        [DataMember(Name = "xPos")]
        [DataTypes(Minimum = 0)]
        public int? XPos { get; init; }

        /// <summary>
        /// Specifies the position of the top edge of the key relative to the top edge of the frame.
        /// </summary>
        [DataMember(Name = "yPos")]
        [DataTypes(Minimum = 0)]
        public int? YPos { get; init; }

        /// <summary>
        /// Specifies the Function Key (FK) width.
        /// </summary>
        [DataMember(Name = "xSize")]
        [DataTypes(Minimum = 1)]
        public int? XSize { get; init; }

        /// <summary>
        /// Specifies the Function Key (FK) height.
        /// </summary>
        [DataMember(Name = "ySize")]
        [DataTypes(Minimum = 1)]
        public int? YSize { get; init; }

        /// <summary>
        /// Specifies the Function Key associated with the area in non-shifted mode.
        /// 
        /// The following standard values are defined:
        /// 
        /// * ```zero``` - Numeric digit 0
        /// * ```one``` - Numeric digit 1
        /// * ```two``` - Numeric digit 2
        /// * ```three``` - Numeric digit 3
        /// * ```four``` - Numeric digit 4
        /// * ```five``` - Numeric digit 5
        /// * ```six``` - Numeric digit 6
        /// * ```seven``` - Numeric digit 7
        /// * ```eight``` - Numeric digit 8
        /// * ```nine``` - Numeric digit 9
        /// * ```[a-f]``` - Hex digit A to F for secure key entry
        /// * ```enter``` - Enter
        /// * ```cancel``` - Cancel
        /// * ```clear``` - Clear
        /// * ```backspace``` - Backspace
        /// * ```help``` - Help
        /// * ```decPoint``` - Decimal point
        /// * ```shift``` - Shift key used during hex entry
        /// * ```doubleZero``` - 00
        /// * ```tripleZero``` - 000
        /// * ```fdk[01-32]``` - 32 FDK keys
        /// 
        /// Additional non-standard values are also allowed:
        /// 
        /// * ```oem[a-zA-Z0-9]*``` - A non-standard value
        /// <example>shift</example>
        /// </summary>
        [DataMember(Name = "key")]
        [DataTypes(Pattern = @"^(zero|one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|doubleZero|tripleZero|fdk(0[1-9]|[12][0-9]|3[0-2])|oem[a-zA-Z0-9]*)$")]
        public string Key { get; init; }

        /// <summary>
        /// Specifies the Function Key associated with the key in shifted mode. 
        /// This property is null if not applicable.
        /// 
        /// See *key* for the valid property values.
        /// <example>a</example>
        /// </summary>
        [DataMember(Name = "shiftKey")]
        [DataTypes(Pattern = @"^(zero|one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|doubleZero|tripleZero|fdk(0[1-9]|[12][0-9]|3[0-2])|oem[a-zA-Z0-9]*)$")]
        public string ShiftKey { get; init; }

    }


    [DataContract]
    public sealed class LayoutFrameClass
    {
        public LayoutFrameClass(int? XPos = null, int? YPos = null, int? XSize = null, int? YSize = null, FloatClass Float = null, List<KeyClass> Keys = null)
        {
            this.XPos = XPos;
            this.YPos = YPos;
            this.XSize = XSize;
            this.YSize = YSize;
            this.Float = Float;
            this.Keys = Keys;
        }

        /// <summary>
        /// If the frame contains Touch Keys, specifies the left edge of the frame as an offset from the left edge of
        /// the screen in pixels and will be less than the width of the screen.
        /// 
        /// If the frame contains Physical Keys on the boundary of the screen, specifies the left coordinate of the
        /// frame as an offset from the left edge of the screen in pixels and will be 0 or the width of the screen in
        /// pixels.
        /// 
        /// If the frame contains Physical Keys not positioned on the screen boundary, this value is 0.
        /// </summary>
        [DataMember(Name = "xPos")]
        [DataTypes(Minimum = 0)]
        public int? XPos { get; init; }

        /// <summary>
        /// If the frame contains Touch Keys, specifies the top edge of the frame as an offset from the top edge of
        /// the screen in pixels and will be less than the height of the screen.
        /// 
        /// If the frame contains Physical Keys on the boundary of the screen, specifies the top edge of the frame as
        /// an offset from the top edge of the screen in pixels and will be 0 or the height of the screen in pixels.
        /// 
        /// If the frame contains Physical Keys not positioned on the screen boundary, this value is 0.
        /// </summary>
        [DataMember(Name = "yPos")]
        [DataTypes(Minimum = 0)]
        public int? YPos { get; init; }

        /// <summary>
        /// If the frame contains Touch Keys, specifies the width of the frame in pixels and will be greater than 0
        /// and less than the width of the screen minus the frame *xPos*.
        /// 
        /// If the frame contains Physical Keys on the boundary of the screen, specifies the width of the frame in
        /// pixels and will be 0 or the width of the screen in pixels.
        /// 
        /// If the frame contains Physical Keys not positioned on the screen boundary, this value is 0.
        /// </summary>
        [DataMember(Name = "xSize")]
        [DataTypes(Minimum = 0)]
        public int? XSize { get; init; }

        /// <summary>
        /// If the frame contains Touch Keys, specifies the height of the frame in pixels and will be greater than 0
        /// and less than the height of the screen minus the frame *yPos*.
        /// 
        /// If the frame contains Physical Keys on the boundary of the screen, specifies the height of the frame in
        /// pixels and will be 0 or the height of the screen in pixels.
        /// 
        /// If the frame contains Physical Keys not positioned on the screen boundary, this value is 0.
        /// </summary>
        [DataMember(Name = "ySize")]
        [DataTypes(Minimum = 0)]
        public int? YSize { get; init; }

        [DataContract]
        public sealed class FloatClass
        {
            public FloatClass(bool? X = null, bool? Y = null)
            {
                this.X = X;
                this.Y = Y;
            }

            /// <summary>
            /// Specifies that the device will randomly shift the layout in a horizontal direction.
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
        /// Specifies if the device can float the touch keyboards. 
        /// If [etsCaps](#common.capabilities.completion.properties.keyboard.etscaps) is null or 
        /// [float](#common.capabilities.completion.properties.keyboard.etscaps.float) is null, this property is null.
        /// If this property is null, the device cannot randomly shift the layout in both horizontal and vertical direction.
        /// </summary>
        [DataMember(Name = "float")]
        public FloatClass Float { get; init; }

        /// <summary>
        /// Specifies the keys in the keyboard.
        /// 
        /// The [layout](#keyboard.generalinformation.layout) section defines valid position and size values for
        /// physical and touch keys.
        /// </summary>
        [DataMember(Name = "keys")]
        public List<KeyClass> Keys { get; init; }

    }


    [DataContract]
    public sealed class LayoutNullableClass
    {
        public LayoutNullableClass(List<LayoutFrameClass> Data = null, List<LayoutFrameClass> Pin = null, List<LayoutFrameClass> Secure = null)
        {
            this.Data = Data;
            this.Pin = Pin;
            this.Secure = Secure;
        }

        /// <summary>
        /// The layout for the [Keyboard.DataEntry](#keyboard.dataentry) command.
        /// 
        /// There can be one or more frames included.
        /// 
        /// Refer to the [layout](#keyboard.generalinformation.layout) section for the different types of frames, and
        /// see the diagram for an example.
        /// </summary>
        [DataMember(Name = "data")]
        public List<LayoutFrameClass> Data { get; init; }

        /// <summary>
        /// The layout for the [Keyboard.PinEntry](#keyboard.pinentry) command.
        /// 
        /// There can be one or more frames included.
        /// 
        /// Refer to the [layout](#keyboard.generalinformation.layout) section for the different types of frames, and
        /// see the diagram for an example.
        /// </summary>
        [DataMember(Name = "pin")]
        public List<LayoutFrameClass> Pin { get; init; }

        /// <summary>
        /// The layout for the [Keyboard.SecureKeyEntry](#keyboard.securekeyentry) command.
        /// 
        /// There can be one or more frames included.
        /// 
        /// Refer to the [layout](#keyboard.generalinformation.layout) section for the different types of frames, and
        /// see the diagram for an example.
        /// </summary>
        [DataMember(Name = "secure")]
        public List<LayoutFrameClass> Secure { get; init; }

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


    [DataContract]
    public sealed class KeyPressedClass
    {
        public KeyPressedClass(EntryCompletionEnum? Completion = null, string Digit = null)
        {
            this.Completion = Completion;
            this.Digit = Digit;
        }

        [DataMember(Name = "completion")]
        public EntryCompletionEnum? Completion { get; init; }

        /// <summary>
        /// Specifies the digit entered by the user. When working in encryption mode or secure key entry mode
        /// ([Keyboard.PinEntry](#keyboard.pinentry) and [Keyboard.SecureKeyEntry](#keyboard.securekeyentry)), this
        /// property is null for the function keys 'one' to 'nine' and 'a' to 'f'. Otherwise, for each key pressed,
        /// the corresponding key value is stored in this property.
        /// 
        /// The following standard values are defined:
        /// 
        /// * ```zero``` - Numeric digit 0
        /// * ```one``` - Numeric digit 1
        /// * ```two``` - Numeric digit 2
        /// * ```three``` - Numeric digit 3
        /// * ```four``` - Numeric digit 4
        /// * ```five``` - Numeric digit 5
        /// * ```six``` - Numeric digit 6
        /// * ```seven``` - Numeric digit 7
        /// * ```eight``` - Numeric digit 8
        /// * ```nine``` - Numeric digit 9
        /// * ```[a-f]``` - Hex digit A to F for secure key entry
        /// * ```enter``` - Enter
        /// * ```cancel``` - Cancel
        /// * ```clear``` - Clear
        /// * ```backspace``` - Backspace
        /// * ```help``` - Help
        /// * ```decPoint``` - Decimal point
        /// * ```shift``` - Shift key used during hex entry
        /// * ```doubleZero``` - 00
        /// * ```tripleZero``` - 000
        /// * ```fdk[01-32]``` - 32 FDK keys
        /// 
        /// Additional non-standard values are also allowed:
        /// 
        /// * ```oem[a-zA-Z0-9]*``` - A non-standard value
        /// <example>five</example>
        /// </summary>
        [DataMember(Name = "digit")]
        [DataTypes(Pattern = @"^(zero|one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|doubleZero|tripleZero|fdk(0[1-9]|[12][0-9]|3[0-2])|oem[a-zA-Z0-9]*)$")]
        public string Digit { get; init; }

    }


    [DataContract]
    public sealed class LayoutClass
    {
        public LayoutClass(List<LayoutFrameClass> Data = null, List<LayoutFrameClass> Pin = null, List<LayoutFrameClass> Secure = null)
        {
            this.Data = Data;
            this.Pin = Pin;
            this.Secure = Secure;
        }

        /// <summary>
        /// The layout for the [Keyboard.DataEntry](#keyboard.dataentry) command.
        /// 
        /// There can be one or more frames included.
        /// 
        /// Refer to the [layout](#keyboard.generalinformation.layout) section for the different types of frames, and
        /// see the diagram for an example.
        /// </summary>
        [DataMember(Name = "data")]
        public List<LayoutFrameClass> Data { get; init; }

        /// <summary>
        /// The layout for the [Keyboard.PinEntry](#keyboard.pinentry) command.
        /// 
        /// There can be one or more frames included.
        /// 
        /// Refer to the [layout](#keyboard.generalinformation.layout) section for the different types of frames, and
        /// see the diagram for an example.
        /// </summary>
        [DataMember(Name = "pin")]
        public List<LayoutFrameClass> Pin { get; init; }

        /// <summary>
        /// The layout for the [Keyboard.SecureKeyEntry](#keyboard.securekeyentry) command.
        /// 
        /// There can be one or more frames included.
        /// 
        /// Refer to the [layout](#keyboard.generalinformation.layout) section for the different types of frames, and
        /// see the diagram for an example.
        /// </summary>
        [DataMember(Name = "secure")]
        public List<LayoutFrameClass> Secure { get; init; }

    }


}
