/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * TextTerminalSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.TextTerminal
{

    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(KeyboardEnum? Keyboard = null, KeyLockEnum? KeyLock = null, int? DisplaySizeX = null, int? DisplaySizeY = null, List<LedsClass> Leds = null)
        {
            this.Keyboard = Keyboard;
            this.KeyLock = KeyLock;
            this.DisplaySizeX = DisplaySizeX;
            this.DisplaySizeY = DisplaySizeY;
            this.Leds = Leds;
        }

        public enum KeyboardEnum
        {
            On,
            Off,
            Na
        }

        /// <summary>
        /// Specifies the state of the keyboard in the text terminal unit as one of the following flags:
        /// * ```on``` - The keyboard is activated.
        /// * ```off``` - The keyboard is not activated.
        /// * ```na``` - The keyboard is not available.
        /// </summary>
        [DataMember(Name = "keyboard")]
        public KeyboardEnum? Keyboard { get; init; }

        public enum KeyLockEnum
        {
            On,
            Off,
            Na
        }

        /// <summary>
        /// Specifies the state of the keyboard lock of the text terminal unit as one of the following flags:
        /// * ```on``` - The keyboard lock switch is activated.
        /// * ```off``` - The keyboard lock switch is not activated.
        /// * ```na``` - The keyboard lock switch is not available.
        /// </summary>
        [DataMember(Name = "keyLock")]
        public KeyLockEnum? KeyLock { get; init; }

        /// <summary>
        /// Specifies the horizontal size of the display of the text terminal unit.
        /// </summary>
        [DataMember(Name = "displaySizeX")]
        [DataTypes(Minimum = 0)]
        public int? DisplaySizeX { get; init; }

        /// <summary>
        /// Specifies the vertical size of the display of the text terminal unit.
        /// </summary>
        [DataMember(Name = "displaySizeY")]
        [DataTypes(Minimum = 0)]
        public int? DisplaySizeY { get; init; }

        [DataContract]
        public sealed class LedsClass
        {
            public LedsClass(bool? Na = null, bool? Off = null, bool? SlowFlash = null, bool? MediumFlash = null, bool? QuickFlash = null, bool? Continuous = null, bool? Red = null, bool? Green = null, bool? Yellow = null, bool? Blue = null, bool? Cyan = null, bool? Magenta = null, bool? White = null)
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

            /// <summary>
            /// The Status is not available.
            /// Type A
            /// </summary>
            [DataMember(Name = "na")]
            public bool? Na { get; init; }

            /// <summary>
            /// The LED is turned off.
            /// Type A
            /// </summary>
            [DataMember(Name = "off")]
            public bool? Off { get; init; }

            /// <summary>
            /// The LED is blinking.
            /// Type B
            /// </summary>
            [DataMember(Name = "slowFlash")]
            public bool? SlowFlash { get; init; }

            /// <summary>
            /// The LED is blinking medium frequency.
            /// Type B
            /// </summary>
            [DataMember(Name = "mediumFlash")]
            public bool? MediumFlash { get; init; }

            /// <summary>
            /// The LED is blinking quickly.
            /// Type B
            /// </summary>
            [DataMember(Name = "quickFlash")]
            public bool? QuickFlash { get; init; }

            /// <summary>
            /// The LED is turned on continuous(steady).
            /// Type B
            /// </summary>
            [DataMember(Name = "continuous")]
            public bool? Continuous { get; init; }

            /// <summary>
            /// The LED is red.
            /// Type C
            /// </summary>
            [DataMember(Name = "red")]
            public bool? Red { get; init; }

            /// <summary>
            /// The LED is green.
            /// Type C
            /// </summary>
            [DataMember(Name = "green")]
            public bool? Green { get; init; }

            /// <summary>
            /// The LED is yellow.
            /// Type C
            /// </summary>
            [DataMember(Name = "yellow")]
            public bool? Yellow { get; init; }

            /// <summary>
            /// The LED is blue.
            /// Type C
            /// </summary>
            [DataMember(Name = "blue")]
            public bool? Blue { get; init; }

            /// <summary>
            /// The LED is cyan.
            /// Type C
            /// </summary>
            [DataMember(Name = "cyan")]
            public bool? Cyan { get; init; }

            /// <summary>
            /// The LED is magenta.
            /// Type C
            /// </summary>
            [DataMember(Name = "magenta")]
            public bool? Magenta { get; init; }

            /// <summary>
            /// The LED is white.
            /// Type C
            /// </summary>
            [DataMember(Name = "white")]
            public bool? White { get; init; }

        }

        /// <summary>
        /// Specifies array that specifies the state of each LED.
        /// Specifies the state of the na, off or a combination of the following flags consisting of one type B, 
        /// and optionally one type C
        /// </summary>
        [DataMember(Name = "leds")]
        public List<LedsClass> Leds { get; init; }

    }


    [DataContract]
    public sealed class ResolutionClass
    {
        public ResolutionClass(int? SizeX = null, int? SizeY = null)
        {
            this.SizeX = SizeX;
            this.SizeY = SizeY;
        }

        /// <summary>
        /// TSpecifies the horizontal size of the display of the text terminal unit (the number of columns that can be displayed).
        /// </summary>
        [DataMember(Name = "sizeX")]
        [DataTypes(Minimum = 0)]
        public int? SizeX { get; init; }

        /// <summary>
        /// Specifies the vertical size of the display of the text terminal unit (the number of rows that can be displayed).
        /// </summary>
        [DataMember(Name = "sizeY")]
        [DataTypes(Minimum = 0)]
        public int? SizeY { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(TypeEnum? Type = null, List<ResolutionClass> Resolutions = null, bool? KeyLock = null, bool? DisplayLight = null, bool? Cursor = null, bool? Forms = null, List<LedsClass> Leds = null)
        {
            this.Type = Type;
            this.Resolutions = Resolutions;
            this.KeyLock = KeyLock;
            this.DisplayLight = DisplayLight;
            this.Cursor = Cursor;
            this.Forms = Forms;
            this.Leds = Leds;
        }

        public enum TypeEnum
        {
            Fixed,
            Removable
        }

        /// <summary>
        /// Specifies the type of the text terminal unit as one of the following flags:
        /// * ```fixed``` - The text terminal unit is a fixed device.
        /// * ```removable``` - The text terminal unit is a removable device.
        /// </summary>
        [DataMember(Name = "type")]
        public TypeEnum? Type { get; init; }

        /// <summary>
        /// Array specifies the resolutions supported by the physical display device. (For the definition of Resolution see 
        /// the command [TextTerminal.SetResolution](#textterminal.setresolution)). The resolution indicated in the first 
        /// position is the default resolution and the device will be placed in this resolution when the Service Provider 
        /// is initialized or reset through the [TextTerminal.Reset](#textterminal.reset) command.
        /// </summary>
        [DataMember(Name = "resolutions")]
        public List<ResolutionClass> Resolutions { get; init; }

        /// <summary>
        /// Specifies whether the text terminal unit has a key lock switch.
        /// </summary>
        [DataMember(Name = "keyLock")]
        public bool? KeyLock { get; init; }

        /// <summary>
        /// Specifies whether the text terminal unit has a display light that can be switched ON and OFF with the 
        /// [TextTerminal.DispLight](#textterminal.displight) command.
        /// </summary>
        [DataMember(Name = "displayLight")]
        public bool? DisplayLight { get; init; }

        /// <summary>
        /// Specifies whether the text terminal unit display supports a cursor.
        /// </summary>
        [DataMember(Name = "cursor")]
        public bool? Cursor { get; init; }

        /// <summary>
        /// Specifies whether the text terminal unit service supports forms oriented input and output.
        /// </summary>
        [DataMember(Name = "forms")]
        public bool? Forms { get; init; }

        [DataContract]
        public sealed class LedsClass
        {
            public LedsClass(bool? Off = null, bool? SlowFlash = null, bool? MediumFlash = null, bool? QuickFlash = null, bool? Continuous = null, bool? Red = null, bool? Green = null, bool? Yellow = null, bool? Blue = null, bool? Cyan = null, bool? Magenta = null, bool? White = null)
            {
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

            /// <summary>
            /// The LED can be off. 
            /// Type:(A)
            /// </summary>
            [DataMember(Name = "off")]
            public bool? Off { get; init; }

            /// <summary>
            /// The LED can be blinking. 
            /// Type:(B)
            /// </summary>
            [DataMember(Name = "slowFlash")]
            public bool? SlowFlash { get; init; }

            /// <summary>
            /// The LED can be blinking medium frequency. 
            /// Type:(B)
            /// </summary>
            [DataMember(Name = "mediumFlash")]
            public bool? MediumFlash { get; init; }

            /// <summary>
            /// The LED can be blinking quickly. 
            /// Type:(B)
            /// </summary>
            [DataMember(Name = "quickFlash")]
            public bool? QuickFlash { get; init; }

            /// <summary>
            /// The LED can be turned on continuous(steady). 
            /// Type:(B)
            /// </summary>
            [DataMember(Name = "continuous")]
            public bool? Continuous { get; init; }

            /// <summary>
            /// The LED can be red. 
            /// Type:(C)
            /// </summary>
            [DataMember(Name = "red")]
            public bool? Red { get; init; }

            /// <summary>
            /// The LED can be green. 
            /// Type:(C)
            /// </summary>
            [DataMember(Name = "green")]
            public bool? Green { get; init; }

            /// <summary>
            /// The LED can be yellow. 
            /// Type:(C)
            /// </summary>
            [DataMember(Name = "yellow")]
            public bool? Yellow { get; init; }

            /// <summary>
            /// The LED can be blue. 
            /// Type:(C)
            /// </summary>
            [DataMember(Name = "blue")]
            public bool? Blue { get; init; }

            /// <summary>
            /// The LED can be cyan. 
            /// Type:(C)
            /// </summary>
            [DataMember(Name = "cyan")]
            public bool? Cyan { get; init; }

            /// <summary>
            /// The LED can be magenta. 
            /// Type:(C)
            /// </summary>
            [DataMember(Name = "magenta")]
            public bool? Magenta { get; init; }

            /// <summary>
            /// The LED can be white. 
            /// Type:(C)
            /// </summary>
            [DataMember(Name = "white")]
            public bool? White { get; init; }

        }

        /// <summary>
        /// Specifies which LEDs are available.
        /// The elements of this array are specified as a combination of the following flags and indicate all 
        /// of the possible flash rates (type B) and colors (type C) that the LED is capable of handling. 
        /// If the LED only supports one color then no value of type C is returned.
        /// </summary>
        [DataMember(Name = "leds")]
        public List<LedsClass> Leds { get; init; }

    }


    [DataContract]
    public sealed class FieldDetailsClass
    {
        public FieldDetailsClass(TypeEnum? Type = null, ClassEnum? Class = null, AccessClass Access = null, OverflowEnum? Overflow = null, string Format = null, string LanguageId = null)
        {
            this.Type = Type;
            this.Class = Class;
            this.Access = Access;
            this.Overflow = Overflow;
            this.Format = Format;
            this.LanguageId = LanguageId;
        }

        public enum TypeEnum
        {
            Text,
            Invisible,
            Password
        }

        /// <summary>
        /// Specifies the type of field and can be one of the following:
        ///   * ```text``` - A text field.
        ///   * ```invisible``` - An invisible text field.
        ///   * ```password``` - A password field, input is echoed as '*'.  
        /// </summary>
        [DataMember(Name = "type")]
        public TypeEnum? Type { get; init; }

        public enum ClassEnum
        {
            Static,
            Optional,
            Required
        }

        /// <summary>
        /// Specifies the class of the field and can be one of the following:
        /// * ```static``` - The field data cannot be set by the application.
        /// * ```optional``` - The field data can be set by the application.
        /// * ```required``` - The field data must be set by the application.
        /// </summary>
        [DataMember(Name = "class")]
        public ClassEnum? Class { get; init; }

        [DataContract]
        public sealed class AccessClass
        {
            public AccessClass(string Read = null, string Write = null)
            {
                this.Read = Read;
                this.Write = Write;
            }

            /// <summary>
            /// The Field is used for input from the physical device.
            /// </summary>
            [DataMember(Name = "read")]
            public string Read { get; init; }

            /// <summary>
            /// The Field is used for output to the physical device.
            /// </summary>
            [DataMember(Name = "write")]
            public string Write { get; init; }

        }

        /// <summary>
        /// Specifies whether the field is to be used for input, output or both.
        /// </summary>
        [DataMember(Name = "access")]
        public AccessClass Access { get; init; }

        public enum OverflowEnum
        {
            Terminate,
            Truncate,
            Overwrite
        }

        /// <summary>
        /// Specifies how an overflow of field data should be handle and can be one of the following:
        /// * ```terminate``` - Return an error and terminate display of the form.
        /// * ```truncate``` - Truncate the field data to fit in the field.
        /// * ```overwrite``` - Print the field data beyond the extents of the field boundary.
        /// </summary>
        [DataMember(Name = "overflow")]
        public OverflowEnum? Overflow { get; init; }

        /// <summary>
        /// Format string as defined in the form for this field.
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; init; }

        /// <summary>
        /// Specifies the language identifier for the field.
        /// </summary>
        [DataMember(Name = "languageId")]
        public string LanguageId { get; init; }

    }


    public enum ModesEnum
    {
        Relative,
        Absolute
    }


}
